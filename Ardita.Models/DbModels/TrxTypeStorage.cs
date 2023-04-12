using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxTypeStorage
{
    public Guid TypeStorageId { get; set; }

    public string? TypeStorageCode { get; set; }

    public string? TypeStorageName { get; set; }

    public Guid? ParentId { get; set; }

    public Guid? ArchiveUnitId { get; set; }

    public int? Length { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnit { get; set; }

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();
}
