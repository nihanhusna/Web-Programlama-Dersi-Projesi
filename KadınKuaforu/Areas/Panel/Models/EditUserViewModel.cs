using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Areas.Panel.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Email")]
        public string? Username { get; set; }
        [Display(Name = "İsim Soyisim")]
        public string? NameSurname { get; set; }
        [Display(Name = "Hakkında")]
        public string? About { get; set; }
        [Display(Name = "Rol")]
        public string RoleName { get; set; }
        public SelectList? Roles { get; set; }
    }
}
