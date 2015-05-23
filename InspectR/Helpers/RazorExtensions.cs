namespace InspectR.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Web.WebPages;

    public static class RazorExtensions
    {
        public static HelperResult Join<T>(this IEnumerable<T> items, string seperator, Func<T, HelperResult> template)
        {
            return new HelperResult(writer =>
                {
                    var isFirst = true;
                    foreach (var item in items)
                    {
                        if (!isFirst)
                        {
                            writer.Write(seperator);
                        }
                        template(item).WriteTo(writer);
                        isFirst = false;
                    }
                });
        }
    }
}
