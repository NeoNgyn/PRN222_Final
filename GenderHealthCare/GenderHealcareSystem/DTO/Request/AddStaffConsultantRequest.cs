using DataAccess.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GenderHealcareSystem.DTO.Request
{
    public class AddStaffConsultantRequest
    {
        public Guid UserId { get; set; }

        public Guid? RoleId { get; set; }

        [MaxLength(20)]
        public string Username { get; set; }

        public string? FullName { get; set; }
        public string Password { get; set; }

        public bool? Gender { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(10)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }

        public DateOnly? Dob { get; set; }

        public bool IsDeleted { get; set; }

        public Role Role { get; set; }
    }
}
