
--教师分类
--2015-05-17

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TeacherSort]') AND type in (N'U'))
DROP TABLE [dbo].[TeacherSort]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TeacherSort](
 [Ths_ID] [int] IDENTITY(1,1) NOT NULL,
 [Ths_Name] [nvarchar](255) NULL,
 [Ths_Tax] [int] NOT NULL,
 [Ths_Intro] [nvarchar](2000) NULL,
 [Ths_IsUse] [bit] NOT NULL,
 [Ths_IsDefault] [bit] NOT NULL,
 [Org_ID] [int] NOT NULL,
 [Org_Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_TeacherSort] PRIMARY KEY CLUSTERED
(
 [Ths_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


--教师表的修改
alter table [Teacher] add Ths_ID int default 0 NOT NULL
go
alter table [Teacher] add Ths_Name varchar(255)
go

--菜单项修改
update [ManageMenu] set mm_link='/manage/Teacher/sort.aspx' where mm_id=610
go
update managemenu set mm_name='学员分组' where mm_id=545
go
update managemenu set mm_name='教师分组' where mm_id=610
go





