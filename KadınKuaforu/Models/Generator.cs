using System.ComponentModel.DataAnnotations.Schema;

namespace KadınKuaforu.Models
{
    public class Generator
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public string IdentityUserId { get; set; }
        [ForeignKey(nameof(IdentityUserId))]
        public Identity_User Identity_User { get; set; }
    }
}
