using KadınKuaforu.DataAccess;
using KadınKuaforu.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KadınKuaforu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KadinKuaforuController : ControllerBase
    {
        private readonly IGeneratorRepository _generatorRepository;

        public KadinKuaforuController(IGeneratorRepository generatorRepository)
        {
            _generatorRepository = generatorRepository;
        }

        [Route("CreateGenerator")]
        [HttpPost]
        public IActionResult CreateGenerator([FromBody] CreateGeneratorModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _generatorRepository.Add(
                new Generator()
                {
                    IdentityUserId = model.IdentityUserId,
                    Request = model.Request,
                    Response = model.Response
                });
            _generatorRepository.Save();
            
            return Ok();
        }
        [HttpGet("AllGenerators/{identityUserId}")]
        [Route("AllGenerators")]
        public IActionResult AllGenerators(string identityUserId)
        {
            var model = _generatorRepository.GetAllByCondition(x=>x.IdentityUserId==identityUserId);
            if (model == null || !model.Any())
                return BadRequest("Kullanıcıya ait yapay zeka sorgusu bulunamadı");
            return Ok(model);
        }
    }
}
