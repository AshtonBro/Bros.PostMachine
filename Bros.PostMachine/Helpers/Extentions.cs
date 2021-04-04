using Microsoft.AspNetCore.Http;
using System.IO;

namespace Bros.PostMachine.Helpers
{
    public static class Extentions
    {
        public static byte[] GetBytes(this IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);

                    var fileBytes = ms.ToArray();

                    return fileBytes;
                }
            }

            return new byte[] { };
        }
    }
}
