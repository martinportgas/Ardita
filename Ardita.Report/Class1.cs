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
    public class ReportDocument
    { 
        public string Location { get; set; }
        public string Ruangan { get; set; }
        public string AsalArsip { get; set; }
        public string Klasifikasi { get; set; }
        public string Keamanan { get; set; }
        public string GMD { get; set; }
        public DateTime PeriodPenciptaan { get; set; }
        public DateTime PeriodInput { get; set; }
    }
}
