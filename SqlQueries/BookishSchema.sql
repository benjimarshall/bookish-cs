-- Drop the old tables

IF OBJECT_ID('loans') IS NOT NULL
    DROP TABLE loans;

IF OBJECT_ID('bookcopies') IS NOT NULL
    DROP TABLE bookcopies;

IF OBJECT_ID('books') IS NOT NULL
    DROP TABLE books;

-- Create the tables

CREATE TABLE books (
    isbn NVARCHAR(17) NOT NULL PRIMARY KEY,
    title NVARCHAR(255) NOT NULL,
    authors NVARCHAR(255) NOT NULL
);

CREATE TABLE bookcopies (
    id int IDENTITY(1, 1) NOT NULL PRIMARY KEY,
    isbn NVARCHAR(17) NOT NULL FOREIGN KEY REFERENCES books (isbn)
);

CREATE TABLE loans (
    bookid int NOT NULL PRIMARY KEY FOREIGN KEY REFERENCES bookcopies (id),
    userid nvarchar(450) NOT NULL FOREIGN KEY REFERENCES AspNetUsers (Id),
    due DATE NOT NULL
);
