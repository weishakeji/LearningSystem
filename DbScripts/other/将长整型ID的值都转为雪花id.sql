/*将专业关联表中的Sbj_ID转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Sbj_ID FROM [Subject] where Sbj_ID<100000 order by Sbj_ID
open cursor_obj
declare @sbjid bigint, @snowsbj bigint
set @snowsbj=15012714692000000
fetch First from cursor_obj into @sbjid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
	set @snowsbj=@snowsbj+1
	Update [Subject] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid	
	Update [Subject] Set Sbj_PID=@snowsbj Where Sbj_PID=@sbjid
	Update [TestResults] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [TestPaper] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [ExamResults] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Examination] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Student_Ques] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Student_Collect] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Course] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Questions] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid
	Update [Outline] Set Sbj_ID=@snowsbj Where Sbj_ID=@sbjid

	fetch next from cursor_obj into @sbjid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj


/*将课程关联表中的Cou_ID转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Cou_ID FROM Course where Cou_ID<100000 order by Cou_ID
open cursor_obj
declare @couid bigint, @snowid bigint
set @snowid=15012714693000000
fetch First from cursor_obj into @couid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
	set @snowid=@snowid+1
	---print @snowid
	Update Course Set Cou_ID=@snowid Where Cou_ID=@couid
	
	Update [TestResults] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [TestPaper] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [KnowledgeSort] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Knowledge] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [GuideColumns] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Guide] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Teacher_Course] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Ques] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Notes] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Student_Course] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Student_Collect] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [CoursePrice] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [QuesTypes] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Questions] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [OutlineEvent] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [Outline] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [MessageBoard] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [Message] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [LogForStudentStudy] Set Cou_ID=@snowid Where Cou_ID=@couid
	Update [LogForStudentQuestions] Set Cou_ID=@snowid Where Cou_ID=@couid

	Update [LogForStudentExercise] Set Cou_ID=@snowid Where Cou_ID=@couid	
	Update [StudentSort_Course] Set Cou_ID=@snowid Where Cou_ID=@couid
	
	fetch next from cursor_obj into @couid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj



/*将章节关联表中的Ol_ID转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Ol_ID FROM outline where Ol_ID<100000 order by Ol_ID
open cursor_obj
declare @olid bigint, @snowol bigint
set @snowol=15012714694000000
fetch First from cursor_obj into @olid
while @@fetch_status=0  --提取成功，进行下一条数据的提取操作 
 begin
	set @snowol=@snowol+1
	---print @snowid
	Update outline Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update outline Set Ol_PID=@snowol Where Ol_PID=@olid	
	Update [TestPaperItem] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [Questions] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [OutlineEvent] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [Message] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentStudy] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentQuestions] Set Ol_ID=@snowol Where Ol_ID=@olid	
	Update [LogForStudentExercise] Set Ol_ID=@snowol Where Ol_ID=@olid	
	fetch next from cursor_obj into @olid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj




/*将试卷关联表中的Tp_Id转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Tp_Id FROM TestPaper where Tp_Id<100000 order by Tp_Id
open cursor_obj
declare @tpid bigint, @snowtp bigint
set @snowtp=15012714695000000
fetch First from cursor_obj into @tpid
while @@fetch_status=0 
 begin
	set @snowtp=@snowtp+1
	Update TestPaper Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [TestPaperQues] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [TestResults] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [ExamResults] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	Update [Examination] Set Tp_Id=@snowtp Where Tp_Id=@tpid
	
	fetch next from cursor_obj into @tpid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj


/*试题部分不要执行，则否之前的考试成绩回顾会有问题

/*试题关联表中的字段Qus_ID转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Qus_ID FROM Questions where Qus_ID<100000 order by Qus_ID
open cursor_obj
declare @qid bigint, @snowqs bigint
set @snowqs=15012714515000000
fetch First from cursor_obj into @qid
while @@fetch_status=0 
 begin
	set @snowqs=@snowqs+1
	Update Questions Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update Student_Ques Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [Student_Notes] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [Student_Collect] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [QuesAnswer] Set Qus_ID=@snowqs Where Qus_ID=@qid
	Update [LogForStudentQuestions] Set Qus_ID=@snowqs Where Qus_ID=@qid
	
	fetch next from cursor_obj into @qid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj
*/

/*学员组的字段Sts_ID转为雪花id*/
declare cursor_obj  cursor scroll
for SELECT Sts_ID FROM [StudentSort] where Sts_ID<100000 order by Sts_ID
open cursor_obj
declare @stsid bigint, @snowsts bigint
set @snowsts=15012714616000000
fetch First from cursor_obj into @stsid
while @@fetch_status=0 
 begin
	set @snowsts=@snowsts+1
	Update [StudentSort] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [TestResults] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [ExamResults] Set Sts_ID=@snowsts Where Sts_ID=@stsid
	Update [ExamGroup] Set Sts_ID=@snowsts Where Sts_ID=@stsid	
	Update [Accounts] Set Sts_ID=@snowsts Where Sts_ID=@stsid	
	fetch next from cursor_obj into @stsid
 end   
--关闭并释放游标
close cursor_obj
deallocate cursor_obj
