using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportDocumentController : Controller
    {
        private IReportService _reportService;
        public ReportDocumentController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportDocument";
            var returnString = await _reportService.GenerateReportDocumentAsync(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
