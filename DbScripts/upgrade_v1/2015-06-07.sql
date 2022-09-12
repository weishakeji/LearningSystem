--2015-06-07

/*
主要升级
1、可以自定义及格分，默认是总分的60%；
2、考试中各场次可以自定义名称，原来是默认为试卷名称

*/

--增加试卷的及格分
alter table TestPaper add Tp_PassScore int default 0 not null

--增加考试中场次的名称
sp_rename 'Examination.Exam_Name','Exam_Title','column'
go
alter table Examination add Th_ID int default 0 not null
go
alter table Examination add Th_Name  [nvarchar](255) NULL
go
alter table [Examination] add [Exam_Name] [nvarchar](255) NULL
go
alter table ExamResults add [Exam_Title] [nvarchar](255) NULL
go
alter table ExamResults add [Exr_SubmitTime] [datetime] NOT NULL
go
--增加考试中的总分（与试卷中有关联，也可以不同步）
alter table Examination add Exam_Total int default 0 not null
go
alter table Examination add Exam_Tax int default 0 not null
go
--考试成绩管理增加学员信息
sp_rename 'ExamResults.Acc_Id','St_ID','column'
go
sp_rename 'ExamResults.Acc_Name','St_Name','column'
go
