DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_Delete (IN id_p INTEGER) BEGIN
DELETE FROM CategoryBudget
WHERE Id = id_p;
END;