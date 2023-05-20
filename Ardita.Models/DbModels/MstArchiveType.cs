using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstArchiveType
{
    public Guid ArchiveTypeId { get; set; }

    public string ArchiveTypeCode { get; set; } = null!;

    public string ArchiveTypeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();
}
