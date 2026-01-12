using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class DoctorAppointmentStats
    {
        public string DoctorName { get; set; } = string.Empty;
        public int AppointmentCount { get; set; }
    }

    public interface IReportService
    {
        Task<List<LabOrder>> GetPendingLabOrdersAsync();
        Task<List<Patient>> GetPatientsWithoutFollowUpAsync(); // Patients with no appointments > Today
        Task<List<DoctorAppointmentStats>> GetDoctorAppointmentStatsAsync();
    }
}
