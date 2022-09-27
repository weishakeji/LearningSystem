declare cursor_obj  cursor scroll
for select name from sysobjects where type='U'
open cursor_obj
declare @name nvarchar(500),@count int,@tatol int 
select @count=0,@tatol=0
fetch First from cursor_obj into @name
while @@fetch_status=0  
 begin    
    select @count=COUNT(*) from syscolumns Where ID=OBJECT_ID(@name) and name='Cou_UID'
    if @count>0
	   begin
		 set @tatol=@tatol+1;
		 print @name
	   end
   fetch next from cursor_obj into @name  --移动游标
 end   
 print @tatol
--关闭并释放游标
close cursor_obj
deallocate cursor_obj

/*查询外键*/
SELECT idx.*,idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) 
where tab.name='TestPaper'

/*查询约束与外键*/
SELECT idx.* FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
where tab.name='TestPaper'

SELECT idx.name as 'df',tab.name as 'tb' FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
where idx.type='d'

SELECT * FROM dbo.sysobjects WHERE type = 'D' and id='213575799'

select * from sys.tables 

SELECT * FROM dbo.sysobjects where parent_obj='213575799'

