INSERT INTO Expense (Name, CategoryId, PersonId, Value, CreateDate, ModifiedDate)
VALUES ("Słodycze", 0, 0, 123.00, CURRENT_DATE(), CURRENT_DATE());

INSERT INTO ExpenseCategory (Name) 
VALUES ("Slodycze");

INSERT INTO User (Name, Email)
VALUES ("Marysia", "maria@test.com");