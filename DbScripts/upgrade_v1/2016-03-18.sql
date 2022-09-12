

/*知识库分类排序的bug修复*/
update [KnowledgeSort] set [Kns_Tax]=1
go
alter table [KnowledgeSort] ALTER COLUMN [Kns_Tax] [int] NOT NULL
go
alter table [KnowledgeSort] ALTER COLUMN [Kns_PID] [int] NOT NULL
go
DECLARE knlCursor CURSOR FOR select [Kns_ID],[Kns_Tax] from [KnowledgeSort]
open knlCursor
DECLARE @count int
SET @count=0
DECLARE @id nvarchar(10), @tax nvarchar(10)
    FETCH NEXT FROM  knlCursor INTO @id,@tax
    WHILE @@FETCH_STATUS =0    
        BEGIN        
            SET @count=@count+1        
            update [KnowledgeSort] set [Kns_Tax]=@count where [Kns_ID]=@id
        FETCH NEXT FROM  knlCursor INTO @id,@tax
    END    
CLOSE knlCursor
DEALLOCATE knlCursor