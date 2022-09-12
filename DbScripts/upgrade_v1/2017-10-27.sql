/*考试时是否允许切换窗口*/
alter table [Examination] add [Exam_IsToggle] [bit] NULL
go
update [Examination] set [Exam_IsToggle]=0
go
alter table [Examination] ALTER COLUMN [Exam_IsToggle] [bit] NOT NULL
go
/*是否显示确认按钮（有把握，完全靠瞎蒙的那一排按钮）*/
alter table [Examination] add [Exam_IsShowBtn] [bit] NULL
go
update [Examination] set [Exam_IsShowBtn]=0
go
alter table [Examination] ALTER COLUMN [Exam_IsShowBtn] [bit] NOT NULL
go
/*是否禁用鼠标右键，禁用后无法复制粘贴*/
alter table [Examination] add [Exam_IsRightClick] [bit] NULL
go
update [Examination] set [Exam_IsRightClick]=1
go
alter table [Examination] ALTER COLUMN [Exam_IsRightClick] [bit] NOT NULL
/*增加学员与账号关联*/
go
alter table [Student] add Ac_ID [int] NULL
go
update [Student] set Ac_ID=0
go
alter table [Student] ALTER COLUMN Ac_ID [int] NOT NULL
go
alter table [Student] add [Ac_UID] [nvarchar](255) NULL
go