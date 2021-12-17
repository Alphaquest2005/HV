
PRINT N'Creating [dbo].[Batches]...';


GO
CREATE TABLE [dbo].[Batches] (
    [BatchId]           INT            IDENTITY (1, 1) NOT NULL,
    [OpeningCash]       FLOAT (53)     NOT NULL,
    [EndingCash]        FLOAT (53)     NULL,
    [OpeningTime]       DATETIME       NOT NULL,
    [ClosingTime]       DATETIME       NULL,
    [TotalTender]       FLOAT (53)     NULL,
    [TotalChange]       FLOAT (53)     NULL,
    [Status]            NVARCHAR (MAX) NOT NULL,
    [StationId]         INT            NOT NULL,
    [OpeningCashier]    INT            NOT NULL,
    [ClosingCashier]    INT            NULL,
    [Sales]             FLOAT (53)     NOT NULL,
    [OpenTransactions]  INT            NOT NULL,
    [CloseTransactions] INT            NOT NULL,
    [EntryTimeStamp]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Batches] PRIMARY KEY CLUSTERED ([BatchId] ASC)
);


GO
PRINT N'Creating [dbo].[Batches].[IX_FK_StationBatch]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_StationBatch]
    ON [dbo].[Batches]([StationId] ASC);


GO
PRINT N'Creating [dbo].[Batches].[IX_FK_BatchCashier]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_BatchCashier]
    ON [dbo].[Batches]([OpeningCashier] ASC);


GO
PRINT N'Creating [dbo].[Batches].[IX_FK_BatchCashier1]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_BatchCashier1]
    ON [dbo].[Batches]([ClosingCashier] ASC);


GO
PRINT N'Creating [dbo].[CashierLogs]...';


GO
CREATE TABLE [dbo].[CashierLogs] (
    [CashierLogId]   INT            IDENTITY (1, 1) NOT NULL,
    [MachineName]    NVARCHAR (MAX) NOT NULL,
    [LoginTime]      DATETIME       NOT NULL,
    [LogoutTime]     DATETIME       NULL,
    [Status]         NVARCHAR (MAX) NOT NULL,
    [PersonId]       INT            NOT NULL,
    [EntryTimeStamp] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_CashierLogs] PRIMARY KEY CLUSTERED ([CashierLogId] ASC)
);


GO
PRINT N'Creating [dbo].[CashierLogs].[IX_FK_CashierCashierLog]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_CashierCashierLog]
    ON [dbo].[CashierLogs]([PersonId] ASC);


GO
PRINT N'Creating [dbo].[Company]...';


GO
CREATE TABLE [dbo].[Company] (
    [CompanyId]      INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName]    NVARCHAR (MAX) NOT NULL,
    [Address]        NVARCHAR (MAX) NOT NULL,
    [Address1]       NVARCHAR (MAX) NULL,
    [SoftwareName]   NVARCHAR (MAX) NOT NULL,
    [PhoneNumber]    NVARCHAR (MAX) NOT NULL,
    [Motto]          NVARCHAR (MAX) NOT NULL,
    [EntryTimeStamp] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED ([CompanyId] ASC)
);


GO
PRINT N'Creating [dbo].[Item]...';


GO
CREATE TABLE [dbo].[Item] (
    [Description]         NVARCHAR (1000) NULL,
    [ItemName]            NVARCHAR (50)   NULL,
    [ItemNumber]          NVARCHAR (50)   NULL,
    [ItemNotDiscountable] BIT             NULL,
    [ItemId]              INT             IDENTITY (1, 1) NOT NULL,
    [ItemLookupCode]      NVARCHAR (1000) NULL,
    [Department]          NVARCHAR (255)  NULL,
    [Category]            NVARCHAR (255)  NULL,
    [Price]               FLOAT (53)      NOT NULL,
    [Cost]                FLOAT (53)      NULL,
    [Quantity]            FLOAT (53)      NOT NULL,
    [ExtendedDescription] NVARCHAR (MAX)  NULL,
    [Inactive]            BIT             NULL,
    [DateCreated]         DATETIME        NULL,
    [SalesTax]            FLOAT (53)      NULL,
    [QBItemListID]        NVARCHAR (MAX)  NULL,
    [UnitOfMeasure]       NVARCHAR (50)   NULL,
    [Size]                NVARCHAR (50)   NULL,
    [EntryTimeStamp]      ROWVERSION      NOT NULL,
    [QBActive]            BIT             NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);


GO
PRINT N'Creating [dbo].[Item_Medicine]...';


GO
CREATE TABLE [dbo].[Item_Medicine] (
    [SuggestedDosage] NVARCHAR (MAX) NULL,
    [ItemId]          INT            NOT NULL,
    [ExpiryDate]      DATE           NULL,
    CONSTRAINT [PK_Item_Medicine] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);


GO
PRINT N'Creating [dbo].[Item_StockItem]...';


GO
CREATE TABLE [dbo].[Item_StockItem] (
    [ItemId] INT NOT NULL,
    CONSTRAINT [PK_Item_StockItem] PRIMARY KEY CLUSTERED ([ItemId] ASC)
);


GO
PRINT N'Creating [dbo].[ItemBkp]...';


GO
CREATE TABLE [dbo].[ItemBkp] (
    [Description]         NVARCHAR (1000) NOT NULL,
    [ItemNotDiscountable] BIT             NULL,
    [ItemId]              INT             IDENTITY (1, 1) NOT NULL,
    [ItemLookupCode]      NVARCHAR (1000) NULL,
    [Department]          NVARCHAR (255)  NULL,
    [Category]            NVARCHAR (255)  NULL,
    [Price]               DECIMAL (19, 4) NOT NULL,
    [Cost]                DECIMAL (19, 4) NULL,
    [Quantity]            FLOAT (53)      NOT NULL,
    [ExtendedDescription] NVARCHAR (MAX)  NULL,
    [Inactive]            BIT             NULL,
    [DateCreated]         DATETIME        NULL,
    [SalesTax]            DECIMAL (5, 3)  NULL,
    [QBItemListID]        NVARCHAR (MAX)  NULL
);


GO
PRINT N'Creating [dbo].[ItemmedicineBkp]...';


GO
CREATE TABLE [dbo].[ItemmedicineBkp] (
    [SuggestedDosage] NVARCHAR (MAX) NULL,
    [ItemId]          INT            NOT NULL,
    [ExpiryDate]      NVARCHAR (MAX) NULL
);


GO
PRINT N'Creating [dbo].[Persons]...';


GO
CREATE TABLE [dbo].[Persons] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]      NVARCHAR (MAX) NOT NULL,
    [LastName]       NVARCHAR (MAX) NOT NULL,
    [CompanyName]    NVARCHAR (MAX) NULL,
    [Salutation]     NVARCHAR (MAX) NULL,
    [Address]        NVARCHAR (MAX) NULL,
    [PhoneNumber]    NVARCHAR (MAX) NULL,
    [InActive]       BIT            NULL,
    [DOB]            DATETIME       NULL,
    [Sex]            BIT            NULL,
    [EntryTimeStamp] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Persons] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Persons_Cashier]...';


GO
CREATE TABLE [dbo].[Persons_Cashier] (
    [SPassword] NVARCHAR (MAX) NULL,
    [LoginName] NVARCHAR (MAX) NULL,
    [Id]        INT            NOT NULL,
    [Role]      NVARCHAR (50)  NULL,
    [Initials]  NVARCHAR (3)   NULL,
    CONSTRAINT [PK_Persons_Cashier] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Persons_Doctor]...';


GO
CREATE TABLE [dbo].[Persons_Doctor] (
    [Code]     NVARCHAR (MAX) NOT NULL,
    [Id]       INT            NOT NULL,
    [Discount] FLOAT (53)     NULL,
    CONSTRAINT [PK_Persons_Doctor] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Persons_Patient]...';


GO
CREATE TABLE [dbo].[Persons_Patient] (
    [Id]        INT            NOT NULL,
    [Allergies] NVARCHAR (MAX) NULL,
    [Guardian]  NVARCHAR (MAX) NULL,
    [Discount]  FLOAT (53)     NULL,
    CONSTRAINT [PK_Persons_Patient] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[QBInventoryItems]...';


GO
CREATE TABLE [dbo].[QBInventoryItems] (
    [ListID]         NVARCHAR (50)  NOT NULL,
    [ItemDesc2]      NVARCHAR (MAX) NULL,
    [ItemName]       NVARCHAR (MAX) NULL,
    [Size]           NVARCHAR (MAX) NULL,
    [DepartmentCode] NVARCHAR (MAX) NULL,
    [ItemNumber]     INT            NOT NULL,
    [TaxCode]        NVARCHAR (MAX) NULL,
    [Price]          FLOAT (53)     NULL,
    [Quantity]       FLOAT (53)     NULL,
    [UnitOfMeasure]  NVARCHAR (50)  NULL,
    [EntryTimeStamp] ROWVERSION     NOT NULL,
    CONSTRAINT [PK_QBInventoryItems] PRIMARY KEY CLUSTERED ([ListID] ASC)
);


GO
PRINT N'Creating [dbo].[quicksalesitemcsv]...';


GO
CREATE TABLE [dbo].[quicksalesitemcsv] (
    [Item Name]         VARCHAR (1000) NULL,
    [UPC]               VARCHAR (1000) NULL,
    [Department Name]   VARCHAR (1000) NULL,
    [Average Unit Cost] VARCHAR (1000) NULL,
    [Regular Price]     VARCHAR (1000) NULL,
    [Qty 1]             VARCHAR (1000) NULL
);


GO
PRINT N'Creating [dbo].[Repeats]...';


GO
CREATE TABLE [dbo].[Repeats] (
    [OldTransactionId] INT NOT NULL,
    [NewTransactionId] INT NOT NULL,
    CONSTRAINT [PK_Repeats_1] PRIMARY KEY CLUSTERED ([OldTransactionId] ASC, [NewTransactionId] ASC)
);


GO
PRINT N'Creating [dbo].[Stations]...';


GO
CREATE TABLE [dbo].[Stations] (
    [StationId]          INT            IDENTITY (1, 1) NOT NULL,
    [StationCode]        NVARCHAR (MAX) NOT NULL,
    [StoreId]            INT            NOT NULL,
    [ReceiptPrinterName] NVARCHAR (MAX) NOT NULL,
    [MachineName]        NVARCHAR (MAX) NOT NULL,
    [PrintServer]        NVARCHAR (MAX) NULL,
    [EntryTimeStamp]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Stations] PRIMARY KEY CLUSTERED ([StationId] ASC)
);


GO
PRINT N'Creating [dbo].[Stations].[IX_FK_StoreStation]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_StoreStation]
    ON [dbo].[Stations]([StoreId] ASC);


GO
PRINT N'Creating [dbo].[Stores]...';


GO
CREATE TABLE [dbo].[Stores] (
    [StoreId]         INT            IDENTITY (1, 1) NOT NULL,
    [StoreCode]       NVARCHAR (MAX) NOT NULL,
    [StoreAddress]    NVARCHAR (MAX) NOT NULL,
    [CompanyId]       INT            NOT NULL,
    [TransactionSeed] INT            NOT NULL,
    [SeedTransaction] INT            NOT NULL,
    [EntryTimeStamp]  ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED ([StoreId] ASC)
);


GO
PRINT N'Creating [dbo].[Stores].[IX_FK_CompanyStore]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_CompanyStore]
    ON [dbo].[Stores]([CompanyId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase]...';


GO
CREATE TABLE [dbo].[TransactionBase] (
    [StationId]           INT            NOT NULL,
    [BatchId]             INT            NOT NULL,
    [CloseBatchId]        INT            NULL,
    [Time]                DATETIME       NOT NULL,
    [CustomerId]          INT            NULL,
    [PharmacistId]        INT            NULL,
    [CashierId]           INT            NOT NULL,
    [Comment]             NVARCHAR (255) NULL,
    [ReferenceNumber]     NVARCHAR (50)  NULL,
    [StoreCode]           VARCHAR (30)   NULL,
    [TransactionId]       INT            IDENTITY (1, 1) NOT NULL,
    [OpenClose]           BIT            NOT NULL,
    [Status]              VARCHAR (50)   NULL,
    [EntryTimeStamp]      ROWVERSION     NOT NULL,
    [ParentTransactionId] INT            NULL,
    CONSTRAINT [PK_TransactionBase] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionBase].[IX_FK_CustomerTransaction]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_CustomerTransaction]
    ON [dbo].[TransactionBase]([CustomerId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase].[IX_FK_CashierTransactionBase]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_CashierTransactionBase]
    ON [dbo].[TransactionBase]([CashierId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase].[IX_FK_BatchTransactionBase]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_BatchTransactionBase]
    ON [dbo].[TransactionBase]([BatchId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase].[IX_FK_StationTransactionBase]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_StationTransactionBase]
    ON [dbo].[TransactionBase]([StationId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase].[IX_FK_BatchTransactionBase1]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_BatchTransactionBase1]
    ON [dbo].[TransactionBase]([CloseBatchId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase_Prescription]...';


GO
CREATE TABLE [dbo].[TransactionBase_Prescription] (
    [DoctorId]      INT NOT NULL,
    [PatientId]     INT NOT NULL,
    [TransactionId] INT NOT NULL,
    CONSTRAINT [PK_TransactionBase_Prescription] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionBase_Prescription].[IX_FK_DoctorPrescription]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_DoctorPrescription]
    ON [dbo].[TransactionBase_Prescription]([DoctorId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase_Prescription].[IX_FK_PatientPrescription]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_PatientPrescription]
    ON [dbo].[TransactionBase_Prescription]([PatientId] ASC);


GO
PRINT N'Creating [dbo].[TransactionBase_QuickPrescription]...';


GO
CREATE TABLE [dbo].[TransactionBase_QuickPrescription] (
    [TransactionId] INT NOT NULL,
    CONSTRAINT [PK_TransactionBase_QuickPrescription] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionEntryBase]...';


GO
CREATE TABLE [dbo].[TransactionEntryBase] (
    [StoreID]            INT            NOT NULL,
    [TransactionEntryId] INT            IDENTITY (1, 1) NOT NULL,
    [TransactionId]      INT            NOT NULL,
    [Price]              FLOAT (53)     NOT NULL,
    [Quantity]           FLOAT (53)     NOT NULL,
    [Taxable]            BIT            NOT NULL,
    [Comment]            NVARCHAR (MAX) NULL,
    [TransactionTime]    DATETIME       NULL,
    [SalesTaxPercent]    FLOAT (53)     NOT NULL,
    [Discount]           FLOAT (53)     NULL,
    [EntryNumber]        SMALLINT       NULL,
    [EntryTimeStamp]     ROWVERSION     NOT NULL,
    CONSTRAINT [PK_TransactionEntryBase] PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionEntryBase].[IX_FK_TransactionTransactionEntry]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_TransactionTransactionEntry]
    ON [dbo].[TransactionEntryBase]([TransactionId] ASC);


GO
PRINT N'Creating [dbo].[TransactionEntryBase_PrescriptionEntry]...';


GO
CREATE TABLE [dbo].[TransactionEntryBase_PrescriptionEntry] (
    [Dosage]             NVARCHAR (MAX) NULL,
    [ExpiryDate]         DATE           NULL,
    [TransactionEntryId] INT            NOT NULL,
    [Repeat]             INT            NULL,
    [RepeatQuantity]     INT            NULL,
    CONSTRAINT [PK_TransactionEntryBase_PrescriptionEntry] PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC)
);


GO
PRINT N'Creating [dbo].[TransactionEntryItem]...';


GO
CREATE TABLE [dbo].[TransactionEntryItem] (
    [TransactionEntryId] INT          NOT NULL,
    [QBItemListID]       VARCHAR (50) NULL,
    [ItemNumber]         VARCHAR (50) NULL,
    [ItemName]           VARCHAR (50) NOT NULL,
    [ItemId]             INT          NULL,
    CONSTRAINT [PK_TransactionEntryItem] PRIMARY KEY CLUSTERED ([TransactionEntryId] ASC)
);


GO
PRINT N'Creating [dbo].[FK_StationBatch]...';


GO
ALTER TABLE [dbo].[Batches] WITH NOCHECK
    ADD CONSTRAINT [FK_StationBatch] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations] ([StationId]);


GO
PRINT N'Creating [dbo].[FK_BatchCashier]...';


GO
ALTER TABLE [dbo].[Batches] WITH NOCHECK
    ADD CONSTRAINT [FK_BatchCashier] FOREIGN KEY ([OpeningCashier]) REFERENCES [dbo].[Persons_Cashier] ([Id]);


GO
PRINT N'Creating [dbo].[FK_BatchCashier1]...';


GO
ALTER TABLE [dbo].[Batches] WITH NOCHECK
    ADD CONSTRAINT [FK_BatchCashier1] FOREIGN KEY ([ClosingCashier]) REFERENCES [dbo].[Persons_Cashier] ([Id]);


GO
PRINT N'Creating [dbo].[FK_CashierCashierLog]...';


GO
ALTER TABLE [dbo].[CashierLogs] WITH NOCHECK
    ADD CONSTRAINT [FK_CashierCashierLog] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Persons_Cashier] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Medicine_inherits_Item]...';


GO
ALTER TABLE [dbo].[Item_Medicine] WITH NOCHECK
    ADD CONSTRAINT [FK_Medicine_inherits_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([ItemId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_StockItem_inherits_Item]...';


GO
ALTER TABLE [dbo].[Item_StockItem] WITH NOCHECK
    ADD CONSTRAINT [FK_StockItem_inherits_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([ItemId]) ON DELETE CASCADE;


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
PRINT N'Creating [dbo].[FK_Patient_inherits_Person]...';


GO
ALTER TABLE [dbo].[Persons_Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_inherits_Person] FOREIGN KEY ([Id]) REFERENCES [dbo].[Persons] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Repeats_TransactionBase]...';


GO
ALTER TABLE [dbo].[Repeats] WITH NOCHECK
    ADD CONSTRAINT [FK_Repeats_TransactionBase] FOREIGN KEY ([OldTransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Repeats_TransactionBase1]...';


GO
ALTER TABLE [dbo].[Repeats] WITH NOCHECK
    ADD CONSTRAINT [FK_Repeats_TransactionBase1] FOREIGN KEY ([NewTransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]);


GO
PRINT N'Creating [dbo].[FK_StoreStation]...';


GO
ALTER TABLE [dbo].[Stations] WITH NOCHECK
    ADD CONSTRAINT [FK_StoreStation] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[Stores] ([StoreId]);


GO
PRINT N'Creating [dbo].[FK_CompanyStore]...';


GO
ALTER TABLE [dbo].[Stores] WITH NOCHECK
    ADD CONSTRAINT [FK_CompanyStore] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]);


GO
PRINT N'Creating [dbo].[FK_TransactionBase_TransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_TransactionBase_TransactionBase] FOREIGN KEY ([ParentTransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]);


GO
PRINT N'Creating [dbo].[FK_BatchTransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_BatchTransactionBase] FOREIGN KEY ([BatchId]) REFERENCES [dbo].[Batches] ([BatchId]);


GO
PRINT N'Creating [dbo].[FK_BatchTransactionBase1]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_BatchTransactionBase1] FOREIGN KEY ([CloseBatchId]) REFERENCES [dbo].[Batches] ([BatchId]);


GO
PRINT N'Creating [dbo].[FK_CashierTransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_CashierTransactionBase] FOREIGN KEY ([CashierId]) REFERENCES [dbo].[Persons_Cashier] ([Id]);


GO
PRINT N'Creating [dbo].[FK_CustomerTransaction]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_CustomerTransaction] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Persons] ([Id]);


GO
PRINT N'Creating [dbo].[FK_StationTransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase] WITH NOCHECK
    ADD CONSTRAINT [FK_StationTransactionBase] FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations] ([StationId]);


GO
PRINT N'Creating [dbo].[FK_DoctorPrescription]...';


GO
ALTER TABLE [dbo].[TransactionBase_Prescription] WITH NOCHECK
    ADD CONSTRAINT [FK_DoctorPrescription] FOREIGN KEY ([DoctorId]) REFERENCES [dbo].[Persons_Doctor] ([Id]);


GO
PRINT N'Creating [dbo].[FK_PatientPrescription]...';


GO
ALTER TABLE [dbo].[TransactionBase_Prescription] WITH NOCHECK
    ADD CONSTRAINT [FK_PatientPrescription] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Persons_Patient] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Prescription_inherits_TransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase_Prescription] WITH NOCHECK
    ADD CONSTRAINT [FK_Prescription_inherits_TransactionBase] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_QuickPrescription_inherits_TransactionBase]...';


GO
ALTER TABLE [dbo].[TransactionBase_QuickPrescription] WITH NOCHECK
    ADD CONSTRAINT [FK_QuickPrescription_inherits_TransactionBase] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TransactionTransactionEntry]...';


GO
ALTER TABLE [dbo].[TransactionEntryBase] WITH NOCHECK
    ADD CONSTRAINT [FK_TransactionTransactionEntry] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[TransactionBase] ([TransactionId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_PrescriptionEntry_inherits_TransactionEntryBase]...';


GO
ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry] WITH NOCHECK
    ADD CONSTRAINT [FK_PrescriptionEntry_inherits_TransactionEntryBase] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntryBase] ([TransactionEntryId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_TransactionEntryItem_Item]...';


GO
ALTER TABLE [dbo].[TransactionEntryItem] WITH NOCHECK
    ADD CONSTRAINT [FK_TransactionEntryItem_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([ItemId]) ON DELETE SET NULL;


GO
PRINT N'Creating [dbo].[FK_TransactionEntryItem_TransactionEntryBase]...';


GO
ALTER TABLE [dbo].[TransactionEntryItem] WITH NOCHECK
    ADD CONSTRAINT [FK_TransactionEntryItem_TransactionEntryBase] FOREIGN KEY ([TransactionEntryId]) REFERENCES [dbo].[TransactionEntryBase] ([TransactionEntryId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[DoctorsView]...';


GO
CREATE VIEW dbo.DoctorsView
AS
SELECT dbo.Persons.FirstName + N' ' + dbo.Persons.LastName AS Name, dbo.Persons_Doctor.Code, dbo.Persons_Doctor.Id AS DoctorId
FROM   dbo.Persons_Doctor INNER JOIN
             dbo.Persons ON dbo.Persons_Doctor.Id = dbo.Persons.Id
GO
PRINT N'Creating [dbo].[ItemDosage]...';


GO

CREATE VIEW [dbo].[ItemDosage]
AS
SELECT TOP (100) PERCENT dbo.TransactionEntryItem.ItemId, dbo.TransactionEntryBase_PrescriptionEntry.Dosage, COUNT(dbo.TransactionEntryBase_PrescriptionEntry.Dosage) AS Count, ISNULL(row_number() over(order by itemid, dosage),0) as ID
FROM   dbo.TransactionEntryBase_PrescriptionEntry INNER JOIN
             dbo.TransactionEntryItem ON dbo.TransactionEntryBase_PrescriptionEntry.TransactionEntryId = dbo.TransactionEntryItem.TransactionEntryId
GROUP BY dbo.TransactionEntryItem.ItemId, dbo.TransactionEntryBase_PrescriptionEntry.Dosage
ORDER BY Count DESC
GO
PRINT N'Creating [dbo].[Patient History]...';


GO
CREATE VIEW dbo.[Patient History]
AS
SELECT        TOP (100) PERCENT dbo.Persons.FirstName + N' ' + dbo.Persons.LastName AS Patient, dbo.Persons.Address, dbo.Persons.PhoneNumber, Persons_1.FirstName + N' ' + Persons_1.LastName AS Doctor, 
                         dbo.TransactionBase.Time AS DateTime, dbo.TransactionEntryBase.Quantity, dbo.TransactionEntryItem.ItemName, dbo.TransactionEntryBase_PrescriptionEntry.Dosage
FROM            dbo.Persons_Patient INNER JOIN
                         dbo.Persons ON dbo.Persons_Patient.Id = dbo.Persons.Id INNER JOIN
                         dbo.TransactionBase_Prescription ON dbo.Persons_Patient.Id = dbo.TransactionBase_Prescription.PatientId INNER JOIN
                         dbo.TransactionBase ON dbo.TransactionBase_Prescription.TransactionId = dbo.TransactionBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase_PrescriptionEntry ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryBase_PrescriptionEntry.TransactionEntryId INNER JOIN
                         dbo.Persons_Doctor ON dbo.TransactionBase_Prescription.DoctorId = dbo.Persons_Doctor.Id INNER JOIN
                         dbo.Persons AS Persons_1 ON dbo.Persons_Doctor.Id = Persons_1.Id INNER JOIN
                         dbo.TransactionEntryItem ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryItem.TransactionEntryId
WHERE        (dbo.Persons.FirstName = N'HEATHER') AND (dbo.Persons.LastName = N'TOUSSAINT')
ORDER BY DateTime DESC
GO
PRINT N'Creating [dbo].[PatientLifeTimeValue]...';


GO
CREATE VIEW dbo.PatientLifeTimeValue
AS
SELECT        dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, dbo.Persons.PhoneNumber, COUNT(DISTINCT dbo.TransactionBase.TransactionId) AS Prescriptions, 
                         SUM(dbo.TransactionEntryBase.Price * dbo.TransactionEntryBase.Quantity) AS TotalValue
FROM            dbo.Persons INNER JOIN
                         dbo.Persons_Patient ON dbo.Persons.Id = dbo.Persons_Patient.Id INNER JOIN
                         dbo.TransactionBase_Prescription ON dbo.Persons_Patient.Id = dbo.TransactionBase_Prescription.PatientId INNER JOIN
                         dbo.TransactionBase ON dbo.TransactionBase_Prescription.TransactionId = dbo.TransactionBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId
GROUP BY dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, dbo.Persons.PhoneNumber
GO
PRINT N'Creating [dbo].[PatientView]...';


GO
CREATE VIEW dbo.PatientView
AS
SELECT dbo.Persons.FirstName + N' ' + dbo.Persons.LastName AS Name, dbo.Persons.PhoneNumber, dbo.Persons.Address, dbo.Persons.Id AS PatientId
FROM   dbo.Persons INNER JOIN
             dbo.Persons_Patient ON dbo.Persons.Id = dbo.Persons_Patient.Id
GO
PRINT N'Creating [dbo].[SearchView]...';


GO
CREATE VIEW dbo.SearchView
AS
SELECT        dbo.TransactionBase.Time, dbo.TransactionBase.TransactionId, dbo.Item.ItemId, ISNULL(dbo.Item.ItemName, dbo.Item.Description) AS ItemInfo, dbo.PatientView.PatientId, ISNULL(dbo.PatientView.Name, N'') 
                         + N' | ' + ISNULL(dbo.PatientView.Address, N'') + N'|' + ISNULL(dbo.PatientView.PhoneNumber, N'') AS PatientInfo, dbo.DoctorsView.DoctorId, ISNULL(dbo.DoctorsView.Name, N'') + N'|' + ISNULL(dbo.DoctorsView.Code, N'') 
                         AS DoctorInfo, ISNULL(dbo.PatientView.Name, N'') + N' | ' + ISNULL(dbo.PatientView.Address, N'') + N'|' + ISNULL(dbo.PatientView.PhoneNumber, N'') + N' | ' + ISNULL(dbo.Item.ItemName, N'') 
                         + N' | ' + ISNULL(dbo.Item.Description, N'') + '|' + CAST(dbo.TransactionBase.TransactionId AS nvarchar(50)) AS SearchInfo, dbo.TransactionBase_Prescription.ParentPrescriptionId
FROM            dbo.PatientView INNER JOIN
                         dbo.DoctorsView INNER JOIN
                         dbo.TransactionBase_Prescription INNER JOIN
                         dbo.TransactionBase ON dbo.TransactionBase_Prescription.TransactionId = dbo.TransactionBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId ON dbo.DoctorsView.DoctorId = dbo.TransactionBase_Prescription.DoctorId ON 
                         dbo.PatientView.PatientId = dbo.TransactionBase_Prescription.PatientId INNER JOIN
                         dbo.TransactionEntryBase_PrescriptionEntry ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryBase_PrescriptionEntry.TransactionEntryId INNER JOIN
                         dbo.TransactionEntryItem ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryItem.TransactionEntryId INNER JOIN
                         dbo.Item ON dbo.TransactionEntryItem.ItemId = dbo.Item.ItemId
GO
PRINT N'Creating [dbo].[TransactionsView]...';


GO
CREATE VIEW dbo.TransactionsView
AS
SELECT dbo.TransactionBase.TransactionId, dbo.TransactionBase.Time, dbo.TransactionBase.ReferenceNumber, dbo.TransactionEntryBase.Price * dbo.TransactionEntryBase.Quantity AS TotalSales, dbo.TransactionBase.CustomerId
FROM   dbo.TransactionBase INNER JOIN
             dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId
GROUP BY dbo.TransactionBase.TransactionId, dbo.TransactionBase.Time, dbo.TransactionBase.ReferenceNumber, dbo.TransactionEntryBase.Price * dbo.TransactionEntryBase.Quantity, dbo.TransactionBase.CustomerId
GO
PRINT N'Creating [dbo].[DoctorsView].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persons_Doctor"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 152
               Right = 279
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons"
            Begin Extent = 
               Top = 9
               Left = 336
               Bottom = 206
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 5
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1760
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'DoctorsView';


GO
PRINT N'Creating [dbo].[DoctorsView].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'DoctorsView';


GO
PRINT N'Creating [dbo].[ItemDosage].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[30] 4[27] 2[25] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TransactionEntryBase_PrescriptionEntry"
            Begin Extent = 
               Top = 3
               Left = 44
               Bottom = 200
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryItem"
            Begin Extent = 
               Top = 9
               Left = 1060
               Bottom = 206
               Right = 1326
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1000
         Width = 2730
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 3280
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ItemDosage';


GO
PRINT N'Creating [dbo].[ItemDosage].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'ItemDosage';


GO
PRINT N'Creating [dbo].[Patient History].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[19] 2[9] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persons_Patient"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 423
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "TransactionBase_Prescription"
            Begin Extent = 
               Top = 6
               Left = 461
               Bottom = 119
               Right = 688
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionBase"
            Begin Extent = 
               Top = 120
               Left = 461
               Bottom = 250
               Right = 646
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryBase"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 244
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryBase_PrescriptionEntry"
            Begin Extent = 
               Top = 138
               Left = 264
               Bottom = 268
               Right = 472
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons_Doctor"
            Begin Extent = 
               Top = 252
               Left = 490
               Bo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Patient History';


GO
PRINT N'Creating [dbo].[Patient History].[MS_DiagramPane2]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'ttom = 348
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons_1"
            Begin Extent = 
               Top = 270
               Left = 38
               Bottom = 400
               Right = 215
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryItem"
            Begin Extent = 
               Top = 270
               Left = 253
               Bottom = 400
               Right = 441
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1005
         Width = 3180
         Width = 1500
         Width = 1500
         Width = 5685
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 3165
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Patient History';


GO
PRINT N'Creating [dbo].[Patient History].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Patient History';


GO
PRINT N'Creating [dbo].[PatientLifeTimeValue].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persons"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 231
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons_Patient"
            Begin Extent = 
               Top = 6
               Left = 269
               Bottom = 136
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionBase_Prescription"
            Begin Extent = 
               Top = 6
               Left = 493
               Bottom = 119
               Right = 679
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionBase"
            Begin Extent = 
               Top = 6
               Left = 717
               Bottom = 136
               Right = 918
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryBase"
            Begin Extent = 
               Top = 6
               Left = 956
               Bottom = 136
               Right = 1160
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PatientLifeTimeValue';


GO
PRINT N'Creating [dbo].[PatientLifeTimeValue].[MS_DiagramPane2]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N' = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PatientLifeTimeValue';


GO
PRINT N'Creating [dbo].[PatientLifeTimeValue].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PatientLifeTimeValue';


GO
PRINT N'Creating [dbo].[PatientView].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[8] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Persons"
            Begin Extent = 
               Top = 9
               Left = 57
               Bottom = 206
               Right = 285
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Persons_Patient"
            Begin Extent = 
               Top = 9
               Left = 342
               Bottom = 206
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 2040
         Width = 1670
         Width = 1890
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PatientView';


GO
PRINT N'Creating [dbo].[PatientView].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'PatientView';


GO
PRINT N'Creating [dbo].[SearchView].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 8
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = -288
      End
      Begin Tables = 
         Begin Table = "PatientView"
            Begin Extent = 
               Top = 352
               Left = 1400
               Bottom = 549
               Right = 1622
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DoctorsView"
            Begin Extent = 
               Top = 9
               Left = 1291
               Bottom = 179
               Right = 1513
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionBase_Prescription"
            Begin Extent = 
               Top = 0
               Left = 957
               Bottom = 170
               Right = 1299
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionBase"
            Begin Extent = 
               Top = 0
               Left = 598
               Bottom = 224
               Right = 896
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryBase"
            Begin Extent = 
               Top = 340
               Left = 671
               Bottom = 537
               Right = 971
            End
            DisplayFlags = 280
            TopColumn = 7
         End
         Begin Table = "TransactionEntryBase_PrescriptionEntry"
            Begin Extent = 
               Top = 0
               Left = 128
               Bottom = 220
               Right = 590
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "TransactionEntryItem"
            Begin Extent = 
               Top = 247
               Left = 1086
    ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'SearchView';


GO
PRINT N'Creating [dbo].[SearchView].[MS_DiagramPane2]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N'           Bottom = 444
               Right = 1336
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Item"
            Begin Extent = 
               Top = 134
               Left = 20
               Bottom = 331
               Right = 293
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 10
         Width = 284
         Width = 1005
         Width = 1005
         Width = 1005
         Width = 3030
         Width = 1005
         Width = 1005
         Width = 1005
         Width = 1005
         Width = 1005
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 9270
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'SearchView';


GO
PRINT N'Creating [dbo].[SearchView].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'SearchView';


GO
PRINT N'Creating [dbo].[TransactionsView].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TransactionBase"
            Begin Extent = 
               Top = 1
               Left = 34
               Bottom = 277
               Right = 281
            End
            DisplayFlags = 280
            TopColumn = 6
         End
         Begin Table = "TransactionEntryBase"
            Begin Extent = 
               Top = 3
               Left = 442
               Bottom = 200
               Right = 692
            End
            DisplayFlags = 280
            TopColumn = 4
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
         Width = 1000
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'TransactionsView';


GO
PRINT N'Creating [dbo].[TransactionsView].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'TransactionsView';


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Batches] WITH CHECK CHECK CONSTRAINT [FK_StationBatch];

ALTER TABLE [dbo].[Batches] WITH CHECK CHECK CONSTRAINT [FK_BatchCashier];

ALTER TABLE [dbo].[Batches] WITH CHECK CHECK CONSTRAINT [FK_BatchCashier1];

ALTER TABLE [dbo].[CashierLogs] WITH CHECK CHECK CONSTRAINT [FK_CashierCashierLog];

ALTER TABLE [dbo].[Item_Medicine] WITH CHECK CHECK CONSTRAINT [FK_Medicine_inherits_Item];

ALTER TABLE [dbo].[Item_StockItem] WITH CHECK CHECK CONSTRAINT [FK_StockItem_inherits_Item];

ALTER TABLE [dbo].[Persons_Cashier] WITH CHECK CHECK CONSTRAINT [FK_Cashier_inherits_Person];

ALTER TABLE [dbo].[Persons_Doctor] WITH CHECK CHECK CONSTRAINT [FK_Doctor_inherits_Person];

ALTER TABLE [dbo].[Persons_Patient] WITH CHECK CHECK CONSTRAINT [FK_Patient_inherits_Person];

ALTER TABLE [dbo].[Repeats] WITH CHECK CHECK CONSTRAINT [FK_Repeats_TransactionBase];

ALTER TABLE [dbo].[Repeats] WITH CHECK CHECK CONSTRAINT [FK_Repeats_TransactionBase1];

ALTER TABLE [dbo].[Stations] WITH CHECK CHECK CONSTRAINT [FK_StoreStation];

ALTER TABLE [dbo].[Stores] WITH CHECK CHECK CONSTRAINT [FK_CompanyStore];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_TransactionBase_TransactionBase];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_BatchTransactionBase];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_BatchTransactionBase1];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_CashierTransactionBase];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_CustomerTransaction];

ALTER TABLE [dbo].[TransactionBase] WITH CHECK CHECK CONSTRAINT [FK_StationTransactionBase];

ALTER TABLE [dbo].[TransactionBase_Prescription] WITH CHECK CHECK CONSTRAINT [FK_DoctorPrescription];

ALTER TABLE [dbo].[TransactionBase_Prescription] WITH CHECK CHECK CONSTRAINT [FK_PatientPrescription];

ALTER TABLE [dbo].[TransactionBase_Prescription] WITH CHECK CHECK CONSTRAINT [FK_Prescription_inherits_TransactionBase];

ALTER TABLE [dbo].[TransactionBase_QuickPrescription] WITH CHECK CHECK CONSTRAINT [FK_QuickPrescription_inherits_TransactionBase];

ALTER TABLE [dbo].[TransactionEntryBase] WITH CHECK CHECK CONSTRAINT [FK_TransactionTransactionEntry];

ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry] WITH CHECK CHECK CONSTRAINT [FK_PrescriptionEntry_inherits_TransactionEntryBase];

ALTER TABLE [dbo].[TransactionEntryItem] WITH CHECK CHECK CONSTRAINT [FK_TransactionEntryItem_Item];

ALTER TABLE [dbo].[TransactionEntryItem] WITH CHECK CHECK CONSTRAINT [FK_TransactionEntryItem_TransactionEntryBase];


GO
PRINT N'Update complete.';


GO
