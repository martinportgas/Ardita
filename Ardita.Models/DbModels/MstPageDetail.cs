using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstPageDetail
{
    public Guid PageDetailId { get; set; }

    public Guid PageId { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual MstPage Page { get; set; } = null!;
}
