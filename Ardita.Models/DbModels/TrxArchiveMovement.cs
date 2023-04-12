using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveMovement
{
    public Guid ArchiveMovementId { get; set; }

    public Guid? ArchiveUnitId { get; set; }

    public Guid? ArchiveId { get; set; }

    public DateTime? DateSchedule { get; set; }

    public DateTime? DateSend { get; set; }

    public DateTime? DateReceived { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchive? Archive { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnit { get; set; }
}
