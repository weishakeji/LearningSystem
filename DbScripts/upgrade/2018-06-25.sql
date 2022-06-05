--2018-06-25

/*增加支付接口中的应用场景项*/
alter table [PayInterface] add [Pai_Scene] [nvarchar](500) NULL
go
UPDATE [PayInterface]   SET [Pai_Scene] = 'alipay,h5' WHERE [Pai_Pattern]='支付宝手机支付'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'alipay,native' WHERE [Pai_Pattern]='支付宝网页直付'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,public' WHERE [Pai_Pattern]='微信公众号支付'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,native' WHERE [Pai_Pattern]='微信扫码支付'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,mini' WHERE [Pai_Pattern]='微信小程序支付'
GO
UPDATE [PayInterface]   SET [Pai_Scene] = 'weixin,h5' WHERE [Pai_Pattern]='微信Html5支付'
GO