--Problem 12.	Highest Peaks in Bulgaria
SELECT c.CountryCode,m.MountainRange, p.PeakName, p.Elevation
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Peaks AS p ON p.MountainId = mc.MountainId
LEFT JOIN Mountains as m ON m.Id = mc.MountainId
WHERE c.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY Elevation DESC

--Problem 13.	Count Mountain Ranges
SELECT mc.CountryCode, COUNT(mc.CountryCode) AS MountainRanges
FROM MountainsCountries AS mc
WHERE CountryCode IN ('BG', 'US', 'RU')
GROUP BY mc.CountryCode

--Problem 14.	Countries with Rivers
SELECT TOP 5 c.CountryName, r.RiverName
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON c.CountryCode =cr.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY c.CountryName ASC

--Problem 15.	*Continents and Currencies
SELECT MostUsedCurrency.ContinentCode, MostUsedCurrency.CurrencyCode, MostUsedCurrency.CurrencyUsage
FROM (
		SELECT c.ContinentCode, c.CurrencyCode,
		COUNT(CurrencyCode) as CurrencyUsage,
		DENSE_RANK() OVER (PARTITION BY c.ContinentCode ORDER BY COUNT(CurrencyCode) DESC ) AS CurrencyRank
		FROM Countries AS c
		GROUP BY c.ContinentCode, c.CurrencyCode
		HAVING COUNT(CurrencyCode) > 1
	 ) AS MostUsedCurrency
WHERE MostUsedCurrency.CurrencyRank = 1
ORDER BY MostUsedCurrency.ContinentCode, MostUsedCurrency.CurrencyUsage 

--Problem 16.	Countries without any Mountains
SELECT COUNT(combined.CountryCode) as CountryCode
FROM ( SELECT m.MountainRange, c.CountryCode FROM MountainsCountries AS mc
	FULL JOIN Mountains AS m ON m.Id = mc.MountainId
	FULL JOIN Countries AS c  ON mc.CountryCode = c.CountryCode
	WHERE m.MountainRange IS NULL) as combined


--Problem 17.	Highest Peak and Longest River by Country
WITH cte_table(CountryName, Elevation , [Length],  [Rank]) AS
(
select c.CountryName, p.Elevation, r.[Length], DENSE_RANK() OVER   
    (PARTITION BY c.CountryName ORDER BY p.Elevation DESC, r.[Length] DESC) as [Rank]
from Rivers as r 
left join CountriesRivers as cr on cr.RiverId = r.Id
left join Countries as c on c.CountryCode = cr.CountryCode
left join MountainsCountries as mc on mc.CountryCode = c.CountryCode
left join Mountains as m on m.Id = mc.MountainId
left join Peaks as p on p.MountainId = mc.MountainId
)
SELECT TOP 5 ct.CountryName, ct.Elevation, ct.Length
FROM cte_table as ct
WHERE [Rank] = 1
ORDER BY Elevation DESC, [Length] DESC, CountryName ASC

--Problem 18.	* Highest Peak Name and Elevation by Country
WITH cte_table(CountryName, PeakName, Elevation, MountainRange, [Rank]) AS
(
select  c.CountryName, p.PeakName, p.Elevation, m.MountainRange , DENSE_RANK() OVER   
    (PARTITION BY c.CountryName ORDER BY p.Elevation DESC) as [Rank]
FROM Countries as c
LEFT JOIN MountainsCountries as mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Peaks as p ON p.MountainId = mc.MountainId
left join Mountains as m ON m.Id = mc.MountainId
)

SELECT TOP 5 ct.CountryName
	, CASE
		WHEN ct.PeakName IS NULL THEN '(no highest peak)' 
		else ct.PeakName
	END as [Highest Peak Name]
	, CASE
		WHEN ct.Elevation IS NULL THEN 0 
		ELSE ct.Elevation
	END as [Highest Peak Elevation]
	, CASE
		WHEN ct.MountainRange IS NULL THEN '(no mountain)'
		ELSE ct.MountainRange
	END as [Mountain]
FROM cte_table as ct
WHERE [Rank] = 1
ORDER BY CountryName ASC, PeakName ASC
