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
        public string ItemArchiveNo { get; set; }
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
    public class TransferMedia
    {
        public string Perusahaan { get; set; }
        public string AsalArsip { get; set; }
        public string PenciptaArsip { get; set; }
        public DateTime Period { get; set; }
        public string Jumlah { get; set; }
        public DateTime PeriodPindai { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NomorArsip { get; set; }
        public string TipeArsip { get; set; }
    }
    public class ArchiveMovement
    {
        public string Perusahaan { get; set; }
        public string AsalArsip { get; set; }
        public string PenciptaArsip { get; set; }
        public DateTime Period { get; set; }
        public string Jumlah { get; set; }
        public DateTime TanggalPindah { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NomorArsip { get; set; }
        public string TipeArsip { get; set; }
        public string RetensiInAktif { get; set; }
        public string KodeMediaSimpan { get; set; }
    }

    public class ArchiveDestroy
    {
        public string Perusahaan { get; set; }
        public string AsalArsip { get; set; }
        public string PenciptaArsip { get; set; }
        public DateTime Period { get; set; }
        public string Jumlah { get; set; }
        public string SifatArsip { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NomorArsip { get; set; }
        public string TipeArsip { get; set; }
        public string RetensiInAktif { get; set; }
        public string KodeMediaSimpan { get; set; }
    }

    public class ArchiveUsed
    {
        public string NoDocumen { get; set; }
        public string NoItemArsip { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string JudulArsip { get; set; }
        public string UraianInformasiArsip { get; set; }
        public DateTime Tanggal { get; set; }
        public string Jumlah { get; set; }
        public string KodeMediaSimpan { get; set; }
    }
}
