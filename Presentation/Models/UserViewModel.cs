namespace Presentation.Models;

public class UserViewModel
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Mail { get; set; }
    public string? DepartmentName { get; set; }
    public string? JobName { get; set; }
    public string? NationalityId { get; set; }
    public decimal? Salary { get; set; }
    public DateTime BirthDate { get; set; }
    public string? EmployeeCode { get; set; }
    public string? PhoneNumber { get; set; }
    public bool Status { get; set; }
}