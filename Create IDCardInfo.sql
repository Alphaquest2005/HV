

CREATE FUNCTION [dbo].[GetOnlyNumbers] (@Temp VARCHAR(1000))
RETURNS VARCHAR (1000) AS 
BEGIN
    DECLARE @KeepValues AS VARCHAR(50)
    SET @KeepValues = '%[^0-9]%'
    WHILE PATINDEX(@KeepValues, @Temp) > 0
        SET @Temp = STUFF(@Temp, PATINDEX(@KeepValues, @Temp), 1, '')

    RETURN @Temp
END

CREATE VIEW [dbo].[IDCardInfo]
AS
SELECT FirstName, LastName, Address, PhoneNumber, LEFT(FirstName, 1) + LEFT(LastName, 1) + LEFT(dbo.GetOnlyNumbers((CASE WHEN LEFT(phonenumber, 3) = '473' THEN substring(PhoneNumber, 4, 7) 
                 ELSE phonenumber END)), 7) AS CardId
FROM    dbo.Persons
GO