using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using VkNet.Model;
using Bros.PostMachine.Helpers;
using Bros.PostMachine.Models;

namespace Bros.PostMachine.Services
{
    public class VkService
    {
        private static readonly VkNet.VkApi vkApi;
        static VkService()
        {
            vkApi = new VkNet.VkApi();
        }

        private static bool Authorize(VkViewModel vkViewModel)
        {
            var result = ulong.TryParse(vkViewModel.ApplicationId, out var apllicationId);

            if (!result)
            {
                return false;
            }

            var autParams = new ApiAuthParams()
            {
                AccessToken = vkViewModel.AccessToken,
                ApplicationId = apllicationId,
                Login = vkViewModel.Login,
                Password = vkViewModel.Password
            };

            try
            {
                if (!vkApi.IsAuthorized)
                {
                    vkApi.Authorize(autParams);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PostOnWall(ContentViewModel contentViewModel, VkViewModel vkViewModel)
        {
            if (!Authorize(vkViewModel))
            {
                return false;
            }

            try
            {
                var wallParams = new VkNet.Model.RequestParams.WallPostParams();

                if (contentViewModel.Image != null)
                {
                    var uploadServer = vkApi.Photo.GetWallUploadServer();

                    var extension = Path.GetExtension(contentViewModel.Image.FileName);

                    // Загрузить картинку на сервер VK.
                    var response = UploadFile(uploadServer.UploadUrl, contentViewModel.Image, extension);

                    // Сохранить загруженный файл
                    var attachment = vkApi.Photo.SaveWallPhoto(response, vkViewModel.UserId);

                    wallParams.Attachments = attachment;
                }

                wallParams.Message = contentViewModel.Message;

                vkApi.Wall.Post(wallParams);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string UploadFile(string serverUrl, IFormFile file, string fileExtension)
        {
            var data = file.GetBytes();

            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();

                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                requestContent.Add(content, "file", $"file{fileExtension}");

                var response = client.PostAsync(serverUrl, requestContent).Result;

                return Encoding.Default.GetString(response.Content.ReadAsByteArrayAsync().Result);
            }
        }

    }
}
