using System.ComponentModel;

namespace Pipes.Net
{
    public static class TypeConverter
    {
        public static T Convert<T>(object? input)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));            
            return (T)converter.ConvertTo(input, typeof(T));
        }
    }




}