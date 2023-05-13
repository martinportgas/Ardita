using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveReceived
{
    public Guid ArchiveReceivedId { get; set; }

    public Guid ArchiveMovementId { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveMovement ArchiveMovement { get; set; } = null!;
}
