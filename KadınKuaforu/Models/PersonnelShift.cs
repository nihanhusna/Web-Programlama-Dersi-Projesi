using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class PersonnelShift
    {
        [Key,ForeignKey("Personnel")]
        public int PersonnelId { get; set; }
        public Personnel Personnel { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public DayOfWeek Day { get; set; }
    }
}
