using System.ComponentModel.DataAnnotations;
 
namespace StudentEngagementPortal.Models
{
    public class Announcement
    {
        public int Id { get; set; }
 
        [Required, MaxLength(150)]
        public string Title { get; set; }
 
        [Required]
        public string Body { get; set; }
 
        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
 
        [Required]
        public string PostedByAdminId { get; set; }
        public ApplicationUser PostedByAdmin { get; set; }
    }
}
