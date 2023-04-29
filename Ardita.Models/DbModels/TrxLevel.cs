using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxLevel
{
    public Guid LevelId { get; set; }

    public Guid? RackId { get; set; }

    public string? LevelCode { get; set; }

    public string? LevelName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxRack? Rack { get; set; }

    public virtual ICollection<TrxRow> TrxRows { get; } = new List<TrxRow>();
}
