﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Reader_UI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Reader_UI.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap candyCorn {
            get {
                object obj = ResourceManager.GetObject("candyCorn", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap cueBall {
            get {
                object obj = ResourceManager.GetObject("cueBall", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE PageMeta (page_id INTEGER, x2 BOOLEAN, title TEXT, promptType TEXT, headerAltText TEXT, PRIMARY KEY (page_id, x2));
        ///CREATE TABLE Dialog (id INTEGER PRIMARY KEY AUTOINCREMENT, page_id INTEGER REFERENCES PageMeta (page_id), x2 BOOLEAN, isNarrative BOOLEAN, isImg BOOLEAN, text TEXT, colour TEXT, precedingLineBreaks INTEGER);
        ///CREATE TABLE Links (id INTEGER PRIMARY KEY AUTOINCREMENT, page_id INTEGER, x2 BOOLEAN, linked_page_id INTEGER, link_text TEXT);
        ///CREATE TABLE PagesArchived (page_id INTEGER, [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQLiteDBCreationScript {
            get {
                return ResourceManager.GetString("SQLiteDBCreationScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SET ANSI_NULLS ON;
        ///SET QUOTED_IDENTIFIER ON;
        ///CREATE TABLE [dbo].[Dialog](
        ///	[id] [int] IDENTITY(1,1) NOT NULL,
        ///	[page_id] [int] NOT NULL,
        ///	[x2] [bit] NOT NULL,
        ///	[isNarrative] [bit] NOT NULL,
        ///	[isImg] [bit] NOT NULL,
        ///	[text] [nvarchar](max) NULL,
        ///	[colour] [nchar](7) NULL,
        ///	[precedingLineBreaks] [int] NOT NULL
        /// CONSTRAINT [PK_Dialog] PRIMARY KEY CLUSTERED 
        ///(
        ///	[id] ASC
        ///)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIM [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SQLSDBCreationScript {
            get {
                return ResourceManager.GetString("SQLSDBCreationScript", resourceCulture);
            }
        }
    }
}
