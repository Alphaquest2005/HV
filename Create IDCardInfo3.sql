USE [QuickSales-Enterprise]
GO

/****** Object:  View [dbo].[IDCardInfo]    Script Date: 9/17/2020 5:25:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[IDCardInfo]
AS
SELECT Id, FirstName, LastName, Address, PhoneNumber, LEFT(FirstName, 1) + LEFT(LastName, 1) + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(phonenumber, 3) = '473' THEN substring(PhoneNumber, 4, 7) 
                 ELSE phonenumber END)), 7) AS CardId
FROM    dbo.Persons
GO


