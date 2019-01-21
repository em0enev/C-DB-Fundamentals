USE Minions

CREATE TABLE People(
PersonId int IDENTITY(1,1) PRIMARY KEY,
Name varchar(200) NOT NULL,
Picture varbinary(max),
Height float(2),
Weight float(2),
Gender char(1) NOT NULL,
Birthdate datetime NOT NULL,
Biography TEXT
);

INSERT INTO People Values
('Stela',Null,1.65,44.55,'f','2000-09-22',Null),
('Ivan',Null,2.15,95.55,'m','1989-11-02',Null),
('Qvor',Null,1.55,33.00,'m','2010-04-11',Null),
('Karolina',Null,2.15,55.55,'f','2001-11-11',Null),
('Pesho',Null,1.85,90.00,'m','1983-07-22',Null)

ALTER TABLE dbo.People
ALTER COLUMN Birthdate date; 

exec sp_RENAME 'dbo.People.[PersonId]' , '[Id]', 'COLUMN'
