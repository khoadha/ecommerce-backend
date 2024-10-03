using AutoMapper;
using DataAccessLayer.Constants;
using DataAccessLayer.Models;
using DataAccessLayer.Shared;
using EXE101_API.Services.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EXE101_API.Controllers {
    [ApiController]
    [Route("api/v1/")]
    [Authorize(Roles = AppRole.ADMIN)]
    public class ReportsController : ControllerBase {

        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportsController(IReportService reportService, IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReports([FromQuery] int? count) {
            var response = await _reportService.GetReports(count);
            return Ok(response.Data);
        }

        [HttpGet("report/{id}")]
        public async Task<IActionResult> GetReportById([FromRoute] int id) {
            var response = await _reportService.GetReportById(id);
            return Ok(response.Data);
        }

        [HttpPost("report")]
        [AllowAnonymous]
        public async Task<IActionResult> AddReport([FromBody] AddReportDto dto) {
            var report = _mapper.Map<Report>(dto);
            await _reportService.AddReport(report);
            return NoContent();
        }

        [HttpPut("report")]
        public async Task<IActionResult> UpdateReportStatus([FromQuery] int reportId, [FromQuery] string staffId) {
            await _reportService.CompleteReportStatus(reportId, staffId);
            return NoContent();
        }
    }
}
