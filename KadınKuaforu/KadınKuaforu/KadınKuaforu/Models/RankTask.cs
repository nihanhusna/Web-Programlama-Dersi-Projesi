using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class RankTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RankId { get; set; }
        [ForeignKey(nameof(RankId))]
        public Rank Rank { get; set; }
        public ICollection<ExpertOfTask> ExpertOfTasks { get; set; }
    }
}
