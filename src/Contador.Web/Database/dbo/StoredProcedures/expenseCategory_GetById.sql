CREATE OR REPLACE PROCEDURE expenseCategory_GetById (id_p INTEGER) BEGIN
SELECT *
FROM ExpenseCategory
WHERE Id = id_p;
END;