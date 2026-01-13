USE HealthcareDb;
GO

-- 1. Insert a test appointment directly (Simulate normal insert)
INSERT INTO [Healthcare].[Appointment] (PatientId, AppointmentDate, Reason, Status, DoctorId)
VALUES ((SELECT TOP 1 Id FROM [Healthcare].[Patient]), DATEADD(day, 1, GETDATE()), 'Trigger Test Direct', 'Scheduled', (SELECT TOP 1 Id FROM [Healthcare].[Doctor]));
GO

-- 2. Insert via Stored Procedure (Simulate App)
DECLARE @NewId INT;
EXEC [Healthcare].[sp_CreateAppointment] 
    @PatientId = 1, -- Assuming ID 1 exists, or use subquery in real app but param must be int
    @AppointmentDate = '2026-01-20',
    @Reason = 'Trigger Test SP',
    @DoctorId = 1, -- Assuming ID 1 exists
    @NewId = @NewId OUTPUT;
GO

-- 3. Check Audit Log
SELECT TOP 5 * FROM [Healthcare].[AuditLog] ORDER BY LogDate DESC;
GO
