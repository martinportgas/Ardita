using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstMenu
{
    public Guid MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string? Path { get; set; }

    public string? Icon { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ICollection<MstSubmenu> MstSubmenus { get; } = new List<MstSubmenu>();
}
