using Ardita.Models.DbModels;

namespace Ardita.Models.ViewModels.MediaStorage;

public class MediaStorageInsertViewModel
{
    public TrxMediaStorage? MediaStorage { get; set; }
    public IEnumerable<TrxMediaStorageDetail>? MediaStorageDetail { get; set; }
}
