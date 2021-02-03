BEGIN TRANSACTION;

ALTER TABLE dbo.Author 
ADD emailAddress varchar(100) NULL


ALTER TABLE dbo.BookAuthor
ALTER COLUMN royality_percentage int

Commit