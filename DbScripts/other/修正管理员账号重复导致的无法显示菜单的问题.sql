
/*修改由1.0升到2.0时，管理员账号重复导致的无法显示菜单的问题*/


/*将根机构的账号全部设置成管理员（即超管）*/
declare @root_orgid int,@posid int
select @root_orgid= Org_ID from Organization where Org_IsRoot=1
select @posid= Posi_Id from Position where Org_ID=@root_orgid and Posi_IsAdmin=1
update EmpAccount set Posi_Id=@posid  where  Org_ID=@root_orgid
/*修正重复的管理员账号，如果重复账号名+序号*/
declare cursor_obj  cursor scroll
for select ROW_NUMBER() over(order by acc_id) as 'row',n.Acc_AccName,emp.Acc_Id,emp.Org_ID from
	 (select Acc_AccName,COUNT(*) as 'num' from EmpAccount  group by Acc_AccName) as n
	inner join
	(select * from EmpAccount) as emp
	on n.Acc_AccName=emp.Acc_AccName where num>1
open cursor_obj
declare @accname nvarchar(500),@accid int,@row int,@orgid int

fetch First from cursor_obj into @row,@accname,@accid,@orgid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin 
	--print @accname
    if @root_orgid!=@orgid
    begin
		update EmpAccount set Acc_AccName+=CAST(@row as nvarchar(50))  where Acc_Id=@accid
		print @accname
		print @accid
    end
   
   fetch next from cursor_obj into @row,@accname,@accid,@orgid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj