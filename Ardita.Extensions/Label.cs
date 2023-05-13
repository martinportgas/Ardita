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
        document.Replace(nameof(TrxRack.RackCode), data.Row!.Level!.Rack!.RackCode, false, true);
        document.Replace(nameof(TrxLevel.LevelCode), data.Row!.Level!.LevelCode, false, true);
        document.Replace(nameof(TrxRow.RowCode), data.Row!.RowCode, false, true);
        document.Replace(nameof(TrxArchiveUnit.ArchiveUnitCode), data.TypeStorage!.ArchiveUnit!.ArchiveUnitCode, false, true);

        using (MemoryStream memoryStream = new())
        {
            document.SaveToStream(memoryStream, FileFormat.PDF);
            toArray = memoryStream.ToArray();
        }
        return toArray;
    }
}
