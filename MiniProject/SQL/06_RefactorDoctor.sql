USE HealthcareDb;
GO

-- 1. Create Doctor Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Healthcare].[Doctor]') AND type in (N'U'))
BEGIN
    CREATE TABLE [Healthcare].[Doctor](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Specialization] [nvarchar](100) NULL,
        CONSTRAINT [PK_Doctor] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- 2. Seed Data
IF NOT EXISTS (SELECT * FROM [Healthcare].[Doctor])
BEGIN
    INSERT INTO [Healthcare].[Doctor] (Name, Specialization) VALUES 
    ('Dr. Alice Smith', 'Cardiology'),
    ('Dr. Bob Jones', 'Pediatrics'),
    ('Dr. Carol White', 'Dermatology'),
    ('Dr. David Brown', 'General Practice');
END
GO

-- 3. Modify Appointment Table
-- Add DoctorId column
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DoctorId' AND Object_ID = Object_ID(N'[Healthcare].[Appointment]'))
BEGIN
    ALTER TABLE [Healthcare].[Appointment] ADD [DoctorId] INT NULL;
    
    -- Add Foreign Key
    ALTER TABLE [Healthcare].[Appointment] WITH CHECK ADD CONSTRAINT [FK_Appointment_Doctor] FOREIGN KEY([DoctorId])
    REFERENCES [Healthcare].[Doctor] ([Id])
    
    ALTER TABLE [Healthcare].[Appointment] CHECK CONSTRAINT [FK_Appointment_Doctor]
END
GO

-- 4. Update existing records (Optional, if we want to keep data, but DoctorName was string)
-- We might loose data if names don't match exactly. For this mini-project, we'll accept starting fresh or NULLs.

-- Drop DoctorName column
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'DoctorName' AND Object_ID = Object_ID(N'[Healthcare].[Appointment]'))
BEGIN
    ALTER TABLE [Healthcare].[Appointment] DROP COLUMN [DoctorName];
END
GO

-- 5. Update Stored Procedure
CREATE OR ALTER PROCEDURE [Healthcare].[sp_CreateAppointment]
    @PatientId INT,
    @AppointmentDate DATETIME2,
    @Reason NVARCHAR(255),
    @DoctorId INT,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Healthcare].[Appointment] (PatientId, AppointmentDate, Reason, Status, DoctorId)
    VALUES (@PatientId, @AppointmentDate, @Reason, 'Scheduled', @DoctorId);

    SET @NewId = SCOPE_IDENTITY();
END
GO
