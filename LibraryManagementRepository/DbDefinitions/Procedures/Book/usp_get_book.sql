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