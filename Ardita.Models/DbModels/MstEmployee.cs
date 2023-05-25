using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstEmployee
{
    public Guid EmployeeId { get; set; }

    public string Nik { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string Gender { get; set; } = null!;

    public string? PlaceOfBirth { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? ProfilePicture { get; set; }

    public Guid? CompanyId { get; set; }

    public Guid? UnitArchiveId { get; set; }

    public Guid PositionId { get; set; }

    public string? EmployeeLevel { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual MstCompany? Company { get; set; }

    public virtual ICollection<MstUser> MstUsers { get; } = new List<MstUser>();

    public virtual MstPosition Position { get; set; } = null!;

    public virtual ICollection<TrxApproval> TrxApprovals { get; } = new List<TrxApproval>();
}
