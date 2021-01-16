using Microsoft.AspNetCore.Mvc;

namespace DocKick.Categorizable.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("api")]
        public string CheckApi()
        {
            return "Done";
        }
    }
}