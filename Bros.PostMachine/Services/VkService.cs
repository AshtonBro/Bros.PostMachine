using Bros.PostMachine.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using VkNet.Model;

namespace Bros.PostMachine.Services
{
    public class VkService
    {
        private static readonly VkNet.VkApi vkApi;
        static VkService()
        {
            vkApi = new VkNet.VkApi();
        }

        private bool Authorize(VkViewModel vkViewModel)
        {
            var result = ulong.TryParse(vkViewModel.ApplicationId, out var apllicationId);

            if (!result)
            {
                throw new InvalidCastException("ApplicationId must be ulong type");
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
            catch (Exception ex)
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

            var uploadServer = vkApi.Photo.GetMessagesUploadServer(384675898);

            // Загрузить картинку на сервер VK.
            var response = UploadFile(uploadServer.UploadUrl, "https://www.gstatic.com/webp/gallery/1.jpg", "jpg");

            // Сохранить загруженный файл
            var attachment = vkApi.Photo.SaveMessagesPhoto(response);

            var wallParams = new VkNet.Model.RequestParams.WallPostParams()
            {
                Attachments = attachment,
                Message = contentViewModel.Message
            };

            vkApi.Wall.Post(wallParams);

            return true;
        }

        private string UploadFile(string serverUrl, string file, string fileExtension)
        {
            // Получение массива байтов из файла
            var data = GetBytes(file);

            // Создание запроса на загрузку файла на сервер
            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                requestContent.Add(content, "file", $"file.{fileExtension}");

                var response = client.PostAsync(serverUrl, requestContent).Result;
                return Encoding.Default.GetString(response.Content.ReadAsByteArrayAsync().Result);
            }
        }

        private byte[] GetBytes(string fileUrl)
        {
            using (var webClient = new WebClient())
            {
                return webClient.DownloadData(fileUrl);
            }
        }
    }
}
