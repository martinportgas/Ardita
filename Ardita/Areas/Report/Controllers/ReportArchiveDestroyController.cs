using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveDestroyController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveDestroyController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportArchiveDestroy";
            var returnString = await _reportService.GenerateReportArchiveDestroyAsync(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
