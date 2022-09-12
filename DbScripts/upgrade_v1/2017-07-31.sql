--2017-07-30
/*增加附件的第三方链接*/
alter table [Accessory] add [As_IsOther] [bit] NULL
go
update [Accessory] set [As_IsOther]=0
go
alter table [Accessory] ALTER COLUMN [As_IsOther] [bit] NOT NULL