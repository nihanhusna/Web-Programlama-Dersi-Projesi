namespace KadınKuaforu.Models
{
    public class ExpertOfTask
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public Personnel Personnel { get; set; }
        public int RankTask { get; set; }
        public RankTask RankTask { get; set; }
    }
}
