

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

--图改系统菜单的图标项
EXEC sp_rename 'ManageMenu.MM_IcoS', 'MM_IcoCode', 'COLUMN';
ALTER TABLE ManageMenu DROP COLUMN MM_IcoB;
go
alter table [ManageMenu] add MM_IcoSize int NULL
go
update [ManageMenu] set MM_IcoSize=0
go
alter table [ManageMenu] ALTER COLUMN MM_IcoSize int not null

go
alter table [ManageMenu] add MM_IcoColor  nvarchar(100) null
go
ALTER TABLE [ManageMenu] ALTER COLUMN MM_IcoCode nvarchar(50) null

--将附件中的flv视频，改为mp4，flv已经不再使用
 Update Accessory Set as_filename=replace(as_filename,'.flv','.mp4') 
 where as_type='CourseVideo' and as_filename like '%.flv'
 --课程管理，增加课程类型的字段
 alter table [Course] add MM_IcoSize int NULL
go
update [Course] set MM_IcoSize=0
go
alter table [Course] ALTER COLUMN MM_IcoSize int not null