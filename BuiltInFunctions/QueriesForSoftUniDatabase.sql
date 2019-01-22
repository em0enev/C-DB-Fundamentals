--Problem 1. Find Names of All Employees by First Name Starts With 'SA'
SELECT FirstName, LastName
FROM Employees
WHERE FirstName LIKE 'SA%'

--Problem 2. Find Names of All employees by Last Name 
SELECT FirstName, LastName
FROM Employees
WHERE LastName LIKE '%ei%'

--Problem 3. Find First Names of All Employees
SELECT FirstName, DepartmentID
FROM Employees
WHERE HireDate BETWEEN '1995-1-1' and '2005-12-31'
AND DepartmentID IN (3,10)

--Problem 4. Find All Employees Except Engineers
SELECT FirstName, LastName, JobTitle
FROM Employees
WHERE JobTitle NOT LIKE '%engineer%'

--Problem 5. Find Towns with Name Length
SELECT [Name]
FROM Towns
WHERE LEN([Name]) BETWEEN 5 and 6
ORDER BY Name ASC

--Problem 6. Find Towns Starting With
SELECT * FROM Towns
WHERE Name LIKE 'M%' 
	or Name LIKE 'K%'
	or Name LIKE 'B%'
	or Name LIKE 'E%'
ORDER BY [Name] ASC 

-- Problem 7.Find Towns Not Starting With
SELECT * FROM Towns
WHERE
	[Name] NOT LIKE 'B%'
	AND NAME NOT LIKE 'D%'
	AND NAME NOT LIKE 'R%'
ORDER BY [Name] ASC 

--Problem 8. Create View Employees Hired After 2000 Year
CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName, LastName
FROM Employees
WHERE HireDate > '2001'

--Problem 9. Length of Last Name
SELECT FirstName, LastName
FROM Employees
WHERE LEN(LastName) = 5

--Problem 10. Rank Employees by Salary
SELECT EmployeeID, FirstName, LastName,Salary,  
       DENSE_RANK() OVER (PARTITION BY Salary ORDER BY EmployeeID) AS Rank
FROM Employees
WHERE Salary BETWEEN 10000 AND 50000
ORDER BY Salary DESC