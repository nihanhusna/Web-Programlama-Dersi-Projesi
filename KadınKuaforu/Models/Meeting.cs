using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public string Identity_UserID { get; set; }
        [ForeignKey(nameof(Identity_UserID))]
        public Identity_User Identity_User { get; set; }
        public int PersonnelId { get; set; }
        [ForeignKey(nameof(PersonnelId))]
        public Personnel Personnel { get; set; }
        public int RankTaskId { get; set; }
        [ForeignKey(nameof(RankTaskId))]
        public RankTask RankTask { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan MeetStart { get; set; }
        public TimeSpan MeetFinish { get; set; }
        public int Price { get; set; }
        public bool IsPassive { get; set; } = false;
    }
}
