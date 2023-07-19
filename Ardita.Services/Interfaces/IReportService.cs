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
        Task<Tuple<byte[], byte[]>> GenerateReportArchiveActiveAsync(string reportName, ArchiveActiveParams param);
        Task<byte[]> GenerateReportDocumentAsync(string reportName);
        Task<byte[]> GenerateReportArchiveLoansInActive(string reportName);
        Task<byte[]> GenerateReportArchiveProcessingInActive(string reportName);
        Task<byte[]> GenerateReportTransferMediaArchiveInActive(string reportName);
        Task<byte[]> GenerateReportListArchiveInActive(string reportName);
        Task<byte[]> GenerateReportListOfPurposeDestructionInActive(string reportName);
        Task<byte[]> GenerateReportArchiveReceivedInActive(string reportName);
        Task<byte[]> GenerateReportTransferMediaAsync(string reportName);
        Task<byte[]> GenerateReportArchiveMovementAsync(string reportName);
        Task<byte[]> GenerateReportArchiveDestroyAsync(string reportName);
        Task<byte[]> GenerateReportArchiveUsedAsync(string reportName);
    }
}
