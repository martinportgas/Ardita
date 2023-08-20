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

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual MstCompany Company { get; set; } = null!;

    public virtual ICollection<IdxUserArchiveUnit> IdxUserArchiveUnits { get; } = new List<IdxUserArchiveUnit>();

    public virtual ICollection<IdxUserRole> IdxUserRoles { get; } = new List<IdxUserRole>();

    public virtual ICollection<MstCreator> MstCreators { get; } = new List<MstCreator>();

    public virtual ICollection<TrxArchiveDestroy> TrxArchiveDestroys { get; } = new List<TrxArchiveDestroy>();

    public virtual ICollection<TrxArchiveExtend> TrxArchiveExtends { get; } = new List<TrxArchiveExtend>();

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovementArchiveUnitIdDestinationNavigations { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxArchiveMovement> TrxArchiveMovementArchiveUnitIdFromNavigations { get; } = new List<TrxArchiveMovement>();

    public virtual ICollection<TrxArchiveRent> TrxArchiveRents { get; } = new List<TrxArchiveRent>();

    public virtual ICollection<TrxFloor> TrxFloors { get; } = new List<TrxFloor>();

    public virtual ICollection<TrxTypeStorage> TrxTypeStorages { get; } = new List<TrxTypeStorage>();
}
