using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using InspectR.Core;
using InspectR.Data;
using InspectR.Hubs;
using Microsoft.AspNet.SignalR;

namespace InspectR.Controllers
{
    public class DefaultInspectRService : IInspectRService
    {
        public DefaultInspectRService()
        {
        }

        public InspectorInfo CreateInspector(bool isprivate)
        {
            var inspector = new InspectorInfo()
                {
                    IsPrivate = isprivate
                };

            using (var context = new InspectRContext())
            {
                context.Inspectors.Add(inspector);
                context.SaveChanges();
            }

            return inspector;
        }

        public InspectorInfo GetInspectorInfo(Guid id)
        {
            using (var context = new InspectRContext())
            {
                return context.Inspectors.Find(id);
            }            
        }

        public InspectorInfo GetInspectorInfoByKey(string uniquekey)
        {
            using (var context = new InspectRContext())
            {
                return context.Inspectors.FirstOrDefault(x => x.UniqueKey == uniquekey);
            }
        }
    }
}