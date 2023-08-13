namespace Ardita.Models.ViewModels;

public class FileModel
{
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public byte[]? Content { get; set; }
    public string? Base64 { get; set; }
}
