USE [MSPATest]
GO

/****** Object:  Table [dbo].[Resources]    Script Date: 22/06/2015 1:05:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Resources](
	[id] [int] NOT NULL IDENTITY (1,1),
	[page_id] [int] NOT NULL,
	[data] [varbinary](max) NULL,
	[original_filename] [nvarchar](max) NULL,
	[title_text] [nvarchar](max) NULL,
 CONSTRAINT [PK_Resources] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

