--2020-7-28
/*ѧԱ�飬���Ӹ���ѧԱ����Ƶѧϰʱ���л�������Ƿ���ͣ���ţ�Ĭ��Ϊfalse*/
alter table StudentSort add Sts_SwitchPlay  bit NULL
go
UPDATE StudentSort SET Sts_SwitchPlay  = 0
GO
alter table StudentSort ALTER COLUMN Sts_SwitchPlay  bit NOT NULL
/*�޸Ĳ˵�����*/
UPDATE ManageMenu
   SET ManageMenu.MM_Name='ѧԱ��'
   where ManageMenu.MM_Name='ѧԱ�༶'