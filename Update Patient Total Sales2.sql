SELECT Persons_Patient.Id, Persons.FirstName, Persons.LastName, SUM(QBSalesDetail.Total) AS TotalSales
FROM    Persons_Patient INNER JOIN
                 QBCustomer ON Persons_Patient.Id = QBCustomer.Id INNER JOIN
                 Persons ON Persons_Patient.Id = Persons.Id INNER JOIN
                 QBSales INNER JOIN
                 QBSalesDetail ON QBSales.Id = QBSalesDetail.Id ON QBCustomer.Id = QBSales.QBCustomerId
GROUP BY Persons_Patient.Id, Persons.FirstName, Persons.LastName

UPDATE Persons_Patient
SET         TotalSales = customerSales.TotalSales
FROM    Persons_Patient INNER JOIN
                 (SELECT Persons_Patient.Id, Persons.FirstName, Persons.LastName, SUM(QBSalesDetail.Total) AS TotalSales
FROM    Persons_Patient INNER JOIN
                 QBCustomer ON Persons_Patient.Id = QBCustomer.Id INNER JOIN
                 Persons ON Persons_Patient.Id = Persons.Id INNER JOIN
                 QBSales INNER JOIN
                 QBSalesDetail ON QBSales.Id = QBSalesDetail.Id ON QBCustomer.Id = QBSales.QBCustomerId
GROUP BY Persons_Patient.Id, Persons.FirstName, Persons.LastName) as customerSales on Persons_Patient.Id = customerSales.Id