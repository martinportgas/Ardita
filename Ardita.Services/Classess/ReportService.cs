using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Collections;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Ardita.Models.ReportModels;

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
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ArchiveActiveParams param)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetArchiveActiveNamingParams(param);

            var data = await _reportRepository.GetArchiveActives(param);

            report.AddDataSource("dsArchiveActive", data);
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]> ( resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<byte[]> GenerateReportTransferMediaAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetTransferMedias();

            report.AddDataSource("dsTransferMedia", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
        public async Task<byte[]> GenerateReportArchiveMovementAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveMovements();

            report.AddDataSource("dsArchiveMovement", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
        public async Task<byte[]> GenerateReportArchiveDestroyAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveDestroys();

            report.AddDataSource("dsArchiveDestroy", data.ToList());
            var result = report.Execute(RenderType.Pdf, 1, parameters);
            return result.MainStream;
        }
        public async Task<byte[]> GenerateReportArchiveUsedAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveUseds();

            report.AddDataSource("dsArchiveUsed", data.ToList());
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
