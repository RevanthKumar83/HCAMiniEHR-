using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniProject.Models;
using MiniProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;

        public IndexModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IList<LabOrder> PendingLabOrders { get; set; } = new List<LabOrder>();
        public IList<Patient> PatientsNoFollowUp { get; set; } = new List<Patient>();
        public IList<DoctorAppointmentStats> DoctorStats { get; set; } = new List<DoctorAppointmentStats>();

        public async Task OnGetAsync()
        {
            PendingLabOrders = await _reportService.GetPendingLabOrdersAsync();
            PatientsNoFollowUp = await _reportService.GetPatientsWithoutFollowUpAsync();
            DoctorStats = await _reportService.GetDoctorAppointmentStatsAsync();
        }
    }
}
