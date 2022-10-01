
/*学员组与课程的关联表*/
CREATE TABLE [StudentSort_Course](
	[Ssc_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ssc_StartTime] [datetime] NOT NULL,
	[Ssc_EndTime] [datetime] NOT NULL,
	[Ssc_IsEnable] [bit] NOT NULL,
	[Cou_ID] [bigint] NOT NULL,
	[Sts_ID] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Ssc_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, 
IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


