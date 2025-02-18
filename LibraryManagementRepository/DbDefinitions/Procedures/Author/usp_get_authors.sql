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