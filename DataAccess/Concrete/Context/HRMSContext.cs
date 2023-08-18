using Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.Context;

public class HRMSContext : IdentityDbContext<AppUser, AppRole, int>
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-GQ26LQR; initial catalog= HrmsTestDb; integrated security = true");
        optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-LVPTDQG\SQLEXPRESS; initial catalog= CasgemDbHRMS; integrated security = true");
    }

    public DbSet<Announcement>? Announcements { get; set; }
    public DbSet<Department>? Departments { get; set; }
    public DbSet<Expense>? Expenses { get; set; }
    public DbSet<Job>? Jobs { get; set; }
    public DbSet<OffDay>? OffDays { get; set; }
    public DbSet<Project>? Projects { get; set; }
    public DbSet<PaySlip>? PaySlips { get; set; }
    public DbSet<Message>? Messages { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Project>().HasMany(x => x.AppUsers).WithMany(x=>x.Projects);
        builder.Entity<AppUser>().HasMany(x => x.Projects).WithMany(x=>x.AppUsers);
        base.OnModelCreating(builder);

    }
}