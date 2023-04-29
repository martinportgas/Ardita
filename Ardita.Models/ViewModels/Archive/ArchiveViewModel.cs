using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Models.ViewModels.Archive
{
    public class ArchiveViewModel
    {
        public Guid ArchiveId { get; set; }

        public Guid GmdId { get; set; }

        public Guid SubSubjectClassificationId { get; set; }

        public Guid SecurityClassificationId { get; set; }

        public Guid CreatorId { get; set; }

        public string TypeSender { get; set; } = null!;

        public string Keyword { get; set; } = null!;

        public string TitleArchive { get; set; } = null!;

        public string TypeArchive { get; set; } = null!;

        public DateTime CreatedDateArchive { get; set; }

        public int ActiveRetention { get; set; }

        public int InactiveRetention { get; set; }

        public int Volume { get; set; }

        public List<IFormFile>? Files { get; set; }
    }
}
