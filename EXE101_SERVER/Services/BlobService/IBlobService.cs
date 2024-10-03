using Azure.Storage.Blobs;
using SixLabors.ImageSharp.Formats.Webp;

public interface IBlobService {
    Task<string> UploadFileAsync(IFormFile file);
    Task<string> UploadTempFileAsync(IFormFile file);
    Task<string> UploadFileAsync(string imageUrl);
    Task<bool> DeleteBlobsByUrlAsync(string imageUrl);
    Task<bool> DeleteBlobsByUrlAsync(string imageUrl, string thumbnailImageUrl);
}

