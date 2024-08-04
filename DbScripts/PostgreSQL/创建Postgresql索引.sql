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
CREATE INDEX IF NOT EXISTS "Accounts_IX_Ac_Name" ON "Accounts" ("Ac_Name" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_AccName" ON "Accounts" ("Ac_AccName" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Login" ON "Accounts" ("Ac_IsUse" DESC,"Ac_IsPass" DESC,"Ac_AccName" DESC,"Ac_Pw" DESC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Org_ID" ON "Accounts" ("Org_ID" ASC);
CREATE INDEX IF NOT EXISTS "Accounts_IX_Sts_ID" ON "Accounts" ("Sts_ID" DESC);
-- 3 . Article
-- 4 . Columns
CREATE INDEX IF NOT EXISTS "Columns_aaaaaColumns_PK" ON "Columns" ("Col_ID" DESC);
-- 5 . CouponAccount
-- 6 . Course
CREATE INDEX IF NOT EXISTS "Course_IX_Org_ID" ON "Course" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Course_IX_Sbj_ID" ON "Course" ("Sbj_ID" DESC);
-- 7 . CoursePrice
-- 8 . Depart
CREATE INDEX IF NOT EXISTS "Depart_aaaaaDepart_PK" ON "Depart" ("Dep_Id" DESC);
-- 9 . EmpAcc_Group
CREATE INDEX IF NOT EXISTS "EmpAcc_Group_aaaaaEmpAcc_Group_PK" ON "EmpAcc_Group" ("Emgr_Id" DESC);
-- 10 . EmpAccount
CREATE INDEX IF NOT EXISTS "EmpAccount_aaaaaEmpAccount_PK" ON "EmpAccount" ("Acc_Id" DESC);
-- 11 . EmpGroup
CREATE INDEX IF NOT EXISTS "EmpGroup_aaaaaEmpGroup_PK" ON "EmpGroup" ("EGrp_Id" DESC);
-- 12 . EmpTitle
CREATE INDEX IF NOT EXISTS "EmpTitle_aaaaaEmpTitle_PK" ON "EmpTitle" ("Title_Id" DESC);
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
-- 23 . LearningCardSet
-- 24 . LimitDomain
-- 25 . Links
CREATE INDEX IF NOT EXISTS "Links_aaaaaLinks_PK" ON "Links" ("Lk_Id" DESC);
-- 26 . LinksSort
CREATE INDEX IF NOT EXISTS "LinksSort_aaaaaLinksSort_PK" ON "LinksSort" ("Ls_Id" DESC);
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
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ac_ID" ON "MoneyAccount" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_CrtTime" ON "MoneyAccount" ("Ma_CrtTime" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_From" ON "MoneyAccount" ("Ma_From" DESC);
CREATE INDEX IF NOT EXISTS "MoneyAccount_IX_Ma_Type" ON "MoneyAccount" ("Ma_Type" DESC);
-- 37 . Navigation
CREATE INDEX IF NOT EXISTS "Navigation_aaaaaNavigation_PK" ON "Navigation" ("Nav_ID" DESC);
-- 38 . NewsNote
CREATE INDEX IF NOT EXISTS "NewsNote_aaaaaNewsNote_PK" ON "NewsNote" ("Nn_Id" DESC);
-- 39 . Notice
-- 40 . Organization
CREATE INDEX IF NOT EXISTS "Organization_aaaaaOrganization_PK" ON "Organization" ("Org_ID" DESC);
-- 41 . OrganLevel
-- 42 . Outline
CREATE INDEX IF NOT EXISTS "Outline_IX_Cou_ID" ON "Outline" ("Cou_ID" ASC,"Ol_Tax" DESC);
-- 43 . OutlineEvent
-- 44 . PayInterface
-- 45 . PointAccount
-- 46 . Position
CREATE INDEX IF NOT EXISTS "Position_aaaaaPosition_PK" ON "Position" ("Posi_Id" DESC);
-- 47 . ProfitSharing
-- 48 . Purview
CREATE INDEX IF NOT EXISTS "Purview_aaaaaPurview_PK" ON "Purview" ("Pur_Id" DESC);
-- 49 . QuesAnswer
-- 50 . Questions
CREATE INDEX IF NOT EXISTS "Questions_IX_Cou_ID" ON "Questions" ("Cou_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Ol_ID" ON "Questions" ("Ol_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Org_ID" ON "Questions" ("Org_ID" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Diff" ON "Questions" ("Qus_Diff" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_ID" ON "Questions" ("Qus_ID" ASC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Qus_Type" ON "Questions" ("Qus_Type" DESC);
CREATE INDEX IF NOT EXISTS "Questions_IX_Sbj_ID" ON "Questions" ("Sbj_ID" DESC);
-- 51 . QuesTypes
-- 52 . RechargeCode
-- 53 . RechargeSet
-- 54 . ShowPicture
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
-- 65 . StudentSort_Course
-- 66 . Subject
-- 67 . SystemPara
CREATE INDEX IF NOT EXISTS "SystemPara_aaaaaSystemPara_PK" ON "SystemPara" ("Sys_Id" DESC);
-- 68 . Teacher
-- 69 . Teacher_Course
-- 70 . TeacherComment
-- 71 . TeacherHistory
-- 72 . TeacherSort
-- 73 . TestPaper
CREATE INDEX IF NOT EXISTS "TestPaper_IX_Cou_ID" ON "TestPaper" ("Cou_ID" DESC);
-- 74 . TestPaperItem
CREATE INDEX IF NOT EXISTS "TestPaperItem_aaaaaTestPagerItem_PK" ON "TestPaperItem" ("TPI_ID" DESC);
-- 75 . TestPaperQues
CREATE INDEX IF NOT EXISTS "TestPaperQues_aaaaaTestPaperQues_PK" ON "TestPaperQues" ("Tq_Id" DESC);
-- 76 . TestResults
CREATE INDEX IF NOT EXISTS "TestResults_aaaaaTestResults_PK" ON "TestResults" ("Tr_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Ac_ID" ON "TestResults" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Cou_ID" ON "TestResults" ("Cou_ID" DESC);
CREATE INDEX IF NOT EXISTS "TestResults_IX_Tp_Id" ON "TestResults" ("Tp_Id" DESC);
-- 77 . ThirdpartyAccounts
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_aaaaaThirdpartyAccounts_PK" ON "ThirdpartyAccounts" ("Ta_ID" DESC);
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ac_ID" ON "ThirdpartyAccounts" ("Ac_ID" DESC);
CREATE INDEX IF NOT EXISTS "ThirdpartyAccounts_IX_Ta_Openid" ON "ThirdpartyAccounts" ("Ta_Openid" DESC);
-- 78 . ThirdpartyLogin
CREATE INDEX IF NOT EXISTS "ThirdpartyLogin_aaaaaThirdpartyLogin_PK" ON "ThirdpartyLogin" ("Tl_ID" DESC);
--总共索引有：114
