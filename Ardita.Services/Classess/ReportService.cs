using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Collections;
using Ardita.Report;
using Ardita.Repositories.Interfaces;

namespace Ardita.Services.Classess
{
    public class ReportService : IReportService
    {
        private IHostingEnvironment Environment;
        private IArchiveRepository _archiveRepository;
        public ReportService(
            IHostingEnvironment _environment,
            IArchiveRepository archiveRepository
            ) 
        {
            Environment = _environment;
            _archiveRepository = archiveRepository;
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
        public async Task<byte[]> GenerateReportArchiveActiveAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var obj = new List<ArchiveActive>();
            var model = new ArchiveActive();
            
            var data = await _archiveRepository.GetReportArchiveActive();

            foreach (var item in data)
            {
                model.DocumentNo = item.DocumentNo;
                model.ArchiveCode = item.ArchiveCode;
                model.ClassificationCode = item.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode;

                model.ArchiveTitle = item.TitleArchive;
                model.ArchiveDescription = item.ArchiveDescription;
                model.ArchiveDate = item.CreatedDate;
                model.ArchiveTotal = 1;
                model.MediaStorageCode = "";
                obj.Add(model);
            }

            report.AddDataSource("dsArchiveActive", obj);
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
    }
}
