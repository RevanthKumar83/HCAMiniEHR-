using Microsoft.EntityFrameworkCore;
using MiniProject.Data;
using MiniProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class LabOrderService : ILabOrderService
    {
        private readonly ApplicationDbContext _context;

        public LabOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LabOrder>> GetAllAsync()
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync();
        }

        public async Task<LabOrder?> GetByIdAsync(int id)
        {
            return await _context.LabOrders
                .Include(l => l.Appointment)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task CreateAsync(LabOrder labOrder)
        {
            _context.LabOrders.Add(labOrder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LabOrder labOrder)
        {
            _context.LabOrders.Update(labOrder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
             var order = await _context.LabOrders.FindAsync(id);
             if (order != null)
             {
                 _context.LabOrders.Remove(order);
                 await _context.SaveChangesAsync();
             }
        }

        public async Task<List<LabOrder>> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.LabOrders
                .Where(l => l.AppointmentId == appointmentId)
                .ToListAsync();
        }
    }
}
