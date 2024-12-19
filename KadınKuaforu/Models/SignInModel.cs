using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class SignInModel
    {
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="Email doğru formatta girilmeli")]
        public string Email { get; set; }

        [Required (ErrorMessage ="Şifre girmelisiniz")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
