--2017-08-21
/*课程排序，tax序号相同的处理*/
declare cur_couid cursor scroll
for select cou_id from Course order by cou_tax asc,Cou_CrtTime asc
open cur_couid
declare @couid int,@tax int
set @tax=0
fetch First from cur_couid into @couid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   set @tax=@tax+1
   Update Course Set [Cou_tax]=@tax Where Cou_ID=@couid  --修改当前行
   --print @tax
   fetch next from cur_couid into @couid   --移动游标
 end   
--关闭并释放游标
close cur_couid
deallocate cur_couid