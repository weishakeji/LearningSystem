
/*
	删除 PostgreSQL 所有非主键索引

	编写目的：
	在创建索引时如果有错误，清除原有的索引，方便重新创建索引
	考虑到主键默认有索引，此处只删除非聚集索引
*/
/*定义函数，用于删除指定表的索引（非聚集）*/
CREATE OR REPLACE FUNCTION del_table_indexs(tbname VARCHAR(200))
RETURNS VOID AS $$
DECLARE cur_obj CURSOR FOR
	select indexname, columnname from
		(SELECT indexname, unnest(REGEXP_MATCHES(indexdef,'btree \(\"(\w[^\)]+)\"')) as columnname
		FROM pg_indexes
		WHERE indexdef NOT LIKE '%CLUSTER%'
		AND schemaname = 'public' and tablename=tbname
		) as tm where "columnname" not in 
		(SELECT kcu.column_name
		FROM information_schema.key_column_usage kcu
		JOIN information_schema.table_constraints tc ON kcu.constraint_name = tc.constraint_name
		WHERE tc.table_schema = 'public' and tc.constraint_type = 'PRIMARY KEY' and kcu.table_name=tbname);
	indexname VARCHAR(200);	--索引名
	tmsql VARCHAR(500);	
BEGIN
    OPEN cur_obj;
    LOOP
        FETCH cur_obj INTO indexname;
        EXIT WHEN NOT FOUND;
			--RAISE NOTICE 'table: %', indexname;	
			tmsql:='DROP INDEX IF EXISTS  "' || indexname || '" CASCADE;';
			EXECUTE tmsql; 
			RAISE NOTICE 'tmsql: %', tmsql;	
    END LOOP;
    CLOSE cur_obj;
END;
$$ LANGUAGE plpgsql;

--利用游标，遍历表，并调用前面的函数，用于删除索引
DO $$
DECLARE cur_name CURSOR FOR
	SELECT tablename FROM pg_tables WHERE schemaname = 'public' order by tablename;	
	tbname VARCHAR(200);	--表名	
BEGIN
OPEN cur_name;
	LOOP
	    FETCH cur_name INTO tbname;
	   	EXIT WHEN NOT FOUND; -- 如果没有数据了，退出循环	
		--RAISE NOTICE 'table: %', tbname;	
		PERFORM del_table_indexs(tbname);
	END LOOP;
	CLOSE cur_name;
END $$;

--PERFORM del_table_indexs('Accounts');

-- 删除函数
DROP FUNCTION IF EXISTS del_table_indexs(VARCHAR(200));