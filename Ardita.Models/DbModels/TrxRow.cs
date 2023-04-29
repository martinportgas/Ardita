using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxRow
{
    public Guid RowId { get; set; }

    public Guid? LevelId { get; set; }

    public string? RowCode { get; set; }

    public string? RowName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxLevel? Level { get; set; }

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();
}
