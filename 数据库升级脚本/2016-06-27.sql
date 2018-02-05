/*增加考试成绩中，是否计算的字段*/
alter table [ExamResults] add Exr_IsCalc [bit] NULL
go
update [ExamResults] set Exr_IsCalc=0
go
alter table [ExamResults] ALTER COLUMN Exr_IsCalc [bit] NOT NULL