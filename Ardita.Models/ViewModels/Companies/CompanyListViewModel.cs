namespace Ardita.Models.ViewModels.Companies;

public class CompanyListViewModel
{
    public string? Draw { get; set; }
    public int RecordsFiltered { get; set; }
    public int RecordsTotal { get; set; }
    public List<CompanyListViewDetailModel>? Data { get; set; }
}
