namespace InspectR.Data
{
    using System;
    using System.Linq;

    public static class InspectRContextQueries
    {
        public static InspectorInfo GetInspectorInfo(this InspectRContext context, Guid id)
        {
            return context.Inspectors.Find(id);
        }

        public static InspectorInfo GetInspectorInfoByKey(this InspectRContext context, string uniquekey)
        {
            return context.Inspectors.FirstOrDefault(x => x.UniqueKey == uniquekey);
        }

        public static InspectRUserProfile GetUserProfile(this InspectRContext context, string username)
        {
            return context.UserProfiles.Include("Inspectors").FirstOrDefault(x => x.UserName == username);
        }
    }
}
