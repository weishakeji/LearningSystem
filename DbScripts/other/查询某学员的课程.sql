
select * from (
select ROW_NUMBER() OVER(Order by Cou_ID) AS 'rowid',*  from (

select cou.* from Course as cou right join  Student_Course as sc
on cou.Cou_ID = sc.Cou_ID
where sc.Ac_ID=2 and sc.Stc_IsEnable=1
--and  sc.Stc_EndTime<getdate()
--and (sc.Stc_StartTime<getdate() and  sc.Stc_EndTime>getdate())
--order by sc.Stc_EndTime desc

--union
except

select cou.* from Course as cou right join  StudentSort_Course as ssc
on cou.Cou_ID = ssc.Cou_ID
where ssc.Sts_ID=2

) as result where Cou_Name like '%¸ß%' 
)as t where  rowid > 1 and rowid<=3