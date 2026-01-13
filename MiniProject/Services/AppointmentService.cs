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
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> CreateAsync(Appointment appointment)
        {
            // Build strongly-typed SqlParameters (explicit types and sizes)
            var patientIdParam = new SqlParameter("@PatientId", SqlDbType.Int)
            {
                Value = appointment.PatientId
            };

            var dateParam = new SqlParameter("@AppointmentDate", SqlDbType.DateTime2)
            {
                Value = appointment.AppointmentDate
            };

            var reasonParam = new SqlParameter("@Reason", SqlDbType.NVarChar, 255)
            {
                Value = (object?)appointment.Reason ?? DBNull.Value
            };

            var doctorIdParam = new SqlParameter("@DoctorId", SqlDbType.Int)
            {
                Value = appointment.DoctorId
            };

            var newIdParam = new SqlParameter("@NewId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            // Use explicit named parameters in the EXEC string and mark only @NewId as OUTPUT
            var sql = "EXEC [Healthcare].[sp_CreateAppointment] " +
                      "@PatientId = @PatientId, " +
                      "@AppointmentDate = @AppointmentDate, " +
                      "@Reason = @Reason, " +
                      "@DoctorId = @DoctorId, " +
                      "@NewId = @NewId OUTPUT";

            await _context.Database.ExecuteSqlRawAsync(sql,
                patientIdParam, dateParam, reasonParam, doctorIdParam, newIdParam);

            return (int)(newIdParam.Value ?? 0);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Load appointment with dependent LabOrders, then delete dependents first.
            var appointment = await _context.Appointments
                .Include(a => a.LabOrders)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return;
            }

            // Use transaction to ensure consistency
            await using var tx = await _context.Database.BeginTransactionAsync();
            try
            {
                if (appointment.LabOrders != null && appointment.LabOrders.Any())
                {
                    _context.LabOrders.RemoveRange(appointment.LabOrders);
                }

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
             return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<bool> IsDoctorAvailableAsync(int doctorId, DateTime appointmentDate)
        {
            // Returns true if NO appointment exists for this doctor at the exact same time
            return !await _context.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.AppointmentDate == appointmentDate);
        }
    }
}
