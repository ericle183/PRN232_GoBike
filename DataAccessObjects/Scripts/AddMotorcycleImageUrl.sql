IF COL_LENGTH('dbo.Motorcycles', 'ImageUrl') IS NULL
BEGIN
    ALTER TABLE dbo.Motorcycles
    ADD ImageUrl nvarchar(500) NULL;
END;
GO
