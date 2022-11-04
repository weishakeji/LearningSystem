
/*附件表中的文件大小，由int型改为bigint*/
alter table [Accessory] ALTER COLUMN As_Size  bigint not null
go

/*章节增加编辑时间*/
alter table [LogForStudentStudy] add Lss_Complete float null
go
update [LogForStudentStudy] set Lss_Complete=ROUND(cast(Lss_StudyTime as float) *1000/cast(Lss_Duration as float) *10000,0)/100
update [LogForStudentStudy] set Lss_Complete=100 where Lss_Complete>100
go
alter table [LogForStudentStudy] ALTER COLUMN Lss_Complete float NOT NULL


--select top 10 * from [LogForStudentStudy]

