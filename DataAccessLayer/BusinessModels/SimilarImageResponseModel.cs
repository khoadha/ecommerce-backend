using Microsoft.AspNetCore.Http;
namespace DataAccessLayer.BusinessModels {
    public class SearchResult {
        public string Kind { get; set; }
        public string Title { get; set; }
        public string HtmlTitle { get; set; }
        public string Link { get; set; }
        public string DisplayLink { get; set; }
        public string Snippet { get; set; }
        public string HtmlSnippet { get; set; }
        public string Mime { get; set; }
        public string FileFormat { get; set; }
        public ImageInfo Image { get; set; }
    }

    public class ImageInfo {
        public string ContextLink { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int ByteSize { get; set; }
        public string ThumbnailLink { get; set; }
        public int ThumbnailHeight { get; set; }
        public int ThumbnailWidth { get; set; }
    }

    public class SearchResponse {
        public List<SearchResult> Items { get; set; }
    }

    public class ImageRequestDto {
        public IFormFile Image { get; set; }
    }
}
