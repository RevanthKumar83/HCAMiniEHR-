using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetAllAsync();
        Task<int> CreateAsync(Doctor doctor);
    }
}
