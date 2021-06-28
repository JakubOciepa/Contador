DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_GetByCategoryAndBudgetId (
		IN budgetId_p INTEGER,
		IN categoryId_p INTEGER
	) BEGIN
SELECT *
FROM CategoryBudget bg
WHERE bg.BudgetId = budgetId_p AND bg.CategoryId = categoryId_p;
END;