--2017-07-10
/*
一、主要升级内容：
1、统一学员与教师账户，采用同一账户、密码登录；教师与学员只是账户的两种身份，初次注册即为学员，可以申请成为教师。
2、账号可以跨机构登录，即A机构的账户，可以在B机构登录，并可以学习B机构课程；
3、注册方式采用手机短信验证；
登录方式，“账号密码登录”或“短信码登录”两种方式。
4、账户增加卡券、积分功能；
积分包括初次注册、转发链接、转发链接产生注册会员。
5、增加分享功能，学员转发课程或系统任何网页，即可以赚积积分；
用户在分享链接注册，即成为分享人的下级会员，将享受下级会员消费提成（后续会增加分润功能）
6、课程视频增加“从服务器端选择文件”与“添加外部链接”；
二、其它：
1、二维码采用Base64编码格式，避免IO冲突。
2、新闻频道页支持分栏展示。
三、修复漏洞：
1、上传文件限制（之前只有客户端验证，此次增加服务器端验证，双重验证更安全）；
2、修复XXE安全漏洞（系统通过xml进行数据交换，存在黑客截获数据并伪造攻击性代码的可能性）
*/ 
/*创建基本账户表*/
CREATE TABLE [dbo].[Accounts](
	[Ac_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ac_PID] [int] NOT NULL,
	[Ac_AccName] [nvarchar](50) NULL,
	[Ac_Pw] [nvarchar](100) NULL,
	[Ac_Name] [nvarchar](50) NULL,
	[Ac_Pinyin] [nvarchar](50) NULL,
	[Ac_IDCardNumber] [nvarchar](50) NULL,
	[Ac_Signature] [nvarchar](255) NULL,
	[Ac_Age] [int] NOT NULL,
	[Ac_Sex] [int] NOT NULL,
	[Ac_Photo] [nvarchar](255) NULL,
	[Ac_Money] [money] NOT NULL,
	[Ac_Point] [int] NOT NULL,
	[Ac_Coupon] [int] NOT NULL,
	[Ac_Birthday] [datetime] NOT NULL,
	[Ac_IsUse] [bit] NOT NULL,
	[Ac_IsPass] [bit] NOT NULL,
	[Ac_IsTeacher] [bit] NOT NULL,
	[Ac_Qus] [nvarchar](255) NULL,
	[Ac_Ans] [nvarchar](255) NULL,
	[Ac_RegTime] [datetime] NOT NULL,
	[Ac_OutTime] [datetime] NOT NULL,
	[Ac_LastTime] [datetime] NOT NULL,
	[Ac_LastIP] [nvarchar](255) NULL,
	[Ac_Tel] [nvarchar](50) NULL,
	[Ac_IsOpenTel] [bit] NOT NULL,
	[Ac_MobiTel1] [nvarchar](50) NULL,
	[Ac_MobiTel2] [nvarchar](50) NULL,
	[Ac_IsOpenMobile] [bit] NOT NULL,
	[Ac_Email] [nvarchar](50) NULL,
	[Ac_Qq] [nvarchar](50) NULL,
	[Ac_Weixin] [nvarchar](100) NULL,
	[Ac_CheckUID] [nvarchar](255) NULL,
	[Ac_UID] [nvarchar](255) NULL,
	[Org_ID] [int] NOT NULL,
	[Ac_Zip] [nvarchar](50) NULL,
	[Ac_LinkMan] [nvarchar](50) NULL,
	[Ac_LinkManPhone] [nvarchar](50) NULL,
	[Ac_Intro] [nvarchar](2000) NULL,
	[Ac_Major] [nvarchar](255) NULL,
	[Ac_Education] [nvarchar](255) NULL,
	[Ac_Native] [nvarchar](255) NULL,
	[Ac_Nation] [nvarchar](50) NULL,
	[Ac_CodeNumber] [nvarchar](50) NULL,
	[Ac_Address] [nvarchar](255) NULL,
	[Ac_AddrContact] [nvarchar](255) NULL,
	[Dep_Id] [int] NOT NULL,
	[Sts_ID] [int] NOT NULL,
	[Sts_Name] [nvarchar](255) NULL,
	[Ac_CurrCourse] [int] NOT NULL,
 CONSTRAINT [aaaaaAccounts_PK] PRIMARY KEY CLUSTERED 
(
	[Ac_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
/*积分总计的字段*/
go
alter table Accounts add Ac_PointAmount [int] NULL
go
update Accounts set Ac_PointAmount=0
go
alter table Accounts ALTER COLUMN Ac_PointAmount [int] NOT NULL
GO
/*增加教师表与基本账户表的关联*/
alter table [Teacher] add Ac_ID [int] NULL
go
update [Teacher] set Ac_ID=0
go
alter table [Teacher] ALTER COLUMN Ac_ID [int] NOT NULL
go
alter table [Teacher] add [Ac_UID] [nvarchar](255) NULL
go
/*修正其它相关表的学员字段*/
sp_rename 'Forum.St_ID', 'Ac_ID'
go
sp_rename 'Forum.St_Name', 'Ac_Name'
go
sp_rename 'TestResults.St_ID', 'Ac_ID'
go
sp_rename 'TestResults.St_Name', 'Ac_Name'
go
sp_rename 'Student_Ques.St_ID', 'Ac_ID'
go
sp_rename 'Student_Notes.St_ID', 'Ac_ID'
go
sp_rename 'Student_Course.St_ID', 'Ac_ID'
go
sp_rename 'Student_Collect.St_ID', 'Ac_ID'
go
sp_rename 'RechargeCode.St_ID', 'Ac_ID'
go
sp_rename 'RechargeCode.St_AccName', 'Ac_AccName'
go
sp_rename 'MoneyAccount.St_ID', 'Ac_ID'
go
sp_rename 'MessageBoard.St_ID', 'Ac_ID'
go
sp_rename 'MessageBoard.St_Name', 'Ac_Name'
go
sp_rename 'MessageBoard.St_Photo', 'Ac_Photo'
go
sp_rename 'Message.St_ID', 'Ac_ID'
go
sp_rename 'LogForStudentStudy.St_ID', 'Ac_ID'
go
sp_rename 'LogForStudentStudy.St_Name', 'Ac_Name'
go
sp_rename 'LogForStudentStudy.St_AccName', 'Ac_AccName'
go
sp_rename 'LogForStudentOnline.St_ID', 'Ac_ID'
go
sp_rename 'LogForStudentOnline.St_Name', 'Ac_Name'
go
sp_rename 'LogForStudentOnline.St_AccName', 'Ac_AccName'
go
sp_rename 'ExamResults.St_ID', 'Ac_ID'
go
sp_rename 'ExamResults.St_Name', 'Ac_Name'
go
sp_rename 'ExamResults.St_Sex', 'Ac_Sex'
go
sp_rename 'ExamResults.St_IDCardNumber', 'Ac_IDCardNumber'
go
/*资金流水记录里，改成money型*/
alter table [MoneyAccount] ALTER COLUMN Ma_Total [money] NOT NULL
go
alter table [MoneyAccount] ALTER COLUMN Ma_Monery [money] NOT NULL
go
/*系统参数字段增加长度*/
alter table [SystemPara] ALTER COLUMN Sys_Value nvarchar(MAX) NULL
go
/*增加附件表中，是否为外部链接*/
alter table Accessory add As_IsOuter [bit] NULL
go
update Accessory set As_IsOuter=0
go
alter table Accessory ALTER COLUMN As_IsOuter [bit] NOT NULL
go


/****** 生成积分账号表 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PointAccount](
	[Pa_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ac_ID] [int] NOT NULL,
	[Pa_Total] [int] NOT NULL,
	[Pa_TotalAmount] [int] NOT NULL,
	[Pa_Value] [int] NOT NULL,
	[Pa_Source] [nvarchar](200) NULL,
	[Pa_Type] [int] NOT NULL,
	[Pa_Info] [nvarchar](500) NULL,
	[Pa_Remark] [nvarchar](1000) NULL,
	[Pa_CrtTime] [datetime] NOT NULL,
	[Org_ID] [int] NOT NULL,
	[Pa_Serial] [nvarchar](100) NULL,	
	[Pa_From] [int] NOT NULL,	
 CONSTRAINT [PK_PointAccount] PRIMARY KEY CLUSTERED 
(
	[Pa_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

/****** 生成卡券账号表 ******/
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CouponAccount](
	[Ca_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ac_ID] [int] NOT NULL,
	[Ca_Total] [int] NOT NULL,
	[Ca_TotalAmount] [int] NOT NULL,
	[Ca_Value] [int] NOT NULL,
	[Ca_Source] [nvarchar](200) NULL,
	[Ca_Type] [int] NOT NULL,
	[Ca_Info] [nvarchar](500) NULL,
	[Ca_Remark] [nvarchar](1000) NULL,
	[Ca_CrtTime] [datetime] NOT NULL,
	[Org_ID] [int] NOT NULL,
	[Ca_Serial] [nvarchar](100) NULL,	
	[Ca_From] [int] NOT NULL,	
 CONSTRAINT [PK_CouponAccount] PRIMARY KEY CLUSTERED 
(
	[Ca_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/*试题增加序号字段*/
alter table Questions add Qus_Tax [int] NULL
go
update Questions set Qus_Tax=0
go
alter table Questions ALTER COLUMN Qus_Tax [int] NOT NULL
go

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__Dep_Id__61316BF4]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__Dep_Id__61316BF4]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__EGrp_Id__6225902D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__EGrp_Id__6225902D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__Posi_Id__6319B466]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__Posi_Id__6319B466]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__MM_Id__640DD89F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__MM_Id__640DD89F]
END

GO


GO

/****** Object:  Table [dbo].[Purview]    Script Date: 07/10/2017 09:44:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purview]') AND type in (N'U'))
DROP TABLE [dbo].[Purview]
GO



GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Ro__01D345B0]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Ro__01D345B0]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Ta__02C769E9]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Ta__02C769E9]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Is__03BB8E22]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Is__03BB8E22]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Is__04AFB25B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Is__04AFB25B]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Is__05A3D694]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Is__05A3D694]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Is__0697FACD]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Is__0697FACD]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_St__078C1F06]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_St__078C1F06]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Wi__0880433F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Wi__0880433F]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__ManageMen__MM_Wi__09746778]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ManageMenu] DROP CONSTRAINT [DF__ManageMen__MM_Wi__09746778]
END

GO


/****** Object:  Table [dbo].[SystemPara]    Script Date: 07/10/2017 12:15:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SystemPara]') AND type in (N'U'))
DROP TABLE [dbo].[SystemPara]
GO


GO

/****** Object:  Table [dbo].[ManageMenu]    Script Date: 07/10/2017 09:44:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ManageMenu]') AND type in (N'U'))
DROP TABLE [dbo].[ManageMenu]
GO



GO
/****** Object:  Table [dbo].[Purview]    Script Date: 07/10/2017 09:43:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purview](
	[Pur_Id] [int] IDENTITY(1,1) NOT NULL,
	[Dep_Id] [int] NULL,
	[EGrp_Id] [int] NULL,
	[Posi_Id] [int] NULL,
	[MM_Id] [int] NULL,
	[Pur_State] [nvarchar](50) NULL,
	[Pur_Type] [nvarchar](50) NULL,
	[Org_ID] [int] NULL,
	[Olv_ID] [int] NULL,
 CONSTRAINT [aaaaaPurview_PK] PRIMARY KEY NONCLUSTERED 
(
	[Pur_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Pur_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Pur_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Dep_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Dep_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Dep_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'EGrp_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'EGrp_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'EGrp_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Posi_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Posi_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Posi_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Pur_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Pur_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_State'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Pur_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Pur_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'Pur_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'DateCreated', @value=N'2013/1/30 6:54:53' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'LastUpdated', @value=N'2013/1/30 6:54:53' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DefaultView', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限记录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderByOn', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Orientation', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'RecordCount', @value=N'70' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
EXEC sys.sp_addextendedproperty @name=N'Updatable', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview'
GO
SET IDENTITY_INSERT [dbo].[Purview] ON
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5326, NULL, NULL, NULL, 522, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5327, NULL, NULL, NULL, 525, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5328, NULL, NULL, NULL, 621, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5329, NULL, NULL, NULL, 524, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4131, NULL, NULL, 13, 522, N'half', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4132, NULL, NULL, 13, 525, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5330, NULL, NULL, NULL, 643, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5331, NULL, NULL, NULL, 527, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5332, NULL, NULL, NULL, 539, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4133, NULL, NULL, 13, 535, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4134, NULL, NULL, 13, 544, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4135, NULL, NULL, 13, 524, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5333, NULL, NULL, NULL, 627, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4137, NULL, NULL, 13, 545, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4138, NULL, NULL, 13, 610, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5334, NULL, NULL, NULL, 665, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5335, NULL, NULL, NULL, 544, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5336, NULL, NULL, NULL, 545, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5337, NULL, NULL, NULL, 535, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5338, NULL, NULL, NULL, 610, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5339, NULL, NULL, NULL, 526, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5340, NULL, NULL, NULL, 537, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5341, NULL, NULL, NULL, 538, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5342, NULL, NULL, NULL, 536, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5343, NULL, NULL, NULL, 523, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5344, NULL, NULL, NULL, 528, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5345, NULL, NULL, NULL, 546, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5346, NULL, NULL, NULL, 630, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5347, NULL, NULL, NULL, 631, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5348, NULL, NULL, NULL, 632, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5349, NULL, NULL, NULL, 633, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5350, NULL, NULL, NULL, 607, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5351, NULL, NULL, NULL, 618, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5352, NULL, NULL, NULL, 619, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5353, NULL, NULL, NULL, 609, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5354, NULL, NULL, NULL, 637, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5355, NULL, NULL, NULL, 639, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5356, NULL, NULL, NULL, 640, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5357, NULL, NULL, NULL, 638, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5358, NULL, NULL, NULL, 642, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5359, NULL, NULL, NULL, 641, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5360, NULL, NULL, NULL, 565, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5361, NULL, NULL, NULL, 530, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5362, NULL, NULL, NULL, 532, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5363, NULL, NULL, NULL, 566, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5364, NULL, NULL, NULL, 568, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5365, NULL, NULL, NULL, 547, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5366, NULL, NULL, NULL, 548, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5367, NULL, NULL, NULL, 550, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5368, NULL, NULL, NULL, 552, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5369, NULL, NULL, NULL, 553, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5370, NULL, NULL, NULL, 554, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5371, NULL, NULL, NULL, 628, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5372, NULL, NULL, NULL, 561, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5373, NULL, NULL, NULL, 635, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5374, NULL, NULL, NULL, 636, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5375, NULL, NULL, NULL, 666, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5376, NULL, NULL, NULL, 562, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5377, NULL, NULL, NULL, 563, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5378, NULL, NULL, NULL, 564, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5379, NULL, NULL, NULL, 663, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5380, NULL, NULL, NULL, 664, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5381, NULL, NULL, NULL, 662, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5382, NULL, NULL, NULL, 569, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5383, NULL, NULL, NULL, 570, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5384, NULL, NULL, NULL, 572, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5385, NULL, NULL, NULL, 622, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5386, NULL, NULL, NULL, 626, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3626, NULL, NULL, NULL, 522, N'half', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3627, NULL, NULL, NULL, 523, N'half', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3628, NULL, NULL, NULL, 607, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3629, NULL, NULL, NULL, 547, N'half', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3630, NULL, NULL, NULL, 548, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3631, NULL, NULL, NULL, 550, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3633, NULL, NULL, NULL, 552, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3634, NULL, NULL, NULL, 553, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3635, NULL, NULL, NULL, 554, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3640, NULL, NULL, NULL, 561, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3641, NULL, NULL, NULL, 562, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3642, NULL, NULL, NULL, 563, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3643, NULL, NULL, NULL, 564, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3644, NULL, NULL, NULL, 569, N'half', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3645, NULL, NULL, NULL, 570, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3646, NULL, NULL, NULL, 572, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3649, NULL, NULL, NULL, 583, N'sel', N'orglevel', NULL, 2)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3934, NULL, NULL, NULL, 522, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3935, NULL, NULL, NULL, 526, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3936, NULL, NULL, NULL, 537, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3937, NULL, NULL, NULL, 538, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3938, NULL, NULL, NULL, 536, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3939, NULL, NULL, NULL, 525, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3940, NULL, NULL, NULL, 535, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3941, NULL, NULL, NULL, 544, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3942, NULL, NULL, NULL, 524, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3944, NULL, NULL, NULL, 545, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3945, NULL, NULL, NULL, 610, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3946, NULL, NULL, NULL, 523, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3947, NULL, NULL, NULL, 609, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3948, NULL, NULL, NULL, 528, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3950, NULL, NULL, NULL, 607, N'sel', N'orglevel', NULL, 6)
GO
print 'Processed 100 total records'
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3951, NULL, NULL, NULL, 546, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3952, NULL, NULL, NULL, 529, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3953, NULL, NULL, NULL, 530, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3954, NULL, NULL, NULL, 531, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3955, NULL, NULL, NULL, 532, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3956, NULL, NULL, NULL, 534, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3957, NULL, NULL, NULL, 533, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3958, NULL, NULL, NULL, 565, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3959, NULL, NULL, NULL, 566, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3960, NULL, NULL, NULL, 568, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3961, NULL, NULL, NULL, 547, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3962, NULL, NULL, NULL, 548, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3963, NULL, NULL, NULL, 552, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3964, NULL, NULL, NULL, 553, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3965, NULL, NULL, NULL, 554, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3972, NULL, NULL, NULL, 561, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3973, NULL, NULL, NULL, 562, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3974, NULL, NULL, NULL, 563, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3975, NULL, NULL, NULL, 564, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3976, NULL, NULL, NULL, 569, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3977, NULL, NULL, NULL, 593, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3978, NULL, NULL, NULL, 594, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3979, NULL, NULL, NULL, 595, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3980, NULL, NULL, NULL, 596, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3981, NULL, NULL, NULL, 597, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3982, NULL, NULL, NULL, 598, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3983, NULL, NULL, NULL, 574, N'half', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3984, NULL, NULL, NULL, 576, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3985, NULL, NULL, NULL, 580, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3986, NULL, NULL, NULL, 577, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3987, NULL, NULL, NULL, 584, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3988, NULL, NULL, NULL, 585, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3989, NULL, NULL, NULL, 586, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3990, NULL, NULL, NULL, 587, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3991, NULL, NULL, NULL, 588, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3992, NULL, NULL, NULL, 589, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3993, NULL, NULL, NULL, 600, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3994, NULL, NULL, NULL, 590, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3995, NULL, NULL, NULL, 591, N'sel', N'orglevel', NULL, 6)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5387, NULL, NULL, NULL, 574, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5388, NULL, NULL, NULL, 576, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5389, NULL, NULL, NULL, 580, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5390, NULL, NULL, NULL, 577, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5391, NULL, NULL, NULL, 588, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5392, NULL, NULL, NULL, 589, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5393, NULL, NULL, NULL, 600, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5394, NULL, NULL, NULL, 590, N'sel', N'organ', -1, NULL)
SET IDENTITY_INSERT [dbo].[Purview] OFF
/****** Object:  Table [dbo].[ManageMenu]    Script Date: 07/10/2017 09:43:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManageMenu](
	[MM_Id] [int] IDENTITY(1,1) NOT NULL,
	[MM_Name] [nvarchar](100) NOT NULL,
	[MM_Type] [nvarchar](50) NULL,
	[MM_Root] [int] NOT NULL,
	[MM_Link] [nvarchar](255) NULL,
	[MM_Marker] [nvarchar](255) NULL,
	[MM_Tax] [int] NOT NULL,
	[MM_PatId] [int] NOT NULL,
	[MM_Color] [nvarchar](50) NULL,
	[MM_Font] [nvarchar](50) NULL,
	[MM_IsBold] [bit] NOT NULL,
	[MM_IsItalic] [bit] NOT NULL,
	[MM_IcoS] [nvarchar](255) NULL,
	[MM_IcoB] [nvarchar](255) NULL,
	[MM_IsUse] [bit] NOT NULL,
	[MM_IsShow] [bit] NOT NULL,
	[MM_Intro] [nvarchar](255) NULL,
	[MM_State] [bit] NOT NULL,
	[MM_Func] [nvarchar](50) NULL,
	[MM_WinWidth] [int] NOT NULL,
	[MM_WinHeight] [int] NOT NULL,
	[MM_IcoX] [int] NOT NULL,
	[MM_IcoY] [int] NOT NULL,
 CONSTRAINT [aaaaaManageMenu_PK] PRIMARY KEY NONCLUSTERED 
(
	[MM_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'1080' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'䏯꟮䕃鮪뗖䵲' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'2760' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'հ碱얚䤇劽ꉸ�늊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'戊恥䊻➇⥸惰玑' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Type' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Type'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'쓽�黫䛚ﲟᡴ覈' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Root' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Root' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Root'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'턡ﾟᅗ䰡�ᩢ㳧泞' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'቙跽⍠䳹ﾄ㨲㱢̿' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Marker' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Marker' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Marker'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'頢ꋭ㓰䨚㪵ⶵꫡ瘯' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Tax' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Tax' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Tax'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'蕾筍䣣잯稨俪' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_PatId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'7' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_PatId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_PatId'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'⻚촫䳀ኳꓻ쪌⠋' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Color' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'8' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Color' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Color'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'䙪쫔뉃䴐⍱䡍朙' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Font' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'9' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Font' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Font'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'ض㞝䑒늾覈ꗡ퀃' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IsBold' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IsBold' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsBold'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'㟳ᚴ䵒䊈跆躼ꗒ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IsItalic' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'11' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IsItalic' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsItalic'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'睛帞䘮䀿璷�꼆祡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IcoS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'12' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IcoS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoS'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'싟�䫣䴣﮸뗐믵뤻' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IcoB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'13' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IcoB' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoB'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'벓䉭咒ᙕ놩遢' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IsUse' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'14' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IsUse' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsUse'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'⌞艭뿱乞ẅ馲䈱敍' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IsShow' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'15' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IsShow' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsShow'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'팇퐏쮦䱉ꮤ鹰㶘쨾' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Intro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'16' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Intro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Intro'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'䐖鋖ꎩ䈢겒୙ᓥょ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Format', @value=N'Yes/No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_State'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'㉶�駯䪀颦횮' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Func' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'18' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'50' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Func' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_Func'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'뻦�ᓭ争ꚓᇀ敚획' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_WinWidth' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'19' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_WinWidth' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinWidth'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'檧츘胖䨁玳叒拀⡤' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_WinHeight' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'20' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_WinHeight' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_WinHeight'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackShade', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackTint', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'BackShade', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'BackTint', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'DatasheetForeThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'DatasheetGridlinesThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'DateCreated', @value=N'2014/9/15 9:42:35' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'DisplayViewsOnSharePointSite', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'FilterOnLoad', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'HideNewField', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'LastUpdated', @value=N'2014/9/15 9:42:35' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DefaultView', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统管理菜单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderByOn', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Orientation', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'OrderByOnLoad', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'RecordCount', @value=N'242' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'ThemeFontIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'TotalsRow', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'Updatable', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu'
GO
SET IDENTITY_INSERT [dbo].[ManageMenu] ON
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (625, N'试题导出', N'item', 569, N'/manage/Questions/Export.aspx', N'', 51, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (626, N'我的学员', N'item', 569, N'/manage/teacher/mystudent.aspx', N'', 251, 570, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (220, N'全站备份', N'item', 88, N'sys/sitebackup.aspx', N'', 80, 125, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (221, N'首选项', N'item', 88, N'', N'', 10, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (222, N'常规', N'item', 88, N'sys/general.aspx', N'', 40, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (627, N'学员在线', N'item', 522, N'/manage/admin/stonline.aspx', N'', 230, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (628, N'在线时间', N'item', 547, N'/manage/student/online.aspx', N'', 140, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 78, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (629, N'学习记录', N'item', 547, N'/manage/student/studylog.aspx', N'', 150, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 101, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (88, N'系统设置', N'item', 88, N'', N'system', 17, 0, N'', N'', 0, 0, NULL, NULL, 1, 1, N'请不要轻易改动！', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (89, N'功能菜单', N'item', 88, N'sys/menuroot.aspx', N'MenuTree', 110, 101, N'', N'', 0, 0, NULL, NULL, 1, 1, N'管理界面左侧的菜单', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (90, N'数据库备份', N'item', 88, N'sys/databasebackup.aspx', N'', 170, 125, N'', N'', 0, 0, NULL, NULL, 1, 1, N'此操作，仅限对access版的数据库备份。', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (94, N'修改密码', N'item', 92, N'ty', NULL, 5, 95, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (95, N'用户管理', N'item', 92, N'ty', NULL, 10, 92, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 1, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (96, N'查询用户信息', N'item', 92, N'ty', NULL, 10, 95, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (101, N'菜单管理', N'item', 88, N'', NULL, 90, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (102, N'系统菜单', N'item', 88, N'sys/SysMenu.aspx', NULL, 160, 101, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'位于管理界面右上方的下拉菜单', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (103, N'系统菜单', NULL, 103, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 1, NULL, 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (104, N'账户', N'item', 103, N'#', N'', 60, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (108, N'关于', N'open', 103, N'help/about.html', N'', 85, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 400, 200, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (111, N'基本资料', N'open', 103, N'Personal/info.aspx', N'', 30, 104, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'用户基本资料', 0, N'sys', 640, 450, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (112, N'密码修改', N'open', 103, N'Personal/password.aspx', N'', 20, 104, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (113, N'首页', N'link', 103, N'/', N'', 10, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (121, N'退出', N'event', 103, N'if(confirm(''是否真的要退出？'')){window.location.href=''/manage/index.aspx'';}', N'', 100, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (122, N'部门信息', N'item', 88, N'sys/depart.aspx', N'purview', 240, 124, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (124, N'组织机构', N'item', 88, N'', N'', 220, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (125, N'安全管理', N'item', 88, N'', N'', 130, 221, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (128, N'公司信息', N'item', 88, N'sys/organizationinfo.aspx', N'', 200, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (129, N'单位介绍', N'item', 88, N'sys/organizationintro.aspx', N'', 210, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (140, N'工作组', N'item', 88, N'sys/empgroup.aspx', N'purview', 270, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (141, N'岗位设置', N'item', 88, N'sys/position.aspx', N'purview', 250, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (142, N'管理员', N'item', 88, N'sys/employee.aspx', N'', 230, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (193, N'系统日志', N'item', 88, N'', N'', 190, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (194, N'登录日志', N'item', 88, N'Sys/LogsLogin.aspx', N'', 100, 193, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (195, N'操作记录', N'item', 88, N'sys/LogsWork.aspx', N'', 180, 193, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (278, N'系统参数', N'item', 88, N'sys/syspara.aspx', N'', 140, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (316, N'基本信息', N'item', 88, N'', N'', 20, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (369, N'数据清理', N'item', 88, N'sys/dataclear.aspx', N'', 150, 221, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (370, N'职务头衔', N'item', 88, N'sys/title.aspx', N'', 260, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (510, N'机构管理', N'item', 651, N'', N'', 30, 651, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (511, N'机构列表', N'item', 651, N'sys/Organization.aspx', N'', 110, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (512, N'机构审核', N'item', 651, N'sys/OrganVerify.aspx', N'', 120, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (513, N'机构等级', N'item', 651, N'sys/OrganLevel.aspx', N'', 140, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (514, N'机构权限', N'item', 651, N'sys/purview.aspx?type=organ', N'', 130, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (542, N'二级域名预留', N'item', 88, N'sys/limitdomain.aspx', N'', 50, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (522, N'机构的管理', N'item', 522, NULL, N'organAdmin', 18, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 105, 69)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (528, N'机构信息', N'item', 522, N'/manage/sys/organizationinfo.aspx', N'', 390, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 101, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (535, N'教师信息', N'item', 522, N'/manage/teacher/list.aspx', N'', 40, 665, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 162, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (536, N'新闻栏目', N'item', 522, N'/manage/content/columns.aspx', N'', 300, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 85, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (537, N'新闻发布', N'item', 522, N'/manage/content/Contents.aspx?action=modify', N'', 280, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'在', 0, N'func', 400, 300, 180, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (541, N'优秀教师', N'item', 522, N'', N'', 220, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 162, 134)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (544, N'学员管理', N'item', 522, N'/manage/admin/students.aspx', N'', 10, 665, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 156, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (547, N'学员的管理', N'item', 547, NULL, N'student', 19, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (548, N'在线学习', N'item', 547, N'', N'', 160, 547, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 32, 165)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (550, N'我的课程', N'item', 547, N'/manage/student/selfcourse.aspx', N'', 90, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 32, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (623, N'院系设置', N'item', 522, N'/manage/admin/Departs.aspx', N'', 50, 621, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (552, N'测试成绩', N'item', 547, N'/manage/Student/TestArchives.aspx', N'', 110, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 84, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (553, N'考试成绩', N'item', 547, N'/manage/Student/Archives.aspx', N'', 120, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 125, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (554, N'错题回顾', N'item', 547, N'/manage/student/QuesError.aspx', N'', 130, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 187, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (556, N'参加考试', N'item', 547, N'/manage/exam/SelfExam.aspx', N'', 100, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 162, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (561, N'我的账户', N'item', 547, N'', N'', 170, 547, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 106, 69)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (562, N'基本信息', N'item', 547, N'/manage/student/info.aspx', N'', 60, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 186, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (563, N'联系方式', N'item', 547, N'/manage/student/link.aspx', N'', 70, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 210, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (564, N'安全管理', N'item', 547, N'/manage/student/safe.aspx', N'', 80, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 57, 199)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (565, N'管理员', N'item', 522, N'', N'', 450, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 130, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (566, N'个人信息', N'item', 522, N'/manage/Personal/info.aspx', N'', 180, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 179, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (523, N'平台管理', N'item', 522, N'', N'', 370, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 132, 37)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (524, N'专业设置', N'item', 522, N'/manage/sys/Subject.aspx', N'', 250, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 206, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (525, N'教务管理', N'item', 522, N'', N'', 340, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'gggg', 0, N'func', 400, 300, 230, 166)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (545, N'学员班级', N'item', 522, N'/manage/admin/studentsort.aspx', N'', 30, 665, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 8, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (568, N'安全管理', N'item', 522, N'/manage/Personal/safe.aspx', N'', 190, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 6, 166)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (569, N'教师的管理', N'item', 569, NULL, N'teacher', 21, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (570, N'课程管理', N'item', 569, N'', N'', 165, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 32, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (572, N'课程列表', N'item', 569, N'/manage/teacher/courses.aspx', N'', 220, 570, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 230, 68)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (574, N'测试/考试', N'item', 569, N'', N'', 170, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 232, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (607, N'关于我们', N'item', 522, N'/manage/sys/organizationintro.aspx', N'', 420, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (576, N'试卷管理', N'item', 569, N'/manage/exam/testpaper.aspx', N'', 140, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (577, N'考试成绩', N'item', 569, N'/manage/teacher/Archives.aspx', N'', 160, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 57, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (606, N'授权', N'open', 103, N'panel/Authorization.aspx', N'', 90, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 600, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (608, N'模板管理', N'item', 88, N'template/list.aspx', N'', 60, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (580, N'考试管理', N'item', 569, N'/manage/exam/Examination.aspx', N'', 150, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 31, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (609, N'界面风格', N'item', 522, N'/manage/template/select.aspx', N'', 380, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 133, 38)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (610, N'教师职称', N'item', 522, N'/manage/Teacher/sort.aspx', N'', 45, 665, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (583, N'知识库管理', N'item', 569, N'/manage/Knowledge/contents.aspx', N'', 60, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 63, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (584, N'交流', N'item', 569, N'', N'', 200, 569, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 8, 197)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (585, N'留言回复', N'item', 569, N'', N'', 70, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 108, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (586, N'在线解答', N'item', 569, N'', N'', 110, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 135, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (587, N'论坛', N'item', 569, N'/manage/site/MessageBoard.aspx', N'', 120, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (588, N'个人信息', N'item', 569, N'', N'', 210, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 162, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (589, N'基本信息', N'item', 569, N'/manage/teacher/info.aspx', N'', 80, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (590, N'工作履历', N'item', 569, N'/manage/teacher/history.aspx', N'', 100, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 231, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (591, N'密码安全', N'item', 569, N'/manage/teacher/safe.aspx', N'', 130, 588, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 57, 199)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (593, N'题库管理', N'link', 569, N'#', N'', 190, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 91, 102)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (594, N'填空题', N'item', 569, N'/manage/Questions/list.aspx?type=5', N'', 0, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 116, 102)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (595, N'单选题', N'item', 569, N'/manage/Questions/list.aspx?type=1', N'', 20, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 140, 99)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (596, N'多选题', N'item', 569, N'/manage/Questions/list.aspx?type=2', N'', 30, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 162, 98)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (597, N'判断题', N'item', 569, N'/manage/Questions/list.aspx?type=3', N'', 40, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (598, N'简答题', N'item', 569, N'/manage/Questions/list.aspx?type=4', N'', 50, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 210, 98)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (600, N'联系方式', N'item', 569, N'/manage/teacher/link.aspx', N'', 90, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 210, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (622, N'课程交流', N'item', 569, N'/manage/site/MessageBoard.aspx', N'', 250, 570, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (526, N'新闻/通知', N'item', 522, N'', N'', 360, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 32, 164)
GO
print 'Processed 100 total records'
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (527, N'统计分析', N'item', 522, N'', N'', 270, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 57, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (529, N'组织机构', N'item', 522, N'', N'', 440, 522, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 230, 38)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (530, N'管理员列表', N'item', 522, N'/manage/sys/employee.aspx', N'', 0, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (531, N'部门信息', N'item', 522, N'/manage/sys/depart.aspx', N'purview', 310, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 160, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (532, N'角色管理', N'item', 522, N'/manage/sys/position.aspx', N'purview', 170, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 135, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (533, N'职务头衔', N'item', 522, N'/manage/sys/title.aspx', N'', 330, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 108, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (534, N'工作组', N'item', 522, N'/manage/sys/empgroup.aspx', N'purview', 320, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 198)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (538, N'通知', N'item', 522, N'/manage/site/notice.aspx', N'', 290, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 58, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (539, N'热门课程', N'item', 522, N'/Manage/Admin/CourseHot.aspx', N'', 200, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'选课最多的专业', 0, N'func', 400, 300, 83, 37)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (540, N'热门教师', N'item', 522, N'', N'', 210, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 59, 39)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (546, N'参数设置', N'item', 522, N'/manage/admin/organsetup.aspx', N'', 400, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 57, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (618, N'友情链接', N'item', 522, N'/Manage/Site/Links/LinksInfo.aspx', N'', 430, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 33, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (619, N'链接分类', N'item', 522, N'/Manage/Site/Links/LinksSort.aspx', N'', 70, 618, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (621, N'课程管理', N'item', 522, N'/manage/admin/courses.aspx?admin=true', N'', 240, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (630, N'资金管理', N'item', 522, N'/manage/money/Details.aspx', N'', 410, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 206, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (631, N'资金流水', N'item', 522, N'/manage/money/Details.aspx', N'', 140, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 6, 38)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (632, N'充值码', N'item', 522, N'/manage/money/RechargeSet.aspx', N'', 150, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 82, 69)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (633, N'支付接口', N'item', 522, N'/manage/pay/paylist.aspx', N'', 160, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 58, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (637, N'Web模板选择', N'item', 522, N'/manage/template/select.aspx?site=web', N'', 80, 609, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 79, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (638, N'手机模板选择', N'item', 522, N'/manage/template/select.aspx?site=mobi', N'', 130, 609, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 210, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (639, N'Web导航菜单', N'item', 522, N'/manage/admin/Navigation.aspx?site=web', N'', 90, 637, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 33, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (640, N'Web轮换图片', N'item', 522, N'/manage/admin/ShowPicture.aspx?site=web', N'', 120, 637, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 32, 165)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (641, N'手机轮换图片', N'item', 522, N'/manage/admin/ShowPicture.aspx?site=mobi', N'', 110, 638, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 163, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (642, N'手机端导航', N'item', 522, N'/manage/admin/Navigation.aspx?site=mobi', N'', 100, 638, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 34, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (635, N'充值', N'item', 547, N'/Manage/Student/Moneyrecharge.aspx', N'', 0, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (636, N'资金流水', N'item', 547, N'/Manage/Student/MoneyDetails.aspx', N'', 50, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (643, N'题库管理', N'item', 522, N'/manage/Questions/list.aspx', N'', 260, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 91, 102)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (650, N'账户管理', N'item', 651, N'sys/Accounts.aspx', N'', 50, 655, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (651, N'基础设置', N'item', 651, NULL, N'base', 15, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (652, N'短信接口', N'item', 88, N'SMS/SMSSetup.aspx', N'', 70, 654, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (653, N'支付接口', N'item', 88, N'', N'', 120, 654, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (654, N'接口管理', N'item', 88, N'', N'', 135, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (655, N'账号管理', N'item', 651, N'', N'', 0, 651, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (656, N'第三方登录', N'item', 651, N'', N'', 60, 655, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (657, N'注册协议', N'item', 651, N'', N'', 80, 655, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (658, N'教师申请', N'item', 651, N'sys/Agreement.aspx?state=teacher', N'', 20, 657, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (659, N'学员注册', N'item', 651, N'sys/Agreement.aspx?state=accounts', N'', 15, 657, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (660, N'分享/分润/分销', N'item', 651, N'', N'', 101, 651, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (661, N'积分设置', N'item', 651, N'sys/PointSetup.aspx', N'', 0, 660, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (662, N'下级会员', N'item', 547, N'/manage/student/subordinates.aspx', N'', 30, 663, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 72)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (663, N'分享', N'item', 547, N'', N'', 180, 547, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (664, N'分享链接', N'item', 547, N'/manage/student/share.aspx', N'', 20, 663, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 34, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (665, N'注册账户', N'item', 522, N'', N'', 350, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (666, N'积分详情', N'item', 547, N'/Manage/Student/PointDetails.aspx', N'', 55, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
SET IDENTITY_INSERT [dbo].[ManageMenu] OFF
/****** Object:  Default [DF__ManageMen__MM_Ro__01D345B0]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Ro__01D345B0]  DEFAULT ((0)) FOR [MM_Root]
GO
/****** Object:  Default [DF__ManageMen__MM_Ta__02C769E9]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Ta__02C769E9]  DEFAULT ((0)) FOR [MM_Tax]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__03BB8E22]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__03BB8E22]  DEFAULT ((0)) FOR [MM_IsBold]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__04AFB25B]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__04AFB25B]  DEFAULT ((0)) FOR [MM_IsItalic]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__05A3D694]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__05A3D694]  DEFAULT ((1)) FOR [MM_IsUse]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__0697FACD]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__0697FACD]  DEFAULT ((1)) FOR [MM_IsShow]
GO
/****** Object:  Default [DF__ManageMen__MM_St__078C1F06]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_St__078C1F06]  DEFAULT ((1)) FOR [MM_State]
GO
/****** Object:  Default [DF__ManageMen__MM_Wi__0880433F]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Wi__0880433F]  DEFAULT ((0)) FOR [MM_WinWidth]
GO
/****** Object:  Default [DF__ManageMen__MM_Wi__09746778]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Wi__09746778]  DEFAULT ((0)) FOR [MM_WinHeight]
GO
/****** Object:  Default [DF__Purview__Dep_Id__61316BF4]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [Dep_Id]
GO
/****** Object:  Default [DF__Purview__EGrp_Id__6225902D]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [EGrp_Id]
GO
/****** Object:  Default [DF__Purview__Posi_Id__6319B466]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [Posi_Id]
GO
/****** Object:  Default [DF__Purview__MM_Id__640DD89F]    Script Date: 07/10/2017 09:43:26 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [MM_Id]
GO

GO
/****** Object:  Table [dbo].[SystemPara]    Script Date: 07/10/2017 12:14:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemPara](
	[Sys_Id] [int] IDENTITY(1,1) NOT NULL,
	[Sys_Key] [nvarchar](255) NULL,
	[Sys_Value] [nvarchar](max) NULL,
	[Sys_Default] [nvarchar](255) NULL,
	[Sys_ParaIntro] [nvarchar](255) NULL,
	[Sys_Unit] [nvarchar](255) NULL,
	[Sys_SelectUnit] [nvarchar](255) NULL,
	[Org_Id] [int] NULL,
	[Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [aaaaaSystemPara_PK] PRIMARY KEY NONCLUSTERED 
(
	[Sys_Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Id'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'3360' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_Key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Key'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_Value' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Value'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_Default' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_Default' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Default'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_ParaIntro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_ParaIntro' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_ParaIntro'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_Unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'5' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_Unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_Unit'
GO
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'Sys_SelectUnit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'6' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'Sys_SelectUnit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara', @level2type=N'COLUMN',@level2name=N'Sys_SelectUnit'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackShade', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'AlternateBackTint', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'BackShade', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'BackTint', @value=N'100' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'DatasheetForeThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'DatasheetGridlinesThemeColorIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'DateCreated', @value=N'2013/1/30 6:54:53' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'DisplayViewsOnSharePointSite', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'FilterOnLoad', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'HideNewField', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'LastUpdated', @value=N'2013/1/30 6:54:54' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DefaultView', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统参数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_OrderByOn', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Orientation', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'SystemPara' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'OrderByOnLoad', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'RecordCount', @value=N'20' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'ThemeFontIndex', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'TotalsRow', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
EXEC sys.sp_addextendedproperty @name=N'Updatable', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemPara'
GO
SET IDENTITY_INSERT [dbo].[SystemPara] ON
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (204, N'Agreement_accounts', N'请务必认真阅读和理解本《用户服务协议》（以下简称《协议》）中规定的所有权利和限制。除非您接受本《协议》条款，否则您无权注册、登录或使用本协议所涉及的相关服务。您一旦注册、登录、使用或以任何方式使用本《协议》所涉及的相关服务的行为将视为对本《协议》的接受，即表示您同意接受本《协议》各项条款的约束。如果您不同意本《协议》中的条款，请不要注册、登录或使用本《协议》相关服务。<br /><br />一、服务内容<br /><br />1. {platform}为{platform}网站（网址：{domain}，以下简称“{platform}”）的所有者及经营者，完全按照其发布的服务条款和操作规则提供基于互联网以及移动互联网的相关服务（以下简称“网络服务”）。{platform}网站网络服务的具体内容由{platform}根据实际情况提供。<br /><br />2. 您一旦注册成功成为用户，您将得到一个密码和账号，您需要对自己在账户中的所有活动和事件负全责。如果由于您的过失导致您的账号和密码脱离您的控制，则由此导致的针对您、{platform}或任何第三方造成的损害，您将承担全部责任。<br /><br />3. 用户应输入账号和密码登录{platform}账户。若您使用第三方登录使用我们的服务，在进行账号设置时，需要注册{platform}账户。<br /><br />4. 用户理解并接受，{platform}仅提供相关的网络服务，除此之外与相关网络服务有关的设备（如个人电脑、手机、及其他与接入互联网或移动互联网有关的装置）及所需的费用（如为接入互联网而支付的电话费及上网费、为使用移动网而支付的手机费）均应由用户自行负担。<br /><br /><br /><br />二、用户使用规则<br /><br />1. 用户在申请使用{platform}网站网络服务时，必须向{platform}提供准确的个人资料，如个人资料有任何变动，必须及时更新。因用户提供个人资料不准确、不真实而引发的一切后果由用户承担。<br /><br />2. 用户不应将其账号、密码转让、出借或以任何脱离用户控制的形式交由他人使用。如用户发现其账号遭他人非法使用，应立即通知{platform}。因黑客行为或用户的保管疏忽导致账号、密码遭他人非法使用，{platform}不承担任何责任。<br /><br />3. 用户应当为自身注册账户下的一切行为负责，因用户行为而导致的用户自身或其他任何第三方的任何损失或损害，{platform}不承担责任。<br /><br />4. 用户理解并接受{platform}网站提供的服务中可能包括广告，同意在使用网络服务的过程中显示{platform}和第三方供应商、合作伙伴提供的广告。<br /><br />5. 用户在使用{platform}网络服务过程中，必须遵循以下原则：<br />（1）遵守中国有关的法律和法规；<br />（2）遵守所有与网络服务有关的网络协议、规定和程序；<br />（3）不得为任何非法目的而使用网络服务系统；<br />（4） 不得利用{platform}网络服务系统进行任何可能对互联网或移动网正常运转造成不利影响的行为；<br />（5） 不得利用{platform}提供的网络服务上传、展示或传播任何虚假的、骚扰性的、中伤他人的、辱骂性的、恐吓性的、庸俗淫秽的或其他任何非法的信息资料；<br />（6）不得侵犯{platform}和其他任何第三方的专利权、著作权、商标权、名誉权或其他任何合法权益；<br />（7） 不得利用{platform}网络服务系统进行任何不利于{platform}的行为；<br />（8） 如发现任何非法使用用户账号或账号出现安全漏洞的情况，应立即通告{platform}。<br /><br />6. 如用户在使用网络服务时违反任何上述规定，{platform}或其授权的人有权要求用户改正或直接采取一切必要的措施（包括但不限于更改或删除用户收藏的内容等、暂停或终止用户使用网络服务的权利）以减轻用户不当行为造成的影响。<br /><br /><br /><br />三、服务变更、中断或终止<br /><br />1. 鉴于网络服务的特殊性，用户同意{platform}有权根据业务发展情况随时变更、中断或终止部分或全部的网络服务而无需通知用户，也无需对任何用户或任何第三方承担任何责任；<br /><br />2. 用户理解，{platform}需要定期或不定期地对提供网络服务的平台（如互联网网站、移动网络等）或相关的设备进行检修或者维护，如因此类情况而造成网络服务在合理时间内的中断，{platform}无需为此承担任何责任，但{platform}应尽可能事先进行通告。<br /><br />3. 如发生下列任何一种情形，{platform}有权随时中断或终止向用户提供本《协议》项下的网络服务（包括收费网络服务）而无需对用户或任何第三方承担任何责任：<br />（1）用户提供的个人资料不真实；<br />（2）用户违反本《协议》中规定的使用规则。<br /><br /><br /><br />四、知识产权<br /><br />1. {platform}提供的网络服务中包含的任何文本、图片、图形、音频和/或视频资料均受版权、商标和/或其它财产所有权法律的保护，未经相关权利人同意，上述资料均不得用于任何商业目的。<br /><br />2. {platform}为提供网络服务而使用的任何软件（包括但不限于软件中所含的任何图像、照片、动画、录像、录音、音乐、文字和附加程序、随附的帮助材料）的一切权利均属于该软件的著作权人，未经该软件的著作权人许可，用户不得对该软件进行反向工程（reverse engineer）、反向编译（decompile）或反汇编（disassemble）。<br /><br /><br /><br />五、隐私保护<br /><br />1. 保护用户隐私是{platform}的一项基本政策，{platform}保证不对外公开或向第三方提供单个用户的注册资料及用户在使用网络服务时存储在{platform}的非公开内容，但下列情况除外：<br />（1）事先获得用户的明确授权；<br />（2）根据有关的法律法规要求；<br />（3）按照相关政府主管部门的要求；<br />（4）为维护社会公众的利益；<br />（5）为维护{platform}的合法权益。<br /><br />2. {platform}可能会与第三方合作向用户提供相关的网络服务，在此情况下，如该第三方同意承担与{platform}同等的保护用户隐私的责任，则{platform}有权将用户的注册资料等提供给该第三方。<br /><br />3. 在不透露单个用户隐私资料的前提下，{platform}有权对整个用户数据库进行分析并对用户数据库进行商业上的利用。<br /><br /><br /><br />六、免责声明<br /><br />1. {platform}不担保网络服务一定能满足用户的要求，也不担保网络服务不会中断，对网络服务的及时性、安全性、准确性也都不作担保。<br /><br />2. {platform}不保证为向用户提供便利而设置的外部链接的准确性和完整性，同时，对于该等外部链接指向的不由{platform}实际控制的任何网页上的内容，{platform}不承担任何责任。<br /><br />3. 对于因电信系统或互联网网络故障、计算机故障或病毒、信息损坏或丢失、计算机系统问题或其它任何不可抗力原因而产生损失，{platform}不承担任何责任，但将尽力减少因此而给用户造成的损失和影响。<br /><br /><br /><br />七、法律及争议解决<br /><br />1. 本协议适用中华人民共和国法律。<br /><br />2. 因本协议引起的或与本协议有关的任何争议，各方应友好协商解决；协商不成的，任何一方均可将有关争议提交至上海仲裁委员会并按照其届时有效的仲裁规则仲裁；仲裁裁决是终局的，对各方均有约束力。<br /><br /><br /><br />八、其他条款<br /><br />1. 如果本协议中的任何条款无论因何种原因完全或部分无效或不具有执行力，或违反任何适用的法律，则该条款被视为删除，但本协议的其余条款仍应有效并且有约束力。<br /><br />2. {platform}有权随时根据有关法律、法规的变化以及公司经营状况和经营策略的调整等修改本协议，而无需另行单独通知用户。修改后的协议会在{platform}网站（{domain}）上公布。用户可随时通过{platform}网站浏览最新服务协议条款。当发生有关争议时，以最新的协议文本为准。如果不同意{platform}对本协议相关条款所做的修改，用户有权停止使用网络服务。如果用户继续使用网络服务，则视为用户接受{platform}对本协议相关条款所做的修改。<br /><br />3. {platform}在法律允许最大范围对本协议拥有解释权与修改权。<br /><br /><br /><br /><br /><br />', N'', N'', N'', N'', NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (205, N'Agreement_teacher', N'{platform}教师注册协议&nbsp;<br />
<br />
导言&nbsp;<br />
<br />
欢迎您使用{platform}提供的服务！&nbsp;<br />
<br />
为使用{platform}（网址：{domain}）提供的服务（以下简称：本服务），您应当阅读并遵守《{platform}教师注册协议》（以下简称：本协议）相关协议、规则。&nbsp;<br />
<br />
请您务必审慎阅读、充分理解各条款内容，特别是免除或者限制责任的条款，以及开通或使用某项服务的单独协议、规则。&nbsp;<br />
<br />
除非您已阅读并接受本协议及相关协议、规则等所有条款，否则，您无权使用{org}提供的本服务。您使用{org}的本服务，即视为您已阅读并同意上述协议、规则等的约束。&nbsp;<br />
<br />
您有违反本协议的任何行为时，{org}有权依照违反情况，随时单方限制、中止或终止向您提供本服务，并有权追究您的相关责任。&nbsp;<br />
<br />
1.术语含义&nbsp;<br />
<br />
如无特别说明，下列术语在本协议中的含义为：&nbsp;<br />
<br />
1.1 课程发布者：指经有效注册、申请后，将其享有相应权利的各种课程接入{platform}并向用户提供各种免费或收费类的在线直播、录播服务以实现与用户进行在线交流与学习目的的个人、法人或其他组织，因前述主体的不同，课程发布者在对外展现上也称“机构”或“老师”，本协议中简称为“您”。&nbsp;<br />
<br />
1.2 课程：指由课程发布者开发，或课程开发者经权利人授权，通过{platform}向相关用户提供各种免费或收费类的在线直播、录播形式的交流与学习服务，包括但不限于网络营销类、语言类、公务员考试类、小学/初中/高中教育辅导类（非学历教育）等现存的各种培训服务及今后可能出现的各种培训服务。课程是否收费、课程收费的数额及课程收费方式等均由课程发布者自行决定。&nbsp;<br />
<br />
1.3 {platform}：指由{org}所拥有、控制、经营的{org}其他平台或网站及前述各平台网站的下属子页面，以下简称为“{org}平台”、“{platform}”、“平台”。{org}通过{platform}向您提供的服务包括但不限于提供课程运营平台、费用支付服务、广点通推广服务等服务，具体以{org}提供和您选择的服务为准，前述服务称为{platform}服务、平台服务、本服务。&nbsp;<br />
<br />
{org}、课程发布者均同意和理解：&nbsp;<br />
<br />
（1）{platform}是一个中立的平台服务提供者，仅向课程发布者提供信息存储空间、链接等中立的网络服务或相关中立的技术支持服务，以供课程发布者在中立的平台上自主发布、运营、推广其课程等；&nbsp;<br />
<br />
（2）课程发布者的课程由课程发布者自主开发、独立运营并独立承担全部责任。{org}不会、也不可能参与课程发布者课程的研发、运营等任何活动，{org}也不会对课程发布者的课程进行任何的修改、编辑或整理等；&nbsp;<br />
<br />
（3）因课程发布者课程及服务产生的任何纠纷、责任等，以及开发者违反相关法律法规或本协议约定引发的任何后果，均由课程发布者独立承担责任、赔偿损失，与{org}无关。如侵害到{org}或他人权益的，课程发布者须自行承担全部责任和赔偿一切损失。&nbsp;<br />
<br />
1.4 {platform}运营数据：是指用户、课程发布者在使用{platform}相关服务中产生的相关运营数据，包括但不限于用户或课程发布者操作行为形成的数据、各类交易数据等，“{platform}数据”、“平台数据、“运营数据”。“{platform}运营数据”的所有权及其他相关权利属于{org}，且是{org}的商业秘密，未经{org}书面授权您不得使用。但依法属于用户、课程发布者享有相关权利的数据或课程内容等除外。&nbsp;<br />
<br />
2.课程发布者的权利和义务&nbsp;<br />
<br />
2.1 帐户注册&nbsp;<br />
<br />
2.1.1 您应当通过登录{platform}网站或{org}其他指定途径，注册课程发布者帐户（下简称：账户）以成为课程发布者，课程发布者帐户一经注册成功，相应的账号不得变更，且该帐户不可转让、不可赠予、不可继承等。&nbsp;<br />
<br />
2.1.2您注册帐号时，应使用您拥有合法使用权的手机号码。&nbsp;<br />
<br />
2.1.3 您不得违反本协议约定将您的账户用于其他目的。否则，{org}有权随时单方限制、中止或终止向您提供本服务，且未经{org}同意您不得再次使用本服务。&nbsp;<br />
<br />
2.1.4 您注册帐号使用的手机号，是您登录及使用本服务的凭证。您应当做好手机号、密码，以及进入和管理本服务中的各类产品与服务的口令、密码等的保密措施。因您保密措施不当或您的其他行为，致使上述口令、密码等丢失或泄漏所引起的一切损失和后果，均由您自行承担。&nbsp;<br />
<br />
2.1.5 {org}可能会向您提供添加其他QQ账号以成为您注册账号的账号管理员的功能，如{org}向您提供账号管理员功能后，您可以根据自己的需求，在使用相应手机号注册成您账号的账号管理员后，进行您指定或授权的操作。&nbsp;<br />
<br />
2.1.6 您保证：您注册本服务账户的手机号及您添加的账号管理员使用的手机号的使用权均是合法获取的。前述全部手机号在本服务中进行的包括但不限于以下事项：注册本服务帐户、提交相应资质材料、确认和同意相关协议和规则、选择具体服务类别以及进行费用结算等事项，均是您自行或您授权他人进行的行为，对您均有约束力。同时，您承担以前述全部手机号为标识进行的全部行为的法律责任。&nbsp;<br />
<br />
2.1.7 若您发现有他人冒用或盗用您的账户及密码、或任何其他未经您合法授权的情形时，您应立即以有效方式通知{org}并提供{org}所需的相关材料（包括但不限于提供您的身份信息和相关身份资料、相关事实情况及您的要求等）。{org}收到您的有效请求并核实身份后，会根据不同情况采取相应措施。若您提供的信息不完全，导致{org}无法核实您的身份或{org}无法判断您的需求等，而导致{org}无法进行及时处理，给您带来的损失，您应自行承担。同时，{org}对您的请求采取措施需要合理期限，对于您通知{org}以及{org}根据您的有效通知采取措施之前，由于他人行为给您造成的损失，{org}不承担任何责任。&nbsp;<br />
<br />
2.2 资质材料&nbsp;<br />
<br />
2.2.1 您保证：您具备使用本服务、接入和运营课程或提供相关服务等行为的相关合法资质或已经过了相关政府部门的审核批准；您提供的主体资质材料、相关资质或证明以及其他任何文件等信息真实、准确、完整，并在信息发生变更后，及时进行更新；您具备履行本协议项下之义务、各种行为的能力；您履行相关义务、从事相关行为不违反任何对您的有约束力的法律文件。否则，您应不使用{org}提供的相关服务，且应独自承担由此带来的一切责任及给用户、{org}造成的全部损失。&nbsp;<br />
<br />
2.2.2 您保证：您会依法及按照{org}要求提交使用本服务所必须的真实、准确的经过您签章确认的主体资质材料以及联系人姓名（名称）、地址、电子邮箱等相关资料。&nbsp;<br />
<br />
2.2.3 您保证：您在{platform}上通过您的课程提供的各种服务，依法已经具有相关的合法资质或获得了有关部门的许可或批准，并会向{org}提交相关资质或证明文件。&nbsp;<br />
<br />
2.2.4 您保证：您在{platform}上通过您的课程提供的各种服务，符合国家相关法规的规定，不违反任何相关法规及相关协议、规则，也不会侵犯任何人的合法权益，同时，会依法、依约或按照{org}的要求提供版权、专利权等相关证明文件。&nbsp;<br />
<br />
2.3 服务费用&nbsp;<br />
<br />
2.3.1 目前，{org}向您提供{platform}服务是免费的，如后续{org}可能会对服务进行调整，并有权根据运营情况单独决定是否向您收取相应服务费用。&nbsp;<br />
<br />
2.3.2 如后续{org}向您收取相应服务费用的，您因使用{org}提供的相关服务而产生的所有服务费用由您自行承担，您应按相关协议、规则等的规定支付费用，否则，{org}有权不提供相关服务。您选择使用相关服务并支付费用后，在服务未到期之前，若您单方要求提前解除服务的，{org}有权将您未使用的服务对应的费用不予退还而作为您单方违约的违约金予以没收。&nbsp;<br />
<br />
在本服务中，{org}如需对服务收取相应服务费用的，{org}有权单方根据实际需要对收费服务的收费标准、方式进行修改和变更，前述修改、变更前，{org}将在相应服务页面进行通知或公告。如果您不同意上述修改、变更，则应立即停止使用相应服务，否则，您的任何使用行为，即视为您同意上述修改、变更。&nbsp;<br />
<br />
您若对{org}提供的收费标准、服务使用期限等费用结算事项的通知内容有异议的，应在收到{org}通知后及时以书面形式告知{org}，{org}收到您的书面告知会进行核实，否则，视为您认可{org}提供的费用结算事项的通知内容，双方应按照{org}提供的费用结算事项的通知内容，进行费用的结算及支付等。&nbsp;<br />
<br />
您理解并同意：若{org}按照前述约定对于本服务是否收费、收费标准等单方做出调整、变化的，您会予以全部遵守、同意，但您依法或按照约定终止使用本服务的除外。&nbsp;<br />
<br />
2.4 课程要求&nbsp;<br />
<br />
2.4.1 您应自行负责您课程的开发、创建、上课、管理、运营等工作，并且自行承担相应的费用。&nbsp;<br />
<br />
2.4.2 您的课程，应符合相关法规、技术规范或标准等，同时，还应符合{platform}的对接入课程在内容、服务等方面的统一要求和您在课程介绍页面对课程的介绍，以确保课程可以在{platform}真实、安全、稳定的运营。&nbsp;<br />
<br />
2.4.3 您不得在课程内宣传与本课程无关的任何其他信息，包括但不限于广告、其他的课程产品信息等（除非双方另有约定），也不得在课程内添加指向非{org}拥有或控制或{org}书面同意的网站链接。&nbsp;<br />
<br />
2.4.4 您课程在{platform}上运营期间，您需向用户提供及时有效的客户服务，客户服务形式包括但不限于通过明确且合理的方式告知用户客户服务渠道、提供QQ/电话等，并自行承担客服费用。&nbsp;<br />
<br />
2.4.5 您应当在课程中向相关权利人提供投诉途径，确保权利人在认为您侵犯其合法权益时可以向您主张权利。&nbsp;<br />
<br />
2.4.6 如果您的课程符合{platform}支付接入的相关要求时，则可依相关规范、流程等接入到{org}的支付系统。&nbsp;<br />
<br />
2.5 课程运营&nbsp;<br />
<br />
2.5.1 您应自行按照相关法规，运营您的课程，履行相关义务，并自行承担全部责任，包括但不限于：&nbsp;<br />
<br />
（1）依照相关法律法规的规定，保留相应的访问、使用等日志记录；&nbsp;<br />
<br />
（2）国家有权机关向您依法查询相关信息时，应积极配合提供；&nbsp;<br />
<br />
（3）主动履行其他您依法应履行的义务。&nbsp;<br />
<br />
2.5.2 您保证：&nbsp;<br />
<br />
（1）您的课程、提供给用户的相关服务及发布的相关信息、内容等，不违反相关法律、法规、政策等的规定及本协议或相关协议、规则等，也不会侵犯任何人的合法权益；&nbsp;<br />
<br />
（2）课程上课过程中应尊重用户知情权、选择权，应当坚持诚信原则，不误导、欺诈、混淆用户，尊重用户的隐私，不骚扰用户，不制造垃圾信息。&nbsp;<br />
<br />
（3）如您的股东或高级管理人员（包括但不限于董事长、总经理、财务总监等）同时也为{org}及{org}关联公司的员工（包括{org}的试用期员工、正式员工、劳务派遣形式的员工及其他受{org}及{org}关联公司管理的其他性质的员工或人员）时，您应当在注册时主动书面告知{org}。&nbsp;<br />
<br />
2.5.3 您不得从事任何包括但不限于以下的违反法规的行为，也不得为以下违反法规的行为提供便利（包括但不限于为您课程的用户的行为提供便利等）：&nbsp;<br />
<br />
（1）反对宪法所确定的基本原则的行为；&nbsp;<br />
<br />
（2）危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的行为；&nbsp;<br />
<br />
（3）损害国家荣誉和利益的行为；&nbsp;<br />
<br />
（4）煽动民族仇恨、民族歧视，破坏民族团结的行为；&nbsp;<br />
<br />
（5）破坏国家宗教政策，宣扬邪教和封建迷信的行为；&nbsp;<br />
<br />
（6）散布谣言，扰乱社会秩序，破坏社会稳定的行为；&nbsp;<br />
<br />
（7）散布淫秽、色情、赌博、暴力、凶杀、恐怖或者教唆犯罪的行为；&nbsp;<br />
<br />
（8）侮辱或者诽谤他人，侵害他人合法权益的行为；&nbsp;<br />
<br />
（9）侵害他人知识产权、商业秘密等合法权利的行为；&nbsp;<br />
<br />
（10）恶意虚构事实、隐瞒真相以误导、欺骗他人的行为；&nbsp;<br />
<br />
（11）发布、传送、传播广告信息及垃圾信息；（12）其他法律法规禁止的行为。&nbsp;<br />
<br />
2.5.4 您不得从事包括但不限于以下行为，也不得为以下行为提供便利（包括但不限于为您的用户的行为提供便利等）：&nbsp;<br />
<br />
（1）删除、隐匿、改变{platform}显示或其中包含的任何专利、著作权、商标或其他所有权声明；&nbsp;<br />
<br />
（2）以任何方式干扰或企图干扰{org}任何产品、任何部分或功能的正常运行，或者制作、发布、传播上述工具、方法等；&nbsp;<br />
<br />
（3）避开、尝试避开或声称能够避开任何内容保护机制，或导致用户认为其直接与{org}{platform}及{org}相关产品进行交互；&nbsp;<br />
<br />
（4）在未获得{org}书面许可的情况下，以任何方式使用{org}URL地址、技术接口等；&nbsp;<br />
<br />
（5）在未经过用户同意的情况下，向任何其他用户及他方显示或以其他任何方式提供该用户的任何信息；&nbsp;<br />
<br />
（6）请求、收集、索取或以其他方式获取用户QQ、{org}朋友或QQ空间等{org}服务的登录帐号、密码或其他任何身份验证凭据；&nbsp;<br />
<br />
（7）在没有获得用户明示同意的情况下，直接联系用户，或向用户发布任何商业广告及骚扰信息；&nbsp;<br />
<br />
（8）为任何用户自动登录到{org}{platform}提供代理身份验证凭据；&nbsp;<br />
<br />
（9）提供跟踪功能，包括但不限于识别其他用户在个人主页上查看、点击等操作行为；&nbsp;<br />
<br />
（10）自动将浏览器窗口定向到其他网页；&nbsp;<br />
<br />
（11）未经授权获取对{org}产品或服务的访问权；&nbsp;<br />
<br />
（12）课程服务内容中含有计算机病毒、木马或其他恶意程序等任何可能危害{org}或用户权益和终端信息安全等的内容；&nbsp;<br />
<br />
（13）设置或发布任何违反相关法规、公序良俗、社会公德等的玩法、内容等；&nbsp;<br />
<br />
（14）公开表达或暗示，您与{org}之间存在合作关系，包括但不限于相互持股、商业往来或合作关系等，或声称{org}对您的认可；&nbsp;<br />
<br />
（15）未经{org}许可，实施包括但不限于以赠送、发售等方式使用{org}或第三方的任何虚拟货币或品牌服务（例如Q币、Q点、黄钻等）的行为；&nbsp;<br />
<br />
（16）其他{org}认为不应该、不适当的行为、内容。&nbsp;<br />
<br />
2.5.5 本服务中可能会使用第三方软件或技术，若有使用，前述第三方软件或技术相关协议或其他文件，均是本协议不可分割的组成部分，与本协议具有同等的法律效力，您应当遵守这些要求。否则，因此带来的一切责任您应自行承担。如因本服务使用的第三方软件或技术引发的任何纠纷，由该第三方负责解决。&nbsp;<br />
<br />
2.6 课程的下线规则&nbsp;<br />
<br />
2.6.1 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排履行完毕的，或课程有用户已经报名但未学习完的，在未经已报名但未学习完用户和{org}都同意的前提下，您不得擅自终止课程的运营或服务的提供。&nbsp;<br />
<br />
2.6.2 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排，或课程有用户已经报名但仍未学习完的，若您确须要提前终止课程运营或服务提供，您应至少提前通知用户和{org}，同时，您在取得已报名但未学习完用户同意并妥善处理相关退费或各类用户损失等事宜后，并经{org}同意后，方可终止相关课程的运营或服务的提供。&nbsp;<br />
<br />
2.6.3 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排，或课程有用户已经报名但仍未学习完的，若您发布的课程由于违反相关法规或本协议相关约定时，您应该按照{org}的要求终止课程的运营或{org}有权随时终止课程的运营或服务的提供，同时，您应妥善处理已报名但未学习完用户的退费、各类用户损失等问题，若由此造成用户、{org}损失或有纠纷的，您应负责解决相关纠纷并自行承担全部责任。&nbsp;<br />
<br />
2.6.4 若课程因为任何原因需要下线的，在课程下线过程中，您应积极与用用户联系并妥善处理已报名但未学习完用户的退费、各类用户损失等事宜，依法保护用户的合法权益不受损害，否则，若由此造成用户、{org}损失或有纠纷的，您应负责解决相关纠纷并自行承担全部责任。同时，您应发布下线公告直至课程下线（下线公告应当包含但不限于下线时间、退费方式、用户补偿方案、客服联系电话等内容），且您不得强制用户切换到其他第三方平台进行学习，亦不得免除自身对未结束课程进行退款责任等义务。如您选择退还用户已支付的课程相应费用、补偿用户损失的，您同意{org}在您的账户结算额中直接扣除前述退还给用户的课程相应费用、补偿用户损失费用等全部费用。&nbsp;<br />
<br />
2.6.5 您同意，无论课程因为任何原因需要下线的，{org}有权单方冻结尚未支付给您的所有款项，并从前述冻结款项中直接扣除应当退还给用户的费用、补偿用户损失费用等，其他条款与该条有不一致的，以该条为准。&nbsp;<br />
<br />
2.7 关于用户数据的规则&nbsp;<br />
<br />
2.7.1 您的课程或服务对于用户数据的收集、保存、使用等必须满足以下要求：（1）您的课程或服务需要收集用户任何数据的，必须事先获得用户的明确同意，且仅应当收集为课程运行而必要的用户数据，同时应当告知用户相关数据收集的目的、范围及使用方式等，保障用户知情权；（2）您收集用户的数据后，必须采取必要的保护措施，防止用户数据被盗、泄漏等；（3）您在特定课程中收集的用户数据仅可以在该特定课程中使用，不得将其使用在该特定课程之外或为其他任何目的进行使用，也不得以任何方式将其提供给他人；（4）您应向用户提供隐私保护政策。隐私保护政策须在课程界面上明显位置向用户展示。&nbsp;<br />
<br />
2.7.2 您不得收集用户的隐私信息数据及其他{org}认为属于敏感信息范畴的数据，包括但不限于不得收集或要求用户提供任何手机号、QQ密码、用户关系链、好友列表数据、银行账号和密码等。&nbsp;<br />
<br />
2.7.3 您不得使用{platform}数据用于向用户进行广告宣传使用。&nbsp;<br />
<br />
2.7.4 如果{org}认为您收集、使用用户数据的方式，可能损害用户体验，{org}有权要求您删除相关数据并不得再以该方式收集、使用用户数据。&nbsp;<br />
<br />
2.7.5 {org}有权限制或阻止您获取用户数据及{platform}数据。&nbsp;<br />
<br />
2.7.6 未经{org}事先书面同意，您不得为本协议约定之外的目的使用用户数据或平台运营数据，亦不得以任何形式将前述数据提供给他人。&nbsp;<br />
<br />
2.7.7 一旦课程发布者停止使用{platform}，或{org}基于任何原因终止您使用本服务，您必须立即删除全部从{platform}中获得的数据（包括各种备份），且不得再以任何方式进行使用。&nbsp;<br />
<br />
2.7.8 您应自行对因使用本服务而存储在{org}服务器的各类数据等信息，在本服务之外，采取合理、安全的技术措施，确保其安全性，并对自己的行为（包括但不限于自行安装软件、采取加密措施或进行其他安全措施等）所引起的结果承担全部责任。<br />
<br />
2.8 法律责任&nbsp;<br />
<br />
2.8.1 您保证：如果您使用本服务产生相应费用的，您会按照本协议及相关协议、规则等支付相关费用。否则，您理解并同意：每延期一日，您应当向{org}支付所欠费用千分之一的违约金。同时，{org}有权随时单方采取包括但不限于以下措施中的一种或多种，以维护自己的合法权益。&nbsp;<br />
<br />
（1）从其他{org}应支付给您或您关联公司的任何费用中直接抵扣；&nbsp;<br />
<br />
（2）暂停向您或您关联公司结算或支付任何费用；&nbsp;<br />
<br />
（3）中止、终止您或您关联公司使用本服务；&nbsp;<br />
<br />
（4）中止、终止您或您关联公司的后台管理权限；&nbsp;<br />
<br />
（5）删除您或您关联公司在使用本服务中存储的任何数据；&nbsp;<br />
<br />
（6）禁止您今后将您的任何新课程接入{platform}；&nbsp;<br />
<br />
（7）其他{org}可以采取的为维护自己权益的措施。&nbsp;<br />
<br />
2.8.2 您保证：您使用本服务及您的任何行为，不违反任何相关法规、本协议和相关协议、规则等。您理解并同意：若{org}自行发现或根据相关部门的信息、权利人的投诉等发现您可能存在违反前述保证情形的，{org}有权根据一般人的认识自己独立判断，以认定您是否存在违反前述保证情形，若{org}经过判断认为您存在违反前述保证情形的，{org}有权随时单方采取以下一项或多项措施。&nbsp;<br />
<br />
（1）要求您立即更换、修改违反前述保证情形的相关内容；&nbsp;<br />
<br />
（2）对存在违反前述保证情形的课程、您或您关联公司名下的全部课程或任何一款课程采取下线措施即终止课程在{platform}的运营；&nbsp;<br />
<br />
（3）禁止您或您关联公司今后将任何新课程接入{platform}；&nbsp;<br />
<br />
（4）中止、终止向违反前述保证情形的课程、您或您关联公司名下的全部课程或任何一款课程，或您或您关联公司提供部分或全部本服务；&nbsp;<br />
<br />
（5）冻结{org}尚未支付给您的所有款项,并将前述冻结款项予以没收，作为您向{org}承担违约责任的违约金而不再向您支付；&nbsp;<br />
<br />
（6）将您的行为对外予以公告；&nbsp;<br />
<br />
（7）其他{org}认为适合的处理措施。&nbsp;<br />
<br />
2.8.3 如在{org}告知您或您自行得知您存在任何违法情形后，您应按法律法规或{org}的规定向{org}提出反通知。但是，无论{org}是否告知您、您是否提出反通知或反通知是否符合相关法规、{org}要求等，均不影响{org}进行自己的独立判断和采取相关措施。&nbsp;<br />
<br />
2.8.4 若{org}按照上述条款、本协议的其他相关约定或因您违反相关法律的规定，对您或您的课程采取任何行为或措施，所引起的纠纷、责任等一概由您自行负责，造成您损失的，您应自行全部承担，造成{org}或他人损失的，您也应自行承担全部责任。&nbsp;<br />
<br />
2.8.5 若{org}按照上述条款、本协议的其他相关约定或因您违反相关法律的规定，对您或您的课程采取任何行为或措施后，导致您没有使用到相应服务但已经缴纳相应费用的，对该部分费用，{org}有权不予退还，而作为您违反约定的违约金予以没收。&nbsp;<br />
<br />
3.{org}的权利义务&nbsp;<br />
<br />
3.1 {org}会根据您选择的服务向您提供相应的服务，如后续{org}提供服务需要收费的，您应向{org}支付相应费用，{org}在您支付相应费用后，会向您提供相应的服务。&nbsp;<br />
<br />
3.2 保护您的信息的安全是{org}的一项基本原则，未经您的同意，{org}不会向{org}以外的任何公司、组织和个人披露、提供您的信息，但下列情形除外：&nbsp;<br />
<br />
（1）据本协议或其他相关协议、规则等规定可以提供的；&nbsp;<br />
<br />
（2）依据法律法规的规定可以提供的；&nbsp;<br />
<br />
（3）行政、司法等政府部门要求提供的；&nbsp;<br />
<br />
（4）您同意{org}向第三方提供；&nbsp;<br />
<br />
（5）为解决举报事件、提起诉讼而需要提供的；&nbsp;<br />
<br />
（6）为防止严重违法行为或涉嫌犯罪行为发生而采取必要合理行动所必须提供的。&nbsp;<br />
<br />
3.3 尽管{org}对您的信息保护做了极大的努力，但是仍然不能保证在现有的安全技术措施下，您的信息可能会因为不可抗力或非{org}因素造成泄漏、窃取等，由此给您造成损失的，您同意{org}可以免责。&nbsp;<br />
<br />
3.4 {org}有权开发、运营与您课程相似或相竞争的课程，同时{org}也不保证平台上不会出现其他课程发布者提供的与您课程相竞争的课程。&nbsp;<br />
<br />
3.5 {org}有权在包括但不限于课程介绍页等，向用户阐述该课程为您开发以及由您向用户提供客户服务等。&nbsp;<br />
<br />
3.6 {org}可将本协议下的权利和义务的部分或全部转让给他人，如果您不同意{org}的该转让，则有权停止使用本协议下服务。否则，视为您对此予以接受。&nbsp;<br />
<br />
3.7 除了另行有约定外，{org}无需为按照本协议享有的权益而向您支付任何费用。&nbsp;<br />
<br />
3.8 您理解并同意：为向更多互联网使用者推广您的课程，{org}有权采取以下行为，而无须再取得您的同意。&nbsp;<br />
<br />
（1）在{org}{platform}以外的平台、网站等采取各种形式对课程进行宣传、推广；&nbsp;<br />
<br />
（2）{org}可根据整体运营安排，自主选择向整个或部分全世界范围内的互联网用户提供您的课程；&nbsp;<br />
<br />
（3）有权为本协议目的使用您课程的LOGO、标识、名称、图片等相关素材。&nbsp;<br />
<br />
4.广点通服务&nbsp;<br />
<br />
4.1 广点通服务，指{org}向您提供的，供您将您所接入的课程或您提供的服务在QQ空间、朋友及其他{org}平台上进行推广的服务，是{org}向您提供的本服务的一部分，是否使用该服务由您自行选择。该服务的相关协议、规则、公告、提示等，请见相应页面，您若选择使用该服务的，应按照广点通的要求进行开通，并遵守前述相关协议、规则、公告、提示等。&nbsp;<br />
<br />
4.2 您以任何形式登录、使用广点通服务，即表示您已理解并接受广点通服务的相关协议、规则、公告、提示等的约束。&nbsp;<br />
<br />
4.3 您同意{org}有权无须经您同意或提前通知而直接对广点通服务的相关协议、规则等进行修改，修改后的内容一旦在网页上公布即有效代替原来的条款，对您产生约束力。&nbsp;<br />
<br />
5.关于免责&nbsp;<br />
<br />
5.1 您理解并同意：鉴于网络服务的特殊性，{org}有权在无需通知您的情况下根据{org}{platform}的整体运营情况或相关运营规范、规则等，可以随时变更、中止或终止部分或全部的服务，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.2 您理解并同意：为了向您提供更完善的服务，{org}有权定期或不定期地对提供本服务的平台或相关设备进行检修、维护、升级等，此类情况可能会造成相关服务在合理时间内中断或暂停的，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.3 您理解并同意：{org}的服务是按照现有技术和条件所能达到的现状提供的。{org}会尽最大努力向您提供服务，确保服务的连贯性和安全性；但{org}不能保证其所提供的服务毫无瑕疵，也无法随时预见和防范法律、技术以及其他风险，包括但不限于不可抗力、病毒、木马、黑客攻击、系统不稳定、第三方服务瑕疵、政府行为等原因可能导致的服务中断、数据丢失以及其他的损失和风险。所以您也同意：即使{org}提供的服务存在瑕疵，但上述瑕疵是当时行业技术水平所无法避免的，其将不被视为{org}违约，同时，由此给您造成的数据或信息丢失等损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.4 您理解并同意：在使用本服务的过程中，可能会遇到不可抗力等风险因素，使本服务发生中断。不可抗力是指不能预见、不能克服并不能避免且对一方或双方造成重大影响的客观事件，包括但不限于自然灾害如洪水、地震、瘟疫流行和风暴等以及社会事件如战争、动乱、政府行为等。出现上述情况时，{org}将努力在第一时间与相关单位配合，及时进行修复，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.5 您理解并同意：若由于对以下情形导致的服务中断或受阻，给您造成损失的，您同意放弃追究{org}的责任：&nbsp;<br />
<br />
（1）受到计算机病毒、木马或其他恶意程序、黑客攻击的破坏；&nbsp;<br />
<br />
（2）您或{org}的电脑软件、系统、硬件和通信线路出现故障；&nbsp;<br />
<br />
（3）您操作不当；&nbsp;<br />
<br />
（4）您通过非{org}授权的方式使用本服务；&nbsp;<br />
<br />
（5）其他{org}无法控制或合理预见的情形。&nbsp;<br />
6.服务的中止或终止&nbsp;<br />
<br />
6.1 如您书面通知{org}不接受本协议或对其的修改，{org}有权随时中止或终止向您提供本服务。&nbsp;<br />
<br />
6.2 因不可抗力因素导致您无法继续使用本服务或{org}无法提供本服务的，{org}有权随时终止协议。&nbsp;<br />
<br />
6.3 本协议约定的其他中止或终止条件发生或实现的，{org}有权随时中止或终止向您提供本服务。&nbsp;<br />
<br />
6.4 由于您违反本协议约定，{org}依约终止向您提供本服务后，如您后续再直接或间接，或以他人名义注册使用本服务的，{org}有权直接单方面暂停或终止提供本服务。&nbsp;<br />
<br />
6.5 如本协议或本服务因为任何原因终止的，对于您的帐号中的全部数据或您因使用本服务而存储在{org}服务器中的数据等任何信息，{org}可将该等信息保留或删除，包括服务终止前您尚未完成的任何数据。&nbsp;<br />
<br />
6.6 如本协议或本服务因为任何原因终止的，您应自行处理好关于数据等信息的备份以及与您的用户之间的相关事项的处理等，由此造成{org}损失的，您应负责赔偿。&nbsp;<br />
<br />
7.关于通知&nbsp;<br />
<br />
7.1 {org}可能会以网页公告、网页提示、电子邮箱、手机短信、常规的信件传送、您注册的本服务账户的管理系统内发送站内信等方式中的一种或多种，向您送达关于本服务的各种规则、通知、提示等信息，该等信息一经{org}采取前述任何一种方式公布或发送，即视为您已经接受并同意，对您产生约束力。若您不接受的，请您书面通知{org}并停止使用本服务，否则视为您已经接受、同意。&nbsp;<br />
<br />
7.2 若由于您提供的电子邮箱、手机号码、通讯地址等信息错误，导致您未收到相关规则、通知、提示等信息的，您同意仍然视为您已经收到相关信息并受其约束，一切后果及责任由您自行承担。&nbsp;<br />
<br />
7.3 您也同意{org}或合作伙伴可以向您的电子邮件、手机号码等发送可能与本服务不相关的其他各类信息包括但不限于商业广告等。&nbsp;<br />
<br />
7.4 若您有事项需要通知{org}的，应当按照本服务对外正式公布的联系方式书面通知{org}。&nbsp;<br />
<br />
8.知识产权&nbsp;<br />
<br />
8.1 {org}在本服务中提供的信息内容（包括但不限于网页、文字、图片、音频、视频、图表等）的知识产权均归{org}所有，依法属于用户、课程发布者及其他第三方所有的除外。除另有特别声明外，{org}提供本服务时所依托软件的著作权、专利权及其他知识产权均归{org}所有。上述及其他任何{org}依法拥有的知识产权均受到法律保护，未经{org}书面许可，您不得以任何形式进行使用或创造相关衍生作品。&nbsp;<br />
<br />
8.2 您仅拥有依照本协议约定合法使用本服务或相关API的权利，与本服务相关的API相关的著作权、专利权等相关全部权利归{org}所有。未经{org}书面许可，您不得违约或违法使用，不得向任何单位或个人出售、转让、转授权{org}的代码、API及开发工具等。&nbsp;<br />
<br />
9.其他&nbsp;<br />
<br />
9.1 若您和{org}之间发生任何纠纷或争议，首先应友好协商解决；协商不成功的，双方均同意将纠纷或争议提交本协议签订地有管辖权的人民法院解决。&nbsp;<br />
<br />
9.2 本协议所有条款的标题仅为阅读方便，本身并无实际涵义，不能作为本协议涵义解释的依据。&nbsp;<br />
<br />', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (5, N'NewsSourceItem', N'新浪,网易,腾讯', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (7, N'SysIsLoginLogs', N'False', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (8, N'SysIsWorkLogs', N'False', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (9, N'SystemName', N'在线学习云平台', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (10, N'SysLoginTimeSpan', N'10', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (12, N'IsWebsiteSatatic', N'True', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (206, N'LoginPoint', N'10', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (30, N'IsAllowMobile', N'True', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (31, N'IsAllowMobileVerifyCode', N'False', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (207, N'LoginPointMax', N'30', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (208, N'SharePoint', N'3', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (197, N'SmsCurrent', N'河南腾信', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (209, N'SharePointMax', N'12', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (61, N'QrColor', N'#f16522', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (217, N'RegFirst', N'1000', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (193, N'flowNumber', N'222', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (194, N'SubjectForAccout_1', N'8,', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (196, N'SysWorkTimeSpan', N'10', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (210, N'RegPoint', N'100', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (211, N'RegPointMax', N'500', NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SystemPara] ([Sys_Id], [Sys_Key], [Sys_Value], [Sys_Default], [Sys_ParaIntro], [Sys_Unit], [Sys_SelectUnit], [Org_Id], [Org_Name]) VALUES (212, N'PointConvert', N'1000', NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SystemPara] OFF
