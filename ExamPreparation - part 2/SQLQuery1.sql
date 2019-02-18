--1. Database design
CREATE TABLE Cities
(
	Id INT PRIMARY KEY IDENTITY, 
	[Name] nvarchar(20) NOT NULL,
	CountryCode varchar(2) NOT NULL
)

CREATE TABLE Hotels
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] nvarchar(30) NOT NULL,
	CityId INT NOT NULL REFERENCES Cities(Id),
	EmployeeCount INT NOT NULL,
	BaseRate DECIMAL(15,2)
)

CREATE TABLE Rooms
(
	Id INT PRIMARY KEY IDENTITY,
	Price DECIMAL(15,2) NOT NULL,
	[Type] nvarchar(20) NOT NULL,
	Beds INT NOT NULL,
	HotelId INT NOT NULL REFERENCES Hotels(Id)
)

CREATE TABLE Trips
(
	Id INT PRIMARY KEY IDENTITY,
	RoomId INT NOT NULL REFERENCES Rooms(Id),
	BookDate DATE NOT NULL,
	ArrivalDate DATE NOT NULL,
	ReturnDate DATE NOT NULL,
	CancelDate DATE,
	CHECK(BookDate < ArrivalDate),
	CHECK(ArrivalDate < ReturnDate)
)

CREATE TABLE Accounts
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(20),
	LastName NVARCHAR(50) NOT NULL,
	CityId INT NOT NULL REFERENCES Cities(id),
	BirthDate DATE NOT NULL,
	Email varchar(100) UNIQUE NOT NULL
)

CREATE TABLE AccountsTrips
(
	AccountId INT NOT NULL REFERENCES Accounts(Id),
	TripId INT NOT NULL REFERENCES Trips(Id),
	Luggage INT NOT NULL
	CHECK(Luggage >= 0),
	PRIMARY KEY(AccountId,TripId)
)

--2. Insert
INSERT INTO Accounts VALUES
('John',	'Smith',	'Smith',	34,	'1975-07-21',	'j_smith@gmail.com'),
('Gosho',	NULL,	'Petrov',	11,	'1978-05-16'	,'g_petrov@gmail.com'),
('Ivan',	'Petrovich',	'Pavlov',	59,	'1849-09-26'	,'i_pavlov@softuni.bg'),
('Friedrich',	'Wilhelm'	,'Nietzsche',	2,	'1844-10-15'	,'f_nietzsche@softuni.bg')

INSERT INTO Trips VALUES
('101',	'2015-04-12'	,'2015-04-14'	,'2015-04-20'	,'2015-02-02'),
('102'	,'2015-07-07'	,'2015-07-15',	'2015-07-22'	,'2015-04-29'),
('103'	,'2013-07-17'	,'2013-07-23',	'2013-07-24',	NULL	   ),
('104'	,'2012-03-17'	,'2012-03-31',	'2012-04-01'	,'2012-01-10'),
('109'	,'2017-08-07'	,'2017-08-28'	,'2017-08-29'	,NULL	   )

--3. Update
UPDATE Rooms
SET Price *= 1.14
WHERE HotelId IN (5,7,9)

--4. Delete
DELETE FROM AccountsTrips
WHERE AccountId = 47

--5. Bulgarian Cities
SELECT c.Id, c.Name
FROM Cities as c
WHERE c.CountryCode = 'BG'
ORDER BY c.Name

--6. People Born After 1991
SELECT a.FirstName + ' ' + CASE
							 WHEN a.MiddleName IS NULL THEN ''
							 else  a.MiddleName + ' '
							END 
		+ a.LastName as [Full Name],
		YEAR(a.BirthDate) AS BirthYear
FROM Accounts as a
WHERE YEAR(a.BirthDate) > 1991
ORDER BY BirthYear DESC, a.FirstName ASC

--7. EEE-Mails
SELECT a.FirstName, a.LastName, CONVERT(varchar ,a.BirthDate, 110), c.Name, a.Email
FROM Accounts as a
JOIN Cities as c ON a.CityId = c.Id
WHERE LEFT(Email,1) = 'e'
ORDER BY c.Name DESC

--8. City Statistics
SELECT c.[Name], COUNT(h.Id) as Hotels
FROM Cities as c
LEFT JOIN Hotels as h ON H.CityId = c.Id
GROUP BY c.[Name]
ORDER BY Hotels DESC, c.[Name]

--9. Expensive First-Class Rooms
SELECT r.Id, r.Price, h.Name,c.Name
FROM Rooms as r
JOIN Hotels as h ON r.HotelId = h.Id
join Cities as c on c.Id = h.CityId
WHERE Type = 'First Class'
ORDER BY r.Price DESC, r.Id 

--10. Longest and Shortest Trips
SELECT at.AccountId
	, CONCAT(a.FirstName,' ',a.LastName) as FullName
	, MAX(DATEDIFF(DAY,t.ArrivalDate, t.ReturnDate)) as LongestTrip
	, MIN(DATEDIFF(DAY,t.ArrivalDate, t.ReturnDate)) as ShortestTrip
FROM AccountsTrips as [at]
JOIN Trips as t ON at.TripId = t.Id
JOIN Accounts as a ON at.AccountId = a.Id
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY a.FirstName, a.LastName, a.Id, at.AccountId
ORDER BY LongestTrip DESC, at.AccountId

--11. Metropolis
SELECT TOP 5 c.id, c.Name, c.CountryCode, count(c.Id) as Accounts
FROM Cities as c
JOIN Accounts as a ON a.CityId = c.Id 
GROUP BY c.Name, c.CountryCode, c.Id
ORDER BY Accounts DESC

--12. Romantic Getaways
SELECT a.Id, a.Email, c.Name, count(a.Id) as Trips
FROM Trips as t 
join Rooms as r on t.RoomId = r.Id
join Hotels as h on h.Id = r.HotelId
join AccountsTrips as at on at.TripId = t.Id
join Accounts as a on a.Id = at.AccountId
join Cities as c on c.Id = a.CityId
where a.CityId = h.CityId
GROUP BY a.id, a.Email, c.Name
ORDER BY Trips DESC, a.Id

--13. Lucrative Destinations
SELECT top 10 c.Id 
		,c.Name
		, sum(BaseRate+ price) as [Total Revenue]
		, count(c.Name) as [Trips]
from cities as c
join Hotels as h on h.CityId = c.Id
join Rooms as r on r.HotelId = h.Id
join Trips as t on t.RoomId = r.Id
where YEAR(t.BookDate) = 2016
group by c.Name , c.Id
order by [Total Revenue] DESC, Trips DESC

--14. Trip Revenues
SELECT t.id, h.Name, r.Type,CASE
WHEN t.CancelDate IS  NOT NULL
THEN 0.00
ELSE
SUM(h.BaseRate + r.Price)
END AS Revenue
FROM Rooms as r 
JOIN Hotels as h on r.HotelId = h.Id
JOIN Trips as t on r.Id = t.RoomId
JOIN AccountsTrips as at on at.TripId = t.Id
group by t.Id, h.Name, r.Type, t.CancelDate, r.Id
order by r.Type, t.Id

--15. Top Travelers
SELECT k.Id, k.Email, k.CountryCode, k.Trips
FROM(
SELECT a.Id,
		a.Email
		,c.CountryCode
		,count(at.AccountId) as Trips
		,ROW_NUMBER() OVER (PARTITION BY c.CountryCode ORDER BY count(at.AccountId) DESC) as [Rank]
FROM AccountsTrips as at 
join Trips as t on t.Id = at.TripId 
join Rooms as r on r.Id = t.Id
join Hotels as h on r.HotelId = h.Id
join Cities as c on h.CityId = c.Id
join Accounts as a on at.AccountId = a.Id
group by a.Id, a.Email, c.CountryCode) as k
WHERE Rank = 1
order by k.Trips DESC, k.Id

--16. Luggage Fees
select at.TripId
		, sum(at.Luggage) as Luggage
		, case
			when sum(at.Luggage) >5 then '$' + CAST(sum(at.Luggage *5)as varchar)
			else '$0'
		end as Fee
from AccountsTrips as at
group by at.TripId
having sum(at.Luggage) > 0 
order by Luggage desc	

--17. GDPR Violation
select t.Id
		,CONCAT(a.FirstName,' '+ a.MiddleName,' ',a.LastName) as FullName 
		,accCity.Name as [From]
		,hotCity.Name as [To]--, DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate), t.CancelDate
		,CASE
		   WHEN t.CancelDate IS NOT NULL THEN 'Canceled' 
		   ELSE cast(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)as varchar) + ' days'
		END
from Trips as t
join AccountsTrips as at on at.TripId = t.Id
join Accounts as a on at.AccountId = a.Id
join Rooms as r on t.RoomId = r.Id
join Hotels as h on r.HotelId = h.Id
join Cities as accCity on a.CityId =accCity.Id
join Cities as hotCity on hotCity.Id = h.CityId
ORDER BY FullName, t.Id


