using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxFloor
{
    public Guid FloorId { get; set; }

    public Guid? ArchiveUnitId { get; set; }

    public string? FloorCode { get; set; }

    public string? FloorName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnit { get; set; }
}
