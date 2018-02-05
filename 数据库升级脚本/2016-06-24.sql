

/*增加附件的宽度、高度、文件扩展名*/
alter table [Accessory] add As_Width [int] NULL
go
update [Accessory] set As_Width=0
go
alter table [Accessory] ALTER COLUMN As_Width [int] NOT NULL
go
alter table [Accessory] add As_Height [int] NULL
go
update [Accessory] set As_Height=0
go
alter table [Accessory] ALTER COLUMN As_Height [int] NOT NULL
go
alter table [Accessory] add [As_Extension] [nvarchar](255) NULL
go
/*增加附件的播放时长（如果是视频）*/
alter table [Accessory] add As_Duration [int] NULL
go
update [Accessory] set As_Duration=0
go
alter table [Accessory] ALTER COLUMN As_Duration [int] NOT NULL
go
/*更改记录中的名称，为了更加语义化*/
sp_rename 'LogForStudentStudy.Lss_TotalTime','Lss_Duration','column' 
go