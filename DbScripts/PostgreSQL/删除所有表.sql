
/*
	删除 PostgreSQL 所有表，清空数据库

	编写目的：
	从Sqlserver向Postgresql导入失败时，造成大量冗余数据，为了清理方便可以执行下述sql脚本
	请谨慎操作
*/

--利用游标，遍历表,并删除
DO $$
DECLARE cur_name CURSOR FOR
	SELECT tablename FROM pg_tables WHERE schemaname = 'public';	
	tbname VARCHAR(200);	--表名
	tmsql VARCHAR(500);
BEGIN
OPEN cur_name;
	LOOP
	    FETCH cur_name INTO tbname;
	   	EXIT WHEN NOT FOUND; -- 如果没有数据了，退出循环
		--获取当前表的最大自增字段的值
	    tmsql:='DROP TABLE IF EXISTS  "' || tbname || '" CASCADE;';
		EXECUTE tmsql; 	
		RAISE NOTICE 'tmsql: %', tmsql;		
    
	END LOOP;
	CLOSE cur_name;
END $$;