-- 创建表 LlmRecords --
IF OBJECT_ID('dbo.LlmRecords', 'U') IS NOT NULL
    DROP TABLE dbo.LlmRecords;

CREATE TABLE dbo.LlmRecords
(
    Llr_ID int NOT NULL IDENTITY(1,1),
    Llr_CrtTime DATETIMEOFFSET NOT NULL,
    Llr_LastTime DATETIMEOFFSET NOT NULL,
    Llr_Topic varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS,
    Llr_Records text,
    Ac_ID int NOT NULL,
    CONSTRAINT key_llmrecords PRIMARY KEY (Llr_ID)
);

CREATE INDEX LlmRecords_aaaaaLlmRecords_PK ON LlmRecords (Llr_ID ASC);
CREATE INDEX LlmRecords_IX_Llr_CrtTime ON LlmRecords (Llr_CrtTime ASC);
CREATE INDEX LlmRecords_IX_Llr_LastTimee ON LlmRecords (Llr_LastTime ASC);
CREATE INDEX LlmRecords_IX_Ac_ID ON LlmRecords (Ac_ID ASC);

/*添加大语言模型的菜单项*/
INSERT INTO ManageMenu(MM_AbbrName,MM_Color,MM_Complete,MM_Font,MM_Func,MM_Help,MM_IcoCode,MM_IcoColor,MM_IcoSize,MM_IcoX,MM_IcoY,MM_Intro,MM_IsBold,MM_IsChilds,MM_IsFixed,MM_IsItalic,MM_IsShow,MM_IsUse,MM_Link,MM_Marker,MM_Name,MM_PatId,MM_Root,MM_Tax,MM_Type,MM_UID,MM_WinHeight,MM_WinID,MM_WinMax,MM_WinMin,MM_WinMove,MM_WinResize,MM_WinWidth) 
VALUES ('','',100,'','func','','e820','',5,0,0,'',0,0,0,0,1,1,'/manage/setup/LargeLanguageModel','','大语言模型','742f03375a49149ef533668189ec0777',88,6,'item','1749396749753',0,'',0,0,0,0,0);