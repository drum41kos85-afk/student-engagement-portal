using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; } // report requires filtering by category

        [Required]
        public DateTime StartsAt { get; set; }

        [Required, Range(1, 1000)]
        public int MaxParticipants { get; set; } // report requires a participant limit

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}