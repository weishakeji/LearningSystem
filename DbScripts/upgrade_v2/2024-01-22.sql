
/*增加学员在线记录相关的地理信息*/
--省，市，区县，
alter table [LogForStudentOnline] add Lso_Province [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_City [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_District [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Source [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Info [nvarchar](255) NULL
go
--行政区划编码
alter table [LogForStudentOnline] add Lso_Code int NULL
go
update [LogForStudentOnline] set Lso_Code=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Code int NOT NULL
go
--经度
alter table [LogForStudentOnline] add Lso_Longitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Longitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Longitude decimal(20, 15) NOT NULL
go
--纬度
alter table [LogForStudentOnline] add Lso_Latitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Latitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Latitude decimal(20, 15) NOT NULL
go
--数据获取方式（GPS，IP），默认为0，IP方式为1
alter table [LogForStudentOnline] add Lso_GeogType [int] NULL
go
update [LogForStudentOnline] set Lso_GeogType=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_GeogType [int] NOT NULL



/*试题练习记录，增加地理信息字段*/
alter table LogForStudentExercise drop column Lse_UID
go
alter table LogForStudentExercise add Lse_GeogData nvarchar(max) NULL
go
/*视频学习记录，增加地理信息字段*/
alter table LogForStudentStudy add Lss_GeogData nvarchar(max) NULL
go

alter table [Columns] ALTER COLUMN Col_IsChildren [bit] NOT NULL
go

/****** 重建管理菜单******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ManageMenu]') AND type in (N'U'))
DROP TABLE [dbo].[ManageMenu]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purview]') AND type in (N'U'))
DROP TABLE [dbo].[Purview]
GO
/****** Object:  Table [dbo].[Purview]    Script Date: 01/22/2024 10:24:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purview](
	[Pur_Id] [int] IDENTITY(1,1) NOT NULL,
	[Dep_Id] [int] NOT NULL,
	[EGrp_Id] [int] NOT NULL,
	[Posi_Id] [int] NOT NULL,
	[MM_UID] [nvarchar](50) NULL,
	[Pur_State] [nvarchar](50) NULL,
	[Pur_Type] [nvarchar](50) NULL,
	[Org_ID] [int] NOT NULL,
	[Olv_ID] [int] NOT NULL,
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
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'1033' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DecimalPlaces', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'Purview' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'4' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Purview', @level2type=N'COLUMN',@level2name=N'MM_UID'
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
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2887, 0, 0, 0, N'a2d8c81ec24efe439b4c9b2d139e99fe', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2888, 0, 0, 0, N'f9b11e7920f6ab15ead04eeafb511830', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2889, 0, 0, 0, N'8546016f8e1c6e078b5dddd0eab7920d', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2890, 0, 0, 0, N'b50ea4a3ed65be9d39651c1f1ecf014c', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2891, 0, 0, 0, N'53774f25cbb2a6248bdc4b5783d1f842', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2892, 0, 0, 0, N'19c839b6968161696712b7e7b76c9772', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2893, 0, 0, 0, N'651397af8465c643284ff8e137fd8079', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2894, 0, 0, 0, N'1639658295720', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2895, 0, 0, 0, N'606b87e461d6b43e1ff789ad9b1b11c2', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2896, 0, 0, 0, N'f2b59e41fb0d29f16707ad11b590e686', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2897, 0, 0, 0, N'1697428790833', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2898, 0, 0, 0, N'1697428791888', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2899, 0, 0, 0, N'1697428808320', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2900, 0, 0, 0, N'1697791615582', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2901, 0, 0, 0, N'7505573f225da91c421b31e8e950aa16', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2902, 0, 0, 0, N'fdffc2a7aa807909b9c259169c70794d', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2903, 0, 0, 0, N'e3346bd15202ce654c42f126d2153a41', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2904, 0, 0, 0, N'1697789410970', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2905, 0, 0, 0, N'1697789613476', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2954, 0, 0, 0, N'a2d8c81ec24efe439b4c9b2d139e99fe', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2955, 0, 0, 0, N'f9b11e7920f6ab15ead04eeafb511830', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2956, 0, 0, 0, N'8546016f8e1c6e078b5dddd0eab7920d', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2957, 0, 0, 0, N'b50ea4a3ed65be9d39651c1f1ecf014c', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2958, 0, 0, 0, N'53774f25cbb2a6248bdc4b5783d1f842', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2959, 0, 0, 0, N'19c839b6968161696712b7e7b76c9772', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2960, 0, 0, 0, N'651397af8465c643284ff8e137fd8079', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2961, 0, 0, 0, N'1639658295720', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2962, 0, 0, 0, N'606b87e461d6b43e1ff789ad9b1b11c2', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2963, 0, 0, 0, N'f2b59e41fb0d29f16707ad11b590e686', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2964, 0, 0, 0, N'1697428790833', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2965, 0, 0, 0, N'1697428791888', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2966, 0, 0, 0, N'1697428808320', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2967, 0, 0, 0, N'1697791615582', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2968, 0, 0, 0, N'7505573f225da91c421b31e8e950aa16', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2969, 0, 0, 0, N'fdffc2a7aa807909b9c259169c70794d', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2970, 0, 0, 0, N'e3346bd15202ce654c42f126d2153a41', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2971, 0, 0, 0, N'1697789410970', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2972, 0, 0, 0, N'1697789613476', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2973, 0, 0, 0, N'1697791007226', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2974, 0, 0, 0, N'5f81b3a13ce40cdc0525d3346bcdc682', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2975, 0, 0, 0, N'1697790883366', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2976, 0, 0, 0, N'1697791356432', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2977, 0, 0, 0, N'1705402993842', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2978, 0, 0, 0, N'1705403019261', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2979, 0, 0, 0, N'fc60823be11ec1b67cbc8865085928ca', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2980, 0, 0, 0, N'1697103541077', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2981, 0, 0, 0, N'5f8650559e67d7aee0865ab46abc57e5', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2982, 0, 0, 0, N'5f3a00f6661e44c530939cb7ad74845f', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2983, 0, 0, 0, N'1639834362028', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2984, 0, 0, 0, N'1695313545347', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2985, 0, 0, 0, N'1695313546693', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2986, 0, 0, 0, N'1695313569415', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2987, 0, 0, 0, N'4be6e84aeacaec7514680b72499b7c19', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2988, 0, 0, 0, N'185d53f8d69610c63281766012d17a8d', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2989, 0, 0, 0, N'e99f9b903ccc0bdefdbca97abcc9f4b1', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2990, 0, 0, 0, N'df3455c4a980c841604b55dc6651a92f', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2991, 0, 0, 0, N'9bbdcbde47d569e6a9d5c59a8947a445', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2992, 0, 0, 0, N'5469dba2b4b8d54745500eea8c1ba089', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2993, 0, 0, 0, N'c68451aaa777687e559756c9f02f68d3', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2994, 0, 0, 0, N'8a539ecff79b6ede1b38b2a8380e86cd', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2995, 0, 0, 0, N'1697103633401', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2996, 0, 0, 0, N'99e3c10a6ff4c0af38d4ad6551662222', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2997, 0, 0, 0, N'0df6f2c3642c081462a35f1f1ada550a', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2998, 0, 0, 0, N'caedac420273252de2dadfd450a98382', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2999, 0, 0, 0, N'1642663352817', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3000, 0, 0, 0, N'1641613171019', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3001, 0, 0, 0, N'7c5f1c92ee9e6c364a46c755df860b26', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3002, 0, 0, 0, N'22475af5e44f46286660708fb4f2c4c9', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3003, 0, 0, 0, N'1641735504763', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3004, 0, 0, 0, N'501e00e137aebb030d30b8de30edec06', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2906, 0, 0, 0, N'1697791007226', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2907, 0, 0, 0, N'5f81b3a13ce40cdc0525d3346bcdc682', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3005, 0, 0, 0, N'ca6c8e9988678ea4bc089a98d64dbe43', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3006, 0, 0, 0, N'02d9f63ce76365a7d986e0b0a0ea70e4', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3007, 0, 0, 0, N'b632ee17275095c13cfb8055129c59cd', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3008, 0, 0, 0, N'17ed5191fd4a3b9d3fc366a1cda5b4dc', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3009, 0, 0, 0, N'3ed08ea8c3a6dbbb0b14535e27357061', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3010, 0, 0, 0, N'3a108c8fbb70ddb57532149a214bc427', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3011, 0, 0, 0, N'1641640249725', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3012, 0, 0, 0, N'82809d1a369ab3c44c330c909a532866', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3013, 0, 0, 0, N'83f7721be4b7779ce3f097a59b81adb9', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3014, 0, 0, 0, N'6b83ad54dc5319393f4eaf23b6ae14c8', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3015, 0, 0, 0, N'cc6e884e86541560bddc33e539cfdbc7', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3016, 0, 0, 0, N'e32ff65ff0db40c9d569a80d95550c25', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3017, 0, 0, 0, N'1673081413592', NULL, N'orglevel', 0, 5)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2908, 0, 0, 0, N'1697790883366', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2909, 0, 0, 0, N'1697791356432', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2910, 0, 0, 0, N'1701256457042', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2911, 0, 0, 0, N'1705402993842', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2912, 0, 0, 0, N'1705403019261', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2913, 0, 0, 0, N'fc60823be11ec1b67cbc8865085928ca', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2914, 0, 0, 0, N'1697103541077', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2915, 0, 0, 0, N'5f8650559e67d7aee0865ab46abc57e5', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2916, 0, 0, 0, N'5f3a00f6661e44c530939cb7ad74845f', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2917, 0, 0, 0, N'1639834362028', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2918, 0, 0, 0, N'1695313545347', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2919, 0, 0, 0, N'1695313546693', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2920, 0, 0, 0, N'1695313569415', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2921, 0, 0, 0, N'4be6e84aeacaec7514680b72499b7c19', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2922, 0, 0, 0, N'185d53f8d69610c63281766012d17a8d', NULL, N'orglevel', 0, 1)
GO
print 'Processed 100 total records'
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2923, 0, 0, 0, N'e99f9b903ccc0bdefdbca97abcc9f4b1', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2924, 0, 0, 0, N'df3455c4a980c841604b55dc6651a92f', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2925, 0, 0, 0, N'9bbdcbde47d569e6a9d5c59a8947a445', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2926, 0, 0, 0, N'5469dba2b4b8d54745500eea8c1ba089', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2927, 0, 0, 0, N'c68451aaa777687e559756c9f02f68d3', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2928, 0, 0, 0, N'8a539ecff79b6ede1b38b2a8380e86cd', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2929, 0, 0, 0, N'1697103633401', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2930, 0, 0, 0, N'99e3c10a6ff4c0af38d4ad6551662222', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2931, 0, 0, 0, N'0df6f2c3642c081462a35f1f1ada550a', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2932, 0, 0, 0, N'caedac420273252de2dadfd450a98382', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2933, 0, 0, 0, N'1642663352817', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2934, 0, 0, 0, N'1641613171019', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2935, 0, 0, 0, N'1704957895680', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2936, 0, 0, 0, N'7c5f1c92ee9e6c364a46c755df860b26', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2937, 0, 0, 0, N'22475af5e44f46286660708fb4f2c4c9', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2938, 0, 0, 0, N'1641735504763', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2939, 0, 0, 0, N'501e00e137aebb030d30b8de30edec06', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2940, 0, 0, 0, N'ca6c8e9988678ea4bc089a98d64dbe43', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2941, 0, 0, 0, N'02d9f63ce76365a7d986e0b0a0ea70e4', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2942, 0, 0, 0, N'b632ee17275095c13cfb8055129c59cd', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2943, 0, 0, 0, N'17ed5191fd4a3b9d3fc366a1cda5b4dc', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2944, 0, 0, 0, N'3ed08ea8c3a6dbbb0b14535e27357061', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2945, 0, 0, 0, N'3a108c8fbb70ddb57532149a214bc427', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2946, 0, 0, 0, N'1641640249725', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2947, 0, 0, 0, N'82809d1a369ab3c44c330c909a532866', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2948, 0, 0, 0, N'83f7721be4b7779ce3f097a59b81adb9', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2949, 0, 0, 0, N'6b83ad54dc5319393f4eaf23b6ae14c8', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2950, 0, 0, 0, N'cc6e884e86541560bddc33e539cfdbc7', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2951, 0, 0, 0, N'e32ff65ff0db40c9d569a80d95550c25', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2952, 0, 0, 0, N'1673081413592', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (2953, 0, 0, 0, N'b486c9a4eca4cc594585bd6639e281fe', NULL, N'orglevel', 0, 1)
INSERT [dbo].[Purview] ([Pur_Id], [Dep_Id], [EGrp_Id], [Posi_Id], [MM_UID], [Pur_State], [Pur_Type], [Org_ID], [Olv_ID]) VALUES (3018, 0, 0, 0, N'b486c9a4eca4cc594585bd6639e281fe', NULL, N'orglevel', 0, 5)
SET IDENTITY_INSERT [dbo].[Purview] OFF
/****** Object:  Table [dbo].[ManageMenu]    Script Date: 01/22/2024 10:24:24 ******/
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
	[MM_PatId] [nvarchar](255) NULL,
	[MM_Color] [nvarchar](50) NULL,
	[MM_Font] [nvarchar](50) NULL,
	[MM_IsBold] [bit] NOT NULL,
	[MM_IsItalic] [bit] NOT NULL,
	[MM_IcoCode] [nvarchar](50) NULL,
	[MM_IsUse] [bit] NOT NULL,
	[MM_IsShow] [bit] NOT NULL,
	[MM_Intro] [nvarchar](255) NULL,
	[MM_IsChilds] [bit] NOT NULL,
	[MM_Func] [nvarchar](50) NULL,
	[MM_WinWidth] [int] NOT NULL,
	[MM_WinHeight] [int] NOT NULL,
	[MM_IcoX] [int] NOT NULL,
	[MM_IcoY] [int] NOT NULL,
	[MM_UID] [nvarchar](255) NULL,
	[MM_WinMin] [bit] NOT NULL,
	[MM_WinMax] [bit] NOT NULL,
	[MM_WinMove] [bit] NOT NULL,
	[MM_WinResize] [bit] NOT NULL,
	[MM_WinID] [nvarchar](255) NULL,
	[MM_AbbrName] [nvarchar](255) NULL,
	[MM_IsFixed] [bit] NOT NULL,
	[MM_Help] [nvarchar](1000) NULL,
	[MM_Complete] [int] NOT NULL,
	[MM_IcoColor] [nvarchar](100) NULL,
	[MM_IcoSize] [int] NOT NULL,
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
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'睛帞䘮䀿璷�꼆祡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'109' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMEMode', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_IMESentMode', @value=N'3' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_IcoS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'12' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'255' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_IcoS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'10' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
GO
EXEC sys.sp_addextendedproperty @name=N'UnicodeCompression', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IcoCode'
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
EXEC sys.sp_addextendedproperty @name=N'AggregateType', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'AllowZeroLength', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'AppendOnly', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'Attributes', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'CollatingOrder', @value=N'2052' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnHidden', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnOrder', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'ColumnWidth', @value=N'-1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'CurrencyLCID', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'DataUpdatable', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'DefaultValue', @value=N'True' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'GUID', @value=N'䐖鋖ꎩ䈢겒୙ᓥょ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DisplayControl', @value=N'106' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Format', @value=N'Yes/No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'Name', @value=N'MM_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'OrdinalPosition', @value=N'17' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'Required', @value=N'False' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'ResultType', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'Size', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceField', @value=N'MM_State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'SourceTable', @value=N'ManageMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'TextAlign', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
GO
EXEC sys.sp_addextendedproperty @name=N'Type', @value=N'1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ManageMenu', @level2type=N'COLUMN',@level2name=N'MM_IsChilds'
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
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (6881, N'常规', N'item', 88, N'/manage/Platform/Platinfo', NULL, 0, N'651', NULL, NULL, 0, 0, N'a030', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'6362b3b6a38c5c7b976fa64e40219e46', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (88, N'系统设置', N'item', 88, NULL, N'system', 3, N'0', NULL, NULL, 0, 0, N'a038', 1, 1, N'请不要轻易改动！', 0, N'func', 400, 300, 0, 0, N'88', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22731, N'注册协议', N'item', 651, N'/manage/Platform/Agreement', NULL, 0, N'88', NULL, NULL, 0, 0, N'a022', 1, 1, NULL, 1, N'func', 400, 300, 0, 0, N'69475b3d3dfff2e4470402686bbe9393', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22732, N'保留域名', N'item', 88, N'/manage/Platform/limitdomain', NULL, 1, N'88', NULL, NULL, 0, 0, N'e7d4', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'70c20a152062c09c598384f35d9cde36', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22733, N'系统参数', N'item', 88, N'/manage/Platform/SystemPara', NULL, 2, N'88', NULL, NULL, 0, 0, N'a030', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'b918a2008b0d5adb9e852b6bb113cf9c', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22734, N'接口管理', N'item', 88, NULL, NULL, 3, N'88', NULL, NULL, 0, 0, N'a01c', 1, 1, NULL, 1, N'func', 400, 300, 0, 0, N'742f03375a49149ef533668189ec0777', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22735, N'支付接口', N'item', 88, N'/manage/pay/list', NULL, 0, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e824', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'a19b1b08a06bac9d0adc044e0055a53b', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22736, N'第三方登录', N'item', 88, N'/manage/OtherLogin/setup', NULL, 1, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e645', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'6a47cb4dcaff0fe97b45c0347139f438', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22737, N'短信接口', N'item', 88, N'/manage/SMS/Setup', NULL, 2, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e76e', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'f4c77daf1be33a237f6e5ee64da40c63', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22738, N'单点登录', N'item', 88, N'/manage/Sso/Setup', NULL, 3, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e639', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'8b60275871e44251d0712030ec9a44e7', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22739, N'七牛云直播', N'item', 88, N'/manage/live/qiniuyun', NULL, 4, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e661', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'8d34c9389a5dd4ec6ab6c3bf50060b10', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (2318, N'分支机构', NULL, 2318, NULL, N'organ', 2, N'0', NULL, NULL, 0, 0, N'a003', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'99171ec989452adea2c9d8d8b4a2c3a4', 0, 0, 0, 0, NULL, N'分校', 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22740, N'百度地图', N'item', 88, N'/manage/setup/BaiduLBS', NULL, 5, N'742f03375a49149ef533668189ec0777', NULL, NULL, 0, 0, N'e64c', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'1701333322013', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22741, N'菜单管理', N'item', 88, NULL, NULL, 4, N'88', NULL, NULL, 0, 0, N'a00c', 1, 1, NULL, 1, N'func', 0, 0, 0, 0, N'9e4d9f97f71fc3a076e9893449bea4be', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22742, N'功能菜单', N'item', 88, N'/manage/Platform/menuroot', N'MenuTree', 0, N'9e4d9f97f71fc3a076e9893449bea4be', NULL, NULL, 0, 0, N'a024', 1, 1, N'管理界面左侧的菜单', 0, N'func', 0, 0, 0, 0, N'33f1cd7ad494a32c5a061babf4d62599', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22743, N'系统菜单', N'item', 88, N'/manage/Platform/sysmenu', NULL, 1, N'9e4d9f97f71fc3a076e9893449bea4be', NULL, NULL, 0, 0, N'a005', 1, 1, N'位于管理界面左上方的下拉菜单', 0, N'func', 0, 0, 0, 0, N'26c139b82c6c9455864a56a4c8ba0f6f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22744, N'积分设置', N'item', 651, N'/manage/Platform/PointSetup', NULL, 5, N'88', NULL, NULL, 0, 0, N'e88a', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'2ad779650365a880c345016b15ed9401', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22745, N'系统日志', N'item', 88, NULL, NULL, 6, N'88', NULL, NULL, 0, 0, N'a025', 0, 1, NULL, 1, N'func', 0, 0, 0, 0, N'485d923c64b7a076861d2189d31bc52b', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22746, N'数据清理', N'item', 88, NULL, NULL, 0, N'485d923c64b7a076861d2189d31bc52b', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'ef5ed2135cec3915a022d5f8d6d91b45', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (522, N'机构管理', N'item', 522, NULL, N'organAdmin', 4, N'0', NULL, NULL, 0, 0, N'a003', 1, 0, N'用于机构管理员操作', 0, N'func', 400, 300, 0, 0, N'522', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (547, N'学员的管理', N'item', 547, NULL, N'student', 5, N'0', NULL, NULL, 0, 0, N'e804', 1, 0, N'学员登录后看到的菜单项', 0, N'func', 0, 0, 0, 0, N'547', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (569, N'教师的管理', N'item', 569, NULL, N'teacher', 6, N'0', NULL, NULL, 0, 0, N'e647', 1, 0, N'教师进入教学管理时看到的菜单项', 0, N'func', 400, 300, 0, 0, N'569', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22747, N'登录日志', N'item', 88, NULL, NULL, 1, N'485d923c64b7a076861d2189d31bc52b', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'da544360c2d541f501758b826d1ca510', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9247, N'账户', N'item', 651, N'/manage/Platform/Accounts', NULL, 0, N'651', NULL, NULL, 0, 0, N'e67d', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'552b799487f974e4e2705254180ab16e', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9248, N'资金流水', N'item', 651, N'/manage/Capital/Records', NULL, 1, N'651', NULL, NULL, 0, 0, N'e746', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'de1cfb2ccec3e0befab63d0957227213', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9249, N'学习卡', N'item', 0, N'/manage/Learningcard/cardset', NULL, 2, N'651', NULL, NULL, 0, 0, N'e60f', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'eab253f97e1b78feab00fc256047acd5', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9250, N'充值码', N'item', 651, N'/manage/Rechargecode/Codeset', NULL, 3, N'651', NULL, NULL, 0, 0, N'e62f', 1, 1, NULL, 1, N'func', 400, 300, 0, 0, N'9eb880dcf1d228e4e75f07429e0fe17a', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9251, N'考试管理', N'item', 0, NULL, NULL, 4, N'651', NULL, NULL, 0, 0, N'f008f', 0, 1, NULL, 1, N'func', 0, 0, 0, 0, N'71fd444a28f50e71f664761603daf63e', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9252, N'考试成绩', N'item', 0, N'/teacher/exam/results', NULL, 0, N'71fd444a28f50e71f664761603daf63e', NULL, NULL, 0, 0, N'e82e', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'9773958521dd3c6f46bbc424d26b9770', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (651, N'基础设置', N'item', 651, NULL, N'base', 0, N'0', NULL, NULL, 0, 0, N'e72f', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'651', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10193, N'分机构管理', N'item', 651, NULL, NULL, 0, N'99171ec989452adea2c9d8d8b4a2c3a4', NULL, NULL, 0, 0, N'e770', 1, 1, NULL, 1, N'func', 400, 300, 0, 0, N'dbda24c276292d3229c7df766a890ca1', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10194, N'机构列表', N'item', 651, N'/manage/Organs/list', NULL, 0, N'dbda24c276292d3229c7df766a890ca1', NULL, NULL, 0, 0, N'a02a', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'a93acf4d1572ef78fa2d748a69b58738', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10195, N'机构审核', N'item', 651, N'/manage/Organs/Verify', NULL, 1, N'dbda24c276292d3229c7df766a890ca1', NULL, NULL, 0, 0, N'a042', 0, 1, NULL, 0, N'func', 400, 300, 0, 0, N'ee61424a18a1b5544124b197dad6142e', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10196, N'机构等级', N'item', 651, N'/manage/Organs/Level', NULL, 2, N'dbda24c276292d3229c7df766a890ca1', NULL, NULL, 0, 0, N'e81b', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'3f9df262b89e46fa930b4369a5e239bd', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10197, N'分润设置', N'item', 651, N'/manage/Platform/ProfitSharing', NULL, 3, N'dbda24c276292d3229c7df766a890ca1', NULL, NULL, 0, 0, N'e699', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'9fba648f72cf773e0a560c421aae8420', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10198, N'当前机构', N'item', 88, NULL, NULL, 1, N'99171ec989452adea2c9d8d8b4a2c3a4', NULL, NULL, 0, 0, N'a003', 1, 1, NULL, 1, N'func', 400, 300, 0, 0, N'c251c17f570f77bf00728d8d8dd0ae9f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10199, N'机构信息', N'item', 88, N'/manage/Organs/organinfo', NULL, 0, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e6a2', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'7383d2425a17f4ce59cb13094c96118a', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10200, N'单位介绍', N'item', 88, N'/manage/Organs/about', NULL, 1, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e667', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'151c0f86093805fe0fcdda6be39bb007', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10201, N'管理员', N'item', 88, N'/manage/Platform/Employee', NULL, 2, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e645', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'ff70834b1af658beaa1ba86dd2514880', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10202, N'部门信息', N'item', 88, N'sys/depart.aspx', N'purview', 3, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e68b', 0, 1, NULL, 0, N'func', 400, 300, 0, 0, N'887e1c3444208f98b12445e7a62e1c2f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10203, N'岗位设置', N'item', 88, N'/manage/Platform/Position', N'purview', 4, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e655', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'871a922530ef97717cbd6f5f61d5cafc', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10204, N'职务头衔', N'item', 88, N'/manage/Platform/EmpTitle', NULL, 5, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e804', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'baa31535166ec9736c4ccf03a44a5dd9', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24780, N'数据校正', N'item', 522, NULL, NULL, 5, N'1697428052534', NULL, NULL, 0, 0, N'e79e', 1, 0, NULL, 1, N'func', 0, 0, 0, 0, N'1697789368523', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(102, 4, 214)', 10)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24781, N'试题修复', N'item', 522, N'/manage/ques/itemrepeat', NULL, 0, N'1697789368523', NULL, NULL, 0, 0, N'e6d8', 1, 0, NULL, 0, N'func', 0, 0, 0, 2, N'1697789410970', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(245, 74, 0)', -6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24782, N'学习进度', N'item', 522, N'/orgadmin/data/VideoProgress', NULL, 1, N'1697789368523', NULL, NULL, 0, 0, N'a049', 1, 0, N'人工修正视频学习进度、试题练习完成度', 0, N'func', 0, 0, 0, 0, N'1697789613476', 0, 0, 0, 0, NULL, NULL, 0, NULL, 86, N'rgb(15, 134, 197)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24783, N'试题练习', N'item', 522, N'/orgadmin/data/Exercise', NULL, 2, N'1697789368523', NULL, NULL, 0, 0, N'e6b0', 0, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697789631914', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(75, 2, 197)', -6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24784, N'考试加分', N'item', 522, NULL, NULL, 3, N'1697789368523', NULL, NULL, 0, 0, NULL, 0, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697789647933', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24785, N'数据分析', N'item', 522, NULL, NULL, 3, N'522', NULL, NULL, 0, 0, N'e6ef', 1, 0, NULL, 1, N'func', 400, 300, 0, 2, N'2b858af35952ad4106e1f97b10b20ff9', 0, 0, 0, 0, NULL, N'数据', 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24786, N'收入汇总', N'item', 522, N'/orgadmin/Capital/Summary', NULL, 0, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e81c', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697791007226', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(213, 25, 4)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24787, N'热门课程', N'item', 522, N'/orgadmin/Statis/CourseHot', NULL, 1, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e856', 1, 0, N'选课最多的专业', 0, N'func', 400, 300, 0, 0, N'5f81b3a13ce40cdc0525d3346bcdc682', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(255, 66, 18)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24788, N'课程收入', N'item', 522, N'/orgadmin/Statis/CourseAmount', NULL, 2, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e746', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697790883366', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(230, 145, 10)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24789, N'资源存储', N'item', 522, N'/orgadmin/Statis/Storage', NULL, 3, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e6a4', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697791356432', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(22, 127, 183)', -6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24790, N'热门教师', N'item', 522, NULL, NULL, 4, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e650', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'ee0d5748fa5855398e9d2a0ba5ac5651', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10205, N'工作组', N'item', 88, N'/manage/Platform/EmpGroup', N'purview', 6, N'c251c17f570f77bf00728d8d8dd0ae9f', NULL, NULL, 0, 0, N'e67d', 1, 1, NULL, 0, N'func', 400, 300, 0, 0, N'82757f86610edd41612051d8484dc6e9', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24791, N'优秀教师', N'item', 522, N'/manage/teacher/order.aspx', NULL, 5, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e8c9', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'53308789bf582bb1e60583d0df808580', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24792, N'学员在线', N'item', 522, N'/manage/orgadmin/stonline.aspx', NULL, 6, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e67d', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'a24cbf78d35248503adfaa03ff60ca11', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24793, N'教师评价', N'item', 522, N'/manage/teacher/Comments.aspx', NULL, 7, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e81b', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'843c0827d3d2810782adda9aaa6f409a', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24794, N'数据大屏', N'item', 522, N'/web/viewport/Index', NULL, 8, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'e6f1', 0, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1701256457042', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24795, N'登录日志', N'item', 522, NULL, NULL, 9, N'2b858af35952ad4106e1f97b10b20ff9', NULL, NULL, 0, 0, N'a01d', 1, 0, NULL, 1, N'func', 0, 0, 5, 0, N'1705109474971', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 15)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24796, N'日志详情', N'item', 522, N'/orgadmin/log/loginlog', NULL, 0, N'1705109474971', NULL, NULL, 0, 0, N'e61d', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1705402993842', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24797, N'地域分布', N'item', 522, N'/web/viewport/login', NULL, 1, N'1705109474971', NULL, NULL, 0, 0, N'a04a', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1705403019261', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24798, N'平台管理', N'item', 522, NULL, NULL, 4, N'522', NULL, NULL, 0, 0, N'a038', 1, 0, NULL, 1, N'func', 400, 300, 0, 2, N'd42f434639edecdbd7f7f5919e6d086a', 0, 0, 0, 0, NULL, N'管理', 0, NULL, 100, NULL, 3)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24799, N'基础信息', N'item', 522, N'/orgadmin/setup/General', NULL, 0, N'd42f434639edecdbd7f7f5919e6d086a', NULL, NULL, 0, 0, N'e732', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'fc60823be11ec1b67cbc8865085928ca', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(237, 104, 9)', -5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24800, N'关于我们', N'item', 522, N'/orgadmin/setup/about', NULL, 1, N'd42f434639edecdbd7f7f5919e6d086a', NULL, NULL, 0, 0, N'e67d', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697103541077', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(175, 3, 196)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24801, N'管理员', N'item', 522, NULL, NULL, 2, N'd42f434639edecdbd7f7f5919e6d086a', NULL, NULL, 0, 0, N'e6ed', 1, 0, N'当前机构的管理人员', 1, N'func', 400, 300, 0, -2, N'76d02763628ceb20715ff8700f93d711', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(4, 109, 173)', 6)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24802, N'管理人员', N'item', 522, N'/orgadmin/admin/Employee', NULL, 0, N'76d02763628ceb20715ff8700f93d711', NULL, NULL, 0, 0, N'e812', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'5f8650559e67d7aee0865ab46abc57e5', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(2, 120, 71)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24803, N'角色/岗位', N'item', 522, N'/orgadmin/admin/Position', N'purview', 1, N'76d02763628ceb20715ff8700f93d711', NULL, NULL, 0, 0, N'e635', 1, 0, NULL, 0, N'func', 400, 300, 0, -2, N'5f3a00f6661e44c530939cb7ad74845f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 89, N'rgb(168, 2, 177)', 5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24804, N'职务', N'item', 522, N'/orgadmin/admin/EmpTitle', NULL, 2, N'76d02763628ceb20715ff8700f93d711', NULL, NULL, 0, 0, N'e645', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1639834362028', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(2, 119, 177)', -1)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24805, N'组织机构', N'item', 522, NULL, NULL, 3, N'd42f434639edecdbd7f7f5919e6d086a', NULL, NULL, 0, 0, N'e711', 0, 0, NULL, 1, N'func', 400, 300, 0, 0, N'85c3ddb99d58e33f9e3637cfb6c2b298', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(9, 126, 184)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24806, N'部门信息', N'item', 522, N'/manage/sys/depart.aspx', N'purview', 0, N'85c3ddb99d58e33f9e3637cfb6c2b298', NULL, NULL, 0, 0, N'e687', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'482695a528fb8f892b6dceb259bd40b3', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(24, 85, 2)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24807, N'工作组', N'item', 522, N'/manage/sys/empgroup.aspx', N'purview', 1, N'85c3ddb99d58e33f9e3637cfb6c2b298', NULL, NULL, 0, 0, N'e67d', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'8238df6e47c88762ba42a7995e547f3e', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(2, 57, 85)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24808, N'职务头衔', N'item', 522, N'/manage/sys/title.aspx', NULL, 2, N'85c3ddb99d58e33f9e3637cfb6c2b298', NULL, NULL, 0, 0, N'e639', 0, 0, NULL, 0, N'func', 400, 300, 0, -2, N'0dd1db3b35c9fe75e38edcdf738ebd2c', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(83, 5, 188)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23680, N'课程管理', N'item', 569, N'/teacher/course/list', NULL, 0, N'569', NULL, NULL, 0, 0, N'e813', 1, 0, NULL, 1, N'func', 400, 300, 0, 3, N'6b83ad54dc5319393f4eaf23b6ae14c8', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (9974, N'试题修复', N'item', 651, N'/manage/ques/itemrepeat', NULL, 6, N'651', NULL, NULL, 0, 0, N'e755', 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'1669172453942', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23681, N'课程交流', N'item', 569, N'/teacher/course/Messages', NULL, 1, N'569', NULL, NULL, 0, 0, N'e817', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'cc6e884e86541560bddc33e539cfdbc7', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23682, N'我的学员', N'item', 569, N'/teacher/student/list', NULL, 2, N'569', NULL, NULL, 0, 0, N'e67d', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'a4c7e947718555647d5c033409812101', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23683, N'测试/考试', N'item', 569, NULL, NULL, 3, N'569', NULL, NULL, 0, 0, N'e816', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'ceef164ede5ac404041250eb01c46be3', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23684, N'试卷管理', N'item', 569, N'/teacher/testpaper/list', NULL, 0, N'ceef164ede5ac404041250eb01c46be3', NULL, NULL, 0, 0, N'e6b0', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'e32ff65ff0db40c9d569a80d95550c25', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23685, N'专项考试成绩', N'item', 569, N'/teacher/exam/results', NULL, 1, N'ceef164ede5ac404041250eb01c46be3', NULL, NULL, 0, 0, N'e810', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1673081413592', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23686, N'个人信息', N'item', 569, N'/teacher/self/index', NULL, 4, N'569', NULL, NULL, 0, 0, N'e669', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'b486c9a4eca4cc594585bd6639e281fe', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24809, N'功能设置', N'item', 522, N'/orgadmin/setup/function', NULL, 4, N'd42f434639edecdbd7f7f5919e6d086a', NULL, NULL, 0, 0, N'a038', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'acc513b05fe589c27716631c54ef30f8', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(208, 107, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10136, N'首页', N'link', 103, N'/', NULL, 0, N'0', NULL, NULL, 0, 0, N'a020', 1, 1, N'系统首页', 0, N'sys', 0, 0, 0, 0, N'd9ffe1150602ac1fd987007396520189', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10137, N'源代码', N'item', 103, N'#', NULL, 1, N'0', NULL, NULL, 0, 0, N'a034', 1, 1, NULL, 1, N'sys', 0, 0, 0, 0, N'5f241cb3015f4e6ba505ae9243feef99', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10138, N'产品源码', N'link', 103, N'https://github.com/weishakeji/LearningSystem', NULL, 0, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, N'e691', 1, 1, N'支持二次开发', 0, N'sys', 400, 300, 0, 0, N'b6570c588539269c6de5ac06b7438e70', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10139, N'WebUI源码', N'link', 103, N'https://github.com/weishakeji/WebdeskUI', NULL, 1, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, N'a010', 1, 1, NULL, 0, N'sys', 640, 450, 0, 0, N'e680f05fce7d34c783286d98e2beeb97', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10140, N'国内镜像', N'link', 103, N'https://gitee.com/weishakeji', NULL, 2, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, N'e686', 1, 1, N'国内Gitee开源镜像', 0, N'sys', 0, 0, 0, 0, N'c834f537e216e4b529ef7d4449593524', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10141, N'null', N'hr', 103, NULL, NULL, 3, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'b6936694735702ccacb76f692444f547', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10142, N'开发文档', N'item', 103, NULL, NULL, 4, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, N'a022', 1, 1, NULL, 1, N'sys', 0, 0, 0, 0, N'36851ec244b58c6e893a788c049298e5', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10143, N'ViewData API 说明', N'link', 103, N'/help/api/', NULL, 0, N'36851ec244b58c6e893a788c049298e5', NULL, NULL, 0, 0, N'a01c', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'7bfaf2ad62853302824b625c926a5a2f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10144, N'数据字典', N'link', 103, N'/help/datas/', NULL, 1, N'36851ec244b58c6e893a788c049298e5', NULL, NULL, 0, 0, N'e6a4', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'd14c2127edbccdaf73490709d0cf78c2', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10145, N'WebUI 开发文档', N'link', 103, N'http://webdesk.weisha100.cn/', NULL, 2, N'36851ec244b58c6e893a788c049298e5', NULL, NULL, 0, 0, N'a010', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'5e4e303706b7d0583453a3ada7f267bf', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10146, N'newnode-7879', N'hr', 103, NULL, NULL, 3, N'36851ec244b58c6e893a788c049298e5', NULL, NULL, 0, 0, N'e600', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'0d73063295412838f39d31e015036d40', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10147, N'图标库', N'link', 103, N'/Utilities/Fonts/index.html', NULL, 4, N'36851ec244b58c6e893a788c049298e5', NULL, NULL, 0, 0, N'e610', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'23f7e9cf86ce216020ab6251693d49b1', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10148, N'升级日志', N'link', 103, N'http://www.weishakeji.net/download.html', NULL, 5, N'5f241cb3015f4e6ba505ae9243feef99', NULL, NULL, 0, 0, N'e836', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'f268a63814cc7500123950307b3faa4f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10149, N'帮助', N'item', 103, NULL, NULL, 2, N'0', NULL, NULL, 0, 0, N'a026', 1, 1, NULL, 1, N'sys', 400, 200, 0, 0, N'83c860c6e6d9efc97c1d643a69562a0a', 0, 0, 0, 0, NULL, NULL, 1, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10150, N'自述文件', N'link', 103, N'/readme.html', NULL, 0, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a022', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'4a0ba13c193afd931459fe4422fe2284', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10151, N'在线教程', N'link', 103, N'http://www.weisha100.net/', NULL, 1, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'e7d4', 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'f5383ee2e50f234d322ec3e6c6ab22ec', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10152, N'帮助', N'link', 103, N'/help', NULL, 2, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a026', 1, 1, N'在线学习教程', 0, N'sys', 0, 0, 0, 0, N'8eb4784b53887148a45bc80fc16a19c1', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10153, N'newnode-5338', N'hr', 103, NULL, NULL, 3, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'26de70c3a084bf45bba3d1108abf2144', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10154, N'开源协议', N'open', 103, N'/help/License.html', NULL, 4, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a034', 1, 1, NULL, 0, N'sys', 600, 400, 0, 0, N'a2980cfee1ad6268ab5a3af56b8b3b64', 0, 0, 1, 0, N'PublicLicense', NULL, 0, NULL, 0, NULL, 0)
GO
print 'Processed 100 total records'
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10155, N'版权信息', N'open', 103, N'/manage/Platform/copyright', NULL, 5, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a027', 1, 1, NULL, 0, N'sys', 800, 600, 0, 0, N'60a81a51602ea583adc3d086d10a5424', 1, 1, 1, 1, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10156, N'newnode-7595', N'hr', 103, NULL, NULL, 6, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'sys', 0, 0, 0, 0, N'b39062b640c982317666591ac7481a2d', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10157, N'关于', N'open', 103, NULL, NULL, 7, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a031', 1, 1, NULL, 1, N'sys', 600, 400, 0, 0, N'8b6bd638621686c382eb11d1d6759ad7', 0, 0, 1, 1, N'about', NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (10158, N'微厦科技', N'link', 103, N'http://www.weishakeji.net', NULL, 8, N'83c860c6e6d9efc97c1d643a69562a0a', NULL, NULL, 0, 0, N'a001', 0, 1, N'微厦科技官网', 0, N'sys', 0, 0, 0, 0, N'8fcb85b04376833dc6fb8a2fda5abab8', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24755, N'教务管理', N'item', 522, NULL, NULL, 0, N'522', NULL, NULL, 0, 0, N'e76b', 1, 0, N'gggg', 1, N'func', 400, 300, 0, 2, N'cba85fde312efe019f9acafe32038fe4', 0, 0, 0, 0, NULL, N'教务', 0, NULL, 100, NULL, -2)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24756, N'学习内容', N'item', 522, NULL, NULL, 0, N'cba85fde312efe019f9acafe32038fe4', NULL, NULL, 0, 0, N'e813', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'a76471634c09b23347199ee23682d1ed', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(42, 130, 3)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24757, N'课程', N'item', 522, N'/orgadmin/course/list', NULL, 0, N'a76471634c09b23347199ee23682d1ed', N'rgb(66, 163, 6)', NULL, 0, 0, N'e813', 1, 0, N'课程管理，包括课程章节、视频、试题、价格等', 0, N'func', 400, 300, 0, 0, N'a2d8c81ec24efe439b4c9b2d139e99fe', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24758, N'专业', N'item', 522, N'/orgadmin/Subject/list', NULL, 1, N'a76471634c09b23347199ee23682d1ed', N'rgb(3, 121, 180)', NULL, 0, 0, N'e750', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'f9b11e7920f6ab15ead04eeafb511830', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(5, 74, 191)', 3)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24759, N'试题库', N'item', 522, N'/orgadmin/Question/list', NULL, 2, N'a76471634c09b23347199ee23682d1ed', N'rgb(107, 8, 210)', NULL, 0, 0, N'e755', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'8546016f8e1c6e078b5dddd0eab7920d', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, -4)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24760, N'学员管理', N'item', 522, NULL, NULL, 1, N'cba85fde312efe019f9acafe32038fe4', NULL, NULL, 0, 0, N'e804', 1, 0, NULL, 1, N'func', 0, 0, 0, 0, N'fc20f555f70b93019964a391d33a14d5', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(220, 51, 6)', 8)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24761, N'学员信息', N'item', 522, N'/orgadmin/Student/list', NULL, 0, N'fc20f555f70b93019964a391d33a14d5', NULL, NULL, 0, 0, N'e808', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'b50ea4a3ed65be9d39651c1f1ecf014c', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(2, 122, 20)', 2)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24762, N'学员组', N'item', 522, N'/orgadmin/Student/sort', NULL, 1, N'fc20f555f70b93019964a391d33a14d5', NULL, NULL, 0, 0, N'e67d', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'53774f25cbb2a6248bdc4b5783d1f842', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(34, 0, 252)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24763, N'教师管理', N'item', 522, NULL, NULL, 2, N'cba85fde312efe019f9acafe32038fe4', NULL, NULL, 0, 0, N'e650', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'bc07d10b0c411cab2e14f60328e8e4ad', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(0, 85, 169)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24764, N'教师信息', N'item', 522, N'/orgadmin/teacher/list', NULL, 0, N'bc07d10b0c411cab2e14f60328e8e4ad', NULL, NULL, 0, 0, N'e6a1', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'19c839b6968161696712b7e7b76c9772', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(1, 143, 36)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24765, N'教师职称', N'item', 522, N'/orgadmin/Teacher/titles', NULL, 1, N'bc07d10b0c411cab2e14f60328e8e4ad', NULL, NULL, 0, 0, N'e639', 1, 0, NULL, 0, N'func', 400, 300, 0, -2, N'651397af8465c643284ff8e137fd8079', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(173, 20, 238)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24766, N'学习证明', N'item', 522, N'/orgadmin/setup/Stamp', NULL, 3, N'cba85fde312efe019f9acafe32038fe4', NULL, NULL, 0, 0, N'e68f', 1, 0, NULL, 0, N'func', 0, 0, 0, -2, N'1639658295720', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(183, 56, 4)', 1)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24767, N'专项考试', N'item', 522, NULL, NULL, 1, N'522', NULL, NULL, 0, 0, N'e810', 1, 0, NULL, 1, N'func', 0, 0, 0, 3, N'1665903171969', 0, 0, 0, 0, NULL, N'考试', 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24768, N'考试管理', N'item', 569, N'/teacher/exam/list', NULL, 0, N'1665903171969', NULL, NULL, 0, 0, N'e810', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'606b87e461d6b43e1ff789ad9b1b11c2', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(82, 4, 242)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24769, N'考试成绩', N'item', 569, N'/teacher/exam/results', NULL, 1, N'1665903171969', NULL, NULL, 0, 0, N'e82e', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'f2b59e41fb0d29f16707ad11b590e686', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(5, 137, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (22748, N'操作记录', N'item', 88, NULL, NULL, 2, N'485d923c64b7a076861d2189d31bc52b', NULL, NULL, 0, 0, NULL, 1, 1, NULL, 0, N'func', 0, 0, 0, 0, N'4635221fa43b3a4f0a55e39cc0d5772c', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24770, N'运营与维护', N'item', 522, NULL, NULL, 2, N'522', NULL, NULL, 0, 0, N'e79b', 1, 0, NULL, 1, N'func', 0, 0, 0, 3, N'1697428052534', 0, 0, 0, 0, NULL, N'运维', 0, NULL, 100, NULL, 3)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24771, N'充值卡', N'item', 522, N'/orgadmin/Rechargecode/Codeset', NULL, 0, N'1697428052534', NULL, NULL, 0, 0, N'e60f', 1, 0, NULL, 0, N'func', 0, 0, 0, -2, N'1697428790833', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(196, 32, 2)', 5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24772, N'学习卡', N'item', 522, N'/orgadmin/Learningcard/cardset', NULL, 1, N'1697428052534', NULL, NULL, 0, 0, N'e685', 1, 0, NULL, 0, N'func', 0, 0, 0, -2, N'1697428791888', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(0, 130, 30)', 5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24773, N'资金管理', N'item', 522, NULL, NULL, 2, N'1697428052534', NULL, NULL, 0, 0, N'e824', 1, 0, NULL, 1, N'func', 0, 0, 0, 0, N'1697791565174', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(255, 123, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24774, N'资金流水', N'item', 522, N'/orgadmin/Capital/Records', NULL, 0, N'1697791565174', NULL, NULL, 0, 0, N'e749', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697428808320', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(235, 160, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24775, N'人工充值', N'item', 522, N'/orgadmin/Capital/Accounts', NULL, 1, N'1697791565174', NULL, NULL, 0, 0, N'e607', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697791615582', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(0, 154, 19)', 10)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24776, N'通知公告', N'item', 522, N'/orgadmin/notice/list', NULL, 3, N'1697428052534', NULL, NULL, 0, 0, N'e697', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'7505573f225da91c421b31e8e950aa16', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(1, 163, 29)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24777, N'新闻管理', N'item', 522, NULL, NULL, 4, N'1697428052534', NULL, NULL, 0, 0, N'e75c', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'd001a8ab9566b79e46dc6d81fb7fa213', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(244, 56, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24778, N'新闻发布', N'item', 522, N'/orgadmin/news/list', NULL, 0, N'd001a8ab9566b79e46dc6d81fb7fa213', NULL, NULL, 0, 0, N'e71c', 1, 0, N'新闻管理', 0, N'func', 400, 300, 0, -2, N'fdffc2a7aa807909b9c259169c70794d', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(201, 101, 6)', -5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24779, N'新闻栏目', N'item', 522, N'/orgadmin/news/Columns', NULL, 1, N'd001a8ab9566b79e46dc6d81fb7fa213', NULL, NULL, 0, 0, N'e668', 1, 0, NULL, 0, N'func', 400, 300, 0, -2, N'e3346bd15202ce654c42f126d2153a41', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(7, 120, 201)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23653, N'我的课程', N'item', 547, N'/student/course/index', NULL, 0, N'547', NULL, NULL, 0, 0, N'e813', 1, 0, N'已经选学的课程', 1, N'func', 400, 300, 0, 0, N'99e3c10a6ff4c0af38d4ad6551662222', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23654, N'学习回顾', N'item', 547, NULL, NULL, 1, N'547', N'rgb(1, 159, 200)', NULL, 0, 0, N'e6f1', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'8f2fedf0d52426d8107380eb60af23cb', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23655, N'测试成绩', N'item', 547, N'/Student/Test/Archives', NULL, 0, N'8f2fedf0d52426d8107380eb60af23cb', NULL, NULL, 0, 0, N'e634', 0, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'e61b2ed6eed9cb98323eadfde84d1d99', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23656, N'专项考试', N'item', 547, N'/Student/exam/Results', NULL, 1, N'8f2fedf0d52426d8107380eb60af23cb', NULL, NULL, 0, 0, N'e810', 1, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'0df6f2c3642c081462a35f1f1ada550a', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23657, N'错题回顾', N'item', 547, N'/student/Question/errors', NULL, 2, N'8f2fedf0d52426d8107380eb60af23cb', NULL, NULL, 0, 0, N'e6b0', 1, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'caedac420273252de2dadfd450a98382', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23658, N'学习证明', N'item', 547, N'/student/study/certificate', NULL, 3, N'8f2fedf0d52426d8107380eb60af23cb', NULL, NULL, 0, 0, N'e667', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'9570d28a7c77d0137551fa4d499e9988', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23659, N'学习证明', N'item', 547, N'/student/study/Certificate', NULL, 2, N'547', N'rgb(2, 128, 29)', NULL, 0, 0, N'e639', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1642663352817', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23660, N'--分隔线-', N'hr', 547, NULL, NULL, 3, N'547', NULL, NULL, 0, 0, NULL, 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1641613171019', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23661, N'登录日志', N'item', 547, N'/student/self/loginlog', NULL, 4, N'547', N'rgb(235, 49, 49)', NULL, 0, 0, N'a01d', 1, 0, NULL, 0, N'func', 0, 0, 5, 2, N'1704957895680', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, N'rgb(18, 176, 47)', 30)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23662, N'个人信息', N'node', 547, NULL, NULL, 5, N'547', NULL, NULL, 0, 0, N'e68f', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'c1d2a8cba0766a36dafa91f43a581f18', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23663, N'基本信息', N'item', 547, N'/student/Self/info', NULL, 0, N'c1d2a8cba0766a36dafa91f43a581f18', NULL, NULL, 0, 0, N'a043', 1, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'7c5f1c92ee9e6c364a46c755df860b26', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23664, N'联系方式', N'item', 547, N'/student/Self/link', NULL, 1, N'c1d2a8cba0766a36dafa91f43a581f18', NULL, NULL, 0, 0, N'e71a', 1, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'22475af5e44f46286660708fb4f2c4c9', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23665, N'个人介绍', N'item', 547, N'/student/Self/Intro', NULL, 2, N'c1d2a8cba0766a36dafa91f43a581f18', NULL, NULL, 0, 0, N'e669', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1641735504763', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23666, N'安全管理', N'item', 547, N'/student/Self/safe', NULL, 3, N'c1d2a8cba0766a36dafa91f43a581f18', NULL, NULL, 0, 0, N'e76a', 1, 0, N'已经选学的课程', 0, N'func', 400, 300, 0, 0, N'501e00e137aebb030d30b8de30edec06', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23667, N'在线时间', N'item', 547, N'/manage/student/online.aspx', NULL, 4, N'c1d2a8cba0766a36dafa91f43a581f18', NULL, NULL, 0, 0, N'a039', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'4cbafbacb24a146eb1e03cca80d32db6', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23668, N'充值/资金', N'node', 547, NULL, NULL, 6, N'547', NULL, NULL, 0, 0, N'e81c', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'f4c2e87c58a014d0eaaed7ba1a459314', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23669, N'充值', N'item', 547, N'/Student/Money/recharge', NULL, 0, N'f4c2e87c58a014d0eaaed7ba1a459314', NULL, NULL, 0, 0, N'e749', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'ca6c8e9988678ea4bc089a98d64dbe43', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23670, N'资金', N'item', 547, N'/Student/Money/Details', NULL, 1, N'f4c2e87c58a014d0eaaed7ba1a459314', NULL, NULL, 0, 0, N'e824', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'02d9f63ce76365a7d986e0b0a0ea70e4', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23671, N'卡券', N'item', 547, N'/Student/Coupon/index', NULL, 2, N'f4c2e87c58a014d0eaaed7ba1a459314', NULL, NULL, 0, 0, N'e847', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'b632ee17275095c13cfb8055129c59cd', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23672, N'积分', N'item', 547, N'/Student/Point/index', NULL, 3, N'f4c2e87c58a014d0eaaed7ba1a459314', NULL, NULL, 0, 0, N'e88a', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'17ed5191fd4a3b9d3fc366a1cda5b4dc', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23673, N'收益', N'item', 547, N'/Student/Money/Profit', NULL, 4, N'f4c2e87c58a014d0eaaed7ba1a459314', NULL, NULL, 0, 0, N'e666', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'3ed08ea8c3a6dbbb0b14535e27357061', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23674, N'学习卡', N'node', 547, NULL, NULL, 7, N'547', NULL, NULL, 0, 0, N'e685', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'dff9c491a3f3e5493cec04abf7244b69', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23675, N'我的学习卡', N'item', 547, N'/student/Learningcard/index', NULL, 0, N'dff9c491a3f3e5493cec04abf7244b69', NULL, NULL, 0, 0, N'e685', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'3a108c8fbb70ddb57532149a214bc427', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23676, N'newnode-6141', N'hr', 547, NULL, NULL, 8, N'547', NULL, NULL, 0, 0, NULL, 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1641640249725', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23677, N'分享', N'item', 547, NULL, NULL, 9, N'547', NULL, NULL, 0, 0, N'e690', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'2a3233d51cbc815b00bce3e9d7788dbb', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23678, N'分享链接', N'item', 547, N'/student/share/link', NULL, 0, N'2a3233d51cbc815b00bce3e9d7788dbb', NULL, NULL, 0, 0, N'e73e', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'82809d1a369ab3c44c330c909a532866', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (23679, N'我的朋友', N'item', 547, N'/student/share/subordinates', NULL, 1, N'2a3233d51cbc815b00bce3e9d7788dbb', NULL, NULL, 0, 0, N'e67d', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'83f7721be4b7779ce3f097a59b81adb9', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24810, N'二维码', N'item', 522, N'/orgadmin/setup/qrcode', NULL, 0, N'acc513b05fe589c27716631c54ef30f8', NULL, NULL, 0, 0, N'a053', 0, 0, NULL, 0, N'func', 400, 300, 0, 0, N'95aca19ba38c02a7b9f3d2b7a7e0c7e5', 0, 0, 0, 0, NULL, NULL, 0, NULL, 0, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24811, N'登录注册', N'item', 522, N'/orgadmin/setup/Register', NULL, 1, N'acc513b05fe589c27716631c54ef30f8', NULL, NULL, 0, 0, N'a035', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1695313545347', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(8, 113, 166)', -3)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24812, N'学习相关', N'item', 522, N'/orgadmin/setup/study', NULL, 2, N'acc513b05fe589c27716631c54ef30f8', NULL, NULL, 0, 0, N'e813', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1695313546693', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(3, 137, 62)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24813, N'终端设备', N'item', 522, N'/orgadmin/setup/device', NULL, 3, N'acc513b05fe589c27716631c54ef30f8', NULL, NULL, 0, 0, N'e79b', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1695313569415', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(64, 88, 0)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24814, N'界面风格', N'item', 522, NULL, NULL, 5, N'522', NULL, NULL, 0, 0, N'a010', 1, 0, NULL, 1, N'func', 400, 300, 0, 3, N'abf085d525ccc320ed7b6d05cd02f161', 0, 0, 0, 0, NULL, N'界面', 0, NULL, 100, NULL, 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24815, N'Web端风格', N'item', 522, NULL, NULL, 0, N'abf085d525ccc320ed7b6d05cd02f161', NULL, NULL, 0, 0, N'e609', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'65af6428b0ff12160d40e372ae5b8337', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(3, 39, 239)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24816, N'Web端模板', N'item', 522, N'/orgadmin/template/select.web', NULL, 0, N'65af6428b0ff12160d40e372ae5b8337', NULL, NULL, 0, 0, N'a044', 1, 0, NULL, 0, N'func', 400, 300, -2, -2, N'4be6e84aeacaec7514680b72499b7c19', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(74, 163, 6)', -5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24817, N'导航菜单', N'item', 522, N'/orgadmin/template/Navigation.web', NULL, 1, N'65af6428b0ff12160d40e372ae5b8337', NULL, NULL, 0, 0, N'a009', 1, 0, NULL, 0, N'func', 400, 300, -2, 0, N'185d53f8d69610c63281766012d17a8d', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(7, 78, 142)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24818, N'轮播图片', N'item', 522, N'/orgadmin/template/ShowPicture.web', NULL, 2, N'65af6428b0ff12160d40e372ae5b8337', NULL, NULL, 0, 0, N'a045', 1, 0, NULL, 0, N'func', 400, 300, 0, -1, N'e99f9b903ccc0bdefdbca97abcc9f4b1', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(22, 132, 210)', 1)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24819, N'移动端风格', N'item', 522, NULL, NULL, 1, N'abf085d525ccc320ed7b6d05cd02f161', NULL, NULL, 0, 0, N'e622', 1, 0, NULL, 1, N'func', 400, 300, 0, -2, N'bc979c4b488eaffbf119e6ab518f7689', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(204, 103, 5)', 2)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24820, N'移动端模板', N'item', 522, N'/orgadmin/template/select.mobi', NULL, 0, N'bc979c4b488eaffbf119e6ab518f7689', NULL, NULL, 0, 0, N'e677', 1, 0, NULL, 0, N'func', 400, 300, -2, -2, N'df3455c4a980c841604b55dc6651a92f', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(1, 174, 73)', 5)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24821, N'导航按钮', N'item', 522, N'/orgadmin/template/Navigation.mobi', NULL, 1, N'bc979c4b488eaffbf119e6ab518f7689', NULL, NULL, 0, 0, N'e632', 1, 0, NULL, 0, N'func', 400, 300, -2, -1, N'9bbdcbde47d569e6a9d5c59a8947a445', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(7, 134, 159)', -9)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24822, N'轮播图片', N'item', 522, N'/orgadmin/template/ShowPicture.mobi', NULL, 2, N'bc979c4b488eaffbf119e6ab518f7689', NULL, NULL, 0, 0, N'a045', 1, 0, NULL, 0, N'func', 400, 300, 0, -2, N'5469dba2b4b8d54745500eea8c1ba089', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(159, 65, 7)', 1)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24823, N'友情链接', N'item', 522, NULL, NULL, 2, N'abf085d525ccc320ed7b6d05cd02f161', NULL, NULL, 0, 0, N'a029', 1, 0, NULL, 1, N'func', 400, 300, 0, 0, N'2f1359ccc50792dbf8c75483a6f508fb', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(2, 89, 111)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24824, N'所有链接', N'item', 522, N'/orgadmin/links/list', NULL, 0, N'2f1359ccc50792dbf8c75483a6f508fb', NULL, NULL, 0, 0, N'a03d', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'c68451aaa777687e559756c9f02f68d3', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(4, 103, 177)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24825, N'链接分类', N'item', 522, N'/orgadmin/links/sort', NULL, 1, N'2f1359ccc50792dbf8c75483a6f508fb', NULL, NULL, 0, 0, N'a015', 1, 0, NULL, 0, N'func', 400, 300, 0, 0, N'8a539ecff79b6ede1b38b2a8380e86cd', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(132, 59, 7)', 0)
INSERT [dbo].[ManageMenu] ([MM_Id], [MM_Name], [MM_Type], [MM_Root], [MM_Link], [MM_Marker], [MM_Tax], [MM_PatId], [MM_Color], [MM_Font], [MM_IsBold], [MM_IsItalic], [MM_IcoCode], [MM_IsUse], [MM_IsShow], [MM_Intro], [MM_IsChilds], [MM_Func], [MM_WinWidth], [MM_WinHeight], [MM_IcoX], [MM_IcoY], [MM_UID], [MM_WinMin], [MM_WinMax], [MM_WinMove], [MM_WinResize], [MM_WinID], [MM_AbbrName], [MM_IsFixed], [MM_Help], [MM_Complete], [MM_IcoColor], [MM_IcoSize]) VALUES (24826, N'附加代码', N'item', 522, N'/orgadmin/setup/Extracode', NULL, 3, N'abf085d525ccc320ed7b6d05cd02f161', NULL, NULL, 0, 0, N'a033', 1, 0, NULL, 0, N'func', 0, 0, 0, 0, N'1697103633401', 0, 0, 0, 0, NULL, NULL, 0, NULL, 100, N'rgb(131, 4, 194)', 0)
SET IDENTITY_INSERT [dbo].[ManageMenu] OFF
