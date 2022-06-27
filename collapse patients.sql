

drop table #persons
select * 
into #persons
from (select Persons.id as Id, firstname, lastname, phonenumber,sex, address, DOB, ROW_NUMBER() over (partition by trim(firstname),lastname, isnull(phonenumber, address) order by Persons.id) as row from Persons inner join Persons_Patient on Persons.id = Persons_Patient.Id) as t --where row > 5


select * from #persons where row > 1 order by firstname, lastname 

select * from #persons where firstname = 'Andrea' and lastname = 'Charles'

update TransactionBase_Prescription
SET         PatientId = original.Id
--SELECT original.id, Dups.id AS Expr1, Dups.firstname, Dups.lastname, Dups.[key]
FROM    (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                 FROM     [#persons]
                 WHERE  (row = 1)) AS original INNER JOIN
                     (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                      FROM     [#persons] AS [#persons_1]
                      WHERE  (row > 1)) AS Dups ON original.firstname = Dups.firstname AND original.lastname = Dups.lastname AND isnull(original.[key],'') = isnull(Dups.[key],'') INNER JOIN
                 TransactionBase_Prescription ON Dups.id = TransactionBase_Prescription.PatientId

update TransactionBase_QuickPrescription
SET         PatientId = original.Id
--SELECT original.id, Dups.id AS Expr1, Dups.firstname, Dups.lastname, Dups.[key]
FROM    (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                 FROM     [#persons]
                 WHERE  (row = 1)) AS original INNER JOIN
                     (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                      FROM     [#persons] AS [#persons_1]
                      WHERE  (row > 1)) AS Dups ON original.firstname = Dups.firstname AND original.lastname = Dups.lastname AND  isnull(original.[key],'') = isnull(Dups.[key],'') INNER JOIN
                 TransactionBase_QuickPrescription ON Dups.id = TransactionBase_QuickPrescription.PatientId


--SELECT original.id, Dups.id AS Expr1, Dups.firstname, Dups.lastname, Dups.phonenumber
--FROM    (SELECT id, firstname, lastname, phonenumber, sex, address, DOB
--                 FROM     [#persons]
--                 WHERE  (row = 1)) AS original INNER JOIN
--                     (SELECT id, firstname, lastname, phonenumber, sex, address, DOB
--                      FROM     [#persons] AS [#persons_1]
--                      WHERE  (row > 1)) AS Dups ON original.firstname = Dups.firstname AND original.lastname = Dups.lastname AND original.phonenumber = Dups.phonenumber INNER JOIN
--                 TransactionBase_Prescription ON Dups.id = TransactionBase_Prescription.PatientId


--SELECT original.id, Dups.id AS Expr1, Dups.firstname, Dups.lastname, Dups.phonenumber
--FROM    (SELECT id, firstname, lastname, phonenumber, sex, address, DOB
--                 FROM     [#persons]
--                 WHERE  (row = 1)) AS original INNER JOIN
--                     (SELECT id, firstname, lastname, phonenumber, sex, address, DOB
--                      FROM     [#persons] AS [#persons_1]
--                      WHERE  (row > 1)) AS Dups ON original.firstname = Dups.firstname AND original.lastname = Dups.lastname AND original.phonenumber = Dups.phonenumber INNER JOIN
--                 TransactionBase_QuickPrescription ON Dups.id = TransactionBase_QuickPrescription.PatientId


--SELECT Persons.FirstName, Persons.LastName, Persons.id, TransactionBase_Prescription.TransactionId
--FROM    Persons INNER JOIN
--                 TransactionBase_Prescription ON Persons.Id = TransactionBase_Prescription.PatientId
--WHERE (Persons.Id IN (53348, 53927))


--SELECT Persons.FirstName, Persons.LastName, Persons.id, TransactionBase_QuickPrescription.TransactionId
--FROM    Persons INNER JOIN
--                 TransactionBase_QuickPrescription ON Persons.Id = TransactionBase_QuickPrescription.PatientId
--WHERE (Persons.Id IN (	1150))




Update Persons
set sex = t.Sex, address = t.Address, DOB = t.DOB
from (SELECT original.id, Dups.id AS Expr1, isnull(original.Sex, Dups.Sex) as Sex, isnull(original.Address,Dups.Address) as Address, isnull(original.DOB,Dups.DOB) as DOB
FROM    (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                 FROM     [#persons]
                 WHERE  (row = 1)) AS original INNER JOIN
                     (SELECT id, firstname, lastname, isnull(phonenumber, address) as [key], sex, address, DOB
                      FROM     [#persons] AS [#persons_1]
                      WHERE  (row > 1)) AS Dups ON original.firstname = Dups.firstname AND original.lastname = Dups.lastname AND original.[key] = Dups.[key]) as t INNER JOIN
                 Persons ON t.id = Persons.Id
---WHERE (Persons.Id IN (53348, 53927))


update persons
set InActive = 1
where id in (select id from #persons where row > 1)


delete from persons
--select * 
from Persons inner join 
 (select id from #persons 
				left outer join 
					(select isnull(patientid,0) as patientid from TransactionBase_Prescription union select isnull(patientid,0) as patientid from TransactionBase_QuickPrescription) as trans
				on #persons.id = trans.patientid where trans.patientid is null and row > 1 ) as t on Persons.id = t.Id



--SELECT FirstName, LastName, Id
--FROM    Persons
--WHERE (Id IN (54726, 71780, 71782))



--SELECT FirstName, LastName, Id, sex, address, dob
--FROM    Persons
--WHERE (Persons.Id IN (1150))

--select * from (select patientid from TransactionBase_Prescription union select patientid from TransactionBase_QuickPrescription) as t where patientid = 81038


--select * from (select patientid from TransactionBase_Prescription union select patientid from TransactionBase_QuickPrescription) as t right outer join #persons on t.patientid = #persons.id
--where #persons.row > 1 and t.PatientId 

--select * from #persons where row > 1 and id = 27671


--select * from TransactionBase_Prescription where patientid = 72834

Create view DuplicatePatients
as
select * 
from (select Persons.id as Id, firstname, lastname, phonenumber,sex, address, DOB, ROW_NUMBER() over (partition by firstname,lastname, isnull(phonenumber, address) order by Persons.id) as row from Persons inner join Persons_Patient on Persons.id = Persons_Patient.Id) as t where row > 1


