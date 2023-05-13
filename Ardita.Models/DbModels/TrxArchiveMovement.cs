using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveMovement
{
    public Guid ArchiveMovementId { get; set; }

    public string? MovementCode { get; set; }

    public string? MovementName { get; set; }

    public string? Description { get; set; }

    public Guid TypeStorageId { get; set; }

    public int? TotalVolume { get; set; }

    public int? DifferenceVolume { get; set; }

    public Guid? ArchiveUnitIdDestination { get; set; }

    public Guid? ArchiveUnitIdFrom { get; set; }

    public DateTime? DateSchedule { get; set; }

    public DateTime? DateSend { get; set; }

    public DateTime? DateReceived { get; set; }

    public int? ApproveLevel { get; set; }

    public int? ApproveMax { get; set; }

    public long StatusId { get; set; }

    public string? Note { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnitIdDestinationNavigation { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnitIdFromNavigation { get; set; }

    public virtual MstStatus Status { get; set; } = null!;

    public virtual ICollection<TrxArchiveMovementDetail> TrxArchiveMovementDetails { get; } = new List<TrxArchiveMovementDetail>();

    public virtual ICollection<TrxArchiveReceived> TrxArchiveReceiveds { get; } = new List<TrxArchiveReceived>();

    public virtual TrxTypeStorage TypeStorage { get; set; } = null!;
}
