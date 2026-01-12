using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniProject.Models;
using MiniProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _apptService;
        private readonly IPatientService _patientService;

        public CreateModel(IAppointmentService apptService, IPatientService patientService)
        {
            _apptService = apptService;
            _patientService = patientService;
        }

        public SelectList PatientList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var patients = await _patientService.GetAllAsync();
            PatientList = new SelectList(patients, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Appointment Appointment { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var patients = await _patientService.GetAllAsync();
                PatientList = new SelectList(patients, "Id", "Name");
                return Page();
            }

            // Calls the Service which calls the SP
            await _apptService.CreateAsync(Appointment);

            return RedirectToPage("./Index");
        }
    }
}
