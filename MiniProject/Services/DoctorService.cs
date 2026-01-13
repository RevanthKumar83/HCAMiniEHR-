using Microsoft.EntityFrameworkCore;
using MiniProject.Data;
using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext _context;

        public DoctorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<int> CreateAsync(Doctor doctor)
        {
            var nameParam = new Microsoft.Data.SqlClient.SqlParameter("@Name", doctor.Name);
            var specializationParam = new Microsoft.Data.SqlClient.SqlParameter("@Specialization", (object?)doctor.Specialization ?? System.DBNull.Value);

            var newIdParam = new Microsoft.Data.SqlClient.SqlParameter("@NewId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [Healthcare].[sp_CreateDoctor] @Name, @Specialization, @NewId OUTPUT",
                nameParam, specializationParam, newIdParam);

            doctor.Id = (int)newIdParam.Value;
            return doctor.Id;
        }
    }
}
