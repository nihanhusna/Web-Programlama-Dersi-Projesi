using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly IRankTaskRepository _taskRepository;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IExpertOfTaskRepository _expertOfTaskRepository;
        private readonly UserManager<Identity_User> userManager;
        private readonly IMeetingRepository _meetingRepository;
        public MeetingsController(IRankTaskRepository taskRepository, IPersonnelRepository personnelRepository, IExpertOfTaskRepository expertOfTaskRepository, UserManager<Identity_User> userManager, IMeetingRepository meetingRepository)
        {
            _taskRepository = taskRepository;
            _personnelRepository = personnelRepository;
            _expertOfTaskRepository = expertOfTaskRepository;
            this.userManager = userManager;
            _meetingRepository = meetingRepository;
        }
        [HttpGet]
        public IActionResult Index(string name, int? taskId)
        {
            ViewBag.Name = name;
            ViewBag.TaskID = taskId;
            var tasks = _taskRepository.GetAllByCondition(t=>t.Rank.Name==name,"Rank");
            return View(tasks);
        }
        [HttpGet]
        public IActionResult PersonnelsByTask(string name, int id)
        {
            return RedirectToAction("Index", "Meetings", new {name= name, taskId=id});
        }
        [HttpGet]
        public IActionResult CreateMeeting(int taskid, int personnelid)
        {
            var task = _taskRepository.GetById(taskid);
            var personnel = _personnelRepository.GetByIdWithProps(p=>p.Id == personnelid, "Identity_User,PersonnelShift");
            var isExpert = _expertOfTaskRepository.GetByIdWithProps(ep=>ep.RankTaskId==taskid&&ep.PersonnelId==personnelid);
            var model = new CreateMeetingModel()
            {
                PersonnelId=personnelid,
                PersonnelName=personnel.Identity_User.NameSurname,
                RankTaskId=taskid,
                RankTaskName = task.Name,
                Price = task.Price,
                Time = task.Time,
                PersonnelStart = personnel.PersonnelShift.Start,
                PersonnelEnd = personnel.PersonnelShift.End
            };
            if (isExpert !=null)
            {
                model.Price = task.Price +(task.Price*20/100);
                model.Time = task.Time - (task.Time * 25 / 100);
            }
            var offDay = (int)personnel.PersonnelShift.Day;
            if (offDay == 0) { model.PersonnelOFFDay = "Pazar"; }
            else if(offDay == 1) { model.PersonnelOFFDay = "Pazartesi"; }
            else if(offDay == 2) { model.PersonnelOFFDay = "Salı"; }
            else if(offDay == 3) { model.PersonnelOFFDay = "Çarşamba"; }
            else if(offDay == 4) { model.PersonnelOFFDay = "Perşembe"; }
            else if(offDay == 5) { model.PersonnelOFFDay = "Cuma"; }
            else if(offDay == 6) { model.PersonnelOFFDay = "Cumartesi"; }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMeeting(CreateMeetingModel model)
        {

            if(!ModelState.IsValid)
                return View(model);
            var startMeet = model.Start;
            var endMeet = model.Start.Add(TimeSpan.FromMinutes(model.Time));
            var shift = _personnelRepository.GetByIdWithProps(p=>p.Id==model.PersonnelId,"PersonnelShift");
            if (model.Date.DayOfWeek == shift.PersonnelShift.Day)
            {
                ViewData["MeetMsg"] = "Belirtilen tarih için çalışan izinli";
                return View(model);
            }
            if(shift.PersonnelShift.Start>startMeet || shift.PersonnelShift.End < endMeet)
            {
                ViewData["MeetMsg"] = "Belirtilen saatlerde çalışan mesai yapmıyor";
                return View(model);
            }
            var personnelMeets = _meetingRepository.GetAllByCondition(m=>m.PersonnelId==model.PersonnelId && m.Date==model.Date.Date);
            if(personnelMeets!=null && personnelMeets.Any())
            {
                foreach(var meeting in personnelMeets)
                {
                    if((meeting.MeetStart <= startMeet && startMeet <= meeting.MeetFinish) || (meeting.MeetStart <= endMeet && endMeet <= meeting.MeetFinish))
                    {
                        ViewData["MeetMsg"] = $"{meeting.MeetStart} - {meeting.MeetFinish} saatlerinde personelin farklı bir randevusu bulunuyor.";
                        return View(model);
                    }
                }
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userMeets = _meetingRepository.GetAllByCondition(m => m.Identity_UserID == user.Id && m.Date == model.Date.Date);
            if (userMeets != null && userMeets.Any())
            {
                foreach (var meeting in userMeets)
                {
                    if ((meeting.MeetStart <= startMeet && startMeet <= meeting.MeetFinish) || (meeting.MeetStart <= endMeet && endMeet <= meeting.MeetFinish))
                    {
                        ViewData["MeetMsg"] = $"{meeting.MeetStart} - {meeting.MeetFinish} saatlerinde farklı bir randevunuz bulunuyor.";
                        return View(model);
                    }
                }
            }
            var meet = new Meeting()
            {
                Identity_UserID = user.Id,
                PersonnelId = model.PersonnelId,
                RankTaskId = model.RankTaskId,
                Date = model.Date.Date,
                MeetStart = model.Start,
                MeetFinish = model.Start.Add(TimeSpan.FromMinutes(model.Time)),
                Price = model.Price
            };
            _meetingRepository.Add(meet);
            _meetingRepository.Save();
            ViewData["MeetMsg"] = "Randevunuz başarıyla oluşturuldu";
            return View(model);
        }
        public async Task<IActionResult> MyMeetings()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userMeetings = _meetingRepository.GetAllByCondition(m => m.Identity_UserID == user.Id, "RankTask,Personnel,Personnel.Identity_User");
            return View(userMeetings);
        }
        [HttpPost]
        public IActionResult UpdateMeeting([FromBody] UpdateMeetingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Geçersiz veri." });
            }
            var modelDate = DateTime.Parse(model.Date);
            var modelMeetStart = TimeSpan.Parse(model.MeetStart);
            var meeting = _meetingRepository.GetById(model.Id);
            if (meeting == null)
            {
                return Json(new { success = false, message = "Randevu bulunamadı." });
            }
            var startmeeting = (int)meeting.MeetStart.TotalMinutes;
            var endmeeting = (int)meeting.MeetFinish.TotalMinutes;
            var duration = endmeeting - startmeeting;
            var shift = _personnelRepository.GetByIdWithProps(p => p.Id == meeting.PersonnelId, "PersonnelShift");
            if (modelDate.DayOfWeek == shift.PersonnelShift.Day)
            {
                return Json(new { success = false, message = "Belirtilen tarih için çalışan izinli" });
            }
            if (shift.PersonnelShift.Start > modelMeetStart || shift.PersonnelShift.End < modelMeetStart.Add(TimeSpan.FromMinutes(duration)))
            {
                return Json(new { success = false, message = "Belirtilen saatlerde çalışan mesai yapmıyor" });
            }
            var personnelMeets = _meetingRepository.GetAllByCondition(m =>m.Id!=model.Id && m.PersonnelId == meeting.PersonnelId && m.Date == modelDate.Date);
            if (personnelMeets != null && personnelMeets.Any())
            {
                foreach (var personnelmeeting in personnelMeets)
                {
                    if ((personnelmeeting.MeetStart <= modelMeetStart && modelMeetStart <= personnelmeeting.MeetFinish) || (personnelmeeting.MeetStart <= modelMeetStart.Add(TimeSpan.FromMinutes(duration)) && modelMeetStart.Add(TimeSpan.FromMinutes(duration)) <= personnelmeeting.MeetFinish))
                    {
                        return Json(new { success = false, message = $"{personnelmeeting.MeetStart} - {personnelmeeting.MeetFinish} saatlerinde personelin farklı bir randevusu bulunuyor." });
                    }
                }
            }
            var userMeets = _meetingRepository.GetAllByCondition(m =>m.Id!=meeting.Id && m.Identity_UserID == meeting.Identity_UserID && m.Date == modelDate.Date);
            if (userMeets != null && userMeets.Any())
            {
                foreach (var usermeeting in userMeets)
                {
                    if ((usermeeting.MeetStart <= modelMeetStart && modelMeetStart <= usermeeting.MeetFinish) || (usermeeting.MeetStart <= modelMeetStart.Add(TimeSpan.FromMinutes(duration)) && modelMeetStart.Add(TimeSpan.FromMinutes(duration)) <= usermeeting.MeetFinish))
                    {
                        return Json(new { success = false, message = $"{usermeeting.MeetStart} - {usermeeting.MeetFinish} saatlerinde farklı bir randevunuz bulunuyor." });
                    }
                }
            }
            meeting.Date = modelDate;
            meeting.MeetStart = modelMeetStart;
            meeting.MeetFinish = modelMeetStart.Add(TimeSpan.FromMinutes(duration));

            _meetingRepository.Update(meeting);
            _meetingRepository.Save();

            return Json(new { success = true });


        }

        public IActionResult DeleteMeeting(int id)
        {
            _meetingRepository.Delete(_meetingRepository.GetById(id));
            _meetingRepository.Save();
            return View();
        }
    }
}
