/* 学员的课程，一部分为购买的课程，一部分是所属学员组关联的课程*/
select * from 
(
	select ROW_NUMBER() OVER(Order by Cou_ID) AS 'rowid',*  from 
	(
		select cou.* from Course as cou left join  Student_Course as sc
		on cou.Cou_ID = sc.Cou_ID
		where sc.Ac_ID=2 and sc.Stc_IsEnable=1 and sc.Stc_Type!=5
		and (sc.Stc_StartTime<getdate() and  sc.Stc_EndTime>getdate())
	
		union
		--except

		select cou.* from Course as cou right join  StudentSort_Course as ssc
		on cou.Cou_ID = ssc.Cou_ID
		where ssc.Sts_ID=2

	) as result where 1=1 or Cou_Name like '%高%' 
)as t 

--where  rowid > 1 and rowid<=3