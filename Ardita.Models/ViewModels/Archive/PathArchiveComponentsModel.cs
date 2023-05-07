namespace Ardita.Models.ViewModels.Archive;

public struct PathArchiveComponentsModel
{
    public PathArchiveComponentsModel()
    {
    }

    public string CompanyCode { get; set; } = null!;
    public string Year { get; set; } = null!;
    public string Month { get; set; } = null!;
    public string ArchiveUnitCode { get; set; } = null!;
    public string CreatorCode { get; set; } = null!;
    public string SubSubjectClassificationCode { get; set; } = null!;
}
