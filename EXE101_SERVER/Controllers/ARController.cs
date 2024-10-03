using DataAccessLayer.BusinessModels;
using EXE101_API.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EXE101_API.Controllers {
    [ApiController]
    [Route("api/v1/")]
    public class ARController : ControllerBase {

        private readonly IUserContext _userContext;
        private readonly IBlobService _blobService;
        private readonly IHttpClientFactory _clientFactory;
        private static readonly IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

        private readonly string customSearchKey = config["Google:CustomSearch:Key"];
        private readonly string customSearchCx = config["Google:CustomSearch:Cx"];

        public ARController(IHttpClientFactory clientFactory, IUserContext userContext, IBlobService blobService) {
            _clientFactory = clientFactory;
            _blobService = blobService;
            _userContext = userContext;
        }

        [HttpGet("image-generate")]
        [Authorize]
        public async Task<IActionResult> GenerateSimilarImagesByQuery([FromQuery] string query) {

            var client = _clientFactory.CreateClient();

            var uri = GetGoogleSearchUri(query);

            var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri,
            };
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();
            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(result);
            var links = searchResponse.Items.Select(item => item.Link).ToList();
            return Ok(new { data = links });
        }

        [HttpPost("image-generate-by-media")]
        [Authorize]
        public async Task<IActionResult> GenerateSimilarImagesByImage([FromForm] ImageRequestDto dto) {

            var client = _clientFactory.CreateClient();

            var imageUrl = await _blobService.UploadTempFileAsync(dto.Image);

            var query = await GetQueryStringFromImageUrl(imageUrl);

            var uri = GetGoogleSearchUri(query);

            var request = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri,
            };

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            string result = await response.Content.ReadAsStringAsync();

            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(result);
            var links = searchResponse.Items.Select(item => item.Link).ToList();
            return Ok(new { data = links });
        }

        [NonAction]
        private async Task<string> GetQueryStringFromImageUrl(string url) {

            var client = _clientFactory.CreateClient();

            const string prefix = "Results for ";

            var reverseImageResultTextUri = "https://google-reverse-image-api.vercel.app/reverse";

            var uri = new Uri(reverseImageResultTextUri);

            var requestBody = JsonConvert.SerializeObject(new { imageUrl = url });

            var request = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };  

            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            var searchResponse = JsonConvert.DeserializeObject<dynamic>(result);

            string resultText = searchResponse.data.resultText;

            if (resultText.StartsWith(prefix)) {
                resultText = resultText.Substring(prefix.Length).Trim();
            }

            return resultText;
        }

        private Uri GetGoogleSearchUri(string query) {
            var result = new Uri($"https://www.googleapis.com/customsearch/v1?key={customSearchKey}&cx={customSearchCx}&q={query}&searchType=image&safe=active");
            return result;
        }
    }
}
