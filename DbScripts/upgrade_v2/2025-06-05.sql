-- 创建表 LlmRecords --
DROP TABLE IF EXISTS public."LlmRecords" CASCADE;
CREATE TABLE IF NOT EXISTS public."LlmRecords"
(
	"Llr_ID" integer NOT NULL,
	"Llr_CrtTime" TIMESTAMP WITH TIME ZONE NOT NULL,
  "Llr_LastTime" TIMESTAMP WITH TIME ZONE NOT NULL,
	"Llr_Topic" character varying(500) COLLATE pg_catalog."default",	
	"Llr_Records" text,	
	"Ac_ID" integer NOT NULL,	
	 CONSTRAINT key_llmrecords PRIMARY KEY ("Llr_ID")
);
CREATE SEQUENCE IF NOT EXISTS public."LlmRecords_Llr_ID_seq" START WITH 1 INCREMENT BY 1 MINVALUE 1  MAXVALUE 9223372036854775807  CYCLE;
ALTER SEQUENCE IF EXISTS public."LlmRecords_Llr_ID_seq" OWNED BY public."LlmRecords"."Llr_ID";
ALTER TABLE "LlmRecords" ALTER COLUMN "Llr_ID" SET DEFAULT NEXTVAL('"LlmRecords_Llr_ID_seq"'::regclass);

CREATE INDEX IF NOT EXISTS "LlmRecords_aaaaaLlmRecords_PK" ON "LlmRecords" ("Llr_ID" ASC);
CREATE INDEX IF NOT EXISTS "LlmRecords_IX_Llr_CrtTime" ON "LlmRecords" ("Llr_CrtTime" ASC);
CREATE INDEX IF NOT EXISTS "LlmRecords_IX_Llr_LastTimee" ON "LlmRecords" ("Llr_LastTime" ASC);
CREATE INDEX IF NOT EXISTS "LlmRecords_IX_Ac_ID" ON "LlmRecords" ("Ac_ID" ASC);


/*添加大语言模型的菜单项*/
INSERT INTO "ManageMenu"("MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES ('','',100,'','func','','e820','',5,0,0,'',false,false,false,false,true,true,'/manage/setup/LargeLanguageModel','','大语言模型','742f03375a49149ef533668189ec0777',88,6,'item','1749396749753',0,'',false,false,false,false,0);

