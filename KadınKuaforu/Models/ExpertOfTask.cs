using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class ExpertOfTask
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Personel bilgisi alınamadı")]
        [Display(Name ="Personel")]
        public int PersonnelId { get; set; }
        [ForeignKey(nameof(PersonnelId))]
        public Personnel Personnel { get; set; }
        [Required(ErrorMessage = "Hizmet bilgisi alınamadı")]
        [Display(Name = "Hizmet")]
        public int RankTaskId { get; set; }

        [ForeignKey(nameof(RankTaskId))]
        public RankTask RankTask { get; set; }
    }
}
