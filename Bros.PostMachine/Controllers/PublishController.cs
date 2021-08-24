using Microsoft.AspNetCore.Mvc;
using Bros.PostMachine.Models;
using Bros.PostMachine.Services;

namespace Bros.PostMachine.Controllers
{
    public class PublishController : Controller
    {
        public IActionResult Index(SocialNetWorksViewModel socialViewModel)
        {
            var vkService = new VkService();

            var success = vkService.PostOnWall(socialViewModel.ContentViewModel, socialViewModel.VkViewModel);

            if (success)
            {
                CustomSettings.Instance.VkUserId = socialViewModel.VkViewModel.UserId;
                CustomSettings.Instance.VkLogin = socialViewModel.VkViewModel.Login;
                CustomSettings.Instance.VkPassword = socialViewModel.VkViewModel.Password;
                CustomSettings.Instance.VkApplicationId = socialViewModel.VkViewModel.ApplicationId;
                CustomSettings.Instance.VkAccessToken = socialViewModel.VkViewModel.AccessToken;

                CustomSettings.Instance.Save();

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Error", "Home");
        }
    }
}