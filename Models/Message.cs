using System.ComponentModel.DataAnnotations;
 
namespace StudentEngagementPortal.Models
{
    public enum MessageStatus { Open, Resolved }
 
    public class Message
    {
        public int Id { get; set; }
 
        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
 
        [Required, MaxLength(150)]
        public string Subject { get; set; }
 
        [Required]
        public string Body { get; set; }
 
        public MessageStatus Status { get; set; } = MessageStatus.Open;
 
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
