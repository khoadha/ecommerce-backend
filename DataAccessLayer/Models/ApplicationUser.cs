using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models {

    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class ApplicationUser : IdentityUser {
        public string? ImgPath { get; set; }
        public int WemadePoint { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLoginTime { get; set;}
        [ForeignKey("ManagedStoreId")]
        public int? ManagedStoreId { get; set; }
        public Store? ManagedStore { get; set; }
        public bool? IsFirstTopup { get; set; }
        [ForeignKey("CartId")]
        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
        [NotMapped]
        public string? RoleName { get; set; }
    }

    public class Response {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }

    public class AuthResult {
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
    }
}