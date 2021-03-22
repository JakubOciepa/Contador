DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetByMonth (month_p INTEGER, year_p INTEGER) BEGIN
SELECT *
FROM Expense ex
    LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
    LEFT JOIN User us ON us.Id = ex.UserId
WHERE DATE_FORMAT(ex.CreateDate, '%m') = month_p
    AND DATE_FORMAT(ex.CreateDate, '%Y') = year_p;
END;