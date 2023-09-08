namespace SqlDbEditor.RegEx
{
    public class RegExConstants
    {
        public static string NameRegEx => @"^[A-Z][a-zA-Z]*$";
        public static string AddressRegEx => @"[\a-zA-Z\s]+";
        public static string CityRegEx => @"^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$";
        public static string ZipRegEx => @"^[0-9]{5}(?:-[0-9]{4})?$";
        public static string PhoneRexEx => @"^\d{9}$";
    }
}
