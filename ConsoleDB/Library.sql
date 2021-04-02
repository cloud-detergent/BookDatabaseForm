Use [Library];
GO

CREATE TABLE Authors (
	Id	INTEGER NOT NULL IDENTITY(1,1),
	FirstName	NVARCHAR(50),
	LastName	NVARCHAR(50),
	PRIMARY KEY(id)
);

CREATE TABLE Books (
	Id	INTEGER NOT NULL IDENTITY(1,1),
	Name	NVARCHAR(50),
	PRIMARY KEY(Id)
);

CREATE TABLE AuthorsBooks (
	BookId	INTEGER,
	AuthorId	INTEGER,
	PRIMARY KEY(BookId, AuthorId),
	FOREIGN KEY(AuthorId) REFERENCES Authors(id),
	FOREIGN KEY(BookId) REFERENCES Books(id)
);
