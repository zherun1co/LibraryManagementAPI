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