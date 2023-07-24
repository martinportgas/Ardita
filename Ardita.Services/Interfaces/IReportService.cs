using Ardita.Models.ReportModels;
using Ardita.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName);
        Task<Tuple<byte[], byte[]>> GenerateReportAsync<T>(string reportName, bool isActive, List<T> data, string dataSource, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveLoansInActive(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveProcessingInActive(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaArchiveInActive(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportListArchiveInActive(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveReceivedInActive(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportTransferMediaAsync(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveMovementAsync(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyAsync(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveDestroyInActiveAsync(string reportName, ReportGlobalParams param);
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveUsedAsync(string reportName, ReportGlobalParams param);
    }
}
