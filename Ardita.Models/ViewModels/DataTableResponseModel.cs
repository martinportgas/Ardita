namespace Ardita.Models.ViewModels
{
    public class DataTableResponseModel<T>
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<T> data { get; set; }
        public string? ErrorMessage { get; set; } 
    }
}
