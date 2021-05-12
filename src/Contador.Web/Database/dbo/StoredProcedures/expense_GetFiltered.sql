DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetFiltered (
		IN name_p VARCHAR(255),
		IN categoryName_p VARCHAR(255),
		IN userName_p VARCHAR(255)
	) BEGIN
declare pattern varchar(255);
set pattern = CONCAT('%', name_p, '%');
SELECT *
FROM Expense ex
	LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
	LEFT JOIN ContadorUsers.AspNetUsers us ON us.Id = ex.UserId
WHERE ec.Name = categoryName_p
	AND us.UserName = userName_p
	AND ex.Name LIKE pattern;
END;