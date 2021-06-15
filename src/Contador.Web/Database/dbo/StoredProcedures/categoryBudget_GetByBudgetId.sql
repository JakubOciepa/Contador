DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_GetByBudgetId (id_p INTEGER) BEGIN
SELECT *
FROM CategoryBudget bg
    LEFT JOIN ExpenseCategory ec ON ec.Id = bg.CategoryId
WHERE bg.BudgetId = id_p;
END;