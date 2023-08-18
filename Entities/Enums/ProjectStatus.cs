using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Entities.Enums;

public enum ProjectStatus
{
    [Display(Name = "Başladı")] Start = 0,
    [Display(Name = "Devam Ediyor")] Continue = 1,
    [Display(Name = "Başarılı Bitti")] SuccessEnd = 2,
    [Display(Name = "Başarısız Bitti")] FailedEnd = 3,
}