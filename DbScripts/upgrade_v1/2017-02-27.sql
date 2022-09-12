--2017-02-27
/*增加专业的详情、推荐字段*/
alter table [Subject] add Sbj_IsRec [bit] NULL
go
update [Subject] set Sbj_IsRec=0
go
alter table [Subject] ALTER COLUMN Sbj_IsRec [bit] NOT NULL
go
alter table [Subject] add Sbj_Details [nvarchar](max) NULL
go
/*增加课程推荐字段*/
alter table [Course] add Cou_IsRec [bit] NULL
go
update [Course] set Cou_IsRec=0
go
alter table [Course] ALTER COLUMN Cou_IsRec [bit] NOT NULL
go
/*增加课程价格、价格区间、价格单位*/
alter table [Course] add Cou_PriceSpan [int] NULL
go
update [Course] set Cou_PriceSpan=0
go
alter table [Course] ALTER COLUMN Cou_PriceSpan [int] NOT NULL
go
alter table [Course] add Cou_PriceUnit nvarchar(100)
go
EXEC sp_rename '[Course].Cou_Money','Cou_Price','column' 
go
/*在课程留中增加回复教师的关联id*/
alter table MessageBoard add Th_ID [int] NULL
go
update MessageBoard set Th_ID=0
go
alter table MessageBoard ALTER COLUMN Th_ID [int] NOT NULL
go
/*在学员中记录当前正在学习的课程id*/
alter table [Student] add St_CurrCourse [int] NULL
go
update [Student] set St_CurrCourse=0
go
alter table [Student] ALTER COLUMN St_CurrCourse [int] NOT NULL
go
/*机构的二维码，不再以图片方式存储，改用Base64*/
EXEC sp_rename '[Organization].Org_QrCode','Org_QrCodeUrl','column' 
go
alter table [Organization] add Org_QrCode nvarchar(MAX) NULL
go
alter table [Organization] ALTER COLUMN Org_Intro nvarchar(MAX) NULL
go
update Navigation set Nav_isshow=0 where Nav_Site='mobi'
go