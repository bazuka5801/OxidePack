using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMultipartParser;
using RustyCode.Core.Parsers;
using RustyCode.WebAPI.Repository;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Constants;
using Unosquare.Labs.EmbedIO.Modules;

namespace RustyCode.WebAPI.Controllers
{
    public class PluginController : WebApiController
    {
        public PluginController(IHttpContext context) : base(context)
        {
        }
        
        // You need to include the WebApiHandler attribute to each method
        // where you want to export an endpoint. The method should return
        // bool or Task<bool>.
        [WebApiHandler(HttpVerbs.Get, "/api/")]
        public async Task<bool> ApiTest()
        {
            try
            {
                return await this.StringResponseAsync("It is working!!!");
            }
            catch (Exception ex)
            {
                return await this.JsonExceptionResponseAsync(ex);
            }
        }

        [WebApiHandler(HttpVerbs.Post, "/plugin/upload/")]
        public async Task<bool> Upload()
        {
            if (Request.Headers.Get("Key") != "E2F8C8B6EF65928BD2F3B413AC879")
            {
                return await Error("Access denied!");
            }

            try
            {

                using (var ms = new MemoryStream())
                {
                    var parser = new MultipartFormDataParser(Request.InputStream, Encoding.UTF8);
                    var file = parser.Files.First();
                    file.Data.CopyTo(ms);

                    var pluginParser = PluginParser.Create(ms.ToArray());
                    var pluginMeta = pluginParser.Meta;
                    var uploadResult = PluginRepository.Upload(pluginMeta, ms.ToArray());
                    if (uploadResult.success)
                    {
                        return await Message(uploadResult.message);
                    }
                    else
                    {
                        return await Error(uploadResult.message);
                    }
                }
            }
            catch (Exception ex)
            {
                return (await this.StringResponseAsync("Error: " + ex.Message));
            }
        }
        private Task<bool> Error(string message)
        {
            return this.JsonResponseAsync(new { error = message });
        }
        private Task<bool> Message(string message)
        {
            return this.JsonResponseAsync(new { message = message });
        }
    }
}