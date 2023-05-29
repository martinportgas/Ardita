using Ardita.Models.DbModels;
using Azure;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using System.Collections;
using System.Data;
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
        document.Replace(nameof(TrxSubjectClassification.SubjectClassificationCode), data.SubjectClassification.SubjectClassificationCode, false, true);
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
    public static byte[] GenerateBADestroy(string template, TrxArchiveDestroy data, IEnumerable<TrxArchiveDestroyDetail> detail, dynamic additionalData)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace("[DocumentNo]", data.DocumentCode, false, true);
        document.Replace("[Day]", data.CreatedDate.ToString("dddd", culture), false, true);
        document.Replace("[Date]", data.CreatedDate.ToString("dd"), false, true);
        document.Replace("[Month]", data.CreatedDate.ToString("MMMM", culture), false, true);
        document.Replace("[Year]", data.CreatedDate.ToString("yyyy"), false, true);
        document.Replace("[CompanyAddress]", data.ArchiveUnit.Company.Address, false, true);
        document.Replace("[ArchiveUnit]", data.ArchiveUnit.ArchiveUnitName, false, true);
        document.Replace("[CompanyName]", data.ArchiveUnit.Company.CompanyName, false, true);
        document.Replace("[DestroyCode]", data.DestroyCode, false, true);
        document.Replace("[Title]", data.DestroyName, false, true);
        document.Replace("[CreatedDate]", data.CreatedDate.ToString("dd MMMM yyyy", culture), false, true);
        document.Replace("[CreatedBy]", additionalData.Name, false, true);

        var file = QRCodeExtension.Generate(additionalData.Nik + " - " + additionalData.Name, 3);
        int index = 0;
        TextRange range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRBy", true, true);
            index = 0;
            range = null;

            foreach (TextSelection selection in selections)
            {
                DocPicture pic = new DocPicture(document);
                pic.LoadImage(image);

                range = selection.GetAsOneRange();
                index = range.OwnerParagraph.ChildObjects.IndexOf(range);
                range.OwnerParagraph.ChildObjects.Insert(index, pic);
                range.OwnerParagraph.ChildObjects.Remove(range);

            }
        }

        Section section = document.Sections[0];
        TextSelection selection2 = document.FindString("[ListArchive]", true, true);
        range = selection2.GetAsOneRange();
        Paragraph paragraph = range.OwnerParagraph;
        Body body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        string[] Header = { "No", "Nomor Arsip", "Nomor Dokumen", "Judul Arsip", "Bentuk Media Arsip", "Klasifikasi Keamanan" };

        Table table = section.AddTable(true);
        table.ResetCells(detail.Count() + 1, Header.Length);

        TableRow FRow = table.Rows[0];
        FRow.IsHeader = true;

        FRow.Height = 23;
        FRow.RowFormat.BackColor = Color.LightGray;
        for (int i = 0; i < Header.Length; i++)
        {
            Paragraph p = FRow.Cells[i].AddParagraph();
            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange TR = p.AppendText(Header[i]);
            TR.CharacterFormat.FontName = "Calibri";
            TR.CharacterFormat.FontSize = 10;
            TR.CharacterFormat.Bold = true;
        }

        int x = 1;
        foreach(TrxArchiveDestroyDetail item in detail)
        {
            TableRow DataRow = table.Rows[x];
            DataRow.Height = 20;

            SetRowData(0, DataRow, x.ToString());
            SetRowData(1, DataRow, item.Archive.ArchiveCode!);
            SetRowData(2, DataRow, item.Archive.DocumentNo!);
            SetRowData(3, DataRow, item.Archive.TitleArchive);
            SetRowData(4, DataRow, item.Archive.Gmd.GmdName);
            SetRowData(5, DataRow, item.Archive.SecurityClassification.SecurityClassificationName);

            x++;
        }

        table.AutoFit(AutoFitBehaviorType.AutoFitToContents);

        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);

        using (MemoryStream memoryStream = new())
        {
            document.SaveToStream(memoryStream, FileFormat.PDF);
            toArray = memoryStream.ToArray();
        }

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        return toArray;
    }
    private static void SetRowData(int index, TableRow dataRow, string value)
    {
        dataRow.Cells[index].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
        Paragraph p2 = dataRow.Cells[index].AddParagraph();
        TextRange TR2 = p2.AppendText(value);
        p2.Format.HorizontalAlignment = HorizontalAlignment.Center;

        TR2.CharacterFormat.FontName = "Calibri";
        TR2.CharacterFormat.FontSize = 10;
    }
}
