CREATE OR REPLACE TABLE Expenses (
    Id INTEGER AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    CategoryId INTEGER NOT NULL,
    PersonId INTEGER NOT NULL,
    Value DECIMAL NOT NULL,
    Description VARCHAR(255),
    CreateDate DATETIME NOT NULL,
    ModifiedDate DATETIME NOT NULL,
    ImagePath VARCHAR(4096),
    primary key(Id)
);