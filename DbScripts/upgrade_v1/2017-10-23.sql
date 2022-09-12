--2017-10-23

--2017-10-23

/*教师评价表中的机构id，设置为教师所在的机构id（由于之前的bug导致没有这一项）*/
update [TeacherComment] set Org_ID=1
go
alter table [TeacherComment] ALTER COLUMN Org_ID int NOT NULL
go
declare cur_thid cursor scroll
for select th_id from TeacherComment group by th_id
open cur_thid
declare @orgid int,@thid int
set @orgid=0
fetch First from cur_thid into @thid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   select @orgid=Org_ID from Teacher where th_ID=@thid
   Update TeacherComment Set [Org_ID]=@orgid Where th_ID=@thid  --修改当前行
   --print @orgid
   fetch next from cur_thid into @thid   --移动游标
 end   
--关闭并释放游标
close cur_thid
deallocate cur_thid