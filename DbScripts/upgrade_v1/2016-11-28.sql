-2016-11-28
/*
升级内容：
1、试卷随机抽题，可以按章节抽题；
2、政务版全面更新，采用新的设计风格；
3、优化模板引擎，简化表现层代码；
4、修正若干bug。
*/
/*增加试卷的注意事项字段*/
alter table [TestPaper] add Tp_Remind [nvarchar](max) NULL
go
/*增加试卷的副标题字段*/
alter table [TestPaper] add Tp_SubName [nvarchar](255) NULL
go
/*原试卷的名称字段长度增加到255*/
alter table [TestPaper] ALTER COLUMN [Tp_Name] [nvarchar](255) NULL
go
/*试卷的试题选择范围，为0时按课程选题，为1时按章节选题*/
alter table [TestPaper]  add Tp_FromType [int] NULL
go
update [TestPaper]  set Tp_FromType=0
go
alter table [TestPaper]  ALTER COLUMN Tp_FromType [int] NOT NULL
go
alter table [TestPaper] add Tp_FromConfig [nvarchar](max) NULL
go
/*试卷设定项表，增加所属章节*/
alter table [TestPagerItem]  add Ol_ID [int] NULL
go
update [TestPagerItem]  set Ol_ID=0
go
alter table [TestPagerItem]  ALTER COLUMN Ol_ID [int] NOT NULL
go
/*学员字段中的时间，设置为不可为空*/
update [Student]  set [St_CrtTime]=getdate() where [St_CrtTime] is null
go
alter table [Student]  ALTER COLUMN [St_CrtTime] [datetime] not NULL
go
/*单词拼错了，改一下*/
EXEC sp_rename 'TestPagerItem', 'TestPaperItem';
/*修正一些字段不可为空*/
update AddressList  set Ads_Id=0 where Ads_Id is null
go
alter table AddressList  ALTER COLUMN Ads_Id [int] NOT NULL
go
update AddressList  set Adl_Sex=0 where Adl_Sex is null
go
alter table AddressList  ALTER COLUMN Adl_Sex [int] NOT NULL
go
update AddressList  set Acc_Id=0 where Acc_Id is null
go
alter table AddressList  ALTER COLUMN Acc_Id [int] NOT NULL
go
update AddressList  set Adl_State=0 where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_State [int] NOT NULL
go
update AddressList  set Adl_Birthday=dateadd(day,2,'1770-01-01') where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_Birthday [datetime] NOT NULL
go
update AddressList  set Adl_CrtTime=dateadd(day,2,'1770-01-01') where Adl_State is null
go
alter table AddressList  ALTER COLUMN Adl_CrtTime [datetime] NOT NULL
go
