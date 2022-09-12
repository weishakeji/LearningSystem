/*增加试卷所属的课程名称*/
alter table TestPaper add [Cou_Name] [nvarchar](100) NULL
go
/*试卷中的课程名称与课程表同步*/
declare cur_couid cursor scroll
for select cou_id from TestPaper group by cou_id
open cur_couid
declare @couid int,@couname nvarchar(100)
fetch First from cur_couid into @couid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   select @couname=cou_name from Course where Cou_ID=@couid
   Update TestPaper Set [Cou_Name]=@couname Where Cou_ID=@couid  --修改当前行
   fetch next from cur_couid into @couid   --移动游标
 end   
--关闭并释放游标
close cur_couid
deallocate cur_couid