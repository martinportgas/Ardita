using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GenerateReport()
        {
            var reportName = "RptArchive";
            var returnString = _reportService.GenerateReportAsync(reportName);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".pdf");
        }
    }
}
