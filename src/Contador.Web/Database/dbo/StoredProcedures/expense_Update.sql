DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_Update (
        IN id_p INTEGER,
        IN name_p VARCHAR(255),
        IN value_p DECIMAL,
        IN description_p VARCHAR(255),
        IN categoryId_p INTEGER,
        IN userId_p INTEGER,
        IN image_path_p VARCHAR(4096)
    ) BEGIN
UPDATE Expense
SET Name = name_p,
    CategoryId = categoryId_p,
    UserId = userId_p,
    Value = value_p,
    ModifiedDate = CURRENT_TIME(),
    Description = description_p,
    ImagePath = image_path_p
WHERE Id = id_p;
SELECT *
FROM Expense ex
    LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
    LEFT JOIN User us ON us.Id = ex.UserId
WHERE ex.Id = id_p;
END;