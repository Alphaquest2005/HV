USE [QuickSales-Enterprise]
GO
/****** Object:  Table [dbo].[RxAbbrevations]    Script Date: 11/15/2019 9:30:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RxAbbrevations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Shortcut] [varchar](50) NOT NULL,
	[Sentence] [varchar](255) NOT NULL,
 CONSTRAINT [PK_RxAbbrevations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RxAbbrevations] ON 
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (1, N'Shortcut', N'Sentence')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (2, N'1/2 TSP 3D', N'Take half a teaspoonful (2.5ml) by mouth, three (3) a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (3, N'1 TSP 3D', N'Take one a teaspoonful (5ml) by mouth, three (3) a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (4, N'2 TSP 3D', N'Take two (2) teaspoonful (10ml) by mouth, three (3) a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (5, N'1/2 TSP 2D', N'Take half a teaspoonful (2.5ml) by mouth, two (2) times a day ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (6, N'1 TSP 2D', N'Take one teaspoonful (5mls) by mouth, two (2) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (7, N'2 TSP 2D', N'Take two teaspoonful (10mls) by mouth, two (2) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (8, N'1/2 TAB 3D', N'Take half (1/2) a tablet by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (9, N'1 TAB 3D', N'Take one (1) tablet by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (10, N'2 TAB 3D', N'Take two (2) tablets by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (11, N'3 TAB 3D', N'Take Three (3) tablets by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (12, N'1 TAB BD', N'Take one (1) tablet by mouth, two (2) times a day ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (13, N'2 TAB', N'Take two (2) tablet by mouth, two (2) times a day ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (14, N'1 TAB OD', N'Take one (1) tablet by mouth, once a day ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (15, N'1 TAB Q4', N'Take one (1) tablet by mouth, every four (4) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (16, N'1 TAB Q6', N'Take one (1) tablet by mouth, every six (6) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (17, N'1 TAB Q8', N'Take one (1) tablet by mouth, every eight (8) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (18, N'1 TAB Q12 ', N'Take one (1) tablet by mouth, every twelve (12) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (19, N'1 TAB NOCTE', N'Take one (1) tablet at bedtime ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (20, N'2 TAB NOCTE', N'Take two (2) tablets at bedtime ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (21, N'1 TAB AM', N'Take one (1) tablet every morning ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (22, N'1 TAB PM', N'Take one (1) tablet at night ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (23, N'1 TAB STAT', N'Take one (1) tablet immediately ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (24, N'2 TAB STAT', N'Take two (2) tablets immediately ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (25, N'4 TAB STAT', N'Take four (4) tablets immediately ')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (26, N'1 CAP OD', N'Take one (1) capsule once a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (27, N'1 CAP BD', N'Take one (1) capsule by mouth, two (2) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (28, N'1 CAP TD', N'Take one (1) capsule by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (29, N'2 CAP OD', N'Take two (2) capsules by mouth, once a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (30, N'2 CAP BD', N'Take two (2) capsules by mouth, two (2) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (31, N'2 CAP TD', N'Take two (2) capsules by mouth, three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (32, N'1 CAP Q4', N'Take one (1) capsule by mouth, every four (4) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (33, N'1 CAP Q6', N'Take one (1) capsule by mouth, every Six (6) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (34, N'1 CAP Q8', N'Take one (1) capsule by mouth, every eight (8) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (35, N'1 CAP Q12', N'Take one (1) capsule by mouth, every twelve (12) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (36, N'AAA OD', N'Apply to affected area/s once a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (37, N'AAA BD', N'Apply to affected area/s twice a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (38, N'AAA TD', N'Apply to affected area/s three (3) times a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (39, N'AAA Q4', N'Apply to affected area/s every four (4) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (40, N'AAA Q6', N'Apply to affected area/s every six (6) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (41, N'AAA Q8', N'Apply to affected area/s every eight (8) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (42, N'AAA Q12', N'Apply to affected area/s every twelve (12) hours')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (43, N'1 GTTS B Ey', N'Instill One (1) drop into both eyes')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (44, N'1 GTTS L Ey', N'Instill One (1) drop into left eye')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (45, N'1 GTTS R Ey', N'Instill One (1) drop into right eye')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (46, N'2 GTTS B Ey', N'Instill two (2) drop into both eyes')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (47, N'2 GTTS L Ey', N'Instill two (2) drop into left eye')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (48, N'2 GTTS R Ey', N'Instill two (2) drop into right eye')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (49, N'1 GTTS B Er', N'Instill One (1) drop into both ears')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (50, N'1 GTTS L Er', N'Instill One (1) drop into left ear')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (51, N'1 GTTS R Er', N'Instill One (1) drop into right ear')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (52, N'2 GTTS B Er', N'Instill two (2) drop into both ears')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (53, N'2 GTTS L Er', N'Instill two (2) drop into left ear')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (54, N'2 GTTS R Er', N'Instill two (2) drop into right ear')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (55, N'1 TSP 4D', N'Take one a teaspoonful (5ml) by mouth, three (4) a day')
GO
INSERT [dbo].[RxAbbrevations] ([Id], [Shortcut], [Sentence]) VALUES (56, N'3 TSP 3D', N'Take two (3) teaspoonful (15ml) by mouth, three (3) a day')
GO
SET IDENTITY_INSERT [dbo].[RxAbbrevations] OFF
GO
