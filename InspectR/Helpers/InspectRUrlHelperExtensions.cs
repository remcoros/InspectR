namespace InspectR.Helpers
{
    public static class InspectRUrlHelperExtensions
    {
        public static string Index(this InspectRUrlHelper url)
        {
            return url.Url.Action("Index", "InspectR");
        }

        public static string Create(this InspectRUrlHelper url, bool isprivate = false)
        {
            return url.Url.Action("Create", "InspectR", new
                {
                    isprivate
                });
        }

        public static string Inspect(this InspectRUrlHelper url, string key)
        {
            return url.Url.RouteUrl("log", new { id = key }) + "?inspect";
        }
        
        public static string Log(this InspectRUrlHelper url, string key)
        {
            return url.Url.RouteUrl("log", new { id = key });
        }

        public static string FullLogUrl(this InspectRUrlHelper url, string key)
        {
            return url.Url.RouteUrl("log", new { id = key }, url.Url.RequestContext.HttpContext.Request.Url.Scheme);
        }
    }
}