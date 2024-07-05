-- 设置表 {{table}} 的自增id初始值
DO $$
DECLARE cur_name CURSOR FOR
	SELECT sequence_name,
	substring(sequence_name FROM '(\w+)_[A-Za-z]+_[A-Za-z]+_seq$') as tbname,
	substring(sequence_name FROM '[A-Za-z]+_([A-Za-z]+_[A-Za-z]+)_seq') as keyname
	FROM information_schema.sequences 
	where sequence_name like '{{table}}_%'
	order by sequence_name;
	sname VARCHAR(200);		--序列的名称
	tbname VARCHAR(200);	--表名
	mainkey VARCHAR(50);	--字段名
	tmsql VARCHAR(500);		
	maxid int;			--最大的自增id的值
BEGIN
OPEN cur_name;
	LOOP
	    FETCH cur_name INTO sname ,tbname, mainkey;
	   	EXIT WHEN NOT FOUND; -- 如果没有数据了，退出循环
		--获取当前表的最大自增字段的值
	    tmsql:='SELECT COALESCE(MAX("' || mainkey || '"),0) FROM "' || tbname || '"';
		EXECUTE tmsql INTO maxid; 	
		--RAISE NOTICE 'maxid: %, maxid:%', tmsql,maxid;

		--修改序列的启始值
		tmsql:='ALTER SEQUENCE "' || sname || '" RESTART WITH ' || (maxid+1);
		RAISE NOTICE 'sql: %', tmsql;
		EXECUTE tmsql; 
    
	END LOOP;
	CLOSE cur_name;
END $$;