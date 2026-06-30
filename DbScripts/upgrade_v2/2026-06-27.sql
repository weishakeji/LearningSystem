/*考试增加删除时间字段*/
ALTER TABLE "Examination" ADD COLUMN IF NOT EXISTS "Exam_DeleteTime" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP;
CREATE INDEX  IF NOT EXISTS "Examination_IX_DeleteTime" ON "Examination"("Exam_DeleteTime");
/*是否显示成绩*/
ALTER TABLE "Examination" ADD COLUMN IF NOT EXISTS "Exam_IsShowScore" BOOLEAN NOT NULL DEFAULT TRUE;
CREATE INDEX  IF NOT EXISTS "Examination_IX_IsShowScore" ON "Examination"("Exam_IsShowScore");
/*是否显允许回顾试卷*/
ALTER TABLE "Examination" ADD COLUMN IF NOT EXISTS "Exam_IsAllowReview" BOOLEAN NOT NULL DEFAULT TRUE;
CREATE INDEX  IF NOT EXISTS "Examination_IX_IsAllowReview" ON "Examination"("Exam_IsAllowReview");