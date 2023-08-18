using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete;

public class AppRole : IdentityRole<int>
{
    public string? RoleName { get; set; }

}