/*删除一些重复的学习记录*/
declare @num int
set @num=0
declare cur_acc cursor scroll
for select Ac_ID,ol_id  from LogForStudentStudy
open cur_acc
declare @acid int,@ol_id bigint,@count int,@lssid int
select @num=0
fetch First from cur_acc into @acid,@ol_id
while @@fetch_status=0  
 begin
   select @count=COUNT(*) from LogForStudentStudy where Ac_ID=@acid and Ol_ID=@ol_id
   if @count>1
   begin
		select @num=@num+1
		set @lssid= (select top 1 Lss_ID from LogForStudentStudy  where Ac_ID=@acid and Ol_ID=@ol_id order by Lss_CrtTime desc)
		DELETE FROM LogForStudentStudy where Lss_ID=@lssid
		print @count
   end  
   --Update TestPaper Set [Cou_Name]=@couname Where Cou_ID=@couid  
   fetch next from cur_acc into @acid,@ol_id
 end   
--关闭并释放游标
close cur_acc
deallocate cur_acc

print @num