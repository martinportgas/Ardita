using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class IdxUserRole
{
    public Guid UserRoleId { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Guid? ArchiveUnitId { get; set; }

    public Guid? CreatorId { get; set; }

    public bool IsPrimary { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnit { get; set; }

    public virtual MstCreator? Creator { get; set; }

    public virtual MstRole Role { get; set; } = null!;

    public virtual MstUser User { get; set; } = null!;
}
