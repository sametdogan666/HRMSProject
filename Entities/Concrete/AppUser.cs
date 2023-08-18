using System.ComponentModel.DataAnnotations.Schema;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete;

public class AppUser : IdentityUser<int>
{

    public string? FullName { get; set; }
    public bool Status { get; set; } = true;
    public DateTime CreatedTime { get; set; }
    public int? DepartmentId { get; set; }
    public virtual Department? Department { get; set; }
    public int? JobId { get; set; }
    public Job? Job { get; set; }
    public string? NationalityId { get; set; }
    public GenderType? GenderType { get; set; }
    public decimal? Salary { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public DateTime? DateOfLeaving { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }
    public string? ImageUrl { get; set; }
    public string? EmployeeCode { get; set; }

    public List<Project>? Projects { get; set; } 

    public List<OffDay>? OffDays { get; set; }
    public List<PaySlip>? PaySlips { get; set; }
    
}