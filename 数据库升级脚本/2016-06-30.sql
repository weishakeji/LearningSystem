--2016-06-30
/*将考试成绩记录改为nvarchar(max)类型*/
alter table [ExamResults] ALTER COLUMN Exr_Results [nvarchar](max) NULL
go
/*将章节内容改为nvarchar(max)类型*/
alter table [Outline] ALTER COLUMN Ol_Intro[nvarchar](max) NULL
go

--创建随机取试题的存储过程
if (exists (select * from sys.objects where name = 'PROC_QuesRandom'))
    drop proc PROC_QuesRandom
go
create proc PROC_QuesRandom(
    @orgid int, 
    @sbjid int,
    @couid int,
    @olid int,
    @type int,
    @diff1 int,
    @diff2 int,
    @isUse bit,
    @count int
)
as
declare @sql varchar(1000);
declare @where varchar(1000);
Declare @d Datetime;
set @d=getdate();
SET @sql = 'SELECT * FROM Questions';
--查询条件
set @where=' Qus_Diff>='+cast(@diff1 as varchar(1000))
    +' and Qus_Diff<='+ cast(@diff2 as varchar(1000))+' and Qus_IsError=0 and Qus_IsUse='
    +cast(@isUse as varchar(1000))+' and org_id=' + cast(@orgid as varchar(1000)) + ' ';
--试题类型，如果小于等于0，则不增加该条件
if (@type>0)
 begin
    set @where=@where+' and Qus_Type=' + cast(@type as varchar(1000)) ;
 end
--专业的条件
if (@sbjid>0)
 begin
    set @where=@where+' and Sbj_ID=' + cast(@sbjid as varchar(1000)) ;
 end
--课程的条件
if (@couid>0)
 begin
    set @where=@where+' and Cou_ID=' + cast(@couid as varchar(1000)) ;
 end
--章节的条件
if (@olid>0)
 begin
    set @where=@where+' and Ol_ID=' + cast(@olid as varchar(1000)) ;
 end
 --数量
 if (@count<1)
 begin
    set @count=99999;
 end 
--拼接最终的sql语句
set @sql='select top ' + cast(@count as varchar(1000)) + ' newid() as n,  *  from Questions where ' + @where + ' order by n';
set @sql = 'select * from (' + @sql + ') as t order by t.Qus_Type asc';
--set @sql='select top ' + @count + ' newid() as n,  *  from (select * from Questions where ' + @where + ') as tm order by n';
--print @where;
print @sql;
exec (@sql);

--select [语句执行花费时间(毫秒)]=Datediff(ms,@d,Getdate());
print '[语句执行花费时间(毫秒)]:'+cast(Datediff(ms,@d,Getdate()) as varchar(1000));
go

