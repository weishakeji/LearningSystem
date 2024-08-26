
/****** 

	-- 创建数据库的表与初始数据 --

	数据库类型：PostgreSQL 16
	
	生成时间：2024-08-26 19:07:36

	提示：
	        您需要手工创建数据库，然后执行下述代码
	        下述代码是创建初始数据，请勿多次执行

******/



-- 创建表 Accessory --
CREATE TABLE IF NOT EXISTS public."Accessory"
(
	"As_Id" integer NOT NULL,
	"As_CrtTime" timestamp without time zone NOT NULL,
	"As_Duration" integer NOT NULL,
	"As_Extension" character varying(255) COLLATE pg_catalog."default",
	"As_FileName" character varying(3000) COLLATE pg_catalog."default",
	"As_Height" integer NOT NULL,
	"As_IsOther" boolean NOT NULL,
	"As_IsOuter" boolean NOT NULL,
	"As_Name" character varying(255) COLLATE pg_catalog."default",
	"As_Size" bigint NOT NULL,
	"As_Type" character varying(50) COLLATE pg_catalog."default",
	"As_Uid" character varying(64) COLLATE pg_catalog."default",
	"As_Width" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_accessory PRIMARY KEY ("As_Id")
);
-- 表 Accessory 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Accessory_As_Id_seq";
ALTER SEQUENCE IF EXISTS public."Accessory_As_Id_seq" OWNED BY public."Accessory"."As_Id";
ALTER TABLE "Accessory" ALTER COLUMN "As_Id" SET DEFAULT NEXTVAL('"Accessory_As_Id_seq"'::regclass);


-- 表 Accessory 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Accessory_aaaaaAccessory_PK" ON public."Accessory" USING btree ("As_Id");
CREATE INDEX IF NOT EXISTS "Accessory_As_Type" ON public."Accessory" USING btree ("As_Type" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_IX_As_Uid" ON public."Accessory" USING btree ("As_Uid" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_As_Uid_Type" ON public."Accessory" USING btree ("As_Uid" DESC, "As_Type" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_IX_Org_ID" ON public."Accessory" USING btree ("Org_ID");

-- 创建表 Accounts --
CREATE TABLE IF NOT EXISTS public."Accounts"
(
	"Ac_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_AddrContact" character varying(255) COLLATE pg_catalog."default",
	"Ac_Address" character varying(255) COLLATE pg_catalog."default",
	"Ac_Age" integer NOT NULL,
	"Ac_Ans" character varying(255) COLLATE pg_catalog."default",
	"Ac_Birthday" timestamp without time zone NOT NULL,
	"Ac_CheckUID" character varying(255) COLLATE pg_catalog."default",
	"Ac_CodeNumber" character varying(50) COLLATE pg_catalog."default",
	"Ac_Coupon" integer NOT NULL,
	"Ac_CurrCourse" integer NOT NULL,
	"Ac_Dingding" character varying(100) COLLATE pg_catalog."default",
	"Ac_Education" character varying(255) COLLATE pg_catalog."default",
	"Ac_Email" character varying(50) COLLATE pg_catalog."default",
	"Ac_IDCardNumber" character varying(50) COLLATE pg_catalog."default",
	"Ac_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Ac_IsOpenMobile" boolean NOT NULL,
	"Ac_IsOpenTel" boolean NOT NULL,
	"Ac_IsPass" boolean NOT NULL,
	"Ac_IsTeacher" boolean NOT NULL,
	"Ac_IsUse" boolean NOT NULL,
	"Ac_Jindie" character varying(100) COLLATE pg_catalog."default",
	"Ac_LastIP" character varying(255) COLLATE pg_catalog."default",
	"Ac_LastTime" timestamp without time zone NOT NULL,
	"Ac_LinkMan" character varying(50) COLLATE pg_catalog."default",
	"Ac_LinkManPhone" character varying(50) COLLATE pg_catalog."default",
	"Ac_Major" character varying(255) COLLATE pg_catalog."default",
	"Ac_MobiTel1" character varying(50) COLLATE pg_catalog."default",
	"Ac_MobiTel2" character varying(50) COLLATE pg_catalog."default",
	"Ac_Money" numeric(18,4) NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Ac_Nation" character varying(50) COLLATE pg_catalog."default",
	"Ac_Native" character varying(255) COLLATE pg_catalog."default",
	"Ac_OutTime" timestamp without time zone NOT NULL,
	"Ac_PID" integer NOT NULL,
	"Ac_Photo" character varying(255) COLLATE pg_catalog."default",
	"Ac_Pinyin" character varying(50) COLLATE pg_catalog."default",
	"Ac_Point" integer NOT NULL,
	"Ac_PointAmount" integer NOT NULL,
	"Ac_Pw" character varying(100) COLLATE pg_catalog."default",
	"Ac_QiyeWeixin" character varying(100) COLLATE pg_catalog."default",
	"Ac_Qq" character varying(50) COLLATE pg_catalog."default",
	"Ac_QqOpenID" character varying(100) COLLATE pg_catalog."default",
	"Ac_Qus" character varying(255) COLLATE pg_catalog."default",
	"Ac_RegTime" timestamp without time zone NOT NULL,
	"Ac_School" character varying(255) COLLATE pg_catalog."default",
	"Ac_Sex" integer NOT NULL,
	"Ac_Signature" character varying(255) COLLATE pg_catalog."default",
	"Ac_Tel" character varying(50) COLLATE pg_catalog."default",
	"Ac_UID" character varying(255) COLLATE pg_catalog."default",
	"Ac_Weixin" character varying(100) COLLATE pg_catalog."default",
	"Ac_WeixinOpenID" character varying(100) COLLATE pg_catalog."default",
	"Ac_Zhifubao" character varying(100) COLLATE pg_catalog."default",
	"Ac_Zip" character varying(50) COLLATE pg_catalog."default",
	"Ac_ZzGongshang" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Sts_ID" bigint NOT NULL,
	"Sts_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_accounts PRIMARY KEY ("Ac_ID")
);
-- 表 Accounts 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Accounts_Ac_ID_seq";
ALTER SEQUENCE IF EXISTS public."Accounts_Ac_ID_seq" OWNED BY public."Accounts"."Ac_ID";
ALTER TABLE "Accounts" ALTER COLUMN "Ac_ID" SET DEFAULT NEXTVAL('"Accounts_Ac_ID_seq"'::regclass);

INSERT INTO "Accounts"("Ac_ID","Ac_AccName","Ac_AddrContact","Ac_Address","Ac_Age","Ac_Ans","Ac_Birthday","Ac_CheckUID","Ac_CodeNumber","Ac_Coupon","Ac_CurrCourse","Ac_Dingding","Ac_Education","Ac_Email","Ac_IDCardNumber","Ac_Intro","Ac_IsOpenMobile","Ac_IsOpenTel","Ac_IsPass","Ac_IsTeacher","Ac_IsUse","Ac_Jindie","Ac_LastIP","Ac_LastTime","Ac_LinkMan","Ac_LinkManPhone","Ac_Major","Ac_MobiTel1","Ac_MobiTel2","Ac_Money","Ac_Name","Ac_Nation","Ac_Native","Ac_OutTime","Ac_PID","Ac_Photo","Ac_Pinyin","Ac_Point","Ac_PointAmount","Ac_Pw","Ac_QiyeWeixin","Ac_Qq","Ac_QqOpenID","Ac_Qus","Ac_RegTime","Ac_School","Ac_Sex","Ac_Signature","Ac_Tel","Ac_UID","Ac_Weixin","Ac_WeixinOpenID","Ac_Zhifubao","Ac_Zip","Ac_ZzGongshang","Dep_Id","Org_ID","Sts_ID","Sts_Name") VALUES (2,'tester','','',1978,'13','1995-03-07 00:00:00','e7d5ac9764e621c908e99265d2ae19df','',1002,84,'','31','666@qq.com','410105199503071228','3333ss',False,False,True,True,True,'','::1','2024-01-22 17:58:18','6','777','111','400 6015615','400 6015615',160.0000,'韩梅梅','','河南省,郑州市,金水区','1753-01-01 00:00:00',0,'523656bef604ea1b2519550ffd952802.jpg','HMM',2434,9358,'c4ca4238a0b923820dcc509a6f75849b','','111','','1在','1753-01-01 00:00:00','',2,'我的签名，测试一下下','400 6015615','0f6305210623cffd6f966db6a3606a1c','1','','','','',0,4,15012714616000001,'默认组d');INSERT INTO "Accounts"("Ac_ID","Ac_AccName","Ac_AddrContact","Ac_Address","Ac_Age","Ac_Ans","Ac_Birthday","Ac_CheckUID","Ac_CodeNumber","Ac_Coupon","Ac_CurrCourse","Ac_Dingding","Ac_Education","Ac_Email","Ac_IDCardNumber","Ac_Intro","Ac_IsOpenMobile","Ac_IsOpenTel","Ac_IsPass","Ac_IsTeacher","Ac_IsUse","Ac_Jindie","Ac_LastIP","Ac_LastTime","Ac_LinkMan","Ac_LinkManPhone","Ac_Major","Ac_MobiTel1","Ac_MobiTel2","Ac_Money","Ac_Name","Ac_Nation","Ac_Native","Ac_OutTime","Ac_PID","Ac_Photo","Ac_Pinyin","Ac_Point","Ac_PointAmount","Ac_Pw","Ac_QiyeWeixin","Ac_Qq","Ac_QqOpenID","Ac_Qus","Ac_RegTime","Ac_School","Ac_Sex","Ac_Signature","Ac_Tel","Ac_UID","Ac_Weixin","Ac_WeixinOpenID","Ac_Zhifubao","Ac_Zip","Ac_ZzGongshang","Dep_Id","Org_ID","Sts_ID","Sts_Name") VALUES (44,'lilei','','',2017,'','2017-09-13 00:00:00','0eddf0907fb4276e31f4e1cee2d9f77a','',0,0,'','81','','','',False,False,True,False,True,'','::1','2017-08-08 16:06:15','','','','18037155756','18037155756',0.0000,'李雷','','','1753-01-01 00:00:00',0,'','LL',1020,1020,'c4ca4238a0b923820dcc509a6f75849b','','55','','','2017-07-16 19:07:57','郑州大学',1,'','','','ss','','','','',0,4,15012714616000001,'默认组d');
-- 表 Accounts 的索引 --
CREATE INDEX IF NOT EXISTS "Accounts_IX_AccName" ON public."Accounts" USING btree ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Birthday" ON public."Accounts" USING btree ("Ac_Birthday" DESC);
CREATE UNIQUE INDEX IF NOT EXISTS "Accounts_aaaaaAccounts_PK" ON public."Accounts" USING btree ("Ac_ID");
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_IDCardNumber" ON public."Accounts" USING btree ("Ac_IDCardNumber" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Login" ON public."Accounts" USING btree ("Ac_IsUse" DESC, "Ac_IsPass" DESC, "Ac_AccName" DESC, "Ac_Pw" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_LastTime" ON public."Accounts" USING btree ("Ac_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_MobiTel1" ON public."Accounts" USING btree ("Ac_MobiTel1" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Name" ON public."Accounts" USING btree ("Ac_Name" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Org_ID" ON public."Accounts" USING btree ("Org_ID");
CREATE INDEX IF NOT EXISTS "Accounts_IX_Sts_ID" ON public."Accounts" USING btree ("Sts_ID" DESC);

-- 创建表 Article --
CREATE TABLE IF NOT EXISTS public."Article"
(
	"Art_ID" bigint NOT NULL,
	"Acc_Id" integer NOT NULL,
	"Acc_Name" character varying(255) COLLATE pg_catalog."default",
	"Art_Author" character varying(50) COLLATE pg_catalog."default",
	"Art_Color" character varying(50) COLLATE pg_catalog."default",
	"Art_CrtTime" timestamp without time zone NOT NULL,
	"Art_Descr" character varying(255) COLLATE pg_catalog."default",
	"Art_Details" text,
	"Art_Endnote" text,
	"Art_ErrInfo" character varying(255) COLLATE pg_catalog."default",
	"Art_Font" character varying(50) COLLATE pg_catalog."default",
	"Art_Intro" text,
	"Art_IsDel" boolean NOT NULL,
	"Art_IsError" boolean NOT NULL,
	"Art_IsHot" boolean NOT NULL,
	"Art_IsImg" boolean NOT NULL,
	"Art_IsNote" boolean NOT NULL,
	"Art_IsOut" boolean NOT NULL,
	"Art_IsRec" boolean NOT NULL,
	"Art_IsShow" boolean NOT NULL,
	"Art_IsStatic" boolean NOT NULL,
	"Art_IsTop" boolean NOT NULL,
	"Art_IsUse" boolean NOT NULL,
	"Art_IsVerify" boolean NOT NULL,
	"Art_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Art_Label" character varying(255) COLLATE pg_catalog."default",
	"Art_LastTime" timestamp without time zone NOT NULL,
	"Art_Logo" character varying(255) COLLATE pg_catalog."default",
	"Art_Number" integer NOT NULL,
	"Art_OutUrl" character varying(255) COLLATE pg_catalog."default",
	"Art_PushTime" timestamp without time zone NOT NULL,
	"Art_Source" character varying(100) COLLATE pg_catalog."default",
	"Art_Title" character varying(255) COLLATE pg_catalog."default",
	"Art_TitleAbbr" character varying(50) COLLATE pg_catalog."default",
	"Art_TitleFull" character varying(255) COLLATE pg_catalog."default",
	"Art_TitleSub" character varying(255) COLLATE pg_catalog."default",
	"Art_Uid" character varying(64) COLLATE pg_catalog."default",
	"Art_VerifyMan" character varying(50) COLLATE pg_catalog."default",
	"Art_VerifyTime" timestamp without time zone NOT NULL,
	"Col_Name" character varying(255) COLLATE pg_catalog."default",
	"Col_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"OtherData" text,
	 CONSTRAINT key_article PRIMARY KEY ("Art_ID")
);


-- 创建表 Columns --
CREATE TABLE IF NOT EXISTS public."Columns"
(
	"Col_ID" integer NOT NULL,
	"Col_ByName" character varying(255) COLLATE pg_catalog."default",
	"Col_CrtTime" timestamp without time zone NOT NULL,
	"Col_Descr" character varying(255) COLLATE pg_catalog."default",
	"Col_Intro" text,
	"Col_IsChildren" boolean NOT NULL,
	"Col_IsNote" boolean NOT NULL,
	"Col_IsUse" boolean NOT NULL,
	"Col_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Col_Name" character varying(255) COLLATE pg_catalog."default",
	"Col_PID" character varying(255) COLLATE pg_catalog."default",
	"Col_Tax" integer NOT NULL,
	"Col_Title" character varying(255) COLLATE pg_catalog."default",
	"Col_Type" character varying(255) COLLATE pg_catalog."default",
	"Col_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_columns PRIMARY KEY ("Col_ID")
);
-- 表 Columns 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Columns_Col_ID_seq";
ALTER SEQUENCE IF EXISTS public."Columns_Col_ID_seq" OWNED BY public."Columns"."Col_ID";
ALTER TABLE "Columns" ALTER COLUMN "Col_ID" SET DEFAULT NEXTVAL('"Columns_Col_ID_seq"'::regclass);


-- 表 Columns 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Columns_aaaaaColumns_PK" ON public."Columns" USING btree ("Col_ID");

-- 创建表 CouponAccount --
CREATE TABLE IF NOT EXISTS public."CouponAccount"
(
	"Ca_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Ca_CrtTime" timestamp without time zone NOT NULL,
	"Ca_From" integer NOT NULL,
	"Ca_Info" character varying(500) COLLATE pg_catalog."default",
	"Ca_Remark" character varying(1000) COLLATE pg_catalog."default",
	"Ca_Serial" character varying(100) COLLATE pg_catalog."default",
	"Ca_Source" character varying(200) COLLATE pg_catalog."default",
	"Ca_Total" integer NOT NULL,
	"Ca_TotalAmount" integer NOT NULL,
	"Ca_Type" integer NOT NULL,
	"Ca_Value" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Rc_Code" character varying(100) COLLATE pg_catalog."default",
	 CONSTRAINT key_couponaccount PRIMARY KEY ("Ca_ID")
);
-- 表 CouponAccount 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."CouponAccount_Ca_ID_seq";
ALTER SEQUENCE IF EXISTS public."CouponAccount_Ca_ID_seq" OWNED BY public."CouponAccount"."Ca_ID";
ALTER TABLE "CouponAccount" ALTER COLUMN "Ca_ID" SET DEFAULT NEXTVAL('"CouponAccount_Ca_ID_seq"'::regclass);



-- 创建表 Course --
CREATE TABLE IF NOT EXISTS public."Course"
(
	"Cou_ID" bigint NOT NULL,
	"Cou_Allowedit" boolean NOT NULL,
	"Cou_Content" text,
	"Cou_CrtTime" timestamp without time zone NOT NULL,
	"Cou_ExistExam" boolean NOT NULL,
	"Cou_ExistLive" boolean NOT NULL,
	"Cou_ExistQues" boolean NOT NULL,
	"Cou_FreeEnd" timestamp without time zone NOT NULL,
	"Cou_FreeStart" timestamp without time zone NOT NULL,
	"Cou_Intro" text,
	"Cou_IsFree" boolean NOT NULL,
	"Cou_IsLimitFree" boolean NOT NULL,
	"Cou_IsRec" boolean NOT NULL,
	"Cou_IsStudy" boolean NOT NULL,
	"Cou_IsTry" boolean NOT NULL,
	"Cou_IsUse" boolean NOT NULL,
	"Cou_Level" integer NOT NULL,
	"Cou_Logo" character varying(100) COLLATE pg_catalog."default",
	"Cou_LogoSmall" character varying(100) COLLATE pg_catalog."default",
	"Cou_Name" character varying(100) COLLATE pg_catalog."default",
	"Cou_PID" bigint NOT NULL,
	"Cou_Price" integer NOT NULL,
	"Cou_PriceSpan" integer NOT NULL,
	"Cou_PriceUnit" character varying(100) COLLATE pg_catalog."default",
	"Cou_Score" integer NOT NULL,
	"Cou_StudentSum" integer NOT NULL,
	"Cou_Target" character varying(1000) COLLATE pg_catalog."default",
	"Cou_Tax" integer NOT NULL,
	"Cou_TryNum" integer NOT NULL,
	"Cou_Type" integer NOT NULL,
	"Cou_UID" character varying(100) COLLATE pg_catalog."default",
	"Cou_ViewNum" integer NOT NULL,
	"Cou_XPath" character varying(255) COLLATE pg_catalog."default",
	"Dep_CnName" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) NOT NULL COLLATE pg_catalog."default",
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(50) COLLATE pg_catalog."default",
	 CONSTRAINT key_course PRIMARY KEY ("Cou_ID")
);

-- 表 Course 的索引 --
CREATE INDEX IF NOT EXISTS "Course_IX_Org_ID" ON public."Course" USING btree ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Sbj_ID" ON public."Course" USING btree ("Sbj_ID" DESC);

-- 创建表 CoursePrice --
CREATE TABLE IF NOT EXISTS public."CoursePrice"
(
	"CP_ID" integer NOT NULL,
	"CP_Coupon" integer NOT NULL,
	"CP_Group" character varying(100) COLLATE pg_catalog."default",
	"CP_IsUse" boolean NOT NULL,
	"CP_Price" integer NOT NULL,
	"CP_Span" integer NOT NULL,
	"CP_Tax" integer NOT NULL,
	"CP_Unit" character varying(100) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Cou_UID" character varying(100) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_courseprice PRIMARY KEY ("CP_ID")
);
-- 表 CoursePrice 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."CoursePrice_CP_ID_seq";
ALTER SEQUENCE IF EXISTS public."CoursePrice_CP_ID_seq" OWNED BY public."CoursePrice"."CP_ID";
ALTER TABLE "CoursePrice" ALTER COLUMN "CP_ID" SET DEFAULT NEXTVAL('"CoursePrice_CP_ID_seq"'::regclass);



-- 创建表 Depart --
CREATE TABLE IF NOT EXISTS public."Depart"
(
	"Dep_Id" integer NOT NULL,
	"Dep_CnAbbr" character varying(50) COLLATE pg_catalog."default",
	"Dep_CnName" character varying(100) NOT NULL COLLATE pg_catalog."default",
	"Dep_Code" character varying(50) COLLATE pg_catalog."default",
	"Dep_Count" integer NOT NULL,
	"Dep_Email" character varying(255) COLLATE pg_catalog."default",
	"Dep_EnAbbr" character varying(50) COLLATE pg_catalog."default",
	"Dep_EnName" character varying(255) COLLATE pg_catalog."default",
	"Dep_Fax" character varying(255) COLLATE pg_catalog."default",
	"Dep_Func" character varying(1000) COLLATE pg_catalog."default",
	"Dep_IsAdmin" boolean NOT NULL,
	"Dep_IsShow" boolean NOT NULL,
	"Dep_IsUse" boolean NOT NULL,
	"Dep_Level" integer NOT NULL,
	"Dep_Msn" character varying(255) COLLATE pg_catalog."default",
	"Dep_PatId" integer NOT NULL,
	"Dep_Phone" character varying(255) COLLATE pg_catalog."default",
	"Dep_State" boolean NOT NULL,
	"Dep_Tax" integer NOT NULL,
	"Dep_WorkAddr" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_depart PRIMARY KEY ("Dep_Id")
);
-- 表 Depart 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Depart_Dep_Id_seq";
ALTER SEQUENCE IF EXISTS public."Depart_Dep_Id_seq" OWNED BY public."Depart"."Dep_Id";
ALTER TABLE "Depart" ALTER COLUMN "Dep_Id" SET DEFAULT NEXTVAL('"Depart_Dep_Id_seq"'::regclass);


-- 表 Depart 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Depart_aaaaaDepart_PK" ON public."Depart" USING btree ("Dep_Id");

-- 创建表 EmpAcc_Group --
CREATE TABLE IF NOT EXISTS public."EmpAcc_Group"
(
	"Emgr_Id" integer NOT NULL,
	"Acc_Id" integer NOT NULL,
	"EGrp_Id" integer NOT NULL,
	"Org_Id" integer NOT NULL,
	 CONSTRAINT key_empacc_group PRIMARY KEY ("Emgr_Id")
);
-- 表 EmpAcc_Group 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."EmpAcc_Group_Emgr_Id_seq";
ALTER SEQUENCE IF EXISTS public."EmpAcc_Group_Emgr_Id_seq" OWNED BY public."EmpAcc_Group"."Emgr_Id";
ALTER TABLE "EmpAcc_Group" ALTER COLUMN "Emgr_Id" SET DEFAULT NEXTVAL('"EmpAcc_Group_Emgr_Id_seq"'::regclass);


-- 表 EmpAcc_Group 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "EmpAcc_Group_aaaaaEmpAcc_Group_PK" ON public."EmpAcc_Group" USING btree ("Emgr_Id");

-- 创建表 EmpAccount --
CREATE TABLE IF NOT EXISTS public."EmpAccount"
(
	"Acc_Id" integer NOT NULL,
	"Acc_AccName" character varying(255) NOT NULL COLLATE pg_catalog."default",
	"Acc_Age" integer NOT NULL,
	"Acc_Ans" character varying(255) COLLATE pg_catalog."default",
	"Acc_Birthday" timestamp without time zone NOT NULL,
	"Acc_CheckUID" character varying(255) COLLATE pg_catalog."default",
	"Acc_Email" character varying(100) COLLATE pg_catalog."default",
	"Acc_EmpCode" character varying(255) COLLATE pg_catalog."default",
	"Acc_IDCardNumber" character varying(18) COLLATE pg_catalog."default",
	"Acc_IsAutoOut" boolean NOT NULL,
	"Acc_IsOpenMobile" boolean NOT NULL,
	"Acc_IsOpenTel" boolean NOT NULL,
	"Acc_IsPartTime" boolean NOT NULL,
	"Acc_IsUse" boolean NOT NULL,
	"Acc_IsUseCard" boolean NOT NULL,
	"Acc_LastTime" timestamp without time zone NOT NULL,
	"Acc_MobileTel" character varying(50) COLLATE pg_catalog."default",
	"Acc_Name" character varying(50) COLLATE pg_catalog."default",
	"Acc_NamePinyin" character varying(255) COLLATE pg_catalog."default",
	"Acc_OutTime" timestamp without time zone NOT NULL,
	"Acc_Photo" character varying(255) COLLATE pg_catalog."default",
	"Acc_Pw" character varying(255) COLLATE pg_catalog."default",
	"Acc_QQ" character varying(50) COLLATE pg_catalog."default",
	"Acc_Qus" character varying(255) COLLATE pg_catalog."default",
	"Acc_RegTime" timestamp without time zone NOT NULL,
	"Acc_Sex" integer NOT NULL,
	"Acc_Signature" character varying(255) COLLATE pg_catalog."default",
	"Acc_Tel" character varying(50) COLLATE pg_catalog."default",
	"Acc_Weixin" character varying(255) COLLATE pg_catalog."default",
	"Dep_CnName" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"EGrp_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Posi_Id" integer NOT NULL,
	"Posi_Name" character varying(255) COLLATE pg_catalog."default",
	"Title_Id" integer NOT NULL,
	"Title_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_empaccount PRIMARY KEY ("Acc_Id")
);
-- 表 EmpAccount 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."EmpAccount_Acc_Id_seq";
ALTER SEQUENCE IF EXISTS public."EmpAccount_Acc_Id_seq" OWNED BY public."EmpAccount"."Acc_Id";
ALTER TABLE "EmpAccount" ALTER COLUMN "Acc_Id" SET DEFAULT NEXTVAL('"EmpAccount_Acc_Id_seq"'::regclass);

INSERT INTO "EmpAccount"("Acc_Id","Acc_AccName","Acc_Age","Acc_Ans","Acc_Birthday","Acc_CheckUID","Acc_Email","Acc_EmpCode","Acc_IDCardNumber","Acc_IsAutoOut","Acc_IsOpenMobile","Acc_IsOpenTel","Acc_IsPartTime","Acc_IsUse","Acc_IsUseCard","Acc_LastTime","Acc_MobileTel","Acc_Name","Acc_NamePinyin","Acc_OutTime","Acc_Photo","Acc_Pw","Acc_QQ","Acc_Qus","Acc_RegTime","Acc_Sex","Acc_Signature","Acc_Tel","Acc_Weixin","Dep_CnName","Dep_Id","EGrp_Id","Org_ID","Org_Name","Posi_Id","Posi_Name","Title_Id","Title_Name") VALUES (23,'admin',1017,'没钱','2021-12-10 00:00:00','f4c257d95d03531475a33d2e7fbb8b0f','','','',False,False,False,False,True,False,'2020-11-11 22:32:13','123','机构管理员','JGGLY','3017-01-04 20:44:42','','c4ca4238a0b923820dcc509a6f75849b','','我口袋里有几块钱？','2017-01-04 20:44:42',1,'','','','科长',0,0,4,'郑州微厦计算机科技有限公司',10,'管理员',11,'科长');INSERT INTO "EmpAccount"("Acc_Id","Acc_AccName","Acc_Age","Acc_Ans","Acc_Birthday","Acc_CheckUID","Acc_Email","Acc_EmpCode","Acc_IDCardNumber","Acc_IsAutoOut","Acc_IsOpenMobile","Acc_IsOpenTel","Acc_IsPartTime","Acc_IsUse","Acc_IsUseCard","Acc_LastTime","Acc_MobileTel","Acc_Name","Acc_NamePinyin","Acc_OutTime","Acc_Photo","Acc_Pw","Acc_QQ","Acc_Qus","Acc_RegTime","Acc_Sex","Acc_Signature","Acc_Tel","Acc_Weixin","Dep_CnName","Dep_Id","EGrp_Id","Org_ID","Org_Name","Posi_Id","Posi_Name","Title_Id","Title_Name") VALUES (1,'super',1978,'南小','1753-01-01 00:00:00','1df117cdba1dfd53596ab3a154e06014','5','A01','',False,True,True,False,True,False,'2024-08-26 19:06:32','4006015615','超管','CG2','1753-01-01 00:00:00','','c4ca4238a0b923820dcc509a6f75849b','19303340','我就读的第一所学校的名称？','2005-01-12 00:00:00',1,'','888','','核心开发部',32,0,2,'郑州微厦计算机科技有限公司',3,'管理员',3,'系统架构师');
-- 表 EmpAccount 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "EmpAccount_aaaaaEmpAccount_PK" ON public."EmpAccount" USING btree ("Acc_Id");

-- 创建表 EmpGroup --
CREATE TABLE IF NOT EXISTS public."EmpGroup"
(
	"EGrp_Id" integer NOT NULL,
	"EGrp_Intro" character varying(255) COLLATE pg_catalog."default",
	"EGrp_IsSystem" boolean NOT NULL,
	"EGrp_IsUse" boolean NOT NULL,
	"EGrp_Name" character varying(255) COLLATE pg_catalog."default",
	"EGrp_Tax" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_empgroup PRIMARY KEY ("EGrp_Id")
);
-- 表 EmpGroup 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."EmpGroup_EGrp_Id_seq";
ALTER SEQUENCE IF EXISTS public."EmpGroup_EGrp_Id_seq" OWNED BY public."EmpGroup"."EGrp_Id";
ALTER TABLE "EmpGroup" ALTER COLUMN "EGrp_Id" SET DEFAULT NEXTVAL('"EmpGroup_EGrp_Id_seq"'::regclass);

INSERT INTO "EmpGroup"("EGrp_Id","EGrp_Intro","EGrp_IsSystem","EGrp_IsUse","EGrp_Name","EGrp_Tax","Org_ID","Org_Name") VALUES (1,'d',False,True,'测试一',2,2,'');INSERT INTO "EmpGroup"("EGrp_Id","EGrp_Intro","EGrp_IsSystem","EGrp_IsUse","EGrp_Name","EGrp_Tax","Org_ID","Org_Name") VALUES (2,'',False,True,'测试二',1,2,'');
-- 表 EmpGroup 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "EmpGroup_aaaaaEmpGroup_PK" ON public."EmpGroup" USING btree ("EGrp_Id");

-- 创建表 EmpTitle --
CREATE TABLE IF NOT EXISTS public."EmpTitle"
(
	"Title_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Title_Intro" character varying(255) COLLATE pg_catalog."default",
	"Title_IsUse" boolean NOT NULL,
	"Title_Name" character varying(255) COLLATE pg_catalog."default",
	"Title_Tax" integer NOT NULL,
	 CONSTRAINT key_emptitle PRIMARY KEY ("Title_Id")
);
-- 表 EmpTitle 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."EmpTitle_Title_Id_seq";
ALTER SEQUENCE IF EXISTS public."EmpTitle_Title_Id_seq" OWNED BY public."EmpTitle"."Title_Id";
ALTER TABLE "EmpTitle" ALTER COLUMN "Title_Id" SET DEFAULT NEXTVAL('"EmpTitle_Title_Id_seq"'::regclass);

INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (1,2,'','',True,'大区经理',1);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (2,2,'','',True,'测试工程师',4);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (3,2,'','',True,'系统架构师',2);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (4,2,'','',True,'招商经理',3);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (9,4,'郑州微厦计算机科技有限公司','',True,'院长',2);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (10,4,'郑州微厦计算机科技有限公司','',True,'主任',1);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (11,4,'郑州微厦计算机科技有限公司','',True,'科长',5);INSERT INTO "EmpTitle"("Title_Id","Org_ID","Org_Name","Title_Intro","Title_IsUse","Title_Name","Title_Tax") VALUES (12,4,'郑州微厦计算机科技有限公司','',True,'处长',6);
-- 表 EmpTitle 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "EmpTitle_aaaaaEmpTitle_PK" ON public."EmpTitle" USING btree ("Title_Id");

-- 创建表 ExamGroup --
CREATE TABLE IF NOT EXISTS public."ExamGroup"
(
	"Eg_ID" integer NOT NULL,
	"Eg_Type" integer NOT NULL,
	"Exam_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sts_ID" bigint NOT NULL,
	 CONSTRAINT key_examgroup PRIMARY KEY ("Eg_ID")
);
-- 表 ExamGroup 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ExamGroup_Eg_ID_seq";
ALTER SEQUENCE IF EXISTS public."ExamGroup_Eg_ID_seq" OWNED BY public."ExamGroup"."Eg_ID";
ALTER TABLE "ExamGroup" ALTER COLUMN "Eg_ID" SET DEFAULT NEXTVAL('"ExamGroup_Eg_ID_seq"'::regclass);


-- 表 ExamGroup 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "ExamGroup_aaaaaExamGroup_PK" ON public."ExamGroup" USING btree ("Eg_ID");

-- 创建表 ExamResults --
CREATE TABLE IF NOT EXISTS public."ExamResults"
(
	"Exr_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Ac_IDCardNumber" character varying(50) COLLATE pg_catalog."default",
	"Ac_Name" character varying(255) COLLATE pg_catalog."default",
	"Ac_Sex" integer NOT NULL,
	"Dep_Id" integer NOT NULL,
	"Exam_ID" integer NOT NULL,
	"Exam_Name" character varying(255) COLLATE pg_catalog."default",
	"Exam_Title" character varying(255) COLLATE pg_catalog."default",
	"Exam_UID" character varying(255) COLLATE pg_catalog."default",
	"Exr_CalcTime" timestamp without time zone NOT NULL,
	"Exr_Colligate" real NOT NULL,
	"Exr_CrtTime" timestamp without time zone NOT NULL,
	"Exr_Draw" real NOT NULL,
	"Exr_IP" character varying(255) COLLATE pg_catalog."default",
	"Exr_IsCalc" boolean NOT NULL,
	"Exr_IsManual" boolean NOT NULL,
	"Exr_IsSubmit" boolean NOT NULL,
	"Exr_LastTime" timestamp without time zone NOT NULL,
	"Exr_Mac" character varying(255) COLLATE pg_catalog."default",
	"Exr_OverTime" timestamp without time zone NOT NULL,
	"Exr_Results" text,
	"Exr_Score" real NOT NULL,
	"Exr_ScoreFinal" real NOT NULL,
	"Exr_SubmitTime" timestamp without time zone NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"Sts_ID" bigint NOT NULL,
	"Tp_Id" bigint NOT NULL,
	 CONSTRAINT key_examresults PRIMARY KEY ("Exr_ID")
);
-- 表 ExamResults 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ExamResults_Exr_ID_seq";
ALTER SEQUENCE IF EXISTS public."ExamResults_Exr_ID_seq" OWNED BY public."ExamResults"."Exr_ID";
ALTER TABLE "ExamResults" ALTER COLUMN "Exr_ID" SET DEFAULT NEXTVAL('"ExamResults_Exr_ID_seq"'::regclass);


-- 表 ExamResults 的索引 --
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Ac_ID" ON public."ExamResults" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Exam_ID" ON public."ExamResults" USING btree ("Exam_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Exr_CrtTime" ON public."ExamResults" USING btree ("Exr_CrtTime");
CREATE UNIQUE INDEX IF NOT EXISTS "ExamResults_aaaaaExamResults_PK" ON public."ExamResults" USING btree ("Exr_ID");
CREATE INDEX IF NOT EXISTS "ExamResults_IX_IsSubmit" ON public."ExamResults" USING btree ("Exr_IsSubmit" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_OverTime" ON public."ExamResults" USING btree ("Exr_OverTime" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Score" ON public."ExamResults" USING btree ("Exr_ScoreFinal" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Org_ID" ON public."ExamResults" USING btree ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Sts_ID" ON public."ExamResults" USING btree ("Sts_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Tp_Id" ON public."ExamResults" USING btree ("Tp_Id" DESC);

-- 创建表 Examination --
CREATE TABLE IF NOT EXISTS public."Examination"
(
	"Exam_ID" integer NOT NULL,
	"Exam_CrtTime" timestamp without time zone NOT NULL,
	"Exam_Date" timestamp without time zone NOT NULL,
	"Exam_DateOver" timestamp without time zone NOT NULL,
	"Exam_DateType" integer NOT NULL,
	"Exam_GroupType" integer NOT NULL,
	"Exam_Intro" text,
	"Exam_IsRightClick" boolean NOT NULL,
	"Exam_IsShowBtn" boolean NOT NULL,
	"Exam_IsTheme" boolean NOT NULL,
	"Exam_IsToggle" boolean NOT NULL,
	"Exam_IsUse" boolean NOT NULL,
	"Exam_Monitor" character varying(255) COLLATE pg_catalog."default",
	"Exam_Name" character varying(255) COLLATE pg_catalog."default",
	"Exam_PassScore" integer NOT NULL,
	"Exam_Span" integer NOT NULL,
	"Exam_Tax" integer NOT NULL,
	"Exam_Title" character varying(255) COLLATE pg_catalog."default",
	"Exam_Total" integer NOT NULL,
	"Exam_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(255) COLLATE pg_catalog."default",
	"Tp_Id" bigint NOT NULL,
	 CONSTRAINT key_examination PRIMARY KEY ("Exam_ID")
);
-- 表 Examination 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Examination_Exam_ID_seq";
ALTER SEQUENCE IF EXISTS public."Examination_Exam_ID_seq" OWNED BY public."Examination"."Exam_ID";
ALTER TABLE "Examination" ALTER COLUMN "Exam_ID" SET DEFAULT NEXTVAL('"Examination_Exam_ID_seq"'::regclass);


-- 表 Examination 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Examination_aaaaaExamination_PK" ON public."Examination" USING btree ("Exam_ID");

-- 创建表 FuncPoint --
CREATE TABLE IF NOT EXISTS public."FuncPoint"
(
	"FPI_Id" integer NOT NULL,
	"FPI_IsShow" boolean NOT NULL,
	"FPI_IsUse" boolean NOT NULL,
	"FPI_Name" character varying(50) NOT NULL COLLATE pg_catalog."default",
	"Org_Id" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_funcpoint PRIMARY KEY ("FPI_Id")
);
-- 表 FuncPoint 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."FuncPoint_FPI_Id_seq";
ALTER SEQUENCE IF EXISTS public."FuncPoint_FPI_Id_seq" OWNED BY public."FuncPoint"."FPI_Id";
ALTER TABLE "FuncPoint" ALTER COLUMN "FPI_Id" SET DEFAULT NEXTVAL('"FuncPoint_FPI_Id_seq"'::regclass);

INSERT INTO "FuncPoint"("FPI_Id","FPI_IsShow","FPI_IsUse","FPI_Name","Org_Id","Org_Name") VALUES (2,True,True,'新增',0,'');INSERT INTO "FuncPoint"("FPI_Id","FPI_IsShow","FPI_IsUse","FPI_Name","Org_Id","Org_Name") VALUES (3,True,True,'修改',0,'');INSERT INTO "FuncPoint"("FPI_Id","FPI_IsShow","FPI_IsUse","FPI_Name","Org_Id","Org_Name") VALUES (4,True,True,'删除',0,'');INSERT INTO "FuncPoint"("FPI_Id","FPI_IsShow","FPI_IsUse","FPI_Name","Org_Id","Org_Name") VALUES (5,True,True,'审核',0,'');
-- 表 FuncPoint 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "FuncPoint_aaaaaFuncPoint_PK" ON public."FuncPoint" USING btree ("FPI_Id");

-- 创建表 Guide --
CREATE TABLE IF NOT EXISTS public."Guide"
(
	"Gu_ID" bigint NOT NULL,
	"Acc_Id" integer NOT NULL,
	"Acc_Name" character varying(255) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Cou_Name" character varying(255) COLLATE pg_catalog."default",
	"Gc_Title" character varying(255) COLLATE pg_catalog."default",
	"Gc_UID" character varying(255) COLLATE pg_catalog."default",
	"Gu_Author" character varying(50) COLLATE pg_catalog."default",
	"Gu_Color" character varying(50) COLLATE pg_catalog."default",
	"Gu_CrtTime" timestamp without time zone,
	"Gu_Descr" character varying(255) COLLATE pg_catalog."default",
	"Gu_Details" text,
	"Gu_Endnote" text,
	"Gu_ErrInfo" character varying(255) COLLATE pg_catalog."default",
	"Gu_Font" character varying(50) COLLATE pg_catalog."default",
	"Gu_Intro" text,
	"Gu_IsDel" boolean NOT NULL,
	"Gu_IsError" boolean NOT NULL,
	"Gu_IsHot" boolean NOT NULL,
	"Gu_IsImg" boolean NOT NULL,
	"Gu_IsNote" boolean NOT NULL,
	"Gu_IsOut" boolean NOT NULL,
	"Gu_IsRec" boolean NOT NULL,
	"Gu_IsShow" boolean NOT NULL,
	"Gu_IsStatic" boolean NOT NULL,
	"Gu_IsTop" boolean NOT NULL,
	"Gu_IsUse" boolean NOT NULL,
	"Gu_IsVerify" boolean NOT NULL,
	"Gu_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Gu_Label" character varying(255) COLLATE pg_catalog."default",
	"Gu_LastTime" timestamp without time zone,
	"Gu_Logo" character varying(255) COLLATE pg_catalog."default",
	"Gu_Number" integer NOT NULL,
	"Gu_OutUrl" character varying(255) COLLATE pg_catalog."default",
	"Gu_PushTime" timestamp without time zone,
	"Gu_Source" character varying(100) COLLATE pg_catalog."default",
	"Gu_Title" character varying(255) COLLATE pg_catalog."default",
	"Gu_TitleAbbr" character varying(50) COLLATE pg_catalog."default",
	"Gu_TitleFull" character varying(255) COLLATE pg_catalog."default",
	"Gu_TitleSub" character varying(255) COLLATE pg_catalog."default",
	"Gu_Uid" character varying(64) COLLATE pg_catalog."default",
	"Gu_VerifyMan" character varying(50) COLLATE pg_catalog."default",
	"Gu_VerifyTime" timestamp without time zone,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"OtherData" text,
	 CONSTRAINT key_guide PRIMARY KEY ("Gu_ID")
);

-- 表 Guide 的索引 --
CREATE INDEX IF NOT EXISTS "Guide_IX_Cou_ID" ON public."Guide" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Guide_IX_IsShow" ON public."Guide" USING btree ("Gu_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Guide_IX_IsUse" ON public."Guide" USING btree ("Gu_IsUse" DESC);

-- 创建表 GuideColumns --
CREATE TABLE IF NOT EXISTS public."GuideColumns"
(
	"Gc_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Cou_Name" character varying(255) COLLATE pg_catalog."default",
	"Gc_ByName" character varying(255) COLLATE pg_catalog."default",
	"Gc_CrtTime" timestamp without time zone NOT NULL,
	"Gc_Descr" character varying(255) COLLATE pg_catalog."default",
	"Gc_Intro" text,
	"Gc_IsNote" boolean NOT NULL,
	"Gc_IsUse" boolean NOT NULL,
	"Gc_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Gc_PID" character varying(255) COLLATE pg_catalog."default",
	"Gc_Tax" integer NOT NULL,
	"Gc_Title" character varying(255) COLLATE pg_catalog."default",
	"Gc_Type" character varying(255) COLLATE pg_catalog."default",
	"Gc_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_guidecolumns PRIMARY KEY ("Gc_ID")
);
-- 表 GuideColumns 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."GuideColumns_Gc_ID_seq";
ALTER SEQUENCE IF EXISTS public."GuideColumns_Gc_ID_seq" OWNED BY public."GuideColumns"."Gc_ID";
ALTER TABLE "GuideColumns" ALTER COLUMN "Gc_ID" SET DEFAULT NEXTVAL('"GuideColumns_Gc_ID_seq"'::regclass);


-- 表 GuideColumns 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "GuideColumns_aaaaaGuideColumns_PK" ON public."GuideColumns" USING btree ("Gc_ID");

-- 创建表 InternalLink --
CREATE TABLE IF NOT EXISTS public."InternalLink"
(
	"IL_ID" integer NOT NULL,
	"IL_CrtTime" timestamp without time zone,
	"IL_IsUse" boolean NOT NULL,
	"IL_Name" character varying(255) COLLATE pg_catalog."default",
	"IL_Target" character varying(255) COLLATE pg_catalog."default",
	"IL_Title" character varying(255) COLLATE pg_catalog."default",
	"IL_Url" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_internallink PRIMARY KEY ("IL_ID")
);
-- 表 InternalLink 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."InternalLink_IL_ID_seq";
ALTER SEQUENCE IF EXISTS public."InternalLink_IL_ID_seq" OWNED BY public."InternalLink"."IL_ID";
ALTER TABLE "InternalLink" ALTER COLUMN "IL_ID" SET DEFAULT NEXTVAL('"InternalLink_IL_ID_seq"'::regclass);


-- 表 InternalLink 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "InternalLink_aaaaaInternalLink_PK" ON public."InternalLink" USING btree ("IL_ID");

-- 创建表 Knowledge --
CREATE TABLE IF NOT EXISTS public."Knowledge"
(
	"Kn_ID" bigint NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Kn_Author" character varying(50) COLLATE pg_catalog."default",
	"Kn_CrtTime" timestamp without time zone,
	"Kn_Descr" character varying(255) COLLATE pg_catalog."default",
	"Kn_Details" text,
	"Kn_Intro" text,
	"Kn_IsDel" boolean NOT NULL,
	"Kn_IsHot" boolean NOT NULL,
	"Kn_IsNote" boolean NOT NULL,
	"Kn_IsRec" boolean NOT NULL,
	"Kn_IsTop" boolean NOT NULL,
	"Kn_IsUse" boolean NOT NULL,
	"Kn_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Kn_Label" character varying(255) COLLATE pg_catalog."default",
	"Kn_LastTime" timestamp without time zone,
	"Kn_Logo" character varying(255) COLLATE pg_catalog."default",
	"Kn_Number" integer NOT NULL,
	"Kn_Source" character varying(100) COLLATE pg_catalog."default",
	"Kn_Title" character varying(255) COLLATE pg_catalog."default",
	"Kn_TitleFull" character varying(255) COLLATE pg_catalog."default",
	"Kn_TitleSub" character varying(255) COLLATE pg_catalog."default",
	"Kn_Uid" character varying(64) COLLATE pg_catalog."default",
	"Kns_ID" bigint NOT NULL,
	"Kns_Name" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"OtherData" text,
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_knowledge PRIMARY KEY ("Kn_ID")
);


-- 创建表 KnowledgeSort --
CREATE TABLE IF NOT EXISTS public."KnowledgeSort"
(
	"Kns_ID" bigint NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Kns_ByName" character varying(255) COLLATE pg_catalog."default",
	"Kns_CrtTime" timestamp without time zone,
	"Kns_Intro" character varying(255) COLLATE pg_catalog."default",
	"Kns_IsUse" boolean NOT NULL,
	"Kns_Name" character varying(50) COLLATE pg_catalog."default",
	"Kns_PID" bigint NOT NULL,
	"Kns_Tax" integer NOT NULL,
	"Kns_Type" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_knowledgesort PRIMARY KEY ("Kns_ID")
);


-- 创建表 LearningCard --
CREATE TABLE IF NOT EXISTS public."LearningCard"
(
	"Lc_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Lc_Code" character varying(100) COLLATE pg_catalog."default",
	"Lc_Coupon" integer NOT NULL,
	"Lc_CrtTime" timestamp without time zone NOT NULL,
	"Lc_IsEnable" boolean NOT NULL,
	"Lc_IsUsed" boolean NOT NULL,
	"Lc_LimitEnd" timestamp without time zone NOT NULL,
	"Lc_LimitStart" timestamp without time zone NOT NULL,
	"Lc_Price" real NOT NULL,
	"Lc_Pw" character varying(50) COLLATE pg_catalog."default",
	"Lc_QrcodeBase64" text,
	"Lc_ReceiveTime" timestamp without time zone NOT NULL,
	"Lc_Span" integer NOT NULL,
	"Lc_State" integer NOT NULL,
	"Lc_UsedTime" timestamp without time zone NOT NULL,
	"Lcs_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_learningcard PRIMARY KEY ("Lc_ID")
);
-- 表 LearningCard 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LearningCard_Lc_ID_seq";
ALTER SEQUENCE IF EXISTS public."LearningCard_Lc_ID_seq" OWNED BY public."LearningCard"."Lc_ID";
ALTER TABLE "LearningCard" ALTER COLUMN "Lc_ID" SET DEFAULT NEXTVAL('"LearningCard_Lc_ID_seq"'::regclass);


-- 表 LearningCard 的索引 --
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Ac_ID" ON public."LearningCard" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_Code" ON public."LearningCard" USING btree ("Lc_Code" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_IsUsed" ON public."LearningCard" USING btree ("Lc_IsUsed" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_Pw" ON public."LearningCard" USING btree ("Lc_Pw" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_State" ON public."LearningCard" USING btree ("Lc_State" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lcs_ID" ON public."LearningCard" USING btree ("Lcs_ID" DESC);

-- 创建表 LearningCardSet --
CREATE TABLE IF NOT EXISTS public."LearningCardSet"
(
	"Lcs_ID" integer NOT NULL,
	"Lcs_BuildCount" integer NOT NULL,
	"Lcs_CodeLength" integer NOT NULL,
	"Lcs_Count" integer NOT NULL,
	"Lcs_Coupon" integer NOT NULL,
	"Lcs_CoursesCount" integer NOT NULL,
	"Lcs_CrtTime" timestamp without time zone NOT NULL,
	"Lcs_Intro" character varying(500) COLLATE pg_catalog."default",
	"Lcs_IsEnable" boolean NOT NULL,
	"Lcs_IsFixed" boolean NOT NULL,
	"Lcs_LimitEnd" timestamp without time zone NOT NULL,
	"Lcs_LimitStart" timestamp without time zone NOT NULL,
	"Lcs_MaxCount" integer NOT NULL,
	"Lcs_Price" real NOT NULL,
	"Lcs_PwLength" integer NOT NULL,
	"Lcs_RelatedCourses" text,
	"Lcs_SecretKey" character varying(100) COLLATE pg_catalog."default",
	"Lcs_Span" integer NOT NULL,
	"Lcs_Theme" character varying(500) COLLATE pg_catalog."default",
	"Lcs_Unit" character varying(50) COLLATE pg_catalog."default",
	"Lsc_UsedCount" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_learningcardset PRIMARY KEY ("Lcs_ID")
);
-- 表 LearningCardSet 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LearningCardSet_Lcs_ID_seq";
ALTER SEQUENCE IF EXISTS public."LearningCardSet_Lcs_ID_seq" OWNED BY public."LearningCardSet"."Lcs_ID";
ALTER TABLE "LearningCardSet" ALTER COLUMN "Lcs_ID" SET DEFAULT NEXTVAL('"LearningCardSet_Lcs_ID_seq"'::regclass);



-- 创建表 LimitDomain --
CREATE TABLE IF NOT EXISTS public."LimitDomain"
(
	"LD_ID" integer NOT NULL,
	"LD_Intro" character varying(500) COLLATE pg_catalog."default",
	"LD_IsUse" boolean NOT NULL,
	"LD_Name" character varying(50) COLLATE pg_catalog."default",
	 CONSTRAINT key_limitdomain PRIMARY KEY ("LD_ID")
);
-- 表 LimitDomain 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LimitDomain_LD_ID_seq";
ALTER SEQUENCE IF EXISTS public."LimitDomain_LD_ID_seq" OWNED BY public."LimitDomain"."LD_ID";
ALTER TABLE "LimitDomain" ALTER COLUMN "LD_ID" SET DEFAULT NEXTVAL('"LimitDomain_LD_ID_seq"'::regclass);

INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (1,'邮件服务器2',True,'email');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (2,'',True,'bbs');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (3,'',True,'student');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (4,'',True,'admin');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (5,'',True,'teacher');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (6,'',False,'course');INSERT INTO "LimitDomain"("LD_ID","LD_Intro","LD_IsUse","LD_Name") VALUES (7,'',True,'classone');

-- 创建表 Links --
CREATE TABLE IF NOT EXISTS public."Links"
(
	"Lk_Id" integer NOT NULL,
	"Lk_Email" character varying(255) COLLATE pg_catalog."default",
	"Lk_Explain" character varying(255) COLLATE pg_catalog."default",
	"Lk_IsApply" boolean NOT NULL,
	"Lk_IsShow" boolean NOT NULL,
	"Lk_IsUse" boolean NOT NULL,
	"Lk_IsVerify" boolean NOT NULL,
	"Lk_Logo" character varying(255) COLLATE pg_catalog."default",
	"Lk_LogoSmall" character varying(255) COLLATE pg_catalog."default",
	"Lk_Mobile" character varying(255) COLLATE pg_catalog."default",
	"Lk_Name" character varying(255) COLLATE pg_catalog."default",
	"Lk_QQ" character varying(255) COLLATE pg_catalog."default",
	"Lk_SiteMaster" character varying(255) COLLATE pg_catalog."default",
	"Lk_Tax" integer NOT NULL,
	"Lk_Tootip" character varying(255) COLLATE pg_catalog."default",
	"Lk_Url" character varying(255) COLLATE pg_catalog."default",
	"Ls_Id" integer NOT NULL,
	"Ls_Name" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_links PRIMARY KEY ("Lk_Id")
);
-- 表 Links 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Links_Lk_Id_seq";
ALTER SEQUENCE IF EXISTS public."Links_Lk_Id_seq" OWNED BY public."Links"."Lk_Id";
ALTER TABLE "Links" ALTER COLUMN "Lk_Id" SET DEFAULT NEXTVAL('"Links_Lk_Id_seq"'::regclass);

INSERT INTO "Links"("Lk_Id","Lk_Email","Lk_Explain","Lk_IsApply","Lk_IsShow","Lk_IsUse","Lk_IsVerify","Lk_Logo","Lk_LogoSmall","Lk_Mobile","Lk_Name","Lk_QQ","Lk_SiteMaster","Lk_Tax","Lk_Tootip","Lk_Url","Ls_Id","Ls_Name","Org_ID","Org_Name") VALUES (114,'','郑州双庆数字科技有限公司于2016年，公司专注于企业数字化和智能化建设，一直以来，我们推崇以"一体化、多元化、数字化、智能化"的理念来指导大中型企业进行信息化升级和改造。公司拥有一批经验丰富的专家，自成立以来参与过国内多家国企和上市企业的信息化系统建设。',False,True,True,True,'','','13673362803','双庆ERP（企业资源计划）','','李经理',2,'郑州双庆数字科技有限公司','',22,'友商推荐',4,'郑州微厦计算机科技有限公司');INSERT INTO "Links"("Lk_Id","Lk_Email","Lk_Explain","Lk_IsApply","Lk_IsShow","Lk_IsUse","Lk_IsVerify","Lk_Logo","Lk_LogoSmall","Lk_Mobile","Lk_Name","Lk_QQ","Lk_SiteMaster","Lk_Tax","Lk_Tootip","Lk_Url","Ls_Id","Ls_Name","Org_ID","Org_Name") VALUES (115,'','满足中小型医院信息化需求，成本低、功能强，性能稳定、操作方便，已经成功应用多家医院，安全运行十几年。',False,True,True,True,'','','13526666703','博尔卓医院信息管理系统','10839036','于经理',1,'郑州博尔卓信息科技有限公司','http://www.boerzhuo.com',22,'友商推荐',4,'郑州微厦计算机科技有限公司');
-- 表 Links 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Links_aaaaaLinks_PK" ON public."Links" USING btree ("Lk_Id");

-- 创建表 LinksSort --
CREATE TABLE IF NOT EXISTS public."LinksSort"
(
	"Ls_Id" integer NOT NULL,
	"Ls_IsImg" boolean NOT NULL,
	"Ls_IsShow" boolean NOT NULL,
	"Ls_IsText" boolean NOT NULL,
	"Ls_IsUse" boolean NOT NULL,
	"Ls_Logo" character varying(255) COLLATE pg_catalog."default",
	"Ls_Name" character varying(255) COLLATE pg_catalog."default",
	"Ls_PatId" integer NOT NULL,
	"Ls_Tax" integer NOT NULL,
	"Ls_Tootip" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_linkssort PRIMARY KEY ("Ls_Id")
);
-- 表 LinksSort 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LinksSort_Ls_Id_seq";
ALTER SEQUENCE IF EXISTS public."LinksSort_Ls_Id_seq" OWNED BY public."LinksSort"."Ls_Id";
ALTER TABLE "LinksSort" ALTER COLUMN "Ls_Id" SET DEFAULT NEXTVAL('"LinksSort_Ls_Id_seq"'::regclass);

INSERT INTO "LinksSort"("Ls_Id","Ls_IsImg","Ls_IsShow","Ls_IsText","Ls_IsUse","Ls_Logo","Ls_Name","Ls_PatId","Ls_Tax","Ls_Tootip","Org_ID","Org_Name") VALUES (22,False,True,True,True,'','友商推荐',0,1,'朋友开发的软件产品',4,'郑州微厦计算机科技有限公司');
-- 表 LinksSort 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "LinksSort_aaaaaLinksSort_PK" ON public."LinksSort" USING btree ("Ls_Id");

-- 创建表 LogForStudentExercise --
CREATE TABLE IF NOT EXISTS public."LogForStudentExercise"
(
	"Lse_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Lse_Answer" integer NOT NULL,
	"Lse_Browser" character varying(255) COLLATE pg_catalog."default",
	"Lse_Correct" integer NOT NULL,
	"Lse_CrtTime" timestamp without time zone NOT NULL,
	"Lse_GeogData" text,
	"Lse_IP" character varying(50) COLLATE pg_catalog."default",
	"Lse_JsonData" text,
	"Lse_LastTime" timestamp without time zone NOT NULL,
	"Lse_OS" character varying(255) COLLATE pg_catalog."default",
	"Lse_Platform" character varying(255) COLLATE pg_catalog."default",
	"Lse_Rate" numeric(18,12) NOT NULL,
	"Lse_Sum" integer NOT NULL,
	"Lse_Wrong" integer NOT NULL,
	"Ol_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_logforstudentexercise PRIMARY KEY ("Lse_ID")
);
-- 表 LogForStudentExercise 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LogForStudentExercise_Lse_ID_seq";
ALTER SEQUENCE IF EXISTS public."LogForStudentExercise_Lse_ID_seq" OWNED BY public."LogForStudentExercise"."Lse_ID";
ALTER TABLE "LogForStudentExercise" ALTER COLUMN "Lse_ID" SET DEFAULT NEXTVAL('"LogForStudentExercise_Lse_ID_seq"'::regclass);


-- 表 LogForStudentExercise 的索引 --
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Ac_ID" ON public."LogForStudentExercise" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Cou_ID" ON public."LogForStudentExercise" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Lse_LastTime" ON public."LogForStudentExercise" USING btree ("Lse_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Ol_ID" ON public."LogForStudentExercise" USING btree ("Ol_ID" DESC);

-- 创建表 LogForStudentOnline --
CREATE TABLE IF NOT EXISTS public."LogForStudentOnline"
(
	"Lso_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Lso_Address" character varying(255) COLLATE pg_catalog."default",
	"Lso_BrowseTime" integer NOT NULL,
	"Lso_Browser" character varying(255) COLLATE pg_catalog."default",
	"Lso_City" character varying(255) COLLATE pg_catalog."default",
	"Lso_Code" integer NOT NULL,
	"Lso_CrtTime" timestamp without time zone NOT NULL,
	"Lso_District" character varying(255) COLLATE pg_catalog."default",
	"Lso_GeogType" integer NOT NULL,
	"Lso_IP" character varying(50) COLLATE pg_catalog."default",
	"Lso_Info" character varying(255) COLLATE pg_catalog."default",
	"Lso_LastTime" timestamp without time zone NOT NULL,
	"Lso_Latitude" numeric(20,15) NOT NULL,
	"Lso_LoginDate" timestamp without time zone NOT NULL,
	"Lso_LoginTime" timestamp without time zone NOT NULL,
	"Lso_LogoutTime" timestamp without time zone NOT NULL,
	"Lso_Longitude" numeric(20,15) NOT NULL,
	"Lso_OS" character varying(255) COLLATE pg_catalog."default",
	"Lso_OnlineTime" integer NOT NULL,
	"Lso_Platform" character varying(255) COLLATE pg_catalog."default",
	"Lso_Province" character varying(255) COLLATE pg_catalog."default",
	"Lso_Source" character varying(255) COLLATE pg_catalog."default",
	"Lso_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_logforstudentonline PRIMARY KEY ("Lso_ID")
);
-- 表 LogForStudentOnline 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LogForStudentOnline_Lso_ID_seq";
ALTER SEQUENCE IF EXISTS public."LogForStudentOnline_Lso_ID_seq" OWNED BY public."LogForStudentOnline"."Lso_ID";
ALTER TABLE "LogForStudentOnline" ALTER COLUMN "Lso_ID" SET DEFAULT NEXTVAL('"LogForStudentOnline_Lso_ID_seq"'::regclass);

INSERT INTO "LogForStudentOnline"("Lso_ID","Ac_AccName","Ac_ID","Ac_Name","Lso_Address","Lso_BrowseTime","Lso_Browser","Lso_City","Lso_Code","Lso_CrtTime","Lso_District","Lso_GeogType","Lso_IP","Lso_Info","Lso_LastTime","Lso_Latitude","Lso_LoginDate","Lso_LoginTime","Lso_LogoutTime","Lso_Longitude","Lso_OS","Lso_OnlineTime","Lso_Platform","Lso_Province","Lso_Source","Lso_UID","Org_ID") VALUES (949,'tester',2,'韩梅梅','',0,'Chrome 94.0','',0,'2024-01-22 17:58:18','',0,'::1','账号密码登录','2024-01-22 17:58:18',0.000000000000000,'2024-01-22 00:00:00','2024-01-22 17:58:18','2024-01-22 17:59:18',0.000000000000000,'Windows 10',1,'PC','','电脑网页','e7d5ac9764e621c908e99265d2ae19df',4);
-- 表 LogForStudentOnline 的索引 --
CREATE INDEX IF NOT EXISTS "LogForStudentOnline_IX_Ac_ID" ON public."LogForStudentOnline" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentOnline_IX_CrtTime" ON public."LogForStudentOnline" USING btree ("Lso_CrtTime" DESC);

-- 创建表 LogForStudentQuestions --
CREATE TABLE IF NOT EXISTS public."LogForStudentQuestions"
(
	"Lsq_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Lsq_CrtTime" timestamp without time zone NOT NULL,
	"Lsq_Index" integer NOT NULL,
	"Lsq_LastTime" timestamp without time zone NOT NULL,
	"Ol_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	"Qus_ID" bigint NOT NULL,
	 CONSTRAINT key_logforstudentquestions PRIMARY KEY ("Lsq_ID")
);
-- 表 LogForStudentQuestions 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LogForStudentQuestions_Lsq_ID_seq";
ALTER SEQUENCE IF EXISTS public."LogForStudentQuestions_Lsq_ID_seq" OWNED BY public."LogForStudentQuestions"."Lsq_ID";
ALTER TABLE "LogForStudentQuestions" ALTER COLUMN "Lsq_ID" SET DEFAULT NEXTVAL('"LogForStudentQuestions_Lsq_ID_seq"'::regclass);



-- 创建表 LogForStudentStudy --
CREATE TABLE IF NOT EXISTS public."LogForStudentStudy"
(
	"Lss_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Lss_Browser" character varying(255) COLLATE pg_catalog."default",
	"Lss_Complete" real NOT NULL,
	"Lss_CrtTime" timestamp without time zone NOT NULL,
	"Lss_Details" text,
	"Lss_Duration" integer NOT NULL,
	"Lss_GeogData" text,
	"Lss_IP" character varying(50) COLLATE pg_catalog."default",
	"Lss_LastTime" timestamp without time zone NOT NULL,
	"Lss_OS" character varying(255) COLLATE pg_catalog."default",
	"Lss_Platform" character varying(255) COLLATE pg_catalog."default",
	"Lss_PlayTime" integer NOT NULL,
	"Lss_StudyTime" integer NOT NULL,
	"Lss_UID" character varying(255) COLLATE pg_catalog."default",
	"Ol_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_logforstudentstudy PRIMARY KEY ("Lss_ID")
);
-- 表 LogForStudentStudy 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."LogForStudentStudy_Lss_ID_seq";
ALTER SEQUENCE IF EXISTS public."LogForStudentStudy_Lss_ID_seq" OWNED BY public."LogForStudentStudy"."Lss_ID";
ALTER TABLE "LogForStudentStudy" ALTER COLUMN "Lss_ID" SET DEFAULT NEXTVAL('"LogForStudentStudy_Lss_ID_seq"'::regclass);


-- 表 LogForStudentStudy 的索引 --
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Ac_Ol_Cou" ON public."LogForStudentStudy" USING btree ("Ac_ID", "Ol_ID", "Cou_ID");
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Cou_ID" ON public."LogForStudentStudy" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Lss_LastTime" ON public."LogForStudentStudy" USING btree ("Lss_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Ol_ID" ON public."LogForStudentStudy" USING btree ("Ol_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Org_ID" ON public."LogForStudentStudy" USING btree ("Org_ID" DESC);

-- 创建表 Logs --
CREATE TABLE IF NOT EXISTS public."Logs"
(
	"Log_Id" integer NOT NULL,
	"Acc_Id" integer NOT NULL,
	"Acc_Name" character varying(50) COLLATE pg_catalog."default",
	"Log_Browser" character varying(50) COLLATE pg_catalog."default",
	"Log_FileName" character varying(255) COLLATE pg_catalog."default",
	"Log_IP" character varying(50) COLLATE pg_catalog."default",
	"Log_MenuId" integer NOT NULL,
	"Log_MenuName" character varying(50) COLLATE pg_catalog."default",
	"Log_OS" character varying(50) COLLATE pg_catalog."default",
	"Log_Time" timestamp without time zone,
	"Log_Type" character varying(50) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_logs PRIMARY KEY ("Log_Id")
);
-- 表 Logs 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Logs_Log_Id_seq";
ALTER SEQUENCE IF EXISTS public."Logs_Log_Id_seq" OWNED BY public."Logs"."Log_Id";
ALTER TABLE "Logs" ALTER COLUMN "Log_Id" SET DEFAULT NEXTVAL('"Logs_Log_Id_seq"'::regclass);


-- 表 Logs 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Logs_aaaaaLogs_PK" ON public."Logs" USING btree ("Log_Id");

-- 创建表 ManageMenu --
CREATE TABLE IF NOT EXISTS public."ManageMenu"
(
	"MM_Id" integer NOT NULL,
	"MM_AbbrName" character varying(255) COLLATE pg_catalog."default",
	"MM_Color" character varying(50) COLLATE pg_catalog."default",
	"MM_Complete" integer NOT NULL,
	"MM_Font" character varying(50) COLLATE pg_catalog."default",
	"MM_Func" character varying(50) COLLATE pg_catalog."default",
	"MM_Help" character varying(1000) COLLATE pg_catalog."default",
	"MM_IcoCode" character varying(50) COLLATE pg_catalog."default",
	"MM_IcoColor" character varying(100) COLLATE pg_catalog."default",
	"MM_IcoSize" integer NOT NULL,
	"MM_IcoX" integer NOT NULL,
	"MM_IcoY" integer NOT NULL,
	"MM_Intro" character varying(255) COLLATE pg_catalog."default",
	"MM_IsBold" boolean NOT NULL,
	"MM_IsChilds" boolean NOT NULL,
	"MM_IsFixed" boolean NOT NULL,
	"MM_IsItalic" boolean NOT NULL,
	"MM_IsShow" boolean NOT NULL,
	"MM_IsUse" boolean NOT NULL,
	"MM_Link" character varying(255) COLLATE pg_catalog."default",
	"MM_Marker" character varying(255) COLLATE pg_catalog."default",
	"MM_Name" character varying(100) NOT NULL COLLATE pg_catalog."default",
	"MM_PatId" character varying(255) COLLATE pg_catalog."default",
	"MM_Root" integer NOT NULL,
	"MM_Tax" integer NOT NULL,
	"MM_Type" character varying(50) COLLATE pg_catalog."default",
	"MM_UID" character varying(255) COLLATE pg_catalog."default",
	"MM_WinHeight" integer NOT NULL,
	"MM_WinID" character varying(255) COLLATE pg_catalog."default",
	"MM_WinMax" boolean NOT NULL,
	"MM_WinMin" boolean NOT NULL,
	"MM_WinMove" boolean NOT NULL,
	"MM_WinResize" boolean NOT NULL,
	"MM_WinWidth" integer NOT NULL,
	 CONSTRAINT key_managemenu PRIMARY KEY ("MM_Id")
);
-- 表 ManageMenu 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ManageMenu_MM_Id_seq";
ALTER SEQUENCE IF EXISTS public."ManageMenu_MM_Id_seq" OWNED BY public."ManageMenu"."MM_Id";
ALTER TABLE "ManageMenu" ALTER COLUMN "MM_Id" SET DEFAULT NEXTVAL('"ManageMenu_MM_Id_seq"'::regclass);

INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (6881,'','',100,'','func','','a030','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/Platinfo','','常规','651',88,0,'item','6362b3b6a38c5c7b976fa64e40219e46',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (88,'','',0,'','func','','a038','',0,0,0,'请不要轻易改动！',False,False,True,False,True,True,'','system','系统设置','0',88,3,'item','88',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22731,'','',100,'','func','','a022','',0,0,0,'',False,True,False,False,True,True,'/manage/Platform/Agreement','','注册协议','88',651,0,'item','69475b3d3dfff2e4470402686bbe9393',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22732,'','',100,'','func','','e7d4','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/limitdomain','','保留域名','88',88,1,'item','70c20a152062c09c598384f35d9cde36',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22733,'','',100,'','func','','a030','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/SystemPara','','系统参数','88',88,2,'item','b918a2008b0d5adb9e852b6bb113cf9c',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22734,'','',100,'','func','','a01c','',0,0,0,'',False,True,False,False,True,True,'','','接口管理','88',88,3,'item','742f03375a49149ef533668189ec0777',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22735,'','',100,'','func','','e824','',0,0,0,'',False,False,False,False,True,True,'/manage/pay/list','','支付接口','742f03375a49149ef533668189ec0777',88,0,'item','a19b1b08a06bac9d0adc044e0055a53b',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22736,'','',100,'','func','','e645','',0,0,0,'',False,False,False,False,True,True,'/manage/OtherLogin/setup','','第三方登录','742f03375a49149ef533668189ec0777',88,1,'item','6a47cb4dcaff0fe97b45c0347139f438',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22737,'','',100,'','func','','e76e','',0,0,0,'',False,False,False,False,True,True,'/manage/SMS/Setup','','短信接口','742f03375a49149ef533668189ec0777',88,2,'item','f4c77daf1be33a237f6e5ee64da40c63',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22738,'','',100,'','func','','e639','',0,0,0,'',False,False,False,False,True,True,'/manage/Sso/Setup','','单点登录','742f03375a49149ef533668189ec0777',88,3,'item','8b60275871e44251d0712030ec9a44e7',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22739,'','',100,'','func','','e661','',0,0,0,'',False,False,False,False,True,True,'/manage/live/qiniuyun','','七牛云直播','742f03375a49149ef533668189ec0777',88,4,'item','8d34c9389a5dd4ec6ab6c3bf50060b10',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (2318,'分校','',0,'','func','','a003','',0,0,0,'',False,False,True,False,True,True,'','organ','分支机构','0',2318,2,'','99171ec989452adea2c9d8d8b4a2c3a4',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22740,'','',100,'','func','','e64c','',0,0,0,'',False,False,False,False,True,True,'/manage/setup/BaiduLBS','','百度地图','742f03375a49149ef533668189ec0777',88,5,'item','1701333322013',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22741,'','',100,'','func','','a00c','',0,0,0,'',False,True,False,False,True,True,'','','菜单管理','88',88,4,'item','9e4d9f97f71fc3a076e9893449bea4be',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22742,'','',100,'','func','','a024','',0,0,0,'管理界面左侧的菜单',False,False,False,False,True,True,'/manage/Platform/menuroot','MenuTree','功能菜单','9e4d9f97f71fc3a076e9893449bea4be',88,0,'item','33f1cd7ad494a32c5a061babf4d62599',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22743,'','',100,'','func','','a005','',0,0,0,'位于管理界面左上方的下拉菜单',False,False,False,False,True,True,'/manage/Platform/sysmenu','','系统菜单','9e4d9f97f71fc3a076e9893449bea4be',88,1,'item','26c139b82c6c9455864a56a4c8ba0f6f',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22744,'','',100,'','func','','e88a','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/PointSetup','','积分设置','88',651,5,'item','2ad779650365a880c345016b15ed9401',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22745,'','',0,'','func','','a025','',0,0,0,'',False,True,False,False,True,False,'','','系统日志','88',88,6,'item','485d923c64b7a076861d2189d31bc52b',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22746,'','',0,'','func','','','',0,0,0,'',False,False,False,False,True,True,'','','数据清理','485d923c64b7a076861d2189d31bc52b',88,0,'item','ef5ed2135cec3915a022d5f8d6d91b45',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (522,'','',0,'','func','','a003','',0,0,0,'用于机构管理员操作',False,False,True,False,False,True,'','organAdmin','机构管理','0',522,4,'item','522',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (547,'','',0,'','func','','e804','',0,0,0,'学员登录后看到的菜单项',False,False,True,False,False,True,'','student','学员的管理','0',547,5,'item','547',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (569,'','',0,'','func','','e647','',0,0,0,'教师进入教学管理时看到的菜单项',False,False,True,False,False,True,'','teacher','教师的管理','0',569,6,'item','569',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22747,'','',0,'','func','','','',0,0,0,'',False,False,False,False,True,True,'','','登录日志','485d923c64b7a076861d2189d31bc52b',88,1,'item','da544360c2d541f501758b826d1ca510',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9247,'','',100,'','func','','e67d','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/Accounts','','账户','651',651,0,'item','552b799487f974e4e2705254180ab16e',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9248,'','',100,'','func','','e746','',0,0,0,'',False,False,False,False,True,True,'/manage/Capital/Records','','资金流水','651',651,1,'item','de1cfb2ccec3e0befab63d0957227213',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9249,'','',100,'','func','','e60f','',0,0,0,'',False,False,False,False,True,True,'/manage/Learningcard/cardset','','学习卡','651',0,2,'item','eab253f97e1b78feab00fc256047acd5',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9250,'','',100,'','func','','e62f','',0,0,0,'',False,True,False,False,True,True,'/manage/Rechargecode/Codeset','','充值码','651',651,3,'item','9eb880dcf1d228e4e75f07429e0fe17a',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9251,'','',100,'','func','','f008f','',0,0,0,'',False,True,False,False,True,False,'','','考试管理','651',0,4,'item','71fd444a28f50e71f664761603daf63e',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9252,'','',100,'','func','','e82e','',0,0,0,'',False,False,False,False,True,True,'/teacher/exam/results','','考试成绩','71fd444a28f50e71f664761603daf63e',0,0,'item','9773958521dd3c6f46bbc424d26b9770',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (651,'','',0,'','func','','e72f','',0,0,0,'',False,False,True,False,True,True,'','base','基础设置','0',651,0,'item','651',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10193,'','',100,'','func','','e770','',0,0,0,'',False,True,False,False,True,True,'','','分机构管理','99171ec989452adea2c9d8d8b4a2c3a4',651,0,'item','dbda24c276292d3229c7df766a890ca1',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10194,'','',100,'','func','','a02a','',0,0,0,'',False,False,False,False,True,True,'/manage/Organs/list','','机构列表','dbda24c276292d3229c7df766a890ca1',651,0,'item','a93acf4d1572ef78fa2d748a69b58738',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10195,'','',0,'','func','','a042','',0,0,0,'',False,False,False,False,True,False,'/manage/Organs/Verify','','机构审核','dbda24c276292d3229c7df766a890ca1',651,1,'item','ee61424a18a1b5544124b197dad6142e',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10196,'','',100,'','func','','e81b','',0,0,0,'',False,False,False,False,True,True,'/manage/Organs/Level','','机构等级','dbda24c276292d3229c7df766a890ca1',651,2,'item','3f9df262b89e46fa930b4369a5e239bd',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10197,'','',100,'','func','','e699','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/ProfitSharing','','分润设置','dbda24c276292d3229c7df766a890ca1',651,3,'item','9fba648f72cf773e0a560c421aae8420',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10198,'','',100,'','func','','a003','',0,0,0,'',False,True,False,False,True,True,'','','当前机构','99171ec989452adea2c9d8d8b4a2c3a4',88,1,'item','c251c17f570f77bf00728d8d8dd0ae9f',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10199,'','',100,'','func','','e6a2','',0,0,0,'',False,False,False,False,True,True,'/manage/Organs/organinfo','','机构信息','c251c17f570f77bf00728d8d8dd0ae9f',88,0,'item','7383d2425a17f4ce59cb13094c96118a',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10200,'','',100,'','func','','e667','',0,0,0,'',False,False,False,False,True,True,'/manage/Organs/about','','单位介绍','c251c17f570f77bf00728d8d8dd0ae9f',88,1,'item','151c0f86093805fe0fcdda6be39bb007',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10201,'','',100,'','func','','e645','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/Employee','','管理员','c251c17f570f77bf00728d8d8dd0ae9f',88,2,'item','ff70834b1af658beaa1ba86dd2514880',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10202,'','',0,'','func','','e68b','',0,0,0,'',False,False,False,False,True,False,'sys/depart.aspx','purview','部门信息','c251c17f570f77bf00728d8d8dd0ae9f',88,3,'item','887e1c3444208f98b12445e7a62e1c2f',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10203,'','',100,'','func','','e655','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/Position','purview','岗位设置','c251c17f570f77bf00728d8d8dd0ae9f',88,4,'item','871a922530ef97717cbd6f5f61d5cafc',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10204,'','',100,'','func','','e804','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/EmpTitle','','职务头衔','c251c17f570f77bf00728d8d8dd0ae9f',88,5,'item','baa31535166ec9736c4ccf03a44a5dd9',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10205,'','',100,'','func','','e67d','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/EmpGroup','purview','工作组','c251c17f570f77bf00728d8d8dd0ae9f',88,6,'item','82757f86610edd41612051d8484dc6e9',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23680,'','',100,'','func','','e813','',0,0,3,'',False,True,False,False,False,True,'/teacher/course/list','','课程管理','569',569,0,'item','6b83ad54dc5319393f4eaf23b6ae14c8',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (9974,'','',100,'','func','','e755','',0,0,0,'',False,False,False,False,True,True,'/manage/ques/itemrepeat','','试题修复','651',651,6,'item','1669172453942',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23681,'','',100,'','func','','e817','',0,0,0,'',False,False,False,False,False,True,'/teacher/course/Messages','','课程交流','569',569,1,'item','cc6e884e86541560bddc33e539cfdbc7',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23682,'','',0,'','func','','e67d','',0,0,0,'',False,False,False,False,False,False,'/teacher/student/list','','我的学员','569',569,2,'item','a4c7e947718555647d5c033409812101',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23683,'','',100,'','func','','e816','',0,0,0,'',False,True,False,False,False,True,'','','测试/考试','569',569,3,'item','ceef164ede5ac404041250eb01c46be3',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23684,'','',100,'','func','','e6b0','',0,0,0,'',False,False,False,False,False,True,'/teacher/testpaper/list','','试卷管理','ceef164ede5ac404041250eb01c46be3',569,0,'item','e32ff65ff0db40c9d569a80d95550c25',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23685,'','',100,'','func','','e810','',0,0,0,'',False,False,False,False,False,True,'/teacher/exam/results','','专项考试成绩','ceef164ede5ac404041250eb01c46be3',569,1,'item','1673081413592',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (23686,'','',100,'','func','','e669','',0,0,0,'',False,True,False,False,False,True,'/teacher/self/index','','个人信息','569',569,4,'item','b486c9a4eca4cc594585bd6639e281fe',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10136,'','',0,'','sys','','a020','',0,0,0,'系统首页',False,False,True,False,True,True,'/','','首页','0',103,0,'link','d9ffe1150602ac1fd987007396520189',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10137,'','',0,'','sys','','a034','',0,0,0,'',False,True,True,False,True,True,'#','','源代码','0',103,1,'item','5f241cb3015f4e6ba505ae9243feef99',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10138,'','',0,'','sys','','e691','',0,0,0,'支持二次开发',False,False,False,False,True,True,'https://github.com/weishakeji/LearningSystem','','产品源码','5f241cb3015f4e6ba505ae9243feef99',103,0,'link','b6570c588539269c6de5ac06b7438e70',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10139,'','',0,'','sys','','a010','',0,0,0,'',False,False,False,False,True,True,'https://github.com/weishakeji/WebdeskUI','','WebUI源码','5f241cb3015f4e6ba505ae9243feef99',103,1,'link','e680f05fce7d34c783286d98e2beeb97',450,'',False,False,False,False,640);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10140,'','',0,'','sys','','e686','',0,0,0,'国内Gitee开源镜像',False,False,False,False,True,True,'https://gitee.com/weishakeji','','国内镜像','5f241cb3015f4e6ba505ae9243feef99',103,2,'link','c834f537e216e4b529ef7d4449593524',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10141,'','',0,'','sys','','','',0,0,0,'',False,False,False,False,True,True,'','','null','5f241cb3015f4e6ba505ae9243feef99',103,3,'hr','b6936694735702ccacb76f692444f547',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10142,'','',0,'','sys','','a022','',0,0,0,'',False,True,False,False,True,True,'','','开发文档','5f241cb3015f4e6ba505ae9243feef99',103,4,'item','36851ec244b58c6e893a788c049298e5',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10143,'','',0,'','sys','','a01c','',0,0,0,'',False,False,False,False,True,True,'/help/api/','','ViewData API 说明','36851ec244b58c6e893a788c049298e5',103,0,'link','7bfaf2ad62853302824b625c926a5a2f',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10144,'','',0,'','sys','','e6a4','',0,0,0,'',False,False,False,False,True,True,'/help/datas/','','数据字典','36851ec244b58c6e893a788c049298e5',103,1,'link','d14c2127edbccdaf73490709d0cf78c2',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10145,'','',0,'','sys','','a010','',0,0,0,'',False,False,False,False,True,True,'http://webdesk.weisha100.cn/','','WebUI 开发文档','36851ec244b58c6e893a788c049298e5',103,2,'link','5e4e303706b7d0583453a3ada7f267bf',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10146,'','',0,'','sys','','e600','',0,0,0,'',False,False,False,False,True,True,'','','newnode-7879','36851ec244b58c6e893a788c049298e5',103,3,'hr','0d73063295412838f39d31e015036d40',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10147,'','',0,'','sys','','e610','',0,0,0,'',False,False,False,False,True,True,'/Utilities/Fonts/index.html','','图标库','36851ec244b58c6e893a788c049298e5',103,4,'link','23f7e9cf86ce216020ab6251693d49b1',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10148,'','',0,'','sys','','e836','',0,0,0,'',False,False,False,False,True,True,'http://www.weishakeji.net/download.html','','升级日志','5f241cb3015f4e6ba505ae9243feef99',103,5,'link','f268a63814cc7500123950307b3faa4f',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10149,'','',0,'','sys','','a026','',0,0,0,'',False,True,True,False,True,True,'','','帮助','0',103,2,'item','83c860c6e6d9efc97c1d643a69562a0a',200,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10150,'','',0,'','sys','','a022','',0,0,0,'',False,False,False,False,True,True,'/readme.html','','自述文件','83c860c6e6d9efc97c1d643a69562a0a',103,0,'link','4a0ba13c193afd931459fe4422fe2284',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10151,'','',0,'','sys','','e7d4','',0,0,0,'',False,False,False,False,True,True,'http://www.weisha100.net/','','在线教程','83c860c6e6d9efc97c1d643a69562a0a',103,1,'link','f5383ee2e50f234d322ec3e6c6ab22ec',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10152,'','',0,'','sys','','a026','',0,0,0,'在线学习教程',False,False,False,False,True,True,'/help','','帮助','83c860c6e6d9efc97c1d643a69562a0a',103,2,'link','8eb4784b53887148a45bc80fc16a19c1',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10153,'','',0,'','sys','','','',0,0,0,'',False,False,False,False,True,True,'','','newnode-5338','83c860c6e6d9efc97c1d643a69562a0a',103,3,'hr','26de70c3a084bf45bba3d1108abf2144',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10154,'','',0,'','sys','','a034','',0,0,0,'',False,False,False,False,True,True,'/help/License.html','','开源协议','83c860c6e6d9efc97c1d643a69562a0a',103,4,'open','a2980cfee1ad6268ab5a3af56b8b3b64',400,'PublicLicense',False,False,True,False,600);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10155,'','',0,'','sys','','a027','',0,0,0,'',False,False,False,False,True,True,'/manage/Platform/copyright','','版权信息','83c860c6e6d9efc97c1d643a69562a0a',103,5,'open','60a81a51602ea583adc3d086d10a5424',600,'',True,True,True,True,800);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10156,'','',0,'','sys','','','',0,0,0,'',False,False,False,False,True,True,'','','newnode-7595','83c860c6e6d9efc97c1d643a69562a0a',103,6,'hr','b39062b640c982317666591ac7481a2d',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10157,'','',0,'','sys','','a031','',0,0,0,'',False,True,False,False,True,True,'','','关于','83c860c6e6d9efc97c1d643a69562a0a',103,7,'open','8b6bd638621686c382eb11d1d6759ad7',400,'about',False,False,True,True,600);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (10158,'','',0,'','sys','','a001','',0,0,0,'微厦科技官网',False,False,False,False,True,False,'http://www.weishakeji.net','','微厦科技','83c860c6e6d9efc97c1d643a69562a0a',103,8,'link','8fcb85b04376833dc6fb8a2fda5abab8',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24983,'','',0,'','func','','e6b0','rgb(75, 2, 197)',-6,0,0,'',False,False,False,False,False,False,'/orgadmin/data/Exercise','','试题练习','1697789368523',522,2,'item','1697789631914',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24984,'','',0,'','func','','','',0,0,0,'',False,False,False,False,False,False,'','','考试加分','1697789368523',522,3,'item','1697789647933',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24985,'数据','',100,'','func','','e6ef','',0,0,2,'',False,True,False,False,False,True,'','','数据分析','522',522,3,'item','2b858af35952ad4106e1f97b10b20ff9',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24986,'','',100,'','func','','e81c','rgb(213, 25, 4)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/Capital/Summary','','收入汇总','2b858af35952ad4106e1f97b10b20ff9',522,0,'item','1697791007226',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24987,'','',100,'','func','','e856','rgb(255, 66, 18)',0,0,0,'选课最多的专业',False,False,False,False,False,True,'/orgadmin/Statis/CourseHot','','热门课程','2b858af35952ad4106e1f97b10b20ff9',522,1,'item','5f81b3a13ce40cdc0525d3346bcdc682',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24988,'','',100,'','func','','e746','rgb(230, 145, 10)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/Statis/CourseAmount','','课程收入','2b858af35952ad4106e1f97b10b20ff9',522,2,'item','1697790883366',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24989,'','',100,'','func','','e6a4','rgb(22, 127, 183)',-6,0,0,'',False,False,False,False,False,True,'/orgadmin/Statis/Storage','','资源存储','2b858af35952ad4106e1f97b10b20ff9',522,3,'item','1697791356432',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24990,'','',0,'','func','','e650','',0,0,0,'',False,False,False,False,False,False,'','','热门教师','2b858af35952ad4106e1f97b10b20ff9',522,4,'item','ee0d5748fa5855398e9d2a0ba5ac5651',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24991,'','',0,'','func','','e8c9','',0,0,0,'',False,False,False,False,False,False,'/manage/teacher/order.aspx','','优秀教师','2b858af35952ad4106e1f97b10b20ff9',522,5,'item','53308789bf582bb1e60583d0df808580',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24992,'','',0,'','func','','e67d','',0,0,0,'',False,False,False,False,False,False,'/manage/orgadmin/stonline.aspx','','学员在线','2b858af35952ad4106e1f97b10b20ff9',522,6,'item','a24cbf78d35248503adfaa03ff60ca11',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24993,'','',0,'','func','','e81b','',0,0,0,'',False,False,False,False,False,False,'/manage/teacher/Comments.aspx','','教师评价','2b858af35952ad4106e1f97b10b20ff9',522,7,'item','843c0827d3d2810782adda9aaa6f409a',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24994,'','',0,'','func','','e6f1','',0,0,0,'',False,False,False,False,False,False,'/web/viewport/Index','','数据大屏','2b858af35952ad4106e1f97b10b20ff9',522,8,'item','1701256457042',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24995,'','',100,'','func','','a01d','',15,5,0,'',False,True,False,False,False,True,'','','学员登录','2b858af35952ad4106e1f97b10b20ff9',522,9,'item','1705109474971',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24996,'','',100,'','func','','e61d','',0,0,0,'',False,False,False,False,False,True,'/orgadmin/log/loginlog','','登录日志','1705109474971',522,0,'item','1705402993842',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24997,'','',100,'','func','','a04a','',0,0,0,'',False,False,False,False,False,True,'/web/viewport/login','','地域分布','1705109474971',522,1,'item','1705403019261',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (22748,'','',0,'','func','','','',0,0,0,'',False,False,False,False,True,True,'','','操作记录','485d923c64b7a076861d2189d31bc52b',88,2,'item','4635221fa43b3a4f0a55e39cc0d5772c',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24998,'管理','',100,'','func','','a038','',3,0,2,'',False,True,False,False,False,True,'','','平台管理','522',522,4,'item','d42f434639edecdbd7f7f5919e6d086a',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24999,'','',100,'','func','','e732','rgb(237, 104, 9)',-5,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/General','','基础信息','d42f434639edecdbd7f7f5919e6d086a',522,0,'item','fc60823be11ec1b67cbc8865085928ca',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25000,'','',100,'','func','','e67d','rgb(175, 3, 196)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/about','','关于我们','d42f434639edecdbd7f7f5919e6d086a',522,1,'item','1697103541077',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25001,'','',100,'','func','','e6ed','rgb(4, 109, 173)',6,0,-2,'当前机构的管理人员',False,True,False,False,False,True,'','','管理员','d42f434639edecdbd7f7f5919e6d086a',522,2,'item','76d02763628ceb20715ff8700f93d711',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25002,'','',100,'','func','','e812','rgb(2, 120, 71)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/admin/Employee','','管理人员','76d02763628ceb20715ff8700f93d711',522,0,'item','5f8650559e67d7aee0865ab46abc57e5',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25003,'','',89,'','func','','e635','rgb(168, 2, 177)',5,0,-2,'',False,False,False,False,False,True,'/orgadmin/admin/Position','purview','角色/岗位','76d02763628ceb20715ff8700f93d711',522,1,'item','5f3a00f6661e44c530939cb7ad74845f',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25004,'','',100,'','func','','e645','rgb(2, 119, 177)',-1,0,0,'',False,False,False,False,False,True,'/orgadmin/admin/EmpTitle','','职务','76d02763628ceb20715ff8700f93d711',522,2,'item','1639834362028',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25005,'','',0,'','func','','e711','rgb(9, 126, 184)',0,0,0,'',False,True,False,False,False,False,'','','组织机构','d42f434639edecdbd7f7f5919e6d086a',522,3,'item','85c3ddb99d58e33f9e3637cfb6c2b298',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25006,'','',0,'','func','','e687','rgb(24, 85, 2)',0,0,0,'',False,False,False,False,False,False,'/manage/sys/depart.aspx','purview','部门信息','85c3ddb99d58e33f9e3637cfb6c2b298',522,0,'item','482695a528fb8f892b6dceb259bd40b3',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25007,'','',0,'','func','','e67d','rgb(2, 57, 85)',0,0,0,'',False,False,False,False,False,False,'/manage/sys/empgroup.aspx','purview','工作组','85c3ddb99d58e33f9e3637cfb6c2b298',522,1,'item','8238df6e47c88762ba42a7995e547f3e',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25008,'','',0,'','func','','e639','rgb(83, 5, 188)',0,0,-2,'',False,False,False,False,False,False,'/manage/sys/title.aspx','','职务头衔','85c3ddb99d58e33f9e3637cfb6c2b298',522,2,'item','0dd1db3b35c9fe75e38edcdf738ebd2c',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24793,'','',100,'','func','','e669','',0,0,0,'',False,False,False,False,False,True,'/student/Self/Intro','','个人介绍','c1d2a8cba0766a36dafa91f43a581f18',547,2,'item','1641735504763',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24794,'','',100,'','func','','e76a','',0,0,0,'已经选学的课程',False,False,False,False,False,True,'/student/Self/safe','','安全管理','c1d2a8cba0766a36dafa91f43a581f18',547,3,'item','501e00e137aebb030d30b8de30edec06',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24795,'','',100,'','func','','a01d','',30,5,2,'',False,False,False,False,False,True,'/student/self/loginlog','','登录日志','c1d2a8cba0766a36dafa91f43a581f18',547,4,'item','1704957895680',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24796,'','',0,'','func','','a039','',0,0,0,'',False,False,False,False,False,False,'/manage/student/online.aspx','','在线时间','c1d2a8cba0766a36dafa91f43a581f18',547,5,'item','4cbafbacb24a146eb1e03cca80d32db6',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24797,'','',100,'','func','','e81c','',0,0,0,'',False,True,False,False,False,True,'','','充值/资金','547',547,5,'node','f4c2e87c58a014d0eaaed7ba1a459314',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24798,'','',100,'','func','','e749','',0,0,0,'',False,False,False,False,False,True,'/Student/Money/recharge','','充值','f4c2e87c58a014d0eaaed7ba1a459314',547,0,'item','ca6c8e9988678ea4bc089a98d64dbe43',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24799,'','',100,'','func','','e824','',0,0,0,'',False,False,False,False,False,True,'/Student/Money/Details','','资金','f4c2e87c58a014d0eaaed7ba1a459314',547,1,'item','02d9f63ce76365a7d986e0b0a0ea70e4',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24800,'','',100,'','func','','e847','',0,0,0,'',False,False,False,False,False,True,'/Student/Coupon/index','','卡券','f4c2e87c58a014d0eaaed7ba1a459314',547,2,'item','b632ee17275095c13cfb8055129c59cd',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24801,'','',100,'','func','','e88a','',0,0,0,'',False,False,False,False,False,True,'/Student/Point/index','','积分','f4c2e87c58a014d0eaaed7ba1a459314',547,3,'item','17ed5191fd4a3b9d3fc366a1cda5b4dc',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24802,'','',100,'','func','','e666','',0,0,0,'',False,False,False,False,False,True,'/Student/Money/Profit','','收益','f4c2e87c58a014d0eaaed7ba1a459314',547,4,'item','3ed08ea8c3a6dbbb0b14535e27357061',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24803,'','',100,'','func','','e685','',0,0,0,'',False,True,False,False,False,True,'','','学习卡','547',547,6,'node','dff9c491a3f3e5493cec04abf7244b69',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24804,'','',100,'','func','','e685','',0,0,0,'',False,False,False,False,False,True,'/student/Learningcard/index','','我的学习卡','dff9c491a3f3e5493cec04abf7244b69',547,0,'item','3a108c8fbb70ddb57532149a214bc427',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24805,'','',100,'','func','','','',0,0,0,'',False,False,False,False,False,True,'','','newnode-6141','547',547,7,'hr','1641640249725',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24806,'','',100,'','func','','e690','',0,0,0,'',False,True,False,False,False,True,'','','分享','547',547,8,'item','2a3233d51cbc815b00bce3e9d7788dbb',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24807,'','',100,'','func','','e73e','',0,0,0,'',False,False,False,False,False,True,'/student/share/link','','分享链接','2a3233d51cbc815b00bce3e9d7788dbb',547,0,'item','82809d1a369ab3c44c330c909a532866',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24808,'','',100,'','func','','e67d','',0,0,0,'',False,True,False,False,False,True,'/student/share/subordinates','','我的朋友','2a3233d51cbc815b00bce3e9d7788dbb',547,1,'item','83f7721be4b7779ce3f097a59b81adb9',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25009,'','',100,'','func','','a038','rgb(208, 107, 0)',0,0,0,'',False,True,False,False,False,True,'/orgadmin/setup/function','','功能设置','d42f434639edecdbd7f7f5919e6d086a',522,4,'item','acc513b05fe589c27716631c54ef30f8',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25010,'','',0,'','func','','a053','',0,0,0,'',False,False,False,False,False,False,'/orgadmin/setup/qrcode','','二维码','acc513b05fe589c27716631c54ef30f8',522,0,'item','95aca19ba38c02a7b9f3d2b7a7e0c7e5',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25011,'','',100,'','func','','a035','rgb(8, 113, 166)',-3,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/Register','','登录注册','acc513b05fe589c27716631c54ef30f8',522,1,'item','1695313545347',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25012,'','',100,'','func','','e813','rgb(3, 137, 62)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/study','','学习相关','acc513b05fe589c27716631c54ef30f8',522,2,'item','1695313546693',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25013,'','',100,'','func','','e79b','rgb(64, 88, 0)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/device','','终端设备','acc513b05fe589c27716631c54ef30f8',522,3,'item','1695313569415',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25014,'界面','',100,'','func','','a010','',0,0,3,'',False,True,False,False,False,True,'','','界面风格','522',522,5,'item','abf085d525ccc320ed7b6d05cd02f161',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25015,'','',100,'','func','','e609','rgb(3, 39, 239)',0,0,0,'',False,True,False,False,False,True,'','','Web端风格','abf085d525ccc320ed7b6d05cd02f161',522,0,'item','65af6428b0ff12160d40e372ae5b8337',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25016,'','',100,'','func','','a044','rgb(74, 163, 6)',-5,-2,-2,'',False,False,False,False,False,True,'/orgadmin/template/select.web','','Web端模板','65af6428b0ff12160d40e372ae5b8337',522,0,'item','4be6e84aeacaec7514680b72499b7c19',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25017,'','',100,'','func','','a009','rgb(7, 78, 142)',0,-2,0,'',False,False,False,False,False,True,'/orgadmin/template/Navigation.web','','导航菜单','65af6428b0ff12160d40e372ae5b8337',522,1,'item','185d53f8d69610c63281766012d17a8d',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25018,'','',100,'','func','','a045','rgb(22, 132, 210)',1,0,-1,'',False,False,False,False,False,True,'/orgadmin/template/ShowPicture.web','','轮播图片','65af6428b0ff12160d40e372ae5b8337',522,2,'item','e99f9b903ccc0bdefdbca97abcc9f4b1',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25019,'','',100,'','func','','e622','rgb(204, 103, 5)',2,0,-2,'',False,True,False,False,False,True,'','','移动端风格','abf085d525ccc320ed7b6d05cd02f161',522,1,'item','bc979c4b488eaffbf119e6ab518f7689',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25020,'','',100,'','func','','e677','rgb(1, 174, 73)',5,-2,-2,'',False,False,False,False,False,True,'/orgadmin/template/select.mobi','','移动端模板','bc979c4b488eaffbf119e6ab518f7689',522,0,'item','df3455c4a980c841604b55dc6651a92f',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25021,'','',100,'','func','','e632','rgb(7, 134, 159)',-9,-2,-1,'',False,False,False,False,False,True,'/orgadmin/template/Navigation.mobi','','导航按钮','bc979c4b488eaffbf119e6ab518f7689',522,1,'item','9bbdcbde47d569e6a9d5c59a8947a445',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25022,'','',100,'','func','','a045','rgb(159, 65, 7)',1,0,-2,'',False,False,False,False,False,True,'/orgadmin/template/ShowPicture.mobi','','轮播图片','bc979c4b488eaffbf119e6ab518f7689',522,2,'item','5469dba2b4b8d54745500eea8c1ba089',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25023,'','',100,'','func','','a029','rgb(2, 89, 111)',0,0,0,'',False,True,False,False,False,True,'','','友情链接','abf085d525ccc320ed7b6d05cd02f161',522,2,'item','2f1359ccc50792dbf8c75483a6f508fb',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25024,'','',100,'','func','','a03d','rgb(4, 103, 177)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/links/list','','所有链接','2f1359ccc50792dbf8c75483a6f508fb',522,0,'item','c68451aaa777687e559756c9f02f68d3',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25025,'','',100,'','func','','a015','rgb(132, 59, 7)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/links/sort','','链接分类','2f1359ccc50792dbf8c75483a6f508fb',522,1,'item','8a539ecff79b6ede1b38b2a8380e86cd',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (25026,'','',100,'','func','','a033','rgb(131, 4, 194)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/setup/Extracode','','附加代码','abf085d525ccc320ed7b6d05cd02f161',522,3,'item','1697103633401',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24954,'教务','',100,'','func','','e76b','',-2,0,2,'gggg',False,True,False,False,False,True,'','','教务管理','522',522,0,'item','cba85fde312efe019f9acafe32038fe4',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24955,'','',100,'','func','','e813','rgb(42, 130, 3)',0,0,0,'',False,True,False,False,False,True,'','','学习内容','cba85fde312efe019f9acafe32038fe4',522,0,'item','a76471634c09b23347199ee23682d1ed',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24956,'','rgb(66, 163, 6)',100,'','func','','e813','',0,0,0,'课程管理，包括课程章节、视频、试题、价格等',False,False,False,False,False,True,'/orgadmin/course/list','','课程','a76471634c09b23347199ee23682d1ed',522,0,'item','a2d8c81ec24efe439b4c9b2d139e99fe',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24957,'','rgb(3, 121, 180)',100,'','func','','e750','rgb(5, 74, 191)',3,0,0,'',False,False,False,False,False,True,'/orgadmin/Subject/list','','专业','a76471634c09b23347199ee23682d1ed',522,1,'item','f9b11e7920f6ab15ead04eeafb511830',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24958,'','rgb(107, 8, 210)',100,'','func','','e755','',-4,0,0,'',False,False,False,False,False,True,'/orgadmin/Question/list','','试题库','a76471634c09b23347199ee23682d1ed',522,2,'item','8546016f8e1c6e078b5dddd0eab7920d',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24959,'','',100,'','func','','e804','rgb(220, 51, 6)',8,0,0,'',False,True,False,False,False,True,'','','学员管理','cba85fde312efe019f9acafe32038fe4',522,1,'item','fc20f555f70b93019964a391d33a14d5',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24960,'','',100,'','func','','e808','rgb(2, 122, 20)',2,0,0,'',False,False,False,False,False,True,'/orgadmin/Student/list','','学员信息','fc20f555f70b93019964a391d33a14d5',522,0,'item','b50ea4a3ed65be9d39651c1f1ecf014c',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24961,'','',100,'','func','','e723','',0,0,0,'',False,False,False,False,False,True,'/orgadmin/Student/Activation','','活跃度','fc20f555f70b93019964a391d33a14d5',522,1,'item','1708482766944',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24962,'','',100,'','func','','e67d','rgb(34, 0, 252)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/Student/sort','','学员组','fc20f555f70b93019964a391d33a14d5',522,2,'item','53774f25cbb2a6248bdc4b5783d1f842',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24963,'','',100,'','func','','e650','rgb(0, 85, 169)',0,0,0,'',False,True,False,False,False,True,'','','教师管理','cba85fde312efe019f9acafe32038fe4',522,2,'item','bc07d10b0c411cab2e14f60328e8e4ad',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24964,'','',100,'','func','','e6a1','rgb(1, 143, 36)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/teacher/list','','教师信息','bc07d10b0c411cab2e14f60328e8e4ad',522,0,'item','19c839b6968161696712b7e7b76c9772',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24965,'','',100,'','func','','e639','rgb(173, 20, 238)',0,0,-2,'',False,False,False,False,False,True,'/orgadmin/Teacher/titles','','教师职称','bc07d10b0c411cab2e14f60328e8e4ad',522,1,'item','651397af8465c643284ff8e137fd8079',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24966,'','',100,'','func','','e68f','rgb(183, 56, 4)',1,0,-2,'',False,False,False,False,False,True,'/orgadmin/setup/Stamp','','学习证明','cba85fde312efe019f9acafe32038fe4',522,3,'item','1639658295720',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24967,'考试','',100,'','func','','e810','',0,0,3,'',False,True,False,False,False,True,'','','专项考试','522',522,1,'item','1665903171969',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24968,'','',100,'','func','','e810','rgb(82, 4, 242)',0,0,0,'',False,False,False,False,False,True,'/teacher/exam/list','','考试管理','1665903171969',569,0,'item','606b87e461d6b43e1ff789ad9b1b11c2',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24969,'','',100,'','func','','e82e','rgb(5, 137, 0)',0,0,0,'',False,False,False,False,False,True,'/teacher/exam/results','','考试成绩','1665903171969',569,1,'item','f2b59e41fb0d29f16707ad11b590e686',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24970,'运维','',100,'','func','','e79b','',3,0,3,'',False,True,False,False,False,True,'','','运营与维护','522',522,2,'item','1697428052534',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24971,'','',100,'','func','','e60f','rgb(196, 32, 2)',5,0,-2,'',False,False,False,False,False,True,'/orgadmin/Rechargecode/Codeset','','充值卡','1697428052534',522,0,'item','1697428790833',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24972,'','',100,'','func','','e685','rgb(0, 130, 30)',5,0,-2,'',False,False,False,False,False,True,'/orgadmin/Learningcard/cardset','','学习卡','1697428052534',522,1,'item','1697428791888',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24973,'','',100,'','func','','e824','rgb(255, 123, 0)',0,0,0,'',False,True,False,False,False,True,'','','资金管理','1697428052534',522,2,'item','1697791565174',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24974,'','',100,'','func','','e749','rgb(235, 160, 0)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/Capital/Records','','资金流水','1697791565174',522,0,'item','1697428808320',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24975,'','',100,'','func','','e607','rgb(0, 154, 19)',10,0,0,'',False,False,False,False,False,True,'/orgadmin/Capital/Accounts','','人工充值','1697791565174',522,1,'item','1697791615582',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24976,'','',100,'','func','','e697','rgb(1, 163, 29)',0,0,0,'',False,False,False,False,False,True,'/orgadmin/notice/list','','通知公告','1697428052534',522,3,'item','7505573f225da91c421b31e8e950aa16',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24977,'','',100,'','func','','e75c','rgb(244, 56, 0)',0,0,0,'',False,True,False,False,False,True,'','','新闻管理','1697428052534',522,4,'item','d001a8ab9566b79e46dc6d81fb7fa213',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24978,'','',100,'','func','','e71c','rgb(201, 101, 6)',-5,0,-2,'新闻管理',False,False,False,False,False,True,'/orgadmin/news/list','','新闻发布','d001a8ab9566b79e46dc6d81fb7fa213',522,0,'item','fdffc2a7aa807909b9c259169c70794d',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24979,'','',100,'','func','','e668','rgb(7, 120, 201)',0,0,-2,'',False,False,False,False,False,True,'/orgadmin/news/Columns','','新闻栏目','d001a8ab9566b79e46dc6d81fb7fa213',522,1,'item','e3346bd15202ce654c42f126d2153a41',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24980,'','',100,'','func','','e79e','rgb(102, 4, 214)',10,0,0,'',False,True,False,False,False,True,'','','数据校正','1697428052534',522,5,'item','1697789368523',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24981,'','',100,'','func','','e6d8','rgb(245, 74, 0)',-6,0,2,'',False,False,False,False,False,True,'/manage/ques/itemrepeat','','试题修复','1697789368523',522,0,'item','1697789410970',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24982,'','',86,'','func','','a049','rgb(15, 134, 197)',0,0,0,'人工修正视频学习进度、试题练习完成度',False,False,False,False,False,True,'/orgadmin/data/VideoProgress','','学习进度','1697789368523',522,1,'item','1697789613476',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24782,'','',100,'','func','','e813','',0,0,0,'已经选学的课程',False,True,False,False,False,True,'/student/course/index','','我的课程','547',547,0,'item','99e3c10a6ff4c0af38d4ad6551662222',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24783,'','rgb(1, 159, 200)',100,'','func','','e6f1','',0,0,0,'',False,True,False,False,False,True,'','','学习回顾','547',547,1,'item','8f2fedf0d52426d8107380eb60af23cb',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24784,'','',0,'','func','','e634','',0,0,0,'已经选学的课程',False,False,False,False,False,False,'/Student/Test/Archives','','测试成绩','8f2fedf0d52426d8107380eb60af23cb',547,0,'item','e61b2ed6eed9cb98323eadfde84d1d99',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24785,'','',100,'','func','','e810','',0,0,0,'已经选学的课程',False,False,False,False,False,True,'/Student/exam/Results','','专项考试','8f2fedf0d52426d8107380eb60af23cb',547,1,'item','0df6f2c3642c081462a35f1f1ada550a',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24786,'','',100,'','func','','e6b0','',0,0,0,'已经选学的课程',False,False,False,False,False,True,'/student/Question/errors','','错题回顾','8f2fedf0d52426d8107380eb60af23cb',547,2,'item','caedac420273252de2dadfd450a98382',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24787,'','',0,'','func','','e667','',0,0,0,'',False,False,False,False,False,False,'/student/study/certificate','','学习证明','8f2fedf0d52426d8107380eb60af23cb',547,3,'item','9570d28a7c77d0137551fa4d499e9988',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24788,'','rgb(2, 128, 29)',100,'','func','','e639','',0,0,0,'',False,False,False,False,False,True,'/student/study/Certificate','','学习证明','547',547,2,'item','1642663352817',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24789,'','',100,'','func','','','',0,0,0,'',False,False,False,False,False,True,'','','--分隔线-','547',547,3,'hr','1641613171019',0,'',False,False,False,False,0);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24790,'','',100,'','func','','e68f','',0,0,0,'',False,True,False,False,False,True,'','','个人信息','547',547,4,'node','c1d2a8cba0766a36dafa91f43a581f18',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24791,'','',100,'','func','','a043','',0,0,0,'已经选学的课程',False,False,False,False,False,True,'/student/Self/info','','基本信息','c1d2a8cba0766a36dafa91f43a581f18',547,0,'item','7c5f1c92ee9e6c364a46c755df860b26',300,'',False,False,False,False,400);INSERT INTO "ManageMenu"("MM_Id","MM_AbbrName","MM_Color","MM_Complete","MM_Font","MM_Func","MM_Help","MM_IcoCode","MM_IcoColor","MM_IcoSize","MM_IcoX","MM_IcoY","MM_Intro","MM_IsBold","MM_IsChilds","MM_IsFixed","MM_IsItalic","MM_IsShow","MM_IsUse","MM_Link","MM_Marker","MM_Name","MM_PatId","MM_Root","MM_Tax","MM_Type","MM_UID","MM_WinHeight","MM_WinID","MM_WinMax","MM_WinMin","MM_WinMove","MM_WinResize","MM_WinWidth") VALUES (24792,'','',100,'','func','','e71a','',0,0,0,'已经选学的课程',False,False,False,False,False,True,'/student/Self/link','','联系方式','c1d2a8cba0766a36dafa91f43a581f18',547,1,'item','22475af5e44f46286660708fb4f2c4c9',300,'',False,False,False,False,400);
-- 表 ManageMenu 的索引 --
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_Func" ON public."ManageMenu" USING btree ("MM_Func" DESC);
CREATE UNIQUE INDEX IF NOT EXISTS "ManageMenu_aaaaaManageMenu_PK" ON public."ManageMenu" USING btree ("MM_Id");
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_IsShow" ON public."ManageMenu" USING btree ("MM_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_IsUse" ON public."ManageMenu" USING btree ("MM_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_PatId" ON public."ManageMenu" USING btree ("MM_PatId" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_Tax" ON public."ManageMenu" USING btree ("MM_Tax" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_UID" ON public."ManageMenu" USING btree ("MM_UID" DESC);

-- 创建表 ManageMenu_Point --
CREATE TABLE IF NOT EXISTS public."ManageMenu_Point"
(
	"MMP_Id" integer NOT NULL,
	"FPI_Id" integer NOT NULL,
	"MMP_FileName" character varying(150) COLLATE pg_catalog."default",
	"MMP_IsShow" boolean NOT NULL,
	"MMP_IsUse" boolean NOT NULL,
	"MM_Id" integer NOT NULL,
	 CONSTRAINT key_managemenu_point PRIMARY KEY ("MMP_Id")
);
-- 表 ManageMenu_Point 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ManageMenu_Point_MMP_Id_seq";
ALTER SEQUENCE IF EXISTS public."ManageMenu_Point_MMP_Id_seq" OWNED BY public."ManageMenu_Point"."MMP_Id";
ALTER TABLE "ManageMenu_Point" ALTER COLUMN "MMP_Id" SET DEFAULT NEXTVAL('"ManageMenu_Point_MMP_Id_seq"'::regclass);


-- 表 ManageMenu_Point 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "ManageMenu_Point_aaaaaManageMenu_Point_PK" ON public."ManageMenu_Point" USING btree ("MMP_Id");

-- 创建表 Message --
CREATE TABLE IF NOT EXISTS public."Message"
(
	"Msg_Id" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Ac_Photo" character varying(255) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Msg_Context" character varying(255) COLLATE pg_catalog."default",
	"Msg_CrtTime" timestamp without time zone,
	"Msg_Del" boolean NOT NULL,
	"Msg_IP" character varying(200) COLLATE pg_catalog."default",
	"Msg_IsReply" boolean NOT NULL,
	"Msg_Likenum" integer NOT NULL,
	"Msg_Phone" character varying(255) COLLATE pg_catalog."default",
	"Msg_PlayTime" integer NOT NULL,
	"Msg_QQ" character varying(255) COLLATE pg_catalog."default",
	"Msg_ReContext" character varying(1000) COLLATE pg_catalog."default",
	"Msg_ReadTime" timestamp without time zone,
	"Msg_State" integer NOT NULL,
	"Msg_Title" character varying(100) COLLATE pg_catalog."default",
	"Ol_ID" bigint NOT NULL,
	"Org_Id" integer NOT NULL,
	 CONSTRAINT key_message PRIMARY KEY ("Msg_Id")
);
-- 表 Message 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Message_Msg_Id_seq";
ALTER SEQUENCE IF EXISTS public."Message_Msg_Id_seq" OWNED BY public."Message"."Msg_Id";
ALTER TABLE "Message" ALTER COLUMN "Msg_Id" SET DEFAULT NEXTVAL('"Message_Msg_Id_seq"'::regclass);


-- 表 Message 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Message_aaaaaMessage_PK" ON public."Message" USING btree ("Msg_Id");

-- 创建表 MessageBoard --
CREATE TABLE IF NOT EXISTS public."MessageBoard"
(
	"Mb_Id" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(255) COLLATE pg_catalog."default",
	"Ac_Photo" character varying(255) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Mb_AnsTime" timestamp without time zone,
	"Mb_Answer" text,
	"Mb_At" integer NOT NULL,
	"Mb_Content" text,
	"Mb_CrtTime" timestamp without time zone,
	"Mb_Email" character varying(255) COLLATE pg_catalog."default",
	"Mb_FluxNumber" integer NOT NULL,
	"Mb_IP" character varying(255) COLLATE pg_catalog."default",
	"Mb_IsAns" boolean NOT NULL,
	"Mb_IsDel" boolean NOT NULL,
	"Mb_IsShow" boolean NOT NULL,
	"Mb_IsTheme" boolean NOT NULL,
	"Mb_PID" integer NOT NULL,
	"Mb_Phone" character varying(255) COLLATE pg_catalog."default",
	"Mb_QQ" character varying(50) COLLATE pg_catalog."default",
	"Mb_ReplyNumber" integer NOT NULL,
	"Mb_Title" character varying(255) COLLATE pg_catalog."default",
	"Mb_UID" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Th_ID" integer NOT NULL,
	 CONSTRAINT key_messageboard PRIMARY KEY ("Mb_Id")
);
-- 表 MessageBoard 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."MessageBoard_Mb_Id_seq";
ALTER SEQUENCE IF EXISTS public."MessageBoard_Mb_Id_seq" OWNED BY public."MessageBoard"."Mb_Id";
ALTER TABLE "MessageBoard" ALTER COLUMN "Mb_Id" SET DEFAULT NEXTVAL('"MessageBoard_Mb_Id_seq"'::regclass);


-- 表 MessageBoard 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "MessageBoard_aaaaaMessageBoard_PK" ON public."MessageBoard" USING btree ("Mb_Id");

-- 创建表 MoneyAccount --
CREATE TABLE IF NOT EXISTS public."MoneyAccount"
(
	"Ma_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(50) COLLATE pg_catalog."default",
	"Ma_Buyer" character varying(255) COLLATE pg_catalog."default",
	"Ma_CrtTime" timestamp without time zone NOT NULL,
	"Ma_From" integer NOT NULL,
	"Ma_Info" character varying(500) COLLATE pg_catalog."default",
	"Ma_IsSuccess" boolean NOT NULL,
	"Ma_Money" numeric(18,4) NOT NULL,
	"Ma_Remark" character varying(1000) COLLATE pg_catalog."default",
	"Ma_Seller" character varying(255) COLLATE pg_catalog."default",
	"Ma_Serial" character varying(100) COLLATE pg_catalog."default",
	"Ma_Source" character varying(200) COLLATE pg_catalog."default",
	"Ma_Status" integer NOT NULL,
	"Ma_Total" numeric(18,4) NOT NULL,
	"Ma_Type" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Pai_ID" integer NOT NULL,
	"Rc_Code" character varying(100) COLLATE pg_catalog."default",
	 CONSTRAINT key_moneyaccount PRIMARY KEY ("Ma_ID")
);
-- 表 MoneyAccount 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."MoneyAccount_Ma_ID_seq";
ALTER SEQUENCE IF EXISTS public."MoneyAccount_Ma_ID_seq" OWNED BY public."MoneyAccount"."Ma_ID";
ALTER TABLE "MoneyAccount" ALTER COLUMN "Ma_ID" SET DEFAULT NEXTVAL('"MoneyAccount_Ma_ID_seq"'::regclass);


-- 表 MoneyAccount 的索引 --
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ac_ID" ON public."MoneyAccount" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_CrtTime" ON public."MoneyAccount" USING btree ("Ma_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_From" ON public."MoneyAccount" USING btree ("Ma_From" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_Type" ON public."MoneyAccount" USING btree ("Ma_Type" DESC);

-- 创建表 Navigation --
CREATE TABLE IF NOT EXISTS public."Navigation"
(
	"Nav_ID" integer NOT NULL,
	"Nav_Child" integer NOT NULL,
	"Nav_Color" character varying(255) COLLATE pg_catalog."default",
	"Nav_CrtTime" timestamp without time zone NOT NULL,
	"Nav_EnName" character varying(255) COLLATE pg_catalog."default",
	"Nav_Event" text,
	"Nav_Font" character varying(255) COLLATE pg_catalog."default",
	"Nav_Icon" character varying(50) COLLATE pg_catalog."default",
	"Nav_Image" character varying(255) COLLATE pg_catalog."default",
	"Nav_Intro" character varying(255) COLLATE pg_catalog."default",
	"Nav_IsBold" boolean NOT NULL,
	"Nav_IsShow" boolean NOT NULL,
	"Nav_Logo" character varying(255) COLLATE pg_catalog."default",
	"Nav_Name" character varying(255) COLLATE pg_catalog."default",
	"Nav_PID" character varying(255) COLLATE pg_catalog."default",
	"Nav_Site" character varying(255) COLLATE pg_catalog."default",
	"Nav_Target" character varying(255) COLLATE pg_catalog."default",
	"Nav_Tax" integer NOT NULL,
	"Nav_Title" character varying(255) COLLATE pg_catalog."default",
	"Nav_Type" character varying(255) COLLATE pg_catalog."default",
	"Nav_UID" character varying(255) COLLATE pg_catalog."default",
	"Nav_Url" character varying(1000) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_navigation PRIMARY KEY ("Nav_ID")
);
-- 表 Navigation 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Navigation_Nav_ID_seq";
ALTER SEQUENCE IF EXISTS public."Navigation_Nav_ID_seq" OWNED BY public."Navigation"."Nav_ID";
ALTER TABLE "Navigation" ALTER COLUMN "Nav_ID" SET DEFAULT NEXTVAL('"Navigation_Nav_ID_seq"'::regclass);

INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (2,0,'','2014-05-31 11:12:22','','','','','','',False,True,'201608240908032030.jpg','首页','','web','',1,'','main','2','/default.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (6,0,'','2014-05-31 14:05:13','','','','','','',False,True,'','课程中心','','web','',18,'课程中心','main','6','/Courses.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (11,0,'','2014-06-01 16:44:56','','','','','','',False,True,'','通知公告','','web','',5,'','foot','11','/notices.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (12,0,'','2014-06-01 16:45:22','','','','','','',False,True,'','新闻资讯','','web','',6,'','foot','12','/news.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (13,0,'#003562','2014-06-01 16:45:44','','','','','','',True,True,'','机构管理','','web','_blank',7,'','foot','13','/admin/index.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (14,0,'','2014-06-01 16:46:07','','','','','','',False,True,'','友情链接','','web','',8,'','foot','14','/links.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (15,0,'','2014-06-01 16:46:30','','','','','','',False,True,'','关于我们','','web','',9,'','foot','15','/about.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (16,0,'','2014-06-01 16:46:55','','','','','','',False,True,'','联系我们','','web','',12,'','foot','16','/Contactus.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (21,0,'','2014-06-01 17:23:39','','','','','','',False,False,'','教师','','web','',19,'','main','21','/teacher/List.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (22,0,'','2014-06-01 17:24:39','','','','','','',False,True,'','在线练习','','web','',20,'','main','22','/Training.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (26,0,'','2014-06-01 17:30:11','','','','','','',False,False,'','测试','','web','',22,'','main','26','/test.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (27,0,'','2014-06-01 17:30:23','','','','','','',False,True,'','在线考试','','web','',21,'','main','27','/exam.ashx',3,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (47,0,'','2016-09-11 10:48:07','','','','','','',False,True,'','工作动态','','web','',17,'','main','47','/newslist.ashx?colid=5',3,'郑州市司法局网络培训学院');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (48,0,'','2016-09-11 10:48:19','','','','','','',False,True,'','新闻中心','','web','',4,'','main','48','/news.ashx',3,'郑州市司法局网络培训学院');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (49,0,'','2016-09-11 10:48:32','','','','','','',False,False,'','在线帮助','','web','',23,'','main','49','/newslist.ashx?colid=11',3,'郑州市司法局网络培训学院');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (50,0,'','2016-09-11 10:48:40','','','','','','',False,True,'','关于我们','','web','',24,'','main','50','/about.ashx',3,'郑州市司法局网络培训学院');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (51,0,'','2016-09-11 10:51:12','','','','','','',False,True,'','政策法规','','web','',15,'','main','51','/newslist.ashx?colid=6',3,'郑州市司法局网络培训学院');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (52,0,'','2016-09-26 20:54:24','','','','','','',False,True,'','国内新闻','48','web','',0,'','main','52','/newslist.ashx?colid=8',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (53,0,'','2016-09-26 20:59:29','','','','','','',False,True,'','省内新闻','48','web','',1,'','main','53','/newslist.ashx?colid=9',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (54,0,'','2016-09-26 20:59:49','','','','','','',False,True,'','工作动态','48','web','',2,'','main','54','/newslist.ashx?colid=5',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (57,0,'','2016-09-28 23:10:09','','','','','','',False,True,'','十八届五中全会精神','6','web','',0,'','main','57','/Courses.ashx?sbjid=94',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (58,0,'','2016-09-28 23:10:51','','','','','','',False,True,'','两学一做','6','web','',1,'','main','58','/Courses.ashx?sbjid=92',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (59,0,'','2016-09-28 23:12:08','','','','','','',False,True,'','通知公告','','web','',14,'','main','59','/notices.ashx',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (60,0,'','2016-09-29 15:41:41','','','','','','',False,False,'','司法资讯','52','web','',0,'','main','60','',3,'郑州市司法系统学法用法平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (61,0,'','2016-11-27 11:21:15','','','','','','',False,False,'','首页','','web','',0,'','main','61','/default.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (62,0,'','2016-11-27 11:22:11','','','','','','',False,True,'','课程','','web','',1,'课程中心','main','62','/Courses.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (63,0,'','2016-11-27 11:25:23','','','','','','',False,True,'','新闻','','web','',2,'','main','63','/news.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (646,0,'','1753-01-01 00:00:00','','','','e697','','',False,True,'','通知公告','','web','',0,'','foot','80','/web/notice',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (647,0,'','1753-01-01 00:00:00','','','','e75c','','',False,True,'','新闻资讯','','web','',1,'','foot','81','/web/news',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (91,0,'','2017-02-25 16:57:27','','','','','','',False,True,'201702250523350090.jpg','自定义菜单2','','mobi','',1,'','main','91','',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (92,0,'','2017-02-25 16:57:47','','','','','','',False,True,'','自定义菜单1','','mobi','',0,'','main','92','',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (93,0,'','2017-02-25 17:10:00','','','','','','',False,True,'201702250524198570.jpg','自定义菜单3','','mobi','',2,'','main','93','',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (64,0,'','2016-11-27 11:25:49','','','','','','',False,False,'','教师','','web','',3,'','main','64','/teacher/List.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (65,0,'','2016-11-27 11:26:09','','','','','','',False,False,'','练习','','web','',4,'','main','65','/Training.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (66,0,'','2016-11-27 11:26:31','','','','','','',False,True,'','测试','','web','',5,'','main','66','/test.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (67,0,'','2016-11-27 11:27:09','','','','','','',False,True,'','考试','','web','',6,'','main','67','/exam.ashx',2,'郑州微厦计算机科技有限公司');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (648,0,'','1753-01-01 00:00:00','','','','a038','','',False,True,'','机构管理','','web','',2,'','foot','82','/orgadmin',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (649,0,'','1753-01-01 00:00:00','','','','e67d','','',False,True,'','关于我们','','web','',3,'','foot','83','/web/about',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (650,1,'','1753-01-01 00:00:00','','','','e751','','',False,False,'','联系我们','','web','',4,'','foot','84','/web/conn',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (850,0,'','1753-01-01 00:00:00','','','','a020','','',False,True,'','首页','','web','',0,'','main','8b6a174de0e1ecceede7106127e7d83a','/',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (851,1,'','1753-01-01 00:00:00','','','','e813','','',False,True,'','课程中心','','web','',1,'','main','68','/web/Course',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (852,0,'','1753-01-01 00:00:00','','','','e810','','',False,True,'','考务中心','','web','',2,'','main','72','/web/exam',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (853,1,'','1753-01-01 00:00:00','','','','e75c','','',False,True,'','新闻资讯','','web','',3,'','main','73','/web/news',4,'云课堂网校平台');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (854,0,'','1753-01-01 00:00:00','','','','e697','','',False,True,'','通知公告','','web','',4,'','main','8106f4b832995ffcd174b5274c85a40e','/web/notice',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (855,1,'','1753-01-01 00:00:00','','','','a026','','',False,True,'','帮助','','web','',5,'','main','3d094a35a075a331545bc407e9aa57f6','',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (856,0,'','1753-01-01 00:00:00','','','','a026','','',False,True,'','帮助中心','3d094a35a075a331545bc407e9aa57f6','web','_blank',0,'','main','74daa309accbe357765af9be24ff3c3a','/help',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (857,0,'','1753-01-01 00:00:00','','','','a03d','','',False,True,'','在线帮助','3d094a35a075a331545bc407e9aa57f6','web','_blank',1,'','main','7c28d33b967c57712d32edc585e25f8a','http://www.weisha100.net/',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (858,1,'','1753-01-01 00:00:00','','','','a034','','',False,True,'','开源代码','3d094a35a075a331545bc407e9aa57f6','web','',2,'','main','c4708328e12561efd42f000a13f3e28c','',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (859,0,'','1753-01-01 00:00:00','','','','e686','','',False,True,'','Gitee源码库','c4708328e12561efd42f000a13f3e28c','web','_blank',0,'','main','777c15c95d66c4fd146c4460ced427b1','https://gitee.com/weishakeji/LearningSystem',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (860,0,'','1753-01-01 00:00:00','','','','e691','','',False,True,'','Github源码库','c4708328e12561efd42f000a13f3e28c','web','_blank',1,'','main','4ffaf75cc966646a4694cf5d3ad78b3d','https://github.com/weishakeji/LearningSystem',4,'');INSERT INTO "Navigation"("Nav_ID","Nav_Child","Nav_Color","Nav_CrtTime","Nav_EnName","Nav_Event","Nav_Font","Nav_Icon","Nav_Image","Nav_Intro","Nav_IsBold","Nav_IsShow","Nav_Logo","Nav_Name","Nav_PID","Nav_Site","Nav_Target","Nav_Tax","Nav_Title","Nav_Type","Nav_UID","Nav_Url","Org_ID","Org_Name") VALUES (861,0,'','1753-01-01 00:00:00','','','','e67d','','',False,True,'','关于我们','3d094a35a075a331545bc407e9aa57f6','web','',3,'','main','f883521693b859158123f9bd9e835d99','/web/about',4,'');
-- 表 Navigation 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Navigation_aaaaaNavigation_PK" ON public."Navigation" USING btree ("Nav_ID");

-- 创建表 NewsNote --
CREATE TABLE IF NOT EXISTS public."NewsNote"
(
	"Nn_Id" integer NOT NULL,
	"Art_Id" bigint NOT NULL,
	"Nn_City" character varying(255) COLLATE pg_catalog."default",
	"Nn_CrtTime" timestamp without time zone,
	"Nn_Details" character varying(50) COLLATE pg_catalog."default",
	"Nn_Email" character varying(100) COLLATE pg_catalog."default",
	"Nn_IP" character varying(50) COLLATE pg_catalog."default",
	"Nn_IsShow" boolean NOT NULL,
	"Nn_Name" character varying(50) COLLATE pg_catalog."default",
	"Nn_Province" character varying(255) COLLATE pg_catalog."default",
	"Nn_Title" character varying(100) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_newsnote PRIMARY KEY ("Nn_Id")
);
-- 表 NewsNote 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."NewsNote_Nn_Id_seq";
ALTER SEQUENCE IF EXISTS public."NewsNote_Nn_Id_seq" OWNED BY public."NewsNote"."Nn_Id";
ALTER TABLE "NewsNote" ALTER COLUMN "Nn_Id" SET DEFAULT NEXTVAL('"NewsNote_Nn_Id_seq"'::regclass);


-- 表 NewsNote 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "NewsNote_aaaaaNewsNote_PK" ON public."NewsNote" USING btree ("Nn_Id");

-- 创建表 Notice --
CREATE TABLE IF NOT EXISTS public."Notice"
(
	"No_Id" bigint NOT NULL,
	"Acc_Id" integer NOT NULL,
	"Acc_Name" character varying(50) COLLATE pg_catalog."default",
	"No_BgImage" text,
	"No_Context" text,
	"No_CrtTime" timestamp without time zone,
	"No_EndTime" timestamp without time zone,
	"No_Height" integer NOT NULL,
	"No_Interval" character varying(2000) COLLATE pg_catalog."default",
	"No_IsShow" boolean NOT NULL,
	"No_IsTop" boolean NOT NULL,
	"No_Linkurl" character varying(2000) COLLATE pg_catalog."default",
	"No_OpenCount" integer NOT NULL,
	"No_Organ" character varying(50) COLLATE pg_catalog."default",
	"No_Page" character varying(200) COLLATE pg_catalog."default",
	"No_Range" integer NOT NULL,
	"No_StartTime" timestamp without time zone,
	"No_StudentSort" character varying(2000) COLLATE pg_catalog."default",
	"No_Timespan" integer NOT NULL,
	"No_Ttl" character varying(255) COLLATE pg_catalog."default",
	"No_Type" integer NOT NULL,
	"No_ViewNum" integer NOT NULL,
	"No_Width" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_notice PRIMARY KEY ("No_Id")
);
INSERT INTO "Notice"("No_Id","Acc_Id","Acc_Name","No_BgImage","No_Context","No_CrtTime","No_EndTime","No_Height","No_Interval","No_IsShow","No_IsTop","No_Linkurl","No_OpenCount","No_Organ","No_Page","No_Range","No_StartTime","No_StudentSort","No_Timespan","No_Ttl","No_Type","No_ViewNum","No_Width","Org_ID","Org_Name") VALUES (129504671140155392,0,'','','<p>&ldquo;视频学习、试题练习、在线考试&rdquo;紧密相联，打造成为集&nbsp;<strong>&ldquo;学、练、考&rdquo;</strong>&nbsp;于一体的在线学习系统。&ldquo;点播/直播&rdquo;、&ldquo;刷题/测试&rdquo;、&ldquo;组卷/考试&rdquo;，根据学习内容的不同权重汇总综合成绩，生成学习证明。</p>
<p>支持在线支付（微信支付、支付宝支付）；利用充值卡、学习卡配合线下营销；Web端、APP、小程序，多终端方便学习。<strong><span style="color: #e03e2d;">安装用户突破四万家。</span></strong></p>
<p><a href="/help/" target="_blank" rel="noopener">?帮助中心</a> &nbsp;<a href="/web/link" target="_blank" rel="noopener">&spades;友商推荐</a></p>
<p><strong>演示信息</strong></p>
<ul>
<li style="list-style-type: initial;">测试账号： tester 密码1 （教师与学员只是不同角色）</li>
<li style="list-style-type: initial;">机构管理员： <a href="/orgadmin" target="_blank" rel="noopener">/orgadmin</a> 账号admin 密码1</li>
<li style="list-style-type: initial;">超级管理员：<a href="/manage" target="_blank" rel="noopener"> /manage</a> 账号super 密码1</li>
</ul>
<p><strong>开源地址</strong></p>
<ul>
<li style="list-style-type: initial;">Gitee ：<a href="https://gitee.com/weishakeji/LearningSystem" target="_blank" rel="noopener">https://gitee.com/weishakeji/LearningSystem</a></li>
<li style="list-style-type: initial;">GitHub ：<a href="https://github.com/weishakeji/LearningSystem" target="_blank" rel="noopener">https://github.com/weishakeji/LearningSystem</a></li>
</ul>','2023-02-09 16:12:16','2223-01-01 23:59:59',600,'',True,False,'',0,'','all_home',1,'2023-01-01 00:00:00','',300,'产品简介 - 安装用户突破四万家',2,13,800,4,'郑州微厦计算机科技有限公司');

-- 创建表 OrganLevel --
CREATE TABLE IF NOT EXISTS public."OrganLevel"
(
	"Olv_ID" integer NOT NULL,
	"Olv_Intro" character varying(1000) COLLATE pg_catalog."default",
	"Olv_IsDefault" boolean NOT NULL,
	"Olv_IsUse" boolean NOT NULL,
	"Olv_Level" integer NOT NULL,
	"Olv_Name" character varying(255) COLLATE pg_catalog."default",
	"Olv_Tag" character varying(255) COLLATE pg_catalog."default",
	"Olv_Tax" integer NOT NULL,
	"Ps_ID" integer NOT NULL,
	 CONSTRAINT key_organlevel PRIMARY KEY ("Olv_ID")
);
-- 表 OrganLevel 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."OrganLevel_Olv_ID_seq";
ALTER SEQUENCE IF EXISTS public."OrganLevel_Olv_ID_seq" OWNED BY public."OrganLevel"."Olv_ID";
ALTER TABLE "OrganLevel" ALTER COLUMN "Olv_ID" SET DEFAULT NEXTVAL('"OrganLevel_Olv_ID_seq"'::regclass);

INSERT INTO "OrganLevel"("Olv_ID","Olv_Intro","Olv_IsDefault","Olv_IsUse","Olv_Level","Olv_Name","Olv_Tag","Olv_Tax","Ps_ID") VALUES (1,'',False,True,1,'VIP','vip0',2,1);INSERT INTO "OrganLevel"("Olv_ID","Olv_Intro","Olv_IsDefault","Olv_IsUse","Olv_Level","Olv_Name","Olv_Tag","Olv_Tax","Ps_ID") VALUES (2,'',False,True,0,'钻石级','vip1',1,1);INSERT INTO "OrganLevel"("Olv_ID","Olv_Intro","Olv_IsDefault","Olv_IsUse","Olv_Level","Olv_Name","Olv_Tag","Olv_Tax","Ps_ID") VALUES (5,'',True,True,0,'默认机构','default',0,14);INSERT INTO "OrganLevel"("Olv_ID","Olv_Intro","Olv_IsDefault","Olv_IsUse","Olv_Level","Olv_Name","Olv_Tag","Olv_Tax","Ps_ID") VALUES (6,'仅提供在线考试功能。',False,True,0,'在线考试','exam',3,0);

-- 创建表 Organization --
CREATE TABLE IF NOT EXISTS public."Organization"
(
	"Org_ID" integer NOT NULL,
	"Olv_ID" integer NOT NULL,
	"Olv_Name" character varying(255) COLLATE pg_catalog."default",
	"Org_AbbrEnName" character varying(255) COLLATE pg_catalog."default",
	"Org_AbbrName" character varying(255) COLLATE pg_catalog."default",
	"Org_Address" character varying(255) COLLATE pg_catalog."default",
	"Org_BankAcc" character varying(255) COLLATE pg_catalog."default",
	"Org_City" character varying(255) COLLATE pg_catalog."default",
	"Org_CoBank" character varying(255) COLLATE pg_catalog."default",
	"Org_Config" text,
	"Org_Description" character varying(1000) COLLATE pg_catalog."default",
	"Org_District" character varying(255) COLLATE pg_catalog."default",
	"Org_Email" character varying(255) COLLATE pg_catalog."default",
	"Org_EnName" character varying(255) COLLATE pg_catalog."default",
	"Org_ExtraMobi" text,
	"Org_ExtraWeb" text,
	"Org_Fax" character varying(255) COLLATE pg_catalog."default",
	"Org_GonganBeian" character varying(255) COLLATE pg_catalog."default",
	"Org_ICP" character varying(255) COLLATE pg_catalog."default",
	"Org_Intro" text,
	"Org_IsDefault" boolean NOT NULL,
	"Org_IsPass" boolean NOT NULL,
	"Org_IsRoot" boolean NOT NULL,
	"Org_IsShow" boolean NOT NULL,
	"Org_IsUse" boolean NOT NULL,
	"Org_Keywords" character varying(1000) COLLATE pg_catalog."default",
	"Org_Lang" character varying(50) COLLATE pg_catalog."default",
	"Org_Latitude" character varying(255) COLLATE pg_catalog."default",
	"Org_Linkman" character varying(255) COLLATE pg_catalog."default",
	"Org_LinkmanPhone" character varying(255) COLLATE pg_catalog."default",
	"Org_LinkmanQQ" character varying(255) COLLATE pg_catalog."default",
	"Org_Logo" character varying(255) COLLATE pg_catalog."default",
	"Org_Longitude" character varying(255) COLLATE pg_catalog."default",
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Org_Owner" character varying(50) COLLATE pg_catalog."default",
	"Org_Phone" character varying(255) COLLATE pg_catalog."default",
	"Org_PlatformName" character varying(255) COLLATE pg_catalog."default",
	"Org_Province" character varying(255) COLLATE pg_catalog."default",
	"Org_RegTime" timestamp without time zone NOT NULL,
	"Org_Street" character varying(255) COLLATE pg_catalog."default",
	"Org_Template" character varying(255) COLLATE pg_catalog."default",
	"Org_TemplateMobi" character varying(255) COLLATE pg_catalog."default",
	"Org_TwoDomain" character varying(255) COLLATE pg_catalog."default",
	"Org_USCI" character varying(255) COLLATE pg_catalog."default",
	"Org_WebSite" character varying(255) COLLATE pg_catalog."default",
	"Org_Weixin" character varying(255) COLLATE pg_catalog."default",
	"Org_Zip" character varying(20) COLLATE pg_catalog."default",
	 CONSTRAINT key_organization PRIMARY KEY ("Org_ID")
);
-- 表 Organization 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Organization_Org_ID_seq";
ALTER SEQUENCE IF EXISTS public."Organization_Org_ID_seq" OWNED BY public."Organization"."Org_ID";
ALTER TABLE "Organization" ALTER COLUMN "Org_ID" SET DEFAULT NEXTVAL('"Organization_Org_ID_seq"'::regclass);

INSERT INTO "Organization"("Org_ID","Olv_ID","Olv_Name","Org_AbbrEnName","Org_AbbrName","Org_Address","Org_BankAcc","Org_City","Org_CoBank","Org_Config","Org_Description","Org_District","Org_Email","Org_EnName","Org_ExtraMobi","Org_ExtraWeb","Org_Fax","Org_GonganBeian","Org_ICP","Org_Intro","Org_IsDefault","Org_IsPass","Org_IsRoot","Org_IsShow","Org_IsUse","Org_Keywords","Org_Lang","Org_Latitude","Org_Linkman","Org_LinkmanPhone","Org_LinkmanQQ","Org_Logo","Org_Longitude","Org_Name","Org_Owner","Org_Phone","Org_PlatformName","Org_Province","Org_RegTime","Org_Street","Org_Template","Org_TemplateMobi","Org_TwoDomain","Org_USCI","Org_WebSite","Org_Weixin","Org_Zip") VALUES (2,2,'钻石级','WeiSha','微厦科技','郑州市农科路鑫苑世家二号楼606室','','郑州市','','','','金水区','','WeiShaKeji','','','','','','<div class="TRS_Editor" style="font-size: 16px; color: rgb(0, 0, 0); line-height: 28px; overflow: hidden; font-family: 瀹嬩綋;"><div class="Custom_UnionStyle" style="color: rgb(67, 67, 67); line-height: 28px; overflow: hidden;"><p align="justify" style="line-height: 28px; color: black;">经济日报-中国经济网北京3月21日讯（记者吴佳佳）国务院联防联控机制今天召开新闻发布会介绍，当前国内疫情防控总体保持良好态势，我国重点人群新冠疫苗接种工作顺利推进，截至3月20日24时，全国累计报告接种7495.6万剂次，下一步全国将大规模开展60岁以上老年人群的疫苗接种。</p><p align="justify" style="line-height: 28px; color: black;">　　国家卫健委宣传司副司长、新闻发言人米锋指出，各地正在全面抓好常态化疫情防控，保障群众安全、顺畅出行。当前，国内低风险地区持健康通行“绿码”，在测温正常且做好个人防护的前提下可有序出行，各地不得擅自加码。对新增散发病例，要发现一起、扑灭一起，确保不出现规模性反弹。</p><p align="justify" style="line-height: 28px; color: black;">　　关于60岁以上人群新冠疫苗接种的情况，国家卫生健康委员会疾控局一级巡视员贺青华在会上表示，部分地区在充分评估健康状况的情况下和被感染风险的前提下，已经开始为60岁以上身体条件比较好的老人接种新冠疫苗。同时，疫苗研发单位也在加快推进研发，在临床试验取得足够安全性、有效性数据以后，将大规模开展60岁以上老年人群的疫苗接种。</p><p align="justify" style="line-height: 28px; color: black;">　　既然疫苗需要大规模接种，那么产量如何保证？工业和信息化部消费品工业司副司长毛俊锋表示，目前疫苗产量与2月初相比已经有了大幅度提高，下一步还会进一步提升。全年疫苗产量完全可以满足全国人民的接种需求。目前，我国已有5款疫苗获批了附条件上市或者是获准了紧急使用，其他技术路线的疫苗也会陆续上市，一旦产品获批，就会启动生产、上市供应。</p><p align="justify" style="line-height: 28px; color: black;">　　“疫苗生产周期长，涉及环节多，技术含量更高，特别是对它的监管要求也更为严格，尤其是新冠病毒疫苗，现在疫苗生产总量、扩产增产速度在我国都是前所未有的，企业在增产扩能过程中要始终把质量安全放在第一位。”毛俊锋说，企业要切实履行疫苗质量安全的主体责任，严格落实质量管理体系。</p><p align="justify" style="line-height: 28px; color: black;">　　针对国产疫苗不良反应情况，中国疾控中心免疫规划首席专家王华庆介绍，现在监测的不良反应主要包括局部反应和全身反应。局部的不良反应包括如疼痛、红肿、硬结的情况，这些无须处理，会自行痊愈；全身的不良反应包括如头痛、乏力、低热的情况。他介绍，当前接到的不良反应报告为疑似不良反应，也就是说，属于怀疑和疫苗有关的反应。后续将继续开展较为严重的不良反应调查，通过补充调查、了解接种史、疾病情况等，再由专家组做出诊断。</p><p align="justify" style="line-height: 28px; color: black;">　　不少人关心，接种疫苗后可以摘下口罩吗？对此，中国疾控中心副主任冯子健说，由于当前全球新冠疫情仍在持续流行，国内疫苗接种率较低，来自高流行地区的人员或物品入境，仍有导致在境内传播的风险。因此，我国在人群疫苗接种达到较高免疫水平之前，无论是否接种疫苗，在人群聚集的室内或封闭场所，仍然需要继续佩戴口罩，并严格遵循当地具体的防控措施要求。</p></div></div><p style="font-size: 16px; line-height: 28px; color: rgb(0, 0, 0); font-family: 瀹嬩綋; float: right;">（责任编辑：符仲明）</p>',False,True,True,True,True,'','','34.7969676989','','','','201809030230274008.jpg','113.681890337','郑州微厦计算机科技有限公司','','王','微厦在线学习平台','河南省','1753-01-01 00:00:00','农科路','School','','root','','','','');INSERT INTO "Organization"("Org_ID","Olv_ID","Olv_Name","Org_AbbrEnName","Org_AbbrName","Org_Address","Org_BankAcc","Org_City","Org_CoBank","Org_Config","Org_Description","Org_District","Org_Email","Org_EnName","Org_ExtraMobi","Org_ExtraWeb","Org_Fax","Org_GonganBeian","Org_ICP","Org_Intro","Org_IsDefault","Org_IsPass","Org_IsRoot","Org_IsShow","Org_IsUse","Org_Keywords","Org_Lang","Org_Latitude","Org_Linkman","Org_LinkmanPhone","Org_LinkmanQQ","Org_Logo","Org_Longitude","Org_Name","Org_Owner","Org_Phone","Org_PlatformName","Org_Province","Org_RegTime","Org_Street","Org_Template","Org_TemplateMobi","Org_TwoDomain","Org_USCI","Org_WebSite","Org_Weixin","Org_Zip") VALUES (4,5,'默认机构','icloud','网校平台','郑州市','','','','<?xml version="1.0" encoding="UTF-8"?><items><item key="IsLoginForPw" value="True" /><item key="IsLoginForSms" value="True" /><item key="IsSwitchPlay" value="False" /><item key="VideoTolerance" value="6" /><item key="Stamp" value="a9fd754c1b5a257130d2c50c615a1af1.png" /><item key="StampPosition" value="right-top" /><item key="IsVerifyStudent" value="False" /><item key="IsRegStudent" value="True" /><item key="IsDisableChat" value="True" /><item key="finaltest_condition_video" value="1" /><item key="finaltest_weight_video" value="40" /><item key="finaltest_weight_ques" value="0" /><item key="finaltest_weight_exam" value="60" /><item key="finaltest_score_pass" value="60" /><item key="finaltest_condition_ques" value="0" /><item key="finaltest_count" value="5" /></items>','','','','icloud','','','','','','<div>该信息由管理员后台编辑；</div>
<div>&nbsp;</div>
<div>菜单路径：管理中心（管理员）=&gt;&nbsp; 平台管理=&gt;&nbsp; 关于我们</div>
<div>&nbsp;</div>',True,True,False,True,True,'','','34.819187','','','','dc282e74901b2e88fd47a2306bb65f43.jpg','113.757022','郑州微厦计算机科技有限公司','','400-6015615','云课堂网校平台','','2016-12-28 16:45:12','','Default','Default','exam','','','','');
-- 表 Organization 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Organization_aaaaaOrganization_PK" ON public."Organization" USING btree ("Org_ID");

-- 创建表 Outline --
CREATE TABLE IF NOT EXISTS public."Outline"
(
	"Ol_ID" bigint NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Ol_Courseware" text,
	"Ol_CrtTime" timestamp without time zone NOT NULL,
	"Ol_Intro" text,
	"Ol_IsAccessory" boolean NOT NULL,
	"Ol_IsChecked" boolean NOT NULL,
	"Ol_IsFinish" boolean NOT NULL,
	"Ol_IsFree" boolean NOT NULL,
	"Ol_IsLive" boolean NOT NULL,
	"Ol_IsNode" boolean NOT NULL,
	"Ol_IsUse" boolean NOT NULL,
	"Ol_IsVideo" boolean NOT NULL,
	"Ol_LessonPlan" text,
	"Ol_Level" integer NOT NULL,
	"Ol_LiveID" character varying(200) COLLATE pg_catalog."default",
	"Ol_LiveSpan" integer NOT NULL,
	"Ol_LiveTime" timestamp without time zone NOT NULL,
	"Ol_ModifyTime" timestamp without time zone NOT NULL,
	"Ol_Name" character varying(500) COLLATE pg_catalog."default",
	"Ol_PID" bigint NOT NULL,
	"Ol_QuesCount" integer NOT NULL,
	"Ol_QusNumber" integer NOT NULL,
	"Ol_Tax" integer NOT NULL,
	"Ol_UID" character varying(200) NOT NULL COLLATE pg_catalog."default",
	"Ol_Video" text,
	"Ol_XPath" character varying(255) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Sbj_ID" bigint NOT NULL,
	 CONSTRAINT key_outline PRIMARY KEY ("Ol_ID")
);

-- 表 Outline 的索引 --
CREATE INDEX IF NOT EXISTS "Outline_IX_Cou_ID" ON public."Outline" USING btree ("Cou_ID", "Ol_Tax" DESC);

-- 创建表 OutlineEvent --
CREATE TABLE IF NOT EXISTS public."OutlineEvent"
(
	"Oe_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Oe_Answer" character varying(500) COLLATE pg_catalog."default",
	"Oe_Context" text,
	"Oe_CrtTime" timestamp without time zone NOT NULL,
	"Oe_Datatable" text,
	"Oe_EventType" integer NOT NULL,
	"Oe_Height" integer NOT NULL,
	"Oe_IsUse" boolean NOT NULL,
	"Oe_Questype" integer NOT NULL,
	"Oe_Title" character varying(500) COLLATE pg_catalog."default",
	"Oe_TriggerPoint" integer NOT NULL,
	"Oe_Width" integer NOT NULL,
	"Ol_ID" bigint NOT NULL,
	"Ol_UID" character varying(200) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	 CONSTRAINT key_outlineevent PRIMARY KEY ("Oe_ID")
);
-- 表 OutlineEvent 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."OutlineEvent_Oe_ID_seq";
ALTER SEQUENCE IF EXISTS public."OutlineEvent_Oe_ID_seq" OWNED BY public."OutlineEvent"."Oe_ID";
ALTER TABLE "OutlineEvent" ALTER COLUMN "Oe_ID" SET DEFAULT NEXTVAL('"OutlineEvent_Oe_ID_seq"'::regclass);



-- 创建表 PayInterface --
CREATE TABLE IF NOT EXISTS public."PayInterface"
(
	"Pai_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Pai_Config" text,
	"Pai_Currency" character varying(255) COLLATE pg_catalog."default",
	"Pai_Feerate" real NOT NULL,
	"Pai_InterfaceType" character varying(255) COLLATE pg_catalog."default",
	"Pai_Intro" character varying(1000) COLLATE pg_catalog."default",
	"Pai_IsEnable" boolean NOT NULL,
	"Pai_Key" character varying(255) COLLATE pg_catalog."default",
	"Pai_Name" character varying(255) COLLATE pg_catalog."default",
	"Pai_ParterID" character varying(255) COLLATE pg_catalog."default",
	"Pai_Pattern" character varying(255) COLLATE pg_catalog."default",
	"Pai_Platform" character varying(255) COLLATE pg_catalog."default",
	"Pai_Returl" character varying(500) COLLATE pg_catalog."default",
	"Pai_Scene" character varying(500) COLLATE pg_catalog."default",
	"Pai_Tax" integer NOT NULL,
	 CONSTRAINT key_payinterface PRIMARY KEY ("Pai_ID")
);
-- 表 PayInterface 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."PayInterface_Pai_ID_seq";
ALTER SEQUENCE IF EXISTS public."PayInterface_Pai_ID_seq" OWNED BY public."PayInterface"."Pai_ID";
ALTER TABLE "PayInterface" ALTER COLUMN "Pai_ID" SET DEFAULT NEXTVAL('"PayInterface_Pai_ID_seq"'::regclass);



-- 创建表 PointAccount --
CREATE TABLE IF NOT EXISTS public."PointAccount"
(
	"Pa_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Pa_CrtTime" timestamp without time zone NOT NULL,
	"Pa_From" integer NOT NULL,
	"Pa_Info" character varying(500) COLLATE pg_catalog."default",
	"Pa_Remark" character varying(1000) COLLATE pg_catalog."default",
	"Pa_Serial" character varying(100) COLLATE pg_catalog."default",
	"Pa_Source" character varying(200) COLLATE pg_catalog."default",
	"Pa_Total" integer NOT NULL,
	"Pa_TotalAmount" integer NOT NULL,
	"Pa_Type" integer NOT NULL,
	"Pa_Value" integer NOT NULL,
	 CONSTRAINT key_pointaccount PRIMARY KEY ("Pa_ID")
);
-- 表 PointAccount 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."PointAccount_Pa_ID_seq";
ALTER SEQUENCE IF EXISTS public."PointAccount_Pa_ID_seq" OWNED BY public."PointAccount"."Pa_ID";
ALTER TABLE "PointAccount" ALTER COLUMN "Pa_ID" SET DEFAULT NEXTVAL('"PointAccount_Pa_ID_seq"'::regclass);

INSERT INTO "PointAccount"("Pa_ID","Ac_ID","Org_ID","Pa_CrtTime","Pa_From","Pa_Info","Pa_Remark","Pa_Serial","Pa_Source","Pa_Total","Pa_TotalAmount","Pa_Type","Pa_Value") VALUES (1770,2,4,'2023-12-05 04:26:46',1,'账号密码登录','','0004202312050426465585EXAM19','电脑网页',2424,9348,2,10);INSERT INTO "PointAccount"("Pa_ID","Ac_ID","Org_ID","Pa_CrtTime","Pa_From","Pa_Info","Pa_Remark","Pa_Serial","Pa_Source","Pa_Total","Pa_TotalAmount","Pa_Type","Pa_Value") VALUES (1771,2,4,'2024-01-22 17:58:18',1,'账号密码登录','','0004202401220558185804EXAM14','电脑网页',2434,9358,2,10);

-- 创建表 Position --
CREATE TABLE IF NOT EXISTS public."Position"
(
	"Posi_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Posi_Intro" character varying(255) COLLATE pg_catalog."default",
	"Posi_IsAdmin" boolean NOT NULL,
	"Posi_IsUse" boolean NOT NULL,
	"Posi_Name" character varying(255) COLLATE pg_catalog."default",
	"Posi_Tax" integer NOT NULL,
	 CONSTRAINT key_position PRIMARY KEY ("Posi_Id")
);
-- 表 Position 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Position_Posi_Id_seq";
ALTER SEQUENCE IF EXISTS public."Position_Posi_Id_seq" OWNED BY public."Position"."Posi_Id";
ALTER TABLE "Position" ALTER COLUMN "Posi_Id" SET DEFAULT NEXTVAL('"Position_Posi_Id_seq"'::regclass);

INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (2,2,'','',False,True,'财务',3);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (3,2,'','jjj',True,True,'管理员',5);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (4,2,'','',False,True,'总经理',0);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (5,2,'','',False,True,'员工',4);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (6,2,'','',False,True,'部门经理',2);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (7,2,'','',False,True,'副总',1);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (10,4,'中国珠宝网','',True,True,'管理员',0);INSERT INTO "Position"("Posi_Id","Org_ID","Org_Name","Posi_Intro","Posi_IsAdmin","Posi_IsUse","Posi_Name","Posi_Tax") VALUES (19,4,'','',False,True,'测试一下，又一个管理',1);
-- 表 Position 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Position_aaaaaPosition_PK" ON public."Position" USING btree ("Posi_Id");

-- 创建表 ProfitSharing --
CREATE TABLE IF NOT EXISTS public."ProfitSharing"
(
	"Ps_ID" integer NOT NULL,
	"Ps_CouponValue" integer NOT NULL,
	"Ps_Couponratio" integer NOT NULL,
	"Ps_Intro" character varying(500) COLLATE pg_catalog."default",
	"Ps_IsTheme" boolean NOT NULL,
	"Ps_IsUse" boolean NOT NULL,
	"Ps_Level" integer NOT NULL,
	"Ps_MoneyValue" numeric(18,4) NOT NULL,
	"Ps_Moneyratio" integer NOT NULL,
	"Ps_Name" character varying(100) COLLATE pg_catalog."default",
	"Ps_PID" integer NOT NULL,
	 CONSTRAINT key_profitsharing PRIMARY KEY ("Ps_ID")
);
-- 表 ProfitSharing 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ProfitSharing_Ps_ID_seq";
ALTER SEQUENCE IF EXISTS public."ProfitSharing_Ps_ID_seq" OWNED BY public."ProfitSharing"."Ps_ID";
ALTER TABLE "ProfitSharing" ALTER COLUMN "Ps_ID" SET DEFAULT NEXTVAL('"ProfitSharing_Ps_ID_seq"'::regclass);

INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (1,0,0,'含自身，则为三级',True,True,1,0.0000,0,'二级分润',0);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (4,0,30,'',False,True,1,0.0000,40,'',1);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (9,0,20,'',False,True,7,0.0000,18,'',1);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (13,0,0,'含自身，则为四级',True,False,2,0.0000,0,'三级分润',0);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (14,0,0,'只有直接下线购买课程有提成',True,True,0,0.0000,0,'一级分润',0);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (15,0,50,'',False,True,1,0.0000,35,'',14);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (17,0,30,'',False,True,0,0.0000,35,'',13);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (18,0,30,'',False,True,2,0.0000,20,'',13);INSERT INTO "ProfitSharing"("Ps_ID","Ps_CouponValue","Ps_Couponratio","Ps_Intro","Ps_IsTheme","Ps_IsUse","Ps_Level","Ps_MoneyValue","Ps_Moneyratio","Ps_Name","Ps_PID") VALUES (20,0,20,'',False,True,1,0.0000,5,'',13);

-- 创建表 Purview --
CREATE TABLE IF NOT EXISTS public."Purview"
(
	"Pur_Id" integer NOT NULL,
	"Dep_Id" integer NOT NULL,
	"EGrp_Id" integer NOT NULL,
	"MM_UID" character varying(50) COLLATE pg_catalog."default",
	"Olv_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Posi_Id" integer NOT NULL,
	"Pur_State" character varying(50) COLLATE pg_catalog."default",
	"Pur_Type" character varying(50) COLLATE pg_catalog."default",
	 CONSTRAINT key_purview PRIMARY KEY ("Pur_Id")
);
-- 表 Purview 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Purview_Pur_Id_seq";
ALTER SEQUENCE IF EXISTS public."Purview_Pur_Id_seq" OWNED BY public."Purview"."Pur_Id";
ALTER TABLE "Purview" ALTER COLUMN "Pur_Id" SET DEFAULT NEXTVAL('"Purview_Pur_Id_seq"'::regclass);

INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3154,0,0,'a2d8c81ec24efe439b4c9b2d139e99fe',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3155,0,0,'f9b11e7920f6ab15ead04eeafb511830',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3156,0,0,'8546016f8e1c6e078b5dddd0eab7920d',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3157,0,0,'b50ea4a3ed65be9d39651c1f1ecf014c',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3158,0,0,'1708482766944',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3159,0,0,'53774f25cbb2a6248bdc4b5783d1f842',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3160,0,0,'19c839b6968161696712b7e7b76c9772',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3161,0,0,'651397af8465c643284ff8e137fd8079',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3162,0,0,'1639658295720',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3163,0,0,'606b87e461d6b43e1ff789ad9b1b11c2',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3164,0,0,'f2b59e41fb0d29f16707ad11b590e686',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3165,0,0,'1697428790833',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3166,0,0,'1697428791888',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3167,0,0,'1697428808320',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3168,0,0,'1697791615582',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3169,0,0,'7505573f225da91c421b31e8e950aa16',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3170,0,0,'fdffc2a7aa807909b9c259169c70794d',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3171,0,0,'e3346bd15202ce654c42f126d2153a41',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3172,0,0,'1697789410970',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3221,0,0,'a2d8c81ec24efe439b4c9b2d139e99fe',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3222,0,0,'f9b11e7920f6ab15ead04eeafb511830',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3223,0,0,'8546016f8e1c6e078b5dddd0eab7920d',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3224,0,0,'b50ea4a3ed65be9d39651c1f1ecf014c',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3225,0,0,'1708482766944',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3226,0,0,'53774f25cbb2a6248bdc4b5783d1f842',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3227,0,0,'19c839b6968161696712b7e7b76c9772',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3228,0,0,'651397af8465c643284ff8e137fd8079',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3229,0,0,'1639658295720',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3230,0,0,'606b87e461d6b43e1ff789ad9b1b11c2',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3231,0,0,'f2b59e41fb0d29f16707ad11b590e686',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3232,0,0,'1697428790833',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3233,0,0,'1697428791888',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3234,0,0,'1697428808320',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3235,0,0,'1697791615582',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3236,0,0,'7505573f225da91c421b31e8e950aa16',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3237,0,0,'fdffc2a7aa807909b9c259169c70794d',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3238,0,0,'e3346bd15202ce654c42f126d2153a41',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3239,0,0,'1697789410970',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3240,0,0,'1697789613476',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3241,0,0,'1697791007226',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3242,0,0,'5f81b3a13ce40cdc0525d3346bcdc682',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3243,0,0,'1697790883366',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3244,0,0,'1697791356432',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3245,0,0,'1705402993842',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3246,0,0,'1705403019261',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3247,0,0,'fc60823be11ec1b67cbc8865085928ca',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3248,0,0,'1697103541077',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3249,0,0,'5f8650559e67d7aee0865ab46abc57e5',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3250,0,0,'5f3a00f6661e44c530939cb7ad74845f',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3251,0,0,'1639834362028',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3252,0,0,'1695313545347',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3253,0,0,'1695313546693',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3254,0,0,'1695313569415',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3255,0,0,'4be6e84aeacaec7514680b72499b7c19',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3256,0,0,'185d53f8d69610c63281766012d17a8d',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3257,0,0,'e99f9b903ccc0bdefdbca97abcc9f4b1',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3258,0,0,'df3455c4a980c841604b55dc6651a92f',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3259,0,0,'9bbdcbde47d569e6a9d5c59a8947a445',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3260,0,0,'5469dba2b4b8d54745500eea8c1ba089',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3261,0,0,'c68451aaa777687e559756c9f02f68d3',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3262,0,0,'8a539ecff79b6ede1b38b2a8380e86cd',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3263,0,0,'1697103633401',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3264,0,0,'99e3c10a6ff4c0af38d4ad6551662222',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3265,0,0,'0df6f2c3642c081462a35f1f1ada550a',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3266,0,0,'caedac420273252de2dadfd450a98382',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3267,0,0,'1642663352817',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3268,0,0,'1641613171019',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3269,0,0,'7c5f1c92ee9e6c364a46c755df860b26',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3270,0,0,'22475af5e44f46286660708fb4f2c4c9',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3271,0,0,'1641735504763',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3173,0,0,'1697789613476',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3174,0,0,'1697791007226',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3272,0,0,'501e00e137aebb030d30b8de30edec06',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3175,0,0,'5f81b3a13ce40cdc0525d3346bcdc682',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3176,0,0,'1697790883366',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3177,0,0,'1697791356432',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3178,0,0,'1705402993842',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3179,0,0,'1705403019261',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3180,0,0,'fc60823be11ec1b67cbc8865085928ca',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3181,0,0,'1697103541077',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3182,0,0,'5f8650559e67d7aee0865ab46abc57e5',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3183,0,0,'5f3a00f6661e44c530939cb7ad74845f',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3184,0,0,'1639834362028',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3185,0,0,'1695313545347',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3186,0,0,'1695313546693',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3187,0,0,'1695313569415',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3188,0,0,'4be6e84aeacaec7514680b72499b7c19',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3189,0,0,'185d53f8d69610c63281766012d17a8d',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3190,0,0,'e99f9b903ccc0bdefdbca97abcc9f4b1',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3191,0,0,'df3455c4a980c841604b55dc6651a92f',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3192,0,0,'9bbdcbde47d569e6a9d5c59a8947a445',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3193,0,0,'5469dba2b4b8d54745500eea8c1ba089',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3194,0,0,'c68451aaa777687e559756c9f02f68d3',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3195,0,0,'8a539ecff79b6ede1b38b2a8380e86cd',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3196,0,0,'1697103633401',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3197,0,0,'99e3c10a6ff4c0af38d4ad6551662222',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3198,0,0,'0df6f2c3642c081462a35f1f1ada550a',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3199,0,0,'caedac420273252de2dadfd450a98382',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3200,0,0,'1642663352817',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3201,0,0,'1641613171019',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3202,0,0,'7c5f1c92ee9e6c364a46c755df860b26',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3203,0,0,'22475af5e44f46286660708fb4f2c4c9',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3204,0,0,'1641735504763',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3205,0,0,'501e00e137aebb030d30b8de30edec06',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3206,0,0,'1704957895680',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3207,0,0,'ca6c8e9988678ea4bc089a98d64dbe43',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3208,0,0,'02d9f63ce76365a7d986e0b0a0ea70e4',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3209,0,0,'b632ee17275095c13cfb8055129c59cd',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3210,0,0,'17ed5191fd4a3b9d3fc366a1cda5b4dc',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3211,0,0,'3ed08ea8c3a6dbbb0b14535e27357061',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3212,0,0,'3a108c8fbb70ddb57532149a214bc427',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3213,0,0,'1641640249725',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3214,0,0,'82809d1a369ab3c44c330c909a532866',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3215,0,0,'83f7721be4b7779ce3f097a59b81adb9',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3216,0,0,'6b83ad54dc5319393f4eaf23b6ae14c8',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3217,0,0,'cc6e884e86541560bddc33e539cfdbc7',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3218,0,0,'e32ff65ff0db40c9d569a80d95550c25',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3219,0,0,'1673081413592',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3220,0,0,'b486c9a4eca4cc594585bd6639e281fe',1,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3273,0,0,'ca6c8e9988678ea4bc089a98d64dbe43',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3274,0,0,'02d9f63ce76365a7d986e0b0a0ea70e4',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3275,0,0,'b632ee17275095c13cfb8055129c59cd',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3276,0,0,'17ed5191fd4a3b9d3fc366a1cda5b4dc',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3277,0,0,'3ed08ea8c3a6dbbb0b14535e27357061',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3278,0,0,'3a108c8fbb70ddb57532149a214bc427',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3279,0,0,'1641640249725',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3280,0,0,'82809d1a369ab3c44c330c909a532866',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3281,0,0,'83f7721be4b7779ce3f097a59b81adb9',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3282,0,0,'6b83ad54dc5319393f4eaf23b6ae14c8',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3283,0,0,'cc6e884e86541560bddc33e539cfdbc7',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3284,0,0,'e32ff65ff0db40c9d569a80d95550c25',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3285,0,0,'1673081413592',5,0,0,'','orglevel');INSERT INTO "Purview"("Pur_Id","Dep_Id","EGrp_Id","MM_UID","Olv_ID","Org_ID","Posi_Id","Pur_State","Pur_Type") VALUES (3286,0,0,'b486c9a4eca4cc594585bd6639e281fe',5,0,0,'','orglevel');
-- 表 Purview 的索引 --
CREATE INDEX IF NOT EXISTS "Purview_IX_Olv_ID" ON public."Purview" USING btree ("Olv_ID" DESC);
CREATE UNIQUE INDEX IF NOT EXISTS "Purview_aaaaaPurview_PK" ON public."Purview" USING btree ("Pur_Id");

-- 创建表 QuesAnswer --
CREATE TABLE IF NOT EXISTS public."QuesAnswer"
(
	"Ans_Context" text,
	"Ans_ID" bigint NOT NULL,
	"Ans_IsCorrect" boolean NOT NULL,
	"Qus_ID" bigint NOT NULL,
	"Qus_UID" character varying(255) COLLATE pg_catalog."default"
);


-- 创建表 QuesTypes --
CREATE TABLE IF NOT EXISTS public."QuesTypes"
(
	"Qt_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	"Qt_Count" integer NOT NULL,
	"Qt_Intro" text,
	"Qt_IsUse" boolean NOT NULL,
	"Qt_Name" character varying(300) COLLATE pg_catalog."default",
	"Qt_Tax" integer NOT NULL,
	"Qt_Type" integer NOT NULL,
	"Qt_TypeName" character varying(100) COLLATE pg_catalog."default",
	 CONSTRAINT key_questypes PRIMARY KEY ("Qt_ID")
);
-- 表 QuesTypes 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."QuesTypes_Qt_ID_seq";
ALTER SEQUENCE IF EXISTS public."QuesTypes_Qt_ID_seq" OWNED BY public."QuesTypes"."Qt_ID";
ALTER TABLE "QuesTypes" ALTER COLUMN "Qt_ID" SET DEFAULT NEXTVAL('"QuesTypes_Qt_ID_seq"'::regclass);



-- 创建表 Questions --
CREATE TABLE IF NOT EXISTS public."Questions"
(
	"Qus_ID" bigint NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Kn_Uid" character varying(255) COLLATE pg_catalog."default",
	"Ol_ID" bigint NOT NULL,
	"Ol_Name" character varying(500) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Qt_ID" integer NOT NULL,
	"Qus_Answer" text,
	"Qus_CrtTime" timestamp without time zone NOT NULL,
	"Qus_Diff" integer NOT NULL,
	"Qus_ErrorInfo" character varying(255) COLLATE pg_catalog."default",
	"Qus_Errornum" integer NOT NULL,
	"Qus_Explain" text,
	"Qus_IsCorrect" boolean NOT NULL,
	"Qus_IsError" boolean NOT NULL,
	"Qus_IsTitle" boolean NOT NULL,
	"Qus_IsUse" boolean NOT NULL,
	"Qus_IsWrong" boolean NOT NULL,
	"Qus_Items" text,
	"Qus_LastTime" timestamp without time zone NOT NULL,
	"Qus_Number" real NOT NULL,
	"Qus_Tax" integer NOT NULL,
	"Qus_Title" text,
	"Qus_Type" integer NOT NULL,
	"Qus_UID" character varying(255) COLLATE pg_catalog."default",
	"Qus_WrongInfo" text,
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(500) COLLATE pg_catalog."default",
	 CONSTRAINT key_questions PRIMARY KEY ("Qus_ID")
);

-- 表 Questions 的索引 --
CREATE INDEX IF NOT EXISTS "Questions_IX_Cou_ID" ON public."Questions" USING btree ("Cou_ID");
CREATE INDEX IF NOT EXISTS "Questions_IX_Ol_ID" ON public."Questions" USING btree ("Ol_ID");
CREATE INDEX IF NOT EXISTS "Questions_IX_Org_ID" ON public."Questions" USING btree ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Diff" ON public."Questions" USING btree ("Qus_Diff" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_ID" ON public."Questions" USING btree ("Qus_ID");
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Type" ON public."Questions" USING btree ("Qus_Type" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Sbj_ID" ON public."Questions" USING btree ("Sbj_ID" DESC);

-- 创建表 RechargeCode --
CREATE TABLE IF NOT EXISTS public."RechargeCode"
(
	"Rc_ID" integer NOT NULL,
	"Ac_AccName" character varying(50) COLLATE pg_catalog."default",
	"Ac_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Rc_Code" character varying(100) COLLATE pg_catalog."default",
	"Rc_CrtTime" timestamp without time zone NOT NULL,
	"Rc_IsEnable" boolean NOT NULL,
	"Rc_IsUsed" boolean NOT NULL,
	"Rc_LimitEnd" timestamp without time zone NOT NULL,
	"Rc_LimitStart" timestamp without time zone NOT NULL,
	"Rc_Price" integer NOT NULL,
	"Rc_Pw" character varying(20) COLLATE pg_catalog."default",
	"Rc_QrcodeBase64" text,
	"Rc_Type" integer NOT NULL,
	"Rc_UsedTime" timestamp without time zone NOT NULL,
	"Rs_ID" integer NOT NULL,
	 CONSTRAINT key_rechargecode PRIMARY KEY ("Rc_ID")
);
-- 表 RechargeCode 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."RechargeCode_Rc_ID_seq";
ALTER SEQUENCE IF EXISTS public."RechargeCode_Rc_ID_seq" OWNED BY public."RechargeCode"."Rc_ID";
ALTER TABLE "RechargeCode" ALTER COLUMN "Rc_ID" SET DEFAULT NEXTVAL('"RechargeCode_Rc_ID_seq"'::regclass);



-- 创建表 RechargeSet --
CREATE TABLE IF NOT EXISTS public."RechargeSet"
(
	"Rs_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Rs_CodeLength" integer NOT NULL,
	"Rs_Count" integer NOT NULL,
	"Rs_CrtTime" timestamp without time zone NOT NULL,
	"Rs_Intro" character varying(1000) COLLATE pg_catalog."default",
	"Rs_IsEnable" boolean NOT NULL,
	"Rs_LimitEnd" timestamp without time zone NOT NULL,
	"Rs_LimitStart" timestamp without time zone NOT NULL,
	"Rs_Price" integer NOT NULL,
	"Rs_Pw" character varying(100) COLLATE pg_catalog."default",
	"Rs_PwLength" integer NOT NULL,
	"Rs_Theme" character varying(200) COLLATE pg_catalog."default",
	"Rs_UsedCount" integer NOT NULL,
	 CONSTRAINT key_rechargeset PRIMARY KEY ("Rs_ID")
);
-- 表 RechargeSet 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."RechargeSet_Rs_ID_seq";
ALTER SEQUENCE IF EXISTS public."RechargeSet_Rs_ID_seq" OWNED BY public."RechargeSet"."Rs_ID";
ALTER TABLE "RechargeSet" ALTER COLUMN "Rs_ID" SET DEFAULT NEXTVAL('"RechargeSet_Rs_ID_seq"'::regclass);



-- 创建表 ShowPicture --
CREATE TABLE IF NOT EXISTS public."ShowPicture"
(
	"Shp_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Shp_BgColor" character varying(50) COLLATE pg_catalog."default",
	"Shp_File" character varying(100) COLLATE pg_catalog."default",
	"Shp_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Shp_IsShow" boolean NOT NULL,
	"Shp_Site" character varying(50) COLLATE pg_catalog."default",
	"Shp_Target" character varying(100) COLLATE pg_catalog."default",
	"Shp_Tax" integer NOT NULL,
	"Shp_Url" character varying(500) COLLATE pg_catalog."default",
	 CONSTRAINT key_showpicture PRIMARY KEY ("Shp_ID")
);
-- 表 ShowPicture 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ShowPicture_Shp_ID_seq";
ALTER SEQUENCE IF EXISTS public."ShowPicture_Shp_ID_seq" OWNED BY public."ShowPicture"."Shp_ID";
ALTER TABLE "ShowPicture" ALTER COLUMN "Shp_ID" SET DEFAULT NEXTVAL('"ShowPicture_Shp_ID_seq"'::regclass);



-- 创建表 SingleSignOn --
CREATE TABLE IF NOT EXISTS public."SingleSignOn"
(
	"SSO_ID" integer NOT NULL,
	"SSO_APPID" character varying(500) NOT NULL COLLATE pg_catalog."default",
	"SSO_Config" text,
	"SSO_CrtTime" timestamp without time zone NOT NULL,
	"SSO_Direction" character varying(50) COLLATE pg_catalog."default",
	"SSO_Domain" character varying(500) COLLATE pg_catalog."default",
	"SSO_Email" character varying(50) COLLATE pg_catalog."default",
	"SSO_Info" character varying(500) COLLATE pg_catalog."default",
	"SSO_IsAdd" boolean NOT NULL,
	"SSO_IsAddSort" boolean NOT NULL,
	"SSO_IsUse" boolean NOT NULL,
	"SSO_Name" character varying(100) COLLATE pg_catalog."default",
	"SSO_Phone" character varying(50) COLLATE pg_catalog."default",
	"SSO_Power" character varying(50) COLLATE pg_catalog."default",
	 CONSTRAINT key_singlesignon PRIMARY KEY ("SSO_ID")
);
-- 表 SingleSignOn 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."SingleSignOn_SSO_ID_seq";
ALTER SEQUENCE IF EXISTS public."SingleSignOn_SSO_ID_seq" OWNED BY public."SingleSignOn"."SSO_ID";
ALTER TABLE "SingleSignOn" ALTER COLUMN "SSO_ID" SET DEFAULT NEXTVAL('"SingleSignOn_SSO_ID_seq"'::regclass);



-- 创建表 SmsFault --
CREATE TABLE IF NOT EXISTS public."SmsFault"
(
	"Smf_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Smf_Company" character varying(255) COLLATE pg_catalog."default",
	"Smf_Context" character varying(255) COLLATE pg_catalog."default",
	"Smf_CrtTime" timestamp without time zone,
	"Smf_MobileTel" character varying(255) COLLATE pg_catalog."default",
	"Smf_SendName" character varying(255) COLLATE pg_catalog."default",
	"Smf_SendTime" timestamp without time zone,
	 CONSTRAINT key_smsfault PRIMARY KEY ("Smf_Id")
);
-- 表 SmsFault 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."SmsFault_Smf_Id_seq";
ALTER SEQUENCE IF EXISTS public."SmsFault_Smf_Id_seq" OWNED BY public."SmsFault"."Smf_Id";
ALTER TABLE "SmsFault" ALTER COLUMN "Smf_Id" SET DEFAULT NEXTVAL('"SmsFault_Smf_Id_seq"'::regclass);


-- 表 SmsFault 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "SmsFault_aaaaaSmsFault_PK" ON public."SmsFault" USING btree ("Smf_Id");

-- 创建表 SmsMessage --
CREATE TABLE IF NOT EXISTS public."SmsMessage"
(
	"SMS_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sms_Context" character varying(255) COLLATE pg_catalog."default",
	"Sms_CrtTime" timestamp without time zone,
	"Sms_MailBox" integer NOT NULL,
	"Sms_SendId" integer NOT NULL,
	"Sms_SendName" character varying(255) COLLATE pg_catalog."default",
	"Sms_SendTime" timestamp without time zone,
	"Sms_State" integer NOT NULL,
	"Sms_Type" integer NOT NULL,
	 CONSTRAINT key_smsmessage PRIMARY KEY ("SMS_Id")
);
-- 表 SmsMessage 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."SmsMessage_SMS_Id_seq";
ALTER SEQUENCE IF EXISTS public."SmsMessage_SMS_Id_seq" OWNED BY public."SmsMessage"."SMS_Id";
ALTER TABLE "SmsMessage" ALTER COLUMN "SMS_Id" SET DEFAULT NEXTVAL('"SmsMessage_SMS_Id_seq"'::regclass);


-- 表 SmsMessage 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "SmsMessage_aaaaaSmsMessage_PK" ON public."SmsMessage" USING btree ("SMS_Id");

-- 创建表 Special --
CREATE TABLE IF NOT EXISTS public."Special"
(
	"Sp_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"OtherData" text,
	"Sp_Banner" character varying(255) COLLATE pg_catalog."default",
	"Sp_Descr" character varying(255) COLLATE pg_catalog."default",
	"Sp_Details" text,
	"Sp_Intro" text,
	"Sp_IsOut" boolean NOT NULL,
	"Sp_IsShow" boolean NOT NULL,
	"Sp_IsUse" boolean NOT NULL,
	"Sp_Keywords" character varying(255) COLLATE pg_catalog."default",
	"Sp_Label" character varying(255) COLLATE pg_catalog."default",
	"Sp_Logo" character varying(255) COLLATE pg_catalog."default",
	"Sp_Name" character varying(255) COLLATE pg_catalog."default",
	"Sp_OutUrl" character varying(255) COLLATE pg_catalog."default",
	"Sp_PatId" integer NOT NULL,
	"Sp_PushTime" timestamp without time zone,
	"Sp_QrCode" character varying(255) COLLATE pg_catalog."default",
	"Sp_Tax" integer NOT NULL,
	"Sp_Tootip" character varying(255) COLLATE pg_catalog."default",
	"Sp_Uid" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_special PRIMARY KEY ("Sp_Id")
);
-- 表 Special 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Special_Sp_Id_seq";
ALTER SEQUENCE IF EXISTS public."Special_Sp_Id_seq" OWNED BY public."Special"."Sp_Id";
ALTER TABLE "Special" ALTER COLUMN "Sp_Id" SET DEFAULT NEXTVAL('"Special_Sp_Id_seq"'::regclass);


-- 表 Special 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Special_aaaaaSpecial_PK" ON public."Special" USING btree ("Sp_Id");

-- 创建表 Special_Article --
CREATE TABLE IF NOT EXISTS public."Special_Article"
(
	"Spa_Id" integer NOT NULL,
	"Art_Id" bigint NOT NULL,
	"Org_Id" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sp_Id" integer NOT NULL,
	 CONSTRAINT key_special_article PRIMARY KEY ("Spa_Id")
);
-- 表 Special_Article 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Special_Article_Spa_Id_seq";
ALTER SEQUENCE IF EXISTS public."Special_Article_Spa_Id_seq" OWNED BY public."Special_Article"."Spa_Id";
ALTER TABLE "Special_Article" ALTER COLUMN "Spa_Id" SET DEFAULT NEXTVAL('"Special_Article_Spa_Id_seq"'::regclass);


-- 表 Special_Article 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "Special_Article_aaaaaSpecial_Article_PK" ON public."Special_Article" USING btree ("Spa_Id");

-- 创建表 StudentSort --
CREATE TABLE IF NOT EXISTS public."StudentSort"
(
	"Sts_ID" bigint NOT NULL,
	"Dep_CnName" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) NOT NULL COLLATE pg_catalog."default",
	"Sts_Count" integer NOT NULL,
	"Sts_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Sts_IsDefault" boolean NOT NULL,
	"Sts_IsUse" boolean NOT NULL,
	"Sts_Name" character varying(255) COLLATE pg_catalog."default",
	"Sts_SwitchPlay" boolean NOT NULL,
	"Sts_Tax" integer NOT NULL,
	 CONSTRAINT key_studentsort PRIMARY KEY ("Sts_ID")
);
INSERT INTO "StudentSort"("Sts_ID","Dep_CnName","Dep_Id","Org_ID","Org_Name","Sts_Count","Sts_Intro","Sts_IsDefault","Sts_IsUse","Sts_Name","Sts_SwitchPlay","Sts_Tax") VALUES (15012714616000001,'',0,4,'郑州微厦计算机科技有限公司',2,'',True,True,'默认组d',False,2);

-- 创建表 StudentSort_Course --
CREATE TABLE IF NOT EXISTS public."StudentSort_Course"
(
	"Ssc_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Ssc_EndTime" timestamp without time zone NOT NULL,
	"Ssc_IsEnable" boolean NOT NULL,
	"Ssc_StartTime" timestamp without time zone NOT NULL,
	"Sts_ID" bigint NOT NULL,
	 CONSTRAINT key_studentsort_course PRIMARY KEY ("Ssc_ID")
);
-- 表 StudentSort_Course 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."StudentSort_Course_Ssc_ID_seq";
ALTER SEQUENCE IF EXISTS public."StudentSort_Course_Ssc_ID_seq" OWNED BY public."StudentSort_Course"."Ssc_ID";
ALTER TABLE "StudentSort_Course" ALTER COLUMN "Ssc_ID" SET DEFAULT NEXTVAL('"StudentSort_Course_Ssc_ID_seq"'::regclass);



-- 创建表 Student_Collect --
CREATE TABLE IF NOT EXISTS public."Student_Collect"
(
	"Stc_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Qus_Diff" integer NOT NULL,
	"Qus_ID" bigint NOT NULL,
	"Qus_Title" text,
	"Qus_Type" integer NOT NULL,
	"Sbj_ID" bigint NOT NULL,
	"Stc_CrtTime" timestamp without time zone NOT NULL,
	"Stc_Level" integer NOT NULL,
	"Stc_Strange" integer NOT NULL,
	 CONSTRAINT key_student_collect PRIMARY KEY ("Stc_ID")
);
-- 表 Student_Collect 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Student_Collect_Stc_ID_seq";
ALTER SEQUENCE IF EXISTS public."Student_Collect_Stc_ID_seq" OWNED BY public."Student_Collect"."Stc_ID";
ALTER TABLE "Student_Collect" ALTER COLUMN "Stc_ID" SET DEFAULT NEXTVAL('"Student_Collect_Stc_ID_seq"'::regclass);


-- 表 Student_Collect 的索引 --
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Ac_ID" ON public."Student_Collect" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Cou_ID" ON public."Student_Collect" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Qus_ID" ON public."Student_Collect" USING btree ("Qus_ID" DESC);

-- 创建表 Student_Course --
CREATE TABLE IF NOT EXISTS public."Student_Course"
(
	"Stc_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Lc_Code" character varying(100) COLLATE pg_catalog."default",
	"Lc_Pw" character varying(50) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Rc_Code" character varying(100) COLLATE pg_catalog."default",
	"Stc_Coupon" integer NOT NULL,
	"Stc_CrtTime" timestamp without time zone NOT NULL,
	"Stc_EndTime" timestamp without time zone NOT NULL,
	"Stc_ExamScore" real NOT NULL,
	"Stc_IsEnable" boolean NOT NULL,
	"Stc_IsFree" boolean NOT NULL,
	"Stc_IsTry" boolean NOT NULL,
	"Stc_Money" numeric(18,4) NOT NULL,
	"Stc_QuesScore" real NOT NULL,
	"Stc_ResultScore" real NOT NULL,
	"Stc_StartTime" timestamp without time zone NOT NULL,
	"Stc_StudyScore" real NOT NULL,
	"Stc_Type" integer NOT NULL,
	"Sts_ID" bigint NOT NULL,
	 CONSTRAINT key_student_course PRIMARY KEY ("Stc_ID")
);
-- 表 Student_Course 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Student_Course_Stc_ID_seq";
ALTER SEQUENCE IF EXISTS public."Student_Course_Stc_ID_seq" OWNED BY public."Student_Course"."Stc_ID";
ALTER TABLE "Student_Course" ALTER COLUMN "Stc_ID" SET DEFAULT NEXTVAL('"Student_Course_Stc_ID_seq"'::regclass);


-- 表 Student_Course 的索引 --
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Ac_ID" ON public."Student_Course" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Cou_ID" ON public."Student_Course" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Org_ID" ON public."Student_Course" USING btree ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_EndTime" ON public."Student_Course" USING btree ("Stc_EndTime" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_StartTime" ON public."Student_Course" USING btree ("Stc_StartTime" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_Type" ON public."Student_Course" USING btree ("Stc_Type" DESC);

-- 创建表 Student_Notes --
CREATE TABLE IF NOT EXISTS public."Student_Notes"
(
	"Stn_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Qus_ID" bigint NOT NULL,
	"Qus_Title" text,
	"Qus_Type" integer NOT NULL,
	"Stn_Context" character varying(1000) COLLATE pg_catalog."default",
	"Stn_CrtTime" timestamp without time zone NOT NULL,
	"Stn_PID" integer NOT NULL,
	"Stn_Title" character varying(100) COLLATE pg_catalog."default",
	 CONSTRAINT key_student_notes PRIMARY KEY ("Stn_ID")
);
-- 表 Student_Notes 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Student_Notes_Stn_ID_seq";
ALTER SEQUENCE IF EXISTS public."Student_Notes_Stn_ID_seq" OWNED BY public."Student_Notes"."Stn_ID";
ALTER TABLE "Student_Notes" ALTER COLUMN "Stn_ID" SET DEFAULT NEXTVAL('"Student_Notes_Stn_ID_seq"'::regclass);


-- 表 Student_Notes 的索引 --
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Ac_ID" ON public."Student_Notes" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Cou_ID" ON public."Student_Notes" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Qus_ID" ON public."Student_Notes" USING btree ("Qus_ID" DESC);

-- 创建表 Student_Ques --
CREATE TABLE IF NOT EXISTS public."Student_Ques"
(
	"Squs_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Qus_Diff" integer NOT NULL,
	"Qus_ID" bigint NOT NULL,
	"Qus_Type" integer NOT NULL,
	"Sbj_ID" bigint NOT NULL,
	"Squs_CrtTime" timestamp without time zone NOT NULL,
	"Squs_Level" integer NOT NULL,
	 CONSTRAINT key_student_ques PRIMARY KEY ("Squs_ID")
);
-- 表 Student_Ques 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Student_Ques_Squs_ID_seq";
ALTER SEQUENCE IF EXISTS public."Student_Ques_Squs_ID_seq" OWNED BY public."Student_Ques"."Squs_ID";
ALTER TABLE "Student_Ques" ALTER COLUMN "Squs_ID" SET DEFAULT NEXTVAL('"Student_Ques_Squs_ID_seq"'::regclass);


-- 表 Student_Ques 的索引 --
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Ac_ID" ON public."Student_Ques" USING btree ("Ac_ID");
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Cou_ID" ON public."Student_Ques" USING btree ("Cou_ID");
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_Diff" ON public."Student_Ques" USING btree ("Qus_Diff" DESC);
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_ID" ON public."Student_Ques" USING btree ("Qus_ID");
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_Type" ON public."Student_Ques" USING btree ("Qus_Type" DESC);

-- 创建表 Subject --
CREATE TABLE IF NOT EXISTS public."Subject"
(
	"Sbj_ID" bigint NOT NULL,
	"Dep_CnName" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_ByName" character varying(255) COLLATE pg_catalog."default",
	"Sbj_CouNumber" integer NOT NULL,
	"Sbj_CrtTime" timestamp without time zone NOT NULL,
	"Sbj_Details" text,
	"Sbj_Intro" text,
	"Sbj_IsRec" boolean NOT NULL,
	"Sbj_IsUse" boolean NOT NULL,
	"Sbj_Level" integer NOT NULL,
	"Sbj_Logo" character varying(100) COLLATE pg_catalog."default",
	"Sbj_LogoSmall" character varying(100) COLLATE pg_catalog."default",
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_PID" bigint NOT NULL,
	"Sbj_PassScore" integer NOT NULL,
	"Sbj_Tax" integer NOT NULL,
	"Sbj_XPath" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_subject PRIMARY KEY ("Sbj_ID")
);


-- 创建表 SystemPara --
CREATE TABLE IF NOT EXISTS public."SystemPara"
(
	"Sys_Id" integer NOT NULL,
	"Org_Id" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sys_Default" character varying(255) COLLATE pg_catalog."default",
	"Sys_Key" character varying(255) COLLATE pg_catalog."default",
	"Sys_ParaIntro" character varying(255) COLLATE pg_catalog."default",
	"Sys_SelectUnit" character varying(255) COLLATE pg_catalog."default",
	"Sys_Unit" character varying(255) COLLATE pg_catalog."default",
	"Sys_Value" text,
	 CONSTRAINT key_systempara PRIMARY KEY ("Sys_Id")
);
-- 表 SystemPara 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."SystemPara_Sys_Id_seq";
ALTER SEQUENCE IF EXISTS public."SystemPara_Sys_Id_seq" OWNED BY public."SystemPara"."Sys_Id";
ALTER TABLE "SystemPara" ALTER COLUMN "Sys_Id" SET DEFAULT NEXTVAL('"SystemPara_Sys_Id_seq"'::regclass);

INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (204,0,'','','Agreement_accounts','','','','<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
请务必认真阅读和理解本《用户服务协议》（以下简称《协议》）中规定的所有权利和限制。除非您接受本《协议》条款，否则您无权注册、登录或使用本协议所涉及的相关服务。您一旦注册、登录、使用或以任何方式使用本《协议》所涉及的相关服务的行为将视为对本《协议》的接受，即表示您同意接受本《协议》各项条款的约束。如果您不同意本《协议》中的条款，请不要注册、登录或使用本《协议》相关服务。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
一、服务内容</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. {platform}为{platform}网站（网址：{domain}，以下简称“{platform}”）的所有者及经营者，完全按照其发布的服务条款和操作规则提供基于互联网以及移动互联网的相关服务（以下简称“网络服务”）。{platform}网站网络服务的具体内容由{platform}根据实际情况提供。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. 您一旦注册成功成为用户，您将得到一个密码和账号，您需要对自己在账户中的所有活动和事件负全责。如果由于您的过失导致您的账号和密码脱离您的控制，则由此导致的针对您、{platform}或任何第三方造成的损害，您将承担全部责任。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. 用户应输入账号和密码登录{platform}账户。若您使用第三方登录使用我们的服务，在进行账号设置时，需要注册{platform}账户。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
4. 用户理解并接受，{platform}仅提供相关的网络服务，除此之外与相关网络服务有关的设备（如个人电脑、手机、及其他与接入互联网或移动互联网有关的装置）及所需的费用（如为接入互联网而支付的电话费及上网费、为使用移动网而支付的手机费）均应由用户自行负担。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
二、用户使用规则</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. 用户在申请使用{platform}网站网络服务时，必须向{platform}提供准确的个人资料，如个人资料有任何变动，必须及时更新。因用户提供个人资料不准确、不真实而引发的一切后果由用户承担。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. 用户不应将其账号、密码转让、出借或以任何脱离用户控制的形式交由他人使用。如用户发现其账号遭他人非法使用，应立即通知{platform}。因黑客行为或用户的保管疏忽导致账号、密码遭他人非法使用，{platform}不承担任何责任。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. 用户应当为自身注册账户下的一切行为负责，因用户行为而导致的用户自身或其他任何第三方的任何损失或损害，{platform}不承担责任。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
4. 用户理解并接受{platform}网站提供的服务中可能包括广告，同意在使用网络服务的过程中显示{platform}和第三方供应商、合作伙伴提供的广告。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
5. 用户在使用{platform}网络服务过程中，必须遵循以下原则：</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（1）遵守中国有关的法律和法规；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（2）遵守所有与网络服务有关的网络协议、规定和程序；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（3）不得为任何非法目的而使用网络服务系统；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（4） 不得利用{platform}网络服务系统进行任何可能对互联网或移动网正常运转造成不利影响的行为；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（5） 不得利用{platform}提供的网络服务上传、展示或传播任何虚假的、骚扰性的、中伤他人的、辱骂性的、恐吓性的、庸俗淫秽的或其他任何非法的信息资料；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（6）不得侵犯{platform}和其他任何第三方的专利权、著作权、商标权、名誉权或其他任何合法权益；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（7） 不得利用{platform}网络服务系统进行任何不利于{platform}的行为；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（8） 如发现任何非法使用用户账号或账号出现安全漏洞的情况，应立即通告{platform}。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
6. 如用户在使用网络服务时违反任何上述规定，{platform}或其授权的人有权要求用户改正或直接采取一切必要的措施（包括但不限于更改或删除用户收藏的内容等、暂停或终止用户使用网络服务的权利）以减轻用户不当行为造成的影响。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
三、服务变更、中断或终止</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. 鉴于网络服务的特殊性，用户同意{platform}有权根据业务发展情况随时变更、中断或终止部分或全部的网络服务而无需通知用户，也无需对任何用户或任何第三方承担任何责任；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. 用户理解，{platform}需要定期或不定期地对提供网络服务的平台（如互联网网站、移动网络等）或相关的设备进行检修或者维护，如因此类情况而造成网络服务在合理时间内的中断，{platform}无需为此承担任何责任，但{platform}应尽可能事先进行通告。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. 如发生下列任何一种情形，{platform}有权随时中断或终止向用户提供本《协议》项下的网络服务（包括收费网络服务）而无需对用户或任何第三方承担任何责任：</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（1）用户提供的个人资料不真实；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（2）用户违反本《协议》中规定的使用规则。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
四、知识产权</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. {platform}提供的网络服务中包含的任何文本、图片、图形、音频和/或视频资料均受版权、商标和/或其它财产所有权法律的保护，未经相关权利人同意，上述资料均不得用于任何商业目的。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. {platform}为提供网络服务而使用的任何软件（包括但不限于软件中所含的任何图像、照片、动画、录像、录音、音乐、文字和附加程序、随附的帮助材料）的一切权利均属于该软件的著作权人，未经该软件的著作权人许可，用户不得对该软件进行反向工程（reverse engineer）、反向编译（decompile）或反汇编（disassemble）。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
五、隐私保护</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. 保护用户隐私是{platform}的一项基本政策，{platform}保证不对外公开或向第三方提供单个用户的注册资料及用户在使用网络服务时存储在{platform}的非公开内容，但下列情况除外：</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（1）事先获得用户的明确授权；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（2）根据有关的法律法规要求；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（3）按照相关政府主管部门的要求；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（4）为维护社会公众的利益；</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
（5）为维护{platform}的合法权益。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. {platform}可能会与第三方合作向用户提供相关的网络服务，在此情况下，如该第三方同意承担与{platform}同等的保护用户隐私的责任，则{platform}有权将用户的注册资料等提供给该第三方。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. 在不透露单个用户隐私资料的前提下，{platform}有权对整个用户数据库进行分析并对用户数据库进行商业上的利用。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
六、免责声明</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. {platform}不担保网络服务一定能满足用户的要求，也不担保网络服务不会中断，对网络服务的及时性、安全性、准确性也都不作担保。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. {platform}不保证为向用户提供便利而设置的外部链接的准确性和完整性，同时，对于该等外部链接指向的不由{platform}实际控制的任何网页上的内容，{platform}不承担任何责任。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. 对于因电信系统或互联网网络故障、计算机故障或病毒、信息损坏或丢失、计算机系统问题或其它任何不可抗力原因而产生损失，{platform}不承担任何责任，但将尽力减少因此而给用户造成的损失和影响。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
七、法律及争议解决</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. 本协议适用中华人民共和国法律。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. 因本协议引起的或与本协议有关的任何争议，各方应友好协商解决；协商不成的，任何一方均可将有关争议提交至上海仲裁委员会并按照其届时有效的仲裁规则仲裁；仲裁裁决是终局的，对各方均有约束力。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
八、其他条款</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
1. 如果本协议中的任何条款无论因何种原因完全或部分无效或不具有执行力，或违反任何适用的法律，则该条款被视为删除，但本协议的其余条款仍应有效并且有约束力。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
2. {platform}有权随时根据有关法律、法规的变化以及公司经营状况和经营策略的调整等修改本协议，而无需另行单独通知用户。修改后的协议会在{platform}网站（{domain}）上公布。用户可随时通过{platform}网站浏览最新服务协议条款。当发生有关争议时，以最新的协议文本为准。如果不同意{platform}对本协议相关条款所做的修改，用户有权停止使用网络服务。如果用户继续使用网络服务，则视为用户接受{platform}对本协议相关条款所做的修改。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<span style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
3. {platform}在法律允许最大范围对本协议拥有解释权与修改权。</span>
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">
<br style="color: rgb(0, 0, 0); font-family: &quot;sans serif&quot;, tahoma, verdana, helvetica; font-size: 12px;">');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (205,0,'','','Agreement_teacher','','','','{platform}教师注册协议&nbsp;<br />
<br />
导言&nbsp;<br />
<br />
欢迎您使用{platform}提供的服务！&nbsp;<br />
<br />
为使用{platform}（网址：{domain}）提供的服务（以下简称：本服务），您应当阅读并遵守《{platform}教师注册协议》（以下简称：本协议）相关协议、规则。&nbsp;<br />
<br />
请您务必审慎阅读、充分理解各条款内容，特别是免除或者限制责任的条款，以及开通或使用某项服务的单独协议、规则。&nbsp;<br />
<br />
除非您已阅读并接受本协议及相关协议、规则等所有条款，否则，您无权使用{org}提供的本服务。您使用{org}的本服务，即视为您已阅读并同意上述协议、规则等的约束。&nbsp;<br />
<br />
您有违反本协议的任何行为时，{org}有权依照违反情况，随时单方限制、中止或终止向您提供本服务，并有权追究您的相关责任。&nbsp;<br />
<br />
1.术语含义&nbsp;<br />
<br />
如无特别说明，下列术语在本协议中的含义为：&nbsp;<br />
<br />
1.1 课程发布者：指经有效注册、申请后，将其享有相应权利的各种课程接入{platform}并向用户提供各种免费或收费类的在线直播、录播服务以实现与用户进行在线交流与学习目的的个人、法人或其他组织，因前述主体的不同，课程发布者在对外展现上也称“机构”或“老师”，本协议中简称为“您”。&nbsp;<br />
<br />
1.2 课程：指由课程发布者开发，或课程开发者经权利人授权，通过{platform}向相关用户提供各种免费或收费类的在线直播、录播形式的交流与学习服务，包括但不限于网络营销类、语言类、公务员考试类、小学/初中/高中教育辅导类（非学历教育）等现存的各种培训服务及今后可能出现的各种培训服务。课程是否收费、课程收费的数额及课程收费方式等均由课程发布者自行决定。&nbsp;<br />
<br />
1.3 {platform}：指由{org}所拥有、控制、经营的{org}其他平台或网站及前述各平台网站的下属子页面，以下简称为“{org}平台”、“{platform}”、“平台”。{org}通过{platform}向您提供的服务包括但不限于提供课程运营平台、费用支付服务、广点通推广服务等服务，具体以{org}提供和您选择的服务为准，前述服务称为{platform}服务、平台服务、本服务。&nbsp;<br />
<br />
{org}、课程发布者均同意和理解：&nbsp;<br />
<br />
（1）{platform}是一个中立的平台服务提供者，仅向课程发布者提供信息存储空间、链接等中立的网络服务或相关中立的技术支持服务，以供课程发布者在中立的平台上自主发布、运营、推广其课程等；&nbsp;<br />
<br />
（2）课程发布者的课程由课程发布者自主开发、独立运营并独立承担全部责任。{org}不会、也不可能参与课程发布者课程的研发、运营等任何活动，{org}也不会对课程发布者的课程进行任何的修改、编辑或整理等；&nbsp;<br />
<br />
（3）因课程发布者课程及服务产生的任何纠纷、责任等，以及开发者违反相关法律法规或本协议约定引发的任何后果，均由课程发布者独立承担责任、赔偿损失，与{org}无关。如侵害到{org}或他人权益的，课程发布者须自行承担全部责任和赔偿一切损失。&nbsp;<br />
<br />
1.4 {platform}运营数据：是指用户、课程发布者在使用{platform}相关服务中产生的相关运营数据，包括但不限于用户或课程发布者操作行为形成的数据、各类交易数据等，“{platform}数据”、“平台数据、“运营数据”。“{platform}运营数据”的所有权及其他相关权利属于{org}，且是{org}的商业秘密，未经{org}书面授权您不得使用。但依法属于用户、课程发布者享有相关权利的数据或课程内容等除外。&nbsp;<br />
<br />
2.课程发布者的权利和义务&nbsp;<br />
<br />
2.1 帐户注册&nbsp;<br />
<br />
2.1.1 您应当通过登录{platform}网站或{org}其他指定途径，注册课程发布者帐户（下简称：账户）以成为课程发布者，课程发布者帐户一经注册成功，相应的账号不得变更，且该帐户不可转让、不可赠予、不可继承等。&nbsp;<br />
<br />
2.1.2您注册帐号时，应使用您拥有合法使用权的手机号码。&nbsp;<br />
<br />
2.1.3 您不得违反本协议约定将您的账户用于其他目的。否则，{org}有权随时单方限制、中止或终止向您提供本服务，且未经{org}同意您不得再次使用本服务。&nbsp;<br />
<br />
2.1.4 您注册帐号使用的手机号，是您登录及使用本服务的凭证。您应当做好手机号、密码，以及进入和管理本服务中的各类产品与服务的口令、密码等的保密措施。因您保密措施不当或您的其他行为，致使上述口令、密码等丢失或泄漏所引起的一切损失和后果，均由您自行承担。&nbsp;<br />
<br />
2.1.5 {org}可能会向您提供添加其他QQ账号以成为您注册账号的账号管理员的功能，如{org}向您提供账号管理员功能后，您可以根据自己的需求，在使用相应手机号注册成您账号的账号管理员后，进行您指定或授权的操作。&nbsp;<br />
<br />
2.1.6 您保证：您注册本服务账户的手机号及您添加的账号管理员使用的手机号的使用权均是合法获取的。前述全部手机号在本服务中进行的包括但不限于以下事项：注册本服务帐户、提交相应资质材料、确认和同意相关协议和规则、选择具体服务类别以及进行费用结算等事项，均是您自行或您授权他人进行的行为，对您均有约束力。同时，您承担以前述全部手机号为标识进行的全部行为的法律责任。&nbsp;<br />
<br />
2.1.7 若您发现有他人冒用或盗用您的账户及密码、或任何其他未经您合法授权的情形时，您应立即以有效方式通知{org}并提供{org}所需的相关材料（包括但不限于提供您的身份信息和相关身份资料、相关事实情况及您的要求等）。{org}收到您的有效请求并核实身份后，会根据不同情况采取相应措施。若您提供的信息不完全，导致{org}无法核实您的身份或{org}无法判断您的需求等，而导致{org}无法进行及时处理，给您带来的损失，您应自行承担。同时，{org}对您的请求采取措施需要合理期限，对于您通知{org}以及{org}根据您的有效通知采取措施之前，由于他人行为给您造成的损失，{org}不承担任何责任。&nbsp;<br />
<br />
2.2 资质材料&nbsp;<br />
<br />
2.2.1 您保证：您具备使用本服务、接入和运营课程或提供相关服务等行为的相关合法资质或已经过了相关政府部门的审核批准；您提供的主体资质材料、相关资质或证明以及其他任何文件等信息真实、准确、完整，并在信息发生变更后，及时进行更新；您具备履行本协议项下之义务、各种行为的能力；您履行相关义务、从事相关行为不违反任何对您的有约束力的法律文件。否则，您应不使用{org}提供的相关服务，且应独自承担由此带来的一切责任及给用户、{org}造成的全部损失。&nbsp;<br />
<br />
2.2.2 您保证：您会依法及按照{org}要求提交使用本服务所必须的真实、准确的经过您签章确认的主体资质材料以及联系人姓名（名称）、地址、电子邮箱等相关资料。&nbsp;<br />
<br />
2.2.3 您保证：您在{platform}上通过您的课程提供的各种服务，依法已经具有相关的合法资质或获得了有关部门的许可或批准，并会向{org}提交相关资质或证明文件。&nbsp;<br />
<br />
2.2.4 您保证：您在{platform}上通过您的课程提供的各种服务，符合国家相关法规的规定，不违反任何相关法规及相关协议、规则，也不会侵犯任何人的合法权益，同时，会依法、依约或按照{org}的要求提供版权、专利权等相关证明文件。&nbsp;<br />
<br />
2.3 服务费用&nbsp;<br />
<br />
2.3.1 目前，{org}向您提供{platform}服务是免费的，如后续{org}可能会对服务进行调整，并有权根据运营情况单独决定是否向您收取相应服务费用。&nbsp;<br />
<br />
2.3.2 如后续{org}向您收取相应服务费用的，您因使用{org}提供的相关服务而产生的所有服务费用由您自行承担，您应按相关协议、规则等的规定支付费用，否则，{org}有权不提供相关服务。您选择使用相关服务并支付费用后，在服务未到期之前，若您单方要求提前解除服务的，{org}有权将您未使用的服务对应的费用不予退还而作为您单方违约的违约金予以没收。&nbsp;<br />
<br />
在本服务中，{org}如需对服务收取相应服务费用的，{org}有权单方根据实际需要对收费服务的收费标准、方式进行修改和变更，前述修改、变更前，{org}将在相应服务页面进行通知或公告。如果您不同意上述修改、变更，则应立即停止使用相应服务，否则，您的任何使用行为，即视为您同意上述修改、变更。&nbsp;<br />
<br />
您若对{org}提供的收费标准、服务使用期限等费用结算事项的通知内容有异议的，应在收到{org}通知后及时以书面形式告知{org}，{org}收到您的书面告知会进行核实，否则，视为您认可{org}提供的费用结算事项的通知内容，双方应按照{org}提供的费用结算事项的通知内容，进行费用的结算及支付等。&nbsp;<br />
<br />
您理解并同意：若{org}按照前述约定对于本服务是否收费、收费标准等单方做出调整、变化的，您会予以全部遵守、同意，但您依法或按照约定终止使用本服务的除外。&nbsp;<br />
<br />
2.4 课程要求&nbsp;<br />
<br />
2.4.1 您应自行负责您课程的开发、创建、上课、管理、运营等工作，并且自行承担相应的费用。&nbsp;<br />
<br />
2.4.2 您的课程，应符合相关法规、技术规范或标准等，同时，还应符合{platform}的对接入课程在内容、服务等方面的统一要求和您在课程介绍页面对课程的介绍，以确保课程可以在{platform}真实、安全、稳定的运营。&nbsp;<br />
<br />
2.4.3 您不得在课程内宣传与本课程无关的任何其他信息，包括但不限于广告、其他的课程产品信息等（除非双方另有约定），也不得在课程内添加指向非{org}拥有或控制或{org}书面同意的网站链接。&nbsp;<br />
<br />
2.4.4 您课程在{platform}上运营期间，您需向用户提供及时有效的客户服务，客户服务形式包括但不限于通过明确且合理的方式告知用户客户服务渠道、提供QQ/电话等，并自行承担客服费用。&nbsp;<br />
<br />
2.4.5 您应当在课程中向相关权利人提供投诉途径，确保权利人在认为您侵犯其合法权益时可以向您主张权利。&nbsp;<br />
<br />
2.4.6 如果您的课程符合{platform}支付接入的相关要求时，则可依相关规范、流程等接入到{org}的支付系统。&nbsp;<br />
<br />
2.5 课程运营&nbsp;<br />
<br />
2.5.1 您应自行按照相关法规，运营您的课程，履行相关义务，并自行承担全部责任，包括但不限于：&nbsp;<br />
<br />
（1）依照相关法律法规的规定，保留相应的访问、使用等日志记录；&nbsp;<br />
<br />
（2）国家有权机关向您依法查询相关信息时，应积极配合提供；&nbsp;<br />
<br />
（3）主动履行其他您依法应履行的义务。&nbsp;<br />
<br />
2.5.2 您保证：&nbsp;<br />
<br />
（1）您的课程、提供给用户的相关服务及发布的相关信息、内容等，不违反相关法律、法规、政策等的规定及本协议或相关协议、规则等，也不会侵犯任何人的合法权益；&nbsp;<br />
<br />
（2）课程上课过程中应尊重用户知情权、选择权，应当坚持诚信原则，不误导、欺诈、混淆用户，尊重用户的隐私，不骚扰用户，不制造垃圾信息。&nbsp;<br />
<br />
（3）如您的股东或高级管理人员（包括但不限于董事长、总经理、财务总监等）同时也为{org}及{org}关联公司的员工（包括{org}的试用期员工、正式员工、劳务派遣形式的员工及其他受{org}及{org}关联公司管理的其他性质的员工或人员）时，您应当在注册时主动书面告知{org}。&nbsp;<br />
<br />
2.5.3 您不得从事任何包括但不限于以下的违反法规的行为，也不得为以下违反法规的行为提供便利（包括但不限于为您课程的用户的行为提供便利等）：&nbsp;<br />
<br />
（1）反对宪法所确定的基本原则的行为；&nbsp;<br />
<br />
（2）危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的行为；&nbsp;<br />
<br />
（3）损害国家荣誉和利益的行为；&nbsp;<br />
<br />
（4）煽动民族仇恨、民族歧视，破坏民族团结的行为；&nbsp;<br />
<br />
（5）破坏国家宗教政策，宣扬邪教和封建迷信的行为；&nbsp;<br />
<br />
（6）散布谣言，扰乱社会秩序，破坏社会稳定的行为；&nbsp;<br />
<br />
（7）散布淫秽、色情、赌博、暴力、凶杀、恐怖或者教唆犯罪的行为；&nbsp;<br />
<br />
（8）侮辱或者诽谤他人，侵害他人合法权益的行为；&nbsp;<br />
<br />
（9）侵害他人知识产权、商业秘密等合法权利的行为；&nbsp;<br />
<br />
（10）恶意虚构事实、隐瞒真相以误导、欺骗他人的行为；&nbsp;<br />
<br />
（11）发布、传送、传播广告信息及垃圾信息；（12）其他法律法规禁止的行为。&nbsp;<br />
<br />
2.5.4 您不得从事包括但不限于以下行为，也不得为以下行为提供便利（包括但不限于为您的用户的行为提供便利等）：&nbsp;<br />
<br />
（1）删除、隐匿、改变{platform}显示或其中包含的任何专利、著作权、商标或其他所有权声明；&nbsp;<br />
<br />
（2）以任何方式干扰或企图干扰{org}任何产品、任何部分或功能的正常运行，或者制作、发布、传播上述工具、方法等；&nbsp;<br />
<br />
（3）避开、尝试避开或声称能够避开任何内容保护机制，或导致用户认为其直接与{org}{platform}及{org}相关产品进行交互；&nbsp;<br />
<br />
（4）在未获得{org}书面许可的情况下，以任何方式使用{org}URL地址、技术接口等；&nbsp;<br />
<br />
（5）在未经过用户同意的情况下，向任何其他用户及他方显示或以其他任何方式提供该用户的任何信息；&nbsp;<br />
<br />
（6）请求、收集、索取或以其他方式获取用户QQ、{org}朋友或QQ空间等{org}服务的登录帐号、密码或其他任何身份验证凭据；&nbsp;<br />
<br />
（7）在没有获得用户明示同意的情况下，直接联系用户，或向用户发布任何商业广告及骚扰信息；&nbsp;<br />
<br />
（8）为任何用户自动登录到{org}{platform}提供代理身份验证凭据；&nbsp;<br />
<br />
（9）提供跟踪功能，包括但不限于识别其他用户在个人主页上查看、点击等操作行为；&nbsp;<br />
<br />
（10）自动将浏览器窗口定向到其他网页；&nbsp;<br />
<br />
（11）未经授权获取对{org}产品或服务的访问权；&nbsp;<br />
<br />
（12）课程服务内容中含有计算机病毒、木马或其他恶意程序等任何可能危害{org}或用户权益和终端信息安全等的内容；&nbsp;<br />
<br />
（13）设置或发布任何违反相关法规、公序良俗、社会公德等的玩法、内容等；&nbsp;<br />
<br />
（14）公开表达或暗示，您与{org}之间存在合作关系，包括但不限于相互持股、商业往来或合作关系等，或声称{org}对您的认可；&nbsp;<br />
<br />
（15）未经{org}许可，实施包括但不限于以赠送、发售等方式使用{org}或第三方的任何虚拟货币或品牌服务（例如Q币、Q点、黄钻等）的行为；&nbsp;<br />
<br />
（16）其他{org}认为不应该、不适当的行为、内容。&nbsp;<br />
<br />
2.5.5 本服务中可能会使用第三方软件或技术，若有使用，前述第三方软件或技术相关协议或其他文件，均是本协议不可分割的组成部分，与本协议具有同等的法律效力，您应当遵守这些要求。否则，因此带来的一切责任您应自行承担。如因本服务使用的第三方软件或技术引发的任何纠纷，由该第三方负责解决。&nbsp;<br />
<br />
2.6 课程的下线规则&nbsp;<br />
<br />
2.6.1 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排履行完毕的，或课程有用户已经报名但未学习完的，在未经已报名但未学习完用户和{org}都同意的前提下，您不得擅自终止课程的运营或服务的提供。&nbsp;<br />
<br />
2.6.2 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排，或课程有用户已经报名但仍未学习完的，若您确须要提前终止课程运营或服务提供，您应至少提前通知用户和{org}，同时，您在取得已报名但未学习完用户同意并妥善处理相关退费或各类用户损失等事宜后，并经{org}同意后，方可终止相关课程的运营或服务的提供。&nbsp;<br />
<br />
2.6.3 您发布课程后，无论课程是否收费，在课程未按照您发布课程时的安排，或课程有用户已经报名但仍未学习完的，若您发布的课程由于违反相关法规或本协议相关约定时，您应该按照{org}的要求终止课程的运营或{org}有权随时终止课程的运营或服务的提供，同时，您应妥善处理已报名但未学习完用户的退费、各类用户损失等问题，若由此造成用户、{org}损失或有纠纷的，您应负责解决相关纠纷并自行承担全部责任。&nbsp;<br />
<br />
2.6.4 若课程因为任何原因需要下线的，在课程下线过程中，您应积极与用用户联系并妥善处理已报名但未学习完用户的退费、各类用户损失等事宜，依法保护用户的合法权益不受损害，否则，若由此造成用户、{org}损失或有纠纷的，您应负责解决相关纠纷并自行承担全部责任。同时，您应发布下线公告直至课程下线（下线公告应当包含但不限于下线时间、退费方式、用户补偿方案、客服联系电话等内容），且您不得强制用户切换到其他第三方平台进行学习，亦不得免除自身对未结束课程进行退款责任等义务。如您选择退还用户已支付的课程相应费用、补偿用户损失的，您同意{org}在您的账户结算额中直接扣除前述退还给用户的课程相应费用、补偿用户损失费用等全部费用。&nbsp;<br />
<br />
2.6.5 您同意，无论课程因为任何原因需要下线的，{org}有权单方冻结尚未支付给您的所有款项，并从前述冻结款项中直接扣除应当退还给用户的费用、补偿用户损失费用等，其他条款与该条有不一致的，以该条为准。&nbsp;<br />
<br />
2.7 关于用户数据的规则&nbsp;<br />
<br />
2.7.1 您的课程或服务对于用户数据的收集、保存、使用等必须满足以下要求：（1）您的课程或服务需要收集用户任何数据的，必须事先获得用户的明确同意，且仅应当收集为课程运行而必要的用户数据，同时应当告知用户相关数据收集的目的、范围及使用方式等，保障用户知情权；（2）您收集用户的数据后，必须采取必要的保护措施，防止用户数据被盗、泄漏等；（3）您在特定课程中收集的用户数据仅可以在该特定课程中使用，不得将其使用在该特定课程之外或为其他任何目的进行使用，也不得以任何方式将其提供给他人；（4）您应向用户提供隐私保护政策。隐私保护政策须在课程界面上明显位置向用户展示。&nbsp;<br />
<br />
2.7.2 您不得收集用户的隐私信息数据及其他{org}认为属于敏感信息范畴的数据，包括但不限于不得收集或要求用户提供任何手机号、QQ密码、用户关系链、好友列表数据、银行账号和密码等。&nbsp;<br />
<br />
2.7.3 您不得使用{platform}数据用于向用户进行广告宣传使用。&nbsp;<br />
<br />
2.7.4 如果{org}认为您收集、使用用户数据的方式，可能损害用户体验，{org}有权要求您删除相关数据并不得再以该方式收集、使用用户数据。&nbsp;<br />
<br />
2.7.5 {org}有权限制或阻止您获取用户数据及{platform}数据。&nbsp;<br />
<br />
2.7.6 未经{org}事先书面同意，您不得为本协议约定之外的目的使用用户数据或平台运营数据，亦不得以任何形式将前述数据提供给他人。&nbsp;<br />
<br />
2.7.7 一旦课程发布者停止使用{platform}，或{org}基于任何原因终止您使用本服务，您必须立即删除全部从{platform}中获得的数据（包括各种备份），且不得再以任何方式进行使用。&nbsp;<br />
<br />
2.7.8 您应自行对因使用本服务而存储在{org}服务器的各类数据等信息，在本服务之外，采取合理、安全的技术措施，确保其安全性，并对自己的行为（包括但不限于自行安装软件、采取加密措施或进行其他安全措施等）所引起的结果承担全部责任。<br />
<br />
2.8 法律责任&nbsp;<br />
<br />
2.8.1 您保证：如果您使用本服务产生相应费用的，您会按照本协议及相关协议、规则等支付相关费用。否则，您理解并同意：每延期一日，您应当向{org}支付所欠费用千分之一的违约金。同时，{org}有权随时单方采取包括但不限于以下措施中的一种或多种，以维护自己的合法权益。&nbsp;<br />
<br />
（1）从其他{org}应支付给您或您关联公司的任何费用中直接抵扣；&nbsp;<br />
<br />
（2）暂停向您或您关联公司结算或支付任何费用；&nbsp;<br />
<br />
（3）中止、终止您或您关联公司使用本服务；&nbsp;<br />
<br />
（4）中止、终止您或您关联公司的后台管理权限；&nbsp;<br />
<br />
（5）删除您或您关联公司在使用本服务中存储的任何数据；&nbsp;<br />
<br />
（6）禁止您今后将您的任何新课程接入{platform}；&nbsp;<br />
<br />
（7）其他{org}可以采取的为维护自己权益的措施。&nbsp;<br />
<br />
2.8.2 您保证：您使用本服务及您的任何行为，不违反任何相关法规、本协议和相关协议、规则等。您理解并同意：若{org}自行发现或根据相关部门的信息、权利人的投诉等发现您可能存在违反前述保证情形的，{org}有权根据一般人的认识自己独立判断，以认定您是否存在违反前述保证情形，若{org}经过判断认为您存在违反前述保证情形的，{org}有权随时单方采取以下一项或多项措施。&nbsp;<br />
<br />
（1）要求您立即更换、修改违反前述保证情形的相关内容；&nbsp;<br />
<br />
（2）对存在违反前述保证情形的课程、您或您关联公司名下的全部课程或任何一款课程采取下线措施即终止课程在{platform}的运营；&nbsp;<br />
<br />
（3）禁止您或您关联公司今后将任何新课程接入{platform}；&nbsp;<br />
<br />
（4）中止、终止向违反前述保证情形的课程、您或您关联公司名下的全部课程或任何一款课程，或您或您关联公司提供部分或全部本服务；&nbsp;<br />
<br />
（5）冻结{org}尚未支付给您的所有款项,并将前述冻结款项予以没收，作为您向{org}承担违约责任的违约金而不再向您支付；&nbsp;<br />
<br />
（6）将您的行为对外予以公告；&nbsp;<br />
<br />
（7）其他{org}认为适合的处理措施。&nbsp;<br />
<br />
2.8.3 如在{org}告知您或您自行得知您存在任何违法情形后，您应按法律法规或{org}的规定向{org}提出反通知。但是，无论{org}是否告知您、您是否提出反通知或反通知是否符合相关法规、{org}要求等，均不影响{org}进行自己的独立判断和采取相关措施。&nbsp;<br />
<br />
2.8.4 若{org}按照上述条款、本协议的其他相关约定或因您违反相关法律的规定，对您或您的课程采取任何行为或措施，所引起的纠纷、责任等一概由您自行负责，造成您损失的，您应自行全部承担，造成{org}或他人损失的，您也应自行承担全部责任。&nbsp;<br />
<br />
2.8.5 若{org}按照上述条款、本协议的其他相关约定或因您违反相关法律的规定，对您或您的课程采取任何行为或措施后，导致您没有使用到相应服务但已经缴纳相应费用的，对该部分费用，{org}有权不予退还，而作为您违反约定的违约金予以没收。&nbsp;<br />
<br />
3.{org}的权利义务&nbsp;<br />
<br />
3.1 {org}会根据您选择的服务向您提供相应的服务，如后续{org}提供服务需要收费的，您应向{org}支付相应费用，{org}在您支付相应费用后，会向您提供相应的服务。&nbsp;<br />
<br />
3.2 保护您的信息的安全是{org}的一项基本原则，未经您的同意，{org}不会向{org}以外的任何公司、组织和个人披露、提供您的信息，但下列情形除外：&nbsp;<br />
<br />
（1）据本协议或其他相关协议、规则等规定可以提供的；&nbsp;<br />
<br />
（2）依据法律法规的规定可以提供的；&nbsp;<br />
<br />
（3）行政、司法等政府部门要求提供的；&nbsp;<br />
<br />
（4）您同意{org}向第三方提供；&nbsp;<br />
<br />
（5）为解决举报事件、提起诉讼而需要提供的；&nbsp;<br />
<br />
（6）为防止严重违法行为或涉嫌犯罪行为发生而采取必要合理行动所必须提供的。&nbsp;<br />
<br />
3.3 尽管{org}对您的信息保护做了极大的努力，但是仍然不能保证在现有的安全技术措施下，您的信息可能会因为不可抗力或非{org}因素造成泄漏、窃取等，由此给您造成损失的，您同意{org}可以免责。&nbsp;<br />
<br />
3.4 {org}有权开发、运营与您课程相似或相竞争的课程，同时{org}也不保证平台上不会出现其他课程发布者提供的与您课程相竞争的课程。&nbsp;<br />
<br />
3.5 {org}有权在包括但不限于课程介绍页等，向用户阐述该课程为您开发以及由您向用户提供客户服务等。&nbsp;<br />
<br />
3.6 {org}可将本协议下的权利和义务的部分或全部转让给他人，如果您不同意{org}的该转让，则有权停止使用本协议下服务。否则，视为您对此予以接受。&nbsp;<br />
<br />
3.7 除了另行有约定外，{org}无需为按照本协议享有的权益而向您支付任何费用。&nbsp;<br />
<br />
3.8 您理解并同意：为向更多互联网使用者推广您的课程，{org}有权采取以下行为，而无须再取得您的同意。&nbsp;<br />
<br />
（1）在{org}{platform}以外的平台、网站等采取各种形式对课程进行宣传、推广；&nbsp;<br />
<br />
（2）{org}可根据整体运营安排，自主选择向整个或部分全世界范围内的互联网用户提供您的课程；&nbsp;<br />
<br />
（3）有权为本协议目的使用您课程的LOGO、标识、名称、图片等相关素材。&nbsp;<br />
<br />
4.广点通服务&nbsp;<br />
<br />
4.1 广点通服务，指{org}向您提供的，供您将您所接入的课程或您提供的服务在QQ空间、朋友及其他{org}平台上进行推广的服务，是{org}向您提供的本服务的一部分，是否使用该服务由您自行选择。该服务的相关协议、规则、公告、提示等，请见相应页面，您若选择使用该服务的，应按照广点通的要求进行开通，并遵守前述相关协议、规则、公告、提示等。&nbsp;<br />
<br />
4.2 您以任何形式登录、使用广点通服务，即表示您已理解并接受广点通服务的相关协议、规则、公告、提示等的约束。&nbsp;<br />
<br />
4.3 您同意{org}有权无须经您同意或提前通知而直接对广点通服务的相关协议、规则等进行修改，修改后的内容一旦在网页上公布即有效代替原来的条款，对您产生约束力。&nbsp;<br />
<br />
5.关于免责&nbsp;<br />
<br />
5.1 您理解并同意：鉴于网络服务的特殊性，{org}有权在无需通知您的情况下根据{org}{platform}的整体运营情况或相关运营规范、规则等，可以随时变更、中止或终止部分或全部的服务，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.2 您理解并同意：为了向您提供更完善的服务，{org}有权定期或不定期地对提供本服务的平台或相关设备进行检修、维护、升级等，此类情况可能会造成相关服务在合理时间内中断或暂停的，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.3 您理解并同意：{org}的服务是按照现有技术和条件所能达到的现状提供的。{org}会尽最大努力向您提供服务，确保服务的连贯性和安全性；但{org}不能保证其所提供的服务毫无瑕疵，也无法随时预见和防范法律、技术以及其他风险，包括但不限于不可抗力、病毒、木马、黑客攻击、系统不稳定、第三方服务瑕疵、政府行为等原因可能导致的服务中断、数据丢失以及其他的损失和风险。所以您也同意：即使{org}提供的服务存在瑕疵，但上述瑕疵是当时行业技术水平所无法避免的，其将不被视为{org}违约，同时，由此给您造成的数据或信息丢失等损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.4 您理解并同意：在使用本服务的过程中，可能会遇到不可抗力等风险因素，使本服务发生中断。不可抗力是指不能预见、不能克服并不能避免且对一方或双方造成重大影响的客观事件，包括但不限于自然灾害如洪水、地震、瘟疫流行和风暴等以及社会事件如战争、动乱、政府行为等。出现上述情况时，{org}将努力在第一时间与相关单位配合，及时进行修复，若由此给您造成损失的，您同意放弃追究{org}的责任。&nbsp;<br />
<br />
5.5 您理解并同意：若由于对以下情形导致的服务中断或受阻，给您造成损失的，您同意放弃追究{org}的责任：&nbsp;<br />
<br />
（1）受到计算机病毒、木马或其他恶意程序、黑客攻击的破坏；&nbsp;<br />
<br />
（2）您或{org}的电脑软件、系统、硬件和通信线路出现故障；&nbsp;<br />
<br />
（3）您操作不当；&nbsp;<br />
<br />
（4）您通过非{org}授权的方式使用本服务；&nbsp;<br />
<br />
（5）其他{org}无法控制或合理预见的情形。&nbsp;<br />
6.服务的中止或终止&nbsp;<br />
<br />
6.1 如您书面通知{org}不接受本协议或对其的修改，{org}有权随时中止或终止向您提供本服务。&nbsp;<br />
<br />
6.2 因不可抗力因素导致您无法继续使用本服务或{org}无法提供本服务的，{org}有权随时终止协议。&nbsp;<br />
<br />
6.3 本协议约定的其他中止或终止条件发生或实现的，{org}有权随时中止或终止向您提供本服务。&nbsp;<br />
<br />
6.4 由于您违反本协议约定，{org}依约终止向您提供本服务后，如您后续再直接或间接，或以他人名义注册使用本服务的，{org}有权直接单方面暂停或终止提供本服务。&nbsp;<br />
<br />
6.5 如本协议或本服务因为任何原因终止的，对于您的帐号中的全部数据或您因使用本服务而存储在{org}服务器中的数据等任何信息，{org}可将该等信息保留或删除，包括服务终止前您尚未完成的任何数据。&nbsp;<br />
<br />
6.6 如本协议或本服务因为任何原因终止的，您应自行处理好关于数据等信息的备份以及与您的用户之间的相关事项的处理等，由此造成{org}损失的，您应负责赔偿。&nbsp;<br />
<br />
7.关于通知&nbsp;<br />
<br />
7.1 {org}可能会以网页公告、网页提示、电子邮箱、手机短信、常规的信件传送、您注册的本服务账户的管理系统内发送站内信等方式中的一种或多种，向您送达关于本服务的各种规则、通知、提示等信息，该等信息一经{org}采取前述任何一种方式公布或发送，即视为您已经接受并同意，对您产生约束力。若您不接受的，请您书面通知{org}并停止使用本服务，否则视为您已经接受、同意。&nbsp;<br />
<br />
7.2 若由于您提供的电子邮箱、手机号码、通讯地址等信息错误，导致您未收到相关规则、通知、提示等信息的，您同意仍然视为您已经收到相关信息并受其约束，一切后果及责任由您自行承担。&nbsp;<br />
<br />
7.3 您也同意{org}或合作伙伴可以向您的电子邮件、手机号码等发送可能与本服务不相关的其他各类信息包括但不限于商业广告等。&nbsp;<br />
<br />
7.4 若您有事项需要通知{org}的，应当按照本服务对外正式公布的联系方式书面通知{org}。&nbsp;<br />
<br />
8.知识产权&nbsp;<br />
<br />
8.1 {org}在本服务中提供的信息内容（包括但不限于网页、文字、图片、音频、视频、图表等）的知识产权均归{org}所有，依法属于用户、课程发布者及其他第三方所有的除外。除另有特别声明外，{org}提供本服务时所依托软件的著作权、专利权及其他知识产权均归{org}所有。上述及其他任何{org}依法拥有的知识产权均受到法律保护，未经{org}书面许可，您不得以任何形式进行使用或创造相关衍生作品。&nbsp;<br />
<br />
8.2 您仅拥有依照本协议约定合法使用本服务或相关API的权利，与本服务相关的API相关的著作权、专利权等相关全部权利归{org}所有。未经{org}书面许可，您不得违约或违法使用，不得向任何单位或个人出售、转让、转授权{org}的代码、API及开发工具等。&nbsp;<br />
<br />
9.其他&nbsp;<br />
<br />
9.1 若您和{org}之间发生任何纠纷或争议，首先应友好协商解决；协商不成功的，双方均同意将纠纷或争议提交本协议签订地有管辖权的人民法院解决。&nbsp;<br />
<br />
9.2 本协议所有条款的标题仅为阅读方便，本身并无实际涵义，不能作为本协议涵义解释的依据。&nbsp;<br />
<br />');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (5,0,'','','NewsSourceItem','','','','新浪,网易,腾讯');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (7,0,'','','SysIsLoginLogs','','','','False');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (8,0,'','','SysIsWorkLogs','','','','False');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (9,0,'','','SystemName','','','','微厦在线学习云平台');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (10,0,'','','SysLoginTimeSpan','','','','10');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (12,0,'','','IsWebsiteSatatic','','','','True');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (206,0,'','','LoginPoint','','','','10');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (30,0,'','','IsAllowMobile','','','','True');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (31,0,'','','IsAllowMobileVerifyCode','','','','False');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (207,0,'','','LoginPointMax','','','','30');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (208,0,'','','SharePoint','','','','3');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (197,0,'','','SmsCurrent','','','','河南腾信');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (209,0,'','','SharePointMax','','','','12');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (254,0,'','','111','','','','22');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (217,0,'','','RegFirst','','','','2000');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (228,0,'','','QQLoginIsUse','','','','True');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (193,0,'','','flowNumber','','','','222');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (194,0,'','','SubjectForAccout_1','','','','8,');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (196,0,'','','SysWorkTimeSpan','','','','10');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (210,0,'','','RegPoint','','','','100');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (211,0,'','','RegPointMax','','','','500');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (212,0,'','','PointConvert','','','','1000');INSERT INTO "SystemPara"("Sys_Id","Org_Id","Org_Name","Sys_Default","Sys_Key","Sys_ParaIntro","Sys_SelectUnit","Sys_Unit","Sys_Value") VALUES (229,0,'','','WeixinLoginIsUse','','','','True');
-- 表 SystemPara 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "SystemPara_aaaaaSystemPara_PK" ON public."SystemPara" USING btree ("Sys_Id");

-- 创建表 Teacher --
CREATE TABLE IF NOT EXISTS public."Teacher"
(
	"Th_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Ac_UID" character varying(100) COLLATE pg_catalog."default",
	"Dep_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) NOT NULL COLLATE pg_catalog."default",
	"Th_AccName" character varying(50) COLLATE pg_catalog."default",
	"Th_AddrContact" character varying(255) COLLATE pg_catalog."default",
	"Th_Address" character varying(255) COLLATE pg_catalog."default",
	"Th_Age" integer NOT NULL,
	"Th_Anwser" character varying(255) COLLATE pg_catalog."default",
	"Th_Birthday" timestamp without time zone NOT NULL,
	"Th_CodeNumber" character varying(50) COLLATE pg_catalog."default",
	"Th_CrtTime" timestamp without time zone NOT NULL,
	"Th_Education" character varying(255) COLLATE pg_catalog."default",
	"Th_Email" character varying(50) COLLATE pg_catalog."default",
	"Th_IDCardNumber" character varying(50) COLLATE pg_catalog."default",
	"Th_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Th_IsOpenMobi" boolean NOT NULL,
	"Th_IsOpenPhone" boolean NOT NULL,
	"Th_IsPass" boolean NOT NULL,
	"Th_IsShow" boolean NOT NULL,
	"Th_IsUse" boolean NOT NULL,
	"Th_Job" character varying(255) COLLATE pg_catalog."default",
	"Th_LastTime" timestamp without time zone NOT NULL,
	"Th_LinkMan" character varying(50) COLLATE pg_catalog."default",
	"Th_LinkManPhone" character varying(50) COLLATE pg_catalog."default",
	"Th_Major" character varying(255) COLLATE pg_catalog."default",
	"Th_Name" character varying(50) COLLATE pg_catalog."default",
	"Th_Nation" character varying(50) COLLATE pg_catalog."default",
	"Th_Native" character varying(255) COLLATE pg_catalog."default",
	"Th_Phone" character varying(50) COLLATE pg_catalog."default",
	"Th_PhoneMobi" character varying(50) COLLATE pg_catalog."default",
	"Th_Photo" character varying(255) COLLATE pg_catalog."default",
	"Th_Pinyin" character varying(50) COLLATE pg_catalog."default",
	"Th_Pw" character varying(100) COLLATE pg_catalog."default",
	"Th_Qq" character varying(50) COLLATE pg_catalog."default",
	"Th_Qus" character varying(255) COLLATE pg_catalog."default",
	"Th_RegTime" timestamp without time zone NOT NULL,
	"Th_Score" integer NOT NULL,
	"Th_Sex" integer NOT NULL,
	"Th_Signature" character varying(255) COLLATE pg_catalog."default",
	"Th_Tax" integer NOT NULL,
	"Th_Title" character varying(100) COLLATE pg_catalog."default",
	"Th_ViewNum" integer NOT NULL,
	"Th_Weixin" character varying(100) COLLATE pg_catalog."default",
	"Th_Zip" character varying(50) COLLATE pg_catalog."default",
	"Ths_ID" integer NOT NULL,
	"Ths_Name" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_teacher PRIMARY KEY ("Th_ID")
);
-- 表 Teacher 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Teacher_Th_ID_seq";
ALTER SEQUENCE IF EXISTS public."Teacher_Th_ID_seq" OWNED BY public."Teacher"."Th_ID";
ALTER TABLE "Teacher" ALTER COLUMN "Th_ID" SET DEFAULT NEXTVAL('"Teacher_Th_ID_seq"'::regclass);

INSERT INTO "Teacher"("Th_ID","Ac_ID","Ac_UID","Dep_Id","Org_ID","Org_Name","Th_AccName","Th_AddrContact","Th_Address","Th_Age","Th_Anwser","Th_Birthday","Th_CodeNumber","Th_CrtTime","Th_Education","Th_Email","Th_IDCardNumber","Th_Intro","Th_IsOpenMobi","Th_IsOpenPhone","Th_IsPass","Th_IsShow","Th_IsUse","Th_Job","Th_LastTime","Th_LinkMan","Th_LinkManPhone","Th_Major","Th_Name","Th_Nation","Th_Native","Th_Phone","Th_PhoneMobi","Th_Photo","Th_Pinyin","Th_Pw","Th_Qq","Th_Qus","Th_RegTime","Th_Score","Th_Sex","Th_Signature","Th_Tax","Th_Title","Th_ViewNum","Th_Weixin","Th_Zip","Ths_ID","Ths_Name") VALUES (28,2,'0f6305210623cffd6f966db6a3606a1c',0,4,'郑州微厦计算机科技有限公司','tester','','',1995,'','1995-03-07 00:00:00','','2016-11-26 17:40:41','31','','410105199503071228','',True,False,True,False,True,'','2024-01-22 17:58:18','','','','韩晓梅','','河南省,郑州市,金水区','','400 6015615','','HXM','e10adc3949ba59abbe56e057f20f883e','','','1753-01-01 00:00:00',0,2,'',0,'',6,'','',1,'讲师');

-- 创建表 TeacherComment --
CREATE TABLE IF NOT EXISTS public."TeacherComment"
(
	"Thc_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(50) COLLATE pg_catalog."default",
	"Thc_Comment" character varying(1000) COLLATE pg_catalog."default",
	"Thc_CrtTime" timestamp without time zone NOT NULL,
	"Thc_Device" character varying(50) COLLATE pg_catalog."default",
	"Thc_IP" character varying(100) COLLATE pg_catalog."default",
	"Thc_IsShow" boolean NOT NULL,
	"Thc_IsUse" boolean NOT NULL,
	"Thc_Reply" character varying(1000) COLLATE pg_catalog."default",
	"Thc_Score" real NOT NULL,
	 CONSTRAINT key_teachercomment PRIMARY KEY ("Thc_ID")
);
-- 表 TeacherComment 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TeacherComment_Thc_ID_seq";
ALTER SEQUENCE IF EXISTS public."TeacherComment_Thc_ID_seq" OWNED BY public."TeacherComment"."Thc_ID";
ALTER TABLE "TeacherComment" ALTER COLUMN "Thc_ID" SET DEFAULT NEXTVAL('"TeacherComment_Thc_ID_seq"'::regclass);



-- 创建表 TeacherHistory --
CREATE TABLE IF NOT EXISTS public."TeacherHistory"
(
	"Thh_ID" integer NOT NULL,
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(50) COLLATE pg_catalog."default",
	"Thh_Compay" character varying(200) COLLATE pg_catalog."default",
	"Thh_CrtTime" timestamp without time zone,
	"Thh_Education" character varying(200) COLLATE pg_catalog."default",
	"Thh_EndTime" timestamp without time zone,
	"Thh_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Thh_Job" character varying(200) COLLATE pg_catalog."default",
	"Thh_Major" character varying(200) COLLATE pg_catalog."default",
	"Thh_Post" character varying(200) COLLATE pg_catalog."default",
	"Thh_School" character varying(200) COLLATE pg_catalog."default",
	"Thh_StartTime" timestamp without time zone NOT NULL,
	"Thh_Success" character varying(1000) COLLATE pg_catalog."default",
	"Thh_Theme" character varying(100) COLLATE pg_catalog."default",
	"Thh_Type" character varying(50) COLLATE pg_catalog."default",
	 CONSTRAINT key_teacherhistory PRIMARY KEY ("Thh_ID")
);
-- 表 TeacherHistory 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TeacherHistory_Thh_ID_seq";
ALTER SEQUENCE IF EXISTS public."TeacherHistory_Thh_ID_seq" OWNED BY public."TeacherHistory"."Thh_ID";
ALTER TABLE "TeacherHistory" ALTER COLUMN "Thh_ID" SET DEFAULT NEXTVAL('"TeacherHistory_Thh_ID_seq"'::regclass);



-- 创建表 TeacherSort --
CREATE TABLE IF NOT EXISTS public."TeacherSort"
(
	"Ths_ID" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Ths_Intro" character varying(2000) COLLATE pg_catalog."default",
	"Ths_IsDefault" boolean NOT NULL,
	"Ths_IsUse" boolean NOT NULL,
	"Ths_Name" character varying(255) COLLATE pg_catalog."default",
	"Ths_Tax" integer NOT NULL,
	 CONSTRAINT key_teachersort PRIMARY KEY ("Ths_ID")
);
-- 表 TeacherSort 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TeacherSort_Ths_ID_seq";
ALTER SEQUENCE IF EXISTS public."TeacherSort_Ths_ID_seq" OWNED BY public."TeacherSort"."Ths_ID";
ALTER TABLE "TeacherSort" ALTER COLUMN "Ths_ID" SET DEFAULT NEXTVAL('"TeacherSort_Ths_ID_seq"'::regclass);

INSERT INTO "TeacherSort"("Ths_ID","Org_ID","Org_Name","Ths_Intro","Ths_IsDefault","Ths_IsUse","Ths_Name","Ths_Tax") VALUES (1,4,'郑州微厦计算机科技有限公司','中级（讲师、中学一级教师、小学高级教师、实验师）',True,True,'讲师',2);INSERT INTO "TeacherSort"("Ths_ID","Org_ID","Org_Name","Ths_Intro","Ths_IsDefault","Ths_IsUse","Ths_Name","Ths_Tax") VALUES (4,4,'郑州微厦计算机科技有限公司','初级（助教、助理讲师、教员、中学二级教师、中学三级教师、小学一级教师、小学二级教师、助理实验师、实验员）',False,True,'助教',1);INSERT INTO "TeacherSort"("Ths_ID","Org_ID","Org_Name","Ths_Intro","Ths_IsDefault","Ths_IsUse","Ths_Name","Ths_Tax") VALUES (5,4,'郑州微厦计算机科技有限公司','副高（副教授、中学高级教师、高级讲师、高级实验师）',False,True,'副教授',3);INSERT INTO "TeacherSort"("Ths_ID","Org_ID","Org_Name","Ths_Intro","Ths_IsDefault","Ths_IsUse","Ths_Name","Ths_Tax") VALUES (6,4,'郑州微厦计算机科技有限公司','正高级（教授、中学研究员级教师）',False,True,'正教授',4);INSERT INTO "TeacherSort"("Ths_ID","Org_ID","Org_Name","Ths_Intro","Ths_IsDefault","Ths_IsUse","Ths_Name","Ths_Tax") VALUES (7,4,'郑州微厦计算机科技有限公司','',False,False,'金牌大师',5);

-- 创建表 Teacher_Course --
CREATE TABLE IF NOT EXISTS public."Teacher_Course"
(
	"Thc_ID" integer NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Th_ID" integer NOT NULL,
	 CONSTRAINT key_teacher_course PRIMARY KEY ("Thc_ID")
);
-- 表 Teacher_Course 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."Teacher_Course_Thc_ID_seq";
ALTER SEQUENCE IF EXISTS public."Teacher_Course_Thc_ID_seq" OWNED BY public."Teacher_Course"."Thc_ID";
ALTER TABLE "Teacher_Course" ALTER COLUMN "Thc_ID" SET DEFAULT NEXTVAL('"Teacher_Course_Thc_ID_seq"'::regclass);



-- 创建表 TestPaper --
CREATE TABLE IF NOT EXISTS public."TestPaper"
(
	"Tp_Id" bigint NOT NULL,
	"Cou_ID" bigint NOT NULL,
	"Cou_Name" character varying(100) COLLATE pg_catalog."default",
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"Th_ID" integer NOT NULL,
	"Th_Name" character varying(255) COLLATE pg_catalog."default",
	"Tp_Author" character varying(50) COLLATE pg_catalog."default",
	"Tp_Count" integer NOT NULL,
	"Tp_CrtTime" timestamp without time zone NOT NULL,
	"Tp_Diff" integer NOT NULL,
	"Tp_Diff2" integer NOT NULL,
	"Tp_FromConfig" text,
	"Tp_FromType" integer NOT NULL,
	"Tp_Intro" text,
	"Tp_IsBuild" boolean NOT NULL,
	"Tp_IsFinal" boolean NOT NULL,
	"Tp_IsRec" boolean NOT NULL,
	"Tp_IsUse" boolean NOT NULL,
	"Tp_Lasttime" timestamp without time zone NOT NULL,
	"Tp_Logo" character varying(255) COLLATE pg_catalog."default",
	"Tp_Name" character varying(255) COLLATE pg_catalog."default",
	"Tp_PassScore" integer NOT NULL,
	"Tp_Remind" text,
	"Tp_Span" integer NOT NULL,
	"Tp_SubName" character varying(255) COLLATE pg_catalog."default",
	"Tp_Total" integer NOT NULL,
	"Tp_Type" integer NOT NULL,
	"Tp_UID" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_testpaper PRIMARY KEY ("Tp_Id")
);

-- 表 TestPaper 的索引 --
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Cou_ID" ON public."TestPaper" USING btree ("Cou_ID" DESC);

-- 创建表 TestPaperItem --
CREATE TABLE IF NOT EXISTS public."TestPaperItem"
(
	"TPI_ID" integer NOT NULL,
	"Ol_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"TPI_Count" integer NOT NULL,
	"TPI_Number" integer NOT NULL,
	"TPI_Percent" integer NOT NULL,
	"TPI_Type" integer NOT NULL,
	"Tp_UID" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_testpaperitem PRIMARY KEY ("TPI_ID")
);
-- 表 TestPaperItem 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TestPaperItem_TPI_ID_seq";
ALTER SEQUENCE IF EXISTS public."TestPaperItem_TPI_ID_seq" OWNED BY public."TestPaperItem"."TPI_ID";
ALTER TABLE "TestPaperItem" ALTER COLUMN "TPI_ID" SET DEFAULT NEXTVAL('"TestPaperItem_TPI_ID_seq"'::regclass);

INSERT INTO "TestPaperItem"("TPI_ID","Ol_ID","Org_ID","Org_Name","TPI_Count","TPI_Number","TPI_Percent","TPI_Type","Tp_UID") VALUES (680,0,0,'',10,50,50,1,'b98629d1e5117d62206b79cd6587dd8c');INSERT INTO "TestPaperItem"("TPI_ID","Ol_ID","Org_ID","Org_Name","TPI_Count","TPI_Number","TPI_Percent","TPI_Type","Tp_UID") VALUES (681,0,0,'',10,40,40,2,'b98629d1e5117d62206b79cd6587dd8c');INSERT INTO "TestPaperItem"("TPI_ID","Ol_ID","Org_ID","Org_Name","TPI_Count","TPI_Number","TPI_Percent","TPI_Type","Tp_UID") VALUES (682,0,0,'',0,0,0,3,'b98629d1e5117d62206b79cd6587dd8c');INSERT INTO "TestPaperItem"("TPI_ID","Ol_ID","Org_ID","Org_Name","TPI_Count","TPI_Number","TPI_Percent","TPI_Type","Tp_UID") VALUES (683,0,0,'',0,0,0,4,'b98629d1e5117d62206b79cd6587dd8c');INSERT INTO "TestPaperItem"("TPI_ID","Ol_ID","Org_ID","Org_Name","TPI_Count","TPI_Number","TPI_Percent","TPI_Type","Tp_UID") VALUES (684,0,0,'',10,10,10,5,'b98629d1e5117d62206b79cd6587dd8c');
-- 表 TestPaperItem 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "TestPaperItem_aaaaaTestPagerItem_PK" ON public."TestPaperItem" USING btree ("TPI_ID");

-- 创建表 TestPaperQues --
CREATE TABLE IF NOT EXISTS public."TestPaperQues"
(
	"Tq_Id" integer NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Qk_Id" integer NOT NULL,
	"Tp_Id" bigint NOT NULL,
	"Tq_Number" real NOT NULL,
	"Tq_Percent" integer NOT NULL,
	"Tq_Type" integer NOT NULL,
	 CONSTRAINT key_testpaperques PRIMARY KEY ("Tq_Id")
);
-- 表 TestPaperQues 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TestPaperQues_Tq_Id_seq";
ALTER SEQUENCE IF EXISTS public."TestPaperQues_Tq_Id_seq" OWNED BY public."TestPaperQues"."Tq_Id";
ALTER TABLE "TestPaperQues" ALTER COLUMN "Tq_Id" SET DEFAULT NEXTVAL('"TestPaperQues_Tq_Id_seq"'::regclass);


-- 表 TestPaperQues 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "TestPaperQues_aaaaaTestPaperQues_PK" ON public."TestPaperQues" USING btree ("Tq_Id");

-- 创建表 TestResults --
CREATE TABLE IF NOT EXISTS public."TestResults"
(
	"Tr_ID" integer NOT NULL,
	"Ac_ID" integer NOT NULL,
	"Ac_Name" character varying(255) COLLATE pg_catalog."default",
	"Cou_ID" bigint NOT NULL,
	"Org_ID" integer NOT NULL,
	"Org_Name" character varying(255) COLLATE pg_catalog."default",
	"Sbj_ID" bigint NOT NULL,
	"Sbj_Name" character varying(255) COLLATE pg_catalog."default",
	"St_IDCardNumber" character varying(50) COLLATE pg_catalog."default",
	"St_Sex" integer NOT NULL,
	"Sts_ID" bigint NOT NULL,
	"Sts_Name" character varying(255) COLLATE pg_catalog."default",
	"Tp_Id" bigint NOT NULL,
	"Tp_Name" character varying(255) COLLATE pg_catalog."default",
	"Tr_Colligate" real NOT NULL,
	"Tr_CrtTime" timestamp without time zone,
	"Tr_Draw" real NOT NULL,
	"Tr_IP" character varying(255) COLLATE pg_catalog."default",
	"Tr_IsSubmit" boolean NOT NULL,
	"Tr_Mac" character varying(255) COLLATE pg_catalog."default",
	"Tr_Name" character varying(255) COLLATE pg_catalog."default",
	"Tr_Results" text,
	"Tr_Score" real NOT NULL,
	"Tr_ScoreFinal" real NOT NULL,
	"Tr_UID" character varying(255) COLLATE pg_catalog."default",
	 CONSTRAINT key_testresults PRIMARY KEY ("Tr_ID")
);
-- 表 TestResults 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."TestResults_Tr_ID_seq";
ALTER SEQUENCE IF EXISTS public."TestResults_Tr_ID_seq" OWNED BY public."TestResults"."Tr_ID";
ALTER TABLE "TestResults" ALTER COLUMN "Tr_ID" SET DEFAULT NEXTVAL('"TestResults_Tr_ID_seq"'::regclass);


-- 表 TestResults 的索引 --
CREATE INDEX IF NOT EXISTS "TestResults_IX_Ac_ID" ON public."TestResults" USING btree ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Cou_ID" ON public."TestResults" USING btree ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Tp_Id" ON public."TestResults" USING btree ("Tp_Id" DESC);
CREATE UNIQUE INDEX IF NOT EXISTS "TestResults_aaaaaTestResults_PK" ON public."TestResults" USING btree ("Tr_ID");

-- 创建表 ThirdpartyAccounts --
CREATE TABLE IF NOT EXISTS public."ThirdpartyAccounts"
(
	"Ta_ID" integer NOT NULL,
	"Ac_ID" bigint NOT NULL,
	"Ta_Headimgurl" character varying(500) COLLATE pg_catalog."default",
	"Ta_NickName" character varying(255) COLLATE pg_catalog."default",
	"Ta_Openid" character varying(255) COLLATE pg_catalog."default",
	"Ta_Tag" character varying(100) COLLATE pg_catalog."default",
	 CONSTRAINT key_thirdpartyaccounts PRIMARY KEY ("Ta_ID")
);
-- 表 ThirdpartyAccounts 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ThirdpartyAccounts_Ta_ID_seq";
ALTER SEQUENCE IF EXISTS public."ThirdpartyAccounts_Ta_ID_seq" OWNED BY public."ThirdpartyAccounts"."Ta_ID";
ALTER TABLE "ThirdpartyAccounts" ALTER COLUMN "Ta_ID" SET DEFAULT NEXTVAL('"ThirdpartyAccounts_Ta_ID_seq"'::regclass);


-- 表 ThirdpartyAccounts 的索引 --
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ac_ID" ON public."ThirdpartyAccounts" USING btree ("Ac_ID" DESC);
CREATE UNIQUE INDEX IF NOT EXISTS "ThirdpartyAccounts_aaaaaThirdpartyAccounts_PK" ON public."ThirdpartyAccounts" USING btree ("Ta_ID");
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ta_Openid" ON public."ThirdpartyAccounts" USING btree ("Ta_Openid" DESC);

-- 创建表 ThirdpartyLogin --
CREATE TABLE IF NOT EXISTS public."ThirdpartyLogin"
(
	"Tl_ID" integer NOT NULL,
	"Tl_APPID" character varying(255) COLLATE pg_catalog."default",
	"Tl_Account" character varying(255) COLLATE pg_catalog."default",
	"Tl_Config" text,
	"Tl_Domain" character varying(500) COLLATE pg_catalog."default",
	"Tl_IsRegister" boolean NOT NULL,
	"Tl_IsUse" boolean NOT NULL,
	"Tl_Name" character varying(255) COLLATE pg_catalog."default",
	"Tl_Returl" character varying(1000) COLLATE pg_catalog."default",
	"Tl_Secret" character varying(2000) COLLATE pg_catalog."default",
	"Tl_Tag" character varying(255) COLLATE pg_catalog."default",
	"Tl_Tax" integer NOT NULL,
	 CONSTRAINT key_thirdpartylogin PRIMARY KEY ("Tl_ID")
);
-- 表 ThirdpartyLogin 的序列 --
CREATE SEQUENCE IF NOT EXISTS public."ThirdpartyLogin_Tl_ID_seq";
ALTER SEQUENCE IF EXISTS public."ThirdpartyLogin_Tl_ID_seq" OWNED BY public."ThirdpartyLogin"."Tl_ID";
ALTER TABLE "ThirdpartyLogin" ALTER COLUMN "Tl_ID" SET DEFAULT NEXTVAL('"ThirdpartyLogin_Tl_ID_seq"'::regclass);

INSERT INTO "ThirdpartyLogin"("Tl_ID","Tl_APPID","Tl_Account","Tl_Config","Tl_Domain","Tl_IsRegister","Tl_IsUse","Tl_Name","Tl_Returl","Tl_Secret","Tl_Tag","Tl_Tax") VALUES (1,'','','','',True,True,'QQ','','','QqOpenID',1);INSERT INTO "ThirdpartyLogin"("Tl_ID","Tl_APPID","Tl_Account","Tl_Config","Tl_Domain","Tl_IsRegister","Tl_IsUse","Tl_Name","Tl_Returl","Tl_Secret","Tl_Tag","Tl_Tax") VALUES (2,'','','','',True,True,'微信','','','WeixinOpenID',2);INSERT INTO "ThirdpartyLogin"("Tl_ID","Tl_APPID","Tl_Account","Tl_Config","Tl_Domain","Tl_IsRegister","Tl_IsUse","Tl_Name","Tl_Returl","Tl_Secret","Tl_Tag","Tl_Tax") VALUES (3,'','','','',True,True,'金蝶','','','Jindie',3);INSERT INTO "ThirdpartyLogin"("Tl_ID","Tl_APPID","Tl_Account","Tl_Config","Tl_Domain","Tl_IsRegister","Tl_IsUse","Tl_Name","Tl_Returl","Tl_Secret","Tl_Tag","Tl_Tax") VALUES (4,'','','','',True,False,'郑州工商','','','ZzGongshang',4);
-- 表 ThirdpartyLogin 的索引 --
CREATE UNIQUE INDEX IF NOT EXISTS "ThirdpartyLogin_aaaaaThirdpartyLogin_PK" ON public."ThirdpartyLogin" USING btree ("Tl_ID");


/*
	修正主键的自增ID的启始值为最大ID值

	原因：
		当从其它数据库转到Postgresql时，自增ID默认是从1开始，如果表中有历史数据
		在新增记录时，自增ID有可能与之前的记录重复，导致插入失败
		在Sqlite中不存在这个问题
*/
--利用游标，获取序列与所属表、字段
DO $$
DECLARE cur_name CURSOR FOR
	SELECT sequence_name,
	substring(sequence_name FROM '(\w+)_[A-Za-z]+_[A-Za-z]+_seq$') as tbname,
	substring(sequence_name FROM '[A-Za-z]+_([A-Za-z]+_[A-Za-z]+)_seq') as keyname
	FROM information_schema.sequences 
	order by sequence_name;
	sname VARCHAR(200);		--序列的名称
	tbname VARCHAR(200);	--表名
	mainkey VARCHAR(50);	--字段名
	tmsql VARCHAR(500);		
	maxid bigint;			--最大的自增id的值
BEGIN
OPEN cur_name;
	LOOP
	    FETCH cur_name INTO sname ,tbname, mainkey;
	   	EXIT WHEN NOT FOUND; -- 如果没有数据了，退出循环
		--获取当前表的最大自增字段的值
	    tmsql:='SELECT COALESCE(MAX("' || mainkey || '"),0) FROM "' || tbname || '"';
		EXECUTE tmsql INTO maxid; 	
		--RAISE NOTICE 'maxid: %, maxid:%', tmsql,maxid;

		--修改序列的启始值
		tmsql:='ALTER SEQUENCE "' || sname || '" RESTART WITH ' || (maxid+1);
		RAISE NOTICE 'sql: %', tmsql;
		EXECUTE tmsql; 
    
	END LOOP;
	CLOSE cur_name;
END $$;
