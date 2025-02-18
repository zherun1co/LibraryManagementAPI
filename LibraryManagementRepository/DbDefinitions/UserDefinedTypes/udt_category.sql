-- Create the User-Defined Table Type for categories
CREATE TYPE udt_category AS TABLE (
	CategoryId UNIQUEIDENTIFIER NOT NULL
);
GO