-- Select "Minions" database 
use Minions

-- Create table dbo.Users
CREATE TABLE Users(
Id bigint IDENTITY(1,1),
Username varchar(30) NOT NULL UNIQUE,
Password varchar(26) NOT NULL,
ProfilePicture varbinary(2),
LastLoginTime datetime,
IsDeleted bit NOT NULL,
)

-- Add constraint: primary key combination of Id and Username
ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY(ID, Username)

-- Insert data into table
INSERT INTO Users Values
('Stela', '21321s', null, null, 0),
('Vasko', 'divkon1', null, null, 1),
('Pesho', 'dsads', null, null, 0),
('Ludiq', '213dsad21s', null, null, 0),
('Diviq', 'dsadw', null, null, 1)

-- Remove constaint 
ALTER TABLE Users
DROP CONSTRAINT PK_Users

-- Check password length
ALTER TABLE Users
ADD CHECK (LEN(password) >= 5)

--Set default value for field "LastLoginTime"
ALTER TABLE Users
ADD CONSTRAINT df_LoginTime
DEFAULT GETDATE() FOR LastLoginTime; 

-- Test constaint
INSERT INTO Users Values
('petko', 'petkotest01', null, default , 0)