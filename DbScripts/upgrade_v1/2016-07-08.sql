--2016-07-08

/*增加考试成绩中，考试结束时（如果考试没有结束，则不进行成绩计算）*/
alter table [ExamResults] add [Exr_OverTime] [datetime] NULL
go
update [ExamResults] set [Exr_OverTime]='2000-01-01'
go
alter table [ExamResults] ALTER COLUMN [Exr_OverTime] [datetime] not NULL
/*增加考试成绩中，成绩计算的时间*/
alter table [ExamResults] add [Exr_CalcTime] [datetime] NULL
go
update [ExamResults] set [Exr_CalcTime]='2000-01-01'
go
alter table [ExamResults] ALTER COLUMN [Exr_CalcTime] [datetime] not NULL
/*增加考试成绩中，成绩最后递交时间*/
alter table [ExamResults] add [Exr_LastTime] [datetime] NULL
go
update [ExamResults] set [Exr_LastTime]='2000-01-01'
go
alter table [ExamResults] ALTER COLUMN [Exr_LastTime] [datetime] not NULL