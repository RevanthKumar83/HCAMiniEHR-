using Microsoft.EntityFrameworkCore;
using MiniProject.Data;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Report 1: Pending Lab Orders (Where, OrderBy)
        public async Task<List<LabOrder>> GetPendingLabOrdersAsync()
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .Where(l => l.Status == "Pending")
                .OrderBy(l => l.OrderDate)
                .ToListAsync();
        }

        // Report 2: Patients Without Follow-Up (Group Join / Select / Where)
        // Def: Patients who do NOT have any appointment with Date > Now
        public async Task<List<Patient>> GetPatientsWithoutFollowUpAsync()
        {
            var today = DateTime.Now;
            
            // Method 1: LINQ with subquery
            /*
            return await _context.Patients
                .Where(p => !p.Appointments.Any(a => a.AppointmentDate > today))
                .ToListAsync();
            */
            
            // Method 2: Left Join logic (more "LINQ-y" for demonstration)
            // But EF Core handles navigation properties best.
            // Requirement says "Where, Select, GroupBy, Join, OrderBy"
            // Let's use a Join or GroupJoin approach?
            // "Patients who have no future appointments"
            
            var query = from p in _context.Patients
                        join a in _context.Appointments.Where(x => x.AppointmentDate > today)
                        on p.Id equals a.PatientId into futureApps
                        from subApp in futureApps.DefaultIfEmpty()
                        where subApp == null // No future appointment
                        select p;
            
            // Distinct in case validation needs it, though Patient join null should be unique if logic is correct
            return await query.Distinct().ToListAsync();
        }

        // Report 3: Doctor Productivity (GroupBy, Select, OrderBy)
        public async Task<List<DoctorAppointmentStats>> GetDoctorAppointmentStatsAsync()
        {
            // Group by Doctor.Name
            return await _context.Appointments
                .Include(a => a.Doctor)
                .GroupBy(a => a.Doctor != null ? a.Doctor.Name : "Unknown")
                .Select(g => new DoctorAppointmentStats
                {
                    DoctorName = g.Key,
                    AppointmentCount = g.Count()
                })
                .OrderByDescending(x => x.AppointmentCount)
                .ToListAsync();
        }
    }
}
