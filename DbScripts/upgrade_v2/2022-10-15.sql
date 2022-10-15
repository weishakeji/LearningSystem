/*É¾³ýÒ»Ð©ÈßÓà±í*/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressList]') AND type in (N'U'))
DROP TABLE [dbo].[AddressList]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddressSort]') AND type in (N'U'))
DROP TABLE [dbo].[AddressSort]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Download]') AND type in (N'U'))
DROP TABLE [dbo].[Download]
GO