/*第三方登录的管理*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ThirdpartyLogin](
	[Tl_ID] [int] IDENTITY(1,1) NOT NULL,
	[Tl_Tag] [nvarchar](255) NULL,
	[Tl_Tax] [int] not NULL,
	[Tl_Account] [nvarchar](255) NULL,
	[Tl_Domain] [nvarchar](500) NULL,
	[Tl_Name] [nvarchar](255) NULL,
	[Tl_IsUse] [bit] NOT NULL,
	[Tl_IsRegister] [bit] not NULL,
	[Tl_APPID] [nvarchar](255) NULL,
	[Tl_Secret]  [nvarchar](2000) NULL,
	[Tl_Returl] [nvarchar](1000) NULL,
	[Tl_Config] [nvarchar](max) NULL,
 CONSTRAINT [aaaaaThirdpartyLogin_PK] PRIMARY KEY NONCLUSTERED 
(
	[Tl_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*插件初始数据*/
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('qq','QQ',1,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('weixin','微信',2,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('yunzhijia','金蝶',3,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('zzgongshang','郑州工商',4,1,1,'','','','')
GO
/*qq登录由SystemPara转到thirdpartylogin表*/
update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='QQAPPID') where tl_tag='qq'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='QQAPPKey') where tl_tag='qq'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='QQReturl') where tl_tag='qq'
go
declare @qqdir int,@qqlogin int
select @qqdir= COUNT(*) from SystemPara where Sys_Key='QQDirectIs'
select @qqlogin= COUNT(*) from SystemPara where Sys_Key='QQLoginIsUse'
if @qqdir>0
begin
	update thirdpartylogin set [Tl_IsRegister]=(select  case when Sys_Value='True' then 1 else 0 end from SystemPara where Sys_Key='QQDirectIs') 
	where tl_tag='qq'
end
if @qqlogin>0
begin
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='QQLoginIsUse') 
	where tl_tag='qq'
end

/*云之家由SystemPara转到thirdpartylogin表*/
update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAppid') where tl_tag='yunzhijia'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAppSecret') where tl_tag='yunzhijia'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaDomain') where tl_tag='yunzhijia'
update thirdpartylogin set [Tl_Account]=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAcc') where tl_tag='yunzhijia'
go
declare @yunzhijiao int
select @yunzhijiao= COUNT(*) from SystemPara where Sys_Key='YunzhijiaLoginIsuse'
if @yunzhijiao>0
begin
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='YunzhijiaLoginIsuse') 
	where tl_tag='yunzhijia'
end


/*微信登录由SystemPara转到thirdpartylogin表*/
select * from SystemPara where Sys_Key like 'weixin%'

update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='WeixinAPPID') where tl_tag='weixin'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='WeixinSecret') where tl_tag='weixin'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='WeixinReturl') where tl_tag='weixin'
go
declare @weixindir int,@weixinlogin int
select @weixindir= COUNT(*) from SystemPara where Sys_Key='WeixinDirectIs'
select @weixinlogin= COUNT(*) from SystemPara where Sys_Key='WeixinLoginIsUse'
if @weixindir>0
begin
	update thirdpartylogin set [Tl_IsRegister]=(select  case when Sys_Value='True' then 1 else 0 end from SystemPara where Sys_Key='WeixinDirectIs') 
	where tl_tag='weixin'
end
if @weixinlogin>0
begin	
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='WeixinLoginIsUse') 
	where tl_tag='weixin'
end



declare @json nvarchar(1000),@pubappid nvarchar(200),@secret nvarchar(200),@returl nvarchar(200)
select  @pubappid= Sys_Value from SystemPara where Sys_Key='WeixinpubAPPID'
select  @secret= Sys_Value from SystemPara where Sys_Key='WeixinpubSecret'
select  @returl= Sys_Value from SystemPara where Sys_Key='WeixinpubReturl'
print @pubappid

set @json='{"pubAppid":"'+@pubappid+'","pubSecret":"'+@secret+'","pubReturl":"'+@returl+'"}'
print @json
update thirdpartylogin set [Tl_Config]=@json where tl_tag='weixin'

/**/
