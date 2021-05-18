DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_Update (
		IN id_p INTEGER,
		IN name_p VARCHAR(255),
		IN value_p DECIMAL(10, 2),
		IN description_p VARCHAR(255),
		IN categoryId_p INTEGER,
		IN userId_p VARCHAR(255),
		IN imagePath_p VARCHAR(4096),
		IN createDate_p DATETIME
	) BEGIN
UPDATE Expense
SET Name = name_p,
	CategoryId = categoryId_p,
	UserId = userId_p,
	Value = value_p,
	CreateDate = createDate_p,
	ModifiedDate = CURRENT_TIME(),
	Description = description_p,
	ImagePath = imagePath_p
WHERE Id = id_p;
SELECT *
FROM Expense ex
	LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
	LEFT JOIN ContadorUsers.AspNetUsers us ON us.Id = ex.UserId
WHERE ex.Id = id_p;
END;