using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace AdsVenture.Presentation.ContentServer.Helpers.Files
{
    public class ApiFileReader
    {
        public class Data
        {
            public string Filename { get; set; }
            public byte[] Bytes { get; set; }
        }

        public static async Task<Data> Read(HttpContent content)
        {
            // Check if the request contains multipart/form-data.
            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            //Read multipart data
            var provider = new MultipartMemoryStreamProvider();
            await content.ReadAsMultipartAsync(provider);

            if (!provider.Contents.Any())
            {
                return null;
            }

            var stream = await provider.Contents.Last().ReadAsStreamAsync();

            //Read bytes
            byte[] bytes;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                bytes = ms.ToArray();
            }

            var filename = provider.Contents.Last().Headers.ContentDisposition.FileName.Trim(new char[] { '"' });

            return new Data
            {
                Bytes = bytes,
                Filename = filename
            };
        }

    }
}