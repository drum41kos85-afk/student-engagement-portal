using Microsoft.AspNetCore.Identity;
 
namespace StudentEngagementPortal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = "Student"; // "Student" or "Admin"
    }
}
