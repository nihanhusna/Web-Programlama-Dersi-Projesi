namespace KadınKuaforu.Models
{
    public class CreateMeetingModel
    {
        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public int RankTaskId { get; set; }
        public string RankTaskName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public TimeSpan PersonnelStart { get; set; }
        public TimeSpan PersonnelEnd { get; set; }
        public string PersonnelOFFDay { get; set; }
    }
}
