/*
“批量修改视频文件的连接信息”
例如，当更改视频云存储路径时

注：
1、下述代码仅支持PostgrSQL数据库
2、下述代码用了REPLACE替换，如果要替换的信息比较短小，建议用更精准的匹配方式

*/


DO $$  
DECLARE  
    old_text TEXT := '视频地址';  -- 原字符串  
    new_text TEXT := '新视频地址'; -- 新字符串  
BEGIN  
    UPDATE "Accessory"  
    SET "As_FileName" = REPLACE("As_FileName", old_text, new_text)  
    WHERE "As_Type" = 'CourseVideo'  
      AND "As_IsOuter" = true  --必须是外部连接视频
      AND "As_IsOther" = false 
      AND "As_FileName" LIKE '%' || old_text || '%';  
END $$; 

