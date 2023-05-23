using Ardita.Models.DbModels;
using Azure;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using System.Collections;
using System.Drawing;

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
        document.Replace(nameof(TrxSubjectClassification.SubjectClassificationCode), data.SubSubjectClassification.SubjectClassification!.SubjectClassificationCode, false, true);
        document.Replace("Month", data.CreatedDate.Month.ToString("D2"), false, true);
        document.Replace("Year", data.ArchiveYear, false, true);

        var file = QRCodeExtension.Generate(data.MediaStorageCode);

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRCode", true, true);
            int index = 0;
            TextRange range = null;

            foreach (TextSelection selection in selections)
            {
                DocPicture pic = new DocPicture(document);
                pic.LoadImage(image);

                range = selection.GetAsOneRange();
                index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                range.OwnerParagraph.ChildObjects.Insert(index, pic);
                range.OwnerParagraph.ChildObjects.Remove(range);

            }

            using (MemoryStream memoryStream = new())
            {
                document.SaveToStream(memoryStream, FileFormat.PDF);
                toArray = memoryStream.ToArray();
            }
        }

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        return toArray;
    }
}
