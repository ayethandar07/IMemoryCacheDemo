-- Set the random seed for reproducibility (optional)
DECLARE @Seed INT = 1;

-- Loop to insert 10,000 rows
DECLARE @Counter INT = 1;

WHILE @Counter <= 10000
BEGIN
    INSERT INTO [dbo].[Employee] (FirstName, LastName, Email, Phone, HireDate, Salary, DepartmentID, IsActive, CreatedAt, UpdatedAt)
    VALUES (
        CONCAT('FirstName', @Counter),                   -- FirstName
        CONCAT('LastName', @Counter),                    -- LastName
        CONCAT('employee', @Counter, '@example.com'),    -- Email
        CONCAT('123-456-789', CAST((@Counter % 10) AS VARCHAR(1))),  -- Phone (simple format)
        DATEADD(DAY, -(@Counter % 365), GETDATE()),      -- HireDate (random within the last year)
        ROUND(RAND(@Seed + @Counter) * 100000, 2),       -- Salary (random salary)
        CAST((@Counter % 5) + 1 AS INT),                  -- DepartmentID (random from 1 to 5)
        CASE WHEN @Counter % 2 = 0 THEN 1 ELSE 0 END,     -- IsActive (alternates between 1 and 0)
        GETDATE(),                                       -- CreatedAt
        GETDATE()                                        -- UpdatedAt
    );

    SET @Counter = @Counter + 1;
END
