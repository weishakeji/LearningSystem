 Update Accessory Set as_filename=replace(as_filename,'http://','https://') 
 where (as_type='CourseVideo' and as_isouter=1 and as_isother=0)
 and as_filename like 'http%' and as_filename not like 'https%'