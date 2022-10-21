
/*新的方法，完成度统计更准确*/
select * from course as c inner join 
(
	select Cou_ID, max(lastTime) as 'lastTime', SUM(studyTime) as 'studyTime'	
	,cast(convert(decimal(18,4), cast(sum(complete) as float)/COUNT(*)) as float) as 'complete'
	 from 
	(	
		select c.*, s.lastTime
		,CASE WHEN s.studyTime is null THEN 0 ELSE s.studyTime END as 'studyTime'
		,CASE WHEN s.complete is null THEN 0 WHEN s.complete>100  THEN 100 ELSE s.complete END as 'complete'
		from 
		(
			(SELECT * from outline where Cou_ID=132 and Ol_IsUse=1 and Ol_IsFinish=1 and Ol_IsVideo=1)  as c left join 
				(SELECT ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
							 sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
							 (case  when max(Lss_Duration)>0 then
							 cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
							 else 0 end
						  ) as 'complete'
					FROM [LogForStudentStudy]  where Cou_ID=132 and  ac_id=2  group by ol_id 
				) as s on c.Ol_ID=s.Ol_ID
		) 
	) as t group by Cou_ID
) as tm on c.Cou_ID=tm.Cou_ID