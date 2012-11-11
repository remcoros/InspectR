using System.IO;
using System.Web;
using InspectR.Data;

namespace InspectR.Core.RequestLogger
{
    public class DefaultRequestCollector : IRequestCollector
    {
 
        public DefaultRequestCollector()
        {
        }

        public void Collect(RequestInfo info, InspectorInfo inspector)
        {
            var context = HttpContext.Current;
            var req = context.Request;

            info.Client.HostAddress = req.UserHostAddress;
            info.Client.HostName = req.UserHostName;
            info.Client.Agent = req.UserAgent;
            info.Client.Languages = req.UserLanguages;

            info.ContentLength = req.ContentLength;
            info.ContentType = req.ContentType;
            
            // info.Cookies = req.Cookies;
            // info.Form = req.Form;
            info.Headers = req.Headers.AsKeyValuePairList();

            info.HttpMethod = req.HttpMethod;
            info.RequestType = req.RequestType;

            info.RawUrl = req.RawUrl;
            info.QueryString = req.Url.Query;

            if (req.UrlReferrer != null)
            {
                info.UrlReferrer = req.UrlReferrer.ToString();
            }

            // TODO: nicer way of getting body?
            req.InputStream.Position = 0;
            using (var rdr = new StreamReader(req.InputStream))
            {
                info.Content = rdr.ReadToEnd();
            }
            if (ContentDecoders.Decoders.ContainsKey(req.ContentType))
            {
                var contentDecoder = ContentDecoders.Decoders[req.ContentType];
                if (contentDecoder != null)
                {
                    info.Content = contentDecoder(info.Content);
                }                
            }
        }
    }
}