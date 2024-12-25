namespace KadınKuaforu.Areas.Panel.Models
{
    public class EditPersonnelModel
    {
        public int PersonnelId { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string RankName { get; set; }
        public int RankTaskId { get; set; }
        public bool IsExpert { get; set; }
        public string RankTaskName { get; set; }
    }
}
