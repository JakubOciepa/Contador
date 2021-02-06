CREATE OR REPLACE PROCEDURE expenseCategory_Update (
        IN id_p INTEGER,
        IN name_p VARCHAR(255)
    ) BEGIN
UPDATE Expense
SET Name = name_p
WHERE Id = id_p;
SELECT *
FROM ExpenseCategory
WHERE Id = id_p;
END;