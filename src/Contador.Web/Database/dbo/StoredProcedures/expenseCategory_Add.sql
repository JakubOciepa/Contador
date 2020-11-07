CREATE OR REPLACE PROCEDURE expenseCategory_Add (IN name_p VARCHAR(255)) BEGIN
INSERT INTO ExpenseCategory (Name)
VALUES (
        name_p,
        CURRENT_DATE(),
        CURRENT_DATE()
    );
END;