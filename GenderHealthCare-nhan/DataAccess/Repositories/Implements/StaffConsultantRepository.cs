using DataAccess.DBContext;
using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implements
{
    public class StaffConsultantRepository : IStaffConsultantRepository
    {
        private readonly AppDbContext _context;

        public StaffConsultantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existedUser = await GetByIdAsync(id);

            if (existedUser == null)       
                return false;

            existedUser.IsDeleted = true;
            _context.Users.Update(existedUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id && !u.IsDeleted);
        }

        public async Task<User?> UpdateAsync(Guid id, User dto)
        {
            var existedUser = await _context.Users.FindAsync(id);

            if(existedUser == null)
                return null;
            else
            {
                existedUser.Username = dto.Username;
                existedUser.FullName = dto.FullName;
                existedUser.Password = dto.Password;
                existedUser.Gender = dto.Gender;
                existedUser.Email = dto.Email;
                existedUser.PhoneNumber = dto.PhoneNumber;
                existedUser.Address = dto.Address;
                existedUser.Dob = dto.Dob;
                existedUser.Role = dto.Role;

                _context.Users.Update(existedUser);
                await _context.SaveChangesAsync();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

                return user;
            }

        }
    }
}
