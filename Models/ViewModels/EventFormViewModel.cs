using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Models.ViewModels
{
    public class EventFormViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime StartsAt { get; set; }

        [Required, Range(1, 1000)]
        public int MaxParticipants { get; set; }
    }
}