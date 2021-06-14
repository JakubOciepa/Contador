DELIMITER $$
CREATE OR REPLACE PROCEDURE budget_GetById (id_p INTEGER) BEGIN
SELECT *
FROM Budget bg
	LEFT JOIN CategoryBudget cb ON bg.Id = cb.BudgetId
WHERE bg.Id = id_p;
END;