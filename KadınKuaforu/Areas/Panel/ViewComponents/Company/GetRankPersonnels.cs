using KadınKuaforu.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Areas.Panel.ViewComponents.Company
{
    public class GetRankPersonnels : ViewComponent
    {
        private readonly IPersonnelRepository personnelRepository;

        public GetRankPersonnels(IPersonnelRepository personnelRepository)
        {
            this.personnelRepository = personnelRepository;
        }

        public IViewComponentResult Invoke(int rankid)
        {
            var personnels = personnelRepository.GetAllByCondition(p=>p.RankId==rankid,"Identity_User");
            return View(personnels);
        }
    }
}
