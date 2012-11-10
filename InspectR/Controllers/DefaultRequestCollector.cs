using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using InspectR.Data;

namespace InspectR.Controllers
{
    public static class NameValueCollectionExtensions
    {
        public static IList<KeyValuePair<String, String>> AsKeyValuePairList(this NameValueCollection nvc)
        {
            var list = new List<KeyValuePair<String, String>>();
            for (int i = 0; i < nvc.Keys.Count; i++)
            {
                var key = nvc.GetKey(i);
                var values = nvc.GetValues(i);
                if (values != null)
                {
                    foreach (var value in values)
                    {
                        list.Add(new KeyValuePair<string, string>(key, value));
                    }                    
                }
            }
            return list;
        }
    }

    public static class ContentDecoders
    {
        static ContentDecoders()
        {
            Decoders = new Dictionary<string, Func<string, string>>();

            Decoders.Add("application/x-www-form-urlencoded", HttpUtility.UrlDecode);
        }

        public static IDictionary<string, Func<string, string>> Decoders { get; set; }
    }

    public class DefaultRequestCollector : IRequestCollector
    {
 
        public DefaultRequestCollector()
        {
        }

        public void Collect(RequestInfo info, InspectorInfo inspector, HttpContextBase controller)
        {
            var req = controller.Request;

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
            if (req.Url != null)
            {
                info.QueryString = req.Url.Query;
            }

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