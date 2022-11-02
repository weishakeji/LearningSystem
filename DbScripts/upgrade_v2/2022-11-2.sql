/*学员选修课程的记录，增加学习卡卡号信息的记录*/
alter table [Student_Course] add Lc_Code nvarchar(100)
go
alter table [Student_Course] add Lc_Pw nvarchar(50)
go
/*视频学习记录增加详情*/
alter table [LogForStudentStudy] add Lss_Details [nvarchar](max) NULL
go
/*章节增加编辑时间*/
alter table [Outline] add Ol_ModifyTime datetime null
go
update [Outline] set Ol_ModifyTime=GETDATE()
go
alter table [Outline] ALTER COLUMN Ol_ModifyTime datetime NOT NULL
/*章节增是否审核*/
alter table [Outline] add Ol_IsChecked bit null
go
update [Outline] set Ol_IsChecked=1
go
alter table [Outline] ALTER COLUMN Ol_IsChecked bit NOT NULL
