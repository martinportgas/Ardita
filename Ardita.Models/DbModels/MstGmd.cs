using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class MstGmd
{
    public Guid GmdId { get; set; }

    public string GmdCode { get; set; } = null!;

    public string GmdName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<MstGmdLog> MstGmdLogs { get; } = new List<MstGmdLog>();

    public virtual ICollection<TrxArchive> TrxArchives { get; } = new List<TrxArchive>();

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();
}
