CREATE TABLE Students
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName nvarchar(30) NOT NULL,
	MiddleName nvarchar(25),
	LastName nvarchar(30) NOT NULL,
	Age INT NOT NULL,
	Address nvarchar(50),
	Phone char(10),
	CHECK(Age BETWEEN 5 and 100),
	CHECK(Age >0)
)

CREATE TABLE Subjects
(
	Id INT PRIMARY KEY IDENTITY,
	Name nvarchar(20) NOT NULL,
	Lessons int NOT NULL,
	CHECK(Lessons > 0)
)

CREATE TABLE StudentsSubjects
(
	Id INT PRIMARY KEY IDENTITY,
	StudentId INT NOT NULL REFERENCES Students(Id),
	SubjectId INT NOT NULL REFERENCES Subjects(ID),
	Grade DECIMAL(15,2) NOT NULL,
	CHECK(Grade BETWEEN 2.00 and 6.00)
)

CREATE TABLE Exams
(
	Id INT PRIMARY KEY IDENTITY,
	Date DATETIME,
	SubjectId INT NOT NULL REFERENCES Subjects(ID),
)

CREATE TABLE StudentsExams
(
	StudentId INT NOT NULL REFERENCES Students(Id),
	ExamId INT NOT NULL REFERENCES Exams(Id),
	Grade DECIMAL(15,2) NOT NULL,
	PRIMARY KEY(StudentId,ExamId),
	CHECK(Grade BETWEEN 2.00 and 6.00),
)

CREATE TABLE Teachers
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName nvarchar(20) NOT NULL,
	LastName nvarchar(20) NOT NULL,
	Address nvarchar(20) NOT NULL,
	Phone char(10),
	SubjectId INT NOT NULL REFERENCES Subjects(ID)
)

CREATE TABLE StudentsTeachers
(
	StudentId INT NOT NULL REFERENCES Students(Id),
	TeacherId INT NOT NULL REFERENCES Teachers(Id),
	PRIMARY KEY(StudentId,TeacherId)
)

--2. Insert
INSERT INTO Teachers VALUES
('Ruthanne',	'Bamb',	'84948 Mesta Junction'	,'3105500146',	6),
('Gerrard'	,'Lowin'	,'370 Talisman Plaza',	'3324874824',	2),
('Merrile'	,'Lambdin'	,'81 Dahle Plaza',	'4373065154',	5),
('Bert',	'Ivie',	'2 Gateway Circle',	'4409584510',	4)

INSERT INTO Subjects VALUES
('Geometry',	12),
('Health',	10),
('Drama',	7),
('Sports'	,9)


--3. Update
UPDATE StudentsSubjects
SET Grade =  6.00
WHERE SubjectId = 1 OR SubjectId = 2 and GRADE >5.50 

SELECT * FROM StudentsExams

SELECT SUM(Grade) FROM StudentsSubjects as ss
WHERE SubjectId = 1  and GRADE >= 5.50 OR SubjectId = 2 and ss.Grade >= 5.50 
--4 
DELETE t
FROM Teachers as t 
WHERE t.Phone LIKE '%72%'

DELETE FROM StudentsTeachers WHERE TeacherId  IN (SELECT TeacherId FROM Teachers as t where Phone LIKE '%72%')
DELETE FROM Teachers 

select * FROM Teachers as t where Phone LIKE '%72%'
SELECT * FROM Teachers
where Phone LIKE '%72%'

--5. Teen Students
SELECT s.FirstName, s.LastName, s.Age
FROM Students as s 
where s.Age >=12
order by s.FirstName, s.LastName


-- 6. Cool Addresses
SELECT s.FirstName+ case
					WHEN s.MiddleName is null then ' '
					else
					' '+ s.MiddleName+ ' '
					 end +s.LastName as FullName  , s.Address
from Students as s
WHERE s.Address LIKE '%Road'
order by s.FirstName, s.LastName, s.Address

--7. 42 Phones
select s.FirstName,s.Address, s.Phone
from Students as s
WHERE s.MiddleName IS NOT NULL and s.Phone LIKE '42%'
order by s.FirstName

--8. Students Teachers
select s.FirstName, s.LastName, count(st.TeacherId) 
FROM StudentsTeachers as st
join Students as s on st.StudentId = s.Id
group by s.FirstName, s.LastName

--9. Subjects with Students
SELECT CONCAT(FirstName,' ', t.LastName) as FullName
		,CONCAT(s.Name,'-',s.Lessons) 
		, count(StudentId)  as studc
FROM StudentsTeachers as st
join Teachers as t on st.TeacherId = t.Id
join Subjects as s on t.SubjectId = s.Id
group by t.FirstName, t.LastName, s.Name, s.Lessons
order by studc desc, FullName,s.Name 

select *
FROM StudentsTeachers as st
join Teachers as t on st.TeacherId = t.Id
join Subjects as s on t.SubjectId = s.Id
--group by t.FirstName, t.LastName, s.Name
order by studc desc
--10. Students to Go 
SELECT CONCAT(s.FirstName,' ',s.LastName) as fn
FROM Students as s
left join StudentsExams as se on s.Id = se.StudentId
where ExamId is null
order by fn 

--11. Busiest Teachers 
select top 10  t.FirstName, t.LastName, count(StudentId) as ccnt
from StudentsTeachers as st
join Teachers as t on st.TeacherId = t.Id
group by t.FirstName, t.LastName
order by ccnt desc, t.FirstName, t.LastName

--12. Top Students
select TOP 10 s.FirstName, s.LastName, cast(AVG(Grade) as numeric(36,2)) as ggg
from StudentsExams as se
join Students as s on se.StudentId = s.Id
group by s.FirstName, s.LastName
order by ggg desc, s.FirstName, s.LastName

--13. Second Highest Grade
SELECT t.FirstName, t.LastName, t.Grade
FROM(
select s.FirstName, s.LastName, se.Grade,
		ROW_NUMBER() OVER (PARTITION BY se.StudentId ORDER By se.grade desc ) as rank
from StudentsSubjects as se
join Students as s on se.StudentId = s.Id ) as t
where rank =2 
order by t.FirstName, t.LastName

--14. Not So In The Studying

Select CONCAT(FirstName, ' ' + MiddleName, ' ', LastName) AS [Full Name]
from Students as s
full join StudentsSubjects as ss on ss.StudentId = s.Id
where ss.SubjectId is null
order by [Full Name]
--16. Average Grade per Subject


select * from StudentsSubjects
select * from exams

select s.Name, avg(grade) as avgrd
from StudentsSubjects as se 
join Subjects as s on se.SubjectId = s.Id
group by s.Name,SubjectId
order by SubjectId

--17. Exams Information
--select CASE
--		WHEN MONTH(e.date) BETWEEN 1 and 3 THEN 'Q1' 
--		WHEN MONTH(e.date) BETWEEN 4 and 6 THEN 'Q2' 
--		WHEN MONTH(e.date) BETWEEN 7 and 9 THEN 'Q3' 
--		WHEN MONTH(e.date) BETWEEN 10 and 12 THEN 'Q4' 
--		ELSE 'TBA'
--	END as [Quarter], s.Name, count(s.Name)


SELECT CASE
		WHEN DATEPART(QUARTER,e .Date) is not null then CONCAT('Q',DATEPART(QUARTER,e .Date))
		else 'TBA'
		END
 as [QUARTER], s.Name, count(s.Name)
from StudentsExams as se
join Exams as e on se.ExamId = e.Id
join Subjects as s on e.SubjectId = s.Id
WHERE se.Grade >= 4.00
GROUP BY s.name, e.Date
order by [Quarter]

SELECT QUARTER(e.Date)
from Exams as e 

select CASE
		WHEN datepart(QUARTER,e.date) BETWEEN 1 and 3 THEN 'Q1' 
		WHEN datepart(QUARTER,e.date) BETWEEN 4 and 6 THEN 'Q2' 
		WHEN datepart(QUARTER,e.date) BETWEEN 7 and 9 THEN 'Q3' 
		WHEN datepart(QUARTER,e.date)BETWEEN 10 and 12 THEN 'Q4' 
		ELSE 'TBA'
	END as [Quarter], s.Name, count(se.StudentId)
from StudentsExams as se
join Exams as e on se.ExamId = e.Id
join Subjects as s on e.SubjectId = s.Id
WHERE se.Grade >= 4.00
GROUP BY s.name, se.ExamId, e.Date
order by [Quarter]

select se.ExamId ,count(se.StudentId)
from StudentsExams as 
join Exams as e on se.ExamId = e.Id
join Subjects as s on e.SubjectId = s.Id
WHERE se.Grade >= 4.00
group by se.ExamId
--18
GO
CREATE FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(15,2))
RETURNS varchar(max)
AS 
BEGIN
	IF(@grade > 6.00)
	BEGIN
		RETURN 'Grade cannot be above 6.00!'
	END

	DECLARE @student nvarchar(20)= (SELECT s.FirstName 
									FROM Students as s 
									where @studentId = s.Id)
	IF(@student is null)
	BEGIN
		RETURN 'The student with provided id does not exist in the school!'
	END
	
	DECLARE @numberOfGrades INT = (SELECT COUNT(ss.Grade) FROM StudentsExams as ss  where ss.Grade BETWEEN @grade AND @grade +0.50 and ss.StudentId = @studentId)

	return  'You have to update '+cast(@numberOfGrades as varchar)+' grades for the student '+cast(@student as varchar)
END

--SELECT dbo.udf_ExamGradesToUpdate(12, 6.20)

--drop function udf_ExamGradesToUpdate

SELECT dbo.udf_ExamGradesToUpdate(12, 5.50)


-- 19
go
CREATE PROCEDURE usp_ExcludeFromSchool(@StudentId int)
AS 
	DECLARE @student int = (SELECT s.Id FROM Students as s where @StudentId = s.Id)
	IF(@student is null)
	BEGIN
		RAISERROR('This school has no student with the provided id!',16, 1)
		RETURN
	END

	DELETE FROM StudentsExams
	WHERE StudentId = 1

	DELETE FROM StudentsTeachers
	WHERE StudentId = 1

	DELETE FROM StudentsSubjects
	WHERE StudentId = 1

	DELETE FROM Students
	WHERE Id = 1



EXEC usp_ExcludeFromSchool 1
SELECT COUNT(*) FROM Students


--
SELECT count(ss.Grade)
FROM StudentsExams as ss  
where ss.Grade BETWEEN 5.50 AND 6 and ss.StudentId = 12

--20

CREATE TABLE ExcludedStudents
(
	StudentId int
	, StudentName nvarchar(50)
)

CREATE TABLE DeletedOrders
(
	OrderId int,
	ItemId int, 
	ItemQuantity int
)

go
CREATE TRIGGER tr_DeletedOrders  ON students  FOR DELETE
AS
INSERT INTO ExcludedStudents (StudentId, StudentName)
	SELECT Id, FirstName from deleted

	drop trigger tr_DeletedOrders 




	DELETE FROM StudentsExams
WHERE StudentId = 1

DELETE FROM StudentsTeachers
WHERE StudentId = 1

DELETE FROM StudentsSubjects
WHERE StudentId = 1

DELETE FROM Students
WHERE Id = 1

SELECT * FROM ExcludedStudents
