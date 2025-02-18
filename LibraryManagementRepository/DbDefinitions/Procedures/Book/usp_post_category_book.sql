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