GO
PRINT N'Dropping [dbo].[FK_PersonLocations_Persons]...';


GO
ALTER TABLE [dbo].[PersonLocations] DROP CONSTRAINT [FK_PersonLocations_Persons];


GO
PRINT N'Dropping [dbo].[FK_Cashier_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Cashier] DROP CONSTRAINT [FK_Cashier_inherits_Person];


GO
PRINT N'Dropping [dbo].[FK_Doctor_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Doctor] DROP CONSTRAINT [FK_Doctor_inherits_Person];


GO
PRINT N'Dropping [dbo].[FK_CustomerTransaction]...';


GO
ALTER TABLE [dbo].[TransactionBase] DROP CONSTRAINT [FK_CustomerTransaction];


GO
PRINT N'Dropping [dbo].[FK_Patient_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_Patient_inherits_Person];


GO
PRINT N'Starting rebuilding table [dbo].[Persons]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Persons] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]      NVARCHAR (50)  NOT NULL,
    [LastName]       NVARCHAR (50)  NOT NULL,
    [CompanyName]    NVARCHAR (50)  NULL,
    [Salutation]     NVARCHAR (3)   NULL,
    [Address]        NVARCHAR (255) NULL,
    [PhoneNumber]    NVARCHAR (50)  NULL,
    [InActive]       BIT            NULL,
    [DOB]            DATETIME       NULL,
    [Sex]            BIT            NULL,
    [EntryTimeStamp] ROWVERSION     NOT NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Persons1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Persons])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Persons] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Persons] ([Id], [FirstName], [LastName], [CompanyName], [Salutation], [Address], [PhoneNumber], [InActive], [DOB], [Sex])
        SELECT   [Id],
                 [FirstName],
                 [LastName],
                 [CompanyName],
                 [Salutation],
                 [Address],
                 [PhoneNumber],
                 [InActive],
                 [DOB],
                 [Sex]
        FROM     [dbo].[Persons]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Persons] OFF;
    END

DROP TABLE [dbo].[Persons];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Persons]', N'Persons';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Persons1]', N'PK_Persons', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[Persons_Patient]...';


GO
ALTER TABLE [dbo].[Persons_Patient]
    ADD [TotalSales]    FLOAT (53) NULL,
        [StartingSales] FLOAT (53) NULL;


GO
PRINT N'Creating [dbo].[Persons_Patient].[idxa_totalsales]...';


GO
CREATE NONCLUSTERED INDEX [idxa_totalsales]
    ON [dbo].[Persons_Patient]([TotalSales] ASC)
    INCLUDE([StartingSales]);


GO
PRINT N'Altering [dbo].[TransactionBase_QuickPrescription]...';


GO
ALTER TABLE [dbo].[TransactionBase_QuickPrescription]
    ADD [PatientId] INT NULL;


GO
PRINT N'Creating [dbo].[MembershipRewards]...';


GO
CREATE TABLE [dbo].[MembershipRewards] (
    [Id]               INT        IDENTITY (1, 1) NOT NULL,
    [MembershipTypeId] INT        NOT NULL,
    [Points]           INT        NOT NULL,
    [RewardId]         INT        NOT NULL,
    [Value]            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_MembershipRewards] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[MembershipTypes]...';


GO
CREATE TABLE [dbo].[MembershipTypes] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (50) NOT NULL,
    [EntrySalesAmount]     FLOAT (53)    NOT NULL,
    [MaxSalesAmount]       FLOAT (53)    NOT NULL,
    [PointRatePerDollar]   FLOAT (53)    NOT NULL,
    [QuickBooksPriceLevel] NVARCHAR (50) NOT NULL,
    [Discount]             FLOAT (53)    NOT NULL,
    [EntryAge]             INT           NULL,
    [MaxAge]               INT           NULL,
    CONSTRAINT [PK_MembershipTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[PatientRewards]...';


GO
CREATE TABLE [dbo].[PatientRewards] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [PatientId]  INT           NOT NULL,
    [RewardId]   INT           NOT NULL,
    [DateIssued] DATE          NOT NULL,
    [StoreName]  NVARCHAR (50) NULL,
    [ItemNumber] INT           NULL,
    CONSTRAINT [PK_PatientRewards] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[PatientRewardSale]...';


GO
CREATE TABLE [dbo].[PatientRewardSale] (
    [Id]                INT           NOT NULL,
    [TrackingNumber]    NVARCHAR (50) NOT NULL,
    [SalesDate]         DATE          NOT NULL,
    [SalesReceptNumber] NVARCHAR (50) NULL,
    CONSTRAINT [PK_PatientRewardSale] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Photos]...';


GO
CREATE TABLE [dbo].[Photos] (
    [Id]    INT             NOT NULL,
    [Photo] VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[QBCustomer]...';


GO
CREATE TABLE [dbo].[QBCustomer] (
    [Id]                  INT           NOT NULL,
    [CustomerListID]      NVARCHAR (50) NOT NULL,
    [CustomerDiscPercent] FLOAT (53)    NULL,
    [CustomerDiscType]    NVARCHAR (50) NULL,
    [PriceLevelNumber]    INT           NULL,
    [IsRewardsMember]     BIT           NULL,
    [LastSale]            NVARCHAR (50) NULL,
    [RewardAmount]        FLOAT (53)    NULL,
    [RewardPercent]       FLOAT (53)    NULL,
    [EarnedDate]          DATE          NULL,
    [ExpirationDate]      DATE          NULL,
    [Source]              NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QBCustomer] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[QBSales]...';


GO
CREATE TABLE [dbo].[QBSales] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [QBCustomerId]       INT           NOT NULL,
    [SalesRecieptNumber] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_QBSales] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[QBSalesDetail]...';


GO
CREATE TABLE [dbo].[QBSalesDetail] (
    [Id]      INT        NOT NULL,
    [TxnDate] DATE       NOT NULL,
    [Total]   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_QBSalesDetail] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Rewards]...';


GO
CREATE TABLE [dbo].[Rewards] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)  NOT NULL,
    [Description]      NVARCHAR (255) NOT NULL,
    [Value]            FLOAT (53)     NOT NULL,
    [DaysToExpiration] INT            NULL,
    [StoreName]        NVARCHAR (50)  NOT NULL,
    [ItemNumber]       INT            NOT NULL,
    CONSTRAINT [PK_Rewards] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionBase].[NCIDX_BB_80_TransactionBase]...';


GO
CREATE NONCLUSTERED INDEX [NCIDX_BB_80_TransactionBase]
    ON [dbo].[TransactionBase]([Status] ASC);


GO
PRINT N'Creating [dbo].[TransactionEntryItem].[NCIDX_BB_97_TransactionEntryItem]...';


GO
CREATE NONCLUSTERED INDEX [NCIDX_BB_97_TransactionEntryItem]
    ON [dbo].[TransactionEntryItem]([ItemId] ASC);


GO
PRINT N'Creating [dbo].[DF_PatientRewards_DateRewarded]...';


GO
ALTER TABLE [dbo].[PatientRewards]
    ADD CONSTRAINT [DF_PatientRewards_DateRewarded] DEFAULT (getdate()) FOR [DateIssued];


GO
PRINT N'Creating [dbo].[FK_PersonLocations_Persons]...';


GO
ALTER TABLE [dbo].[PersonLocations] WITH NOCHECK
    ADD CONSTRAINT [FK_PersonLocations_Persons] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Cashier_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Cashier] WITH NOCHECK
    ADD CONSTRAINT [FK_Cashier_inherits_Person] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Doctor_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Doctor] WITH NOCHECK
    ADD CONSTRAINT [FK_Doctor_inherits_Person] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_CustomerTransaction]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_CustomerTransaction] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Persons] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Patient_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_inherits_Person] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MembershipRewards_MembershipTypes]...';


GO
ALTER TABLE [dbo].[MembershipRewards] WITH NOCHECK
    ADD CONSTRAINT [FK_MembershipRewards_MembershipTypes] FOREIGN KEY ([MembershipTypeId]) REFERENCES [dbo].[MembershipTypes] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MembershipRewards_Rewards]...';


GO
ALTER TABLE [dbo].[MembershipRewards] WITH NOCHECK
    ADD CONSTRAINT [FK_MembershipRewards_Rewards] FOREIGN KEY ([RewardId]) REFERENCES [dbo].[Rewards] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PatientRewards_Persons_Patient]...';


GO
ALTER TABLE [dbo].[PatientRewards] WITH NOCHECK
    ADD CONSTRAINT [FK_PatientRewards_Persons_Patient] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Persons_Patient] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PatientRewards_Rewards]...';


GO
ALTER TABLE [dbo].[PatientRewards] WITH NOCHECK
    ADD CONSTRAINT [FK_PatientRewards_Rewards] FOREIGN KEY ([RewardId]) REFERENCES [dbo].[Rewards] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PatientRewardSale_PatientRewards]...';


GO
ALTER TABLE [dbo].[PatientRewardSale] WITH NOCHECK
    ADD CONSTRAINT [FK_PatientRewardSale_PatientRewards] FOREIGN KEY ([Id]) REFERENCES [dbo].[PatientRewards] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Photos_Persons_Patient]...';


GO
ALTER TABLE [dbo].[Photos] WITH NOCHECK
    ADD CONSTRAINT [FK_Photos_Persons_Patient] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons_Patient] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_QBCustomer_Persons_Patient]...';


GO
ALTER TABLE [dbo].[QBCustomer] WITH NOCHECK
    ADD CONSTRAINT [FK_QBCustomer_Persons_Patient] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons_Patient] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_QBSales_QBCustomer]...';


GO
ALTER TABLE [dbo].[QBSales] WITH NOCHECK
    ADD CONSTRAINT [FK_QBSales_QBCustomer] FOREIGN KEY ([QBCustomerId]) REFERENCES [dbo].[QBCustomer] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_QBSalesDetail_QBSales]...';


GO
ALTER TABLE [dbo].[QBSalesDetail] WITH NOCHECK
    ADD CONSTRAINT [FK_QBSalesDetail_QBSales] FOREIGN KEY ([Id]) REFERENCES [dbo].[QBSales] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TransactionBase_QuickPrescription_Persons_Patient]...';


GO
ALTER TABLE [dbo].[TransactionBase_QuickPrescription] WITH NOCHECK
    ADD CONSTRAINT [FK_TransactionBase_QuickPrescription_Persons_Patient] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Persons_Patient] ([Id]);


GO
PRINT N'Refreshing [dbo].[DoctorPopularityLst]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[DoctorPopularityLst]';


GO
PRINT N'Refreshing [dbo].[DoctorsView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[DoctorsView]';


GO
PRINT N'Refreshing [dbo].[Patient History]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[Patient History]';


GO
PRINT N'Altering [dbo].[PatientLifeTimeValue]...';


GO

ALTER VIEW [dbo].[PatientLifeTimeValue]
AS
SELECT        TOP (100) PERCENT dbo.Persons.Id, dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, dbo.Persons.PhoneNumber, COUNT(DISTINCT dbo.TransactionBase.TransactionId) AS Prescriptions, 
                         SUM(dbo.TransactionEntryBase.Price * dbo.TransactionEntryBase.Quantity) AS TotalValue
FROM            dbo.Persons INNER JOIN
                         dbo.Persons_Patient ON dbo.Persons.Id = dbo.Persons_Patient.Id INNER JOIN
                         dbo.TransactionBase_Prescription ON dbo.Persons_Patient.Id = dbo.TransactionBase_Prescription.PatientId INNER JOIN
                         dbo.TransactionBase ON dbo.TransactionBase_Prescription.TransactionId = dbo.TransactionBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId
WHERE        (dbo.TransactionBase.Time >= CONVERT(DATETIME, '2018-01-01 00:00:00', 102) AND dbo.TransactionBase.Time <= CONVERT(DATETIME, '2018-12-31 00:00:00', 102))
GROUP BY dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, dbo.Persons.PhoneNumber, dbo.Persons.Id
ORDER BY TotalValue DESC
GO
PRINT N'Refreshing [dbo].[PatientView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[PatientView]';


GO
PRINT N'Refreshing [dbo].[RepeatPatientList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[RepeatPatientList]';


GO
PRINT N'Altering [dbo].[TransactionData]...';


GO

ALTER VIEW [dbo].[TransactionData]
AS
SELECT dbo.TransactionBase.StationId, dbo.TransactionBase.BatchId, dbo.TransactionBase.CloseBatchId, dbo.TransactionBase.Time, dbo.TransactionBase.CustomerId, dbo.TransactionBase.PharmacistId, 
                 dbo.TransactionBase.CashierId, dbo.TransactionBase.Comment, dbo.TransactionBase.ReferenceNumber, dbo.TransactionBase.StoreCode, dbo.TransactionBase.TransactionId, dbo.TransactionBase.OpenClose, 
                 dbo.TransactionBase.Status, dbo.TransactionBase.EntryTimeStamp, dbo.TransactionBase.ParentTransactionId, dbo.TransactionBase.DeliveryType, dbo.TransactionLocation.Longitude, 
                 dbo.TransactionLocation.Latitude, dbo.TransactionEntryBase.StoreID, dbo.TransactionEntryBase.TransactionEntryId, dbo.TransactionEntryBase.Price, dbo.TransactionEntryBase.Quantity, 
                 dbo.TransactionEntryBase.Taxable, dbo.TransactionBase.Comment AS Expr1, dbo.TransactionEntryBase.TransactionTime, dbo.TransactionEntryBase.SalesTaxPercent, dbo.TransactionEntryBase.Discount, 
                 dbo.TransactionEntryBase.EntryNumber, dbo.TransactionEntryBase_PrescriptionEntry.Dosage, dbo.TransactionEntryBase_PrescriptionEntry.ExpiryDate, dbo.TransactionEntryBase_PrescriptionEntry.Repeat, 
                 dbo.TransactionEntryBase_PrescriptionEntry.RepeatQuantity, dbo.TransactionEntryItem.QBItemListID, dbo.TransactionEntryItem.ItemNumber, dbo.TransactionEntryItem.ItemName, dbo.TransactionEntryItem.ItemId, 
                 PCashier.IsActive, PCashier.Initials, PCashier.Role, PCashier.LoginName, dbo.TransactionEntryBase.Comment AS TransactionEntryComment, Cashier.FirstName AS CashierFirstName, 
                 Cashier.LastName AS CashierLastName, dbo.TransactionBase_Prescription.DoctorId, isnull(dbo.TransactionBase_Prescription.PatientId,dbo.TransactionBase_QuickPrescription.PatientId) as PatientId, Pharmacist.FirstName AS PharmacistFirstName, 
                 Pharmacist.LastName AS PharmacistLastName, dbo.Item.Description
FROM    dbo.Item RIGHT OUTER JOIN
                 dbo.TransactionEntryItem ON dbo.Item.ItemId = dbo.TransactionEntryItem.ItemId RIGHT OUTER JOIN
                 dbo.TransactionBase INNER JOIN
                 dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId LEFT OUTER JOIN
                 dbo.Persons_Cashier AS pPharmacist INNER JOIN
                 dbo.Persons AS Pharmacist ON pPharmacist.Id = Pharmacist.Id ON dbo.TransactionBase.PharmacistId = pPharmacist.Id LEFT OUTER JOIN
                 dbo.Persons AS Cashier INNER JOIN
                 dbo.Persons_Cashier AS PCashier ON Cashier.Id = PCashier.Id ON dbo.TransactionBase.CashierId = PCashier.Id LEFT OUTER JOIN
                 dbo.TransactionBase_Prescription ON dbo.TransactionBase.TransactionId = dbo.TransactionBase_Prescription.TransactionId  LEFT OUTER JOIN
                 dbo.TransactionBase_QuickPrescription ON dbo.TransactionBase.TransactionId = dbo.TransactionBase_QuickPrescription.TransactionId ON 
                 dbo.TransactionEntryItem.TransactionEntryId = dbo.TransactionEntryBase.TransactionEntryId LEFT OUTER JOIN
                 dbo.TransactionEntryBase_PrescriptionEntry ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryBase_PrescriptionEntry.TransactionEntryId LEFT OUTER JOIN
                 dbo.TransactionLocation ON dbo.TransactionBase.TransactionId = dbo.TransactionLocation.TransactionId
GO
PRINT N'Refreshing [dbo].[SearchView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SearchView]';


GO
PRINT N'Refreshing [dbo].[TransactionSummary]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[TransactionSummary]';


GO
PRINT N'Refreshing [dbo].[TransactionValueData]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[TransactionValueData]';


GO
PRINT N'Creating [dbo].[PatientMembership]...';


GO
CREATE VIEW [dbo].[PatientMembership]
AS

SELECT patient.Id AS PatientId,(ISNULL(TotalSales, 0) - ISNULL(StartingSales, 0)) *  MembershipTypes.PointRatePerDollar AS Points, Name as Membership, MembershipTypes.Id as MembershipId,  DATEDIFF(Year, patient.DOB, GETDATE()) AS Age
FROM    (SELECT Persons.Id, Persons.DOB, Persons_Patient.TotalSales, Persons_Patient.StartingSales
                 FROM     Persons INNER JOIN
                                  Persons_Patient ON Persons_Patient.Id = Persons.Id) AS patient INNER JOIN
                 MembershipTypes ON patient.TotalSales BETWEEN MembershipTypes.EntrySalesAmount AND MembershipTypes.MaxSalesAmount
WHERE  (DATEDIFF(Year, patient.DOB, GETDATE()) BETWEEN isnull(MembershipTypes.EntryAge,0) AND isnull(MembershipTypes.MaxAge,60))
GO
PRINT N'Creating [dbo].[PatientRewardStatus]...';


GO




CREATE VIEW [dbo].[PatientRewardStatus]
AS
SELECT PatientRewards.Id as PatientRewardId, PatientRewards.PatientId, PatientRewards.RewardId, PatientRewards.DateIssued, PatientRewardSale.SalesDate, CASE WHEN salesdate IS NULL THEN 'Issued:- ' + cast(datediff(day, dateissued, dateadd(day, 
                 isnull(daystoexpiration,0), dateissued)) as nvarchar(3)) + ' ago' ELSE 'Redeemed:- #' + PatientRewardSale.TrackingNumber END AS Status, ISNULL(PatientRewardSale.SalesDate, PatientRewards.DateIssued) AS StatusDate, PatientRewardSale.SalesReceptNumber
FROM    PatientRewards INNER JOIN
                 Rewards ON PatientRewards.RewardId = Rewards.Id LEFT OUTER JOIN
                 PatientRewardSale ON PatientRewards.Id = PatientRewardSale.Id
GO
PRINT N'Altering [dbo].[IDCardInfo]...';


GO


ALTER VIEW [dbo].[IDCardInfo]
AS
SELECT Persons.Id as PatientId, Persons.FirstName, Persons.LastName, Persons.Address, Persons.PhoneNumber, cast(LEFT(Persons.FirstName, 1) + LEFT(Persons.LastName, 1) 
                 + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(persons.phonenumber, 3) = '473' THEN substring(Persons.PhoneNumber, 4, 7) ELSE persons.phonenumber END)), 7) as nvarchar(15)) AS CardId, Persons_Patient.TotalSales, 
                 PatientMembership.Points, MembershipTypes.Name as Membership, MembershipTypes.EntrySalesAmount, MembershipTypes.MaxSalesAmount, MembershipTypes.PointRatePerDollar, MembershipTypes.Discount, 
                 MembershipTypes.QuickBooksPriceLevel, Company.Address AS Store, MembershipTypes.Id AS MembershipId
FROM    Persons INNER JOIN
                 Persons_Patient ON Persons.Id = Persons_Patient.Id INNER JOIN
                 PatientMembership ON Persons_Patient.Id = PatientMembership.PatientId INNER JOIN
                 MembershipTypes ON PatientMembership.MembershipId = MembershipTypes.Id CROSS JOIN
                 Company
GROUP BY Persons.Id, Persons.FirstName, Persons.LastName, Persons.Address, Persons.PhoneNumber, LEFT(Persons.FirstName, 1) + LEFT(Persons.LastName, 1) 
                 + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(persons.phonenumber, 3) = '473' THEN substring(Persons.PhoneNumber, 4, 7) ELSE persons.phonenumber END)), 7), Persons_Patient.TotalSales, 
                 PatientMembership.Points, MembershipTypes.Name, MembershipTypes.EntrySalesAmount, MembershipTypes.MaxSalesAmount, MembershipTypes.PointRatePerDollar, MembershipTypes.Discount, 
                 MembershipTypes.QuickBooksPriceLevel, Company.Address, MembershipTypes.Id
GO
PRINT N'Creating [dbo].[PatientAvailableRewards]...';


GO

CREATE VIEW [dbo].[PatientAvailableRewards]
AS
SELECT TOP (100) PERCENT (row_number() OVER (ORDER BY IDCardInfo.patientid)) as Id, IDCardInfo.PatientId, Rewards.Name, IDCardInfo.Points, MembershipRewards.Points AS RewardPoints, Rewards.Description, IDCardInfo.Membership, IDCardInfo.PointRatePerDollar, Rewards.Value, 
                 IDCardInfo.MembershipId, PatientRewardStatus.Status, PatientRewardStatus.StatusDate, Rewards.Id AS RewardId, PatientRewardStatus.PatientRewardId, Rewards.DaysToExpiration, Rewards.StoreName, 
                 Rewards.ItemNumber
FROM    IDCardInfo INNER JOIN
                 Rewards INNER JOIN
                 MembershipRewards ON Rewards.Id = MembershipRewards.RewardId ON IDCardInfo.MembershipId = MembershipRewards.MembershipTypeId AND IDCardInfo.Points >= MembershipRewards.Points LEFT OUTER JOIN
                 PatientRewardStatus ON Rewards.Id = PatientRewardStatus.RewardId AND IDCardInfo.PatientId = PatientRewardStatus.PatientId
WHERE (PatientRewardStatus.SalesReceptNumber IS NULL)
ORDER BY Rewards.Value
GO