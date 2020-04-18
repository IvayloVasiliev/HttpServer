using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Results;

namespace IRunes.App.Controlers
{
    public class InfoController : Controller
    {
        public ActionResult Json(IHttpRequest request)
        {
            return Json( new { });
        }

        public IHttpResponse About(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public ActionResult File(IHttpRequest request)
        {
            string folderPrefix = "/../";
            string assemblyLocation = this.GetType().Assembly.Location;
            string resourceFolderPath = "Resources/";
            string requestedResource = request.QueryData["file"].ToString();

            string fullPathToResource = assemblyLocation + folderPrefix
                                                + resourceFolderPath + requestedResource;

            if (System.IO.File.Exists(fullPathToResource))
            {
                byte[] content = System.IO.File.ReadAllBytes(fullPathToResource);
                return File(content);
            }

            return NotFound();
        }

    }
}
