# HCAMiniEHR

## Overview
HCAMiniEHR is a mini Electronic Health Record (EHR) system developed as a capstone project. It manages Patients, Appointments, and Lab Orders with a focus on EF Core, SQL Server integration (Triggers/Stored Procedures), and LINQ reporting.

## Tech Stack
- **Framework**: .NET 8 (ASP.NET Core Razor Pages)
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Tools**: Git, Visual Studio Code

## Features
1.  **Patient Management**: Create, Read, Update, Delete patients.
2.  **Appointment Management**: Schedule appointments using a SQL Stored Procedure (`sp_CreateAppointment`).
3.  **Lab Order Management**: Create lab orders linked to appointments.
4.  **Reporting Dashboard**:
    - Pending Lab Orders.
    - Patients without follow-up appointments.
    - Doctor productivity stats.
5.  **Audit Logging**: Database trigger (`trg_AppointmentAudit`) logs all changes to the Appointment table.

## Design Decisions
- **Layered Architecture**:
    - **Models**: EF Core entities mapping to `Healthcare` schema.
    - **Data**: `ApplicationDbContext` for DB access.
    - **Services**: Business logic including SP calls and LINQ queries.
    - **Pages**: UI layer using Razor Pages.
- **SQL Integration**:
    - Used Stored Procedure for appointment creation to demonstrate hybrid EF/SQL approach.
    - Used Trigger for auditing to ensure data integrity at the database level.
- **Dependency Injection**: All services are registered with Scoped lifetime.

## Setup Instructions
1.  **Database**:
    - Run scripts in `SQL/` folder in SSMS in order:
        1.  `01_InitDatabase.sql`
        2.  `02_CreateTables.sql`
        3.  `03_CreateTrigger.sql`
        4.  `04_CreateStoredProcedure.sql`
    - Alternatively, update `appsettings.json` and run `dotnet ef database update`.
2.  **Run Application**:
    ```bash
    dotnet run
    ```
3.  **Navigate**: Open browser to `http://localhost:5xxx`.

## Git Workflow
- Developed using Feature Branch workflow.
- Pull Requests used for merging major modules.
