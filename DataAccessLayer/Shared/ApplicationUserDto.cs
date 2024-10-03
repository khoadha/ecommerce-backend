using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Shared {
    public class ApplicationUserDto {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleName { get; set; }
        public int? ManagedStoreId { get; set; }
        public string? ImgPath { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
}
