--Problem 1.	Employee Address
SELECT TOP 5 e.EmployeeID, e.JobTitle, e.AddressID, a.AddressText
FROM Employees AS e
LEFT JOIN Addresses AS a ON e.AddressID = a.AddressID
ORDER BY AddressID ASC

--Problem 2.	Addresses with Towns
SELECT TOP 50 e.FirstName, e.LastName, t.[Name], a.AddressText
FROM Employees AS e
LEFT JOIN Addresses AS a ON e.AddressID = a.AddressID
LEFT JOIN Towns AS t ON a.TownID = t.TownID
ORDER BY e.FirstName, e.LastName ASC

--Problem 3.	Sales Employee
SELECT e.EmployeeID, e.FirstName, e.LastName, d.[Name]
FROM Employees AS e
LEFT JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE d.Name = 'Sales'
ORDER BY EmployeeID ASC 

--Problem 4.	Employee Departments
SELECT TOP 5 e.EmployeeID, e.FirstName
FROM Employees AS e
LEFT JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.Salary > 15000
ORDER BY  e.DepartmentID ASC 

--Problem 5.	Employees Without Project
SELECT TOP 3 e.EmployeeID, e.FirstName
FROM Employees AS e
FULL OUTER JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
WHERE ProjectID IS NULL
ORDER BY EmployeeID ASC

--Problem 6.	Employees Hired After
SELECT e.FirstName, e.LastName, e.HireDate, d.[Name]
FROM Employees AS e
JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
WHERE e.HireDate > '1999-01-01' AND d.[Name] IN ('Sales', 'Finance' )
ORDER BY HireDate ASC

--Problem 7.	Employees with Project
SELECT TOP 5 e.EmployeeID ,e.FirstName, p.[Name]
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE p.StartDate > '2002-08-13' AND p.EndDate IS NULL
ORDER BY EmployeeID ASC

--Problem 8.	Employee 24
SELECT e.EmployeeID 
		,e.FirstName
		,(Case
			WHEN p.StartDate <= '2005-01-01' then p.Name 
			else null
		  end) AS ProjectName
FROM Employees AS e
JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID
JOIN Projects AS p ON ep.ProjectID = p.ProjectID
WHERE e.EmployeeID = 24 

--Problem 9.	Employee Manager
SELECT e.EmployeeID, e.FirstName, e.ManagerID, emp.FirstName
FROM Employees AS e
LEFT JOIN Employees as emp ON e.ManagerID = emp.EmployeeID
WHERE e.ManagerID IN (3,7)
ORDER BY EmployeeID ASC

--Problem 10.	Employee Summary
SELECT TOP 50 e.EmployeeID
			 ,e.FirstName + ' '+ e.LastName AS EmployeeName
			 ,emp.FirstName + ' ' + emp.LastName AS ManagerName
			 ,d.[Name]
FROM Employees AS e
LEFT JOIN Employees as emp ON e.ManagerID = emp.EmployeeID
LEFT JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
ORDER BY EmployeeID ASC

--Problem 11.	Min Average Salary
SELECT MIN(a.AverageSalary) as MinAverageSalary
FROM (	SELECT e.DepartmentID,
		AVG(e.Salary) AS AverageSalary
		FROM Employees AS e
		GROUP BY e.DepartmentID ) AS a