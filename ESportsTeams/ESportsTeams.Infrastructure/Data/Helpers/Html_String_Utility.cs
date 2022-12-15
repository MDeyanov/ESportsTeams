using System.Web;

namespace ESportsTeams.Infrastructure.Data.Helpers
{
    public class Html_String_Utility
    {
        public static string EncodeProperty(string property)
        {
            return HttpUtility.HtmlEncode(property);
        }

        public static string DecodeProperty(string property)
        {
            return HttpUtility.HtmlDecode(property);
        }
    }
}
