using Microsoft.AspNetCore.Mvc.Rendering;

namespace KadınKuaforu.Areas.Panel.Models
{
    public class CreateShiftModel
    {
        public int PersonnelId { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int selectedDay { get; set; }
        public List<SelectListItem>? Days { get; set; }
    }
}
