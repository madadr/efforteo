using Microsoft.AspNetCore.Mvc;

namespace Efforteo.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(int id) => Content("Efforteo");
    }
}
