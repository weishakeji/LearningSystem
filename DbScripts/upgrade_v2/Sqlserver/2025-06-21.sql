/*是否启用AI助教,默认为不启用，即false*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_EnabledAI')
BEGIN
    ALTER TABLE Course ADD Cou_EnabledAI bit NOT NULL CONSTRAINT DF_Course_Cou_EnabledAI DEFAULT 0;
END

/*是否采用公域大语言模型,默认为0，即公域大模型*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_AIType')
BEGIN
    ALTER TABLE Course ADD Cou_AIType int NOT NULL CONSTRAINT DF_Course_Cou_AIType DEFAULT 0;
END

/*智能体的网址*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Course') AND name = 'Cou_AIAgent')
BEGIN
    ALTER TABLE Course ADD Cou_AIAgent VARCHAR(2000);
END

/*AI沟通记录，增加课程ID*/
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('LlmRecords') AND name = 'Cou_ID')
BEGIN
    ALTER TABLE LlmRecords ADD Cou_ID bigint NOT NULL DEFAULT 0;
    CREATE INDEX LlmRecords_IX_Cou_ID ON LlmRecords(Cou_ID ASC);
END

/*通知公告的排序字段*/
ALTER TABLE Notice ADD No_Order int NOT NULL CONSTRAINT DF_Notice_No_Order DEFAULT 0
go
    /*设置公告的排序号*/
    UPDATE t SET No_Order = subq.rn
    FROM Notice t
    INNER JOIN (SELECT No_Id, ROW_NUMBER() OVER (ORDER BY No_Id) AS rn FROM Notice) subq ON t.No_Id = subq.No_Id
go   
CREATE INDEX Notice_IX_No_Order ON Notice(No_Order DESC)
go

/*设置学员组的排序号，之前的偶尔有重复*/
UPDATE t SET Sts_Tax = subq.rn
FROM StudentSort t
INNER JOIN (
    SELECT Sts_ID, ROW_NUMBER() OVER (ORDER BY Sts_Tax ASC) AS rn
    FROM StudentSort
) subq ON t.Sts_ID = subq.Sts_ID;


/*新闻文章的排序字段*/
ALTER TABLE Article ADD Art_Order int NOT NULL CONSTRAINT DF_Article_Art_Order DEFAULT 0
go
/*设置文章的排序号*/
    UPDATE t SET Art_Order = subq.rn
    FROM Article t
    INNER JOIN (
        SELECT Art_ID, ROW_NUMBER() OVER (ORDER BY Art_ID) AS rn
        FROM Article
    ) subq ON t.Art_ID = subq.Art_ID
    go
CREATE INDEX Article_IX_Art_Order ON Article(Art_Order DESC)