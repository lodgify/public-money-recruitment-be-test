using Microsoft.AspNetCore.Identity;

namespace VacationRental.DataAccess.Models.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
