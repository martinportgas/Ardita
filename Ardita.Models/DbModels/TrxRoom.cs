﻿using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class TrxRoom
{
    public Guid RoomId { get; set; }

    public Guid? ArchiveUnitId { get; set; }

    public string? RoomCode { get; set; }

    public string? RoomName { get; set; }

    public string? ArchiveRoomType { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TrxArchiveUnit? ArchiveUnit { get; set; }

    public virtual ICollection<TrxMediaStorage> TrxMediaStorages { get; } = new List<TrxMediaStorage>();

    public virtual ICollection<TrxRack> TrxRacks { get; } = new List<TrxRack>();
}