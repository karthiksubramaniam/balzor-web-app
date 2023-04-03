using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace balzor_web_app.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AboutController : ControllerBase
    {
        private string[] _names = { "Karthik", "KK", "Karthik Subramaniam", "Karthikkumar", "Karthikkumar Subramaniam" };

        [HttpGet("author")]
        public string? GetRandomAuthorName()

        {
            var random = new Random();
            return _names.OrderBy(x => random.Next()).FirstOrDefault();
        }
    }
}