using KadınKuaforu.Areas.Panel.Models;
using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.Areas.Panel.Controllers
{
    [Area("Panel")]
    public class UsersController : Controller
    {
        private readonly UserManager<Identity_User> _userManager;
        private readonly RoleManager<Identity_Role> _roleManager;
        private readonly IPersonnelRepository _personnelRepository;
        private readonly IRankRepository _rankRepository;
        private readonly IRankTaskRepository _rankTaskRepository;
        private readonly IExpertOfTaskRepository _expertOfTaskRepository;
        public UsersController(UserManager<Identity_User> userManager, RoleManager<Identity_Role> roleManager, IPersonnelRepository personnelRepository, IRankRepository rankRepository, IRankTaskRepository rankTaskRepository, IExpertOfTaskRepository expertOfTaskRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _personnelRepository = personnelRepository;
            _rankRepository = rankRepository;
            _rankTaskRepository = rankTaskRepository;
            _expertOfTaskRepository = expertOfTaskRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoleList = new List<(string, string)>();
            foreach (var user in users)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                userRoleList.Add((user.Email, userRole.SingleOrDefault()));
            }
            ViewBag.UserRoles=userRoleList;
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            var editUserModel = new EditUserViewModel();
            var user = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(user);
            editUserModel.RoleName = userRole.SingleOrDefault();
            var roles = await _roleManager.Roles.ToListAsync();
            var roleList = new List<string>();
            foreach (var role in roles)
            {
                var roleName = role.Name;
                roleList.Add(roleName);
            }
            editUserModel.Roles = new SelectList(roleList);
            editUserModel.Id = userId;
            editUserModel.Username = user.UserName;
            editUserModel.NameSurname = user.NameSurname;
            editUserModel.About = user.About;
            return View(editUserModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            var personnel = _personnelRepository.GetByIdWithProps(x => x.IdentityUserId == model.Id);
            if (personnel != null && (model.RoleName== "KuaforElemani" || model.RoleName == "GuzellikElemani" || model.RoleName == "BakimElemani"))
            {
                var rank = _rankRepository.GetByIdWithProps(t=>t.Name == model.RoleName);
                personnel.RankId = rank.Id;
                _personnelRepository.Update(personnel);
                _personnelRepository.Save();
                
            }else if(personnel != null && (model.RoleName != "KuaforElemani" || model.RoleName != "GuzellikElemani" || model.RoleName != "BakimElemani"))
            {
                var experts = _expertOfTaskRepository.GetAllByCondition(l=>l.PersonnelId==personnel.Id);
                foreach (var expert in experts)
                {
                    _expertOfTaskRepository.Delete(expert);
                }
                _personnelRepository.Delete(personnel);
                _personnelRepository.Save();
            }else if(personnel == null)
            {
                var rank = _rankRepository.GetByIdWithProps(t => t.Name == model.RoleName);
                var personnelNew = new Personnel()
                {
                    IdentityUserId = user.Id,
                    RankId = rank.Id
                };
                _personnelRepository.Add(personnelNew);
                _personnelRepository.Save();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, model.RoleName);
            return RedirectToAction("Index", "Users", new {area="Panel"});
        }
        [HttpGet]
        public IActionResult User(string userid)
        {
            var personnel = _personnelRepository.GetByIdWithProps(p=>p.IdentityUserId == userid,"Rank,Identity_User");
            var tasks = _rankTaskRepository.GetAllByCondition(rt=>rt.RankId==personnel.RankId);
            var experts = _expertOfTaskRepository.GetAllByCondition(et=>et.PersonnelId==personnel.Id);
            var personnelInfo = new List<EditPersonnelModel>();
            ViewBag.PersonnelId = personnel.Id;
            ViewBag.PersonnelName = personnel.Identity_User.NameSurname;
            ViewBag.Email = personnel.Identity_User.Email;
            foreach (var task in tasks)
            {
                var model = new EditPersonnelModel()
                {
                    PersonnelId = personnel.Id,
                    NameSurname = personnel.Identity_User.NameSurname,
                    Email = personnel.Identity_User.Email,
                    RankName = personnel.Rank.Name,
                    RankTaskId = task.Id,
                    RankTaskName = task.Name
                };
                foreach(var expert in experts)
                {
                    if(expert.RankTaskId== task.Id)
                    {
                        model.IsExpert = true;
                        break;
                    }
                    else model.IsExpert = false;
                }
                personnelInfo.Add(model);
            }
            return View(personnelInfo);
        }
        public IActionResult ChangeExpert(int taskid, int personnelid)
        {
            var expert = _expertOfTaskRepository.GetByIdWithProps(et=>et.RankTaskId==taskid && et.PersonnelId==personnelid);
            if(expert == null)
            {
                var data = new ExpertOfTask()
                {
                    PersonnelId = personnelid,
                    RankTaskId = taskid
                };
                _expertOfTaskRepository.Add(data);
                _expertOfTaskRepository.Save();
            }
            else
            {
                _expertOfTaskRepository.Delete(expert);
                _expertOfTaskRepository.Save();
            }
            var personnel = _personnelRepository.GetById(personnelid);
            return RedirectToAction("User", "Users", new {area="Panel", userid=personnel.IdentityUserId});
        }
    }
}
