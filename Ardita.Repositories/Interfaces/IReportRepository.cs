using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Report;

namespace Ardita.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<ReportDocument>> GetReportDocument();
    }
}
