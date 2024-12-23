using KadınKuaforu.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Areas.Panel.ViewComponents.Company
{
    public class GetRanksTasks : ViewComponent
    {
        private readonly IRankTaskRepository _rankTaskRepository;

        public GetRanksTasks(IRankTaskRepository rankTaskRepository)
        {
            _rankTaskRepository = rankTaskRepository;
        }
        public IViewComponentResult Invoke(int rankid)
        {
            ViewBag.Rankid = rankid;
            var rankTasks = _rankTaskRepository.GetAllByCondition(rt=>rt.RankId == rankid);
            return View(rankTasks);
        }
    }
}
