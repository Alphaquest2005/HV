
--delete from item where itemid in 
--(SELECT Item.ItemId
--FROM   Item LEFT OUTER JOIN
--             TransactionEntryBase ON Item.ItemId = TransactionEntryBase.ItemId
--WHERE (TransactionEntryBase.ItemId IS NULL))

--UPDATE TransactionEntryBase
--SET       ItemId = dd.Expr1

SELECT TransactionEntryBase.ItemId, dd.Expr1
FROM   TransactionEntryBase INNER JOIN
                 (SELECT Item.ItemId, d.ItemId AS Expr1
                 FROM    Item INNER JOIN
                                  (SELECT TOP (100) PERCENT MIN(ItemId) AS ItemId, ItemNumber, COUNT(ItemNumber) AS Expr2
                                  FROM    Item AS Item_1
                                  GROUP BY ItemNumber
                                  HAVING (COUNT(ItemNumber) > 1)
                                  ORDER BY CAST(ItemNumber AS int)) AS d ON Item.ItemNumber = d.ItemNumber
                 WHERE (d.ItemNumber = 260)) AS dd ON TransactionEntryBase.ItemId = dd.ItemId


select * from TransactionEntryBase where itemid = 376006



SELECT Item.ItemId, d.ItemId AS Expr1
FROM   Item INNER JOIN
                 (SELECT TOP (100) PERCENT MIN(ItemId) AS ItemId, ItemNumber, COUNT(ItemNumber) AS Expr2
                 FROM    Item AS Item_1
                 GROUP BY ItemNumber
                 HAVING (COUNT(ItemNumber) > 1)
                 ORDER BY CAST(ItemNumber AS int)) AS d ON Item.ItemNumber = d.ItemNumber
WHERE (d.ItemNumber = 260)

SELECT Item.ItemId AS Expr2, d.ItemId AS Expr1, Item.*
FROM   Item INNER JOIN
                 (SELECT TOP (100) PERCENT MIN(ItemId) AS ItemId, ItemNumber, COUNT(ItemNumber) AS Expr2
                 FROM    Item AS Item_1
                 GROUP BY ItemNumber
                 HAVING (COUNT(ItemNumber) > 1)
                 ORDER BY CAST(ItemNumber AS int)) AS d ON Item.ItemNumber = d.ItemNumber
