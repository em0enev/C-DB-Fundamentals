--Problem 14. Games from 2011 and 2012 year
SELECT TOP 50 [Name], FORMAT( [Start], 'yyyy-MM-dd' , 'bg-BG'  ) as [Start]
FROM Games
WHERE Start BETWEEN '2011/01/01' AND '2012/12/31'
ORDER BY [Start], [Name] ASC  

--Problem 15. User Email Providers
SELECT Username, SUBSTRING( Email, CHARINDEX('@', Email )+ 1, LEN(Email) ) as [Email Provider]
FROM Users
ORDER BY [Email Provider], Username ASC

--Problem 16. Get Users with IPAdress Like Pattern
SELECT Username, IpAddress
FROM Users
WHERE IpAddress LIKE '___.1_%__.%___%'
ORDER BY Username ASC  

--Problem 17. Show All Games with Duration and Part of the Day
SELECT [Name] as Game, [Part of the Day] = 
	CASE 
		WHEN FORMAT(Start,'HH\:mm\:ss', 'bg-BG') BETWEEN '00:00:00' and '11:59:59' THEN 'Morning'
		WHEN FORMAT(Start,'HH\:mm\:ss', 'bg-BG') BETWEEN '12:00:00' and '17:59:59' THEN 'Afternoon'
		WHEN FORMAT(start,'HH\:mm\:ss', 'bg-BG') BETWEEN '18:00:00' and '23:59:59' THEN 'Evening'
	END,
	[Duration] =
	CASE
		WHEN Duration BETWEEN 0 AND 3 THEN 'Extra Short'
		WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
		WHEN Duration > 6 THEN 'Long'
		ELSE 'Extra Long'
	END
FROM Games
ORDER BY Game, [Duration] , [Part of the Day] ASC

Select name, Start,Duration
from Games
ORDER BY Name
