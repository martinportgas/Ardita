using Ardita.Models.DbModels;
using Azure;
using Spire.Doc;
using Spire.Doc.Interface;

namespace Ardita.Extensions;

public static class Label
{
    public static byte[] GenerateLabelArchive(string template, TrxMediaStorage data)
    {
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace("RackCode", data.Row!.Level!.Rack!.RackCode, false, true);
        document.Replace("LevelCode", data.Row!.Level!.LevelCode, false, true);
        document.Replace("RowCode", data.Row!.RowCode, false, true);
        document.Replace("ArchiveUnitCode", data.TypeStorage!.ArchiveUnit!.ArchiveUnitCode, false, true);

        using (MemoryStream ms1 = new())
        {
            document.SaveToStream(ms1, FileFormat.PDF);
            toArray = ms1.ToArray();
        }
        return toArray;
    }
}
