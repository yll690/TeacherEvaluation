USE [TeacherEvaluation]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON
GO


CREATE TABLE [dbo].[institute](
	[iID] [char](5) NOT NULL,
	[iName] [nvarchar](20) NULL,
	[intro] [nvarchar](1000) NULL,
	[picture] [nvarchar](200) NULL,
 CONSTRAINT [PK_institute] PRIMARY KEY CLUSTERED 
(
	[iID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[student](
	[sID] [char](10) NOT NULL,
	[sName] [nvarchar](10) NULL,
	[password] [char](32) NOT NULL,
	[picture] [nvarchar](200) NULL,
	[isMale] [bit] NULL,
	[age] [int] NULL,
	[session] [int] NULL,
	[hideFromStudent] [bit] NULL,
	[hideFromTeacher] [bit] NULL,
 CONSTRAINT [PK_student] PRIMARY KEY CLUSTERED 
(
	[sID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[student] ADD  CONSTRAINT [DF_student_isMale]  DEFAULT ((1)) FOR [isMale]
ALTER TABLE [dbo].[student] ADD  CONSTRAINT [DF_student_hideFromStudent]  DEFAULT ((0)) FOR [hideFromStudent]
ALTER TABLE [dbo].[student] ADD  CONSTRAINT [DF_student_hideFromTeacher]  DEFAULT ((1)) FOR [hideFromTeacher]
GO


CREATE TABLE [dbo].[teacher](
	[tID] [char](10) NOT NULL,
	[iID] [char](5) NOT NULL,
	[tName] [nvarchar](10) NULL,
	[password] [char](32) NOT NULL,
	[profile] [nvarchar](1000) NULL,
	[mark] [float] NULL,
	[picture] [nvarchar](200) NULL,
	[isMale] [bit] NULL,
	[numOfMarks] [int] NULL,
 CONSTRAINT [PK_teacher] PRIMARY KEY CLUSTERED 
(
	[tID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[teacher] ADD  CONSTRAINT [DF_teacher_mark]  DEFAULT ((0)) FOR [mark]
ALTER TABLE [dbo].[teacher] ADD  CONSTRAINT [DF_teacher_isMale]  DEFAULT ((1)) FOR [isMale]
ALTER TABLE [dbo].[teacher] ADD  CONSTRAINT [DF_teacher_numOfMarks]  DEFAULT ((0)) FOR [numOfMarks]
ALTER TABLE [dbo].[teacher]  WITH CHECK ADD  CONSTRAINT [FK_teacher_institute] FOREIGN KEY([iID])
REFERENCES [dbo].[institute] ([iID])
ALTER TABLE [dbo].[teacher] CHECK CONSTRAINT [FK_teacher_institute]
GO


CREATE TABLE [dbo].[manager](
	[mID] [char](5) NOT NULL,
	[password] [char](32) NOT NULL,
	[mName] [nvarchar](10) NULL,
 CONSTRAINT [PK_manager] PRIMARY KEY CLUSTERED 
(
	[mID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[course](
	[cID] [char](10) NOT NULL,
	[cName] [nvarchar](20) NULL,
	[intro] [nvarchar](1000) NULL,
 CONSTRAINT [PK_course] PRIMARY KEY CLUSTERED 
(
	[cID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[teachingCourse](
	[tID] [char](10) NOT NULL,
	[cID] [char](10) NOT NULL,
	[tcID] [uniqueidentifier] NOT NULL,
	[mark] [float] NULL,
	[numOfMarks] [nchar](10) NULL,
	[term] [char](6) NOT NULL,
 CONSTRAINT [PK_teachingCourse] PRIMARY KEY CLUSTERED 
(
	[tcID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[teachingCourse] ADD  CONSTRAINT [DF_teachingCourse_tcID]  DEFAULT (newid()) FOR [tcID]
ALTER TABLE [dbo].[teachingCourse] ADD  CONSTRAINT [DF_teachingCourse_mark]  DEFAULT ((0)) FOR [mark]
ALTER TABLE [dbo].[teachingCourse] ADD  CONSTRAINT [DF_teachingCourse_numOfMarks]  DEFAULT ((0)) FOR [numOfMarks]
ALTER TABLE [dbo].[teachingCourse]  WITH CHECK ADD  CONSTRAINT [FK_teachingCourse_course] FOREIGN KEY([cID])
REFERENCES [dbo].[course] ([cID])
ALTER TABLE [dbo].[teachingCourse] CHECK CONSTRAINT [FK_teachingCourse_course]
ALTER TABLE [dbo].[teachingCourse]  WITH CHECK ADD  CONSTRAINT [FK_teachingCourse_teacher] FOREIGN KEY([tID])
REFERENCES [dbo].[teacher] ([tID])
ALTER TABLE [dbo].[teachingCourse] CHECK CONSTRAINT [FK_teachingCourse_teacher]
GO


CREATE TABLE [dbo].[selectCourse](
	[sID] [char](10) NOT NULL,
	[tcID] [uniqueidentifier] NOT NULL,
	[grade] [float] NULL,
 CONSTRAINT [PK_selectCourse] PRIMARY KEY CLUSTERED 
(
	[sID] ASC,
	[tcID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[selectCourse]  WITH CHECK ADD  CONSTRAINT [FK_selectCourse_teachingCourse] FOREIGN KEY([tcID])
REFERENCES [dbo].[teachingCourse] ([tcID])
ALTER TABLE [dbo].[selectCourse] CHECK CONSTRAINT [FK_selectCourse_teachingCourse]
GO


CREATE TABLE [dbo].[comment](
	[sID] [char](10) NOT NULL,
	[comID] [uniqueidentifier] NOT NULL,
	[cContent] [nvarchar](1000) NOT NULL,
	[isHided] [bit] NOT NULL,
	[time] [datetime] NOT NULL,
	[tcID] [uniqueidentifier] NOT NULL,
	[tReply] [nvarchar](1000) NULL,
 CONSTRAINT [PK_coment] PRIMARY KEY CLUSTERED 
(
	[comID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[comment] ADD  CONSTRAINT [DF_comment_comID]  DEFAULT (newid()) FOR [comID]
ALTER TABLE [dbo].[comment] ADD  CONSTRAINT [DF_coment_isHided]  DEFAULT ((0)) FOR [isHided]
ALTER TABLE [dbo].[comment] ADD  CONSTRAINT [DF_comment_time]  DEFAULT (getdate()) FOR [time]
ALTER TABLE [dbo].[comment]  WITH CHECK ADD  CONSTRAINT [FK_coment_teachingCourse] FOREIGN KEY([tcID])
REFERENCES [dbo].[teachingCourse] ([tcID])
ALTER TABLE [dbo].[comment] CHECK CONSTRAINT [FK_coment_teachingCourse]
GO


CREATE TABLE [dbo].[mark](
	[markID] [uniqueidentifier] NOT NULL,
	[tcID] [uniqueidentifier] NOT NULL,
	[sID] [char](10) NOT NULL,
	[totalMark] [tinyint] NOT NULL,
	[mark1] [tinyint] NOT NULL,
	[mark2] [tinyint] NOT NULL,
	[mark3] [tinyint] NOT NULL,
	[mark4] [tinyint] NOT NULL,
	[mark5] [tinyint] NOT NULL,
 CONSTRAINT [PK_mark] PRIMARY KEY CLUSTERED 
(
	[markID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_markID]  DEFAULT (newid()) FOR [markID]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_totalMark]  DEFAULT ((8)) FOR [totalMark]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_mark1]  DEFAULT ((8)) FOR [mark1]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_mark2]  DEFAULT ((8)) FOR [mark2]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_mark3]  DEFAULT ((8)) FOR [mark3]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_mark4]  DEFAULT ((8)) FOR [mark4]
ALTER TABLE [dbo].[mark] ADD  CONSTRAINT [DF_mark_mark5]  DEFAULT ((8)) FOR [mark5]
ALTER TABLE [dbo].[mark]  WITH CHECK ADD  CONSTRAINT [FK_mark_teachingCourse] FOREIGN KEY([tcID])
REFERENCES [dbo].[teachingCourse] ([tcID])
ALTER TABLE [dbo].[mark] CHECK CONSTRAINT [FK_mark_teachingCourse]
GO