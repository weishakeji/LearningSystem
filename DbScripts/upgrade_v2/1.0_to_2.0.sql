--2021-01-05

/*增加登录校验证的字段*/
alter table EmpAccount add Acc_CheckUID nvarchar(255)
go
/*更改菜单状态名称*/
execute sp_rename 'ManageMenu.MM_State','MM_IsChilds'
go
/*增加菜单标识*/
alter table ManageMenu add MM_UID   nvarchar(255)
go
UPDATE ManageMenu SET MM_UID  = MM_Id where MM_UID  is null
go
alter table Purview add MM_UID   nvarchar(255)
go
/*菜单的帮助项*/
alter table ManageMenu add MM_Help   nvarchar(1000)
go
alter table ManageMenu add MM_Complete  bit NULL
go
UPDATE ManageMenu SET MM_Complete  = 0
GO
alter table ManageMenu ALTER COLUMN MM_Complete  bit NOT NULL
go
alter table ManageMenu ALTER COLUMN  MM_PatId   nvarchar(255)
/*菜单项为窗体时，一些窗体的设置项*/
alter table ManageMenu add MM_WinMin  bit NULL
go
UPDATE ManageMenu SET MM_WinMin  = 0
GO
alter table ManageMenu ALTER COLUMN MM_WinMin  bit NOT NULL
go
alter table ManageMenu add MM_WinMax  bit NULL
go
UPDATE ManageMenu SET MM_WinMax  = 0
GO
alter table ManageMenu ALTER COLUMN MM_WinMax  bit NOT NULL
go
alter table ManageMenu add MM_WinMove  bit NULL
go
UPDATE ManageMenu SET MM_WinMove  = 0
GO
alter table ManageMenu ALTER COLUMN MM_WinMove  bit NOT NULL
go
alter table ManageMenu add MM_WinResize bit NULL
go
UPDATE ManageMenu SET MM_WinResize  = 0
GO
alter table ManageMenu ALTER COLUMN MM_WinResize  bit NOT NULL
go
alter table ManageMenu  add MM_WinID nvarchar(255)
alter table ManageMenu add MM_AbbrName   nvarchar(255)


alter table ManageMenu add MM_IsFixed  bit NULL
go
UPDATE ManageMenu SET MM_IsFixed  = 1 where MM_PatId='0'
go
UPDATE ManageMenu SET MM_IsFixed  = 0 where MM_PatId!='0'
GO
alter table ManageMenu ALTER COLUMN MM_IsFixed  bit NOT NULL

/*导航菜单，增加uid，修改pid*/
alter table Navigation add Nav_UID   nvarchar(255)
go
UPDATE Navigation SET Nav_UID  = Nav_ID where Nav_UID  is null
go
alter  table Navigation ALTER COLUMN Nav_PID  nvarchar(255)
go
UPDATE Navigation SET Nav_PID  = '' where Nav_PID='0'
go
alter table Navigation add Nav_Icon  nvarchar(50)

--2021-12-9
/*栏目，增加uid，修改pid为字符型，增加Col_IsChildren*/
alter table [Columns] add Col_UID   nvarchar(255)
go
UPDATE [Columns]  SET Col_UID  = [Col_ID] where Col_UID  is null
go
alter  table [Columns] ALTER COLUMN Col_PID  nvarchar(255)
go
UPDATE [Columns] SET Col_PID  = '' where Col_PID='0'
go
alter table [Columns] add Col_IsChildren  bit  NULL
go
UPDATE [Columns] SET Col_IsChildren  = 1
/*在文章表中，增加与栏目UID的关联，之前是ID关联*/
alter table Article add Col_UID   nvarchar(255)
go
UPDATE Article  SET Col_UID  = [Col_ID]
go
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Article__Col_Id__56B3DD81]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Article] DROP CONSTRAINT [DF__Article__Col_Id__56B3DD81]
END
go
alter table Article drop column Col_Id

/*2022-2-5 课程公告的分类，增加UID*/
alter table GuideColumns add Gc_UID   nvarchar(255)
go
UPDATE GuideColumns  SET Gc_UID  = [Gc_ID] where Gc_UID  is null
go
alter  table GuideColumns ALTER COLUMN Gc_PID  nvarchar(255)
go
--UPDATE GuideColumns SET Gc_PID  = '' where Gc_PID='0'
go
alter table Guide add Gc_UID   nvarchar(255)
go
UPDATE Guide  SET Gc_UID  = [Gc_ID] where Gc_UID  is null
go
alter table Guide drop column [Gc_ID]
go
/*2022-2-16 课程知识库，增加UID*/
alter table KnowledgeSort add Kns_UID   nvarchar(255)
go
UPDATE KnowledgeSort  SET Kns_UID  = Kns_ID where Kns_UID  is null
go
alter  table KnowledgeSort ALTER COLUMN Kns_PID  nvarchar(255)
go
--UPDATE KnowledgeSort SET Kns_PID  = '' where Kns_PID='0'
go
alter table Knowledge add Kns_UID   nvarchar(255)
go
UPDATE Knowledge  SET Kns_UID  = [Kn_ID] where Kns_UID  is null
go
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Knowledge__Kns_I__17C286CF]') AND type = 'D')
BEGIN
ALTER TABLE [Knowledge] DROP CONSTRAINT [DF__Knowledge__Kns_I__17C286CF]
END
go
alter table Knowledge drop column Kns_ID
/*2022-3-11 试题中增加知识点关联的Uid，删除kn_id*/
alter table Questions add Kn_Uid   nvarchar(255)
go
UPDATE Questions  SET Kn_Uid  = Kn_ID where Kn_Uid  is null
go
alter table Questions drop column Kn_ID

/*在试卷中增加结课考试的字段*/
alter table TestPaper add Tp_IsFinal     bit NULL
go
UPDATE TestPaper  SET Tp_IsFinal = 0
go
alter  table TestPaper ALTER COLUMN Tp_IsFinal   bit NOT NULL


/*增加学员试题练习的记录**/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LogForStudentExercise](
	[Lse_ID] [int] IDENTITY(1,1) NOT NULL,
	[Org_ID] [int] NOT NULL,
	[Ac_ID] [int] NOT NULL,
	[Ac_AccName] [nvarchar](50) NULL,
	[Ac_Name] [nvarchar](50) NULL,
	[Cou_ID] [int] NOT NULL,
	[Ol_ID] [int] NOT NULL,
	[Lse_UID] [nvarchar](255) NULL,
	[Lse_CrtTime] [datetime] NOT NULL,
	[Lse_LastTime] [datetime] NOT NULL,
	[Lse_JsonData] [nvarchar](max) NULL,
	[Lse_Sum] [int] NOT NULL,
	[Lse_Answer] [int] NOT NULL,
	[Lse_Correct] [int] NOT NULL,
	[Lse_Wrong] [int] NOT NULL,
	[Lse_Rate] [decimal](18, 12) NOT NULL,
	[Lse_IP] [nvarchar](50) NULL,
	[Lse_Browser] [nvarchar](255) NULL,
	[Lse_Platform] [nvarchar](255) NULL,
	[Lse_OS] [nvarchar](255) NULL,
 CONSTRAINT [PK_LogForStudentExercise] PRIMARY KEY CLUSTERED 
(
	[Lse_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/* 将试题选项的id设置为长整型*/
go
ALTER TABLE QuesAnswer DROP CONSTRAINT aaaaaQuesAnswer_PK;
go
alter  table QuesAnswer ALTER COLUMN Ans_ID   bigint NOT NULL
go

/*支付接口的参数修正*/
update PayInterface set Pai_InterfaceType='alipayweb' where Pai_Pattern='支付宝网页直付'
go
update PayInterface set Pai_InterfaceType='alipaywap' where Pai_Pattern='支付宝手机支付'
go
update PayInterface set Pai_InterfaceType='weixinpubpay' where Pai_Pattern='微信公众号支付'
go
update PayInterface set Pai_InterfaceType='WeixinAppPay' where Pai_Pattern='微信小程序支付'
go
update PayInterface set Pai_InterfaceType='WeixinH5Pay' where Pai_Pattern='微信Html5支付'
go
update PayInterface set Pai_InterfaceType='weixinnativepay' where Pai_Pattern='微信扫码支付'
go

/*****  2022-8-26 修订学员购买课程的记录*/
/*学员购买记录中，增加禁用启用项*/
alter table Student_Course add Stc_IsEnable  bit NULL
go
UPDATE Student_Course SET Stc_IsEnable = 1
go
alter table Student_Course ALTER COLUMN Stc_IsEnable  bit NOT NULL
go

/*学员购买记录中，增加类型，免费为0，试用为1，购买为2，后台开课为3*/
alter table Student_Course add Stc_Type int NULL
go
UPDATE Student_Course SET Stc_Type = 0 where Stc_IsFree=1
go
UPDATE Student_Course SET Stc_Type = 1 where Stc_IsTry=1
go
UPDATE Student_Course SET Stc_Type = 2 where Stc_IsFree=0 and Stc_IsFree=0
go
alter table Student_Course ALTER COLUMN Stc_Type  int NOT NULL
go
/*学员购买记录中，增加卡券的记录*/
alter table Student_Course add Stc_Coupon  int NULL
go
UPDATE Student_Course SET Stc_Coupon = 0
go
alter table Student_Course ALTER COLUMN Stc_Coupon  int NOT NULL
go