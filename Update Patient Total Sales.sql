/****** Script for SelectTopNRows command from SSMS  ******/
UPDATE Persons_Patient
SET         TotalSales = PatientLifeTimeValue.TotalValue
FROM    PatientLifeTimeValue INNER JOIN
                 Persons_Patient ON PatientLifeTimeValue.Id = Persons_Patient.Id