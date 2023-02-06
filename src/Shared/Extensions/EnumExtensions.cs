using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#nullable disable

namespace RecipesDotNet.Shared.Extensions
{
    public static class EnumExtensions
    {
        internal static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            if (!attributes.Any())
                return null;

            return (T)attributes[0];
        }

        public static bool TryGetAttribute<T>(this Enum value, out T attribute) where T : Attribute
        {
            attribute = value.GetAttribute<T>();
            return attribute != null;
        }

        public static string GetDisplayName(this Enum value)
        {
            string name = null;

            if (value.TryGetAttribute(out DisplayNameAttribute displayNameAttr))
            {
                name = displayNameAttr.DisplayName;
            }
            else if (value.TryGetAttribute(out DisplayAttribute displayAttr))
            {
                if (!string.IsNullOrEmpty(displayAttr.Name))
                    name = displayAttr.Name;
                else if (!string.IsNullOrEmpty(displayAttr.Description))
                    name = displayAttr.Description;
            }

            return name ?? value.ToString();
        }
    }
}
