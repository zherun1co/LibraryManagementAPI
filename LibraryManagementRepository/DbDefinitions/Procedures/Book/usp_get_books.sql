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