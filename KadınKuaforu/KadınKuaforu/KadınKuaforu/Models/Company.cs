using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Display(Name="Şirket İsmi")]
        [Required(ErrorMessage ="Şirket isminizi girmelisiniz")]
        public string Name { get; set; }
        [Display(Name = "Şirket Adresi")]
        [Required(ErrorMessage = "Şirket adresinizi girmelisiniz")]
        public string Address { get; set; }
        [Display(Name = "Şirket Numarası")]
        [Phone(ErrorMessage ="Şirket telefon numaranızı doğru formatta giriniz")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Şirket Email Adresi")]
        [EmailAddress(ErrorMessage ="Şirket Email Adresinizi doğru formatta giriniz")]
        public string EmailAddress { get; set; }
    }
}
