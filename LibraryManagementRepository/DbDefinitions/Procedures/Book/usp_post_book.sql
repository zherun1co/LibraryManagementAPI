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