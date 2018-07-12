using System.Reflection;

namespace SireusRR.Models
{
    public static class Extensions
    {
        public static T Clone<T>(this T obj) where T :class, new()
        {
            T returnValue = new T();
            PropertyInfo[] sourceProperties = obj.GetType().GetProperties();

            foreach (PropertyInfo sourceProp in sourceProperties)
            {
                if (sourceProp.CanWrite)
                {
                    sourceProp.SetValue(returnValue, sourceProp.GetValue(obj, null), null);
                }
            }
            return returnValue;
        }
    }
}
