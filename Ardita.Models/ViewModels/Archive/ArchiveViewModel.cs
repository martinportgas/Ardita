using Microsoft.AspNetCore.Http;

namespace Ardita.Models.ViewModels.Archive
{
    public class ArchiveViewModel
    {
        public Guid ArchiveId { get; set; }
        public string TypeSender { get; set; } = null!;
        public string Keyword { get; set; } = null!;
        public string TitleArchive { get; set; } = null!;
        public string TypeArchive { get; set; } = null!;
        public string ArchiveCreator { get; set; } = null!;
        public int Volume { get; set; }
    }
}
