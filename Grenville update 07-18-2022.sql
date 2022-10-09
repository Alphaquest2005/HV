GO
PRINT N'Altering Table [dbo].[Persons_Patient]...';


GO
ALTER TABLE [dbo].[Persons_Patient]
    ADD [CardId] NVARCHAR (50) NULL;


GO
PRINT N'Creating Index [dbo].[Persons_Patient].[IX_Persons_Patient]...';


GO
CREATE NONCLUSTERED INDEX [IX_Persons_Patient]
    ON [dbo].[Persons_Patient]([Id] ASC, [CardId] ASC);


GO
PRINT N'Altering Table [dbo].[QBCustomer]...';


GO
ALTER TABLE [dbo].[QBCustomer]
    ADD [CustomerId] NVARCHAR (50) NULL;


GO
PRINT N'Altering Table [dbo].[TransactionEntryBase_PrescriptionEntry]...';


GO
ALTER TABLE [dbo].[TransactionEntryBase_PrescriptionEntry]
    ADD [isExtension] BIT NULL;


GO
PRINT N'Creating Table [dbo].[PrescriptionImage]...';


GO
CREATE TABLE [dbo].[PrescriptionImage] (
    [TransactionId] INT            NOT NULL,
    [Image]         IMAGE          NOT NULL,
    [Path]          NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_PrescriptionImage] PRIMARY KEY CLUSTERED ([TransactionId] ASC)
);


GO
PRINT N'Creating Index [dbo].[Persons].[IX_Persons]...';


GO
CREATE NONCLUSTERED INDEX [IX_Persons]
    ON [dbo].[Persons]([Id] ASC, [FirstName] ASC, [LastName] ASC, [PhoneNumber] ASC);


GO
PRINT N'Creating Foreign Key [dbo].[FK_PrescriptionImage_TransactionBase_Prescription]...';


GO
ALTER TABLE [dbo].[PrescriptionImage] WITH NOCHECK
    ADD CONSTRAINT [FK_PrescriptionImage_TransactionBase_Prescription] FOREIGN KEY ([TransactionId]) REFERENCES [dbo].[TransactionBase_Prescription] ([TransactionId]) ON DELETE CASCADE;


GO
PRINT N'Altering View [dbo].[Patient History]...';


GO


ALTER VIEW [dbo].[Patient History]
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
WHERE        (dbo.Persons.FirstName = N'Ronald') AND (dbo.Persons.LastName = N'Brathwaite')
ORDER BY DateTime DESC
GO
PRINT N'Altering View [dbo].[PatientLifeTimeValue]...';


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
WHERE        (dbo.TransactionBase.Time >= CONVERT(DATETIME, '2017-01-01 00:00:00', 102) AND dbo.TransactionBase.Time <= CONVERT(DATETIME, '2017-12-31 00:00:00', 102))
GROUP BY dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, dbo.Persons.PhoneNumber, dbo.Persons.Id
ORDER BY TotalValue DESC
GO
PRINT N'Altering View [dbo].[PatientMembership]...';


GO

ALTER VIEW [dbo].[PatientMembership]
AS

SELECT patient.Id AS PatientId,(ISNULL(TotalSales, 0) - ISNULL(StartingSales, 0)) *  MembershipTypes.PointRatePerDollar AS Points, Name as Membership, MembershipTypes.Id as MembershipId,  DATEDIFF(Year, patient.DOB, GETDATE()) AS Age
FROM    (SELECT Persons.Id, Persons.DOB, Persons_Patient.TotalSales, Persons_Patient.StartingSales
                 FROM     Persons INNER JOIN
                                  Persons_Patient ON Persons_Patient.Id = Persons.Id) AS patient INNER JOIN
                 MembershipTypes ON patient.TotalSales BETWEEN MembershipTypes.EntrySalesAmount AND MembershipTypes.MaxSalesAmount
WHERE  (DATEDIFF(Year, isnull(patient.DOB,getdate()), GETDATE()) BETWEEN isnull(MembershipTypes.EntryAge,0) AND isnull(MembershipTypes.MaxAge,60))
GO
PRINT N'Refreshing View [dbo].[PatientSales]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[PatientSales]';


GO
PRINT N'Refreshing View [dbo].[PatientView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[PatientView]';


GO
PRINT N'Refreshing View [dbo].[SearchView]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[SearchView]';


GO
PRINT N'Refreshing View [dbo].[ItemDosage]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[ItemDosage]';


GO
PRINT N'Refreshing View [dbo].[RepeatPatientList]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[RepeatPatientList]';


GO
PRINT N'Altering View [dbo].[TransactionData]...';


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
                 Pharmacist.LastName AS PharmacistLastName, dbo.Item.Description, TransactionEntryBase_PrescriptionEntry.isExtension
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
PRINT N'Creating View [dbo].[DuplicatePatients]...';


GO

Create view DuplicatePatients
as
select * 
from (select Persons.id as Id, firstname, lastname, phonenumber,sex, address, DOB, ROW_NUMBER() over (partition by firstname,lastname, isnull(phonenumber, address) order by Persons.id) as row from Persons inner join Persons_Patient on Persons.id = Persons_Patient.Id) as t where row > 1
GO
PRINT N'Creating View [dbo].[Find Paitents by ItemName]...';


GO
CREATE VIEW dbo.[Find Paitents by ItemName]
AS
SELECT        TOP (100) PERCENT dbo.TransactionBase_Prescription.ParentPrescriptionId, dbo.TransactionEntryItem.ItemName, dbo.Persons.Id, dbo.Persons.FirstName, dbo.Persons.LastName, dbo.Persons.Address, 
                         dbo.Persons.PhoneNumber, dbo.TransactionBase.Time
FROM            dbo.TransactionBase_Prescription INNER JOIN
                         dbo.TransactionBase ON dbo.TransactionBase_Prescription.TransactionId = dbo.TransactionBase.TransactionId INNER JOIN
                         dbo.TransactionEntryBase ON dbo.TransactionBase.TransactionId = dbo.TransactionEntryBase.TransactionId INNER JOIN
                         dbo.TransactionEntryItem ON dbo.TransactionEntryBase.TransactionEntryId = dbo.TransactionEntryItem.TransactionEntryId INNER JOIN
                         dbo.Persons ON dbo.TransactionBase_Prescription.PatientId = dbo.Persons.Id
WHERE        (dbo.TransactionEntryItem.ItemName = 'apo-bromocriptine 2.5mg')
ORDER BY dbo.TransactionBase.Time DESC
GO
PRINT N'Creating View [dbo].[IDCardInfoData]...';


GO


CREATE VIEW [dbo].[IDCardInfoData]
AS


SELECT Persons.Id as PatientId, Persons.FirstName, Persons.LastName, Persons.Address, Persons.PhoneNumber, 
				cast(LEFT(Persons.FirstName, 1) + LEFT(Persons.LastName, 1) + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(persons.phonenumber, 3) = '473' THEN substring(Persons.PhoneNumber, 4, 7) ELSE persons.phonenumber END)), 7) as nvarchar(15)) AS CardId, Persons_Patient.TotalSales, 
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
--Union All
--select * from [Grenville].[QuickSales-Enterprise].[dbo].[IDCardInfo]
GO
PRINT N'Altering View [dbo].[IDCardInfo]...';


GO



ALTER VIEW [dbo].[IDCardInfo]
AS
SELECT Persons.Id as PatientId, Persons.FirstName, Persons.LastName, Persons.Address, Persons.PhoneNumber,
				coalesce(Persons_Patient.Cardid,
				cast(LEFT(Persons.FirstName, 1) + LEFT(Persons.LastName, 1) + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(persons.phonenumber, 3) = '473' THEN substring(Persons.PhoneNumber, 4, 7) ELSE persons.phonenumber END)), 7) as nvarchar(15))) AS CardId,
				 Persons_Patient.TotalSales, 
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
                 MembershipTypes.QuickBooksPriceLevel, Company.Address, MembershipTypes.Id, Persons_Patient.Cardid
GO
PRINT N'Refreshing View [dbo].[PatientAvailableRewards]...';


GO
EXECUTE sp_refreshsqlmodule N'[dbo].[PatientAvailableRewards]';


GO
PRINT N'Creating View [dbo].[CardIdTobeUpdated]...';


GO

CREATE VIEW [dbo].[CardIdTobeUpdated]
AS
SELECT dbo.Persons_Patient.CardId, dbo.Persons_Patient.Id, dbo.QBCustomer.CustomerListID
FROM     dbo.Persons_Patient INNER JOIN
                  dbo.QBCustomer ON dbo.Persons_Patient.Id = dbo.QBCustomer.Id
WHERE  (dbo.Persons_Patient.CardId IS NOT NULL) and (dbo.Persons_Patient.CardId <> isnull(dbo.QBCustomer.CustomerID,'')) AND (dbo.Persons_Patient.CardId NOT IN
                      (SELECT ISNULL(CardId, '') AS Expr1
                       FROM      dbo.IDCardInfoData))
GO