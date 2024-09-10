 
 
/* 学习卡设置项的主题长度变更为200，原来是500，为了方便索引 */

/* PostgreSQL 升级脚本  */
alter table "Student_Course" add "Stc_ResultScore" real NULL;
update "Student_Course" set "Stc_ResultScore"=0;
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" set NOT NULL;

alter table "Course" add "Cou_Prices" text;

ALTER TABLE "Course" DROP COLUMN "Cou_ExistQues";
alter table "Course" add "Cou_QuesCount" int NULL;
update "Course" set "Cou_QuesCount"=0;
alter table "Course" ALTER COLUMN "Cou_QuesCount" set NOT NULL;

alter table "Course" add "Cou_TestCount" int NULL;
update "Course" set "Cou_TestCount"=0;
alter table "Course" ALTER COLUMN "Cou_TestCount" set NOT NULL;

alter table "Course" add "Cou_OutlineCount" int NULL;
update "Course" set "Cou_OutlineCount"=0;
alter table "Course" ALTER COLUMN "Cou_OutlineCount" set NOT NULL;

alter table "Course" add "Cou_VideoCount" int NULL;
update "Course" set "Cou_VideoCount"=0;
alter table "Course" ALTER COLUMN "Cou_VideoCount" set NOT NULL;

alter table "Course" add "Cou_KnlCount" int NULL;
update "Course" set "Cou_KnlCount"=0;
alter table "Course" ALTER COLUMN "Cou_KnlCount" set NOT NULL;

/* SQLserver  升级脚本*/
alter table "LearningCardSet" ALTER COLUMN Lcs_Theme [nvarchar](200) NULL
go
alter table "Course" add "Cou_Prices" [nvarchar](max) NULL
go
ALTER TABLE "Course" DROP COLUMN "Cou_ExistQues";
go
alter table "Course" add  "Cou_QuesCount" int
go
update "Course" set "Cou_QuesCount"=0 where "Cou_QuesCount" is null
go
alter table "Course" ALTER COLUMN "Cou_QuesCount" int NOT NULL
go

alter table "Course" add  "Cou_TestCount" int
go
update "Course" set "Cou_TestCount"=0 where "Cou_TestCount" is null
go
alter table "Course" ALTER COLUMN "Cou_TestCount" int NOT NULL
go

alter table "Course" add  "Cou_OutlineCount" int
go
update "Course" set "Cou_OutlineCount"=0 where "Cou_OutlineCount" is null
go
alter table "Course" ALTER COLUMN "Cou_OutlineCount" int NOT NULL
go

alter table "Course" add  "Cou_VideoCount" int
go
update "Course" set "Cou_VideoCount"=0 where "Cou_VideoCount" is null
go
alter table "Course" ALTER COLUMN "Cou_VideoCount" int NOT NULL
go

alter table "Course" add  "Cou_KnlCount" int
go
update "Course" set "Cou_KnlCount"=0 where "Cou_KnlCount" is null
go
alter table "Course" ALTER COLUMN "Cou_KnlCount" int NOT NULL
go

