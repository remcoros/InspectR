using System;
using System.Collections.Generic;
using System.Web;

namespace InspectR.Core
{
    public static class ContentDecoders
    {
        static ContentDecoders()
        {
            Decoders = new Dictionary<string, Func<string, string>>();

            Decoders.Add("application/x-www-form-urlencoded", HttpUtility.UrlDecode);
        }

        public static IDictionary<string, Func<string, string>> Decoders { get; set; }
    }
}