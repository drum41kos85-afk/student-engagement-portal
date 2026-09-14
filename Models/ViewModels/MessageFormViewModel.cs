using System.ComponentModel.DataAnnotations;

namespace StudentEngagementPortal.Models.ViewModels
{
    public class MessageFormViewModel
    {
        [Required, MaxLength(150)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}