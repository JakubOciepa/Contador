DELIMITER $$
CREATE OR REPLACE PROCEDURE budget_GetByStartDate (startDate_p DATETIME) BEGIN
SELECT *
FROM Budget bg
	LEFT JOIN CategoryBudget cb ON bg.Id = cb.BudgetId
WHERE DATE(bg.StartDate) = DATE(startDate_p);
END;