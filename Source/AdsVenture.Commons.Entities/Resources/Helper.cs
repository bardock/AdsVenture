using System.Text.RegularExpressions;

namespace AdsVenture.Commons.Entities.Resources
{
    public class Helper
    {
        public static string GetEntityResource(string resourceName)
        {
            var type = typeof(Commons.Entities.Resources.Entities);
            var prop = type.GetProperty(resourceName, typeof(string));

            if ((prop == null))
            {
                //If resource was not found, try removing "ID_" prefix or return resource name
                if ((resourceName.StartsWith("ID_")))
                {
                    return GetEntityResource(Regex.Replace(resourceName, "^ID_", string.Empty));
                }
                else
                {
                    return resourceName;
                }
            }

            return prop.GetValue(null).ToString();
        }
    }
}
