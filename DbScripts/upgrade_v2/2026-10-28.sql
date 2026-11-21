CREATE INDEX  IF NOT EXISTS "StudentSort_IX_Count" ON "StudentSort"("Sts_Count");
-- 为教师职称表，添加教师数量的字段，并添加索引
ALTER TABLE "TeacherSort"  ADD COLUMN "Ths_Count" integer NOT NULL DEFAULT 0;
CREATE INDEX "TeacherSort_IX_Ths_Count" ON "TeacherSort" ("Ths_Count");