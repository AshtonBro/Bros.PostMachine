using Bros.PostMachine.Models;
using Bros.PostMachine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bros.PostMachine.Controllers
{
    public class ContentController : Controller
    {
        public IActionResult Index(SocialViewModel socialViewModel)
        {
            if (ModelState.IsValid)
            {
                var vkService = new VkService();

                vkService.PostOnWall(socialViewModel.ContentViewModel, socialViewModel.VkViewModel);

                if (!string.IsNullOrEmpty(socialViewModel.VkViewModel.Login) &&
                    !string.IsNullOrEmpty(socialViewModel.VkViewModel.Password) &&
                    !string.IsNullOrEmpty(socialViewModel.VkViewModel.ApplicationId) &&
                    !string.IsNullOrEmpty(socialViewModel.VkViewModel.AccessToken))
                {
                    CustomSettings.Instance.VkLogin = socialViewModel.VkViewModel.Login;
                    CustomSettings.Instance.VkPassword = socialViewModel.VkViewModel.Password;
                    CustomSettings.Instance.VkApplicationId = socialViewModel.VkViewModel.ApplicationId;
                    CustomSettings.Instance.VkAccessToken = socialViewModel.VkViewModel.AccessToken;

                    CustomSettings.Instance.Save();
                }

                return RedirectToAction("Index", "Home");
            } else
            {
                return View(socialViewModel);
            }
            
        }
    }
}