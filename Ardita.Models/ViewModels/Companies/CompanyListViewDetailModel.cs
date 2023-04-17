namespace Ardita.Models.ViewModels.Companies;

public class CompanyListViewDetailModel
{
    public Guid CompanyId { get; set; }
    public string? CompanyCode { get; set; }
    public string? CompanyName { get; set; }
    public string? Address { get; set; }
    public string? Telepone { get; set; }
    public string? Email { get; set; }
}