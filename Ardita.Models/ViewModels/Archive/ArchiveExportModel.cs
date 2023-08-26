using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Archive
{
    public class ArchiveExportModel
    {
        public string? ArchiveId { get; set; }
        public string? ArchiveCode { get; set; }
        public string? GmdName { get; set; }
        public string? SubSubjectClassificationName { get; set; }
        public string? SecurityClassificationName { get; set; }
        public string? TypeSender { get; set; }
        public string? ArchiveOwnerName { get; set; }
        public string? Keyword { get; set; }
        public string? DocumentNo { get; set; }
        public string? TitleArchive { get; set; }
        public string? ArchiveTypeName { get; set; }
        public string? CreatedDateArchive { get; set; }
        public string? ActiveRetention { get; set; }
        public string? InactiveRetention { get; set; }
        public string? Volume { get; set; }
        public string? ArchiveDescription { get; set; }
        public string? Description { get; set; }
    }
}
