--2018-11-9
/*增加试题中的答题错误数*/
alter table Questions add Qus_Errornum int NULL
go
UPDATE Questions  SET Qus_Errornum = 0
GO
alter table Questions ALTER COLUMN Qus_Errornum [int] NOT NULL
go
/*增加章节是否完结的字段*/
alter table Outline add Ol_IsFinish bit NULL
go
UPDATE Outline  SET Ol_IsFinish = 1
GO
alter table Outline ALTER COLUMN Ol_IsFinish bit NOT NULL
go
/*增加课程限时免费的字段*/
alter table Course add Cou_IsLimitFree bit NULL
go
UPDATE Course  SET Cou_IsLimitFree = 0
GO
alter table Course ALTER COLUMN Cou_IsLimitFree bit NOT NULL
go
alter table Course add Cou_FreeStart datetime NULL
go
UPDATE Course  SET Cou_FreeStart = GETDATE()
GO
alter table Course ALTER COLUMN Cou_FreeStart datetime NOT NULL
go
alter table Course add Cou_FreeEnd datetime NULL
go
UPDATE Course  SET Cou_FreeEnd = GETDATE()
GO
alter table Course ALTER COLUMN Cou_FreeEnd datetime NOT NULL
go
/*视频名称的字段，调整到3000字符*/
alter table Accessory ALTER COLUMN  As_FileName nvarchar(3000);