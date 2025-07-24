using System.ComponentModel;

namespace Application.Extensions;

public static class EnumExtensions
{
    public static int ToInt<TEnum>(this TEnum value) where TEnum : Enum
    {
        return Convert.ToInt32(value);
    }
    

    public static TEnum ToEnum<TEnum>(this object value) where TEnum : Enum
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), value);
    }
    
    public static TEnum ToEnum<TEnum>(this int value) where TEnum : Enum
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), value);
    }
    
    public static string GetDescription(this Enum value)
    {
        if (value == null) return "";

        var result = Convert.ToString(value);
        var field = value.GetType().GetField(result);

        if (field is null) throw new ArgumentException("Value does not exists in the specified enum", nameof(value));

        return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            ? result : attribute.Description;
    }
}