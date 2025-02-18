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