DELIMITER $$
CREATE OR REPLACE PROCEDURE issue_Add (IN name_p VARCHAR(255)) BEGIN
INSERT INTO Issue (Name, CreateDate)
VALUES (
        name_p,
        CURRENT_TIME()
    );
SELECT *
FROM Issue
WHERE Id = (
        SELECT MAX(Id)
        FROM Issue
    );
END;