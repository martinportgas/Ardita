using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstGmdDetail
{
    public Guid GmdDetailId { get; set; }

    public Guid GmdId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual MstGmd Gmd { get; set; } = null!;

    public virtual ICollection<MstSubTypeStorageDetail> MstSubTypeStorageDetails { get; } = new List<MstSubTypeStorageDetail>();

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovements { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxMediaStorageInActive> TrxMediaStorageInActives { get; } = new List<TrxMediaStorageInActive>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();

    public virtual ICollection<TrxTypeStorageDetail> TrxTypeStorageDetails { get; } = new List<TrxTypeStorageDetail>();
}
