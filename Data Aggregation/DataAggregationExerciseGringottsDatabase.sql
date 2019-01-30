--Problem 1. Records’ Count
SELECT COUNT(Id) AS Count
FROM WizzardDeposits

--Problem 2. Longest Magic Wand
SELECT MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits

--Problem 3. Longest Magic Wand per Deposit Groups
SELECT DepositGroup, MAX(MagicWandSize) AS LongestMagicWand
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY DepositGroup

--Problem 4. * Smallest Deposit Group per Magic Wand Size
SELECT TOP 2 DepositGroup
FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)

--Problem 5. Deposits Sum
SELECT DepositGroup, SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
GROUP BY DepositGroup

--Problem 6. Deposits Sum for Ollivander Family
SELECT DepositGroup, SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup

--Problem 7. Deposits Filter
SELECT DepositGroup, SUM(DepositAmount) AS TotalSum
FROM WizzardDeposits
WHERE MagicWandCreator = 'Ollivander family'
GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
ORDER BY SUM(DepositAmount) DESC

--Problem 8. Deposit Charge
SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS MinDepositCharge
FROM WizzardDeposits 
GROUP BY MagicWandCreator, DepositGroup
ORDER BY MagicWandCreator, DepositGroup

--Problem 9. Age Groups
SELECT  [AgeGroup] =
CASE 
    WHEN Age >= 0 AND age < 11 THEN '[0-10]'
    WHEN age >= 11 AND age < 21 THEN '[11-20]'
    WHEN age >= 21 AND age < 31 THEN '[21-30]'
    WHEN age >= 31 AND age < 41 THEN '[31-40]'
    WHEN age >= 41 AND age < 51 THEN '[41-50]'
    WHEN age >= 51 AND age < 61 THEN '[51-60]'
    WHEN age >= 61 THEN '[61+]'
END 
	, COUNT(age) AS WizardCount 
FROM WizzardDeposits
GROUP BY
CASE 
	WHEN Age >= 0 AND age < 11 THEN '[0-10]'
	WHEN age >= 11 AND age < 21 THEN '[11-20]'
	WHEN age >= 21 AND age < 31 THEN '[21-30]'
	WHEN age >= 31 AND age < 41 THEN '[31-40]'
	WHEN age >= 41 AND age < 51 THEN '[41-50]'
	WHEN age >= 51 AND age < 61 THEN '[51-60]'
	WHEN age >= 61 THEN '[61+]'
END 

--Problem 10. First Letter 
SELECT DISTINCT SUBSTRING(FirstName,1,1) AS FirstLetter
FROM WizzardDeposits
WHERE DepositGroup = 'Troll Chest'
ORDER BY FirstLetter

--Problem 11. Average Interest 
SELECT DepositGroup, IsDepositExpired, AVG(DepositInterest) AS AverageInterest
FROM WizzardDeposits
WHERE DepositStartDate > '01/01/1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired ASC

--Problem 12.* Rich Wizard, Poor Wizard
SELECT SUM(k.SumDiff) AS SumDifference
	FROM (
  SELECT DepositAmount - LEAD(DepositAmount,1) OVER(ORDER BY Id) AS SumDiff
	from WizzardDeposits ) AS k

select * from WizzardDeposits
