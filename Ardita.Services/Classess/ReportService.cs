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
        private IReportRepository _reportRepository;
        public ReportService(
            IHostingEnvironment _environment,
            IArchiveRepository archiveRepository,
            IReportRepository reportRepository
            ) 
        {
            Environment = _environment;
            _archiveRepository = archiveRepository;
            _reportRepository = reportRepository;
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
                model = new ArchiveActive();
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

        public async Task<byte[]> GenerateReportDocumentAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportDocument();

            report.AddDataSource("dsReportDocument", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportArchiveLoansInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveLoansInActive();

            report.AddDataSource("dsReportArchiveLoansInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportArchiveProcessingInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveProcessingInActive();

            report.AddDataSource("dsReportArchiveProcessingInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportTransferMediaArchiveInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportTransferMediaArchiveInActive();

            report.AddDataSource("dsReportTransferMediaArchiveInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportListArchiveInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportListArchiveInActive();

            report.AddDataSource("dsReportListArchiveInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportListOfPurposeDestructionInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportListOfPurposeDestructionInActive();

            report.AddDataSource("dsReportListOfPurposeDestructionInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }

        public async Task<byte[]> GenerateReportArchiveReceivedInActive(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveReceivedInActive();

            report.AddDataSource("dsReportArchiveReceivedInActive", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
    }
}
