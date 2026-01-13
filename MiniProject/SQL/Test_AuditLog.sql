USE HealthcareDb;
GO

-- Check if Trigger exists
SELECT name AS TriggerName FROM sys.triggers WHERE name = 'trg_AppointmentAudit';
GO

-- Check recent Audit Logs
SELECT TOP 5 LogId, TableName, Action, Details, LogDate 
FROM [Healthcare].[AuditLog]
ORDER BY LogDate DESC;
GO
