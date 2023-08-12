using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class LogChange
{
    public Guid LogChangeId { get; set; }

    public Guid? UserId { get; set; }

    public string? Username { get; set; }

    public string? TableReference { get; set; }

    public string? ChangeType { get; set; }

    public DateTime? ChangeDate { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }
}
