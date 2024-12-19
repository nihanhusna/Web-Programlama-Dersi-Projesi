﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class Identity_User : IdentityUser
    {
        [Display(Name = "İsim Soyisim")]
        [Required(ErrorMessage = "İsim Soyisim girmelisiniz")]
        public string NameSurname { get; set; }
        [Display(Name = "Hakkında")]
        [Required(ErrorMessage = "Hakkında girmelisiniz")]
        public string About { get; set; }
    }
}
