/*修改单点登录配置项的启用状态字段为不可为空*/
alter table SingleSignOn ALTER COLUMN 
SSO_IsUse bit NOT NULL
go
/*单点登录配置项的名称长度设置长一些*/
alter table SingleSignOn ALTER COLUMN 
SSO_Name nvarchar(100)
go
/*单点登录配置项增加是否允许创建学员组*/
alter table SingleSignOn add SSO_IsAddSort [bit] default 0 NULL
go



