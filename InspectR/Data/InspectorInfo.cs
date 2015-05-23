namespace InspectR.Data
{
    using System;

    public class InspectorInfo
    {
        public Guid Id { get; protected set; }

        public string UniqueKey { get; protected set; }

        public string Title { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime DateCreated { get; protected set; }

        public InspectorInfo()
        {
            Id = Guid.NewGuid();
            UniqueKey = Id.ToString().GetHashCode().ToString("x");
            DateCreated = DateTime.Now;
            Title = UniqueKey;
        }
    }
}
