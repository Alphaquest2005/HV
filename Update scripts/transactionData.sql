USE [QuickSales-Enterprise]
GO

/****** Object:  View [dbo].[TransactionData]    Script Date: 12/20/2021 8:28:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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


