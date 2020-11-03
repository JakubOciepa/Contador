-- This will be changed in the future steps of develop

CREATE OR REPLACE TABLE User (
    Id INTEGER AUTO_INCREMENT,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    primary key(Id)
);
