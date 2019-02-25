--Problem 1. One-To-One Relationship
CREATE TABLE Persons
( 
	 PersonID INT PRIMARY KEY 
	,FirstName VARCHAR(20) NOT NULL
	,Salary DECIMAL(15,2)
	,PassportID INT NOT NULL  
)

CREATE TABLE Passports
(
	 PassportID INT PRIMARY KEY
	,PassportNumber CHAR(8) NOT NULL 
)

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports FOREIGN KEY (PassportId) REFERENCES Passports(PassportID)

ALTER TABLE Persons
ADD UNIQUE (PassportId)

INSERT INTO Passports VALUES
(101, 'N34FG21B')
,(102, 'K65LO4R7')
,(103, 'ZE657QP2')

INSERT INTO Persons VALUES 
(1,'Roberto', 43300, 102)
,(2,'Tom', 56100, 103)
,(3,'Yana', 60200, 101)

--Problem 2. One-To-Many Relationship
CREATE TABLE Manufacturers
(
	ManufacturerID INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(20) NOT NULL,
	EstablishedOn DATE NOT NULL
)

CREATE TABLE Models
(
	ModelID INT PRIMARY KEY IDENTITY(101,1),
	Name VARCHAR(20) NOT NULL,
	ManufacturerID INT FOREIGN KEY REFERENCES Manufacturers(ManufacturerID)
)

INSERT INTO Manufacturers VALUES
('BMW',	'07/03/1916'),
('Tesla','01/01/2003'),
('Lada','01/05/1966')

INSERT INTO Models VALUES
('X1',1),
('i6',1),
('Model S',	2),
('Model X'	,2),
('Model 3'	,2),
('Nova'	,3)

--Problem 3. Many-To-Many Relationship
CREATE TABLE Students
(
	StudentID INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(50) NOT NULL,
)

CREATE TABLE Exams
(
	ExamID INT PRIMARY KEY IDENTITY(101,1),
	Name VARCHAR(50) NOT NULL,
)

CREATE TABLE StudentsExams
(
	StudentID INT NOT NULL FOREIGN KEY REFERENCES Students(StudentID),
	ExamID INT  NOT NULL FOREIGN KEY REFERENCES Exams(ExamID)
)

ALTER TABLE StudentsExams
ADD CONSTRAINT PK_StudentsExams PRIMARY KEY (StudentId, ExamId) 

INSERT INTO Students VALUES
('Mila'),
('Toni'),
('Ron')

INSERT INTO Exams VALUES
('SpringMVC'),
('Neo4j'),
('Oracle 11g')

INSERT INTO StudentsExams VALUES
(1, 101),
(1, 102),
(2, 101),
(3, 103),
(2, 102),
(2, 103)

--Problem 4.	Self-Referencing 
CREATE TABLE Teachers
(
	TeacherID INT PRIMARY KEY IDENTITY(101,1),
	Name VARCHAR(20) NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES Teachers(TeacherId)
)

--Problem 6. University Database
CREATE UniversityDatabase
use UniversityDatabase

CREATE TABLE Majors
(
	MajorID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(20) NOT NULL
)

CREATE TABLE Payments
(
	PaymentID INT PRIMARY KEY IDENTITY(1,1),
	PaymentDate DATE NOT NULL,
	PaymentAmount DECIMAL(15,2) NOT NULL,
	StudentID INT NOT NULL -- MAKE IT FOREIGN 
)

CREATE TABLE Students
(
	StudentID INT PRIMARY KEY IDENTITY(1,1),
	StudentNumber INT NOT NULL UNIQUE, 
	StudentName VARCHAR(50) NOT NULL, 
	MajorID INT NOT NULL 
)

CREATE TABLE Agenda 
(
	StudentID INT NOT NULL,
	SubjectID INT NOT NULL,
	CONSTRAINT PK_Agenda PRIMARY KEY (StudentID, SubjectID)
)

CREATE TABLE Subjects 
(
	SubjectID INT PRIMARY KEY IDENTITY(1,1),
	SubjectName VARCHAR(50) NOT NULL 
)

ALTER TABLE Agenda
ADD CONSTRAINT  FK_Agenda_Subjects FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId)

ALTER TABLE Agenda
ADD CONSTRAINT  FK_Agenda_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID)

ALTER TABLE Payments
ADD CONSTRAINT  FK_Payments_Students FOREIGN KEY (StudentID) REFERENCES Students(StudentID)

ALTER TABLE Students
ADD CONSTRAINT  FK_Majors_Students FOREIGN KEY (MajorId) REFERENCES Majors(MajorId)
