USE HealthcareDb;
GO

-- Trigger to prevent deletion from AuditLog table
CREATE OR ALTER TRIGGER [Healthcare].[trg_PreventAuditLogDelete]
ON [Healthcare].[AuditLog]
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    RAISERROR('Deletions from AuditLog are not allowed! This table is immutable.', 16, 1);
    ROLLBACK TRANSACTION;
END
GO
