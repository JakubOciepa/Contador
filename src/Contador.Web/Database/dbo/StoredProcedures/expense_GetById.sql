CREATE OR REPLACE PROCEDURE expense_GetById
    (expenseId INTEGER)
    BEGIN
        SELECT * FROM Expense
        WHERE Id = expenseId;
    END;