-- 1 . Accessory
CREATE INDEX IF NOT EXISTS "Accessory_aaaaaAccessory_PK" ON "Accessory" ("As_Id" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_As_Type" ON "Accessory" ("As_Type" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_As_Uid_Type" ON "Accessory" ("As_Uid" DESC,"As_Type" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_IX_As_Uid" ON "Accessory" ("As_Uid" DESC);
CREATE INDEX IF NOT EXISTS "Accessory_IX_Org_ID" ON "Accessory" ("Org_ID" ASC);
-- 2 . Accounts
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Birthday" ON "Accounts" ("Ac_Birthday" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_IDCardNumber" ON "Accounts" ("Ac_IDCardNumber" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_LastTime" ON "Accounts" ("Ac_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_MobiTel1" ON "Accounts" ("Ac_MobiTel1" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Money" ON "Accounts" ("Ac_Money" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Name" ON "Accounts" ("Ac_Name" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Sex" ON "Accounts" ("Ac_Sex" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_AccName" ON "Accounts" ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Login" ON "Accounts" ("Ac_IsUse" DESC,"Ac_IsPass" DESC,"Ac_AccName" DESC,"Ac_Pw" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Org_ID" ON "Accounts" ("Org_ID" ASC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Sts_ID" ON "Accounts" ("Sts_ID" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Sts_Name" ON "Accounts" ("Sts_Name" DESC);
-- 3 . Article
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsHot" ON "Article" ("Art_IsHot" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsImg" ON "Article" ("Art_IsImg" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsRec" ON "Article" ("Art_IsRec" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsTop" ON "Article" ("Art_IsTop" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsUse" ON "Article" ("Art_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_IsVerify" ON "Article" ("Art_IsVerify" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_Number" ON "Article" ("Art_Number" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Art_Title" ON "Article" ("Art_Title" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Col_UID" ON "Article" ("Col_UID" DESC);
CREATE INDEX IF NOT EXISTS "Article_IX_Org_ID" ON "Article" ("Org_ID" DESC);
-- 4 . Columns
CREATE INDEX IF NOT EXISTS "Columns_aaaaaColumns_PK" ON "Columns" ("Col_ID" DESC);
CREATE INDEX IF NOT EXISTS "Columns_IX_Col_IsUse" ON "Columns" ("Col_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Columns_IX_Col_PID" ON "Columns" ("Col_PID" DESC);
CREATE INDEX IF NOT EXISTS "Columns_IX_Col_Tax" ON "Columns" ("Col_Tax" DESC);
CREATE INDEX IF NOT EXISTS "Columns_IX_Col_Type" ON "Columns" ("Col_Type" DESC);
CREATE INDEX IF NOT EXISTS "Columns_IX_Org_ID" ON "Columns" ("Org_ID" DESC);
-- 5 . CouponAccount
CREATE INDEX IF NOT EXISTS "CouponAccount_IX_Ac_ID" ON "CouponAccount" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "CouponAccount_IX_Ca_CrtTime" ON "CouponAccount" ("Ca_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "CouponAccount_IX_Ca_Type" ON "CouponAccount" ("Ca_Type" DESC);
CREATE INDEX IF NOT EXISTS "CouponAccount_IX_Org_ID" ON "CouponAccount" ("Org_ID" DESC);
-- 6 . Course
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_CrtTime" ON "Course" ("Cou_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_ExistLive" ON "Course" ("Cou_ExistLive" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_IsFree" ON "Course" ("Cou_IsFree" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_IsRec" ON "Course" ("Cou_IsRec" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_IsUse" ON "Course" ("Cou_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_Name" ON "Course" ("Cou_Name" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_Tax" ON "Course" ("Cou_Tax" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_UID" ON "Course" ("Cou_UID" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Cou_ViewNum" ON "Course" ("Cou_ViewNum" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Org_ID" ON "Course" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Sbj_ID" ON "Course" ("Sbj_ID" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Th_ID" ON "Course" ("Th_ID" DESC);
-- 7 . CoursePrice
CREATE INDEX IF NOT EXISTS "CoursePrice_IX_Cou_ID" ON "CoursePrice" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "CoursePrice_IX_Cou_UID" ON "CoursePrice" ("Cou_UID" DESC);
CREATE INDEX IF NOT EXISTS "CoursePrice_IX_CP_IsUse" ON "CoursePrice" ("CP_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "CoursePrice_IX_CP_Tax" ON "CoursePrice" ("CP_Tax" DESC);
-- 8 . Depart
CREATE INDEX IF NOT EXISTS "Depart_aaaaaDepart_PK" ON "Depart" ("Dep_Id" DESC);
-- 9 . EmpAcc_Group
CREATE INDEX IF NOT EXISTS "EmpAcc_Group_aaaaaEmpAcc_Group_PK" ON "EmpAcc_Group" ("Emgr_Id" DESC);
-- 10 . EmpAccount
CREATE INDEX IF NOT EXISTS "EmpAccount_aaaaaEmpAccount_PK" ON "EmpAccount" ("Acc_Id" DESC);
CREATE INDEX IF NOT EXISTS "EmpAccount_IX_Acc_IsUse" ON "EmpAccount" ("Acc_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "EmpAccount_IX_Acc_Name" ON "EmpAccount" ("Acc_Name" DESC);
CREATE INDEX IF NOT EXISTS "EmpAccount_IX_Acc_RegTime" ON "EmpAccount" ("Acc_RegTime" DESC);
CREATE INDEX IF NOT EXISTS "EmpAccount_IX_Org_ID" ON "EmpAccount" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "EmpAccount_IX_Posi_Id" ON "EmpAccount" ("Posi_Id" DESC);
-- 11 . EmpGroup
CREATE INDEX IF NOT EXISTS "EmpGroup_aaaaaEmpGroup_PK" ON "EmpGroup" ("EGrp_Id" DESC);
-- 12 . EmpTitle
CREATE INDEX IF NOT EXISTS "EmpTitle_aaaaaEmpTitle_PK" ON "EmpTitle" ("Title_Id" DESC);
CREATE INDEX IF NOT EXISTS "EmpTitle_IX_Org_ID" ON "EmpTitle" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "EmpTitle_IX_Title_IsUse" ON "EmpTitle" ("Title_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "EmpTitle_IX_Title_Name" ON "EmpTitle" ("Title_Name" DESC);
CREATE INDEX IF NOT EXISTS "EmpTitle_IX_Title_Tax" ON "EmpTitle" ("Title_Tax" DESC);
-- 13 . ExamGroup
CREATE INDEX IF NOT EXISTS "ExamGroup_aaaaaExamGroup_PK" ON "ExamGroup" ("Eg_ID" DESC);
-- 14 . Examination
CREATE INDEX IF NOT EXISTS "Examination_aaaaaExamination_PK" ON "Examination" ("Exam_ID" DESC);
-- 15 . ExamResults
CREATE INDEX IF NOT EXISTS "ExamResults_aaaaaExamResults_PK" ON "ExamResults" ("Exr_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Ac_ID" ON "ExamResults" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Exam_ID" ON "ExamResults" ("Exam_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Exr_CrtTime" ON "ExamResults" ("Exr_CrtTime" ASC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_IsSubmit" ON "ExamResults" ("Exr_IsSubmit" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Org_ID" ON "ExamResults" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_OverTime" ON "ExamResults" ("Exr_OverTime" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Score" ON "ExamResults" ("Exr_ScoreFinal" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Sts_ID" ON "ExamResults" ("Sts_ID" DESC);
CREATE INDEX IF NOT EXISTS "ExamResults_IX_Tp_Id" ON "ExamResults" ("Tp_Id" DESC);
-- 16 . FuncPoint
CREATE INDEX IF NOT EXISTS "FuncPoint_aaaaaFuncPoint_PK" ON "FuncPoint" ("FPI_Id" DESC);
-- 17 . Guide
CREATE INDEX IF NOT EXISTS "Guide_IX_Cou_ID" ON "Guide" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Guide_IX_IsShow" ON "Guide" ("Gu_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Guide_IX_IsUse" ON "Guide" ("Gu_IsUse" DESC);
-- 18 . GuideColumns
CREATE INDEX IF NOT EXISTS "GuideColumns_aaaaaGuideColumns_PK" ON "GuideColumns" ("Gc_ID" DESC);
-- 19 . InternalLink
CREATE INDEX IF NOT EXISTS "InternalLink_aaaaaInternalLink_PK" ON "InternalLink" ("IL_ID" DESC);
-- 20 . Knowledge
-- 21 . KnowledgeSort
-- 22 . LearningCard
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Ac_AccName" ON "LearningCard" ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Ac_ID" ON "LearningCard" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_Code" ON "LearningCard" ("Lc_Code" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_IsUsed" ON "LearningCard" ("Lc_IsUsed" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_Pw" ON "LearningCard" ("Lc_Pw" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lc_State" ON "LearningCard" ("Lc_State" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Lcs_ID" ON "LearningCard" ("Lcs_ID" DESC);
CREATE INDEX IF NOT EXISTS "LearningCard_IX_Org_ID" ON "LearningCard" ("Org_ID" DESC);
-- 23 . LearningCardSet
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_Count" ON "LearningCardSet" ("Lcs_Count" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_CoursesCount" ON "LearningCardSet" ("Lcs_CoursesCount" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_CrtTime" ON "LearningCardSet" ("Lcs_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_IsEnable" ON "LearningCardSet" ("Lcs_IsEnable" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_Price" ON "LearningCardSet" ("Lcs_Price" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lcs_Theme" ON "LearningCardSet" ("Lcs_Theme" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Lsc_UsedCount" ON "LearningCardSet" ("Lsc_UsedCount" DESC);
CREATE INDEX IF NOT EXISTS "LearningCardSet_IX_Org_ID" ON "LearningCardSet" ("Org_ID" DESC);
-- 24 . LimitDomain
-- 25 . Links
CREATE INDEX IF NOT EXISTS "Links_aaaaaLinks_PK" ON "Links" ("Lk_Id" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Lk_IsShow" ON "Links" ("Lk_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Lk_IsUse" ON "Links" ("Lk_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Lk_Name" ON "Links" ("Lk_Name" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Lk_Tax" ON "Links" ("Lk_Tax" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Lk_Url" ON "Links" ("Lk_Url" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Ls_Id" ON "Links" ("Ls_Id" DESC);
CREATE INDEX IF NOT EXISTS "Links_IX_Org_ID" ON "Links" ("Org_ID" DESC);
-- 26 . LinksSort
CREATE INDEX IF NOT EXISTS "LinksSort_aaaaaLinksSort_PK" ON "LinksSort" ("Ls_Id" DESC);
CREATE INDEX IF NOT EXISTS "LinksSort_IX_Ls_IsShow" ON "LinksSort" ("Ls_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "LinksSort_IX_Ls_IsUse" ON "LinksSort" ("Ls_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "LinksSort_IX_Ls_Name" ON "LinksSort" ("Ls_Name" DESC);
CREATE INDEX IF NOT EXISTS "LinksSort_IX_Ls_Tax" ON "LinksSort" ("Ls_Tax" DESC);
CREATE INDEX IF NOT EXISTS "LinksSort_IX_Org_ID" ON "LinksSort" ("Org_ID" DESC);
-- 27 . LogForStudentExercise
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Ac_ID" ON "LogForStudentExercise" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Cou_ID" ON "LogForStudentExercise" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Lse_LastTime" ON "LogForStudentExercise" ("Lse_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentExercise_IX_Ol_ID" ON "LogForStudentExercise" ("Ol_ID" DESC);
-- 28 . LogForStudentOnline
CREATE INDEX IF NOT EXISTS "LogForStudentOnline_IX_Ac_ID" ON "LogForStudentOnline" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentOnline_IX_CrtTime" ON "LogForStudentOnline" ("Lso_CrtTime" DESC);
-- 29 . LogForStudentQuestions
-- 30 . LogForStudentStudy
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Ac_Ol_Cou" ON "LogForStudentStudy" ("Ac_ID" ASC,"Ol_ID" ASC,"Cou_ID" ASC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Cou_ID" ON "LogForStudentStudy" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Lss_LastTime" ON "LogForStudentStudy" ("Lss_LastTime" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Ol_ID" ON "LogForStudentStudy" ("Ol_ID" DESC);
CREATE INDEX IF NOT EXISTS "LogForStudentStudy_IX_Org_ID" ON "LogForStudentStudy" ("Org_ID" DESC);
-- 31 . Logs
CREATE INDEX IF NOT EXISTS "Logs_aaaaaLogs_PK" ON "Logs" ("Log_Id" DESC);
-- 32 . ManageMenu
CREATE INDEX IF NOT EXISTS "ManageMenu_aaaaaManageMenu_PK" ON "ManageMenu" ("MM_Id" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_Func" ON "ManageMenu" ("MM_Func" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_IsShow" ON "ManageMenu" ("MM_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_IsUse" ON "ManageMenu" ("MM_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_PatId" ON "ManageMenu" ("MM_PatId" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_Tax" ON "ManageMenu" ("MM_Tax" DESC);
CREATE INDEX IF NOT EXISTS "ManageMenu_IX_MM_UID" ON "ManageMenu" ("MM_UID" DESC);
-- 33 . ManageMenu_Point
CREATE INDEX IF NOT EXISTS "ManageMenu_Point_aaaaaManageMenu_Point_PK" ON "ManageMenu_Point" ("MMP_Id" DESC);
-- 34 . Message
CREATE INDEX IF NOT EXISTS "Message_aaaaaMessage_PK" ON "Message" ("Msg_Id" DESC);
-- 35 . MessageBoard
CREATE INDEX IF NOT EXISTS "MessageBoard_aaaaaMessageBoard_PK" ON "MessageBoard" ("Mb_Id" DESC);
-- 36 . MoneyAccount
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ac_AccName" ON "MoneyAccount" ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ac_ID" ON "MoneyAccount" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ac_Name" ON "MoneyAccount" ("Ac_Name" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_CrtTime" ON "MoneyAccount" ("Ma_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_From" ON "MoneyAccount" ("Ma_From" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_IsSuccess" ON "MoneyAccount" ("Ma_IsSuccess" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_Money" ON "MoneyAccount" ("Ma_Money" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_Serial" ON "MoneyAccount" ("Ma_Serial" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_Type" ON "MoneyAccount" ("Ma_Type" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Org_ID" ON "MoneyAccount" ("Org_ID" DESC);
-- 37 . Navigation
CREATE INDEX IF NOT EXISTS "Navigation_aaaaaNavigation_PK" ON "Navigation" ("Nav_ID" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_IX_Nav_IsShow" ON "Navigation" ("Nav_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_IX_Nav_Site" ON "Navigation" ("Nav_Site" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_IX_Nav_Type" ON "Navigation" ("Nav_Type" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_IX_Org_ID" ON "Navigation" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_Nav_PID" ON "Navigation" ("Nav_PID" DESC);
CREATE INDEX IF NOT EXISTS "Navigation_Nav_Tax" ON "Navigation" ("Nav_Tax" DESC);
-- 38 . NewsNote
CREATE INDEX IF NOT EXISTS "NewsNote_aaaaaNewsNote_PK" ON "NewsNote" ("Nn_Id" DESC);
-- 39 . Notice
CREATE INDEX IF NOT EXISTS "Notice_IX_No_EndTime" ON "Notice" ("No_EndTime" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_IsShow" ON "Notice" ("No_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_IsTop" ON "Notice" ("No_IsTop" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_Page" ON "Notice" ("No_Page" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_StartTime" ON "Notice" ("No_StartTime" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_Ttl" ON "Notice" ("No_Ttl" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_No_Type" ON "Notice" ("No_Type" DESC);
CREATE INDEX IF NOT EXISTS "Notice_IX_Org_ID" ON "Notice" ("Org_ID" DESC);
-- 40 . Organization
CREATE INDEX IF NOT EXISTS "Organization_aaaaaOrganization_PK" ON "Organization" ("Org_ID" DESC);
-- 41 . OrganLevel
-- 42 . Outline
CREATE INDEX IF NOT EXISTS "Outline_IX_Cou_ID" ON "Outline" ("Cou_ID" ASC,"Ol_Tax" DESC);
CREATE INDEX IF NOT EXISTS "Outline_IX_Cou_ID2" ON "Outline" ("Cou_ID" ASC);
CREATE INDEX IF NOT EXISTS "Outline_IX_Ol_IsUse" ON "Outline" ("Ol_IsUse" ASC);
-- 43 . OutlineEvent
-- 44 . PayInterface
-- 45 . PointAccount
CREATE INDEX IF NOT EXISTS "PointAccount_IX_Ac_ID" ON "PointAccount" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "PointAccount_IX_Org_ID" ON "PointAccount" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "PointAccount_IX_Pa_CrtTime" ON "PointAccount" ("Pa_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "PointAccount_IX_Pa_Info" ON "PointAccount" ("Pa_Info" DESC);
CREATE INDEX IF NOT EXISTS "PointAccount_IX_Pa_Type" ON "PointAccount" ("Pa_Type" DESC);
-- 46 . Position
CREATE INDEX IF NOT EXISTS "Position_aaaaaPosition_PK" ON "Position" ("Posi_Id" DESC);
CREATE INDEX IF NOT EXISTS "Position_IX_Org_ID" ON "Position" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Position_IX_Posi_IsUse" ON "Position" ("Posi_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Position_IX_Posi_Tax" ON "Position" ("Posi_Tax" DESC);
-- 47 . ProfitSharing
-- 48 . Purview
CREATE INDEX IF NOT EXISTS "Purview_aaaaaPurview_PK" ON "Purview" ("Pur_Id" DESC);
CREATE INDEX IF NOT EXISTS "Purview_IX_Olv_ID" ON "Purview" ("Olv_ID" DESC);
-- 49 . QuesAnswer
-- 50 . Questions
CREATE INDEX IF NOT EXISTS "Questions_IX_Cou_ID" ON "Questions" ("Cou_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Ol_ID" ON "Questions" ("Ol_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Org_ID" ON "Questions" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Diff" ON "Questions" ("Qus_Diff" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_ID" ON "Questions" ("Qus_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_IsUse" ON "Questions" ("Qus_IsUse" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Type" ON "Questions" ("Qus_Type" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Sbj_ID" ON "Questions" ("Sbj_ID" DESC);
-- 51 . QuesTypes
-- 52 . RechargeCode
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Ac_AccName" ON "RechargeCode" ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Org_ID" ON "RechargeCode" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_Code" ON "RechargeCode" ("Rc_Code" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_CrtTime" ON "RechargeCode" ("Rc_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_IsEnable" ON "RechargeCode" ("Rc_IsEnable" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_IsUsed" ON "RechargeCode" ("Rc_IsUsed" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_Pw" ON "RechargeCode" ("Rc_Pw" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rc_UsedTime" ON "RechargeCode" ("Rc_UsedTime" DESC);
CREATE INDEX IF NOT EXISTS "RechargeCode_IX_Rs_ID" ON "RechargeCode" ("Rs_ID" DESC);
-- 53 . RechargeSet
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Org_ID" ON "RechargeSet" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_Count" ON "RechargeSet" ("Rs_Count" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_CrtTime" ON "RechargeSet" ("Rs_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_Intro" ON "RechargeSet" ("Rs_Intro" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_IsEnable" ON "RechargeSet" ("Rs_IsEnable" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_Price" ON "RechargeSet" ("Rs_Price" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_Theme" ON "RechargeSet" ("Rs_Theme" DESC);
CREATE INDEX IF NOT EXISTS "RechargeSet_IX_Rs_UsedCount" ON "RechargeSet" ("Rs_UsedCount" DESC);
-- 54 . ShowPicture
CREATE INDEX IF NOT EXISTS "ShowPicture_IX_Org_ID" ON "ShowPicture" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "ShowPicture_IX_Shp_IsShow" ON "ShowPicture" ("Shp_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "ShowPicture_IX_Shp_Site" ON "ShowPicture" ("Shp_Site" DESC);
CREATE INDEX IF NOT EXISTS "ShowPicture_IX_Shp_Tax" ON "ShowPicture" ("Shp_Tax" DESC);
-- 55 . SingleSignOn
-- 56 . SmsFault
CREATE INDEX IF NOT EXISTS "SmsFault_aaaaaSmsFault_PK" ON "SmsFault" ("Smf_Id" DESC);
-- 57 . SmsMessage
CREATE INDEX IF NOT EXISTS "SmsMessage_aaaaaSmsMessage_PK" ON "SmsMessage" ("SMS_Id" DESC);
-- 58 . Special
CREATE INDEX IF NOT EXISTS "Special_aaaaaSpecial_PK" ON "Special" ("Sp_Id" DESC);
-- 59 . Special_Article
CREATE INDEX IF NOT EXISTS "Special_Article_aaaaaSpecial_Article_PK" ON "Special_Article" ("Spa_Id" DESC);
-- 60 . Student_Collect
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Ac_ID" ON "Student_Collect" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Cou_ID" ON "Student_Collect" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Collect_IX_Qus_ID" ON "Student_Collect" ("Qus_ID" DESC);
-- 61 . Student_Course
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Ac_ID" ON "Student_Course" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Cou_ID" ON "Student_Course" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Lc_Code" ON "Student_Course" ("Lc_Code" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Org_ID" ON "Student_Course" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_EndTime" ON "Student_Course" ("Stc_EndTime" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_StartTime" ON "Student_Course" ("Stc_StartTime" DESC);
CREATE INDEX IF NOT EXISTS "Student_Course_IX_Stc_Type" ON "Student_Course" ("Stc_Type" DESC);
-- 62 . Student_Notes
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Ac_ID" ON "Student_Notes" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Cou_ID" ON "Student_Notes" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "Student_Notes_IX_Qus_ID" ON "Student_Notes" ("Qus_ID" DESC);
-- 63 . Student_Ques
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Ac_ID" ON "Student_Ques" ("Ac_ID" ASC);
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Cou_ID" ON "Student_Ques" ("Cou_ID" ASC);
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_Diff" ON "Student_Ques" ("Qus_Diff" DESC);
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_ID" ON "Student_Ques" ("Qus_ID" ASC);
CREATE INDEX IF NOT EXISTS "Student_Ques_IX_Qus_Type" ON "Student_Ques" ("Qus_Type" DESC);
-- 64 . StudentSort
CREATE INDEX IF NOT EXISTS "StudentSort_IX_Org_ID" ON "StudentSort" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "StudentSort_IX_Sts_IsUse" ON "StudentSort" ("Sts_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "StudentSort_IX_Sts_Name" ON "StudentSort" ("Sts_Name" DESC);
CREATE INDEX IF NOT EXISTS "StudentSort_IX_Sts_Tax" ON "StudentSort" ("Sts_Tax" DESC);
-- 65 . StudentSort_Course
CREATE INDEX IF NOT EXISTS "StudentSort_Course_IX_Cou_ID" ON "StudentSort_Course" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "StudentSort_Course_IX_Ssc_IsEnable" ON "StudentSort_Course" ("Ssc_IsEnable" DESC);
CREATE INDEX IF NOT EXISTS "StudentSort_Course_IX_Sts_ID" ON "StudentSort_Course" ("Sts_ID" DESC);
-- 66 . Subject
CREATE INDEX IF NOT EXISTS "Subject_IX_Org_ID" ON "Subject" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Subject_IX_Sbj_IsRec" ON "Subject" ("Sbj_IsRec" DESC);
CREATE INDEX IF NOT EXISTS "Subject_IX_Sbj_IsUse" ON "Subject" ("Sbj_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Subject_IX_Sbj_Name" ON "Subject" ("Sbj_Name" DESC);
CREATE INDEX IF NOT EXISTS "Subject_IX_Sbj_Tax" ON "Subject" ("Sbj_Tax" DESC);
-- 67 . SystemPara
CREATE INDEX IF NOT EXISTS "SystemPara_aaaaaSystemPara_PK" ON "SystemPara" ("Sys_Id" DESC);
-- 68 . Teacher
CREATE INDEX IF NOT EXISTS "Teacher_IX_Org_ID" ON "Teacher" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_AccName" ON "Teacher" ("Th_AccName" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_IDCardNumber" ON "Teacher" ("Th_IDCardNumber" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_IsShow" ON "Teacher" ("Th_IsShow" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_IsUse" ON "Teacher" ("Th_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_Name" ON "Teacher" ("Th_Name" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_Phone" ON "Teacher" ("Th_Phone" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_PhoneMobi" ON "Teacher" ("Th_PhoneMobi" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_Pinyin" ON "Teacher" ("Th_Pinyin" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Th_Sex" ON "Teacher" ("Th_Sex" DESC);
CREATE INDEX IF NOT EXISTS "Teacher_IX_Ths_ID" ON "Teacher" ("Ths_ID" DESC);
-- 69 . Teacher_Course
-- 70 . TeacherComment
-- 71 . TeacherHistory
-- 72 . TeacherSort
CREATE INDEX IF NOT EXISTS "TeacherSort_IX_Org_ID" ON "TeacherSort" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "TeacherSort_IX_Ths_IsUse" ON "TeacherSort" ("Ths_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "TeacherSort_IX_Ths_Name" ON "TeacherSort" ("Ths_Name" DESC);
CREATE INDEX IF NOT EXISTS "TeacherSort_IX_Ths_Tax" ON "TeacherSort" ("Ths_Tax" DESC);
-- 73 . TestPaper
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Cou_ID" ON "TestPaper" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Org_ID" ON "TestPaper" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Sbj_ID" ON "TestPaper" ("Sbj_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Tp_CrtTime" ON "TestPaper" ("Tp_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Tp_Diff" ON "TestPaper" ("Tp_Diff" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Tp_IsUse" ON "TestPaper" ("Tp_IsUse" DESC);
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Tp_Name" ON "TestPaper" ("Tp_Name" DESC);
-- 74 . TestPaperItem
CREATE INDEX IF NOT EXISTS "TestPaperItem_aaaaaTestPagerItem_PK" ON "TestPaperItem" ("TPI_ID" DESC);
-- 75 . TestPaperQues
CREATE INDEX IF NOT EXISTS "TestPaperQues_aaaaaTestPaperQues_PK" ON "TestPaperQues" ("Tq_Id" DESC);
-- 76 . TestResults
CREATE INDEX IF NOT EXISTS "TestResults_aaaaaTestResults_PK" ON "TestResults" ("Tr_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Ac_ID" ON "TestResults" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Cou_ID" ON "TestResults" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Tp_Id" ON "TestResults" ("Tp_Id" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Tr_CrtTime" ON "TestResults" ("Tr_CrtTime" DESC);
-- 77 . ThirdpartyAccounts
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_aaaaaThirdpartyAccounts_PK" ON "ThirdpartyAccounts" ("Ta_ID" DESC);
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ac_ID" ON "ThirdpartyAccounts" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ta_Openid" ON "ThirdpartyAccounts" ("Ta_Openid" DESC);
-- 78 . ThirdpartyLogin
CREATE INDEX IF NOT EXISTS "ThirdpartyLogin_aaaaaThirdpartyLogin_PK" ON "ThirdpartyLogin" ("Tl_ID" DESC);
--总共索引有：275
