USE HealthcareDb;
GO

-- 1. Drop the existing foreign key constraint (which has ON DELETE CASCADE)
-- We assume the name is [FK_Appointment_Patient] based on 02_CreateTables.sql
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Healthcare].[FK_Appointment_Patient]') AND parent_object_id = OBJECT_ID(N'[Healthcare].[Appointment]'))
BEGIN
    ALTER TABLE [Healthcare].[Appointment] DROP CONSTRAINT [FK_Appointment_Patient];
END
GO

-- 2. Re-create the foreign key with ON DELETE NO ACTION (default)
-- This ensures that if you try to delete a Patient who is referenced by an Appointment, the delete will fail.
ALTER TABLE [Healthcare].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Patient] FOREIGN KEY([PatientId])
REFERENCES [Healthcare].[Patient] ([Id])
ON DELETE NO ACTION;
GO

ALTER TABLE [Healthcare].[Appointment] CHECK CONSTRAINT [FK_Appointment_Patient];
GO
