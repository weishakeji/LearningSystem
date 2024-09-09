 
 
/* 学习卡设置项的主题长度变更为200，原来是500，为了方便索引 */

--SQLserver脚本
alter table "LearningCardSet" ALTER COLUMN Lcs_Theme [nvarchar](200) NULL
go

--PostgreSQL脚本
alter table "Student_Course" add "Stc_ResultScore" real NULL;
update "Student_Course" set "Stc_ResultScore"=0;
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" set NOT NULL;