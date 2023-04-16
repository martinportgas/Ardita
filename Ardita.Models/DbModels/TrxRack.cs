using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxRack
{
    public Guid RackId { get; set; }

    public Guid? RoomId { get; set; }

    public string? RackCode { get; set; }

    public string? RackName { get; set; }

    public int? Length { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public string? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxRoom? Room { get; set; }

    public virtual ICollection<TrxLevel> TrxLevels { get; } = new List<TrxLevel>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();

    public virtual ICollection<TrxRow> TrxRows { get; } = new List<TrxRow>();
}
