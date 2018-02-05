--2016-08-15

--增加资金流水中的充值时的接口ID
alter table [MoneyAccount] add Pai_ID int  NULL
go
update [MoneyAccount] set Pai_ID=0
go
alter table [MoneyAccount] ALTER COLUMN Pai_ID int  NOT NULL
go
/*增加资金记录中，交易状态字段*/
alter table [MoneyAccount] add Ma_Status [int] NULL
go
update [MoneyAccount] set Ma_Status=0
go
alter table [MoneyAccount] ALTER COLUMN Ma_Status [int] NOT NULL
go
/*增加资金记录中，付款方与收款方的账号信息*/
alter table [MoneyAccount] add Ma_Buyer [nvarchar](255) NULL
go
alter table [MoneyAccount] add Ma_Seller [nvarchar](255) NULL
go
/*增加购买记录中，是否免费的字段*/
alter table [Student_Course] add Stc_IsFree [bit] NULL
go
update [Student_Course] set Stc_IsFree=0
go
alter table [Student_Course] ALTER COLUMN Stc_IsFree [bit] NOT NULL
go
/*增加购买记录中，是否试用的字段*/
alter table [Student_Course] add Stc_IsTry [bit] NULL
go
update [Student_Course] set Stc_IsTry=0
go
alter table [Student_Course] ALTER COLUMN Stc_IsTry [bit] NOT NULL
go


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


/****** Object:  Table [dbo].[ManageMenu]    Script Date: 08/15/2016 22:09:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ManageMenu]') AND type in (N'U'))
DROP TABLE [dbo].[ManageMenu]
GO




IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__Dep_Id__1CA7377D]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__Dep_Id__1CA7377D]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__EGrp_Id__1D9B5BB6]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__EGrp_Id__1D9B5BB6]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__Posi_Id__1E8F7FEF]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__Posi_Id__1E8F7FEF]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Purview__MM_Id__1F83A428]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Purview] DROP CONSTRAINT [DF__Purview__MM_Id__1F83A428]
END

GO

/****** Object:  Table [dbo].[Purview]    Script Date: 08/15/2016 22:10:10 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purview]') AND type in (N'U'))
DROP TABLE [dbo].[Purview]
GO



/****** Object:  Table [dbo].[Purview]    Script Date: 08/15/2016 22:11:17 ******/
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
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4977, NULL, NULL, NULL, 522, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4978, NULL, NULL, NULL, 526, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4979, NULL, NULL, NULL, 537, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4980, NULL, NULL, NULL, 538, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4131, NULL, NULL, 13, 522, N'half', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4132, NULL, NULL, 13, 525, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4981, NULL, NULL, NULL, 536, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4982, NULL, NULL, NULL, 525, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4983, NULL, NULL, NULL, 535, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4133, NULL, NULL, 13, 535, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4134, NULL, NULL, 13, 544, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4135, NULL, NULL, 13, 524, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4984, NULL, NULL, NULL, 544, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4137, NULL, NULL, 13, 545, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4138, NULL, NULL, 13, 610, N'sel', N'posi', NULL, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4985, NULL, NULL, NULL, 621, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4986, NULL, NULL, NULL, 545, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4987, NULL, NULL, NULL, 527, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4988, NULL, NULL, NULL, 539, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4989, NULL, NULL, NULL, 627, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4990, NULL, NULL, NULL, 523, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4991, NULL, NULL, NULL, 623, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4992, NULL, NULL, NULL, 524, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4993, NULL, NULL, NULL, 610, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4994, NULL, NULL, NULL, 609, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4995, NULL, NULL, NULL, 528, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4996, NULL, NULL, NULL, 546, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4997, NULL, NULL, NULL, 630, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4998, NULL, NULL, NULL, 631, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (4999, NULL, NULL, NULL, 632, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5000, NULL, NULL, NULL, 633, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5001, NULL, NULL, NULL, 624, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5002, NULL, NULL, NULL, 607, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5003, NULL, NULL, NULL, 618, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5004, NULL, NULL, NULL, 619, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5005, NULL, NULL, NULL, 565, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5006, NULL, NULL, NULL, 530, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5007, NULL, NULL, NULL, 532, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5008, NULL, NULL, NULL, 566, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5009, NULL, NULL, NULL, 568, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5010, NULL, NULL, NULL, 547, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5011, NULL, NULL, NULL, 548, N'half', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5012, NULL, NULL, NULL, 550, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5013, NULL, NULL, NULL, 552, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5014, NULL, NULL, NULL, 553, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5015, NULL, NULL, NULL, 554, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5016, NULL, NULL, NULL, 628, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5017, NULL, NULL, NULL, 561, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5018, NULL, NULL, NULL, 635, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5019, NULL, NULL, NULL, 636, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5020, NULL, NULL, NULL, 562, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5021, NULL, NULL, NULL, 563, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5022, NULL, NULL, NULL, 564, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5023, NULL, NULL, NULL, 569, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5024, NULL, NULL, NULL, 570, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5025, NULL, NULL, NULL, 572, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5026, NULL, NULL, NULL, 622, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5027, NULL, NULL, NULL, 626, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5028, NULL, NULL, NULL, 574, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5029, NULL, NULL, NULL, 576, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5030, NULL, NULL, NULL, 580, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5031, NULL, NULL, NULL, 577, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5032, NULL, NULL, NULL, 593, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5033, NULL, NULL, NULL, 594, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5034, NULL, NULL, NULL, 595, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5035, NULL, NULL, NULL, 596, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5036, NULL, NULL, NULL, 597, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5037, NULL, NULL, NULL, 598, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5038, NULL, NULL, NULL, 625, N'sel', N'organ', -1, NULL)
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
GO
print 'Processed 100 total records'
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3950, NULL, NULL, NULL, 607, N'sel', N'orglevel', NULL, 6)
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
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5039, NULL, NULL, NULL, 588, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5040, NULL, NULL, NULL, 589, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5041, NULL, NULL, NULL, 600, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5042, NULL, NULL, NULL, 590, N'sel', N'organ', -1, NULL)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_Id], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (5043, NULL, NULL, NULL, 591, N'sel', N'organ', -1, NULL)
SET IDENTITY_INSERT [dbo].[Purview] OFF
/****** Object:  Table [dbo].[ManageMenu]    Script Date: 08/15/2016 22:11:17 ******/
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
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (220, N'全站备份', N'item', 88, N'sys/sitebackup.aspx', N'', 1160, 125, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (221, N'首选项', N'item', 88, N'', N'', 1150, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (222, N'常规', N'item', 88, N'sys/general.aspx', N'', 1110, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (627, N'学员在线', N'item', 522, N'/manage/admin/stonline.aspx', N'', 110, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (628, N'在线时间', N'item', 547, N'/manage/student/online.aspx', N'', 110, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 78, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (629, N'学习记录', N'item', 547, N'/manage/student/studylog.aspx', N'', 120, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 101, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (88, N'系统设置', N'item', 88, N'', NULL, 15, 0, N'', N'', 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (89, N'功能菜单', N'item', 88, N'sys/menuroot.aspx', N'MenuTree', 1260, 101, N'', N'', 0, 0, NULL, NULL, 1, 1, N'管理界面左侧的菜单', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (90, N'数据库备份', N'item', 88, N'sys/databasebackup.aspx', N'', 1500, 125, N'', N'', 0, 0, NULL, NULL, 1, 1, N'此操作，仅限对access版的数据库备份。', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (94, N'修改密码', N'item', 92, N'ty', NULL, 5, 95, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (95, N'用户管理', N'item', 92, N'ty', NULL, 10, 92, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 1, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (96, N'查询用户信息', N'item', 92, N'ty', NULL, 10, 95, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'tyuityityu', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (101, N'菜单管理', N'item', 88, N'', NULL, 1240, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (102, N'系统菜单', N'item', 88, N'sys/SysMenu.aspx', NULL, 1490, 101, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'位于管理界面右上方的下拉菜单', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (103, N'系统菜单', NULL, 103, NULL, NULL, 0, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 1, NULL, 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (104, N'用户', N'item', 103, N'#', N'', 60, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (108, N'关于', N'open', 103, N'help/about.html', N'', 85, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 400, 200, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (111, N'基本资料', N'open', 103, N'Personal/info.aspx', N'', 30, 104, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'用户基本资料', 0, N'sys', 640, 450, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (112, N'密码修改', N'open', 103, N'Personal/password.aspx', N'', 20, 104, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (113, N'首页', N'link', 103, N'/', N'', 10, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (121, N'退出', N'event', 103, N'if(confirm(''是否真的要退出？'')){window.location.href=''/manage/index.aspx'';}', N'', 100, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (122, N'部门信息', N'item', 88, N'sys/depart.aspx', N'purview', 1810, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (124, N'组织机构', N'item', 88, N'', N'', 1700, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (125, N'安全管理', N'item', 88, N'', N'', 1310, 221, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (128, N'公司信息', N'item', 88, N'sys/organizationinfo.aspx', N'', 1620, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (129, N'单位介绍', N'item', 88, N'sys/organizationintro.aspx', N'', 1650, 316, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (140, N'工作组', N'item', 88, N'sys/empgroup.aspx', N'purview', 1930, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (141, N'岗位设置', N'item', 88, N'sys/position.aspx', N'purview', 1870, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (142, N'员工基本信息', N'item', 88, N'sys/employee.aspx', N'', 1770, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (193, N'系统日志', N'item', 88, N'', N'', 1560, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (194, N'登录日志', N'item', 88, N'Sys/LogsLogin.aspx', N'', 1250, 193, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (195, N'操作记录', N'item', 88, N'sys/LogsWork.aspx', N'', 1520, 193, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (278, N'系统参数', N'item', 88, N'sys/syspara.aspx', N'', 1350, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (316, N'基本信息', N'item', 88, N'', N'', 790, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (369, N'数据清理', N'item', 88, N'sys/dataclear.aspx', N'', 1410, 221, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (370, N'职务头衔', N'item', 88, N'sys/title.aspx', N'', 1910, 124, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (510, N'机构管理', N'item', 88, N'', N'', 490, 88, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (511, N'机构列表', N'item', 88, N'sys/Organization.aspx', N'', 590, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (512, N'机构审核', N'item', 88, N'sys/OrganVerify.aspx', N'', 640, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (513, N'机构等级', N'item', 88, N'sys/OrganLevel.aspx', N'', 680, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (514, N'机构权限', N'item', 88, N'sys/purview.aspx?type=organ', N'', 650, 510, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (542, N'二级域名预留', N'item', 88, N'sys/limitdomain.aspx', N'', 1130, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (522, N'机构的管理', N'item', 522, NULL, N'organAdmin', 17, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 105, 69)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (528, N'机构信息', N'item', 522, N'/manage/sys/organizationinfo.aspx', N'', 300, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 101, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (535, N'教师信息', N'item', 522, N'/manage/teacher/list.aspx', N'', 120, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 162, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (536, N'新闻栏目', N'item', 522, N'/manage/content/columns.aspx', N'', 180, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 85, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (537, N'新闻发布', N'item', 522, N'/manage/content/Contents.aspx?action=modify', N'', 160, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'在', 0, N'func', 400, 300, 180, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (541, N'优秀教师', N'item', 522, N'', N'', 100, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 162, 134)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (544, N'学生管理', N'item', 522, N'/manage/student/list.aspx', N'', 130, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 156, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (547, N'学员的管理', N'item', 547, NULL, N'student', 18, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 0, 0, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (548, N'在线学习', N'item', 547, N'', N'', 130, 547, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 32, 165)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (550, N'我的课程', N'item', 547, N'/manage/student/selfcourse.aspx', N'', 60, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 32, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (623, N'院系设置', N'item', 522, N'/manage/admin/Departs.aspx', N'', 260, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (552, N'测试成绩', N'item', 547, N'/manage/exam/TestArchives.aspx', N'', 80, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 84, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (553, N'考试成绩', N'item', 547, N'/manage/exam/Archives.aspx', N'', 90, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 125, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (554, N'错题回顾', N'item', 547, N'/manage/student/QuesError.aspx', N'', 100, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 187, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (556, N'参加考试', N'item', 547, N'/manage/exam/SelfExam.aspx', N'', 70, 548, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 162, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (624, N'其它', N'item', 522, N'', N'', 330, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (561, N'我的账户', N'item', 547, N'', N'', 140, 547, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 106, 69)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (562, N'基本信息', N'item', 547, N'/manage/student/info.aspx', N'', 10, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 186, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (563, N'联系方式', N'item', 547, N'/manage/student/link.aspx', N'', 30, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 210, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (564, N'安全管理', N'item', 547, N'/manage/student/safe.aspx', N'', 50, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 57, 199)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (565, N'管理员', N'item', 522, N'', N'', 350, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 130, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (566, N'个人信息', N'item', 522, N'/manage/Personal/info.aspx', N'', 60, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 179, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (523, N'平台管理', N'item', 522, N'', N'', 250, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 132, 37)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (524, N'专业设置', N'item', 522, N'/manage/sys/Subject.aspx', N'', 270, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 206, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (525, N'教务管理', N'item', 522, N'', N'', 230, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'gggg', 0, N'func', 400, 300, 230, 166)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (545, N'学生分班', N'item', 522, N'/manage/student/sort.aspx', N'', 150, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 8, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (568, N'安全管理', N'item', 522, N'/manage/Personal/safe.aspx', N'', 70, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'已经选学的课程', 0, N'func', 400, 300, 6, 166)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (569, N'教师的管理', N'item', 569, NULL, N'teacher', 19, 0, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (570, N'课程管理', N'item', 569, N'', N'', 165, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 32, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (572, N'课程列表', N'item', 569, N'/manage/teacher/courses.aspx', N'', 220, 570, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 230, 68)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (574, N'测试/考试', N'item', 569, N'', N'', 170, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 232, 100)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (607, N'关于我们', N'item', 522, N'/manage/sys/organizationintro.aspx', N'', 10, 624, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (576, N'试卷管理', N'item', 569, N'/manage/exam/testpaper.aspx', N'', 140, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (577, N'考试成绩', N'item', 569, N'/manage/teacher/Archives.aspx', N'', 160, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 57, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (606, N'授权', N'open', 103, N'panel/Authorization.aspx', N'', 90, 103, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'sys', 600, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (608, N'模板管理', N'item', 88, N'template/list.aspx', N'', 1135, 221, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 0, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (580, N'考试管理', N'item', 569, N'/manage/exam/Examination.aspx', N'', 150, 574, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 31, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (609, N'界面风格', N'item', 522, N'/manage/template/select.aspx', N'', 290, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 133, 38)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (610, N'教师职称', N'item', 522, N'/manage/Teacher/sort.aspx', N'', 280, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (583, N'知识库管理', N'item', 569, N'/manage/Knowledge/contents.aspx', N'', 60, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 63, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (584, N'交流', N'item', 569, N'', N'', 200, 569, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 8, 197)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (585, N'留言回复', N'item', 569, N'', N'', 70, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 108, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (586, N'在线解答', N'item', 569, N'', N'', 110, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 135, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (587, N'论坛', N'item', 569, N'/manage/site/MessageBoard.aspx', N'', 120, 584, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (588, N'个人信息', N'item', 569, N'', N'', 210, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 162, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (589, N'基本信息', N'item', 569, N'/manage/teacher/info.aspx', N'', 80, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (590, N'工作履历', N'item', 569, N'/manage/teacher/history.aspx', N'', 100, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 231, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (591, N'密码安全', N'item', 569, N'/manage/teacher/safe.aspx', N'', 130, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 57, 199)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (592, N'首页轮换图片', N'item', 522, N'', N'', 320, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 81, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (593, N'题库管理', N'link', 569, N'#', N'', 190, 569, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 91, 102)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (594, N'填空题', N'item', 569, N'/manage/Questions/list.aspx?type=5', N'', 0, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 116, 102)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (595, N'单选题', N'item', 569, N'/manage/Questions/list.aspx?type=1', N'', 20, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 140, 99)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (596, N'多选题', N'item', 569, N'/manage/Questions/list.aspx?type=2', N'', 30, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 162, 98)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (597, N'判断题', N'item', 569, N'/manage/Questions/list.aspx?type=3', N'', 40, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 186, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (598, N'简答题', N'item', 569, N'/manage/Questions/list.aspx?type=4', N'', 50, 593, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 210, 98)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (600, N'联系方式', N'item', 569, N'/manage/teacher/link.aspx', N'', 90, 588, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 210, 132)
GO
print 'Processed 100 total records'
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (622, N'课程交流', N'item', 569, N'/manage/site/MessageBoard.aspx', N'', 250, 570, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (526, N'新闻/通知', N'item', 522, N'', N'', 220, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 32, 164)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (527, N'统计分析', N'item', 522, N'', N'', 240, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 57, 132)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (529, N'组织机构', N'item', 522, N'', N'', 340, 522, NULL, NULL, 0, 0, NULL, NULL, 0, 0, N'', 0, N'func', 400, 300, 230, 38)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (530, N'管理员列表', N'item', 522, N'/manage/sys/employee.aspx', N'', 0, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 7, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (531, N'部门信息', N'item', 522, N'/manage/sys/depart.aspx', N'purview', 190, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 160, 70)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (532, N'角色管理', N'item', 522, N'/manage/sys/position.aspx', N'purview', 50, 565, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 135, 133)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (533, N'职务头衔', N'item', 522, N'/manage/sys/title.aspx', N'', 210, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 108, 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (534, N'工作组', N'item', 522, N'/manage/sys/empgroup.aspx', N'purview', 200, 529, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 8, 198)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (538, N'通知', N'item', 522, N'/manage/site/notice.aspx', N'', 170, 526, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 58, 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (539, N'热门课程', N'item', 522, N'/Manage/Admin/CourseHot.aspx', N'', 80, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'选课最多的专业', 0, N'func', 400, 300, 83, 37)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (540, N'热门教师', N'item', 522, N'', N'', 90, 527, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 59, 39)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (546, N'参数设置', N'item', 522, N'/manage/admin/organsetup.aspx', N'', 310, 523, NULL, NULL, 0, 0, NULL, NULL, 1, 0, N'', 0, N'func', 400, 300, 57, 71)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (618, N'友情链接', N'item', 522, N'/Manage/Site/Links/LinksInfo.aspx', N'', 20, 624, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (619, N'友情链接分类', N'item', 522, N'/Manage/Site/Links/LinksSort.aspx', N'', 40, 624, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (621, N'课程管理', N'item', 522, N'/manage/admin/courses.aspx?admin=true', N'', 140, 525, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (630, N'资金管理', N'item', 522, N'', N'', 325, 522, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 206, 7)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (631, N'资金流水', N'item', 522, N'/manage/money/Details.aspx', N'', 0, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (632, N'充值码', N'item', 522, N'/manage/money/RechargeSet.aspx', N'', 1, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (633, N'支付接口', N'item', 522, N'/manage/pay/paylist.aspx', N'', 2, 630, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (635, N'充值', N'item', 547, N'/Manage/Student/Moneyrecharge.aspx', N'', 0, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoS], [MM_IcoB], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_State], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY]) VALUES (636, N'资金流水', N'item', 547, N'/Manage/Student/MoneyDetails.aspx', N'', 5, 561, NULL, NULL, 0, 0, NULL, NULL, 1, 1, N'', 0, N'func', 400, 300, 90, 101)
SET IDENTITY_INSERT [dbo].[ManageMenu] OFF
/****** Object:  Default [DF__ManageMen__MM_Ro__01D345B0]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Ro__01D345B0]  DEFAULT ((0)) FOR [MM_Root]
GO
/****** Object:  Default [DF__ManageMen__MM_Ta__02C769E9]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Ta__02C769E9]  DEFAULT ((0)) FOR [MM_Tax]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__03BB8E22]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__03BB8E22]  DEFAULT ((0)) FOR [MM_IsBold]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__04AFB25B]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__04AFB25B]  DEFAULT ((0)) FOR [MM_IsItalic]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__05A3D694]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__05A3D694]  DEFAULT ((1)) FOR [MM_IsUse]
GO
/****** Object:  Default [DF__ManageMen__MM_Is__0697FACD]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Is__0697FACD]  DEFAULT ((1)) FOR [MM_IsShow]
GO
/****** Object:  Default [DF__ManageMen__MM_St__078C1F06]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_St__078C1F06]  DEFAULT ((1)) FOR [MM_State]
GO
/****** Object:  Default [DF__ManageMen__MM_Wi__0880433F]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Wi__0880433F]  DEFAULT ((0)) FOR [MM_WinWidth]
GO
/****** Object:  Default [DF__ManageMen__MM_Wi__09746778]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[ManageMenu] ADD  CONSTRAINT [DF__ManageMen__MM_Wi__09746778]  DEFAULT ((0)) FOR [MM_WinHeight]
GO
/****** Object:  Default [DF__Purview__Dep_Id__1CA7377D]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [Dep_Id]
GO
/****** Object:  Default [DF__Purview__EGrp_Id__1D9B5BB6]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [EGrp_Id]
GO
/****** Object:  Default [DF__Purview__Posi_Id__1E8F7FEF]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [Posi_Id]
GO
/****** Object:  Default [DF__Purview__MM_Id__1F83A428]    Script Date: 08/15/2016 22:11:17 ******/
ALTER TABLE [dbo].[Purview] ADD  DEFAULT ((0)) FOR [MM_Id]
GO
