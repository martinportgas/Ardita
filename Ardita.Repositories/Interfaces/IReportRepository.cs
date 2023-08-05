using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ReportModels;
using Ardita.Models.ViewModels;
using Ardita.Report;

namespace Ardita.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<string> GetGlobalParamsDescription(ReportGlobalParams param, string item);
        Task<IEnumerable<ReportDocument>> GetReportDocument(ReportGlobalParams param, SessionModel User);
        Task<Dictionary<string, string>> GetArchiveActiveNamingParams(ReportGlobalParams param);
        Task<IEnumerable<ArchiveActive>> GetArchiveActives(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<TransferMedia>> GetTransferMedias(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ArchiveMovement>> GetArchiveMovements(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ArchiveDestroy>> GetArchiveDestroys(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ArchiveUsed>> GetArchiveUseds(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportArchiveReceivedInActive>> GetReportArchiveReceivedInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportArchiveLoansInActive>> GetReportArchiveLoansInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportArchiveProcessingInActive>> GetReportArchiveProcessingInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportTransferMediaArchiveInActive>> GetReportTransferMediaArchiveInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportListArchiveInActive>> GetReportListArchiveInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ReportListOfPurposeDestructionInActive>> GetReportListOfPurposeDestructionInActive(ReportGlobalParams param, SessionModel User);
        Task<IEnumerable<ArchiveDestroy>> GetArchiveInActiveDestroys(ReportGlobalParams param, SessionModel User);
    }
}