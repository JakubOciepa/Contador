DELIMITER $$
CREATE OR REPLACE PROCEDURE categoryBudget_Add (
		IN budgetId_p INTEGER,
		IN categoryId_p INTEGER,
		IN value_p DECIMAL(10,2)
	) BEGIN
INSERT INTO CategoryBudget (
		BudgetId,
		CategoryId,
		Value,
		CreateDate,
		ModifiedDate
	)
VALUES (
		budgetId_p,
		categoryId_p,
		value_p,
		CURRENT_TIME(),
		CURRENT_TIME()
	);
SELECT *
FROM CategoryBudget bg
WHERE bg.Id = (
		SELECT MAX(Id)
		From CategoryBudget
	);
END;