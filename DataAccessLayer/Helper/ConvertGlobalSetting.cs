using DataAccessLayer.Enums;

namespace DataAccessLayer.Helper {
    public static class ConvertGlobalSetting {
        public static object Convert(this string stringValue, GlobalSettingTypes types) {
            switch (types) {
                case GlobalSettingTypes.Int:
                    if (int.TryParse(stringValue, out int intValue)) {
                        return intValue;
                    }
                    throw new FormatException("Invalid integer format");
                case GlobalSettingTypes.Double:
                    if (double.TryParse(stringValue, out double doubleValue)) {
                        return doubleValue;
                    }
                    throw new FormatException("Invalid double format");
                case GlobalSettingTypes.Boolean:
                    return stringValue != "0";
                    throw new FormatException("Invalid boolean format");
                case GlobalSettingTypes.String:
                    return stringValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, "Unsupported GlobalSettingType");
            }
        }

        public static List<string> ConvertStringToList(this string data) {
            if (string.IsNullOrEmpty(data)) {
                return new List<string>();
            }
            return data.Split(',').ToList();
        }

        public static string ConvertListToString(this List<string> list) {
            if (list == null || !list.Any()) {
                return string.Empty;
            }
            return string.Join(",", list);
        }


    }
}
