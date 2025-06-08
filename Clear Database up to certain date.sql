SELECT   MIN(Time) AS Expr1
FROM      TransactionBase


delete from TransactionBase where time >= '3/1/2020'


delete from Persons where id not  in
(
SELECT   Persons.Id FROM      Persons INNER JOIN                TransactionBase ON Persons.Id = TransactionBase.CustomerId
union
SELECT   Persons.Id FROM      Persons INNER JOIN                TransactionBase ON Persons.Id = TransactionBase.PharmacistId
union
SELECT   Persons.Id FROM      Persons INNER JOIN                TransactionBase ON Persons.Id = TransactionBase.CashierId
union
SELECT   Persons.Id
FROM      TransactionBase_Prescription INNER JOIN
                TransactionBase ON TransactionBase_Prescription.TransactionId = TransactionBase.TransactionId INNER JOIN
                Persons ON TransactionBase_Prescription.DoctorId = Persons.Id
union
SELECT   Persons.Id
FROM      TransactionBase_Prescription INNER JOIN
                TransactionBase ON TransactionBase_Prescription.TransactionId = TransactionBase.TransactionId INNER JOIN
                Persons ON TransactionBase_Prescription.PatientId = Persons.Id

union
SELECT   Persons.Id
FROM      TransactionBase_QuickPrescription INNER JOIN
                TransactionBase ON TransactionBase_QuickPrescription.TransactionId = TransactionBase.TransactionId INNER JOIN
                Persons ON TransactionBase_QuickPrescription.PatientId = Persons.Id
		
				
				)

delete from item where itemid not in (
SELECT   Item.ItemId
FROM      Item INNER JOIN
                TransactionEntryItem ON Item.ItemId = TransactionEntryItem.ItemId INNER JOIN
                TransactionEntryBase ON TransactionEntryItem.TransactionEntryId = TransactionEntryBase.TransactionEntryId
GROUP BY Item.ItemId)