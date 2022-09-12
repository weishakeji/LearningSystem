--2019-04-24
/*增加公安备案*/
alter table Organization add Org_GonganBeian  [nvarchar](255) NULL
go
/*删除仅限在微信中使用的字段（该功能采用其它方式体现）*/
ALTER TABLE Organization DROP COLUMN Org_IsOnlyWeixin 
go
--alter table [Student_Course] add Stc_IsTry  float not NULL
/*添加学员试题练习得分记录*/
alter table [Student_Course] add Stc_QuesScore float NULL
go
UPDATE [Student_Course]  SET Stc_QuesScore = 0
GO
alter table [Student_Course] ALTER COLUMN Stc_QuesScore float NOT NULL
go
/*添加学员视频学习进度记录*/
alter table [Student_Course] add Stc_StudyScore float NULL
go
UPDATE [Student_Course]  SET Stc_StudyScore = 0
GO
alter table [Student_Course] ALTER COLUMN Stc_StudyScore float NOT NULL
go
/*添加学员结课考试成绩记录*/
alter table [Student_Course] add Stc_ExamScore float NULL
go
UPDATE [Student_Course]  SET Stc_ExamScore = 0
GO
alter table [Student_Course] ALTER COLUMN Stc_ExamScore float NOT NULL
go