using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Shared {
    public class GetFeedbackDto {
        public string? UserImage { get; set; }
        public string? Username { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public List<string>? ListImages { get; set; }
    }

    public class GetFeedbackPaginationDto {
        public int Total { get; set; }
        public List<GetFeedbackDto>? Data { get; set; }
    }


    public class AddFeedbackDto {
        public string? UserId { get; set; }
        public int? ProductId { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
