--考试与学员的对应关系
--2015-05-14

USE [examweisha]
GO
--修改字段
sp_rename 'ExamGroup.Dep_Id','Sts_ID','column' 
GO
--修改字段
sp_rename 'Examination.Acc_Id','Th_ID','column' 
GO
sp_rename 'Examination.Acc_Name','Th_Name','column'
GO

--删除多余字段
ALTER TABLE ExamGroup   
DROP COLUMN Dep_CnName  
GO

ALTER TABLE ExamGroup   
DROP COLUMN Team_ID 
GO

ALTER TABLE ExamGroup   
DROP COLUMN Team_Name 
GO

ALTER TABLE ExamGroup   
DROP COLUMN EGrp_IsSystem 
GO