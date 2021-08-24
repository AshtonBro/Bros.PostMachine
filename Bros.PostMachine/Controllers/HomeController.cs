using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bros.PostMachine.Models;

namespace Bros.PostMachine.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ILogger<HomeController> Logger => _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            CustomSettings.Instance.Save();
            _logger = logger;
        }

        public IActionResult Index()
        {
            var socialViewModel = new SocialNetWorksViewModel();

            socialViewModel.VkViewModel = new VkViewModel()
            {
                UserId = CustomSettings.Instance.VkUserId,
                Login = CustomSettings.Instance.VkLogin,
                Password = CustomSettings.Instance.VkPassword,
                ApplicationId = CustomSettings.Instance.VkApplicationId,
                AccessToken = CustomSettings.Instance.VkAccessToken
            };

            return View(socialViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
