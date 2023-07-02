﻿using Ardita.Services.Interfaces;
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
            
            var data = await _reportRepository.GetArchiveActives();

            report.AddDataSource("dsArchiveActive", data);
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
    }
}
