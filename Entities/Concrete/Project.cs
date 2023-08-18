using Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete;

public class Project
{
    public Project()
    {
        AppUsers = new List<AppUser>();
    }
    public int Id { get; set; }
    public string? ProjectTitle { get; set; }
    public string? ProjectDescription { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public List<AppUser> AppUsers { get; set; }

    public bool StatusDelete { get; set; } = true;

}