USE EHR;
GO

-- Stored Procedure: sp_CreateAppointment
-- Creates an appointment from C#
CREATE OR ALTER PROCEDURE [Healthcare].[sp_CreateAppointment]
    @PatientId INT,
    @AppointmentDate DATETIME2,
    @Reason NVARCHAR(255),
    @DoctorName NVARCHAR(100),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Healthcare].[Appointment] (PatientId, AppointmentDate, Reason, Status, DoctorName)
    VALUES (@PatientId, @AppointmentDate, @Reason, 'Scheduled', @DoctorName);

    SET @NewId = SCOPE_IDENTITY();
END
GO
