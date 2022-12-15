
/*将考试成绩中的学员姓名重新生成，为了修正乱码*/
declare cursor_obj  cursor scroll
for select Ac_ID,Ac_Name from Accounts
open cursor_obj
declare @name nvarchar(500),@acid int
fetch First from cursor_obj into @acid,@name
while @@fetch_status=0  
 begin    
   Update ExamResults Set Ac_Name=@name Where Ac_ID=@acid
   fetch next from cursor_obj into @acid,@name
 end
close cursor_obj
deallocate cursor_obj