DELIMITER $$
CREATE OR REPLACE PROCEDURE expenseCategory_Add (IN name_p VARCHAR(255)) BEGIN
INSERT INTO ExpenseCategory (Name, CreateDate, ModifiedDate)
VALUES (
        name_p,
        CURRENT_DATE(),
        CURRENT_DATE()
    );
SELECT *
FROM ExpenseCategory
WHERE Id = (
        SELECT MAX(Id)
        FROM ExpenseCategory
    );
END;