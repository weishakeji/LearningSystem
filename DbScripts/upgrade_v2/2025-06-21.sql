/*是否启用AI助教,默认为不启用，即false*/
ALTER TABLE "Course" ADD COLUMN  IF NOT EXISTS "Cou_EnabledAI" boolean NOT NULL DEFAULT false;
    
/*是否采用公域大语言模型,默认为0，即公域大模型*/
ALTER TABLE "Course" ADD COLUMN  IF NOT EXISTS "Cou_AIType"  int NOT NULL DEFAULT 0;

/*智能体的网址*/  
ALTER TABLE "Course" ADD COLUMN IF NOT EXISTS "Cou_AIAgent" VARCHAR(2000) UNIQUE;