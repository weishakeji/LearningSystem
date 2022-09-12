/*增加学习卡的时间跨度*/
alter table LearningCard add Lc_Span int NULL
go
UPDATE LearningCard  SET Lc_Span = 0
GO
alter table LearningCard ALTER COLUMN Lc_Span [int] NOT NULL
go