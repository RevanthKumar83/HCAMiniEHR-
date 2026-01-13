USE HealthcareDb;
GO

-- Create Stored Procedure for Creating Patient
CREATE PROCEDURE [Healthcare].[sp_CreatePatient]
    @Name NVARCHAR(100),
    @DateOfBirth DATETIME2,
    @Gender NVARCHAR(10),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(10),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Healthcare].[Patient] (Name, DateOfBirth, Gender, Email, Phone)
    VALUES (@Name, @DateOfBirth, @Gender, @Email, @Phone);

    SET @NewId = SCOPE_IDENTITY();
END;
GO

-- Create Stored Procedure for Creating Doctor
CREATE PROCEDURE [Healthcare].[sp_CreateDoctor]
    @Name NVARCHAR(100),
    @Specialization NVARCHAR(100),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Healthcare].[Doctor] (Name, Specialization)
    VALUES (@Name, @Specialization);

    SET @NewId = SCOPE_IDENTITY();
END;
GO
