/*修改试卷简介的字段长度*/
alter table [TestPaper] ALTER COLUMN  [Tp_Intro] [nvarchar](max) NULL
go
/*修改课程内容的字段长度*/
alter table [Course] ALTER COLUMN  [Cou_Content] [nvarchar](max) NULL
go
/*将一些ntext字段，改成nvarchar max*/
alter table [Subject] ALTER COLUMN  [Sbj_Intro] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Intro] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Details] [nvarchar](max) NULL
go
alter table [Article] ALTER COLUMN  [Art_Endnote] [nvarchar](max) NULL
go