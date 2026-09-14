namespace StudentEngagementPortal.Models.ViewModels
{
    public class EventListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime StartsAt { get; set; }
        public int MaxParticipants { get; set; }
        public int SpotsTaken { get; set; }
        public int SpotsRemaining => MaxParticipants - SpotsTaken;
        public bool IsRegisteredByCurrentUser { get; set; }
    }
}