/*获取所有表*/
select name,crdate from sysobjects where type='U' order by name asc

select * from sysobjects

/* 表信息 */
SELECT [TableName] = [Tables].name ,
        [TableOwner] = [Schemas].name ,
        [TableCreateDate] = [Tables].create_date ,
        [TableModifyDate] = [Tables].modify_date
FROM    sys.tables AS [Tables]
        INNER JOIN sys.schemas AS [Schemas] ON [Tables].schema_id = [Schemas].schema_id
WHERE   [Tables].name = 'Course'

 /*查询表主键*/
 SELECT distinct
	TABLE_NAME,
	COLUMN_NAME=stuff((
		SELECT '|'+COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		where TABLE_NAME ='Article'
    FOR XML path('')
    ), 1, 1, '')
FROM
	INFORMATION_SCHEMA.KEY_COLUMN_USAGE
where TABLE_NAME ='Article'


/**表的字段信息，包括数据类型，长度*/
SELECT name,type_name(xtype) AS type,
--长度，无限长为-1
length,(type_name(xtype)+'('+CONVERT(varchar,length)+')') as fulltype,
--是否可空，0为可空
isnullable as nullable
FROM syscolumns
WHERE (id = OBJECT_ID('questions'))
order by name asc

/*查询某个数据类型的字段*/
SELECT 
    TABLE_NAME,
    COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE DATA_TYPE = 'float'