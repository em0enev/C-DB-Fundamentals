--Create database
CREATE DATABASE SoftUni
use SoftUni

--Create tables
CREATE TABLE Towns(
Id int IDENTITY PRIMARY KEY,
Name varchar(MAX) NOT NULL
)

CREATE TABLE Addresses(
Id int IDENTITY PRIMARY KEY,
AddresText text NOT NULL,
TownId int NOT NULL
)

CREATE TABLE Departments(
Id int IDENTITY PRIMARY KEY,
Name varchar(MAX) NOT NULL
)

CREATE TABLE Employees(
Id int IDENTITY PRIMARY KEY,
FirstName varchar(50) NOT NULL,
MiddleName varchar(50),
LastName varchar(50) NOT NULL,
JobTitle varchar(50) NOT NULL,
DepartmentId int NOT NULL,
HireDate date NOT NULL,
Salary money NOT NULL,
AddresId int
)

drop table Employees
--Add foreign keys to tables
ALTER TABLE Addresses
ADD FOREIGN KEY (TownID) REFERENCES Towns(ID); 

ALTER TABLE Employees
ADD FOREIGN KEY (DepartmentId) REFERENCES Departments(Id); 

ALTER TABLE Employees
ADD FOREIGN KEY (AddresId) REFERENCES Addresses(Id); 

--Insert data into tables
INSERT INTO Towns VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

INSERT INTO Departments VALUES
('Engineering'),
('Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')

INSERT INTO Employees VALUES
('Ivan','Ivanov','Ivanov','.NET Developer',4,'2013-02-01', 3500.00, null),
('Petar','Petrov','Petrov','Senior Engineer',1,'2004-03-02', 4000.00, null),
('Maria','Petrova','Ivanova','Intern',5,'2016-08-28', 525.25, null),
('Georgi','Teziev','Ivanov','CEO',2,'2007-12-09', 3000.00, null),
('Peter','Pan','Pan','Intern',3,'2016-08-28', 599.88, null)

--Basic select all fields
SELECT * FROM Towns

SELECT * FROM Departments

SELECT * FROM Employees

--Basic select and order
SELECT * FROM Towns
ORDER BY Name ASC

SELECT * FROM Departments
ORDER BY Name ASC

SELECT * FROM Employees
ORDER BY Salary DESC

--Basic select some fileds
SELECT Name FROM Towns
ORDER BY Name ASC

SELECT Name FROM Departments
ORDER BY Name ASC

SELECT FirstName, LastName, JobTitle, Salary FROM Employees
ORDER BY Salary DESC

-- update salary by 10 % 
update Employees
set Salary = Salary + (Salary * 10.0 / 100.0)

SELECT Salary FROM Employees
