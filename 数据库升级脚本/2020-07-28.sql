--2020-7-28
/*学员组，增加该组学员在视频学习时，切换浏览器是否不暂停播放，默认为false*/
alter table StudentSort add Sts_SwitchPlay  bit NULL
go
UPDATE StudentSort SET Sts_SwitchPlay  = 0
GO
alter table StudentSort ALTER COLUMN Sts_SwitchPlay  bit NOT NULL
/*修改菜单名称*/
UPDATE ManageMenu
   SET ManageMenu.MM_Name='学员组'
   where ManageMenu.MM_Name='学员班级'