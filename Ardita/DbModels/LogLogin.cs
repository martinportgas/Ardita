using System;
using System.Collections.Generic;

namespace Ardita.DbModels;

public partial class LogLogin
{
    public Guid LogLoginId { get; set; }

    public Guid? UserId { get; set; }

    public string? Username { get; set; }

    public DateTime? LoginDate { get; set; }

    public string? Ipaddress { get; set; }
}
