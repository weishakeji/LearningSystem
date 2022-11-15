/***********
课程指南id转为雪花id*/

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Guide]') AND name = N'aaaaaGuide_PK')
ALTER TABLE Guide DROP CONSTRAINT [aaaaaGuide_PK]
GO
declare @dfname nvarchar(500), @sql nvarchar(500)
SELECT @dfname = idx.name  FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
	where idx.type='d' and idx.name like '%Guide%'
set @sql='alter table [Guide] drop constraint '+@dfname   
   exec sp_executesql @sql	
go
ALTER TABLE [Guide] ADD Gu_SID bigint default 0 not null
go
update [Guide]  set Gu_SID=Gu_Id
alter table [Guide] drop column Gu_Id
execute sp_rename '[Guide].Gu_SID','Gu_ID'
go
alter table [Guide] add primary key ( Gu_ID );
go
