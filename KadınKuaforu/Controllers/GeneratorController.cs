using KadınKuaforu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KadınKuaforu.Controllers
{
    [Authorize]
    public class GeneratorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<Identity_User> _userManager;

        public GeneratorController(IHttpClientFactory httpClientFactory, UserManager<Identity_User> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] Dictionary<string, string> answers)
        {
            if (answers == null || !answers.Any())
            {
                return BadRequest("Cevaplar alınamadı.");
            }
            string yuztipi = "";
            string sactipi = "";
            string uzunluk = "";
            foreach (var answer in answers)
            {
                if(answer.Key=="answer1")
                    yuztipi = answer.Value;
                if(answer.Key== "answer2")
                    sactipi = answer.Value;
                if(answer.Key== "answer3")
                    uzunluk = answer.Value;
            }
            var request = $"Yüz tipi {yuztipi}, saç tipi {sactipi} ve saç uzunluğu {uzunluk} olan bir kadın için üç saç modeli öner.";
            string apiKey = "";
            string model = "gpt-4o";
            ChatClient client = new ChatClient(model, apiKey);
            ChatCompletion completion = client.CompleteChat(request);
            var response = completion.Content[0].Text;
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var createGeneratorModel = new CreateGeneratorModel()
            {
                Request = request,
                Response = response,
                IdentityUserId = user.Id
            };
            var httpClient = _httpClientFactory.CreateClient();
            var createResponse = await httpClient.PostAsJsonAsync("https://localhost:7154/api/KadinKuaforu/CreateGenerator", createGeneratorModel);

            if (createResponse.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Results), "Generator");
            }
            else
            {
                return StatusCode((int)createResponse.StatusCode, "Veri kaydetme sırasında bir hata oluştu.");
            }
        }
        public async Task<IActionResult> Results()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var client = _httpClientFactory.CreateClient();
            var getGenerators = await client.GetAsync($"https://localhost:7154/api/KadinKuaforu/AllGenerators/{user.Id}");
            if (!getGenerators.IsSuccessStatusCode)
            {
                return StatusCode((int)getGenerators.StatusCode, "Veritabanımızda kayıtlı geçmiş sorgunuz yok");
            }

            var generators = await getGenerators.Content.ReadFromJsonAsync<List<Generator>>();
            if (generators != null && generators.Any())
            {
                foreach (var item in generators)
                {
                    item.Response = Regex.Replace(item.Response, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
                    item.Response = item.Response.Replace("\\n", "<br>");
                    item.Response = item.Response.Replace("\n\n", "</p><p>").Replace("\n", "<br />");
                    item.Response = $"<p>{item.Response}</p>";
                }
            }
            return View(generators.OrderByDescending(m => m.Time).ToList());
        }
    }
}
