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
    public static byte[] GenerateBADestroy(string template, TrxArchiveDestroy data, dynamic additionalData)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace("[DocumentNo]", data.DocumentCode, false, true);
        document.Replace("[Date]", data.CreatedDate.ToString("dddd", culture), false, true);
        document.Replace("[Month]", data.CreatedDate.ToString("MMMM", culture), false, true);
        document.Replace("[Year]", data.CreatedDate.ToString("yyyy"), false, true);
        document.Replace("[CompanyAddress]", data.ArchiveUnit.Company.Address, false, true);
        document.Replace("[ArchiveUnit]", data.ArchiveUnit.ArchiveUnitName, false, true);
        document.Replace("[CompanyName]", data.ArchiveUnit.Company.CompanyName, false, true);
        document.Replace("[DestroyCode]", data.DestroyCode, false, true);
        document.Replace("[Title]", data.DestroyName, false, true);
        document.Replace("[CreatedDate]", data.CreatedDate.ToString("dddd MMMM yyyy", culture), false, true);
        document.Replace("[CreatedBy]", additionalData.Name, false, true);

        var file = QRCodeExtension.Generate(additionalData.Nik + " - " + additionalData.Name, 1);

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRBy", true, true);
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
