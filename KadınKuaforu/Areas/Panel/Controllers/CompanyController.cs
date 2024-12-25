using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KadınKuaforu.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IRankRepository rankRepository;
        private readonly IRankTaskRepository _rankTaskRepository;
        private readonly RoleManager<Identity_Role> roleManager;
        private readonly UserManager<Identity_User> userManager;

        public CompanyController(ICompanyRepository companyRepository, RoleManager<Identity_Role> roleManager, IRankRepository rankRepository, IRankTaskRepository rankTaskRepository, UserManager<Identity_User> userManager)
        {
            _companyRepository = companyRepository;
            this.roleManager = roleManager;
            this.rankRepository = rankRepository;
            _rankTaskRepository = rankTaskRepository;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var companyInfo = _companyRepository.GetAll();
            var company = companyInfo.SingleOrDefault();
            if (companyInfo ==null || !companyInfo.Any())
            {
                company = new Company()
                {
                    Name = "",
                    Address = "",
                    EmailAddress = "example@gmail.com",
                    PhoneNumber = "2642121212",
                    OpenTime = TimeSpan.FromMinutes(480),
                    CloseTime = TimeSpan.FromMinutes(1200)
                };
                _companyRepository.Add(company);
                _companyRepository.Save();
            }
            return View(company);
        }
        [HttpPost]
        public IActionResult Index(Company updateModel)
        {
            if(!ModelState.IsValid)
                return View(updateModel);
            var companyInfo = _companyRepository.GetById(updateModel.Id);
            companyInfo.Name = updateModel.Name;
            companyInfo.Address = updateModel.Address;
            companyInfo.EmailAddress = updateModel.EmailAddress;
            companyInfo.PhoneNumber = updateModel.PhoneNumber;
            companyInfo.OpenTime = updateModel.OpenTime;
            companyInfo.CloseTime = updateModel.CloseTime;
            _companyRepository.Update(companyInfo);
            _companyRepository.Save();
            return RedirectToAction(nameof(Index),"Company");
        }
        public async Task<IActionResult> Rank()
        {
            var roles = new[] { "Admin", "KuaforElemani", "GuzellikElemani", "BakimElemani" };
            var rankList= rankRepository.GetAll().ToList();
            foreach (var role in roles)
            {
                // Rol daha önce eklenmemişse ekle
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Identity_Role() { Name=role});
                    var rank = new Rank() { Name = role };
                    rankRepository.Add(rank);
                    rankRepository.Save();
                    rankList.Add(rank);
                }
            }
            return View(rankList);
        }
        [HttpGet]
        public IActionResult GetPersonnelsByRank(int rank)
        {
            return ViewComponent("GetRankPersonnels", new { rankid = rank });
        }
        [HttpGet]
        public IActionResult GetTasksByRank(int rank)
        {
            return ViewComponent("GetRanksTasks", new { rankid = rank });
        }
        [HttpPost]
        public IActionResult AddNewTask(int rankId, string taskName)
        {
            try
            {
                // Görev ekleme işlemi
                var newTask = new RankTask
                {
                    RankId = rankId,
                    Name = taskName
                };
                _rankTaskRepository.Add(newTask);
                _rankTaskRepository.Save();

                return Json(new { success = true, message=rankId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult UpdateRankTask(int taskId, string taskName)
        {
            try
            {
                // Veritabanında görevi bul
                var task = _rankTaskRepository.GetById(taskId);
                if (task == null)
                {
                    return Json(new { success = false, message = "Görev bulunamadı." });
                }

                // Görevi güncelle
                task.Name = taskName;
                _rankTaskRepository.Update(task);
                _rankTaskRepository.Save();

                return Json(new { success = true, message=task.RankId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult DeleteRankTask(int taskId)
        {
            try
            {
                // Veritabanında görevi bul
                var task = _rankTaskRepository.GetById(taskId);
                if (task == null)
                {
                    return Json(new { success = false, message = "Görev bulunamadı." });
                }

                // Görevi sil
                _rankTaskRepository.Delete(task);
                _rankTaskRepository.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddNewPersonnel(string user, string role)
        {

            try
            {
                var currentUser = await userManager.FindByNameAsync(user);
                var userRoles = await userManager.GetRolesAsync(currentUser);
                await userManager.RemoveFromRolesAsync(currentUser, userRoles);
                var result = await userManager.AddToRoleAsync(currentUser, role);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = "Kullanıcıya rol başarıyla eklendi." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Kullanıcıya rol eklenemedi.", errors = result.Errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
