using System;

namespace InspectR.Data
{
    public class InspectorInfo
    {
        public Guid Id { get; protected set; }

        public string UniqueKey { get; protected set; }

        public bool IsPrivate { get; set; }

        public DateTime DateCreated { get; protected set; }

        public InspectorInfo()
        {
            Id = Guid.NewGuid();
            UniqueKey = Id.ToString().GetHashCode().ToString("x");
            DateCreated = DateTime.Now;
        }
    }
}