using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveUsedController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveUsedController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportArchiveUsed";
            var returnString = await _reportService.GenerateReportArchiveUsedAsync(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
