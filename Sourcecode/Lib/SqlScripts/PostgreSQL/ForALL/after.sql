
/*
	修正主键的自增ID的启始值为最大ID值

	原因：
		当从其它数据库转到Postgresql时，自增ID默认是从1开始，如果表中有历史数据
		在新增记录时，自增ID有可能与之前的记录重复，导致插入失败
		在Sqlite中不存在这个问题
*/
--利用游标，获取序列与所属表、字段
DO $$
DECLARE cur_name CURSOR FOR
	SELECT sequence_name,
	substring(sequence_name FROM '(\w+)_[A-Za-z]+_[A-Za-z]+_seq$') as tbname,
	substring(sequence_name FROM '[A-Za-z]+_([A-Za-z]+_[A-Za-z]+)_seq') as keyname
	FROM information_schema.sequences 
	order by sequence_name;
	sname VARCHAR(200);		--序列的名称
	tbname VARCHAR(200);	--表名
	mainkey VARCHAR(50);	--字段名
	tmsql VARCHAR(500);		
	maxid bigint;			--最大的自增id的值
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