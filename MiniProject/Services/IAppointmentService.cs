using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<int> CreateAsync(Appointment appointment); // Returns new ID
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
        Task<List<Appointment>> GetByPatientIdAsync(int patientId);
    }
}
