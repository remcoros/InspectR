using System.Collections.Generic;
using System.Collections.Specialized;

namespace InspectR.Core
{
    internal static class NameValueCollectionExtensions
    {
        public static IList<KeyValuePair<string, string>> AsKeyValuePairList(this NameValueCollection nvc)
        {
            var list = new List<KeyValuePair<string, string>>();
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
}