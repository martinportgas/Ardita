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
    public class ReportArchiveReceivedInActive
    {
        public string PemilikArsip { get; set; }
        public string AsalArsip { get; set; }
        public int Jumlah { get; set; }
        public string Pengirim { get; set; }
        public string Penerima { get; set; }
        public DateTime? Tanggal { get; set; }
        public string DokumenSerahTerima { get; set; }
        public string NoArsip { get; set; }
    }

    public class ReportArchiveProcessingInActive
    {
        public string PemilikArsip { get; set; }
        public string AsalArsip { get; set; }
        public string UnitPencipta { get; set; }
        public int Jumlah { get; set; }
        public DateTime? TanggalDiterima { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NoArsip { get; set; }
        public string JenisArsip { get; set; }
        public DateTime TahunPenciptaan { get; set; }
        public int Retensi { get; set; }
        public string Lokasi { get; set; }
        public string KodeMediaSimpan { get; set; }
        public DateTime PeriodeOlah { get; set; }
    }

    public class ReportTransferMediaArchiveInActive
    {
        public string Perusahaan { get; set; }
        public string AsalArsip { get; set; }
        public string UnitPencipta { get; set; }
        public DateTime Periode { get; set; }
        public int Jumlah { get; set; }
        public DateTime TanggalPindai { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NoArsip { get; set; }
        public string TipeArsip { get; set; }
    }

    public class ReportListArchiveInActive
    {
        public string PemilikArsip { get; set; }
        public string AsalArsip { get; set; }
        public string UnitPencipta { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NoArsip { get; set; }
        public string JenisArsip { get; set; }
        public string SubjectArsip { get; set; }
        public string DeskripsiArsip { get; set; }
        public DateTime TahunArsip { get; set; }
        public int Jumlah { get; set; }
        public int Retensi { get; set; }
        public string Lokasi { get; set; }
        public string KodeMediaSimpan { get; set; }
    }

    public class ReportListOfPurposeDestructionInActive
    {
        public string PemilikArsip { get; set; }
        public string AsalArsip { get; set; }
        public string UnitPencipta { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NoArsip { get; set; }
        public string JenisArsip { get; set; }
        public string SubjectArsip { get; set; }
        public string DeskripsiArsip { get; set; }
        public DateTime TahunArsip { get; set; }
        public int Jumlah { get; set; }
        public int Retensi { get; set; }
        public string Lokasi { get; set; }
        public string KodeMediaSimpan { get; set; }
    }

    public class ReportArchiveLoansInActive
    {
        public string PemilikArsip { get; set; }
        public string AsalArsip { get; set; }
        public string UnitPencipta { get; set; }
        public string KodeKlasifikasi { get; set; }
        public string NoArsip { get; set; }
        public string JenisArsip { get; set; }
        public DateTime Period { get; set; }
        public int Jumlah { get; set; }
        public DateTime TanggalPinjam { get; set; }
        public DateTime TanggalKembali { get; set; }
        public string NamaPeminjam { get; set; }
        public string Perusahaan { get; set; }
        public string UnitKerja { get; set; }
    }
}
