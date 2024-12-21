using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class Rank
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Personnel> Personnels { get; set; }
        public ICollection<RankTask> RankTasks { get; set; }
    }
}
