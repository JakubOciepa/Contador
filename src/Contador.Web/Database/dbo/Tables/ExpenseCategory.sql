-- This will be changed in the future steps of develop
CREATE OR REPLACE TABLE ExpenseCategory (
        Id INTEGER AUTO_INCREMENT,
        Name VARCHAR(255) NOT NULL,
        CreateDate DATETIME NOT NULL,
        ModifiedDate DATETIME NOT NULL,
        primary key(Id)
    );