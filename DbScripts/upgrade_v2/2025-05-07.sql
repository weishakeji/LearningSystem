/* 修正试题的难度等级，不知道为什么试题难度会出现大于5的情况 */
UPDATE "Questions" SET "Qus_Diff"=5  WHERE "Qus_Diff">=5;
UPDATE "Questions" SET "Qus_Diff"=1  WHERE "Qus_Diff"<=1;