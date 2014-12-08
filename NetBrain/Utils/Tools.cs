using System;
using System.Web.SessionState;

namespace NetBrain.Utils
{
    public static class Tools
    {
        /// <summary>
        /// Checks if the given object can be considered a number of some kind
        /// </summary>
        /// <param name="o">Object ot be checked</param>
        /// <returns>True or false</returns>
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        // Extension method, call for any object, eg "if (x.IsNumeric())..."
        public static bool IsNumeric(this object x) { return IsNumeric(x.GetType()); }

        // Method where you know the type of the object
        public static bool IsNumeric(Type type) { return IsNumeric(type, Type.GetTypeCode(type)); }

        // Method where you know the type and the type code of the object
        public static bool IsNumeric(Type type, TypeCode typeCode)
        {
            return (
                typeCode == TypeCode.Decimal || 
                (type.IsPrimitive && 
                typeCode != TypeCode.Object && 
                typeCode != TypeCode.Boolean && 
                typeCode != TypeCode.Char));
        }
    }
}
