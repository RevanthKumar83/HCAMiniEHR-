using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniProject.Models;
using MiniProject.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Pages.LabOrders
{
    public class IndexModel : PageModel
    {
        private readonly ILabOrderService _service;

        public IndexModel(ILabOrderService service)
        {
            _service = service;
        }

        public IList<LabOrder> LabOrders { get; set; } = default!;

        public async Task OnGetAsync()
        {
            LabOrders = await _service.GetAllAsync();
        }
    }
}
