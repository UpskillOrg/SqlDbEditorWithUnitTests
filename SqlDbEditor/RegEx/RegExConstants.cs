namespace SqlDbEditor.RegEx
{
    public class RegExConstants
    {
        public static string NameRegEx { get; } = @"^[A-Z][a-zA-Z]*$";
        public static string AddressRegEx { get; } = @"[\a-zA-Z\s]+";
        public static string CityRegEx { get; } = @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$";
        public static string ZipRegEx { get; } = @"^[0-9]{5}(?:-[0-9]{4})?$";
        public static string PhoneRexEx { get; } = @"^\d{9}$";
    }
}
