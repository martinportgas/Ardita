﻿namespace Ardita.Extensions;

public static class GlobalConst
{
    public const string BASE_PATH_ARCHIVE = "Path:Archive";
    public const string BASE_PATH_TEMP_ARCHIVE = "Path:TempArchive";

    public enum STATUS
    {
        Draft = 1,
        ApprovalProcess = 2,
        Approved = 3,
        Rejected = 4,
        Submit = 5
    }
}
