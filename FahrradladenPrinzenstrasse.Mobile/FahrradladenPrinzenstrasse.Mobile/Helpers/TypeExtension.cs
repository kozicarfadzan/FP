using System;
using System.Collections.Generic;
using System.Text;

namespace FahrradladenPrinzenstrasse.Mobile.Helpers
{
    public static class TypeExtension
    {
        public static bool IsNullableEnum(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
        public static bool IsNullable(this Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null);
        }
        public static bool IsGenericList(this Type t, out Type listType)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition( )== typeof(List<>))
            {
                listType = t.GetGenericArguments()[0];
                return true;
            }

            listType = null;
            return false;
        }
    }
}
