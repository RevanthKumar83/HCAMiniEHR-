USE HealthcareDb;
GO

-- 1. Assign a default doctor (e.g., the first one) to any appointment with NULL DoctorId
-- This prevents "Unable to cast object of type 'System.DBNull' to type 'System.Int32'" errors.
UPDATE [Healthcare].[Appointment]
SET DoctorId = (SELECT TOP 1 Id FROM [Healthcare].[Doctor])
WHERE DoctorId IS NULL;
GO

-- 2. Once all rows have a value, make the column NOT NULL to match C# model
ALTER TABLE [Healthcare].[Appointment]  
ALTER COLUMN DoctorId INT NOT NULL;
GO
