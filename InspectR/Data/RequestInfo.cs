using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace InspectR.Data
{
    [Serializable]
    public class RequestInfo
    {
        public RequestInfo()
        {
            Client = new ClientRequestInfo();
            DateCreated = DateTime.Now;
        }

        public ClientRequestInfo Client { get; set; }

        public int ContentLength { get; set; }

        public string ContentType { get; set; }

        public string HttpMethod { get; set; }

        public string RequestType { get; set; }

        public string RawUrl { get; set; }

        public string QueryString { get; set; }

        public string UrlReferrer { get; set; }

        public string RawContent { get; set; }
        
        public string Content { get; set; }

        public string Protocol { get; set; }

        public string RawRequest { get; set; }

        public IList<KeyValuePair<string, string>> Headers { get; set; }

        public DateTime DateCreated { get; set; }
    }
}