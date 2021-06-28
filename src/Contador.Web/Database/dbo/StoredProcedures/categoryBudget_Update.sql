DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_Update (
		IN id_p INTEGER,
		IN value_p DECIMAL(10,2)
	) BEGIN
UPDATE CategoryBudget
SET Value = value_p,
	ModifiedDate = CURRENT_TIME()
WHERE Id = id_p;
SELECT *
FROM CategoryBudget
WHERE Id = id_p;
END;