USE HealthcareDb;
GO

-- Trigger: trg_AppointmentAudit
-- Logs INSERT, UPDATE, DELETE on Appointment table
CREATE OR ALTER TRIGGER [Healthcare].[trg_AppointmentAudit]
ON [Healthcare].[Appointment]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Action NVARCHAR(50);
    
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        IF EXISTS (SELECT * FROM deleted)
            SET @Action = 'UPDATE';
        ELSE
            SET @Action = 'INSERT';
    END
    ELSE
        SET @Action = 'DELETE';

    -- Log INSERTs
    IF @Action = 'INSERT'
    BEGIN
        INSERT INTO [Healthcare].[AuditLog] (TableName, RecordId, Action, Details, LogDate)
        SELECT 
            'Appointment', 
            i.Id, 
            'INSERT', 
            CONCAT('New Appointment for PatientId: ', i.PatientId, ', Date: ', i.AppointmentDate),
            GETDATE()
        FROM inserted i;
    END

    -- Log UPDATEs
    IF @Action = 'UPDATE'
    BEGIN
        INSERT INTO [Healthcare].[AuditLog] (TableName, RecordId, Action, Details, LogDate)
        SELECT 
            'Appointment', 
            i.Id, 
            'UPDATE', 
            CONCAT('Status changed from ', d.Status, ' to ', i.Status),
            GETDATE()
        FROM inserted i
        JOIN deleted d ON i.Id = d.Id;
    END

    -- Log DELETEs
    IF @Action = 'DELETE'
    BEGIN
        INSERT INTO [Healthcare].[AuditLog] (TableName, RecordId, Action, Details, LogDate)
        SELECT 
            'Appointment', 
            d.Id, 
            'DELETE', 
            CONCAT('Deleted Appointment for PatientId: ', d.PatientId),
            GETDATE()
        FROM deleted d;
    END
END
GO
