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
        Task<byte[]> GenerateReportArchiveActiveAsync(string reportName);
        Task<byte[]> GenerateReportDocumentAsync(string reportName);
        Task<byte[]> GenerateReportArchiveLoansInActive(string reportName);
        Task<byte[]> GenerateReportArchiveProcessingInActive(string reportName);
        Task<byte[]> GenerateReportTransferMediaArchiveInActive(string reportName);
        Task<byte[]> GenerateReportListArchiveInActive(string reportName);
        Task<byte[]> GenerateReportListOfPurposeDestructionInActive(string reportName);
        Task<byte[]> GenerateReportArchiveReceivedInActive(string reportName);
    }
}
