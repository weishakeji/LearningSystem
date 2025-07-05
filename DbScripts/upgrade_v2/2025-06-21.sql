/*是否启用AI助教,默认为不启用，即false*/
ALTER TABLE "Course" ADD COLUMN  IF NOT EXISTS "Cou_EnabledAI" boolean NOT NULL DEFAULT false;
    
/*是否采用公域大语言模型,默认为0，即公域大模型*/
ALTER TABLE "Course" ADD COLUMN  IF NOT EXISTS "Cou_AIType"  int NOT NULL DEFAULT 0;

/*智能体的网址*/  
ALTER TABLE "Course" ADD COLUMN IF NOT EXISTS "Cou_AIAgent" VARCHAR(2000) UNIQUE;


/*AI沟通记录，增加课程ID*/  
ALTER TABLE "LlmRecords" ADD COLUMN IF NOT EXISTS "Cou_ID" int8 NOT NULL DEFAULT 0;
CREATE INDEX IF NOT EXISTS "LlmRecords_IX_Cou_ID" ON "LlmRecords" ("Cou_ID" ASC);

/*通知公告的排序字段*/
ALTER TABLE "Notice" ADD COLUMN  IF NOT EXISTS "No_Order"  int NOT NULL DEFAULT 0;
CREATE INDEX IF NOT EXISTS "Notice_IX_No_Order" ON "Notice" ("No_Order" DESC);
/*设置公告的排序号*/
UPDATE "Notice" t SET "No_Order" = subq.rn
FROM (
    SELECT "No_Id", ROW_NUMBER() OVER (ORDER BY "No_Id") AS rn
    FROM "Notice"
) subq WHERE t."No_Id" = subq."No_Id";


/*设置学员组的排序号，之前的偶尔有重复*/
UPDATE "StudentSort" t SET "Sts_Tax" = subq.rn
FROM (
    SELECT "Sts_Tax", "Sts_ID",ROW_NUMBER() OVER (ORDER BY "Sts_Tax" asc) AS rn
    FROM "StudentSort"
) subq WHERE t."Sts_ID" = subq."Sts_ID";

Art_Color

/*新闻文章的排序字段*/
ALTER TABLE "Article" ADD COLUMN  IF NOT EXISTS "Art_Order"  int NOT NULL DEFAULT 0;
CREATE INDEX IF NOT EXISTS "Article_IX_Art_Order" ON "Article" ("Art_Order" DESC);
/*设置文章的排序号*/
UPDATE "Article" t SET "Art_Order" = subq.rn
FROM (
    SELECT "Art_ID", ROW_NUMBER() OVER (ORDER BY "Art_ID") AS rn
    FROM "Article"
) subq WHERE t."Art_ID" = subq."Art_ID";


