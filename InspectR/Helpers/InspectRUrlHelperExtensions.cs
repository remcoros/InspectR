namespace InspectR.Helpers
{
    public static class InspectRUrlHelperExtensions
    {
        public static string Index(this UrlHelperExtensions.InspectRUrlHelper url)
        {
            return url.Url.Action("Index", "InspectR");
        }

        public static string Create(this UrlHelperExtensions.InspectRUrlHelper url, bool isprivate = false)
        {
            return url.Url.Action("Create", "InspectR", new
                {
                    isprivate
                });
        }

        public static string Inspect(this UrlHelperExtensions.InspectRUrlHelper url, string key)
        {
            return url.Url.RouteUrl("log", new { id = key }) + "?inspect";
        }
        
        public static string Log(this UrlHelperExtensions.InspectRUrlHelper url, string key)
        {
            return url.Url.RouteUrl("log", new { id = key });
        }
    }
}