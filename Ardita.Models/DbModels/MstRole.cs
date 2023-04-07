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

    public virtual ICollection<MstRolePage> MstRolePages { get; } = new List<MstRolePage>();

    public virtual ICollection<MstUserRole> MstUserRoles { get; } = new List<MstUserRole>();
}
