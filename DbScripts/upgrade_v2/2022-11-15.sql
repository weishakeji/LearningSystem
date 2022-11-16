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

/***********
知识点id转为雪花id*/

--删除外键约束
declare cursor_drop  cursor scroll
for SELECT idx.name as 'df',tab.name as 'tb' FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
	where idx.type='d' and idx.name like '%Knowledge%'
open cursor_drop
declare @df nvarchar(500), @tb nvarchar(500),@sql nvarchar(500)
fetch First from cursor_drop into @df,@tb
while @@fetch_status=0  
 begin  
   set @sql='alter table ['+@tb+'] drop constraint '+@df   
   exec sp_executesql @sql
   fetch next from cursor_drop into  @df,@tb
 end   
close cursor_drop
deallocate cursor_drop


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[Knowledge]') AND name = N'aaaaaKnowledge_PK')
ALTER TABLE [Knowledge] DROP CONSTRAINT [aaaaaKnowledge_PK]
GO
ALTER TABLE [Knowledge] ADD Kn_SID bigint default 0 not null
go
update [Knowledge]  set Kn_SID=Kn_ID
go
alter table [Knowledge] drop column Kn_ID
execute sp_rename '[Knowledge].Kn_SID','Kn_ID'
go
alter table [Knowledge] add primary key ( Kn_ID );
go

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[KnowledgeSort]') AND name = N'aaaaaKnowledgeSort_PK')
ALTER TABLE KnowledgeSort DROP CONSTRAINT [aaaaaKnowledgeSort_PK]
GO
ALTER TABLE KnowledgeSort ADD Kns_SID bigint default 0 not null
go
update KnowledgeSort  set Kns_SID=Kns_ID
alter table KnowledgeSort drop column Kns_ID
execute sp_rename '[KnowledgeSort].Kns_SID','Kns_ID'
go
alter table KnowledgeSort add primary key ( Kns_ID );
go


--为知识点增加与知识分类的id关联，以前是靠udi
ALTER TABLE [Knowledge] ADD Kns_ID bigint default 0 not null
go

ALTER TABLE KnowledgeSort ADD Kns_SPID bigint default 0 not null
go
declare cursor_obj  cursor scroll
for SELECT kns_uid,kns_id FROM KnowledgeSort
open cursor_obj
declare @knsid bigint, @knsuid nvarchar(100)
fetch First from cursor_obj into @knsuid,@knsid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin	
	print @knsuid + @knsuid
	Update [Knowledge] Set Kns_ID=@knsid Where Kns_Uid=@knsuid		
	Update [KnowledgeSort] Set Kns_SPID=@knsid Where Kns_PID=@knsuid		
	fetch next from cursor_obj into @knsuid,@knsid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj
alter table [KnowledgeSort] drop column Kns_PID
execute sp_rename '[KnowledgeSort].Kns_SPID','Kns_PID'

alter table KnowledgeSort drop column Kns_UID
go
alter table KnowledgeSort drop column Kns_PID
go
alter table Knowledge drop column Kns_UID
go
--alter table Knowledge drop column Kn_Uid

/*新闻文章的id转雪花id*/
ALTER TABLE [Article] ADD Art_SId bigint default 0 not null
go
update [Article]  set Art_SId=Art_Id
/*删除原有主键，设置新的主建*/
declare cursor_ol  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Article'
open cursor_ol
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_ol into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Article] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_ol into @pk 
 end   
--关闭并释放游标
close cursor_ol
deallocate cursor_ol
go
alter table [Article] drop column Art_ID
execute sp_rename '[Article].Art_SId','Art_ID'
go
alter table [Article] add primary key ( Art_ID );

alter table Special_Article ALTER COLUMN Art_ID  bigint not null
alter table NewsNote ALTER COLUMN Art_ID  bigint not null
