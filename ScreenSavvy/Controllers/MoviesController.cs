using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ScreenSavvy.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
