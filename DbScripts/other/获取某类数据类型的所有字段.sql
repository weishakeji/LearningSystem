
/*获取所有表的主键数据类型
	用于判断哪些表的主键不是长整型
*/

-- 采用游标遍历所有表
declare cursor_obj  cursor scroll
for select name from sysobjects where type='U' order by name 
open cursor_obj
declare @tbname nvarchar(500),@coldetails VARCHAR(2000),@primary nvarchar(500),@type nvarchar(500), @count int,@tatol int,@exist bit
select @count=0,@tatol=0,@exist=1
fetch First from cursor_obj into @tbname
while @@fetch_status=0  
 begin    
	--print @tbname + ':'
	set @coldetails='';
	set @exist=1
	
		--嵌套游标 -- start
		DECLARE inner_cursor CURSOR FOR
			SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
			FROM INFORMATION_SCHEMA.COLUMNS
			WHERE TABLE_NAME = @tbname
		OPEN inner_cursor;
		DECLARE @colname VARCHAR(100),@coltype VARCHAR(50),@colen int
		FETCH NEXT FROM inner_cursor INTO @colname, @coltype,@colen
		while @@fetch_status=0  
			begin    
				--字段类型的判断
				IF @coltype='nvarchar' and @colen=-1
				BEGIN
					set @coldetails=@coldetails + @colname+','
					set @exist=0
					set @tatol=@tatol+1;
				END	
				fetch next from inner_cursor into @colname, @coltype,@colen	
			end
		close inner_cursor
		deallocate inner_cursor					
		--嵌套游标 --end
	IF @exist=0
	BEGIN
		set @count=@count+1
		print CONVERT(nvarchar(20),@count)+'. '+@tbname + ':  ' + @coldetails
	end
	
   fetch next from cursor_obj into @tbname  --移动游标
 end   
 --print @tatol
--关闭并释放游标
close cursor_obj
deallocate cursor_obj

print '有 '+CONVERT(nvarchar(20),@count)+' 个表包含满足条件的字段，字段总数： '+CONVERT(nvarchar(20),@tatol)