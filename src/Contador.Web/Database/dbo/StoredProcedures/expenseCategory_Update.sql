DELIMITER $$
CREATE OR REPLACE PROCEDURE expenseCategory_Update (
        IN id_p INTEGER,
        IN name_p VARCHAR(255)
    ) BEGIN
UPDATE ExpenseCategory
SET Name = name_p,
    ModifiedDate = CURRENT_TIME()
    WHERE Id = id_p;
SELECT *
FROM ExpenseCategory
WHERE Id = id_p;
END;