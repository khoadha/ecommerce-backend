namespace DataAccessLayer.Shared {
    public class CurrentUser {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public int? ManagedStoreId { get; set; }
    }
}
