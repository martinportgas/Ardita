using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxTypeStorage
{
    public Guid TypeStorageId { get; set; }

    public string TypeStorageCode { get; set; } = null!;

    public string TypeStorageName { get; set; } = null!;

    public Guid ArchiveUnitId { get; set; }

    public int Volume { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit ArchiveUnit { get; set; } = null!;

    public virtual ICollection<IdxSubTypeStorage> IdxSubTypeStorages { get; } = new List<IdxSubTypeStorage>();

    public virtual ICollection<TrxMediaStorageInActive> TrxMediaStorageInActives { get; } = new List<TrxMediaStorageInActive>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();

    public virtual ICollection<TrxTypeStorageDetail> TrxTypeStorageDetails { get; } = new List<TrxTypeStorageDetail>();
}
