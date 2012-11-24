using System.Web.Mvc;

namespace InspectR.Helpers
{
    public static class UrlHelperExtensions
    {
        public static InspectRUrlHelper InspectR(this UrlHelper url)
        {
            return new InspectRUrlHelper(url);
        }

    }

    public class InspectRUrlHelper
    {
        public UrlHelper Url { get; protected set; }

        public InspectRUrlHelper(UrlHelper url)
        {
            Url = url;
        }
    }
}