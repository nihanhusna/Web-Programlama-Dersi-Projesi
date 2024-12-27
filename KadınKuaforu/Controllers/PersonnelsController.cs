using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Controllers
{
    public class PersonnelsController : Controller
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly UserManager<Identity_User> _userManager;
        private readonly IPersonnelRepository _personnelRepository;
        public PersonnelsController(IMeetingRepository meetingRepository, UserManager<Identity_User> userManager, IPersonnelRepository personnelRepository)
        {
            _meetingRepository = meetingRepository;
            _userManager = userManager;
            _personnelRepository = personnelRepository;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);
            var meetings = _meetingRepository.GetAllByCondition(m=>m.Personnel.IdentityUserId==currentUser.Id,"Identity_User,RankTask");
            return View(meetings);
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
                return Json(new { success = false, message = "Belirtilen tarih için izinlisiniz" });
            }
            if (shift.PersonnelShift.Start > modelMeetStart || shift.PersonnelShift.End < modelMeetStart.Add(TimeSpan.FromMinutes(duration)))
            {
                return Json(new { success = false, message = "Belirtilen saatlerde mesainiz bulunmuyor" });
            }
            var personnelMeets = _meetingRepository.GetAllByCondition(m => m.Id != model.Id && m.PersonnelId == meeting.PersonnelId && m.Date == modelDate.Date);
            if (personnelMeets != null && personnelMeets.Any())
            {
                foreach (var personnelmeeting in personnelMeets)
                {
                    if ((personnelmeeting.MeetStart <= modelMeetStart && modelMeetStart <= personnelmeeting.MeetFinish) || (personnelmeeting.MeetStart <= modelMeetStart.Add(TimeSpan.FromMinutes(duration)) && modelMeetStart.Add(TimeSpan.FromMinutes(duration)) <= personnelmeeting.MeetFinish))
                    {
                        return Json(new { success = false, message = $"{personnelmeeting.MeetStart} - {personnelmeeting.MeetFinish} saatlerinde farklı bir randevunuz bulunuyor." });
                    }
                }
            }
            var userMeets = _meetingRepository.GetAllByCondition(m => m.Id != meeting.Id && m.Identity_UserID == meeting.Identity_UserID && m.Date == modelDate.Date);
            if (userMeets != null && userMeets.Any())
            {
                foreach (var usermeeting in userMeets)
                {
                    if ((usermeeting.MeetStart <= modelMeetStart && modelMeetStart <= usermeeting.MeetFinish) || (usermeeting.MeetStart <= modelMeetStart.Add(TimeSpan.FromMinutes(duration)) && modelMeetStart.Add(TimeSpan.FromMinutes(duration)) <= usermeeting.MeetFinish))
                    {
                        return Json(new { success = false, message = $"{usermeeting.MeetStart} - {usermeeting.MeetFinish} saatlerinde müşterinin farklı bir randevusu bulunuyor." });
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
    }
}
