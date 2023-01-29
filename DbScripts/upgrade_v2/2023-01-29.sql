/*考试成绩中增加是否已经批阅的字段*/
alter table [ExamResults] add [Exr_IsManual] [bit] default 0 NOT NULL