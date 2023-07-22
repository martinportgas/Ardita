using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ReportModels;
using Ardita.Report;

namespace Ardita.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<Dictionary<string, string>> GetGlobalParamsDescription(ReportGlobalParams param, List<string> listParameter);
        Task<IEnumerable<ReportDocument>> GetReportDocument(ReportGlobalParams param = null);
        Task<Dictionary<string, string>> GetArchiveActiveNamingParams(ReportGlobalParams param);
        Task<IEnumerable<ArchiveActive>> GetArchiveActives(ReportGlobalParams param);
        Task<IEnumerable<TransferMedia>> GetTransferMedias(ReportGlobalParams param);
        Task<IEnumerable<ArchiveMovement>> GetArchiveMovements(ReportGlobalParams param = null);
        Task<IEnumerable<ArchiveDestroy>> GetArchiveDestroys(ReportGlobalParams param = null);
        Task<IEnumerable<ArchiveUsed>> GetArchiveUseds(ReportGlobalParams param = null);
        Task<IEnumerable<ReportArchiveReceivedInActive>> GetReportArchiveReceivedInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ReportArchiveLoansInActive>> GetReportArchiveLoansInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ReportArchiveProcessingInActive>> GetReportArchiveProcessingInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ReportTransferMediaArchiveInActive>> GetReportTransferMediaArchiveInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ReportListArchiveInActive>> GetReportListArchiveInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ReportListOfPurposeDestructionInActive>> GetReportListOfPurposeDestructionInActive(ReportGlobalParams param = null);
        Task<IEnumerable<ArchiveDestroy>> GetArchiveInActiveDestroys(ReportGlobalParams param = null);
    }
}