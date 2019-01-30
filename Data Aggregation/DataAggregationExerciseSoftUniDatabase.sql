--Problem 13. Departments Total Salaries
SELECT DepartmentId, SUM(Salary) AS TotalSalary
FROM Employees
GROUP BY DepartmentID
ORDER BY DepartmentID

--Problem 14. Employees Minimum Salaries
SELECT DepartmentId, MIN(Salary) AS MinimumSalary
FROM Employees
WHERE DepartmentID in (2,5,7) and HireDate >'01/01/2000'
GROUP BY DepartmentID
ORDER BY DepartmentID

--Problem 15. Employees Average Salaries
SELECT * INTO NewEmployeeTable
FROM Employees
WHERE Salary > 30000

DELETE FROM NewEmployeeTable
WHERE ManagerID = 42

UPDATE NewEmployeeTable
SET Salary += 5000
WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary) as AverageSalary
FROM NewEmployeeTable
GROUP BY DepartmentID

--Problem 16. Employees Maximum Salaries
SELECT DepartmentID,  MAX(Salary) AS MaxSalary
FROM Employees
GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

--Problem 17. Employees Count Salaries
SELECT COUNT(Salary) as Count
FROM Employees
where ManagerID is null

--Problem 18. *3rd Highest Salary
WITH OrderedSalary AS(
SELECT RN = ROW_NUMBER() OVER (PARTITION BY departmentId  ORDER BY salary DESC )
           ,DepartmentID
		   ,Salary
FROM Employees
GROUP BY salary, DepartmentID)

 SELECT DepartmentID
		,Salary AS ThirdHighestSalary
 FROM OrderedSalary
 WHERE RN = 3


