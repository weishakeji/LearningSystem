/*
	将Sqlserver的索引，生成到PostgreSQL
	
	编写思路：
	利用游标遍历Sqlserver中的索引，生成在PostgreSQL中可执行的创建索引的sql脚本
	
	操作步聚：
	1、在Sqlserver中执行下述Sql脚本
	2、在“消息”框中复制创建索引的Sql脚本
	3、将创建索引的脚本，粘贴到PostgreSql中执行即可
	
*/

-- 生成Postgresql的索引创建语句
CREATE FUNCTION dbo.get_index_sql
(
    @tablename nvarchar(100),		--表名
    @indexname nvarchar(200),		--索引名
    @indexcount int					--同名的有几个索引，如果是复合索引会大于1
)
RETURNS nvarchar(1000)
AS
BEGIN
    DECLARE @result nvarchar(1000);
    DECLARE @descending int;	--排序方式,0为升序，1为降序
    DECLARE @columnName nvarchar(100);	--索引的列
    DECLARE @descending_key NVARCHAR(10);
	DECLARE @num int;		--临时计数
    --set @result=@indexname +' : '+ CONVERT(nvarchar(20),@indexcount)
    set @num=0;
    
    if @indexcount<=1
    BEGIN	--如果是单独的索引
		SELECT  @descending=ic.is_descending_key,
				@columnName=c.name
					FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) = @tablename and i.name=@indexname
		IF @descending=1
		BEGIN
			set @descending_key='ASC'					
		END ELSE IF @descending=0
		BEGIN
			set @descending_key='DESC'
		END
		set @result='CREATE INDEX IF NOT EXISTS "'+@tablename+'_'+@indexname+'" ON "'+@tablename+'" ("'+@columnName+'" '+@descending_key+');'
	END ELSE	
	BEGIN  --以下是复合索引
		
			set @result='CREATE INDEX IF NOT EXISTS "'+@tablename+'_'+@indexname+'" ON "'+@tablename+'" (';			
			
			DECLARE inner_cursor CURSOR FOR
			select ic.is_descending_key, c.name FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) = @tablename and i.name=@indexname
			OPEN inner_cursor;
			--DECLARE @indexname NVARCHAR(160),@indexCount int
			FETCH NEXT FROM inner_cursor INTO @descending, @columnName
			while @@fetch_status=0  
				begin    
					set @num=@num+1;					
					IF @descending=1
					BEGIN
						set @descending_key='ASC'					
					END ELSE IF @descending=0
					BEGIN
						set @descending_key='DESC'
					END
					set @result=@result+'"'+@columnName +'" '+@descending_key
					IF @num<@indexcount
					BEGIN
						set @result=@result +','				
					END
					fetch next from inner_cursor into  @descending, @columnName
				end
			close inner_cursor			
			deallocate inner_cursor	
			set @result=@result +');'
	END
    RETURN @result;
END
go

--遍历索引，生成PostgreSql的创建索引的脚本，包括了复合索引
DECLARE  cursor_obj CURSOR SCROLL 
	FOR select name from sysobjects where type='U' order by name
open cursor_obj
DECLARE  @tablename nvarchar(500),@count int,@tatol int,@sql nvarchar(1000)
select @count=0,@tatol=0
FETCH First from cursor_obj into @tablename
WHILE @@fetch_status=0  
 BEGIN     
	 set @count=@count+1;
     print '-- '+CONVERT(nvarchar(20),@count) + ' . '+ @tablename
		--嵌套游标 -- start
		DECLARE inner_cursor CURSOR FOR
					SELECT 
						i.name AS IndexName,COUNT(ic.index_column_id) as 'count'
					FROM 
						sys.indexes i
					JOIN 
						sys.index_columns ic ON i.index_id = ic.index_id AND i.object_id = ic.object_id
					JOIN 
						sys.columns c ON ic.column_id = c.column_id AND ic.object_id = c.object_id
					WHERE 
						i.type_desc='NONCLUSTERED'  and OBJECT_NAME(i.object_id) =  @tablename
					GROUP BY 
						i.object_id, i.name, i.type_desc
		OPEN inner_cursor;
		DECLARE @indexName NVARCHAR(160),@indexCount int
		FETCH NEXT FROM inner_cursor INTO @indexName, @indexCount
		while @@fetch_status=0  
			begin    
				set @tatol=@tatol+1;
				select @sql= dbo.get_index_sql(@tablename,@indexName,@indexCount);
				print @sql
				
				fetch next from inner_cursor into  @indexName,@indexCount
			end
		close inner_cursor
		deallocate inner_cursor					
		--嵌套游标 --end
   
   FETCH next from cursor_obj into @tablename  --移动游标
 END   
 print '--总共索引有：'+CONVERT(nvarchar(20),@tatol) 
--关闭并释放游标
CLOSE cursor_obj
DEALLOCATE cursor_obj

go
DROP FUNCTION dbo.get_index_sql;
