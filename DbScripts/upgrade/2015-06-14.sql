--2015-06-14

--添加考试结果中的机构信息
alter table [ExamResults] add Org_ID int default 0 NOT NULL
go
alter table [ExamResults] add Org_Name varchar(255)
go