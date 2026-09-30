using System.ComponentModel.DataAnnotations;

namespace instituto93.Web.Models;

public class DniModel
{
    [Required(ErrorMessage = "Ingresá tu D.N.I.")]
    [RegularExpression(@"^[\d.\s]+$", ErrorMessage = "El D.N.I. solo debe contener números.")]
    public string Dni { get; set; } = string.Empty;
}
