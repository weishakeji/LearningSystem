
select cou.*, 
CASE WHEN s.count is null THEN 0 ELSE s.count END as '学员数',
CASE WHEN q.count is null THEN 0 ELSE q.count END as '试题数'
from 
(
	/*查询课程总时长,章节数*/
	select c.Cou_ID,c.Cou_Name as '课程', 
	CASE WHEN olcount is null THEN 0 ELSE olcount END as '章节数',
	CASE WHEN c.Th_Name='无' THEN ''  WHEN c.Th_Name is null THEN ''  ELSE c.Th_Name END as '教师',
	CASE WHEN dur.duration is null THEN 0 ELSE dur.duration END  as '总时长' 
	from Course as c left join
	(
		select cou_id,COUNT(*) as olcount, SUM(As_Duration) as 'duration' from (
			select Ol_ID,Ol_Name,Cou_ID,Ol_UID,acc.* from 
			(
				(select * from Outline) as ol
				left join 
				(SELECT [As_Id]
					  ,[As_Name]      
					  ,[As_Uid]    
					  ,[As_Duration]    
				  FROM [Accessory]
				where As_Type='CourseVideo') as acc on ol.Ol_UID=acc.As_Uid
			) 
		) as data group by cou_id
	) as dur on  c.Cou_ID=dur.Cou_ID
) as cou left join (select Cou_ID,COUNT(*) as 'count' from Student_Course group by Cou_ID) as s
on cou.Cou_ID=s.Cou_ID
left join (select Cou_ID,COUNT(*) as 'count' from Questions group by Cou_ID) as q
on cou.Cou_ID=q.Cou_ID