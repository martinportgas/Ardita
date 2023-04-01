using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstUserRole
{
    public Guid UserRoleId { get; set; }

    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual MstRole Role { get; set; } = null!;

    public virtual MstUser User { get; set; } = null!;
}
