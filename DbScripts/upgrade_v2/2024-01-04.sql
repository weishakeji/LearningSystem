
/*增加学员在线记录相关的地理信息*/
--省，市，区县，
alter table [LogForStudentOnline] add Lso_Province [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_City [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_District [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Source [nvarchar](255) NULL
go
alter table [LogForStudentOnline] add Lso_Info [nvarchar](255) NULL
go
--行政区划编码
alter table [LogForStudentOnline] add Lso_Code int NULL
go
update [LogForStudentOnline] set Lso_Code=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Code int NOT NULL
go
--经度
alter table [LogForStudentOnline] add Lso_Longitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Longitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Longitude decimal(20, 15) NOT NULL
go
--纬度
alter table [LogForStudentOnline] add Lso_Latitude decimal(20, 15) NULL
go
update [LogForStudentOnline] set Lso_Latitude=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_Latitude decimal(20, 15) NOT NULL
go
--数据获取方式（GPS，IP），默认为0，IP方式为1
alter table [LogForStudentOnline] add Lso_GeogType [int] NULL
go
update [LogForStudentOnline] set Lso_GeogType=0
go
alter table [LogForStudentOnline] ALTER COLUMN Lso_GeogType [int] NOT NULL



/*试题练习记录，增加地理信息字段*/
alter table LogForStudentExercise drop column Lse_UID
go
alter table LogForStudentExercise add Lse_GeogData nvarchar(max) NULL
go
/*视频学习记录，增加地理信息字段*/
alter table LogForStudentStudy add Lss_GeogData nvarchar(max) NULL
go

alter table [Columns] ALTER COLUMN Col_IsChildren [bit] NOT NULL