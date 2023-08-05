using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class LogActivity
{
    public Guid LogActivityId { get; set; }

    public Guid? UserId { get; set; }

    public string? Username { get; set; }

    public DateTime? ActivityDate { get; set; }

    public Guid? PageId { get; set; }

    public string? PageName { get; set; }
}
