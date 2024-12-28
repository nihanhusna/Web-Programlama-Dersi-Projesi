using KadınKuaforu.Areas.Panel.Models;
using KadınKuaforu.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KadınKuaforu.Areas.Panel.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Panel")]
    public class FollowsController : Controller
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IRankTaskRepository _rankTaskRepository;

        public FollowsController(IMeetingRepository meetingRepository, IPersonnelRepository personnelRepository, IRankTaskRepository rankTaskRepository)
        {
            _meetingRepository = meetingRepository;
            _personnelRepository = personnelRepository;
            _rankTaskRepository = rankTaskRepository;
        }

        public IActionResult Index()
        {
            var meetings = _meetingRepository.GetAll("Identity_User,Personnel,Personnel.Identity_User,RankTask");
            return View(meetings);
        }
        public IActionResult Passive(int id)
        {
            var meet = _meetingRepository.GetById(id);
            meet.IsPassive = true;
            _meetingRepository.Update(meet);
            _meetingRepository.Save();
            return RedirectToAction("Index", "Follows", new {area="Panel"});
        }
        public IActionResult Personnels()
        {
            var model = new FollowCompanyModel();
            var meetings = _meetingRepository.GetAll("Personnel,Personnel.Identity_User,RankTask");
            var frequencyPersonnel = meetings
                .GroupBy(x => x.PersonnelId)
                .Select(g => new
                {
                    PersonnelId = g.Key,
                    AppointmentCount = g.Count(),
                    TotalPrice = g.Sum(x => x.Price) // Her bir grup için toplam kazanç
                })
                .ToList();

            // En çok randevusu olan personeli bul
            var mostBookedPersonnel = frequencyPersonnel
                .OrderByDescending(p => p.AppointmentCount)
                .FirstOrDefault();
            var personnel = _personnelRepository.GetByIdWithProps(x=>x.Id==mostBookedPersonnel.PersonnelId, "Identity_User");
            model.MostBookedPersonnelName = personnel.Identity_User.NameSurname;
            model.MostBookedPersonnelAppointment = mostBookedPersonnel.AppointmentCount;
            model.MostBookedPersonnelPrice=mostBookedPersonnel.TotalPrice;
            var leastBookedPersonnel = frequencyPersonnel
                .OrderBy(p => p.AppointmentCount)
                .FirstOrDefault();
            personnel = _personnelRepository.GetByIdWithProps(x => x.Id == leastBookedPersonnel.PersonnelId, "Identity_User");
            model.LeastBookedPersonnelName = personnel.Identity_User.NameSurname;
            model.LeastBookedPersonnelAppointment = leastBookedPersonnel.AppointmentCount;
            model.LeastBookedPersonnelPrice = leastBookedPersonnel.TotalPrice;
            var mostProfitablePersonnel = frequencyPersonnel
                .OrderByDescending(p => p.TotalPrice)
                .FirstOrDefault();
            personnel = _personnelRepository.GetByIdWithProps(x => x.Id == mostProfitablePersonnel.PersonnelId, "Identity_User");
            model.MostProfitablePersonnelName = personnel.Identity_User.NameSurname;
            model.MostProfitablePersonnelAppointment = mostProfitablePersonnel.AppointmentCount;
            model.MostProfitablePersonnelPrice = mostProfitablePersonnel.TotalPrice;
            var leastProfitablePersonnel = frequencyPersonnel
                .OrderBy(p => p.TotalPrice)
                .FirstOrDefault();
            personnel = _personnelRepository.GetByIdWithProps(x => x.Id == leastProfitablePersonnel.PersonnelId, "Identity_User");
            model.LeastProfitablePersonnelName = personnel.Identity_User.NameSurname;
            model.LeastProfitablePersonnelAppointment = leastProfitablePersonnel.AppointmentCount;
            model.LeastProfitablePersonnelPrice = leastProfitablePersonnel.TotalPrice;

            var frequencyTask = meetings
                .GroupBy(x => x.RankTaskId)
                .Select(g => new
                {
                    RankTaskId = g.Key,
                    AppointmentCount = g.Count(),
                    TotalPrice = g.Sum(x => x.Price) // Her bir grup için toplam kazanç
                })
                .ToList();
            var mostBookedTask = frequencyTask
                .OrderByDescending(p => p.AppointmentCount)
                .FirstOrDefault();
            var task = _rankTaskRepository.GetById(mostBookedTask.RankTaskId);
            model.MostBookedTaskName = task.Name;
            model.MostBookedTaskPrice =mostBookedTask.TotalPrice;
            model.MostBookedTaskAppointment = mostBookedTask.AppointmentCount;
            var leastBookedTask = frequencyTask
                .OrderBy(p => p.AppointmentCount)
                .FirstOrDefault();
            task = _rankTaskRepository.GetById(leastBookedTask.RankTaskId);
            model.LeastBookedTaskName = task.Name;
            model.LeastBookedTaskPrice = leastBookedTask.TotalPrice;
            model.LeastBookedTaskAppointment = leastBookedTask.AppointmentCount;
            var mostProfitableTask = frequencyTask
                .OrderByDescending(p => p.TotalPrice)
                .FirstOrDefault();
            task = _rankTaskRepository.GetById(mostProfitableTask.RankTaskId);
            model.MostProfitableTaskName = task.Name;
            model.MostProfitableTaskPrice = mostProfitableTask.TotalPrice;
            model.MostProfitableTaskAppointment = mostProfitableTask.AppointmentCount;
            var leastProfitableTask = frequencyTask
                .OrderBy(p => p.TotalPrice)
                .FirstOrDefault();
            task = _rankTaskRepository.GetById(leastProfitableTask.RankTaskId);
            model.LeastProfitableTaskName = task.Name;
            model.LeastProfitableTaskPrice = leastProfitableTask.TotalPrice;
            model.LeastProfitableTaskAppointment = leastProfitableTask.AppointmentCount;
            return View(model);
        }
    }
}
