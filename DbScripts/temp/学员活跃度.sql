

--select *, ROW_NUMBER() OVER( ORDER BY acc.ac_id desc ) AS rowid from Accounts 
--where 1=1 and 

select *  from (
	select acc.Ac_ID,Ac_Name,Ac_AccName,Ac_Photo,Ac_IDCardNumber,Ac_MobiTel1,Ac_LastTime,Sts_ID,Sts_Name
		,logincount,logintime
		,coursecount,rechargecount,laststudy,lastexrcise,lasttest,lastexam
		,ROW_NUMBER() OVER( ORDER BY coursecount desc ) AS rowid from Accounts as acc
	left join  --登录次数与最后登录时间
	(select Ac_id, COUNT(*) as 'logincount', max(Lso_CrtTime) as 'logintime' from LogForStudentOnline group by Ac_ID) as ol
		on acc.Ac_ID=ol.Ac_id
	left join --课程购买个数
	(select Ac_id, COUNT(*) as 'coursecount' from Student_Course group by Ac_ID) as buy
		on acc.Ac_ID=buy.Ac_id
	left join ----资金动向
	(select Ac_id, COUNT(*) as 'rechargecount',max(Ma_CrtTime) as 'lastrecharge'  from MoneyAccount group by Ac_ID) as recharge
		on acc.Ac_ID=recharge.Ac_ID			
	left join --视频学习记录
	(select Ac_id, max(Lss_LastTime) as 'laststudy' from LogForStudentStudy group by Ac_ID) as video
		on acc.Ac_ID=video.Ac_ID
	left join --试题练习记录
	(select Ac_id, max(Lse_LastTime) as 'lastexrcise' from LogForStudentExercise group by Ac_ID) as ques
		on acc.Ac_ID=ques.Ac_ID
	left join --测试成绩
	(select Ac_id, max(Tr_CrtTime) as 'lasttest' from TestResults group by Ac_ID) as test
		on acc.Ac_ID=test.Ac_ID
	left join --考试成绩
	(select Ac_id, max(Exr_CrtTime) as 'lastexam' from ExamResults group by Ac_ID) as exam
		on acc.Ac_ID=exam.Ac_ID
	--查询条件
	where Ac_AccName like '%41%' and Ac_Name like '%张%' and Sts_ID=3058
	and Ac_MobiTel1 like '%1%'  and Ac_IDCardNumber like '%41%'
	--and Ac_CodeNumber like '%41%'  	
) as res where rowid BETWEEN 1 AND 20

--order by coursecount desc
--select * from LogForStudentOnline