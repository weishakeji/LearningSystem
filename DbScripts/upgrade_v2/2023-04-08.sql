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
VALUES ('QqOpenID','QQ',1,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('WeixinOpenID','微信',2,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('Jindie','金蝶',3,1,1,'','','','')
GO
INSERT INTO [ThirdpartyLogin]
  ([Tl_Tag],[Tl_Name],[Tl_Tax],[Tl_IsUse],[Tl_IsRegister],[Tl_APPID]
   ,[Tl_Secret],[Tl_Returl],[Tl_Config])
VALUES ('ZzGongshang','郑州工商',4,1,1,'','','','')
GO
/*qq登录由SystemPara转到thirdpartylogin表*/
update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='QQAPPID') where tl_tag='QqOpenID'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='QQAPPKey') where tl_tag='QqOpenID'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='QQReturl') where tl_tag='QqOpenID'
go
declare @qqdir int,@qqlogin int
select @qqdir= COUNT(*) from SystemPara where Sys_Key='QQDirectIs'
select @qqlogin= COUNT(*) from SystemPara where Sys_Key='QQLoginIsUse'
if @qqdir>0
begin
	update thirdpartylogin set [Tl_IsRegister]=(select  case when Sys_Value='True' then 1 else 0 end from SystemPara where Sys_Key='QQDirectIs') 
	where tl_tag='QqOpenID'
end
if @qqlogin>0
begin
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='QQLoginIsUse') 
	where tl_tag='qqQqOpenID'
end

/*云之家由SystemPara转到thirdpartylogin表*/
update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAppid') where tl_tag='Jindie'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAppSecret') where tl_tag='Jindie'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaDomain') where tl_tag='Jindie'
update thirdpartylogin set [Tl_Account]=(select Sys_Value from SystemPara where Sys_Key='YunzhijiaAcc') where tl_tag='Jindie'
go
declare @yunzhijiao int
select @yunzhijiao= COUNT(*) from SystemPara where Sys_Key='YunzhijiaLoginIsuse'
if @yunzhijiao>0
begin
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='YunzhijiaLoginIsuse') 
	where tl_tag='Jindie'
end


/*微信登录由SystemPara转到thirdpartylogin表*/
--select * from SystemPara where Sys_Key like 'weixin%'

update thirdpartylogin set tl_appid=(select Sys_Value from SystemPara where Sys_Key='WeixinAPPID') where tl_tag='WeixinOpenID'
update thirdpartylogin set Tl_Secret=(select Sys_Value from SystemPara where Sys_Key='WeixinSecret') where tl_tag='WeixinOpenID'
update thirdpartylogin set Tl_Returl=(select Sys_Value from SystemPara where Sys_Key='WeixinReturl') where tl_tag='WeixinOpenID'
go
declare @weixindir int,@weixinlogin int
select @weixindir= COUNT(*) from SystemPara where Sys_Key='WeixinDirectIs'
select @weixinlogin= COUNT(*) from SystemPara where Sys_Key='WeixinLoginIsUse'
if @weixindir>0
begin
	update thirdpartylogin set [Tl_IsRegister]=(select  case when Sys_Value='True' then 1 else 0 end from SystemPara where Sys_Key='WeixinDirectIs') 
	where tl_tag='WeixinOpenID'
end
if @weixinlogin>0
begin	
	update thirdpartylogin set [Tl_IsUse]=(select case when Sys_Value='True' then 1 else 0 end  from SystemPara where Sys_Key='WeixinLoginIsUse') 
	where tl_tag='WeixinOpenID'
end



declare @json nvarchar(1000),@pubappid nvarchar(200),@secret nvarchar(200),@returl nvarchar(200)
select  @pubappid= Sys_Value from SystemPara where Sys_Key='WeixinpubAPPID'
select  @secret= Sys_Value from SystemPara where Sys_Key='WeixinpubSecret'
select  @returl= Sys_Value from SystemPara where Sys_Key='WeixinpubReturl'
print @pubappid

set @json='{"pubAppid":"'+@pubappid+'","pubSecret":"'+@secret+'","pubReturl":"'+@returl+'"}'
print @json
update thirdpartylogin set [Tl_Config]=@json where tl_tag='WeixinOpenID'

/**/
/*增加学员账号的第三登录*/
alter table Accounts add Ac_ZzGongshang nvarchar(100)  NULL
go
alter table Accounts add Ac_QiyeWeixin nvarchar(100)  NULL
go
alter table Accounts add Ac_Zhifubao nvarchar(100)  NULL
go
alter table Accounts add Ac_Dingding nvarchar(100)  NULL
go
alter table Accounts add Ac_Jindie nvarchar(100)  NULL
go

/*删除课程公告里的临时ID字段*/
ALTER TABLE Guide DROP COLUMN Gu_SID
go
/*记录第三方登录的绑定信息*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ThirdpartyAccounts](
	[Ta_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ta_NickName] [nvarchar](255) NULL,
	[Ta_Headimgurl]  [nvarchar](500) NULL,
	[Ta_Openid] [nvarchar](255) NULL,
	[Ta_Tag] [nvarchar](100) NULL,
	[Ac_ID] [bigint] not NULL,	
 CONSTRAINT [aaaaaThirdpartyAccounts_PK] PRIMARY KEY NONCLUSTERED 
(
	[Ta_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO