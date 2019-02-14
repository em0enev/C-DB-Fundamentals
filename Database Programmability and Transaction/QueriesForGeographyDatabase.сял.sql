--Problem 9. Find Full Name
CREATE PROCEDURE usp_GetHoldersFullName 
AS
	SELECT FirstName + ' ' + LastName AS [Full Name]
	FROM AccountHolders
GO

EXEC usp_GetHoldersFullName

--Problem 10. People with Balance Higher Than
CREATE PROCEDURE usp_GetHoldersWithBalanceHigherThan(@Number Decimal(18,2))
AS
	SELECT ah.FirstName as [First Name], ah.LastName as [Last Name]
	FROM AccountHolders as ah
	LEFT JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	GROUP BY a.AccountHolderId, ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @Number
	ORDER BY FirstName, LastName
GO

--Problem 11. Future Value Function
CREATE FUNCTION ufn_CalculateFutureValue(@Sum decimal(18,2), @rate float, @years int)
RETURNS DECIMAL(18,4)
AS
BEGIN 
	DECLARE @Result decimal(18,4)
	SET @Result = @sum *(POWER((1+ @rate),@years))
	RETURN @Result
END

--Problem 12. Calculating Interest
CREATE PROCEDURE usp_CalculateFutureValueForAccount(@AccID INT, @InterestRate float)
AS
	SELECT 
		a.AccountHolderId as [Account Id], 
		ah.FirstName, 
		ah.LastName,
		a.Balance as [Current Balance], 
		ufn_CalculateFutureValue( a.Balance , @InterestRate, 5) as [Balance in 5 years]
	FROM Accounts as a
	LEFT JOIN AccountHolders AS ah ON a.AccountHolderId = ah.Id
	WHERE a.AccountHolderId = @AccID


EXEC dbo.usp_CalculateFutureValueForAccount 2, 0.1
drop procedure usp_CalculateFutureValueForAccount