
/***********
学员id转为雪花id*/
ALTER TABLE [Accounts] ADD [Ac_SID] bigint default 0 not null
go
update Accounts  set [Ac_SID]=Ac_ID
/*删除原有主键，设置新的主建*/
declare cursor_acc  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Accounts'
open cursor_acc
declare @accpk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_acc into @accpk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Accounts] drop constraint '+@accpk   
   exec sp_executesql @sql
   fetch next from cursor_acc into @accpk 
 end   
--关闭并释放游标
close cursor_acc
deallocate cursor_acc
go
alter table [Accounts] drop column Ac_ID
execute sp_rename '[Accounts].Ac_SID','Ac_ID'
go
alter table [Accounts] add primary key ( Ac_ID );
/*账号的父id，即推荐人id，转为长整型*/
alter table [Accounts] ALTER COLUMN Ac_PID  bigint not null
/*账号关联表中的字段Ac_ID，转长整型*/
alter table [LearningCard] ALTER COLUMN Ac_ID  bigint not null
alter table [TestResults] ALTER COLUMN Ac_ID  bigint not null
alter table [TeacherComment] ALTER COLUMN Ac_ID  bigint not null
alter table [ExamResults] ALTER COLUMN Ac_ID  bigint not null
alter table [Teacher] ALTER COLUMN Ac_ID  bigint not null

alter table [Student_Ques] ALTER COLUMN Ac_ID  bigint not null
alter table [Student_Notes] ALTER COLUMN Ac_ID  bigint not null
alter table [Student_Course] ALTER COLUMN Ac_ID  bigint not null
alter table [Student_Collect] ALTER COLUMN Ac_ID  bigint not null
alter table [CouponAccount] ALTER COLUMN Ac_ID  bigint not null

alter table [RechargeCode] ALTER COLUMN Ac_ID  bigint not null
alter table [PointAccount] ALTER COLUMN Ac_ID  bigint not null
alter table [MoneyAccount] ALTER COLUMN Ac_ID  bigint not null
alter table [MessageBoard] ALTER COLUMN Ac_ID  bigint not null
alter table [Message] ALTER COLUMN Ac_ID  bigint not null

alter table [LogForStudentStudy] ALTER COLUMN Ac_ID  bigint not null
alter table [LogForStudentQuestions] ALTER COLUMN Ac_ID  bigint not null
alter table [LogForStudentOnline] ALTER COLUMN Ac_ID  bigint not null
alter table [LogForStudentExercise] ALTER COLUMN Ac_ID  bigint not null


/***********
教师id转为雪花id*/
ALTER TABLE [Teacher] ADD [Th_SID] bigint default 0 not null
go
update Teacher  set [Th_SID]=Th_ID
/*删除原有主键，设置新的主建*/
declare cursor_teach  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Teacher'
open cursor_acc
declare @thpk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_teach into @thpk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Teacher] drop constraint '+@thpk   
   exec sp_executesql @sql
   fetch next from cursor_teach into @thpk 
 end   
--关闭并释放游标
close cursor_teach
deallocate cursor_teach
go
alter table [Teacher] drop column Th_ID
execute sp_rename '[Teacher].Th_SID','Th_ID'
go
alter table [Teacher] add primary key ( Th_ID );
/*教师关联表中的字段Th_ID，转长整型*/
alter table [Knowledge] ALTER COLUMN Ac_ID  bigint not null
alter table [TestPaper] ALTER COLUMN Ac_ID  bigint not null
alter table [TeacherHistory] ALTER COLUMN Ac_ID  bigint not null
alter table [TeacherComment] ALTER COLUMN Ac_ID  bigint not null
alter table [Teacher_Course] ALTER COLUMN Ac_ID  bigint not null

alter table [Examination] ALTER COLUMN Ac_ID  bigint not null
alter table [Course] ALTER COLUMN Ac_ID  bigint not null
alter table [MessageBoard] ALTER COLUMN Ac_ID  bigint not null

