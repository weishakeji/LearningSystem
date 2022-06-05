--学员的收藏

CREATE TABLE [dbo].[Student_Collect](
 [Stc_ID] [int] IDENTITY(1,1) NOT NULL,
 [St_ID] [int] NOT NULL,
 [Qus_ID] [int] NOT NULL,
 [Qus_Type] [int] NOT NULL,
 [Qus_Diff] [int] NOT NULL,
 [Sbj_ID] [int] NOT NULL,
 [Stc_Level] [int] NOT NULL,
 [Stc_Strange] [int] NOT NULL,
 CONSTRAINT [PK_Student_Collect] PRIMARY KEY CLUSTERED
(
 [Stc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


--学员的笔记
CREATE TABLE [dbo].[Student_Notes](
 [Stn_ID] [int] IDENTITY(1,1) NOT NULL,
 [Stn_PID] [int] NULL,
 [St_ID] [int] NOT NULL,
  [Qus_ID] [int] NOT NULL,
 [Stn_Title] [nvarchar](100) NULL,
 [Stn_Context] [nvarchar](255) NULL,
 [Stn_CrtTime] [datetime] NULL,
 [Org_ID] [int] NOT NULL,
 [Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_Student_Notes] PRIMARY KEY CLUSTERED
(
 [Stn_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



/*
主要升级
1、针对题库管理开发；
2、专业，学科，章节支持多级，试题与章节关联
时间：2015-08-05
*/

--专业，支持多级，增加PID，层深，层级路径
alter table [Subject] add Sbj_PID int default 0 not null
alter table [Subject] add Sbj_Level int default 0 not null
alter table [Subject] add Sbj_XPath  [nvarchar](255) NULL
alter table [Subject] add [Sbj_Logo] [nvarchar](100) NULL
alter table [Subject] add [Sbj_LogoSmall] [nvarchar](100) NULL
         --当前专业下有多少科目
alter table [Subject] add [Sbj_CouNumber] [int] NULL
go
UPDATE [Subject]
   SET [Sbj_CouNumber]=0
go
ALTER TABLE [Subject] ALTER COLUMN [Sbj_CouNumber] [int] Not NULL

--课程（或叫科目），支持多级，增加PID，层深，层级路径
alter table [Course] add Cou_PID int default 0 not null
alter table [Course] add Cou_Level int default 0 not null
alter table [Course] add Cou_XPath  [nvarchar](255) NULL
alter table [Course] add [Cou_IsTry] [bit] NOT NULL,
alter table [Course] add [Cou_TryNum] [int] NOT NULL

--章节，增加层级路径，试题数，机构id,专业id
alter table [Outline] add Ol_XPath  [nvarchar](255) NULL
alter table [Outline] add Ol_QusNumber int default 0 not null
alter table [Outline] add Org_ID int default 0 not null
alter table [Outline] add Sbj_ID int default 0 not null


--考试指南的分类与信息
CREATE TABLE [dbo].[GuideColumns](
 [Gc_ID] [int] IDENTITY(1,1) NOT NULL,
 [Gc_PID] [int] not NULL,
[Cou_ID] [int] NOT NULL,
 [Cou_Name] [nvarchar](255) NULL,
 [Gc_ByName] [nvarchar](255) NULL,
 [Gc_Title] [nvarchar](255) NULL,
 [Gc_Keywords] [nvarchar](255) NULL,
 [Gc_Descr] [nvarchar](255) NULL,
 [Gc_Intro] [ntext] NULL,
 [Gc_Type] [nvarchar](255) NULL,
 [Gc_Tax] [int] NOT NULL,
 [Gc_IsUse] [bit] NOT NULL,
 [Gc_IsNote] [bit] NOT NULL,
 [Gc_CrtTime] [datetime] not NULL,
 [Org_ID] [int] NOT NULL,
 [Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [aaaaaGuideColumns_PK] PRIMARY KEY NONCLUSTERED
(
 [Gc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


CREATE TABLE [dbo].[Guide](
 [Gu_Id] [int] IDENTITY(1,1) NOT NULL,
 [Gc_Title] [nvarchar](255) NULL,
 [Gc_ID] [int] NULL,
[Cou_ID] [int] NOT NULL,
 [Cou_Name] [nvarchar](255) NULL,
 [Gu_Title] [nvarchar](255) NULL,
 [Gu_TitleAbbr] [nvarchar](50) NULL,
 [Gu_TitleFull] [nvarchar](255) NULL,
 [Gu_TitleSub] [nvarchar](255) NULL,
 [Gu_Color] [nvarchar](50) NULL,
 [Gu_Font] [nvarchar](50) NULL,
 [Gu_IsError] [bit] NOT NULL,
 [Gu_ErrInfo] [nvarchar](255) NULL,
 [Gu_IsUse] [bit] NOT NULL,
 [Gu_IsShow] [bit] NOT NULL,
 [Gu_IsImg] [bit] NOT NULL,
 [Gu_IsHot] [bit] NOT NULL,
 [Gu_IsTop] [bit] NOT NULL,
 [Gu_IsRec] [bit] NOT NULL,
 [Gu_IsDel] [bit] NOT NULL,
 [Gu_IsVerify] [bit] NOT NULL,
 [Gu_VerifyMan] [nvarchar](50) NULL,
 [Gu_IsOut] [bit] NOT NULL,
 [Gu_OutUrl] [nvarchar](255) NULL,
 [Gu_Keywords] [nvarchar](255) NULL,
 [Gu_Descr] [nvarchar](255) NULL,
 [Gu_Author] [nvarchar](50) NULL,
 [Acc_Id] [int] NULL,
 [Acc_Name] [nvarchar](255) NULL,
 [Gu_Source] [nvarchar](100) NULL,
 [Gu_Intro] [ntext] NULL,
 [Gu_Details] [ntext] NULL,
 [Gu_Endnote] [ntext] NULL,
 [Gu_CrtTime] [datetime] NULL,
 [Gu_LastTime] [datetime] NULL,
 [Gu_VerifyTime] [datetime] NULL,
 [Gu_Number] [int] NOT NULL,
 [Gu_IsNote] [bit] NOT NULL,
 [Gu_Logo] [nvarchar](255) NULL,
 [Gu_IsStatic] [bit] NOT NULL,
 [Gu_PushTime] [datetime] NULL,
 [Gu_Label] [nvarchar](255) NULL,
 [Gu_Uid] [nvarchar](64) NULL,
 [OtherData] [ntext] NULL,
 [Org_ID] [int] NOT NULL,
 [Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [aaaaaGuide_PK] PRIMARY KEY NONCLUSTERED
(
 [Gu_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


SQL升级脚本

--试卷表，增加课程ID
alter table [TestPaper] add [Cou_ID] [int] default 0 NULL
update [TestPaper] set [Cou_ID]=0
ALTER TABLE [TestPaper] ALTER COLUMN [Cou_ID] [int] not NULL
--其它，增加科目UID
alter table [Course] add [Cou_UID] [nvarchar](100) NULL
alter table [Questions] add [Qus_IsWrong] [bit] NULL
update [Questions] set [Qus_IsWrong]=1
ALTER TABLE [Questions] ALTER COLUMN [Qus_IsWrong] [bit] not NULL
alter table [Student] add [St_Money] [real] NULL
update [Student] set [St_Money]=0
ALTER TABLE [Student] ALTER COLUMN [St_Money] [real] not NULL

--测试成绩，增加课程ID
alter table TestResults add [Cou_ID] [int] default 0 NULL
update TestResults set [Cou_ID]=0
ALTER TABLE TestResults ALTER COLUMN [Cou_ID] [int] not NULL

--专业，增加图标与计数
alter table [Subject] add [Sbj_CouNumber] [int] default 0 NULL
update [Subject] set [Sbj_CouNumber]=0
ALTER TABLE [Subject] ALTER COLUMN [Sbj_CouNumber] [int] not NULL
alter table [Subject] add [Sbj_Logo] [nvarchar](100) NULL
alter table [Subject] add [Sbj_LogoSmall] [nvarchar](100) NULL
--课程价格
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoursePrice](
 [CP_ID] [int] IDENTITY(1,1) NOT NULL,
 [CP_Tax] [int] NOT NULL,
 [CP_Price] [int] NOT NULL,
 [CP_Span] [int] NOT NULL,
 [CP_Unit] [nvarchar](100) NULL,
 [CP_IsUse] [bit] NOT NULL,
 [CP_Group] [nvarchar](100) NULL,
 [Cou_ID] [int] NOT NULL,
 [Cou_UID] [nvarchar](100) NULL,
 [Org_ID] [int] NOT NULL,
 CONSTRAINT [PK_CoursePrice] PRIMARY KEY CLUSTERED
(
 [CP_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
--资金流水
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MoneyAccount](
 [Ma_ID] [int] IDENTITY(1,1) NOT NULL,
 [St_ID] [int] NOT NULL,
 [Ma_Total] [real] NOT NULL,
 [Ma_Monery] [real] NOT NULL,
 [Ma_Source] [nvarchar](200) NULL,
 [Ma_Type] [int] NOT NULL,
 [Ma_Info] [nvarchar](500) NULL,
 [Ma_Remark] [nvarchar](1000) NULL,
 [Ma_CrtTime] [datetime] NOT NULL,
 [Rc_Code] [nvarchar](100) NULL,
 [Org_ID] [int] NOT NULL,
 [Ma_Serial] [nvarchar](100) NULL,
 CONSTRAINT [PK_MoneyAccount] PRIMARY KEY CLUSTERED
(
 [Ma_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
--充值码管理
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RechargeCode](
 [Rc_ID] [int] IDENTITY(1,1) NOT NULL,
 [Rc_Code] [nvarchar](100) NULL,
 [Rc_Pw] [nvarchar](20) NULL,
 [Rc_Price] [int] NOT NULL,
 [Rc_CrtTime] [datetime] NOT NULL,
 [Rc_UsedTime] [datetime] NOT NULL,
 [Rc_IsEnable] [bit] NULL,
 [Rc_IsUsed] [bit] NOT NULL,
 [Rc_Type] [int] NOT NULL,
 [Rs_ID] [int] NOT NULL,
 [Org_ID] [int] NOT NULL,
 [St_ID] [int] NOT NULL,
 [St_AccName] [nvarchar](50) NULL,
 [Rc_LimitStart] [datetime] NOT NULL,
 [Rc_LimitEnd] [datetime] NOT NULL,
 CONSTRAINT [PK_RechargeCode] PRIMARY KEY CLUSTERED
(
 [Rc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RechargeSet](
 [Rs_ID] [int] IDENTITY(1,1) NOT NULL,
 [Rs_Count] [int] NOT NULL,
 [Rs_Price] [int] NOT NULL,
 [Rs_CrtTime] [datetime] NOT NULL,
 [Rs_Theme] [nvarchar](200) NULL,
 [Rs_Intro] [nvarchar](1000) NULL,
 [Rs_Pw] [nvarchar](100) NULL,
 [Rs_UsedCount] [int] NOT NULL,
 [Org_ID] [int] NOT NULL,
 [Rs_LimitStart] [datetime] NOT NULL,
 [Rs_LimitEnd] [datetime] NOT NULL,
 [Rs_IsEnable] [bit] NOT NULL,
 [Rs_CodeLength] [int] NOT NULL,
 [Rs_PwLength] [int] NOT NULL,
 CONSTRAINT [PK_Recharge] PRIMARY KEY CLUSTERED
(
 [Rs_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

--学员学习课程的关联表
/****** Object:  Table [dbo].[Student_Course]    Script Date: 10/27/2015 22:29:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student_Course]') AND type in (N'U'))
DROP TABLE [dbo].[Student_Course]
GO
/****** Object:  Table [dbo].[Student_Course]    Script Date: 10/27/2015 22:29:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Course](
 [Stc_ID] [int] IDENTITY(1,1) NOT NULL,
 [St_ID] [int] NOT NULL,
 [Cou_ID] [int] NOT NULL,
 [Stc_CrtTime] [datetime] NOT NULL,
 [Stc_Money] [real] NOT NULL,
 [Stc_StartTime] [datetime] NOT NULL,
 [Stc_EndTime] [datetime] NOT NULL,
 [Org_ID] [int] NOT NULL,
 [Rc_Code] [nvarchar](100) NULL,
 CONSTRAINT [PK_Student_Course] PRIMARY KEY CLUSTERED
(
 [Stc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--试题收藏
/****** Object:  Table [dbo].[Student_Collect]    Script Date: 10/27/2015 23:41:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student_Collect]') AND type in (N'U'))
DROP TABLE [dbo].[Student_Collect]
GO
/****** Object:  Table [dbo].[Student_Collect]    Script Date: 10/27/2015 23:41:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Collect](
 [Stc_ID] [int] IDENTITY(1,1) NOT NULL,
 [St_ID] [int] NOT NULL,
 [Cou_ID] [int] NOT NULL,
 [Qus_ID] [int] NOT NULL,
 [Qus_Type] [int] NOT NULL,
 [Qus_Diff] [int] NOT NULL,
 [Qus_Title] [nvarchar](max) NULL,
 [Sbj_ID] [int] NOT NULL,
 [Stc_Level] [int] NOT NULL,
 [Stc_Strange] [int] NOT NULL,
 [Stc_CrtTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Student_Collect] PRIMARY KEY CLUSTERED
(
 [Stc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
--笔记与错题
/****** Object:  Table [dbo].[Student_Ques]    Script Date: 10/27/2015 23:43:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student_Ques]') AND type in (N'U'))
DROP TABLE [dbo].[Student_Ques]
GO
/****** Object:  Table [dbo].[Student_Ques]    Script Date: 10/27/2015 23:43:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Ques](
 [Squs_ID] [int] IDENTITY(1,1) NOT NULL,
 [St_ID] [int] NOT NULL,
 [Cou_ID] [int] NOT NULL,
 [Qus_ID] [int] NOT NULL,
 [Qus_Type] [int] NOT NULL,
 [Qus_Diff] [int] NOT NULL,
 [Sbj_ID] [int] NOT NULL,
 [Squs_Level] [int] NOT NULL,
 [Squs_CrtTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Student_Quest] PRIMARY KEY CLUSTERED
(
 [Squs_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student_Notes]    Script Date: 10/27/2015 23:42:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student_Notes]') AND type in (N'U'))
DROP TABLE [dbo].[Student_Notes]
GO
/****** Object:  Table [dbo].[Student_Notes]    Script Date: 10/27/2015 23:42:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student_Notes](
 [Stn_ID] [int] IDENTITY(1,1) NOT NULL,
 [Stn_PID] [int] NULL,
 [St_ID] [int] NOT NULL,
 [Cou_ID] [int] NOT NULL,
 [Qus_ID] [int] NOT NULL,
 [Qus_Type] [int] NOT NULL,
 [Qus_Title] [nvarchar](max) NULL,
 [Stn_Title] [nvarchar](100) NULL,
 [Stn_Context] [nvarchar](1000) NULL,
 [Stn_CrtTime] [datetime] NOT NULL,
 [Org_ID] [int] NOT NULL,
 [Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_Student_Notes] PRIMARY KEY CLUSTERED
(
 [Stn_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/*
试题增加科目与章节字段
*/

alter table [Questions] add [Cou_ID] [int] NULL
alter table [Questions] add [Ol_ID] [int] NULL
go
update [Questions] set [Cou_ID]=0,[Ol_ID]=0 where [Cou_ID] is null and [Ol_ID] is null
go
alter table [Questions] ALTER COLUMN [Cou_ID] [int] not NULL
alter table [Questions] ALTER COLUMN [Ol_ID] [int] not NULL

/* 增加机构自定义配置*/
alter table [Organization] add [Org_Config] [nvarchar](max) NULL
go
