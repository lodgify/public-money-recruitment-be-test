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

    /// <summary>
    /// This parameter blocks additional X days after each booking for service needs.
    /// </summary>
    /// <example>1</example>
    public int PreparationTimeInDays { get; set; }
}
