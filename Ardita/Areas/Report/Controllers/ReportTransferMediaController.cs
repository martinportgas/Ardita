using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportTransferMediaController : Controller
    {
        private IReportService _reportService;
        public ReportTransferMediaController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportTransferMedia";
            var returnString = await _reportService.GenerateReportTransferMediaAsync(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
