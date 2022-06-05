--2016-08-17 

/*增加学员登录时的验证，每次登录生成随机字符*/
alter table [Student] add St_CheckUID [nvarchar](255) NULL
go