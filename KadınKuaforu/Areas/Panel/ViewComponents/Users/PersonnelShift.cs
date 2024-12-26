using KadınKuaforu.Areas.Panel.Models;
using KadınKuaforu.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KadınKuaforu.Areas.Panel.ViewComponents.Users
{
    public class PersonnelShift : ViewComponent
    {
        private readonly IPersonnelShiftRepository _personnelShiftRepository;

        public PersonnelShift(IPersonnelShiftRepository personnelShiftRepository)
        {
            _personnelShiftRepository = personnelShiftRepository;
        }

        public IViewComponentResult Invoke(int personnelid)
        {
            var personnelShift = _personnelShiftRepository.GetById(personnelid);
            var days = new List<SelectListItem>
            {
                new SelectListItem { Text = "Pazartesi", Value = "1" },
                new SelectListItem { Text = "Salı", Value = "2" },
                new SelectListItem { Text = "Çarşamba", Value = "3" },
                new SelectListItem { Text = "Perşembe", Value = "4" },
                new SelectListItem { Text = "Cuma", Value = "5" },
                new SelectListItem { Text = "Cumartesi", Value = "6" },
                new SelectListItem { Text = "Pazar", Value = "0" }
            };
            var model = new CreateShiftModel();
            model.Days = days;
            model.PersonnelId = personnelid;
            if (personnelShift != null)
            {
                model.Start = personnelShift.Start;
                model.End = personnelShift.End;
                model.selectedDay = (int)personnelShift.Day;
            }
            else
            {
                model.Start = TimeSpan.FromMinutes(0);
                model.End = TimeSpan.FromMinutes(0);
            }
            
            return View(model);
        }
    }
}
