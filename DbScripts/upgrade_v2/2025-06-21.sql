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
CREATE INDEX IF NOT EXISTS "Notice_IX_No_Orde" ON "Notice" ("No_Order" DESC);
/*设置公告的排序号*/
UPDATE "Notice" t SET "No_Order" = subq.rn
FROM (
    SELECT "No_Id", ROW_NUMBER() OVER (ORDER BY "No_Id") AS rn
    FROM "Notice"
) subq WHERE t."No_Id" = subq."No_Id";




