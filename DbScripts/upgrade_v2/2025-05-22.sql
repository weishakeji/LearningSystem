
/*以下是Postgresql数据库升级脚本*/

/*计算专业的试题数，仅当前层级，不包括下级*/
UPDATE "Subject"
SET "Sbj_QuesCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";

/*计算专业的课程数，仅当前层级，不包括下级*/
UPDATE "Subject"
SET "Sbj_CourseCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "Course"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";

/*计算专业的试卷数，仅当前层级，不包括下级*/
UPDATE "Subject"
SET "Sbj_TestCount" = q.count
FROM (
    SELECT "Sbj_ID", COUNT(*) as count
    FROM "TestPaper"
    GROUP BY "Sbj_ID"
) as q
WHERE q."Sbj_ID" = "Subject"."Sbj_ID";



/*计算章节的试题数，仅当前层级，不包括下级*/
UPDATE "Outline"
SET "Ol_QuesCount" = q.count
FROM (
    SELECT "Ol_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Ol_ID"
) as q
WHERE q."Ol_ID" = "Outline"."Ol_ID";

/*试算课程的试题数*/
UPDATE "Course"
SET "Cou_QuesCount" = q.count
FROM (
    SELECT "Cou_ID", COUNT(*) as count
    FROM "Questions"
    GROUP BY "Cou_ID"
) as q
WHERE q."Cou_ID" = "Course"."Cou_ID";


/* 修正试题的难度等级，不知道为什么试题难度会出现大于5的情况 */
UPDATE "Questions" SET "Qus_Diff"=5  WHERE "Qus_Diff">=5;
UPDATE "Questions" SET "Qus_Diff"=1  WHERE "Qus_Diff"<=1;




/*以下是Sqlserver升级脚本*/

-- 计算专业的试题数，仅当前层级，不包括下级
UPDATE s
SET s.Sbj_QuesCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- 计算专业的课程数，仅当前层级，不包括下级
UPDATE s
SET s.Sbj_CourseCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM Course
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- 计算专业的试卷数，仅当前层级，不包括下级
UPDATE s
SET s.Sbj_TestCount = q.count
FROM Subject s
INNER JOIN (
    SELECT Sbj_ID, COUNT(*) as count
    FROM TestPaper
    GROUP BY Sbj_ID
) q ON q.Sbj_ID = s.Sbj_ID;

-- 计算章节的试题数，仅当前层级，不包括下级
UPDATE o
SET o.Ol_QuesCount = q.count
FROM Outline o
INNER JOIN (
    SELECT Ol_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Ol_ID
) q ON q.Ol_ID = o.Ol_ID;

-- 计算课程的试题数
UPDATE c
SET c.Cou_QuesCount = q.count
FROM Course c
INNER JOIN (
    SELECT Cou_ID, COUNT(*) as count
    FROM Questions
    GROUP BY Cou_ID
) q ON q.Cou_ID = c.Cou_ID;

-- 修正试题的难度等级，不知道为什么试题难度会出现大于5的情况
UPDATE Questions SET Qus_Diff = 5 WHERE Qus_Diff >= 5;
UPDATE Questions SET Qus_Diff = 1 WHERE Qus_Diff <= 1;
