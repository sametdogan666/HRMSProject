using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete;

public class PaySlip
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public virtual AppUser? AppUser { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public decimal Awards { get; set; } 
}