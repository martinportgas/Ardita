using System;
using System.Collections.Generic;

namespace Ardita.Models.DbModels;

public partial class LogLogin
{
    public Guid LogLoginId { get; set; }

    public Guid? UserId { get; set; }

    public string? Username { get; set; }

    public DateTime? LoginDate { get; set; }

    public string? ComputerName { get; set; }

    public string? IpAddress { get; set; }

    public string? MacAddress { get; set; }

    public string? OsName { get; set; }

    public string? BrowserName { get; set; }
}
