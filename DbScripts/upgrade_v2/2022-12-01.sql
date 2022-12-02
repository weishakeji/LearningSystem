/*增加章节中是否有附件*/
alter table [Outline] add [Ol_IsAccessory] bit default 0 NOT NULL

/*增加课程中是否允许编辑*/
alter table [Course] add [Cou_Allowedit] bit default 1 NOT NULL




