SELECT 
    table_schema,
    table_name, 
    column_name,
    data_type,
    udt_name
FROM 
    information_schema.columns
WHERE 
    -- 按数据类型名称查询
    data_type = 'money'  -- 例如: 'integer', 'varchar', 'text'等
    -- 或者按UDT(用户定义类型)名称查询
    -- udt_name = '您要查询的类型名称'
ORDER BY 
    table_schema, 
    table_name, 
    column_name;