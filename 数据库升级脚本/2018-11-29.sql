

/****** Object:  Table [dbo].[SingleSignOn]    Script Date: 11/29/2018 09:52:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SingleSignOn](
	[SSO_ID] [int] IDENTITY(1,1) NOT NULL,
	[SSO_Name] [nvarchar](50) NULL,
	[SSO_IsUse] [bit] NULL,
	[SSO_APPID] [nvarchar](500) NOT NULL,
	[SSO_IsAdd] [bit] NOT NULL,
	[SSO_Domain] [nvarchar](500) NULL,
	[SSO_Direction] [nvarchar](50) NULL,
	[SSO_Phone] [nvarchar](50) NULL,
	[SSO_Email] [nvarchar](50) NULL,
	[SSO_Info] [nvarchar](500) NULL,
	[SSO_CrtTime] [datetime] NOT NULL,
	[SSO_Power] [nvarchar](50) NULL,
	[SSO_Config] [nvarchar](max) NULL,
 CONSTRAINT [PK_SingleSignOn] PRIMARY KEY CLUSTERED 
(
	[SSO_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[ManageMenu] ([MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (N'µ¥µãµÇÂ¼', N'item', 88, N'site/ssoapi.aspx', N'', 71, 654, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
go


