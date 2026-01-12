using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public interface ILabOrderService
    {
        Task<List<LabOrder>> GetAllAsync();
        Task<LabOrder?> GetByIdAsync(int id);
        Task CreateAsync(LabOrder labOrder);
        Task UpdateAsync(LabOrder labOrder);
        Task DeleteAsync(int id);
        Task<List<LabOrder>> GetByAppointmentIdAsync(int appointmentId);
    }
}
