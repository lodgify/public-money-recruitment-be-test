using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Resources.ErrorMessages;

namespace VacationRental.Api.Models.BindingModels;

public class RentalBindingModel
{
    /// <summary>
    /// Units count
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "UnitsIsRequired")]
    public int Units { get; set; }
}
