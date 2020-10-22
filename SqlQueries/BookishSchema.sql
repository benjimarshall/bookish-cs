-- Delete and recreate the cat_shelter database

-- DROP DATABASE IF EXISTS Bookish;
-- CREATE DATABASE Bookish;
-- USE Bookish;

-- Create the tables

IF OBJECT_ID('loans') IS NOT NULL
    DROP TABLE loans;

IF OBJECT_ID('bookcopies') IS NOT NULL
    DROP TABLE bookcopies;

IF OBJECT_ID('books') IS NOT NULL
    DROP TABLE books;

IF OBJECT_ID('users') IS NOT NULL
    DROP TABLE users;

CREATE TABLE users (
	id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	username NVARCHAR(255) NOT NULL
);

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
	userid INT NOT NULL FOREIGN KEY REFERENCES users (id),
	due DATE NOT NULL
);

