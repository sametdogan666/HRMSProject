using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace Entities.Enums;

public enum GenderType
{
    [Display(Name = "Erkek")] Male = 0,
    [Display(Name = "Kadın")] Female = 1,
    [Display(Name = "Belirtmek Istemiyor")] Unknown = 2,
}