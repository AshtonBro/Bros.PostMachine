using Bros.PostMachine.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bros.PostMachine.Controllers
{
    public class ContentController : Controller
    {
        public IActionResult Index(SocialViewModel socialViewModel)
        {
            return Redirect("/Home/Index");
        }
    }
}