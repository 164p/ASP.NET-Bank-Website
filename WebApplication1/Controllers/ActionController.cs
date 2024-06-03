using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ActionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
