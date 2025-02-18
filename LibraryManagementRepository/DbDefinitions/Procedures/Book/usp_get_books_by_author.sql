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