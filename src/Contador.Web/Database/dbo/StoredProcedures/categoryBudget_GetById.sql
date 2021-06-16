DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_GetById (
		IN id_p INTEGER
	) BEGIN
SELECT *
FROM CategoryBudget bg
WHERE bg.Id = id_p;
END;