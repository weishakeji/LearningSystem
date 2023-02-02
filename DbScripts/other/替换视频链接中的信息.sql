
 /*替换视频链接地址中的信息*/
declare @old nvarchar(max),@new nvarchar(max),@sql nvarchar(max)
set @old='视频地址'		--原字符串
set @new='新视频地址'	--新字符串
set @sql='update Accessory set as_filename= replace(cast(as_filename as nvarchar(100)), '''+@old+''', '''+@new+''') where (as_type=''CourseVideo'' and as_isouter=1 and as_isother=0) and as_filename like ''%'+@old+'%'''
print @sql
exec(@sql);
 