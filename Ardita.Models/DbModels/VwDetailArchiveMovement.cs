using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class VwDetailArchiveMovement
{
    public int No { get; set; }

    public Guid DetailId { get; set; }

    public Guid Id { get; set; }

    public Guid ArchiveId { get; set; }

    public string JudulArsip { get; set; } = null!;

    public string? NoDokumen { get; set; }

    public string? TanggalPenciptaan { get; set; }

    public string UnitPencipta { get; set; } = null!;

    public string AsalArsip { get; set; } = null!;
}
