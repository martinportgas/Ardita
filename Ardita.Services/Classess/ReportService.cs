using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Collections;
using Ardita.Report;
namespace Ardita.Services.Classess
{
    public class ReportService : IReportService
    {
        private IHostingEnvironment Environment;

        public ReportService(IHostingEnvironment _environment) 
        {
            Environment = _environment;
        }
        public byte[] GenerateReportAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            LocalReport report = new LocalReport(rdlcFilePath);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            List<Archive> obj = new List<Archive>();

           
            obj.Add(new Archive { ArchiveId = "002", ArchiveTitle = "Arhive 2"});
            obj.Add(new Archive { ArchiveId = "003", ArchiveTitle = "Arhive 3"});

            report.AddDataSource("DataSet1", obj);
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
    }
}
