using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Collections;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Ardita.Models.ReportModels;
using Ardita.Extensions;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore.Internal;

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
        private List<string> GetReportParameter(string rdlcFilePath)
        {
            var reportCore = new Microsoft.Reporting.NETCore.LocalReport();
            reportCore.ReportPath = rdlcFilePath;
            return reportCore.GetParameters().Select(x => x.Name).ToList();
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
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ReportGlobalParams param)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";

            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));

            var data = await _reportRepository.GetArchiveActives(param);

            var report = new LocalReport(rdlcFilePath);
            report.AddDataSource("dsArchiveActive", data);
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]> ( resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaAsync(string reportName, ReportGlobalParams param)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";

            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));

            var data = await _reportRepository.GetTransferMedias(param);

            var report = new LocalReport(rdlcFilePath);
            report.AddDataSource("dsTransferMedia", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveMovementAsync(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveMovements();

            report.AddDataSource("dsArchiveMovement", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyAsync(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveDestroys();

            report.AddDataSource("dsArchiveDestroy", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveUsedAsync(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetArchiveUseds();

            report.AddDataSource("dsArchiveUsed", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveLoansInActive(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveLoansInActive();

            report.AddDataSource("dsReportArchiveLoansInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveProcessingInActive(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveProcessingInActive();

            report.AddDataSource("dsReportArchiveProcessingInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaArchiveInActive(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportTransferMediaArchiveInActive();

            report.AddDataSource("dsReportTransferMediaArchiveInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportListArchiveInActive(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportListArchiveInActive();

            report.AddDataSource("dsReportListArchiveInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveReceivedInActive(string reportName, ReportGlobalParams param = null)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = new Dictionary<string, string>();
            var data = await _reportRepository.GetReportArchiveReceivedInActive();

            report.AddDataSource("dsReportArchiveReceivedInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, 1, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, 1, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
    }
}
