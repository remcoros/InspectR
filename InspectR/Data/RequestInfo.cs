using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;

namespace InspectR.Data
{
    public class RequestInfoEntry
    {
        private RequestInfo _request;

        public Guid Id { get; protected set; }

        public Guid InspectorId { get; protected set; }

        [StringLength(int.MaxValue)]
        public string Data { get; protected set; }

        public DateTime Created { get; protected set; }

        protected RequestInfoEntry() { }

        public RequestInfoEntry(InspectorInfo inspector, RequestInfo requestInfo)
        {
            Id = Guid.NewGuid();
            InspectorId = inspector.Id;
            Created = DateTime.Now;

            Data = new JavaScriptSerializer().Serialize(requestInfo);
        }

        public RequestInfo GetRequest()
        {
            if (_request == null)
            {
                _request = new JavaScriptSerializer().Deserialize<RequestInfo>(Data);
            }
            return _request;
        }
    }

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

        public string Content { get; set; }

        public IList<KeyValuePair<string, string>> Headers { get; set; }

        public DateTime DateCreated { get; set; }
    }
}