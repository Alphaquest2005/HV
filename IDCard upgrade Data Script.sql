/*
This script was created by Visual Studio on 12/16/2020 at 12:28 PM.
Run this script on JOSEPH-PC\SQLDEVELOPER2017.QuickSales-Enterprise-Production (JOSEPH-PC\josep) to make it the same as JOSEPH-PC\SQLDEVELOPER2017.QuickSales-Enterprise (JOSEPH-PC\josep).
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[MembershipRewards] DROP CONSTRAINT [FK_MembershipRewards_MembershipTypes]
ALTER TABLE [dbo].[MembershipRewards] DROP CONSTRAINT [FK_MembershipRewards_Rewards]
SET IDENTITY_INSERT [dbo].[Rewards] ON
INSERT INTO [dbo].[Rewards] ([Id], [Name], [Description], [Value], [DaysToExpiration], [StoreName], [ItemNumber]) VALUES (1, N'Lollipop', N'Sweety', 1, NULL, N'G2', 2)
INSERT INTO [dbo].[Rewards] ([Id], [Name], [Description], [Value], [DaysToExpiration], [StoreName], [ItemNumber]) VALUES (2, N'Chololate', N'Cadbury Chocolate', 20, 30, N'Grenville', 6)
INSERT INTO [dbo].[Rewards] ([Id], [Name], [Description], [Value], [DaysToExpiration], [StoreName], [ItemNumber]) VALUES (3, N'Massage', N'Get 1 Free Massage at Well Care Center St. Georges', 100, 360, N'MedCare', 3)
INSERT INTO [dbo].[Rewards] ([Id], [Name], [Description], [Value], [DaysToExpiration], [StoreName], [ItemNumber]) VALUES (4, N'Depends', N'Depends Diaper', 50, 30, N'Grenville', 5)
INSERT INTO [dbo].[Rewards] ([Id], [Name], [Description], [Value], [DaysToExpiration], [StoreName], [ItemNumber]) VALUES (5, N'Chololate', N'Nuggles', 1.5, 30, N'St. George''s', 4)
SET IDENTITY_INSERT [dbo].[Rewards] OFF
SET IDENTITY_INSERT [dbo].[MembershipRewards] ON
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (1, 1, 10, 1, 100)
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (2, 2, 100, 2, 2500)
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (3, 5, 1000, 3, 120)
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (4, 6, 500, 4, 50)
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (5, 1, 20, 5, 2500)
INSERT INTO [dbo].[MembershipRewards] ([Id], [MembershipTypeId], [Points], [RewardId], [Value]) VALUES (6, 3, 100, 4, 5000)
SET IDENTITY_INSERT [dbo].[MembershipRewards] OFF
SET IDENTITY_INSERT [dbo].[MembershipTypes] ON
INSERT INTO [dbo].[MembershipTypes] ([Id], [Name], [EntrySalesAmount], [MaxSalesAmount], [PointRatePerDollar], [QuickBooksPriceLevel], [Discount], [EntryAge], [MaxAge]) VALUES (1, N'Basic', 0, 1000, 0.1, N'Level1', 0.05, NULL, NULL)
INSERT INTO [dbo].[MembershipTypes] ([Id], [Name], [EntrySalesAmount], [MaxSalesAmount], [PointRatePerDollar], [QuickBooksPriceLevel], [Discount], [EntryAge], [MaxAge]) VALUES (2, N'Bronze', 1001, 2000, 0.15, N'Level2', 0.07, NULL, NULL)
INSERT INTO [dbo].[MembershipTypes] ([Id], [Name], [EntrySalesAmount], [MaxSalesAmount], [PointRatePerDollar], [QuickBooksPriceLevel], [Discount], [EntryAge], [MaxAge]) VALUES (3, N'Silver', 2001, 3000, 0.2, N'Level3', 0.1, NULL, NULL)
INSERT INTO [dbo].[MembershipTypes] ([Id], [Name], [EntrySalesAmount], [MaxSalesAmount], [PointRatePerDollar], [QuickBooksPriceLevel], [Discount], [EntryAge], [MaxAge]) VALUES (5, N'Gold', 3001, 10000, 0.25, N'Level4', 0.15, NULL, NULL)
INSERT INTO [dbo].[MembershipTypes] ([Id], [Name], [EntrySalesAmount], [MaxSalesAmount], [PointRatePerDollar], [QuickBooksPriceLevel], [Discount], [EntryAge], [MaxAge]) VALUES (6, N'60+', 0, 0, 0.125, N'60+', 0.07, 60, 120)
SET IDENTITY_INSERT [dbo].[MembershipTypes] OFF
ALTER TABLE [dbo].[MembershipRewards]
    WITH NOCHECK ADD CONSTRAINT [FK_MembershipRewards_MembershipTypes] FOREIGN KEY ([MembershipTypeId]) REFERENCES [dbo].[MembershipTypes] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[MembershipRewards]
    WITH NOCHECK ADD CONSTRAINT [FK_MembershipRewards_Rewards] FOREIGN KEY ([RewardId]) REFERENCES [dbo].[Rewards] ([Id]) ON DELETE CASCADE
COMMIT TRANSACTION
