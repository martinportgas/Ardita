using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportTransferMediaArchiveInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportTransferMediaArchiveInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptTransferMediaArchiveInActive";
            var returnString = await _reportService.GenerateReportTransferMediaArchiveInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
