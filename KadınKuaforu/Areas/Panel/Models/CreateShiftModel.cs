using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KadınKuaforu.Areas.Panel.Models
{
    public class CreateShiftModel
    {
        public int PersonnelId { get; set; }
        [Display(Name = "Mesai Başlama")]
        public TimeSpan Start { get; set; }
        [Display(Name = "Mesai Bitiş")]
        public TimeSpan End { get; set; }
        [Display(Name ="İzin Günü")]
        public int selectedDay { get; set; }
        public List<SelectListItem>? Days { get; set; }
    }
}
