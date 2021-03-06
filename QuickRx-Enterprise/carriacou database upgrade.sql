/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [DoctorId]
      ,[PatientId]
      ,[TransactionId]
  FROM [QuickSales-carriacou].[dbo].[TransactionBase_Prescription]
    where patientid is null or doctorid is null

	update [TransactionBase_Prescription]
	set DoctorId = 0
	where doctorid is null

		update [TransactionBase_Prescription]
	set PatientId = 0
	where PatientId is null


		SET IDENTITY_INSERT Persons on; 
	insert into Persons(id, FirstName, LastName) values (0, 'John', 'Doe')
SET IDENTITY_INSERT Persons off; 


	insert into Persons_doctor(id, code) values (0, '0')



	insert into Persons_patient(id) values (0)

	update item
	set QBActive = 0


  insert into [QuickSales-carriacou].dbo.TransactionEntryItem 
			(TransactionEntryId,ItemId,ItemNumber,QBItemListID,ItemName)
SELECT        TransactionEntryBase.TransactionEntryId, Item.ItemId, QBInventoryItems.ItemNumber, Item.QBItemListID, QBInventoryItems.ItemName
FROM            TransactionEntryBase INNER JOIN
                         Item ON TransactionEntryBase.ItemId = Item.ItemId INNER JOIN
                         QBInventoryItems ON Item.QBItemListID = QBInventoryItems.ListID
UPDATE       Item
SET                ItemNumber = QBInventoryItems.ItemNumber, QBActive = 1
FROM            Item INNER JOIN
                         TransactionEntryItem ON Item.ItemId = TransactionEntryItem.ItemId INNER JOIN
                         QBInventoryItems ON TransactionEntryItem.QBItemListID = QBInventoryItems.ListID