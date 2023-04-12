using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstRole
{
    public Guid RoleId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<IdxRolePage> IdxRolePages { get; } = new List<IdxRolePage>();

    public virtual ICollection<IdxUserRole> IdxUserRoles { get; } = new List<IdxUserRole>();
}
