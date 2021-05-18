DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_GetFiltered (
		IN name_p VARCHAR(255),
		IN categoryName_p VARCHAR(255),
		IN userName_p VARCHAR(255)
	) BEGIN
declare namePattern varchar(255);
declare categoryPattern varchar(255);
declare userNamePattern varchar(255);
set namePattern = CONCAT('%', name_p, '%');
set categoryPattern = CONCAT('%', categoryName_p, '%');
set userNamePattern = CONCAT('%', userName_p, '%');
SELECT *
FROM Expense ex
	LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
	LEFT JOIN ContadorUsers.AspNetUsers us ON us.Id = ex.UserId
WHERE ec.Name LIKE categoryPattern
	AND us.UserName LIKE userNamePattern
	AND ex.Name LIKE namePattern;
END;