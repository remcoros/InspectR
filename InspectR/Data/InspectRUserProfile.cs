using System;
using System.Collections.Generic;

namespace InspectR.Data
{
    public class InspectRUserProfile
    {
        public Guid Id { get; protected set; }

        public string UserName { get; protected set; }

        public IList<InspectorInfo> Inspectors { get; protected set; }

        protected InspectRUserProfile() { }

        public InspectRUserProfile(string userName)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Inspectors = new List<InspectorInfo>();
        }
    }
}