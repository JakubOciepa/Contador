DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetByYear (year_p INTEGER) BEGIN
SELECT *
FROM Expense ex
	LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
	LEFT JOIN ContadorUsers.AspNetUsers us ON us.Id = ex.UserId
WHERE DATE_FORMAT(ex.CreateDate, '%Y') = year_p;
END;