/*导航菜单，增加uid，修改pid*/
alter table Navigation add Nav_UID   nvarchar(255)
go
UPDATE Navigation SET Nav_UID  = Nav_ID where Nav_UID  is null
go
alter  table Navigation ALTER COLUMN Nav_PID  nvarchar(255)
go
UPDATE Navigation SET Nav_PID  = '' where Nav_PID='0'
go
alter table Navigation add Nav_Icon  nvarchar(50)