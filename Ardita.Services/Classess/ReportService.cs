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
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            LocalReport report = new LocalReport(rdlcFilePath);
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            List<Archive> obj = new List<Archive>();

           
            obj.Add(new Archive { ArchiveId = "002", ArchiveTitle = "Arhive 2"});
            obj.Add(new Archive { ArchiveId = "003", ArchiveTitle = "Arhive 3"});

            report.AddDataSource("DataSet1", obj);
            var result = report.Execute(RenderType.Pdf, ext, parameters);
            return result.MainStream;
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ReportGlobalParams param)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";

            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));

            var data = await _reportRepository.GetArchiveActives(param);

            var report = new LocalReport(rdlcFilePath);
            report.AddDataSource("dsArchiveActive", data);
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]> ( resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaAsync(string reportName, ReportGlobalParams param)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";

            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));

            var data = await _reportRepository.GetTransferMedias(param);

            var report = new LocalReport(rdlcFilePath);
            report.AddDataSource("dsTransferMedia", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveMovementAsync(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";

            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));

            var data = await _reportRepository.GetArchiveMovements(param);

            var report = new LocalReport(rdlcFilePath);
            report.AddDataSource("dsArchiveMovement", data);
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyAsync(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetArchiveDestroys(param);

            report.AddDataSource("dsArchiveDestroy", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyInActiveAsync(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetArchiveInActiveDestroys(param);

            report.AddDataSource("dsArchiveDestroy", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveUsedAsync(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetArchiveUseds(param);

            report.AddDataSource("dsArchiveUsed", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveLoansInActive(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetReportArchiveLoansInActive(param);

            report.AddDataSource("dsReportArchiveLoansInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveProcessingInActive(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetReportArchiveProcessingInActive(param);

            report.AddDataSource("dsReportArchiveProcessingInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaArchiveInActive(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetReportTransferMediaArchiveInActive(param);

            report.AddDataSource("dsTransferMedia", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportListArchiveInActive(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetReportListArchiveInActive(param);

            report.AddDataSource("dsReportListArchiveInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveReceivedInActive(string reportName, ReportGlobalParams param = null)
        {
            int ext = (int)(DateTime.Now.Ticks >> 10);
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{GlobalConst.ArchiveInActive}\\{reportName}.rdlc";
            var report = new LocalReport(rdlcFilePath);
            var parameters = await _reportRepository.GetGlobalParamsDescription(param, GetReportParameter(rdlcFilePath));
            var data = await _reportRepository.GetReportArchiveReceivedInActive(param);

            report.AddDataSource("dsReportArchiveReceivedInActive", data.ToList());
            var resultPdf = report.Execute(RenderType.Pdf, ext, parameters);
            var resultExcel = report.Execute(RenderType.ExcelOpenXml, ext, parameters);
            return new Tuple<byte[], byte[]>(resultPdf.MainStream, resultExcel.MainStream);
        }
    }
}
