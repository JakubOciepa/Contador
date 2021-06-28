DELIMITER $$
CREATE OR REPLACE PROCEDURE budget_Update (
		IN id_p INTEGER,
		IN startDate_p DATETIME,
		IN endDate_p DATETIME
	) BEGIN
UPDATE Budget
SET StartDate = startDate_p,
	EndDate = endDate_p,
	ModifiedDate = CURRENT_TIME()
WHERE Id = id_p;
SELECT *
FROM Budget
WHERE Id = id_p;
END;