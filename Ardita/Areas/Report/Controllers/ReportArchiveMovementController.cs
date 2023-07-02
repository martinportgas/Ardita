using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{

    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveMovementController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveMovementController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportArchiveMovement";
            var returnString = await _reportService.GenerateReportArchiveMovementAsync(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
