DELIMITER $$
CREATE OR REPLACE PROCEDURE budget_Add (
		IN startDate_p DATETIME,
		IN endDate_p DATETIME
	) BEGIN
INSERT INTO Budget (
		StartDate,
		EndDate,
		CreateDate,
		ModifiedDate
	)
VALUES (
		startDate_p,
		endDate_p,
		CURRENT_TIME(),
		CURRENT_TIME()
	);
SELECT *
FROM Budget bg
WHERE bg.Id = (
		SELECT MAX(Id)
		From Budget
	);
END;