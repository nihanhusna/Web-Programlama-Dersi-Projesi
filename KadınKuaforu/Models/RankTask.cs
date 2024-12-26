using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class RankTask
    {
        public int Id { get; set; }
        [Display(Name = "Hizmet")]
        [Required(ErrorMessage = "Hizmet ismi zorunludur")]
        public string Name { get; set; }
        [Display(Name="Birim")]
        [Required(ErrorMessage ="Birim bilgisi alınamadı")]
        public int RankId { get; set; }
        [ForeignKey(nameof(RankId))]
        public Rank Rank { get; set; }
        public int Price { get; set; } = 0;
        public int Time { get; set; } = 0;
        public ICollection<ExpertOfTask> ExpertOfTasks { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
    }
}
