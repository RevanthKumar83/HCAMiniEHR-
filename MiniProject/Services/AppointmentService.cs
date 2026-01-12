using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniProject.Data;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> CreateAsync(Appointment appointment)
        {
            // Requirement 6: Primary SQL Stored Procedure invoked from C# (e.g., CreateAppointment SP)
            var patientIdParam = new SqlParameter("@PatientId", appointment.PatientId);
            var dateParam = new SqlParameter("@AppointmentDate", appointment.AppointmentDate);
            var reasonParam = new SqlParameter("@Reason", appointment.Reason ?? (object)DBNull.Value);
            var doctorParam = new SqlParameter("@DoctorName", appointment.DoctorName ?? (object)DBNull.Value);
            var newIdParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [Healthcare].[sp_CreateAppointment] @PatientId, @AppointmentDate, @Reason, @DoctorName, @NewId OUT",
                patientIdParam, dateParam, reasonParam, doctorParam, newIdParam);

            return (int)newIdParam.Value;
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
             return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
