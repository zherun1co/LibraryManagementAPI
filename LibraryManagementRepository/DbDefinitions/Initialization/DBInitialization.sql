USE master;

-- Check if the database LibraryManagementDB exists before creating it
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LibraryManagementDB')
BEGIN
    CREATE DATABASE LibraryManagementDB;
    PRINT 'Database LibraryManagementDB created successfully.';
END
GO
-- Switch to LibraryManagementDB
USE LibraryManagementDB;
GO

-- Check if the Author table exists before creating it
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Author')
BEGIN
    CREATE TABLE Author (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- Primary key with GUID
        Name NVARCHAR(255) NOT NULL, -- Author's name
        DateOfBirth DATETIME2(7) NULL, -- Author's birth date (nullable)
        IsDeleted BIT DEFAULT 0 NOT NULL, -- Soft delete flag (0 = Active, 1 = Deleted)
        CreatedDate SMALLDATETIME NOT NULL DEFAULT GETDATE(), -- Record creation date
        ModifiedDate SMALLDATETIME NULL -- Record modification date
    );

    PRINT 'Table Author created successfully.';

    INSERT INTO Author (Id, Name, DateOfBirth, IsDeleted, CreatedDate, ModifiedDate)
    VALUES
    ('C4D01A85-2B47-4B4F-A2E8-000000000001', 'Stephen King', '1947-09-21', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000002', 'J.K. Rowling', '1965-07-31', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000003', 'George R.R. Martin', '1948-09-20', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000004', 'Agatha Christie', '1890-09-15', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000005', 'Isaac Asimov', '1920-01-02', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000006', 'Ernest Hemingway', '1899-07-21', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000007', 'Mark Twain', '1835-11-30', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000008', 'Jane Austen', '1775-12-16', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000009', 'Charles Dickens', '1812-02-07', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000010', 'Leo Tolstoy', '1828-09-09', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000011', 'Fyodor Dostoevsky', '1821-11-11', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000012', 'H.P. Lovecraft', '1890-08-20', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000013', 'J.R.R. Tolkien', '1892-01-03', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000014', 'Arthur Conan Doyle', '1859-05-22', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000015', 'Edgar Allan Poe', '1809-01-19', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000016', 'William Shakespeare', '1564-04-23', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000017', 'Miguel de Cervantes', '1547-09-29', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000018', 'Herman Melville', '1819-08-01', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000019', 'Jules Verne', '1828-02-08', 0, GETDATE(), NULL),
    ('C4D01A85-2B47-4B4F-A2E8-000000000020', 'Victor Hugo', '1802-02-26', 0, GETDATE(), NULL);

    PRINT 'Records have been successfully inserted into the Author table.';
END
GO

-- Check if the Book table exists before creating it
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Book')
BEGIN
    CREATE TABLE Book (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- Primary key with GUID
        Title NVARCHAR(255) NOT NULL, -- Book title
        AuthorId UNIQUEIDENTIFIER NOT NULL, -- Foreign key referencing Author(Id)
        PublishedDate DATE NULL, -- Book publication date (nullable)
        Genere NVARCHAR(100) NULL, -- Book genre (nullable)
        IsDeleted BIT DEFAULT 0 NOT NULL, -- Soft delete flag (0 = Active, 1 = Deleted)
        CreatedDate SMALLDATETIME NOT NULL DEFAULT GETDATE(), -- Record creation date
        ModifiedDate SMALLDATETIME NULL, -- Record last modification date
        CONSTRAINT FK_Book_Author FOREIGN KEY (AuthorId) REFERENCES Author(Id) ON DELETE CASCADE -- Ensure cascading delete
    );

    PRINT 'Table Book created successfully.';

    INSERT INTO Book (Id, Title, AuthorId, PublishedDate, Genere, IsDeleted, CreatedDate, ModifiedDate)
    VALUES
    ('E2C01A85-4D47-4B4F-A4E8-000000000001', 'It', 'C4D01A85-2B47-4B4F-A2E8-000000000001', '1986-09-15', 'Horror', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000002', 'Harry Potter and the Sorcerer''s Stone', 'C4D01A85-2B47-4B4F-A2E8-000000000002', '1997-06-26', 'Fantasy', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000003', 'A Game of Thrones', 'C4D01A85-2B47-4B4F-A2E8-000000000003', '1996-08-06', 'Fantasy', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000004', 'Murder on the Orient Express', 'C4D01A85-2B47-4B4F-A2E8-000000000004', '1934-01-01', 'Mystery', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000005', 'Foundation', 'C4D01A85-2B47-4B4F-A2E8-000000000005', '1951-05-01', 'Science Fiction', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000006', 'The Shining', 'C4D01A85-2B47-4B4F-A2E8-000000000001', '1977-01-28', 'Horror', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000007', 'Harry Potter and the Chamber of Secrets', 'C4D01A85-2B47-4B4F-A2E8-000000000002', '1998-07-02', 'Fantasy', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000008', 'A Clash of Kings', 'C4D01A85-2B47-4B4F-A2E8-000000000003', '1998-11-16', 'Fantasy', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000009', 'The ABC Murders', 'C4D01A85-2B47-4B4F-A2E8-000000000004', '1936-01-06', 'Mystery', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000010', 'I, Robot', 'C4D01A85-2B47-4B4F-A2E8-000000000005', '1950-12-02', 'Science Fiction', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000011', 'The Old Man and the Sea', 'C4D01A85-2B47-4B4F-A2E8-000000000006', '1952-09-01', 'Drama', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000012', 'Pride and Prejudice', 'C4D01A85-2B47-4B4F-A2E8-000000000008', '1813-01-28', 'Romance', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000013', 'A Tale of Two Cities', 'C4D01A85-2B47-4B4F-A2E8-000000000009', '1859-04-30', 'Historical Fiction', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000014', 'Anna Karenina', 'C4D01A85-2B47-4B4F-A2E8-000000000010', '1877-01-01', 'Drama', 0, GETDATE(), NULL),
    ('E2C01A85-4D47-4B4F-A4E8-000000000015', 'The Adventures of Huckleberry Finn', 'C4D01A85-2B47-4B4F-A2E8-000000000007', '1884-12-10', 'Adventure', 0, GETDATE(), NULL);

    PRINT 'Records have been successfully inserted into the Book table.';
END
GO

-- Check if the Category table exists before creating it
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Category')
BEGIN
    CREATE TABLE Category (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), -- Primary key with GUID
        Name NVARCHAR(255) NOT NULL, -- Category name
        IsDeleted BIT DEFAULT 0 NOT NULL, -- Soft delete flag (0 = Active, 1 = Deleted)
        CreatedDate SMALLDATETIME NOT NULL DEFAULT GETDATE(), -- Record creation date
        ModifiedDate SMALLDATETIME NULL -- Record last modification date
    );

    PRINT 'Table Category created successfully.';

    INSERT INTO Category (Id, Name, IsDeleted, CreatedDate, ModifiedDate)
    VALUES
    ('D3B01A85-3C47-4B4F-A3E8-000000000001', 'Horror', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000002', 'Fantasy', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000003', 'Mystery', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000004', 'Science Fiction', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000005', 'Classics', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000006', 'Thriller', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000007', 'Adventure', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000008', 'Drama', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000009', 'Romance', 0, GETDATE(), NULL),
    ('D3B01A85-3C47-4B4F-A3E8-000000000010', 'Historical Fiction', 0, GETDATE(), NULL),
    ('47B1CBB3-403B-4396-AD3C-27C7F3EC077D', 'New-Category-47B1CBB3-403B-4396-AD3C-27C7F3EC077D', 0, GETDATE(), NULL);

    PRINT 'Records have been successfully inserted into the Category table.';
END
GO

-- Check if the BookCategory table exists before creating it (Join Table)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookCategory')
BEGIN
    CREATE TABLE BookCategory (
        BookId UNIQUEIDENTIFIER NOT NULL, -- Foreign key referencing Book(Id)
        CategoryId UNIQUEIDENTIFIER NOT NULL, -- Foreign key referencing Category(Id)
        CreatedDate SMALLDATETIME NOT NULL DEFAULT GETDATE(), -- Record creation date
        PRIMARY KEY (BookId, CategoryId), -- Composite primary key
        CONSTRAINT FK_BookCategory_Book FOREIGN KEY (BookId) REFERENCES Book(Id) ON DELETE CASCADE, -- Ensure cascading delete
        CONSTRAINT FK_BookCategory_Category FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE -- Ensure cascading delete
    );

    PRINT 'Table BookCategory created successfully.';

    INSERT INTO BookCategory (BookId, CategoryId, CreatedDate)
    VALUES
    ('E2C01A85-4D47-4B4F-A4E8-000000000001', 'D3B01A85-3C47-4B4F-A3E8-000000000001', GETDATE()), -- "It" → Horror
    ('E2C01A85-4D47-4B4F-A4E8-000000000002', 'D3B01A85-3C47-4B4F-A3E8-000000000002', GETDATE()), -- "Harry Potter" → Fantasy
    ('E2C01A85-4D47-4B4F-A4E8-000000000003', 'D3B01A85-3C47-4B4F-A3E8-000000000002', GETDATE()), -- "A Game of Thrones" → Fantasy
    ('E2C01A85-4D47-4B4F-A4E8-000000000004', 'D3B01A85-3C47-4B4F-A3E8-000000000003', GETDATE()), -- "Murder on the Orient Express" → Mystery
    ('E2C01A85-4D47-4B4F-A4E8-000000000005', 'D3B01A85-3C47-4B4F-A3E8-000000000004', GETDATE()), -- "Foundation" → Science Fiction
    ('E2C01A85-4D47-4B4F-A4E8-000000000006', 'D3B01A85-3C47-4B4F-A3E8-000000000001', GETDATE()), -- The Shining → Horror
    ('E2C01A85-4D47-4B4F-A4E8-000000000007', 'D3B01A85-3C47-4B4F-A3E8-000000000002', GETDATE()), -- HP Chamber of Secrets → Fantasy
    ('E2C01A85-4D47-4B4F-A4E8-000000000008', 'D3B01A85-3C47-4B4F-A3E8-000000000002', GETDATE()), -- A Clash of Kings → Fantasy
    ('E2C01A85-4D47-4B4F-A4E8-000000000009', 'D3B01A85-3C47-4B4F-A3E8-000000000003', GETDATE()), -- The ABC Murders → Mystery
    ('E2C01A85-4D47-4B4F-A4E8-000000000010', 'D3B01A85-3C47-4B4F-A3E8-000000000004', GETDATE()), -- I, Robot → Science Fiction
    ('E2C01A85-4D47-4B4F-A4E8-000000000011', 'D3B01A85-3C47-4B4F-A3E8-000000000008', GETDATE()), -- The Old Man and the Sea → Drama
    ('E2C01A85-4D47-4B4F-A4E8-000000000012', 'D3B01A85-3C47-4B4F-A3E8-000000000009', GETDATE()), -- Pride and Prejudice → Romance
    ('E2C01A85-4D47-4B4F-A4E8-000000000013', 'D3B01A85-3C47-4B4F-A3E8-000000000010', GETDATE()), -- A Tale of Two Cities → Historical Fiction
    ('E2C01A85-4D47-4B4F-A4E8-000000000014', 'D3B01A85-3C47-4B4F-A3E8-000000000008', GETDATE()), -- Anna Karenina → Drama
    ('E2C01A85-4D47-4B4F-A4E8-000000000015', 'D3B01A85-3C47-4B4F-A3E8-000000000007', GETDATE()); -- Huckleberry Finn → Adventure

    PRINT 'Records have been successfully inserted into the BookCategory table.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'udt_category' AND is_table_type = 1)
BEGIN
    CREATE TYPE udt_category AS TABLE (
        CategoryId UNIQUEIDENTIFIER NOT NULL
    );
END;
GO

CREATE OR ALTER PROCEDURE dbo.usp_get_category_books
	@CategoryId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- First result set: Category details
    SELECT	 Id
			,Name
			,IsDeleted
			,CreatedDate
			,ModifiedDate
    FROM	Category
    WHERE	Id = @CategoryId

    -- Second result set: Books that belong to the category
	SELECT	 B.Id
			,B.Title
			,B.PublishedDate
			,B.Genere
			,B.AuthorId
    FROM	Book B
		INNER JOIN BookCategory BC
			ON B.Id = BC.BookId
	WHERE	BC.CategoryId = @CategoryId
END;
GO

CREATE OR ALTER PROCEDURE usp_get_authors
     @Name NVARCHAR(255) = NULL
    ,@IsDeleted BIT = NULL
    ,@Offset INT
    ,@Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Fetch total count of authors
    DECLARE @TotalRecords INT;

    -- Declare table variable to store paginated authors
    DECLARE @PaginatedAuthors TABLE (
         Id UNIQUEIDENTIFIER PRIMARY KEY
        ,Name NVARCHAR(255)
		,DateOfBirth DATETIME2(7) NULL
        ,IsDeleted BIT
        ,CreatedDate DATETIME
        ,ModifiedDate DATETIME NULL
    );

    -- Get total count of authors that match the filter
    SELECT  @TotalRecords = COUNT(1)
    FROM    Author A
    WHERE   (@Name IS NULL OR A.Name LIKE '%' + @Name + '%')
    AND     (@IsDeleted IS NULL OR A.IsDeleted = @IsDeleted);

    -- Insert paginated authors into table variable
    INSERT INTO @PaginatedAuthors
    SELECT   A.Id
            ,A.Name
			,A.DateOfBirth
            ,A.IsDeleted
            ,A.CreatedDate
            ,A.ModifiedDate
    FROM    Author A
    WHERE   (@Name IS NULL OR A.Name LIKE '%' + @Name + '%')
    AND     (@IsDeleted IS NULL OR A.IsDeleted = @IsDeleted)
    ORDER BY A.Name ASC
    OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

    -- First resultset: Fetch paginated authors
    SELECT    Id
             ,Name
			 ,DateOfBirth
             ,IsDeleted
             ,CreatedDate
             ,ModifiedDate
    FROM     @PaginatedAuthors;

    -- Second resultset: Metadata for pagination
    SELECT    @Offset AS Offset
             ,@Limit AS Limit
             ,@TotalRecords AS TotalRecords;
END;
GO

CREATE OR ALTER PROCEDURE usp_delete_category_book
    @BookId UNIQUEIDENTIFIER,
    @CategoryId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Remove the relationship between the book and the category
    DELETE FROM BookCategory 
    WHERE	BookId = @BookId 
    AND		CategoryId = @CategoryId;
END;
GO

CREATE OR ALTER PROCEDURE usp_get_book
	@BookId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- First resultset: Fetch book details by ID
    SELECT   B.Id
            ,B.Title
            ,A.Id AS AuthorId
            ,A.Name AS AuthorName
            ,B.PublishedDate
            ,B.Genere
            ,B.IsDeleted
            ,B.CreatedDate
            ,B.ModifiedDate
    FROM    Book B
        INNER JOIN Author A
            ON B.AuthorId = A.Id
    WHERE   B.Id = @BookId;

    -- Second resultset: Fetch categories for the book
    IF EXISTS (SELECT 1 FROM Book WHERE Id = @BookId AND IsDeleted = 0)
    BEGIN
        SELECT   C.Id
                ,C.Name
        FROM    Category C
            INNER JOIN BookCategory BC
                ON C.Id = BC.CategoryId
        WHERE   BC.BookId = @BookId;
    END;
END;
GO

CREATE OR ALTER PROCEDURE usp_get_books
     @AuthorName NVARCHAR(255) = NULL
    ,@Title NVARCHAR(255) = NULL
    ,@CategoryName NVARCHAR(255) = NULL
    ,@IsDeleted BIT = NULL
    ,@Offset INT
    ,@Limit INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Fetch total count of books
    DECLARE @TotalRecords INT;

	-- Declare table variable to store paginated books
    DECLARE @PaginatedBooks TABLE (
         Id UNIQUEIDENTIFIER PRIMARY KEY
        ,Title NVARCHAR(255)
        ,AuthorId UNIQUEIDENTIFIER
        ,AuthorName NVARCHAR(255)
        ,PublishedDate DATE
        ,Genere NVARCHAR(100)
        ,IsDeleted BIT
        ,CreatedDate SMALLDATETIME
        ,ModifiedDate SMALLDATETIME NULL
    );

    SELECT  @TotalRecords = COUNT(DISTINCT B.Id)
    FROM    Book B
        INNER JOIN Author A
            ON B.AuthorId = A.Id
        LEFT JOIN BookCategory BC
            ON B.Id = BC.BookId
        LEFT JOIN Category C
            ON BC.CategoryId = C.Id
    WHERE   (@AuthorName IS NULL OR A.Name LIKE '%' + @AuthorName + '%')
    AND     (@Title IS NULL OR B.Title LIKE '%' + @Title + '%')
    AND     (@CategoryName IS NULL OR C.Name LIKE '%' + @CategoryName + '%')
    AND     (@IsDeleted IS NULL OR B.IsDeleted = @IsDeleted);

    -- Insert paginated books into table variable
    INSERT INTO @PaginatedBooks
		SELECT DISTINCT 
			 B.Id
			,B.Title
			,A.Id AS AuthorId
			,A.Name AS AuthorName
			,B.PublishedDate
			,B.Genere
			,B.IsDeleted
			,B.CreatedDate
			,B.ModifiedDate
		FROM    Book B
			INNER JOIN Author A
				ON B.AuthorId = A.Id
			LEFT JOIN BookCategory BC
				ON B.Id = BC.BookId
			LEFT JOIN Category C
				ON BC.CategoryId = C.Id
		WHERE   (@AuthorName IS NULL OR A.Name LIKE '%' + @AuthorName + '%')
		AND     (@Title IS NULL OR B.Title LIKE '%' + @Title + '%')
		AND     (@CategoryName IS NULL OR C.Name LIKE '%' + @CategoryName + '%')
		AND     (@IsDeleted IS NULL OR B.IsDeleted = @IsDeleted)
		ORDER BY B.Title DESC
		OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;

    -- First resultset: Fetch paginated books
    SELECT	 Id
			,Title
			,AuthorId
			,AuthorName
			,PublishedDate
			,Genere
			,IsDeleted
			,CreatedDate
			,ModifiedDate
	FROM	@PaginatedBooks;

    -- Second resultset: Fetch categories for paginated books
    SELECT   PB.Id AS BookId
            ,C.Id AS CategoryId
            ,C.Name AS CategoryName
    FROM    @PaginatedBooks PB
        LEFT JOIN BookCategory BC
            ON PB.Id = BC.BookId
        LEFT JOIN Category C
            ON BC.CategoryId = C.Id;

    -- Third resultset: Metadata for pagination
    SELECT   @Offset AS Offset
            ,@Limit AS Limit
            ,@TotalRecords AS TotalRecords;
END;
GO

CREATE OR ALTER PROCEDURE usp_get_books_by_author
    @AuthorId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- First resultset: Fetch books details by Author ID
    SELECT   B.Id
            ,B.Title
            ,A.Id AS AuthorId
            ,A.Name AS AuthorName
            ,B.PublishedDate
            ,B.Genere
            ,B.IsDeleted
            ,B.CreatedDate
            ,B.ModifiedDate
    FROM    Book B
        INNER JOIN Author A
            ON B.AuthorId = A.Id
    WHERE   B.AuthorId = @AuthorId
    AND     B.IsDeleted = 0;

    -- Second resultset: Fetch categories for books by Author ID
    IF EXISTS (SELECT 1 FROM Book WHERE AuthorId = @AuthorId)
    BEGIN
        SELECT   BC.BookId
                ,C.Id AS CategoryId
                ,C.Name AS CategoryName
        FROM    Category C
            INNER JOIN BookCategory BC
                ON C.Id = BC.CategoryId
            INNER JOIN Book B
                ON BC.BookId = B.Id
        WHERE   B.AuthorId = @AuthorId
        AND     B.IsDeleted = 0;
    END;
END;
GO

CREATE OR ALTER PROCEDURE usp_post_book
     @Id UNIQUEIDENTIFIER
    ,@Title NVARCHAR(255)
    ,@AuthorId UNIQUEIDENTIFIER
    ,@PublishedDate DATE
    ,@Genere NVARCHAR(255)
    ,@Categories udt_category READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert the new book
    INSERT INTO Book (Id, Title, AuthorId, PublishedDate, Genere, CreatedDate)
        VALUES (@Id, @Title, @AuthorId, @PublishedDate, @Genere, GETDATE());

    -- Insert categories for the book
    INSERT INTO BookCategory (BookId, CategoryId)
        SELECT   @Id
                ,CategoryId
        FROM    @Categories
		WHERE   EXISTS (SELECT 1 FROM @Categories);

    -- Return the inserted book ID
    SELECT @Id AS BookId;
END;
GO

CREATE OR ALTER PROCEDURE usp_post_category_book
	 @BookId UNIQUEIDENTIFIER
    ,@CategoryId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the relationship already exists
    IF NOT EXISTS (
        SELECT	1
        FROM	BookCategory 
        WHERE	BookId = @BookId
		AND		CategoryId = @CategoryId
    )
	BEGIN
        -- Insert the association between the book and the category
        INSERT INTO BookCategory (BookId, CategoryId)
        VALUES (@BookId, @CategoryId);
    END

    -- Return the name of the associated category
    SELECT	Name 
    FROM	Category 
    WHERE	Id = @CategoryId;
END;
GO