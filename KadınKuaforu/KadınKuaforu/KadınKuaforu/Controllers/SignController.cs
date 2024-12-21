using KadınKuaforu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Controllers
{
    public class SignController : Controller
    {
        private readonly SignInManager<Identity_User> _signInManager;
        private readonly UserManager<Identity_User> _userManager;

        public SignController(SignInManager<Identity_User> signInManager, UserManager<Identity_User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Up()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Up(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Identity_User { UserName = model.Email, Email = model.Email , NameSurname =model.NameSurname, About=model.About};
                var result = await _userManager.CreateAsync(user, model.ConfirmPassword);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult In()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> In(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    true,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Hesap kilitlendi, fazla yanlış deneme yapıldı.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Giriş bilgileri geçersiz");
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Out()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
