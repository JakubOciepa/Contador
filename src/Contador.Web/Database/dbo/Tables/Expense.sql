-- This will be changed in the future steps of develop
CREATE OR REPLACE TABLE Expense (
        Id INTEGER AUTO_INCREMENT,
        Name VARCHAR(255) NOT NULL,
        CategoryId INTEGER NOT NULL,
        UserId VARCHAR(255) NOT NULL,
        Value DECIMAL(10,2) NOT NULL,
        Description VARCHAR(255),
        CreateDate DATETIME NOT NULL,
        ModifiedDate DATETIME NOT NULL,
        ImagePath VARCHAR(4096),
        primary key(Id)
    );