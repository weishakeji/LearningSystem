
GO
/*删除所有外键约束*/
declare cursor_drop  cursor scroll
for SELECT idx.name as 'df',tab.name as 'tb' FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
	where idx.type='d'
open cursor_drop
declare @df nvarchar(500), @tb nvarchar(500),@sql nvarchar(500)
fetch First from cursor_drop into @df,@tb
while @@fetch_status=0  
 begin  
   set @sql='alter table ['+@tb+'] drop constraint '+@df   
   exec sp_executesql @sql
   fetch next from cursor_drop into  @df,@tb
 end   
--关闭并释放游标
close cursor_drop
deallocate cursor_drop

/*删除所有非主键索引*/
declare cursor_idx cursor scroll
for SELECT idx.name, OBJECT_NAME(CAST(idx.object_id AS INT)) as 'table' FROM sys.tables tb
    INNER JOIN sys.indexes idx ON idx.object_id = tb.object_id
	WHERE tb.type = 'u' AND idx.is_unique = 0 AND idx.name IS NOT NULL 
open cursor_idx
declare @idx nvarchar(500), @tb2 nvarchar(500),@sqlidx nvarchar(500)
fetch First from cursor_idx into @idx,@tb2
while @@fetch_status=0  
 begin  
   set @sqlidx='drop index '+@idx+' on ' +@tb2 
   --print @sqlidx
   exec sp_executesql @sqlidx
   fetch next from cursor_idx into  @idx,@tb2
 end   
--关闭并释放游标
close cursor_idx
deallocate cursor_idx

/***********
章节id转为雪花id*/
ALTER TABLE [outline] ADD [Ol_SID] bigint default 0 not null
go
update outline  set Ol_SID=Ol_ID
/*删除原有主键，设置新的主建*/
declare cursor_ol  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='outline'
open cursor_ol
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_ol into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [outline] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_ol into @pk 
 end   
--关闭并释放游标
close cursor_ol
deallocate cursor_ol
go
alter table [outline] drop column Ol_ID
execute sp_rename '[outline].Ol_SID','Ol_ID'
go
alter table [outline] add primary key ( Ol_ID );
/*章节父id，转为长整型*/
alter table [outline] ALTER COLUMN Ol_PID  bigint not null
/*章节关联表中的字段Ol_ID，转长整型*/
alter table [TestPaperItem] ALTER COLUMN Ol_ID  bigint not null
alter table [Questions] ALTER COLUMN Ol_ID  bigint not null
alter table [OutlineEvent] ALTER COLUMN Ol_ID  bigint not null
alter table [Message] ALTER COLUMN Ol_ID  bigint not null
alter table [LogForStudentStudy] ALTER COLUMN Ol_ID  bigint not null
alter table [LogForStudentQuestions] ALTER COLUMN Ol_ID  bigint not null
alter table [LogForStudentExercise] ALTER COLUMN Ol_ID  bigint not null

/*****************
试题id转为雪花id*/
ALTER TABLE [Questions] ADD [Qus_SID] bigint default 0 not null
go
update Questions  set Qus_SID=Qus_ID
go
/*删除原有主键，设置新的主建*/
declare cursor_ques  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Questions'
open cursor_ques
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_ques into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Questions] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_ques into @pk 
 end   
--关闭并释放游标
close cursor_ques
deallocate cursor_ques
go
alter table [Questions] drop column Qus_ID
go
execute sp_rename '[Questions].Qus_SID','Qus_ID'
go
alter table [Questions] add primary key ( Qus_ID );
go
/*试题关联表中的字段Qus_ID，转长整型*/
alter table [Student_Ques] ALTER COLUMN Qus_ID  bigint not null
alter table [Student_Notes] ALTER COLUMN Qus_ID  bigint not null
alter table [Student_Collect] ALTER COLUMN Qus_ID  bigint not null
alter table [QuesAnswer] ALTER COLUMN Qus_ID  bigint not null
alter table [LogForStudentQuestions] ALTER COLUMN Qus_ID  bigint not null

/***************
试卷Id转为雪花id
*/
ALTER TABLE TestPaper ADD Tp_SID bigint default 0 not null
go
update TestPaper  set Tp_SId=Tp_Id
go
/*删除原有主键，设置新的主建*/
declare cursor_tp  cursor scroll
for SELECT idx.name,idx.type FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
	where tab.name='TestPaper'
open cursor_tp
declare @name nvarchar(500),@type nvarchar(5),@sql nvarchar(500)
fetch First from cursor_tp into @name,@type
while @@fetch_status=0  
 begin  
   set @sql='alter table [TestPaper] drop constraint '+@name   
   exec sp_executesql @sql
   fetch next from cursor_tp into @name,@type
 end   
--关闭并释放游标
close cursor_tp
deallocate cursor_tp
go
alter table TestPaper drop column Tp_Id
go
execute sp_rename '[TestPaper].Tp_SID','Tp_Id'
go
alter table [TestPaper] add primary key ( Tp_Id );
go
/*试卷关联表中的字段Tp_ID，转长整型*/
alter table [TestPaperQues] ALTER COLUMN Tp_Id  bigint not null
alter table [Test] ALTER COLUMN Tp_Id  bigint not null
alter table [TestResults] ALTER COLUMN Tp_Id  bigint not null
alter table [ExamResultsTemp] ALTER COLUMN Tp_Id  bigint not null
alter table [ExamResults] ALTER COLUMN Tp_Id  bigint not null
alter table [Examination] ALTER COLUMN Tp_Id  bigint not null

/***************
课程Id转为雪花id
*/
ALTER TABLE [Course] ADD [Cou_SID] bigint default 0 not null
go
update [Course]  set Cou_SID=Cou_ID
go
/*删除原有主键，设置新的主建*/
declare cursor_cou  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Course'
open cursor_cou
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_cou into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Course] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_cou into @pk 
 end   
--关闭并释放游标
close cursor_cou
deallocate cursor_cou
go
alter table [Course] drop column Cou_ID
go
execute sp_rename '[Course].Cou_SID','Cou_ID'
go
alter table [Course] add primary key ( Cou_ID );
go
alter table [Course] ALTER COLUMN Cou_PID  bigint not null
/*课程关联表中的字段Cou_ID，转长整型*/
alter table [TestResults] ALTER COLUMN Cou_ID  bigint not null
alter table [TestPaper] ALTER COLUMN Cou_ID  bigint not null

alter table [KnowledgeSort] ALTER COLUMN Cou_ID  bigint not null
alter table [Knowledge] ALTER COLUMN Cou_ID  bigint not null
alter table [GuideColumns] ALTER COLUMN Cou_ID  bigint not null

alter table [Guide] ALTER COLUMN Cou_ID  bigint not null
alter table [Teacher_Course] ALTER COLUMN Cou_ID  bigint not null
alter table [Student_Ques] ALTER COLUMN Cou_ID  bigint not null
alter table [Student_Notes] ALTER COLUMN Cou_ID  bigint not null
alter table [Student_Course] ALTER COLUMN Cou_ID  bigint not null

alter table [Student_Collect] ALTER COLUMN Cou_ID  bigint not null
alter table [CoursePrice] ALTER COLUMN Cou_ID  bigint not null
alter table [QuesTypes] ALTER COLUMN Cou_ID  bigint not null
alter table [Questions] ALTER COLUMN Cou_ID  bigint not null
alter table [OutlineEvent] ALTER COLUMN Cou_ID  bigint not null

alter table [Outline] ALTER COLUMN Cou_ID  bigint not null
alter table [MessageBoard] ALTER COLUMN Cou_ID  bigint not null
alter table [Message] ALTER COLUMN Cou_ID  bigint not null
alter table [LogForStudentStudy] ALTER COLUMN Cou_ID  bigint not null
alter table [LogForStudentQuestions] ALTER COLUMN Cou_ID  bigint not null

alter table [LogForStudentExercise] ALTER COLUMN Cou_ID  bigint not null



/***************
通知公告Id转为雪花id
*/
ALTER TABLE Notice ADD No_SId bigint default 0 not null
go
update Notice  set No_SId=No_Id
go
/*删除原有主键，设置新的主建*/
declare cursor_no cursor scroll
for SELECT idx.name,idx.type FROM sys.sysobjects idx JOIN sys.tables tab ON (idx.parent_obj = tab.object_id) 
	where tab.name='Notice'
open cursor_no
declare @name nvarchar(500),@type nvarchar(5),@sql nvarchar(500)
fetch First from cursor_no into @name,@type
while @@fetch_status=0  
 begin  
   set @sql='alter table [Notice] drop constraint '+@name   
   exec sp_executesql @sql
   fetch next from cursor_no into @name,@type
 end   
--关闭并释放游标
close cursor_no
deallocate cursor_no
go
alter table Notice drop column No_Id
go
execute sp_rename '[Notice].No_SId','No_Id'
go
alter table [Notice] add primary key ( No_Id );
go



/***************
专业Id转为雪花id
*/
ALTER TABLE [Subject] ADD [Sbj_SID] bigint default 0 not null
go
update [Subject]  set Sbj_SID=Sbj_ID
go
/*删除原有主键，设置新的主建*/
declare cursor_sbj  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='Subject'
open cursor_sbj
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_sbj into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [Subject] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_sbj into @pk 
 end   
--关闭并释放游标
close cursor_sbj
deallocate cursor_sbj
go
alter table [Subject] drop column Sbj_ID
go
execute sp_rename '[Subject].Sbj_SID','Sbj_ID'
go
alter table [Subject] add primary key ( Sbj_ID );
go
alter table [Subject] ALTER COLUMN Sbj_PID  bigint not null
/*专业关联表中的字段Cou_ID，转长整型*/
alter table [TrPlan] ALTER COLUMN Sbj_ID  bigint not null
alter table [TestResults] ALTER COLUMN Sbj_ID  bigint not null
alter table [TestPaper] ALTER COLUMN Sbj_ID  bigint not null
alter table [Team] ALTER COLUMN Sbj_ID  bigint not null
alter table [ExamResultsTemp] ALTER COLUMN Sbj_ID  bigint not null

alter table [ExamResults] ALTER COLUMN Sbj_ID  bigint not null
alter table [Examination] ALTER COLUMN Sbj_ID  bigint not null
alter table [Student_Ques] ALTER COLUMN Sbj_ID  bigint not null
alter table [Student_Collect] ALTER COLUMN Sbj_ID  bigint not null
alter table [Depart_Subject] ALTER COLUMN Sbj_ID  bigint not null

alter table [Course] ALTER COLUMN Sbj_ID  bigint not null
alter table [Questions] ALTER COLUMN Sbj_ID  bigint not null
alter table [Outline] ALTER COLUMN Sbj_ID  bigint not null

/***************
学员组自增Id转为雪花id
*/
ALTER TABLE [StudentSort] ADD [Sts_SID] bigint default 0 not null
go
update [StudentSort]  set Sts_SID=Sts_ID
go
/*删除原有主键，设置新的主建*/
declare cursor_ss  cursor scroll
for SELECT idx.name AS pk FROM sys.indexes idx JOIN sys.tables tab ON (idx.object_id = tab.object_id) where tab.name='StudentSort'
open cursor_ss
declare @pk nvarchar(500),@sql nvarchar(500)
fetch First from cursor_ss into @pk
while @@fetch_status=0  
 begin  
   set @sql='alter table [StudentSort] drop constraint '+@pk   
   exec sp_executesql @sql
   fetch next from cursor_ss into @pk 
 end   
--关闭并释放游标
close cursor_ss
deallocate cursor_ss
go
alter table [StudentSort] drop column Sts_ID
go
execute sp_rename '[StudentSort].Sts_SID','Sts_ID'
go
alter table [StudentSort] add primary key ( Sts_ID );
go
alter table [TestResults] ALTER COLUMN Sts_ID  bigint not null
alter table [ExamResults] ALTER COLUMN Sts_ID  bigint not null
alter table [ExamGroup] ALTER COLUMN Sts_ID  bigint not null
alter table [Student] ALTER COLUMN Sts_ID  bigint not null
alter table [Accounts] ALTER COLUMN Sts_ID  bigint not null
go


/*学员组与课程的关联表*/
CREATE TABLE [StudentSort_Course](
	[Ssc_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ssc_StartTime] [datetime] NOT NULL,
	[Ssc_EndTime] [datetime] NOT NULL,
	[Ssc_IsEnable] [bit] NOT NULL,
	[Cou_ID] [bigint] NOT NULL,
	[Sts_ID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Ssc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, 
IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/*在学员与课程的关联记录中，增加学员组id的字段*/
alter table [Student_Course] add [Sts_ID] bigint default 0 not null