namespace InspectR.Helpers
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;

    public static class AnalyticsHelpers
    {
        public static HtmlString Analytics(this HtmlHelper htmlHelper, string account, string domain)
        {
            const string code = @"
        <script type=""text/javascript"">

            var _gaq = _gaq || [];
            _gaq.push(['_setAccount', '{ACCOUNT}']);
            _gaq.push(['_setDomainName', '{DOMAIN}']);
            _gaq.push(['_trackPageview']);
            (function () {
                var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
            })();
        </script>";
            return new HtmlString(code.Replace("{ACCOUNT}", account)
                .Replace("{DOMAIN}", domain));
        }

        public static HtmlString Analytics(this HtmlHelper htmlHelper)
        {
            var account = ConfigurationManager.AppSettings["ga.account"];
            var domain = ConfigurationManager.AppSettings["ga.domain"];
            if (string.IsNullOrEmpty(account))
            {
                return new HtmlString(String.Empty);
            }

            //TODO: maybe a 'ga.push' seperated by comma?
            return Analytics(htmlHelper, account, domain);
        }
    }
}
