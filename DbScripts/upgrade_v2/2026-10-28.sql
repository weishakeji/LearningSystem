CREATE INDEX  IF NOT EXISTS "StudentSort_IX_Count" ON "StudentSort"("Sts_Count");
-- 为教师职称表，添加教师数量的字段，并添加索引
ALTER TABLE "TeacherSort"  ADD COLUMN "Ths_Count" integer NOT NULL DEFAULT 0;
CREATE INDEX "TeacherSort_IX_Ths_Count" ON "TeacherSort" ("Ths_Count");
-- 教师表中的机构名称，可以为空
ALTER TABLE "StudentSort" ALTER COLUMN "Org_Name" DROP NOT NULL;
ALTER TABLE "StudentSort" ALTER COLUMN "Org_Name" SET DEFAULT '';