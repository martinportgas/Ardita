using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class MstSubTypeStorage
{
    public Guid SubTypeStorageId { get; set; }

    public string SubTypeStorageCode { get; set; } = null!;

    public string SubTypeStorageName { get; set; } = null!;

    public int Volume { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<IdxSubTypeStorage> IdxSubTypeStorages { get; } = new List<IdxSubTypeStorage>();

    public virtual ICollection<MstSubTypeStorageDetail> MstSubTypeStorageDetails { get; } = new List<MstSubTypeStorageDetail>();

    public virtual ICollection<TrxMediaStorageInActiveDetail> TrxMediaStorageInActiveDetails { get; } = new List<TrxMediaStorageInActiveDetail>();
}
