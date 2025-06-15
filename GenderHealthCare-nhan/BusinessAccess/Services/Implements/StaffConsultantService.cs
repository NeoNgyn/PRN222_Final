using BusinessAccess.Services.Interfaces;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessAccess.Services.Implements
{
    public class StaffConsultantService : IStaffConsultantService
    {
        private readonly IStaffConsultantRepository _repository;

        public StaffConsultantService(IStaffConsultantRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> UpdateAsync(Guid id, User dto)
        {
            return await _repository.UpdateAsync(id, dto);
        }
    }
}
