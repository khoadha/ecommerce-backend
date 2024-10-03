using DataAccessLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models {
    public class GlobalSettings {
        //public string? SectionName { get; set; }
        [Key]
        public string? SettingKey { get; set; }
        public string? SettingValue { get; set; }
        public GlobalSettingTypes SettingType { get; set; }
    }
}
