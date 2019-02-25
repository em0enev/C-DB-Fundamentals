-- Create database MOVIES and select it;
CREATE DATABASE Movies
use Movies

--Create table DIRECTORS
CREATE TABLE Directors(
Id int IDENTITY(1,1) PRIMARY KEY,
DirectorName varchar(50) NOT NULL,
Notes varchar(255)
)

--Insert records into table DIRECTORS
INSERT INTO Directors VALUES
('Ivan', null),
('Gosho',null),
('Pesho',null),
('Mitko',null),
('BigBoss','i am big boss :D')

--Create table GENRES
CREATE TABLE Genres(
Id int IDENTITY(1,1) PRIMARY KEY,
GenreName varchar(50) NOT NULL,
Notes varchar(255)
)

--Insert records into table GENRES
INSERT INTO Genres VALUES
('comedy', null),
('drama', null),
('action', null),
('western', null),
('horror', null)

--Create table CATEGORIES
CREATE TABLE Categories(
Id int IDENTITY(1,1) PRIMARY KEY,
CategoryName varchar(50) NOT NULL,
Notes varchar(255)
)

--Insert records into table CATEGORIES
INSERT INTO Categories VALUES
('one', null),
('two', null),
('three', null),
('four', null),
('five', null)

--Create table MOVIES
CREATE TABLE Movies(
Id int IDENTITY(1,1) PRIMARY KEY,
Title varchar(50) NOT NULL,
DirectorId int NOT NULL,
CopyrightYear date NOT NULL,
Length time NOT NULL,
GenreId int NOT NULL,
CategoryId int NOT NULL,
Rating int,
Notes varchar(255)
)

--Add foreign keys to table MOVIES
ALTER TABLE Movies
ADD FOREIGN KEY (DirectorId) REFERENCES Directors(Id)

ALTER TABLE Movies
ADD FOREIGN KEY (GenreId) REFERENCES Genres(Id)

ALTER TABLE Movies
ADD FOREIGN KEY (CategoryId) REFERENCES Categories(Id)

--Insert records into table MOVIES
INSERT INTO Movies VALUES
('Movie1',1,'1999-01-01','01:26',1,4,null, null),
('Movie2',2,'2012-01-01','01:15',2,5,null, null),
('Movie3',3,'2013-01-01','01:23',3,3,null, null),
('Movie4',4,'2014-01-01','01:12',5,2,null, null),
('Movie5',5,'2004-01-01','01:11',4,1,null, null)