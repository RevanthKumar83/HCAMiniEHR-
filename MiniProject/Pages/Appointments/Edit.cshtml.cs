using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniProject.Models;
using MiniProject.Services;
using System.Threading.Tasks;

namespace MiniProject.Pages.Appointments
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _apptService;
        private readonly IPatientService _patientService;

        public EditModel(IAppointmentService apptService, IPatientService patientService)
        {
            _apptService = apptService;
            _patientService = patientService;
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public SelectList PatientList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var appt = await _apptService.GetByIdAsync(id.Value);
            if (appt == null) return NotFound();
            Appointment = appt;

            var patients = await _patientService.GetAllAsync();
            PatientList = new SelectList(patients, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllAsync();
                PatientList = new SelectList(patients, "Id", "Name");
                return Page();
            }

            await _apptService.UpdateAsync(Appointment);

            return RedirectToPage("./Index");
        }
    }
}
