DELIMITER $$
CREATE OR REPLACE PROCEDURE expense_Add (
        IN name_p VARCHAR(255),
        IN value_p DECIMAL(10,2),
        IN description_p VARCHAR(255),
        IN categoryId_p INTEGER,
        IN userId_p INTEGER,
        IN image_path_p VARCHAR(4096)
    ) BEGIN
INSERT INTO Expense (
        Name,
        CategoryId,
        UserId,
        Value,
        CreateDate,
        ModifiedDate,
        Description,
        ImagePath
    )
VALUES (
        name_p,
        categoryId_p,
        userId_p,
        value_p,
        CURRENT_TIME(),
        CURRENT_TIME(),
        description_p,
        image_path_p
    );
SELECT *
FROM Expense ex
    LEFT JOIN ExpenseCategory ec ON ec.Id = ex.CategoryId
    LEFT JOIN User us ON us.Id = ex.UserId
WHERE ex.Id = (
        SELECT MAX(Id)
        From Expense
    );
END;