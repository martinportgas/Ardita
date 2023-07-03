using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveLoansInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveLoansInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptArchiveLoansInActive";
            var returnString = await _reportService.GenerateReportArchiveLoansInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
