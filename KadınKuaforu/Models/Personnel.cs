using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class Personnel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Kullanıcı bilgisi alınamadı")]
        [Display(Name ="Kullanıcı")]
        public string IdentityUserId { get; set; }
        [ForeignKey(nameof(IdentityUserId))]
        public Identity_User Identity_User { get; set; }
        [Required(ErrorMessage = "Birim bilgisi alınamadı")]
        [Display(Name = "Birim")]
        public int RankId { get; set; }
        [ForeignKey(nameof(RankId))]   
        public Rank Rank { get; set; }
        public ICollection<ExpertOfTask> ExpertOfTasks { get; set; }
    }
}
