USE [QuickSales-Enterprise]
GO

/****** Object:  UserDefinedFunction [dbo].[GetOnlyNumbers]    Script Date: 9/17/2020 4:24:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetOnlyNumbers] (@Temp VARCHAR(1000))
RETURNS VARCHAR (1000) AS 
BEGIN
    DECLARE @KeepValues AS VARCHAR(50)
    SET @KeepValues = '%[^0-9]%'
    WHILE PATINDEX(@KeepValues, @Temp) > 0
        SET @Temp = STUFF(@Temp, PATINDEX(@KeepValues, @Temp), 1, '')

    RETURN @Temp
END
GO


