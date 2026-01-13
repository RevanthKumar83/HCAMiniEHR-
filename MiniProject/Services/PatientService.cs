using Microsoft.EntityFrameworkCore;
using MiniProject.Data;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniProject.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task InitialCreateAsync(Patient patient)
        {
            var nameParam = new Microsoft.Data.SqlClient.SqlParameter("@Name", patient.Name);
            var dobParam = new Microsoft.Data.SqlClient.SqlParameter("@DateOfBirth", patient.DateOfBirth);
            var genderParam = new Microsoft.Data.SqlClient.SqlParameter("@Gender", patient.Gender);
            var emailParam = new Microsoft.Data.SqlClient.SqlParameter("@Email", patient.Email);
            var phoneParam = new Microsoft.Data.SqlClient.SqlParameter("@Phone", patient.Phone);
            
            var newIdParam = new Microsoft.Data.SqlClient.SqlParameter("@NewId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC [Healthcare].[sp_CreatePatient] @Name, @DateOfBirth, @Gender, @Email, @Phone, @NewId OUTPUT",
                nameParam, dobParam, genderParam, emailParam, phoneParam, newIdParam);

            // Set the ID back to the object so the UI can use it if needed
            patient.Id = (int)newIdParam.Value;
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
