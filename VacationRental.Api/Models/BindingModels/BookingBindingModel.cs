using System;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Resources.ErrorMessages;

namespace VacationRental.Api.Models.BindingModels;

public class BookingBindingModel
{
    /// <summary>
    /// Rental item identifier
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "RentalIdIsRequired")]
    public int RentalId { get; set; }
    
    /// <summary>
    /// Start date
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StartIsRequired")]
    public DateTime Start { get; set; }
  
    /// <summary>
    /// Nights count
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "NightsIsRequired")]
    [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "NightsMustBePositive")]
    public int Nights { get; set; }
}