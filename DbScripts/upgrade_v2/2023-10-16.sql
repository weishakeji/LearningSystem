

--修改机构的字段
--机构的统一社会信用代码（Unified Social Credit Identifier, USCI）
EXEC sp_rename 'Organization.Org_License', 'Org_USCI', 'COLUMN';
go
EXEC sp_rename 'Organization.Org_Extracode', 'Org_ExtraWeb', 'COLUMN';
EXEC sp_rename 'Organization.Org_LicensePic', 'Org_ExtraMobi', 'COLUMN';
--将附加代码分为web端与mobi端
ALTER TABLE Organization ALTER COLUMN Org_ExtraWeb nvarchar(max) null
ALTER TABLE Organization ALTER COLUMN Org_ExtraMobi nvarchar(max) null
go
--删除机构二维码的字段，现在采用js在前端实现了
ALTER TABLE Organization DROP COLUMN Org_QrCode;
ALTER TABLE Organization DROP COLUMN Org_QrCodeUrl;