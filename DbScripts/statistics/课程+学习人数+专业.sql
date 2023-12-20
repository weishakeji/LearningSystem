--开放课程的学习人数 ，以及课程所属专业
 select cou.*, CASE WHEN sc.count IS null THEN 0 else sc.count end as 'count' from 
 (select Cou_Name,[Subject].Sbj_Name,Cou_ID from Course
  left join [Subject] on Course.Sbj_ID=[Subject].Sbj_ID
   where Course.Cou_IsUse=1) as cou
   
   left join 
   
   (select Cou_ID,COUNT(*) as 'count' from Student_Course group by Cou_ID) as sc
   
   on cou.Cou_ID=sc.Cou_ID
 order by count desc