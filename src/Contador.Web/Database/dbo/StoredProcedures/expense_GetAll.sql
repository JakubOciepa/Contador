DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetAll () BEGIN
SELECT *
FROM Expense ex
    LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
    LEFT JOIN User us ON us.Id = ex.UserId;
END;