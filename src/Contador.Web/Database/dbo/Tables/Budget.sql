-- This will be changed in the future steps of develop
CREATE OR REPLACE TABLE Budget (
        Id INTEGER AUTO_INCREMENT,
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        CreateDate DATETIME NOT NULL,
        ModifiedDate DATETIME NOT NULL,
        primary key(Id)
    );