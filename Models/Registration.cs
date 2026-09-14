using System.ComponentModel.DataAnnotations;
 
namespace StudentEngagementPortal.Models
{
    public class Registration
    {
        public int Id { get; set; }
 
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }
 
        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
 
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}
