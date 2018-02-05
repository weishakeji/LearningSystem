

/*增加老师是否显示的字段，可以控制教师是否在前台展示*/
alter table [Teacher] add Th_IsShow [bit] NULL
go
update [Teacher] set Th_IsShow=1
go
alter table [Teacher] ALTER COLUMN Th_IsShow [bit] NOT NULL