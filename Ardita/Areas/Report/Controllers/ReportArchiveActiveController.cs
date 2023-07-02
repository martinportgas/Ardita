using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveActiveController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptArchiveActive";
            var returnString = await _reportService.GenerateReportArchiveActiveAsync(reportName);
            return File(returnString, System.Net.Mime.MediaTypeNames.Application.Octet, reportName + ".pdf");
        }
    }
}
