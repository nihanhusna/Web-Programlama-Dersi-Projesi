using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Email girmelisiniz")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre girmelisiniz")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Tekrar Şifre")]

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmedi.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "İsim Soyisim")]
        [Required(ErrorMessage = "İsim Soyisim girmelisiniz")]
        public string NameSurname { get; set; }
        [Display(Name = "Hakkında")]
        [Required(ErrorMessage = "Hakkında girmelisiniz")]
        public string About { get; set; }
    }
}
