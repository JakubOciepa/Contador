-- This will be changed in the future steps of develop
CREATE OR REPLACE TABLE CategoryBudget (
        Id INTEGER AUTO_INCREMENT,
        BudgetId INTEGER NOT NULL,
        CategoryId INTEGER NOT NULL,
        Value DECIMAL(10,2) NOT NULL,
        CreateDate DATETIME NOT NULL,
        ModifiedDate DATETIME NOT NULL,
        primary key(Id)
    );