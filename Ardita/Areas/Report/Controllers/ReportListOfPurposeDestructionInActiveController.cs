using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportListOfPurposeDestructionInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportListOfPurposeDestructionInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptListOfPurposeDestructionInActive";
            var returnString = await _reportService.GenerateReportListOfPurposeDestructionInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
