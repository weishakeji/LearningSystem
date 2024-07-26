
/*将real类型，转为 float*/
ALTER TABLE ExamResults ALTER COLUMN Exr_Score float;
ALTER TABLE ExamResults ALTER COLUMN Exr_ScoreFinal float;
ALTER TABLE ExamResults ALTER COLUMN Exr_Draw float;
ALTER TABLE ExamResults ALTER COLUMN Exr_Colligate float;
ALTER TABLE LearningCard ALTER COLUMN Lc_Price float;
ALTER TABLE LearningCardSet ALTER COLUMN Lcs_Price float;
ALTER TABLE PayInterface ALTER COLUMN Pai_Feerate float;
ALTER TABLE Questions ALTER COLUMN Qus_Number float;
ALTER TABLE Student_Course ALTER COLUMN Stc_Money float;
ALTER TABLE TestPaperQues ALTER COLUMN Tq_Number float;
ALTER TABLE TestResults ALTER COLUMN Tr_Score float;
ALTER TABLE TestResults ALTER COLUMN Tr_ScoreFinal float;
ALTER TABLE TestResults ALTER COLUMN Tr_Draw float;
ALTER TABLE TestResults ALTER COLUMN Tr_Colligate float;

/*float类型，全部设置为不可为空*/
update ExamResults set Exr_Score=0 where Exr_Score IS NULL;
ALTER TABLE ExamResults ALTER COLUMN Exr_Score float not null;
--
update ExamResults set Exr_ScoreFinal=0 where Exr_ScoreFinal IS NULL;
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[ExamResults]') AND name = N'IX_Score')
DROP INDEX [IX_Score] ON [dbo].[ExamResults] WITH ( ONLINE = OFF )
GO
ALTER TABLE ExamResults ALTER COLUMN Exr_ScoreFinal float not null;
/****** Object:  Index [IX_Score]    Script Date: 07/25/2024 17:13:11 ******/
CREATE NONCLUSTERED INDEX [IX_Score] ON [dbo].[ExamResults] 
(
	[Exr_ScoreFinal] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--
update ExamResults set Exr_Draw=0 where Exr_Draw IS NULL;
ALTER TABLE ExamResults ALTER COLUMN Exr_Draw float not null;
--
update ExamResults set Exr_Colligate=0 where Exr_Colligate IS NULL;
ALTER TABLE ExamResults ALTER COLUMN Exr_Colligate float not null;
--
update LearningCard set Lc_Price=0 where Lc_Price IS NULL;
ALTER TABLE LearningCard ALTER COLUMN Lc_Price float not null;
--
update LearningCardSet set Lcs_Price=0 where Lcs_Price IS NULL;
ALTER TABLE LearningCardSet ALTER COLUMN Lcs_Price float not null;
--
update PayInterface set Pai_Feerate=0 where Pai_Feerate IS NULL;
ALTER TABLE PayInterface ALTER COLUMN Pai_Feerate float not null;
--
update Questions set Qus_Number=0 where Qus_Number IS NULL;
ALTER TABLE Questions ALTER COLUMN Qus_Number float not null;
--
update Student_Course set Stc_Money=0 where Stc_Money IS NULL;
ALTER TABLE Student_Course ALTER COLUMN Stc_Money float not null;
--
update TestPaperQues set Tq_Number=0 where Tq_Number IS NULL;
ALTER TABLE TestPaperQues ALTER COLUMN Tq_Number float not null;
--
update TestResults set Tr_Score=0 where Tr_Score IS NULL;
ALTER TABLE TestResults ALTER COLUMN Tr_Score float not null;
--
update TestResults set Tr_ScoreFinal=0 where Tr_ScoreFinal IS NULL;
ALTER TABLE TestResults ALTER COLUMN Tr_ScoreFinal float not null;
--
update TestResults set Tr_Draw=0 where Tr_Draw IS NULL;
ALTER TABLE TestResults ALTER COLUMN Tr_Draw float not null;
--
update TestResults set Tr_Colligate=0 where Tr_Colligate IS NULL;
ALTER TABLE TestResults ALTER COLUMN Tr_Colligate float not null;
--

/*smallint类型，转int*/
ALTER TABLE ManageMenu_Point ALTER COLUMN MM_Id int;
ALTER TABLE ManageMenu_Point ALTER COLUMN FPI_Id int;

/*所有int，设置为不可为空*/
update EmpAcc_Group set Acc_Id=0 where Acc_Id IS NULL;
ALTER TABLE EmpAcc_Group ALTER COLUMN Acc_Id int not null;
--
update EmpAcc_Group set EGrp_Id=0 where EGrp_Id IS NULL;
ALTER TABLE EmpAcc_Group ALTER COLUMN EGrp_Id int not null;
--
update EmpAcc_Group set Org_Id=0 where Org_Id IS NULL;
ALTER TABLE EmpAcc_Group ALTER COLUMN Org_Id int not null;
--
update EmpTitle set Title_Tax=0 where Title_Tax IS NULL;
ALTER TABLE EmpTitle ALTER COLUMN Title_Tax int not null;
--
update ExamGroup set Eg_Type=0 where Eg_Type IS NULL;
ALTER TABLE ExamGroup ALTER COLUMN Eg_Type int not null;
--
update FuncPoint set Org_Id=0 where Org_Id IS NULL;
ALTER TABLE FuncPoint ALTER COLUMN Org_Id int not null;
--
update Guide set Acc_Id=0 where Acc_Id IS NULL;
ALTER TABLE Guide ALTER COLUMN Acc_Id int not null;
--
update Links set Ls_Id=0 where Ls_Id IS NULL;
ALTER TABLE Links ALTER COLUMN Ls_Id int not null;
--
update Links set Lk_Tax=0 where Lk_Tax IS NULL;
ALTER TABLE Links ALTER COLUMN Lk_Tax int not null;
--
update LinksSort set Ls_PatId=0 where Ls_PatId IS NULL;
ALTER TABLE LinksSort ALTER COLUMN Ls_PatId int not null;
--
update LinksSort set Ls_Tax=0 where Ls_Tax IS NULL;
ALTER TABLE LinksSort ALTER COLUMN Ls_Tax int not null;
--
update Logs set Acc_Id=0 where Acc_Id IS NULL;
ALTER TABLE Logs ALTER COLUMN Acc_Id int not null;
--
update Logs set Log_MenuId=0 where Log_MenuId IS NULL;
ALTER TABLE Logs ALTER COLUMN Log_MenuId int not null;
--
update ManageMenu_Point set MM_Id=0 where MM_Id IS NULL;
ALTER TABLE ManageMenu_Point ALTER COLUMN MM_Id int not null;
--
update ManageMenu_Point set FPI_Id=0 where FPI_Id IS NULL;
ALTER TABLE ManageMenu_Point ALTER COLUMN FPI_Id int not null;
--
update Message set Msg_State=0 where Msg_State IS NULL;
ALTER TABLE Message ALTER COLUMN Msg_State int not null;
--
update Message set Org_Id=0 where Org_Id IS NULL;
ALTER TABLE Message ALTER COLUMN Org_Id int not null;
--
update MessageBoard set Mb_PID=0 where Mb_PID IS NULL;
ALTER TABLE MessageBoard ALTER COLUMN Mb_PID int not null;
--
update MessageBoard set Mb_At=0 where Mb_At IS NULL;
ALTER TABLE MessageBoard ALTER COLUMN Mb_At int not null;
--
update MessageBoard set Mb_FluxNumber=0 where Mb_FluxNumber IS NULL;
ALTER TABLE MessageBoard ALTER COLUMN Mb_FluxNumber int not null;
--
update MessageBoard set Mb_ReplyNumber=0 where Mb_ReplyNumber IS NULL;
ALTER TABLE MessageBoard ALTER COLUMN Mb_ReplyNumber int not null;
--
update Notice set Acc_Id=0 where Acc_Id IS NULL;
ALTER TABLE Notice ALTER COLUMN Acc_Id int not null;
--
update SmsMessage set Sms_Type=0 where Sms_Type IS NULL;
ALTER TABLE SmsMessage ALTER COLUMN Sms_Type int not null;
--
update SmsMessage set Sms_SendId=0 where Sms_SendId IS NULL;
ALTER TABLE SmsMessage ALTER COLUMN Sms_SendId int not null;
--
update SmsMessage set Sms_MailBox=0 where Sms_MailBox IS NULL;
ALTER TABLE SmsMessage ALTER COLUMN Sms_MailBox int not null;
--
update SmsMessage set Sms_State=0 where Sms_State IS NULL;
ALTER TABLE SmsMessage ALTER COLUMN Sms_State int not null;
--
update Special set Sp_PatId=0 where Sp_PatId IS NULL;
ALTER TABLE Special ALTER COLUMN Sp_PatId int not null;
--
update Special set Sp_Tax=0 where Sp_Tax IS NULL;
ALTER TABLE Special ALTER COLUMN Sp_Tax int not null;
--
update Special_Article set Sp_Id=0 where Sp_Id IS NULL;
ALTER TABLE Special_Article ALTER COLUMN Sp_Id int not null;
--
update Special_Article set Org_Id=0 where Org_Id IS NULL;
ALTER TABLE Special_Article ALTER COLUMN Org_Id int not null;
--
update Student_Notes set Stn_PID=0 where Stn_PID IS NULL;
ALTER TABLE Student_Notes ALTER COLUMN Stn_PID int not null;
--
update SystemPara set Org_Id=0 where Org_Id IS NULL;
ALTER TABLE SystemPara ALTER COLUMN Org_Id int not null;
--
update TestPaperQues set Qk_Id=0 where Qk_Id IS NULL;
ALTER TABLE TestPaperQues ALTER COLUMN Qk_Id int not null;
--
update TestPaperQues set Tq_Percent=0 where Tq_Percent IS NULL;
ALTER TABLE TestPaperQues ALTER COLUMN Tq_Percent int not null;
--
update TestPaperQues set Tq_Type=0 where Tq_Type IS NULL;
ALTER TABLE TestPaperQues ALTER COLUMN Tq_Type int not null;
--

/*所有bit，设置不可为空*/
update RechargeCode set Rc_IsEnable=1 where Rc_IsEnable IS NULL;
ALTER TABLE RechargeCode ALTER COLUMN Rc_IsEnable bit not null;
--

/*所有varchar转为nvarchar*/
ALTER TABLE ExamResults ALTER COLUMN Org_Name nvarchar(255);
ALTER TABLE Teacher ALTER COLUMN Ths_Name nvarchar(255);

/*所有money转为decimal，且设置不可为空*/
update Accounts set Ac_Money=0 where Ac_Money IS NULL;
ALTER TABLE Accounts ALTER COLUMN Ac_Money decimal(18,4) not null;

update ProfitSharing set Ps_MoneyValue=0 where Ps_MoneyValue IS NULL;
ALTER TABLE ProfitSharing ALTER COLUMN Ps_MoneyValue decimal(18,4) not null;

update MoneyAccount set Ma_Total=0 where Ma_Total IS NULL;
ALTER TABLE MoneyAccount ALTER COLUMN Ma_Total decimal(18,4) not null;

update MoneyAccount set Ma_Money=0 where Ma_Money IS NULL;
ALTER TABLE MoneyAccount ALTER COLUMN Ma_Money decimal(18,4) not null;

update Student_Course set Stc_Money=0 where Stc_Money IS NULL;
ALTER TABLE Student_Course ALTER COLUMN Stc_Money decimal(18,4) not null;


/*删除QuesAnswer表并重建，某些字段在转为PostgreSQL异常，原因不明，重建后就可以了转了
  QuesAnswer表是一个空表，仅作为试题答案的实体映射，并不实际存储数据
*/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuesAnswer]') AND type in (N'U'))
DROP TABLE [dbo].[QuesAnswer]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuesAnswer](
	[Ans_ID] [bigint] NOT NULL,
	[Qus_ID] [bigint] NOT NULL,
	[Qus_UID] [nvarchar](255) NULL,
	[Ans_Context] [ntext] NULL,
	[Ans_IsCorrect] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


