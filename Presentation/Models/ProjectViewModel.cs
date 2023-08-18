using Entities.Concrete;
using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class ProjectViewModel
{

    public int Id { get; set; }
    [Required(ErrorMessage = "Proje Başlığı zorunludur.")]
    public string ProjectTitle { get; set; }
    [Required(ErrorMessage = "Proje Açıklaması zorunludur.")]
    public string ProjectDescription { get; set; }
    [Required(ErrorMessage = "Proje Başlangıç Tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [Required(ErrorMessage = "Proje Bitiş Tarihi zorunludur.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Required(ErrorMessage = "Proje Durumu zorunludur.")]
    public ProjectStatus Status { get; set; }

    // Seçilen kullanıcıların Id'lerini tutmak için bir liste
    public List<int> SelectedAppUserIds { get; set; }

    // Kullanıcıları seçmek için kullanılacak liste
    public List<AppUser> AvailableAppUsers { get; set; }


}