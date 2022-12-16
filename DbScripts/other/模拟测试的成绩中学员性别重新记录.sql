
/*模拟测试的成绩中学员性别重新记录，之前没有记录*/
declare cursor_obj  cursor scroll
for select Ac_ID,Ac_Sex from Accounts
open cursor_obj
declare @sex int,@acid int
fetch First from cursor_obj into @acid,@sex
while @@fetch_status=0  
 begin    
   Update TestResults Set St_Sex=@sex Where Ac_ID=@acid
   fetch next from cursor_obj into @acid,@sex
 end
close cursor_obj
deallocate cursor_obj