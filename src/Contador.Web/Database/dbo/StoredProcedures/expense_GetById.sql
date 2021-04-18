DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetById (id_p INTEGER) BEGIN
SELECT *
FROM Expense ex
	LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
	LEFT JOIN ContadorUsers.AspNetUsers us ON us.Id = ex.UserId
WHERE ex.Id = id_p;
END;