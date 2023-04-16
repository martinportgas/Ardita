using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstUser
{
    public Guid UserId { get; set; }

    public Guid EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? LastLogin { get; set; }

    public DateTime? LastLogout { get; set; }

    public DateTime? PasswordLastChanged { get; set; }

    public string? PasswordLast { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual MstEmployee Employee { get; set; } = null!;

    public virtual ICollection<IdxUserRole> IdxUserRoles { get; } = new List<IdxUserRole>();
}
