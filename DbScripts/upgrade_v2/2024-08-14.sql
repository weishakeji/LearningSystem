 
 
/* 学习记录中的综合成绩 */

--SQLserver脚本
alter table "Student_Course" add "Stc_ResultScore" float NULL
go
update "Student_Course" set "Stc_ResultScore"=0 where "Stc_ResultScore" is null
go
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" float NOT NULL
go

--PostgreSQL脚本
alter table "Student_Course" add "Stc_ResultScore" real NULL;
update "Student_Course" set "Stc_ResultScore"=0;
alter table "Student_Course" ALTER COLUMN "Stc_ResultScore" set NOT NULL;