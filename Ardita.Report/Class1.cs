using System;

namespace Ardita.Report
{
    public class Archive
    {
        public string ArchiveId { get; set; }
        public string ArchiveTitle { get; set; }
    }

    public class ArchiveActive 
    { 
        public string DocumentNo { get; set; }
        public string ArchiveCode { get; set; }
        public string ClassificationCode { get; set; }
        public string ArchiveTitle { get; set; }
        public string ArchiveDescription { get; set; }
        public DateTime ArchiveDate { get; set; }
        public int ArchiveTotal { get; set; }
        public string MediaStorageCode { get; set; }
    }
}
