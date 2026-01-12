-- Create Database (if not exists)
-- NOTE: In a real deployment, the DB might be created by the platform.
-- IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EHR')
-- BEGIN
--     CREATE DATABASE EHR;
-- END
-- GO

USE EHR;
GO

-- Create Schema
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Healthcare')
BEGIN
    EXEC('CREATE SCHEMA [Healthcare]');
END
GO
