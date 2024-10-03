using DataAccessLayer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
namespace EXE101_API.Controllers
{

    [ApiController]
    [Route("api/v1/")]
    [Authorize]
    public class ProvincesController : ControllerBase
    {

        private readonly IHttpClientFactory _clientFactory;
        private static readonly IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

        private readonly string GhnClientToken = config["GHN:Token"];
        private readonly string ShopId = config["GHN:ShopId"];
        private readonly string provinceApi = config["GHN:APIs:GetProvinces"];
        private readonly string districtApi = config["GHN:APIs:GetDistricts"];
        private readonly string wardApi = config["GHN:APIs:GetWards"];
        private readonly string calculateFeeApi = config["GHN:APIs:CalculateFee"];


        public ProvincesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        [Route("ghn/calculate-fee")]
        public async Task<IActionResult> GHNCalculateFee(CalculateFeeRequestDto dto)
        {

            var request = new HttpRequestMessage(HttpMethod.Post, calculateFeeApi);

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("token", GhnClientToken);
            client.DefaultRequestHeaders.Add("shop_id", ShopId);

            request.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();

                var responseObject = JsonConvert.DeserializeObject<JObject>(responseStream);

                return Ok(responseObject);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("provinces")]
        public async Task<IActionResult> GetProvinces()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, provinceApi);

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Token", GhnClientToken);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return Ok(responseStream);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("provinces/{provinceCode}/districts")]
        public async Task<IActionResult> GetDistrictsByProvinceCode([FromRoute] int provinceCode)
        {

            string getDistrictApi = $"{districtApi}{provinceCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, getDistrictApi);

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Token", GhnClientToken);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return Ok(responseStream);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("districts/{districtCode}/wards")]
        public async Task<IActionResult> GetWardsByDistrictCode([FromRoute] int districtCode)
        {

            string getWardApi = $"{wardApi}{districtCode}";

            var request = new HttpRequestMessage(HttpMethod.Get, getWardApi);

            var client = _clientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Token", GhnClientToken);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                return Ok(responseStream);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

