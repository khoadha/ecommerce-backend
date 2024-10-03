using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Shared {

    public class GetStoreDto {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ImgPath { get; set; } = string.Empty;
        public string ThumbnailImgPath { get; set; } = string.Empty;
        public bool IsOpen { get; set; }
        public bool IsVerified { get; set; }
        public bool IsBanned { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; } = string.Empty;
        public string StoreEmail { get; set; } = string.Empty;
    }


    public class UpdateStoreDto {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }


    public class UpdateStoreImageDto {
        public IFormFile? Image { get; set; }
    }

    public class UpdateStoreThumbnailImageDto {
        public IFormFile? ThumbnailImage { get; set; }
    }
}
