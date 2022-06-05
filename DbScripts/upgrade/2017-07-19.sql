--2018-07-19
/*记录学员练习试题的进度*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogForStudentQuestions](
	[Lsq_ID] [int] IDENTITY(1,1) NOT NULL,
	[Org_ID] [int] NOT NULL,
	[Ac_ID] [int] NOT NULL,
	[Ac_AccName] [nvarchar](50) NULL,
	[Cou_ID] [int] NOT NULL,
	[Ol_ID] [int] NOT NULL,
	[Lsq_CrtTime] [datetime] NOT NULL,
	[Lsq_LastTime] [datetime] NOT NULL,
	[Qus_ID] [int] NOT NULL,
	[Lsq_Index] [int] NOT NULL,
 CONSTRAINT [PK_LogForStudentQuestions] PRIMARY KEY CLUSTERED 
(
	[Lsq_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

