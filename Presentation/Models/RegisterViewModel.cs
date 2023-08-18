using Entities.Enums;

namespace Presentation.Models
{
    public class RegisterViewModel
    {
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public string? Mail { get; set; }
        public string? Password { get; set; }
        public GenderType? GenderType { get; set; }
        public int? DepartmentId { get; set; }
        public int? JobId { get; set; }
        public string? NationalityId { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
        
    }
}
