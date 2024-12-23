using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class Rank
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Birim ismi zorunludur")]
        [Display(Name="Birim")]
        public string Name { get; set; }
        public ICollection<Personnel> Personnels { get; set; } = new List<Personnel>();
        public ICollection<RankTask> RankTasks { get; set; } = new List<RankTask>();
    }
}
