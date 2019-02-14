--1. Database Design
CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL 
)

CREATE TABLE Items
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	CategoryId INT NOT NULL REFERENCES Categories(Id)
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL,
	Salary  DECIMAL(18,2) NOT NULL
)

CREATE TABLE Orders
(
	Id INT PRIMARY KEY IDENTITY,
	[DateTime] DATETIME NOT NULL,
	EmployeeId INT NOT NULL REFERENCES Employees(Id)
)

CREATE TABLE OrderItems
(
	OrderId INT NOT NULL REFERENCES Orders(Id),
	ItemId INT NOT NULL REFERENCES Items(Id),
	Quantity INT NOT NULL CHECK(Quantity >= 1),
	PRIMARY KEY(OrderId, ItemId)
)

CREATE TABLE Shifts
(
	Id INT NOT NULL IDENTITY,
	EmployeeId INT NOT NULL REFERENCES Employees(Id),
	CheckIn DATETIME NOT NULL,
	CheckOut DATETIME NOT NULL 
	PRIMARY KEY(Id, EmployeeId)
)

alter table Shifts
ADD CONSTRAINT CHK_Person CHECK (CheckOut > CheckIn)

--2. Insert
INSERT INTO Employees VALUES
('Stoyan','Petrov','888-785-8573',500.25),
('Stamat','Nikolov','789-613-1122',999995.25),
('Evgeni','Petkov','645-369-9517',1234.51),
('Krasimir','Vidolov','321-471-9982',50.25)

INSERT INTO Items VALUES 
('Tesla battery',154.25,8),
('Chess',30.25,	8),
('Juice',5.32,1),
('Glasses',10,8),
('Bottle of water',	1,1)

--3. Update
UPDATE Items
SET Price *= 1.27
WHERE CategoryId IN (1,2,3)

--4. Delete
DELETE FROM OrderItems
 WHERE OrderId = 48

--5. Richest People
SELECT Id, FirstName
FROM Employees
WHERE Salary > 6500
ORDER BY FirstName, Id

--6. Cool Phone Numbers
SELECT e.FirstName + ' ' + e.LastName AS [Full Name],
		e.Phone
FROM Employees AS e 
WHERE LEFT(Phone,1) = 3
ORDER BY e.FirstName, Phone

--7. Employee Statistics
SELECT e.FirstName, e.LastName, count(o.EmployeeId) as [Count]
FROM Employees as e 
 JOIN Orders AS o ON o.EmployeeId = e.Id
GROUP BY e.FirstName, e.LastName
ORDER BY [Count] DESC, e.FirstName

--8. Hard Workers Club
SELECT e.FirstName, e.LastName, AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) as [Work hours]
FROM Employees as e
JOIN Shifts as s ON s.EmployeeId = e.Id
GROUP BY e.FirstName, e.LastName, s.EmployeeId
HAVING AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) > 7
ORDER BY [Work hours] DESC, s.EmployeeId

--9. The Most Expensive Order
SELECT TOP 1 t.OrderId , sum(Price) as TotalPrice
FROM (
	select oi.ItemId ,oi.OrderId, SUM(Quantity) * Price as Price
	from OrderItems as oi
	join Items as i on i.Id = oi.ItemId
	group by oi.ItemId, oi.OrderId, Price) as t
group by OrderId
order by TotalPrice DESC

--10. Rich Item, Poor Item
SELECT TOP 10 t.OrderId, MAx(t.maxPrice) AS ExpensivePrice, MIN(t.minPrice)
FROM(
	select oi.OrderId, MAX(Price) as maxPrice , MIN(Price) as minPrice
	from OrderItems as oi
	join Items as i on i.Id = oi.ItemId
	group by oi.ItemId, oi.OrderId) as t
group by t.OrderId
ORDER BY MAx(t.maxPrice) DESC, OrderId

--11. Cashiers
SELECT e.Id, e.FirstName, e.LastName
FROM Orders as o
JOIN Employees as e ON e.Id = o.EmployeeId
GROUP BY e.Id, e.FirstName, e.LastName
ORDER BY e.Id

--12. Lazy Employees
SELECT DISTINCT s.EmployeeId as Id, e.FirstName+ ' '+ e.LastName as [Full Name]
FROM Employees as e
JOIN Shifts as s ON s.EmployeeId = e.Id
GROUP BY e.FirstName, e.LastName, s.EmployeeId, s.CheckIn, s.CheckOut
HAVING DATEDIFF(HOUR, s.CheckIn, s.CheckOut) < 4
ORDER BY s.EmployeeId

--13. Sellers
SELECT t.[Full Name]
	, SUM(t.Price) as [Total Price]
	, SUM(t.coun) as Items
FROM (
	SELECT e.FirstName + ' '+ e.LastName AS [Full Name]
			,SUM(Quantity) * Price as Price
			,o.[DateTime]
			,sum(oi.Quantity) as coun
	FROM Employees AS e 
	JOIN Orders AS o ON o.EmployeeId = e.Id
	JOIN OrderItems as oi ON oi.OrderId = o.Id
	JOIN Items AS i on oi.ItemId = i.Id
	GROUP BY e.FirstName, e.LastName, Price, o.[DateTime]) as t
WHERE t.[DateTime] < '2018/06/15'
GROUP BY t.[Full Name]
ORDER BY [Total Price] DESC, Items DESC

--14. Tough days
SELECT e.FirstName+ ' '+  e.LastName AS [Full Name] , DATENAME(dw, s.CheckOut) as [Day of week]
FROM Employees AS e
LEFT JOIN Orders as o ON e.Id = o.EmployeeId
	 JOIN Shifts as s ON e.Id = s.EmployeeId
WHERE o.EmployeeId IS NULL AND DATEDIFF(HOUR, s.CheckIn, s.CheckOut) > 12
ORDER BY e.Id

--15. Top Order per Employee
SELECT t.[Full Name],DATEDIFF(HOUR, s.CheckIn, s.CheckOut) as [WorkHours], t.TotalSum
from (
	SELECT e.FirstName+ ' '+  e.LastName AS [Full Name]
		, SUM(oi.Quantity * i.Price) AS [TotalSum]
		,ROW_NUMBER() OVER (PARTITION BY e.id ORDER BY SUM(oi.Quantity * i.Price)DESC ) as RowNumber
		,e.Id as EmpId
		, o.DateTime
	FROM Employees AS e 
	JOIN Orders AS o ON e.Id = o.EmployeeId
	JOIN OrderItems as oi ON o.Id = oi.OrderId
	JOIN Items AS i ON oi.ItemId = i.Id
	GROUP BY e.FirstName, e.LastName, o.Id, e.Id, o.DateTime ) as t
JOIN Shifts as S on s.EmployeeId = t.EmpId
WHERE RowNumber = 1 AND t.DateTime BETWEEN s.CheckIn AND s.CheckOut
ORDER BY t.[Full Name], WorkHours DESC, TotalSum DESC


--16. Average Profit per Day
SELECT t.Day, CAST(AVG(t.[sum per day]) as decimal(15,2))as average
FROM (
SELECT 
	 DAY( o.DateTime) as [Day]
	,(oi.Quantity * i.Price) as [sum per day]
	, o.DateTime
FROM Orders as o
JOIN OrderItems as oi ON oi.OrderId = o.Id
JOIN Items as i on i.Id = oi.ItemId ) as t
group by t.Day
order by Day

--17. Top Products
SELECT i.Name, c.Name ,sum(Quantity)as [count], sum(Quantity * price) as TotalProfit
FROM Items as i
LEFT JOIN OrderItems as oi on i.Id = oi.ItemId
join Categories as c on c.Id = i.CategoryId
group by ItemId, i.Name, c.Name
order by TotalProfit DESC, [count] Desc

--18. Promotion days
CREATE FUNCTION udf_GetPromotedProducts(@CurrentDate DATETIME, @StartDate DATETIME, @EndDate DATETIME, @Discount INT, @FirstItemId INT, @SecondItemId INT , @ThirdItemId INT)
RETURNS varchar(max)
AS 
BEGIN
	DECLARE @firstItemName varchar(50) = (SELECT i.Name FROM Items as i WHERE i.Id =@FirstItemId)
	DECLARE @secondItemName varchar(50) = (SELECT i.Name FROM Items as i WHERE i.Id =@SecondItemId)
	DECLARE @thirdItemName varchar(50) = (SELECT i.Name FROM Items as i WHERE i.Id =@ThirdItemId)

	IF(@firstItemName IS NULL OR @secondItemName IS NULL OR @thirdItemName IS NULL)
	BEGIN
		RETURN 'One of the items does not exists!'
	END

	IF(@CurrentDate NOT BETWEEN @StartDate AND @EndDate)
	BEGIN
		RETURN 'The current date is not within the promotion dates!'
	END


	DECLARE @firstItemPrice DECIMAL(10,2) = (SELECT i.Price FROM Items as i WHERE i.Id =@FirstItemId) 
	DECLARE @secondItemPrice DECIMAL(10,2) = (SELECT i.Price FROM Items as i WHERE i.Id =@SecondItemId)
	DECLARE @thirdItemPrice DECIMAL(10,2) = (SELECT i.Price FROM Items as i WHERE i.Id = @ThirdItemId)
	 
	 SET @firstItemPrice = @firstItemPrice - (@firstItemPrice * (@Discount/100))
	 SET @secondItemPrice = @secondItemPrice - (@secondItemPrice * (@Discount/100))
	 SET @thirdItemPrice = @thirdItemPrice - (@thirdItemPrice * (@Discount/100))

	RETURN @firstItemName + ' price: ' + cast( @firstItemPrice as varchar(10)) + ' <-> ' +@secondItemName  +' price: ' + cast(@secondItemPrice  as varchar(10))+ ' <-> ' + @thirdItemName + ' price: ' + CAST(@thirdItemPrice  as varchar(10))

END


--19. Cancel order
CREATE PROCEDURE usp_CancelOrder(@OrderId INT, @CancelDate DATETIME)
AS 

DECLARE @targetOrder INT = (SELECT Id FROM Orders WHERE Id = @OrderId)
IF(@targetOrder IS NULL)
BEGIN
	RAISERROR('The order does not exist!',16, 1)
	RETURN
END

DECLARE @orderDatetime DATETIME = (SELECT o.[DateTime] FROM Orders as o WHERE Id = @OrderId)
IF(DATEDIFF(DAY, @orderDatetime, @CancelDate) >3)
BEGIN
	RAISERROR('You cannot cancel the order!',16, 2)
	RETURN
END

DELETE FROM OrderItems
WHERE OrderId = @OrderId

DELETE FROM Orders
WHERE @OrderId = Id


--20. Deleted Order
CREATE TABLE DeletedOrders
(
	OrderId int,
	ItemId int, 
	ItemQuantity int
)


CREATE TRIGGER tr_DeletedOrders  ON OrderItems FOR DELETE
AS
INSERT INTO DeletedOrders (ItemId, OrderId, ItemQuantity)
	SELECT ItemId, OrderId, Quantity from deleted
