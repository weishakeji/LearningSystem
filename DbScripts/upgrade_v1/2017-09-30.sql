--2017-09-30

/*增加考试时间的类型，1为固定时间，2为设置时间区间*/
alter table [Examination] add Exam_DateType int NULL
go
update [Examination] set Exam_DateType=1
go
alter table [Examination] ALTER COLUMN Exam_DateType int NOT NULL
go
/*增加结束时间*/
alter table [Examination] add Exam_DateOver [datetime] NULL
go
update [Examination] set Exam_DateOver=getdate() 
go
alter table [Examination] ALTER COLUMN Exam_DateOver [datetime] NOT NULL
go
/*增加学员毕业院校*/
alter table [Accounts] add [Ac_School] [nvarchar](255) NULL