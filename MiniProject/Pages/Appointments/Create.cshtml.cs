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
        private readonly IDoctorService _doctorService;

        public CreateModel(IAppointmentService apptService, IPatientService patientService, IDoctorService doctorService)
        {
            _apptService = apptService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        public SelectList PatientList { get; set; } = default!;
        public SelectList DoctorList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var patients = await _patientService.GetAllAsync();
            PatientList = new SelectList(patients, "Id", "Name");

            var doctors = await _doctorService.GetAllAsync();
            DoctorList = new SelectList(doctors, "Id", "Name");

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

                var doctors = await _doctorService.GetAllAsync();
                DoctorList = new SelectList(doctors, "Id", "Name");

                return Page();
            }

            // Check for double booking
            if (!await _apptService.IsDoctorAvailableAsync(Appointment.DoctorId, Appointment.AppointmentDate))
            {
                ModelState.AddModelError("Appointment.AppointmentDate", "This doctor is already booked for this time slot.");
                
                var patients = await _patientService.GetAllAsync();
                PatientList = new SelectList(patients, "Id", "Name");

                var doctors = await _doctorService.GetAllAsync();
                DoctorList = new SelectList(doctors, "Id", "Name");

                return Page();
            }

            // Calls the Service which calls the SP
            await _apptService.CreateAsync(Appointment);

            return RedirectToPage("./Index");
        }
    }
}
