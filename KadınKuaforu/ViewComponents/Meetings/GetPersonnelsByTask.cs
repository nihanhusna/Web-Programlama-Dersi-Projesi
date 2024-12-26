using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.ViewComponents.Meetings
{
    public class GetPersonnelsByTask : ViewComponent
    {
        private readonly IPersonnelRepository personnelRepository;
        private readonly IExpertOfTaskRepository expertOfTaskRepository;

        public GetPersonnelsByTask(IPersonnelRepository personnelRepository, IExpertOfTaskRepository expertOfTaskRepository)
        {
            this.personnelRepository = personnelRepository;
            this.expertOfTaskRepository = expertOfTaskRepository;
        }

        public IViewComponentResult Invoke(int taskid, string rankname)
        {
            var expertPersonnels = expertOfTaskRepository.GetAllByCondition(e => e.RankTaskId == taskid);
            var personnels = personnelRepository.GetAllByCondition(p=>p.Rank.Name == rankname,"Identity_User");
            var personnelList = new List<MeetingPersonnelModel>();
            foreach (var personnel in personnels)
            {
                var personnelModel = new MeetingPersonnelModel()
                {
                    PersonnelID = personnel.Id,
                    PersonnelName = personnel.Identity_User.NameSurname,
                    RankTaskID = taskid,
                    isExpert = false
                };
                foreach(var ep in expertPersonnels)
                {
                    if (ep.PersonnelId == personnel.Id)
                    {
                        personnelModel.isExpert = true;
                        break;
                    }
                }
                personnelList.Add(personnelModel);
            }
            return View(personnelList);
        }
    }
}
