---2018-12-21

/*修正课程视频课程字段的长度*/
alter table [Outline] ALTER COLUMN  Ol_Courseware [nvarchar](max) NULL
go
alter table [Outline] ALTER COLUMN  Ol_Vedio [nvarchar](max) NULL
go
alter table [Outline] ALTER COLUMN  Ol_LessonPlan [nvarchar](max) NULL
go
/*修正字段名（以前单词拼错了）*/
execute sp_rename 'Outline.Ol_Vedio','Ol_Video'
go
/*增加章节节点的字段，如果有下级章节，当前章节为节点*/
alter table [Outline] add Ol_IsNode bit NULL
go
UPDATE [Outline]  SET Ol_IsNode = 0
GO
alter table [Outline] ALTER COLUMN Ol_IsNode bit NOT NULL
go
/*增加章节点是否有视频的字段*/
alter table [Outline] add Ol_IsVideo bit NULL
go
UPDATE [Outline]  SET Ol_IsVideo = 0
GO
alter table [Outline] ALTER COLUMN Ol_IsVideo bit NOT NULL
go
/*如果章节有下级章节，则当前章节为章节节点，不再作为学习章节*/
declare cursor_obj  cursor scroll
for select ol_id,ol_name from outline
open cursor_obj
declare @olid int, @olname nvarchar(500), @child int
set @child=0
fetch First from cursor_obj into @olid,@olname
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   select @child=COUNT(*) from outline where Ol_PID=@olid
   if @child>0
   begin
	--如果有下级
	--print convert(varchar(20),@olid)+' '+@olname +' 下级数量' +convert(varchar(20),@child)	
	Update outline Set Ol_IsNode=1 Where ol_id=@olid  --修改当前行
   end
   else
   begin
	--没有下级章节
	Update outline Set Ol_IsNode=0 Where ol_id=@olid  --修改当前行
   end
   
   fetch next from cursor_obj into @olid, @olname   --移动游标
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj

/*如果章节有视频，则作为视频章节*/
declare cursor_obj  cursor scroll
for select ol_id,ol_uid from outline
open cursor_obj
declare @olid2 int, @uid nvarchar(500), @child2 int
set @child2=0
fetch First from cursor_obj into @olid2,@uid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   select @child2=COUNT(*) from Accessory where As_Type='CourseVideo' and  As_Uid=@uid 
   if @child2>0
   begin
	--如果有视频
	--print convert(varchar(20),@olid2)+' '+@uid +' 视频数：' +convert(varchar(20),@child2)	
	Update outline Set Ol_IsVideo=1 Where ol_id=@olid2  --修改当前行
   end
   else
   begin
	--没有视频
	--print convert(varchar(20),@olid2)+' '+@uid +'  ' +convert(varchar(20),@child2)	
	Update outline Set Ol_IsVideo=0 Where ol_id=@olid2  --修改当前行
  	DELETE FROM [LogForStudentStudy] where ol_id=@olid2 
   end
   
   fetch next from cursor_obj into @olid2, @uid   --移动游标
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj
/**/
/*如果父章节不存在，则删除该章节*/
declare cursor_obj  cursor scroll
for select ol_id,ol_name,ol_pid from outline where Ol_PID!=0 
open cursor_obj
declare @olid3 int, @name nvarchar(500),@pid int, @child3 int, @num int
set @child3=0
set @num=0
fetch First from cursor_obj into @olid3,@name,@pid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
   select @child3=COUNT(*) from outline where Ol_ID=@pid 
   if @child3<1
   begin
	--如果父章节不存在，则删除该章节
	--print convert(varchar(20),@olid)+ @name +convert(varchar(20),@child2)	
	DELETE FROM [LogForStudentStudy] where ol_id=@olid3 
	DELETE FROM outline where ol_id=@olid3 
	select @num=@num+1
   end   
   
   fetch next from cursor_obj into @olid3, @name,@pid   --移动游标
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj

print @num