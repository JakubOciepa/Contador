CREATE OR REPLACE PROCEDURE expense_Add (
        IN name_p VARCHAR(255),
        IN value_p DECIMAL,
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
        CURRENT_DATE(),
        CURRENT_DATE(),
        description_p,
        image_path_p
    );
END;