using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class CreateMeetingModel
    {
        public int PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public int RankTaskId { get; set; }
        public string RankTaskName { get; set; }
        [Display(Name = "Randevu Tarihi:")]
        [Required(ErrorMessage = "Randevu tarihi seçmediniz")]
        public DateTime Date { get; set; }
        [Display(Name ="Randevu Saati:")]
        [Required(ErrorMessage = "Randevu saati seçmediniz")]
        public TimeSpan Start { get; set; }
        public int Price { get; set; }
        public int Time { get; set; }
        public TimeSpan PersonnelStart { get; set; }
        public TimeSpan PersonnelEnd { get; set; }
        public string PersonnelOFFDay { get; set; }
    }
}
