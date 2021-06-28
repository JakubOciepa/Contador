DELIMITER $$
CREATE OR REPLACE PROCEDURE budget_GetByStartDate (startDate_p DATETIME) BEGIN
SELECT *
FROM Budget bg
WHERE DATE(bg.StartDate) = DATE(startDate_p);
END;