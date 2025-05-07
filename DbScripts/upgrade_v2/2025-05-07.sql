/* 修正试题的难度等级，不知道为什么会出现大于5的情况 */
UPDATE "Questions" SET "Qus_Diff"=3  WHERE "Qus_Diff"<1 or "Qus_Diff">5