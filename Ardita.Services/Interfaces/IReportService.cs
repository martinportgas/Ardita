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
        Task<byte[]> GenerateReportArchiveActiveAsync(string reportName);
        Task<byte[]> GenerateReportDocumentAsync(string reportName);
        Task<byte[]> GenerateReportTransferMediaAsync(string reportName);
        Task<byte[]> GenerateReportArchiveMovementAsync(string reportName);
        Task<byte[]> GenerateReportArchiveDestroyAsync(string reportName);
        Task<byte[]> GenerateReportArchiveUsedAsync(string reportName);
    }
}
