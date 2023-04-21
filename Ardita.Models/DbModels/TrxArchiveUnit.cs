using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxArchiveUnit
{
    public Guid ArchiveUnitId { get; set; }

    public Guid CompanyId { get; set; }

    public string ArchiveUnitCode { get; set; } = null!;

    public string ArchiveUnitName { get; set; } = null!;

    public string? ArchiveUnitAddress { get; set; }

    public string? ArchiveUnitPhone { get; set; }

    public string? ArchiveUnitEmail { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCompany Company { get; set; } = null!;

    public virtual ICollection<MstCreator> MstCreators { get; } = new List<MstCreator>();

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovements { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxFloor> TrxFloors { get; } = new List<TrxFloor>();

    public virtual ICollection<TrxRoom> TrxRooms { get; } = new List<TrxRoom>();

    public virtual ICollection<TrxTypeStorage> TrxTypeStorages { get; } = new List<TrxTypeStorage>();
}
