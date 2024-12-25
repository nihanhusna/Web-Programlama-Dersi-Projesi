using Microsoft.AspNetCore.Mvc.Rendering;

namespace KadınKuaforu.Areas.Panel.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string? Username { get; set; }
        public string? NameSurname { get; set; }
        public string? About { get; set; }
        public string RoleName { get; set; }
        public SelectList? Roles { get; set; }
    }
}
