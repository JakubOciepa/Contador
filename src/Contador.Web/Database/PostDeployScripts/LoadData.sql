INSERT INTO Expense (Name, CategoryId, UserId, Value, CreateDate, ModifiedDate)
VALUES ("Słodycze", 1, 1, 123.00, CURRENT_DATE(), CURRENT_DATE());

INSERT INTO ExpenseCategory (Name, CreateDate, ModifiedDate) 
VALUES ("Slodycze", CURRENT_DATE(), CURRENT_DATE());

INSERT INTO User (Name, Email, CreateDate, ModifiedDate)
VALUES ("Marysia", "maria@test.com", CURRENT_DATE(), CURRENT_DATE());