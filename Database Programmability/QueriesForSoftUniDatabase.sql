--Problem 1. Employees with Salary Above 35000
CREATE PROC usp_GetEmployeesSalaryAbove35000 
AS
	SELECT FirstName, LastName
	FROM Employees
	WHERE Salary > 35000
GO

exec usp_GetEmployeesSalaryAbove35000

--Problem 2. Employees with Salary Above Number
CREATE PROC usp_GetEmployeesSalaryAboveNumber(@Number DECIMAL(18,4))
AS 
	SELECT e.FirstName, e.LastName
	FROM Employees AS e 
	WHERE Salary >= @Number
GO

exec usp_GetEmployeesSalaryAboveNumber 48000

--Problem 3. Town Names Starting With
CREATE PROCEDURE usp_GetTownsStartingWith(@string VARCHAR(10))
AS
	SELECT t.[Name] AS Town
	FROM Towns AS t
	WHERE LEFT(t.[Name],LEN(@string)) = @string
GO

exec usp_GetTownsStartingWith 'ber'

--Problem 4. Employees from Town
CREATE PROCEDURE usp_GetEmployeesFromTown(@Name VARCHAR(10))
AS
	SELECT e.FirstName, e.LastName
	FROM Employees AS e
	LEFT JOIN Addresses AS a ON a.AddressID = e.AddressID
	LEFT JOIN Towns AS t ON t.TownID = A.TownID
	WHERE t.[Name] = @Name
GO

Execute usp_GetEmployeesFromTown Sofia

--Problem 5. Salary Level Function
CREATE FUNCTION udf_GetSalaryLevel(@Salary MONEY)
RETURNS varchar(10)
AS
BEGIN
	DECLARE @result varchar(10)

	IF(@Salary < 30000)
		SET @result = 'Low'
	ELSE IF(@Salary BETWEEN 30000 AND 50000)
		SET @result = 'Average'
	ELSE
		SET @result = 'High'

	RETURN @result
END

SELECT FirstName, LastName, Salary, dbo.udf_GetSalaryLevel(Salary) AS SalaryLevel
FROM Employees

--Problem 6. Employees by Salary Level
CREATE PROC usp_EmployeesBySalaryLevel(@Level VARCHAR(10))
AS	
	SELECT FirstName, LastName
	FROM Employees
	WHERE dbo.udf_GetSalaryLevel(Salary) = @Level
GO

EXEC usp_EmployeesBySalaryLevel 'High'

--Problem 7. Define Function
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters varchar(MAX), @word VARCHAR(MAX))
RETURNS BIT
AS 
BEGIN 
	DECLARE @Count INT = 1 
	WHILE(@Count <= LEN(@word))
	BEGIN
		DECLARE @currentLetter CHAR(1) = SUBSTRING(@word, @count, 1)
		DECLARE @charIndex INT = CHARINDEX(@currentLetter, @setOfLetters)

		IF(@charIndex = 0)
		BEGIN
			RETURN 0 
		END

		SET @Count +=1
	END
	RETURN 1
END
