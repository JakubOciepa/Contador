CREATE OR REPLACE PROCEDURE expenseCategory_Delete (id_p INTEGER) BEGIN
DELETE FROM ExpenseCategory
WHERE Id = id_p;
END;