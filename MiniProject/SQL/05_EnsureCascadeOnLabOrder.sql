USE [HealthcareDb];
GO

DECLARE @fkname sysname;

SELECT @fkname = fk.name
FROM sys.foreign_keys fk
JOIN sys.objects o ON fk.parent_object_id = o.object_id
WHERE o.name = 'LabOrder'
  AND fk.referenced_object_id = OBJECT_ID('Healthcare.Appointment');

IF @fkname IS NOT NULL
BEGIN
    IF @fkname <> 'FK_LabOrder_Appointment'
    BEGIN
        EXEC('ALTER TABLE [Healthcare].[LabOrder] DROP CONSTRAINT [' + @fkname + ']');
    END
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys fk
    WHERE fk.parent_object_id = OBJECT_ID('Healthcare.LabOrder')
      AND fk.referenced_object_id = OBJECT_ID('Healthcare.Appointment')
      AND fk.name = 'FK_LabOrder_Appointment'
)
BEGIN
    ALTER TABLE [Healthcare].[LabOrder]
    ADD CONSTRAINT FK_LabOrder_Appointment FOREIGN KEY (AppointmentId)
    REFERENCES [Healthcare].[Appointment](Id)
    ON DELETE CASCADE;
END
GO

-- Verification:
SELECT fk.name AS ForeignKeyName, OBJECT_NAME(fk.parent_object_id) AS ParentTable
FROM sys.foreign_keys fk
WHERE fk.parent_object_id = OBJECT_ID('Healthcare.LabOrder');
GO