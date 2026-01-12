using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniProject.Models;
using MiniProject.Services;
using System.Threading.Tasks;

namespace MiniProject.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly IPatientService _service;

        public EditModel(IPatientService service)
        {
            _service = service;
        }

        [BindProperty]
        public Patient Patient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _service.GetByIdAsync(id.Value);
            if (patient == null)
            {
                return NotFound();
            }
            Patient = patient;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _service.UpdateAsync(Patient);

            return RedirectToPage("./Index");
        }
    }
}
