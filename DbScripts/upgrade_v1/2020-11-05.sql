/****** 
2020-11-05
******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

go
--弹窗所在页面
alter table Notice add  No_Page nvarchar(200) NULL
go
--时段，每天的几点到几点，不同于起始时间
alter table Notice add  No_Interval  nvarchar(2000) NULL
go
--受众范围，0为所有，1为登录学员，2为未登录学员
alter table Notice add  No_Range   int NULL
go
UPDATE Notice SET No_Range  = 0
GO
alter table Notice ALTER COLUMN No_Range   int NOT NULL
go
--每天弹出几次
alter table Notice add  No_OpenCount  int NULL
go
UPDATE Notice SET No_OpenCount  = 0
GO
alter table Notice ALTER COLUMN No_OpenCount  int NOT NULL
go
--宽高
alter table Notice add  No_Width  int NULL
go
UPDATE Notice SET No_Width  = 0
GO
alter table Notice ALTER COLUMN No_Width  int NOT NULL
go
alter table Notice add  No_Height  int NULL
go
UPDATE Notice SET No_Height  = 0
GO
alter table Notice ALTER COLUMN No_Height  int NOT NULL
go
--弹窗背景图片
alter table Notice add  No_BgImage  [nvarchar](max) NULL
go
--链接地址，如果为空，则跳转到通知页
alter table Notice add  No_Linkurl  nvarchar(2000) NULL
go
--与学员分组的关联信息
alter table Notice add  No_StudentSort  nvarchar(2000) NULL
go

--弹出的时间（几秒）
alter table Notice add  No_Timespan  int NULL
go
UPDATE Notice SET No_Timespan  = 0
GO
alter table Notice ALTER COLUMN No_Timespan   int NOT NULL
go
--通知类型，1为普通通知，2为弹窗通知，3为短信
alter table Notice add  No_Type  int NULL
go
UPDATE Notice SET No_Type  = 1
GO
alter table Notice ALTER COLUMN No_Type   int NOT NULL
go
ALTER TABLE Notice drop CONSTRAINT DF__Notice__No_IsOpe__6CD828CA
go
alter table Notice drop column No_IsOpen