using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
using System.Collections;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Ardita.Models.ReportModels;
using Ardita.Extensions;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;
using Ardita.Models.ViewModels;

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
        private async Task<ReportParameter[]> GetReportParameter(ReportGlobalParams param, List<string> listParameter)
        {
            ReportParameter[] par = new ReportParameter[listParameter.Count];
            if(listParameter.Count > 0 )
            {
                foreach( var item in listParameter )
                {
                    var data = await _reportRepository.GetGlobalParamsDescription(param, item);
                    par[listParameter.IndexOf(item)] = new ReportParameter(item, data);
                }
            }
            return par;
        }
        public byte[] GenerateReportAsync(string reportName)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\Report\\{reportName}.rdlc";
            LocalReport report = new LocalReport();
            report.ReportPath = rdlcFilePath;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            List<Archive> obj = new List<Archive>();


            obj.Add(new Archive { ArchiveId = "002", ArchiveTitle = "Arhive 2" });
            obj.Add(new Archive { ArchiveId = "003", ArchiveTitle = "Arhive 3" });

            report.DataSources.Add(new ReportDataSource("DataSet1", obj));
            var result = report.Render(GlobalConst.RenderPDF);
            return result;
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportAsync<T>(string reportName, bool isActive, List<T> data, string dataSource, ReportGlobalParams param)
        {
            string rdlcFilePath = $"{this.Environment.WebRootPath}\\{GlobalConst.Report}\\{(isActive ? GlobalConst.ArchiveActive : GlobalConst.ArchiveInActive)}\\{reportName}.rdlc";

            LocalReport report = new LocalReport();
            report.ReportPath = rdlcFilePath;
            report.DataSources.Add(new ReportDataSource(dataSource, data));

            var listParameter = report.GetParameters().Select(x => x.Name).ToList();
            var parameters = await GetReportParameter(param, listParameter);

            report.SetParameters(parameters);

            var resultPdf = report.Render(GlobalConst.RenderPDF);
            var resultExcel = report.Render(GlobalConst.RenderExcel);
            return new Tuple<byte[], byte[]>(resultPdf, resultExcel);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ReportGlobalParams param, SessionModel User)
        {
            var data = await _reportRepository.GetArchiveActives(param, User);

            return await GenerateReportAsync<ArchiveActive>(reportName, true, data.ToList(), "dsArchiveActive", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaAsync(string reportName, ReportGlobalParams param, SessionModel User)
        {
            var data = await _reportRepository.GetTransferMedias(param, User);

            return await GenerateReportAsync<TransferMedia>(reportName, true, data.ToList(), "dsTransferMedia", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveMovementAsync(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetArchiveMovements(param, User);

            return await GenerateReportAsync<ArchiveMovement>(reportName, true, data.ToList(), "dsArchiveMovement", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyAsync(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetArchiveDestroys(param, User);

            return await GenerateReportAsync<ArchiveDestroy>(reportName, true, data.ToList(), "dsArchiveDestroy", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyInActiveAsync(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetArchiveInActiveDestroys(param, User);

            return await GenerateReportAsync<ArchiveDestroy>(reportName, false, data.ToList(), "dsArchiveDestroy", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveUsedAsync(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetArchiveUseds(param, User);

            return await GenerateReportAsync<ArchiveUsed>(reportName, true, data.ToList(), "dsArchiveUsed", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveLoansInActive(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetReportArchiveLoansInActive(param, User);

            return await GenerateReportAsync<ReportArchiveLoansInActive>(reportName, false, data.ToList(), "dsReportArchiveLoansInActive", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveProcessingInActive(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetReportArchiveProcessingInActive(param, User);

            return await GenerateReportAsync<ReportArchiveProcessingInActive>(reportName, false, data.ToList(), "dsReportArchiveProcessingInActive", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaArchiveInActive(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetReportTransferMediaArchiveInActive(param, User);

            return await GenerateReportAsync<ReportTransferMediaArchiveInActive>(reportName, false, data.ToList(), "dsTransferMedia", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportListArchiveInActive(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetReportListArchiveInActive(param, User);

            return await GenerateReportAsync<ReportListArchiveInActive>(reportName, false, data.ToList(), "dsReportListArchiveInActive", param);
        }
        public async Task<Tuple<byte[], byte[]>> GenerateReportArchiveReceivedInActive(string reportName, ReportGlobalParams param, SessionModel User = null)
        {
            var data = await _reportRepository.GetReportArchiveReceivedInActive(param, User);

            return await GenerateReportAsync<ReportArchiveReceivedInActive>(reportName, false, data.ToList(), "dsReportArchiveReceivedInActive", param);
        }
    }
}
