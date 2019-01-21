create database Minions

use Minions

CREATE TABLE Minions(
MinionID int primary key,
Name varchar(50),
Age int
)

create table Towns(
TownID int primary key,
Name varchar(50) NOT NULL,
);

ALTER TABLE Minions
ADD TownID int; 

ALTER TABLE Minions
ADD FOREIGN KEY (TownID) REFERENCES Towns(TownID); 

INSERT INTO dbo.Towns
VALUES(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO dbo.Minions
VALUES(1, 'Kevin', 22, 1),
(2, 'Bob', 15, 3),
(3, 'Steward', null , 2)

DELETE FROM dbo.Minions

DROP TABLE dbo.Minions
drop table dbo.Towns
