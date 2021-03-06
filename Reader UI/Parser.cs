﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;

namespace Reader_UI
{
    public class Parser : IDisposable
    {
        //yes I know about the page text files but they don't quite measure up to what i need
        //it's easy enough to get the stuff i want directly from the page (e.g. see 7980)
        //the main reason i won't use them is because of the lack of wording for the next pages.
        //in using this i'd have to parse 2 files for one page instead of just the one html
        //its worked so far no reason to change it now

        public const string githubRepo = "https://raw.githubusercontent.com/cybnetsurfe3011/MSPA-Reader/master/CurrentVersion.txt";
        public class Resource
        {
            readonly public byte[] data;
            readonly public string originalFileName, titleText;
            public bool isInPesterLog = false;
            public Resource(byte[] idata, string ioFN, string tt = null)
            {
                data = idata;
                originalFileName = ioFN;
                titleText = tt;
            }
        }
        void Dispose(bool mgd)
        {
            client.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Parser()
        {
            Dispose(false);
        }
        public class Text
        {

            public class ScriptLine
            {
                public class SpecialSubText
                {
                    public readonly int begin, length;
                    public readonly bool isImg;
                    public readonly bool isLink;
                    public readonly bool underlined;
                    public readonly string colour;
                    public SpecialSubText(int beg, int len, bool under, string col)
                    {
                        isImg = false;
                        isLink = false;
                        begin = beg;
                        length = len;
                        underlined = under;
                        colour = col;
                    }
                    public SpecialSubText(int beg, int len, string imageName, bool imgnotlink)
                    {
                        isImg = imgnotlink;
                        isLink = !imgnotlink;
                        begin = beg;
                        length = len;
                        underlined = false;
                        colour = imageName;
                    }
                    public SpecialSubText(int beg, int len, bool under, string col, bool i, bool l)
                    {
                        isLink = l;
                        isImg = i;
                        begin = beg;
                        length = len;
                        underlined = under;
                        colour = col;
                    }
                }
                public readonly bool isImg;
                public string hexColour;
                public readonly string text;
                public SpecialSubText[] subTexts = null;
                public readonly int precedingLineBreaks;
                public ScriptLine(string hx, string tx, int prb)
                {
                    hexColour = hx;
                    precedingLineBreaks = prb;
                    text = tx;
                    isImg = false;
                }
                public ScriptLine(string resName, int prb)
                {
                    isImg = true;
                    hexColour = null;
                    text = resName;
                    precedingLineBreaks = prb;
                }
            }

            public string title = null;
            public ScriptLine narr = null;
            public string promptType = null;
            public ScriptLine[] lines = null;
            public string altText = null;
        }
        public void LoadIcons()
        {
            resources.Clear();
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/candycorn.gif"), "candycorn.gif"));
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/candycorn_scratch.png"), "candycorn_scratch.png"));
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/a6a6_tooth2.gif"), "a6a6_tooth2.gif"));
        }
        public class Link
        {
            readonly public string originalText;
            readonly public int pageNumber;
            public Link(string oT, int pN)
            {
                originalText = oT;
                pageNumber = pN;
            }
        }
        public int CheckIfUpdateIsAvailable()
        {
            try
            {
                byte[] raw = DownloadFile("https://raw.githubusercontent.com/cybnetsurfe3011/MSPA-Reader/master/CurrentVersion.txt");

                string source = System.Text.Encoding.UTF8.GetString(raw);
                if (Convert.ToInt32(source) > (int)Writer.Versions.Program)
                    return Convert.ToInt32(source);
                
            }
            catch { }
            return 0;
        }
        //http://stackoverflow.com/questions/1585985/how-to-use-the-webclient-downloaddataasync-method-in-this-context
        class WebDownload
        {
            const int MAX_CLIENTS = 100;
            List<WebClient> downloaders = new List<WebClient>();
            List<UInt64> toServe = new List<UInt64>();
            UInt64 nextToServe = 0;
            object _slock = new object(), _dlock = new object();
            public byte[] DownloadData(string address) 
            {
                WebClient use;
                UInt64 serviceNumber;
                lock (_slock)
                {
                    toServe.Add(nextToServe);
                    serviceNumber = nextToServe;
                    nextToServe++;  //overflow shoudn't matter
                }
                while (true)
                {
                    lock (_slock)
                    {
                        if (serviceNumber == toServe.First())
                        {
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }
                    lock (_dlock)
                    {
                        if (downloaders.Count > 0)
                        {
                            use = downloaders.First();
                            downloaders.RemoveAt(0);
                        }
                        else
                            use = new WebClient();
                    }
                lock (_slock)
                {
                    toServe.RemoveAt(0);
                }
                var res = use.DownloadData(address);

                lock (_dlock)
                {
                    downloaders.Add(use);
                }
                return res;
            }
            void Dispose()
            {

            }
        }
        static WebDownload downloader = new WebDownload();
        const string prepend2 = "http://www.mspaintadventures.com/?s=";
        const string prepend3 = "&p=";
        const string gifRegex = @"http:\/\/(?!" + 
            @".*v2_blankstrip"  //stuff to ignore
            + @"|.*v2_blanksquare2"
            + @"|.*v2_blanksquare3"
            + @"|.*spacer"
            + @"|.*bluetile"
            + @")(.*?)\.(?i)(gif|png|jpg)";
        const string scratchHeaderImageRegex = "src=\\\"(.*?\\.(?i)(gif|png))\\\"";
        const string scratchHeaderImageFilenameRegex = @".*\/(.*)";
        const string scratchTitleRegex = "title=\\\"(.*?)\\\"";
        const string swfRegex = @"http:\/\/.*?\.swf";
        const string linkNumberRegex = @"[0-9]{6}";
        const string logRegex = @"Dialoglog|Spritelog|Pesterlog|Recap log";
        const string npRegex = @"border: 3px solid #c6c6c6; padding: 1px; background: white;";
        const string hexColourRegex = @"#[0-9A-Fa-f]{6}";
        const string underlineRegex = @"underline";
        const string pesterLogRegex = @"-- .*? --|(.*?(\[[G|C|A|T]{2}\]|\[EB\]).*?(\[[G|C|A|T]{2}\]|\[EB\]).*)";
        const string chumhandleRegex = @".*?\[[G|C|A|T]{2}\]|\[EB\]";
        const string gifFileRegex = @".+\.(?i)[gif|png]";
        

        public bool x2Flag;

        HttpClient client = new HttpClient();

        HtmlNode contentTable,secondContentTable;

        List<Resource> resources = new List<Resource>();
        List<Link> links = new List<Link>();
        Text texts;
        List<HtmlNode> linkListForTextParse = new List<HtmlNode>();
        int currentPage;

        public static bool IsGif(string file)
        {
            return Regex.Match(file, gifFileRegex).Success;
        }
        public int GetLatestPage()
        {
            //if this fails we need to check the database
            try
            {
                var response = DownloadFile("http://www.mspaintadventures.com/?viewlog=6",true);
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                var html = new HtmlDocument();
                html.LoadHtml(source);

                //look for view oldest to newest key phrase
                //magic from http://stackoverflow.com/questions/8948895/using-xpath-and-htmlagilitypack-to-find-all-elements-with-innertext-containing-a
                var labelHref = html.DocumentNode.SelectNodes("//*[text()[contains(., 'View oldest to newest')]]").First();
                var firstEntry = labelHref.ParentNode.ParentNode.SelectNodes("a").First();
                var linkText = firstEntry.Attributes["href"].Value;

                string pageNumberAsString = Regex.Match(linkText, linkNumberRegex).Value;

                return Convert.ToInt32(pageNumberAsString);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Error retrieving lastest MSPA page. Range locked to currently archived pages");
                return 0;
            }
        }
        public void LoadTricksterResources()
        {
            resources.Clear();
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/trickster_sitegraphics/Z2.gif"), "Z2.gif"));
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/trickster_sitegraphics/menu.swf"), "menu.swf"));
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/trickster_sitegraphics/bluetile.gif"), "bluetile.gif"));
        }
        public Text GetText()
        {
            return texts;
        }
        void CheckLineForSpecialSubText(HtmlNode currentLine, Text.ScriptLine scriptLine)
        {
            var lineSpecialSubtext = currentLine.SelectNodes("span");
            var lineImages = currentLine.SelectNodes("img");
            var lineLinks = currentLine.SelectNodes("a");
            if (lineImages == null && lineSpecialSubtext == null && lineLinks == null)
                return;

            List<Text.ScriptLine.SpecialSubText> sTs = new List<Text.ScriptLine.SpecialSubText>();
            if (lineSpecialSubtext != null)
            {
                //special subtext alert
                for (int j = 0; j < lineSpecialSubtext.Count(); ++j)
                {
                    var currentSpecialSubtext = lineSpecialSubtext.ElementAt(j);
                    bool underlined = Regex.Match(currentSpecialSubtext.OuterHtml, underlineRegex).Success;
                    var colourReg = Regex.Match(currentSpecialSubtext.OuterHtml, hexColourRegex);
                    string colour = colourReg.Success ? colourReg.Value : scriptLine.hexColour;
                    int begin = currentLine.InnerText.Trim().IndexOf(currentSpecialSubtext.InnerText);
                    int length = currentSpecialSubtext.InnerText.Trim().Length;
                    sTs.Add(new Text.ScriptLine.SpecialSubText(begin, length, underlined, colour));
                }
            }

            if (lineImages != null)
            {
                for (int j = 0; j < lineImages.Count(); ++j)
                {
                    var currentSpecialSubtext = lineImages.ElementAt(j);
                    var reg = Regex.Match(currentSpecialSubtext.OuterHtml, gifRegex);
                    string img = System.IO.Path.GetFileName(new Uri(reg.Value).LocalPath);
                    int begin = currentLine.InnerText.Trim().IndexOf(currentSpecialSubtext.InnerText);
                    int length = currentSpecialSubtext.InnerText.Trim().Length;
                    resources.Find(x => x.originalFileName == img).isInPesterLog = true;
                    sTs.Add(new Text.ScriptLine.SpecialSubText(begin, length, img, true));
                }
            }
            if (lineLinks != null)
                for (int i = 0; i < lineLinks.Count(); ++i)
                {
                    var currentSpecialSubtext = lineLinks.ElementAt(i);
                    string link = currentSpecialSubtext.Attributes["href"].Value;
                    int begin = currentLine.InnerText.Trim().IndexOf(currentSpecialSubtext.InnerText);
                    int length = currentSpecialSubtext.InnerText.Trim().Length;
                    sTs.Add(new Text.ScriptLine.SpecialSubText(begin, length, MakeAbsoluteUrl(link), false));
                }
            scriptLine.subTexts = sTs.ToArray();

        }
        static string MakeAbsoluteUrl(string url)
        {
            Uri result;
            var res = Uri.TryCreate(url, UriKind.Absolute, out result);
            if (res)
                return url;
            return "http://mspaintadventures.com/" + url;
        }
        void ParseText()
        {
            //most difficult part here
            //all text in homestuck is pure html formatting
            //so the styles are all over the place
            texts = new Text();
            {//title
                //easy enough, its the very first p in the content table
                //just clean it up a bit
                texts.title =currentPage == 9828 ? "[S][A6A6I5] ====>" :  contentTable.Descendants("p").First().InnerText.Trim();
            }
            
            /*
             * There are cases where there can be narritive and script on one page so we need something to independantly check if the narrative exists
             * ...
             * which is damn near impossible
             * 
             * 
             * try handling the script first then remove it from it's parent node to clean the doc somewhat
             * 
             * NEVER MIND ALL THAT THE ONE EDGE CASE I THOUGHT OF WAS PART OF THE GIF
             */


               //script
                //check if page HAS a dialoglog , find it and get the lines within
                var reg = Regex.Match(contentTable.InnerText, logRegex);
                var reg2 = Regex.Match(contentTable.InnerHtml, npRegex);
                if (reg.Success || reg2.Success)
                {

                    texts.promptType = reg2.Success ? "CalibornLog" : reg.Value;
                    HtmlNode convParent = null;
                    if (!reg2.Success)
                        convParent = contentTable.SelectSingleNode(".//*[text()[contains(., '" + reg.Value + "')]]").ParentNode.ParentNode;
                    else
                        convParent = contentTable.Descendants("div").First();
                    var logBox = convParent.SelectSingleNode(".//p");
                    var conversationLines = logBox.SelectNodes("span|img|br");   //this will grab lines 

                    if (conversationLines != null)
                    {
                        List<Text.ScriptLine> line = new List<Text.ScriptLine>();
                        List<Text.ScriptLine> logs = new List<Text.ScriptLine>();
                        
                        //conversation lines go in order, no stopping them.
                        //What we can do is insert them where they belong.
                        // by doing a once over once we've done everything
                        //inserting them appropriately

                        var logMessages = Regex.Matches(logBox.InnerText.Trim(), pesterLogRegex);
                        var logMessagesHtml = Regex.Matches(logBox.InnerHtml.Trim(), pesterLogRegex);
                        //the hard part is finding out where these go
                        if(logMessages != null)
                            for (int i = 0; i < logMessages.Count; i++ )
                            {
                                //TODO: figure out where these actually go
                                var tmp = new Text.ScriptLine("#000000", logMessages[i].Value,1);       //Assume 1 line break before a log msg

                                ///OH WE CAN DO THIS TO GET THE NODES!!
                                HtmlDocument tempDoc = new HtmlDocument();
                                tempDoc.LoadHtml(logMessagesHtml[i].Value);
                                CheckLineForSpecialSubText(tempDoc.DocumentNode, tmp);

                                logs.Add(tmp);
                            }

                        
                        int j = 0;
                        for (; j < conversationLines.Count() && conversationLines.ElementAt(j).Name == "br"; j++) { }
                        int precedingLineBreaks = j;
                        //now for each line we need the colour and the text
                        while (j < conversationLines.Count())
                        {
                            var currentLine = conversationLines.ElementAt(j);
                            if (currentLine.Name != "br")
                            {
                                Text.ScriptLine scriptLine;
                                if (currentLine.Name == "img")
                                {
                                    //just add the image
                                    var pathReg = Regex.Match(currentLine.OuterHtml, gifRegex);
                                    var gifReg = Regex.Match(pathReg.Value, scratchHeaderImageFilenameRegex);
                                    scriptLine = new Text.ScriptLine(gifReg.Groups[1].Value, precedingLineBreaks);

                                    //find the resource that matches this image and mark it as pesterlogged
                                    resources.Find(x => x.originalFileName == scriptLine.text).isInPesterLog = true;

                                }
                                else
                                {
                                    //there is no way
                                    if (Regex.Match(currentLine.InnerText, chumhandleRegex).Success)
                                    {
                                        ++j;
                                        continue;
                                    }

                                    var hexReg = Regex.Match(currentLine.OuterHtml, hexColourRegex);

                                    scriptLine = new Text.ScriptLine(hexReg.Success ? hexReg.Value : "#000000", currentLine.InnerText, precedingLineBreaks);

                                    CheckLineForSpecialSubText(currentLine, scriptLine);
                                }

                                line.Add(scriptLine);
                            }
                            //increment i to find the breaks;
                            int jBegin = j + 1;
                            do
                            {
                                j++;
                            } while (j < conversationLines.Count() && conversationLines.ElementAt(j).Name == "br");

                            precedingLineBreaks = j - jBegin;

                        }

                        //now look through the lines adding whats expected
                        int linePositionCount = 0;
                        int logPositionCount = 0;

                        for (int i = 0; i < logBox.ChildNodes.Count && logPositionCount < logs.Count; ++i)
                        {
                            var currentNode = logBox.ChildNodes.ElementAt(i);
                            if (line.Count == linePositionCount)
                            {
                                line.Insert(linePositionCount, logs[logPositionCount]);
                                logPositionCount++;
                                linePositionCount++;
                            }
                            else if (currentNode.Name == "span")
                            {
                                if (currentNode.InnerText == line[linePositionCount].text)
                                {
                                    linePositionCount++;
                                }
                            }
                            else if (currentNode.Name == "img")
                            {
                                linePositionCount++;
                            }
                            else if (logs[logPositionCount].text.Contains(currentNode.InnerText.Trim()) && currentNode.InnerText.Trim() != "")
                            {
                                //at this point we need to keep incrementing i until we stop matching this one
                                //we can expect EXACTLY this many matches
                                i += 1 + logs[logPositionCount].subTexts.Count() * 2;
                                line.Insert(linePositionCount, logs[logPositionCount]);
                                logPositionCount++;
                                linePositionCount++;
                            }
                        }

                        texts.lines = line.ToArray();
                    }
                    else
                    {
                        //Assume simple text in a box
                        texts.lines = new Text.ScriptLine[1];
                        var hexReg = Regex.Match(logBox.OuterHtml, hexColourRegex);
                        texts.lines[0] = new Text.ScriptLine(hexReg.Success ? hexReg.Value : "#000000", logBox.InnerText.Trim(),0);
                        CheckLineForSpecialSubText(logBox, texts.lines[0]);

                    }
                }
                else
                {
                    //check for narrative
                    //narrative
                    //I seriously don't know if this is reliable but narrative seems to come on the second p if it exists

                    //TODO: Support different fonts
                    try
                    {
                        var decs = contentTable.Descendants("p");
                        var narrative = decs.ElementAt(Enum.IsDefined(typeof(Writer.FullScreenFlashes), currentPage) ? 0 : 1);
                        if (narrative != null)
                        {
                            var hexReg = Regex.Match(narrative.OuterHtml, hexColourRegex);
                            Text.ScriptLine narr = new Text.ScriptLine(hexReg.Success ? hexReg.Value : "#000000", narrative.InnerText.Trim(),0);
                            CheckLineForSpecialSubText(narrative, narr);
                            texts.narr = narr;
                        }
                    }
                    catch
                    {
                        texts.narr = new Text.ScriptLine("#000000","",0);
                    }
                    
                }
            

        }
        void ParseResources(bool clear)
        {
            if (clear)
                resources.Clear();
            //we are mainly looking for .gifs and .swfs, there are some things we should ignore, such as /images/v2_blankstrip.gif
            var matches = Regex.Matches(contentTable.InnerHtml, gifRegex);


            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].Value != "http://www.mspaintadventures.com/sweetbroandhellajeff/?cid=035.jpg")
                    resources.Add(new Resource(DownloadFile(matches[i].Value), System.IO.Path.GetFileName(new Uri(matches[i].Value).LocalPath)));
            }

            matches = Regex.Matches(contentTable.InnerHtml, swfRegex);
            List<string> matchNames = new List<string>();
            for (int i = 0; i < matches.Count; i++)
            {
                matchNames.Add(matches[i].Captures[0].Value);//filter out any double grabs
            }
            matchNames = matchNames.Distinct().ToList();

            //seriously couldn't think of a better place to put this
            //not defined in pages of importance because that breaks the flow of things
            if (currentPage == 7623)
                foreach (var match in matchNames)
                    if (match == "http://www.mspaintadventures.com/storyfiles/hs2/scraps/PEACHY.gif" || match == "http://cdn.mspaintadventures.com/storyfiles/hs2/scraps/PEACHY.gif")
                        match.Replace("PEACHY.gif", "CAUCASIAN.gif");
                    else if (match == "http://www.mspaintadventures.com/storyfiles/hs2/scraps/fruitone.gif" || match == "http://cdn.mspaintadventures.com/storyfiles/hs2/scraps/fruitone.gif")
                        match.Replace("/scraps/fruitone.gif", "05720_2.gif");

            for (int i = 0; i < matchNames.Count; i++)
                resources.Add(new Resource(DownloadFile(matchNames[i]), System.IO.Path.GetFileName(new Uri(matchNames[i]).LocalPath)));

            resources = resources.Distinct().ToList();
        }
        public Resource[] GetResources()
        {
            
            return resources.ToArray();
        }
        public void Reparse()
        {
            if (!x2Flag)
                throw new Exception();
            contentTable = secondContentTable;
            ParseResources(true);
            ParseLinks(false);
            ParseText();
        }
        public static bool IsTrickster(int pageno)
        {
            return (pageno >= 7614 && pageno <= 7677);
        }
        void ParseLinks(bool isRQ)
        {
            links.Clear();
            linkListForTextParse.Clear();
            foreach (HtmlNode link in contentTable.Descendants().Where(z => z.Attributes.Contains("href")))
            {
                string actualLink = link.Attributes["href"].Value;
                //we want to ignore the everypage navigation links
                if (link.InnerText == "Go Back"
                    || link.InnerText == "Start Over"
                    || link.InnerText == "Save Game"
                    || link.InnerText == "(?)"
                    || link.InnerText == "Auto-Save!"
                    || link.InnerText == "Delete Game Data"
                    || link.InnerText == "Load Game")
                    continue;
                var res = Regex.Match(actualLink, linkNumberRegex);
                if (res.Success)
                {
                    links.Add(new Link(link.InnerText.Trim(), Convert.ToInt32(res.Value) + (isRQ ? 136 : 0)));
                    linkListForTextParse.Add(link);
                }
            }
            if (links.Count > 20)
            {    //stupidity
                var last = links.Last();
                var ll = linkListForTextParse.Last();
                links.Clear();
                linkListForTextParse.Clear();
                links.Add(last);
                linkListForTextParse.Add(ll);
            }
        }
        public static int GetPageNumberFromURL(string url)
        {
            var reg = Regex.Match(url, linkNumberRegex);
            if (!reg.Success)
            {
                Debugger.Break();
                return 0;
            }
            return Convert.ToInt32(reg.Value);
        }
        public static bool IsHomosuck(int pageno)
        {
            //ty based wiki http://mspaintadventures.wikia.com/wiki/Homestuck:_Act_6_Act_6
            return ((pageno >= 8143 && pageno <= 8177)
                || (pageno >= 8375 && pageno <= 8430)
                || (pageno >= 8753 && pageno <= 8800)   //8801 is GAMEOVER
                || (pageno >= 8821 && pageno <= 8843)
                || (pageno >= 9309 && pageno <= 9348));
        }
        public Link[] GetLinks()
        {
            return links.ToArray();
        }
        public static byte[] DownloadFile(string file, bool explic = false)
        {
            if (file == "http://andrewhussie.com/whistlessite/preview.php?page=000.gif")
                return new byte[0];
            if (explic)
                return downloader.DownloadData(file);
            try
            {
                return downloader.DownloadData(file.Replace("www.mspaintadventures.com", "cdn.mspaintadventures.com"));
            }
            catch
            {
                //try the www if the cdn is jank
                return downloader.DownloadData(file.Replace("cdn.mspaintadventures.com", "www.mspaintadventures.com"));
            }
        }
        void ScratchPreParse(HtmlDocument html)
        {
            //grab the header from the top of the page
            resources.Clear();
            var node = html.DocumentNode.Descendants("img").First();
            string innerHtml = node.OuterHtml;
            var match = Regex.Match(innerHtml, scratchHeaderImageRegex);

            string actualFilePath = "http://cdn.mspaintadventures.com/" + match.Groups[1].Value;
            byte[] data = DownloadFile(actualFilePath);
            string oFN = Regex.Match(actualFilePath, scratchHeaderImageFilenameRegex).Groups[1].Value;
            string title = Regex.Match(innerHtml, scratchTitleRegex).Groups[1].Value;
            resources.Add(new Resource(data, oFN, title));


            resources = resources.Distinct().ToList();  //filter out any double grabs
        }
        void ScratchPostParse(HtmlDocument html, int pageno)
        {
            //maunally add the special LE text
            if (pageno >= 5976 && pageno <= 5981)
            {
                const string LESecretTextfilepref = "http://cdn.mspaintadventures.com/storyfiles/hs2/scraps/";
                string file = "LEtext" + (pageno - 5975) + ".gif";
                resources.Add(new Resource(DownloadFile(LESecretTextfilepref + file), file));
            }

            //grab the alt text
            var node = html.DocumentNode.Descendants("img").First();

            if (node != null && node.Attributes["title"] != null)
                texts.altText = node.Attributes["title"].Value;
        }
        public static bool IsScratch(int page)
        {
            return page >= 5664 && page <= 5981;
        }
        public static bool Is2x(int page)
        {
            return page >= 7688 && page <= 7825;
        }
        bool IsSBAHJ(int pageno)
        {
            return pageno == 5982;
        }
        string GetStoryFromPage(int pg)
        {
            if (pg <= (int)Writer.StoryBoundaries.JAILBREAK_LAST_PAGE)
                return "" + 1;
            if (pg <= (int)Writer.StoryBoundaries.EOBQ)
                return "" + 2; 
            if (pg <= (int)Writer.StoryBoundaries.EOPS)
                return "" + 4;
            if (pg <= (int)Writer.StoryBoundaries.EOHSB)
                return "" + 5;
            return "" + 6;
        }
        public bool LoadPage(int pageno)
        {
            //bardquest is weird as it has this void between pages 136 and 170 so we'll just pretend 136 is 170
            currentPage = pageno;
            if (pageno == (int)Writer.StoryBoundaries.BQ)
                pageno = 136;
            
            //also since ryanquest's page range clashes with jailbreak we'll put it in that void

            bool isRQ = false;
            if (pageno >= (int)Writer.StoryBoundaries.RQ && pageno <= (int)Writer.StoryBoundaries.EORQ)
            {
                pageno -= 136;
                isRQ = true;
            }


            try
            {
                x2Flag = false;
                var uristring = prepend2 + (isRQ ? "ryanquest" : GetStoryFromPage(currentPage)) + prepend3 + pageno.ToString("D6");
                var response = DownloadFile(uristring,true);
                String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);
                var html = new HtmlDocument();
                html.LoadHtml(source);
                
                if (IsScratch(pageno))
                {
                    ScratchPreParse(html);
                    contentTable = html.DocumentNode.Descendants("body").First().Descendants("table").First().Descendants("table").ElementAt(1).Descendants("table").First();
                    ParseResources(false);
                    ParseLinks(false);
                    ParseText();
                    ScratchPostParse(html,pageno);
                    return true;
                }
                else if (IsSBAHJ(pageno))
                {
                    contentTable = html.DocumentNode.Descendants("table").First().Descendants("table").ElementAt(1).Descendants("table").First();
                }
                else if (Is2x(pageno))
                {
                    x2Flag = true;
                    //i think he looks like a bitch

                    //same place in the html as regulars thankfully but that's about as easy as its's going to be from here
                    contentTable = html.DocumentNode.SelectSingleNode("//comment()[contains(., 'COMIC ONE')]").ParentNode.SelectSingleNode("table");
                    secondContentTable = html.DocumentNode.SelectSingleNode("//comment()[contains(., 'COMIC TWO')]").ParentNode.SelectSingleNode("table");

                    //essentially it's two pages of comics right next to each other. Simple enough for the parser. Fucking nightmare for the reader

                }
                else if (IsOpenBound(pageno))
                {
                    ParseOpenbound(pageno,html);
                    return true;
                }
                else
                {
                    //regular, homosuck, or trickster
                    contentTable = html.DocumentNode.Descendants("table").First().SelectNodes("tr").ElementAt(1).SelectNodes("td").First().SelectNodes("table").First();
                }
                ParseResources(true);
                ParseLinks(isRQ);
                ParseText();
            }
            catch
            {
                return false;
            }
            return true;
         }

        public void GetX2Header()
        {
            resources.Clear();
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/images/act6act5act1x2combo.gif"), "act6act5act1x2combo.gif"));
        }
        public void GetTerezi()
        {
            resources.Clear();
            resources.Add(new Resource(DownloadFile("http://cdn.mspaintadventures.com/storyfiles/hs2/scraps/pwimg.gif"), "act6act5act1x2combo.gif"));
        }
        #region Openbound
        public static bool IsOpenBound(int pageno)
        {
            return pageno == 7163
                || pageno == 7208
                || pageno == 7298;
        }

        void ParseOpenbound(int pg,HtmlDocument html)
        {
            contentTable = html.DocumentNode.Descendants("table").First().SelectNodes("tr").ElementAt(1).SelectNodes("td").First().SelectNodes("table").First();
            ParseLinks(false);
            ParseText();    //thats the easy part

            resources.Clear();
            //okay, openbound resources will have to behave differently from everyone else
            //first things first, we need to identity the initialization xml and the sburb javascript

            //we know that the xml will always be in the body
            string jsCall = html.DocumentNode.Descendants("body").First().Attributes["onload"].Value;
            var scripts = html.DocumentNode.Descendants("script");

            const string cdn = "http://cdn.mspaintadventures.com/";
            const string scriptRegex = ".*?Sburb\\.min\\.js";
            foreach (var s in scripts)
            {
                if (s.Attributes["src"] == null)
                    continue;
                var reg = Regex.Match(s.Attributes["src"].Value, scriptRegex);
                if (reg.Success)
                {
                    byte[] jsdata = DownloadFile(cdn + reg.Value);
                    //theres a bug in the javascript that only IE is dumb enough to fall for, a method that CALLS a member.
                    jsdata = System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.UTF8.GetString(jsdata).Replace("this.movie.TotalFrames()", "this.movie.TotalFrames"));
                    resources.Add(new Resource(jsdata, reg.Value));
                    break;
                }
            }

            const string xmlFromJSCallRegex = @",'(.*?\.xml)'";

            var xmlreg = Regex.Match(jsCall, xmlFromJSCallRegex);

            var nextFile = xmlreg.Groups[1].Value;


            byte[] data = DownloadFile(cdn + nextFile);

            
            XmlDocument xml = new XmlDocument();
            using(var ms = new System.IO.MemoryStream(data)){
                xml.Load(ms);
            }

            resources.Add(new Resource(data, nextFile));
            texts.altText = nextFile;

            Stack<string> addressesToParse = new Stack<string>();
            List<string> parsedPages = new List<string>();

            string levelPath =  xml.DocumentElement.Attributes["levelPath"].Value + '/';
            string resourcePath = xml.DocumentElement.Attributes["resourcePath"].Value + '/';

            while (addressesToParse.Count != 0 || xml != null)
            {
                if (xml == null)
                {
                    while (parsedPages.IndexOf(nextFile) != -1 && addressesToParse.Count > 0)
                    {
                        nextFile = addressesToParse.Pop();
                    }
                    if (parsedPages.IndexOf(nextFile) != -1)
                        break;
                    try
                    {
                        data = DownloadFile(cdn + nextFile);
                    }
                    catch
                    {
                        nextFile = addressesToParse.Pop();
                        continue;//if the get fails forget it
                    }
                    xml = new XmlDocument();
                    using (var ms = new System.IO.MemoryStream(data))
                    {
                        xml.Load(ms);
                    }
                    resources.Add(new Resource(data, nextFile));
                }

                //first grab dependancy xmls
                var dependancies = xml.SelectNodes(".//dependency");
                foreach (XmlNode dep in dependancies)
                {
                    addressesToParse.Push(levelPath + dep.InnerText);
                }

                //those don't point to next levels though
                //check the actions for transitions
                var actions = xml.SelectNodes(".//action");
                foreach (XmlNode act in actions)
                {
                    const string xmlSearcher = @".*\.xml";

                    var split = act.InnerText.Split(',');
                    for (int i = 0; i < split.Length; i++)
                    {
                        var reg = Regex.Match(split[i], xmlSearcher);
                        var res = reg.Value.Trim();
                        if (reg.Success && !res.Contains(' '))
                        {
                            addressesToParse.Push(levelPath + res);
                            break;
                        }
                    }
                }


                //then assets
                var assets = xml.SelectNodes(".//asset");
                foreach (XmlNode a in assets)
                {
                    if (a.Attributes["type"].Value == "graphic"
                        || a.Attributes["type"].Value == "movie")
                    {
                        string path = resourcePath + a.InnerText;
                        resources.Add(new Resource(DownloadFile(cdn + path), path));
                    }
                    else if (a.Attributes["type"].Value == "audio")
                    {
                        string path1 = resourcePath + a.InnerText.Substring(0, a.InnerText.IndexOf(';'));
                        string path2 = resourcePath + a.InnerText.Substring(a.InnerText.IndexOf(';') + 1);
                        resources.Add(new Resource(DownloadFile(cdn + path1), path1));
                        resources.Add(new Resource(DownloadFile(cdn + path2), path2));
                    }
                    else if (a.Attributes["type"].Value == "font")
                    {
                        const string fontReg = @"url:(.*\.(?i)(woff|ttf))";

                        var subFonts = a.InnerText.Split(',');
                        foreach (var f in subFonts)
                        {
                            var reg = Regex.Match(f.Trim(), fontReg);
                            if (reg.Success)
                                resources.Add(new Resource(DownloadFile(cdn + resourcePath + reg.Groups[1].Value.Trim()), resourcePath + reg.Groups[1].Value.Trim()));
                        }

                    }
                    else if (a.Attributes["type"].Value != "path")
                    {
                        Debugger.Break();
                    }
                }

                //discard the doc
                parsedPages.Add(nextFile);
                xml = null;
            }

        }
        #endregion
    }
}
