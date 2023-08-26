using Ardita.Models.DbModels;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.Record;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Security.Claims;
using static NPOI.HSSF.Util.HSSFColor;

namespace Ardita.Extensions;

public static class Label
{
    private static readonly BksArditaDevContext _context = new BksArditaDevContext();
    public static byte[] GenerateLabelArchive(string template, TrxMediaStorage data)
    {
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace(nameof(TrxRack.RackCode), data.Row!.Level!.Rack!.RackName, false, true);
        document.Replace(nameof(TrxLevel.LevelCode), data.Row!.Level!.LevelName, false, true);
        document.Replace(nameof(TrxRow.RowCode), data.Row!.RowName, false, true);
        document.Replace(nameof(TrxSubjectClassification.SubjectClassificationCode), data.SubjectClassification.SubjectClassificationCode, false, true);

        string[] arrDate = data.ArchiveYear.Split('-');
        //document.Replace("Month", arrDate.Length > 1 ? arrDate[1] : data.CreatedDate.Month.ToString("D2"), false, true);
        document.Replace("Year", arrDate.Length > 0 ? arrDate[0] : data.ArchiveYear, false, true);

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
    public static byte[] GenerateFromTemplate(List<MstTemplateSettingDetail> settingDetails, DataTable data, string path)
    {
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(path);
        if (settingDetails.Count > 0)
        {
            foreach (var item in settingDetails)
            {
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.Text.ToString())
                    document = ReplaceText(document, item.VariableName, item.VariableData);
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.Data.ToString())
                    document = ReplaceText(document, item.VariableName, data.Rows.Count > 0 ? data.Rows[0][item.VariableData].ToString()! : "");
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.QRText.ToString())
                    document = ReplaceQR(document, item.VariableName, item.VariableData, item.Other);
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.QRData.ToString())
                    document = ReplaceQR(document, item.VariableName, data.Rows.Count > 0 ? data.Rows[0][item.VariableData].ToString()! : "", item.Other);
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.Gambar.ToString())
                    document = ReplaceImage(document, item.VariableName, item.VariableData);
                if (item.VariableType.Replace(" ", "") == GlobalConst.VariableType.DataTable.ToString())
                    document = ReplaceDataTable(document, item.VariableName, item.VariableData, data.Rows.Count > 0 ? data.Rows[0]["MainID"].ToString()! : "");
            }
        }
        using (MemoryStream memoryStream = new())
        {
            document.SaveToStream(memoryStream, FileFormat.PDF);
            toArray = memoryStream.ToArray();
        }

        return toArray;
    }
    private static Document ReplaceText(Document document, string text, string replaceText)
    {
        document.Replace(text, replaceText, false, true);
        return document;
    }
    private static Document ReplaceQR(Document document, string text, string replaceText, string other)
    {
        var size = 3;
        if (!string.IsNullOrEmpty(other))
            int.TryParse(other, out size);

        var file = QRCodeExtension.Generate(replaceText, size);
        int index = 0;
        TextRange range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString(text, true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
        }
        return document;
    }
    private static Document ReplaceImage(Document document, string text, string base64string)
    {
        int index = 0;
        TextRange range = null;
        byte[] bytes = Convert.FromBase64String(base64string);

        using (MemoryStream ms = new MemoryStream(bytes))
        {
            Image image = Image.FromStream(ms);

            TextSelection[] selections = document.FindAllString(text, true, true);
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

        return document;
    }
    private static Document ReplaceDataTable(Document document, string text, string viewName, string Id)
    {
        DataTable data = new();
        using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand($"Select * from {viewName} where ID='{Id}'", connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
            }
        }

        if (data.Rows.Count > 0)
        {
            data = data.ToCleanDataTable();

            int index = 0;
            TextRange range = null;
            Section section = document.Sections[0];
            TextSelection selection2 = document.FindString(text, true, true);
            range = selection2.GetAsOneRange();
            Paragraph paragraph = range.OwnerParagraph;
            Body body = paragraph.OwnerTextBody;
            index = body.ChildObjects.IndexOf(paragraph);

            string[] Header = data.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName.Replace("_", " "))
                                 .ToArray();

            Table table = section.AddTable(true);
            table.ResetCells(data.Rows.Count + 1, Header.Length);

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

            for (int r = 0; r < data.Rows.Count; r++)
            {
                TableRow DataRow = table.Rows[r + 1];
                DataRow.Height = 20;
                for (int c = 0; c < data.Columns.Count + 1; c++)
                {
                    if (c == 0)
                    {
                        SetRowData(c, DataRow, (r + 1).ToString());
                    }
                    else
                    {
                        SetRowData(c, DataRow, data.Rows[r][c].ToString()!);
                    }
                }
            }

            table.AutoFit(AutoFitBehaviorType.AutoFitToContents);

            body.ChildObjects.Remove(paragraph);
            body.ChildObjects.Insert(index, table);

        }
        return document;
    }
    private static DataTable ToCleanDataTable(this DataTable data)
    {
        var listRemove = new List<DataColumn>();
        foreach (DataColumn c in data.Columns)
        {
            if (c.ColumnName.Contains("ID"))
            {
                listRemove.Add(c);
            }
        }
        if (listRemove.Count > 0)
        {
            foreach (DataColumn dc in listRemove)
            {
                data.Columns.Remove(dc);
            }
        }
        return data;
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

        if (File.Exists(file))
        {
            File.Delete(file);
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
        foreach (TrxArchiveDestroyDetail item in detail)
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

        return toArray;
    }
    public static byte[] GenerateBAMovement(string template, TrxArchiveMovement data, IEnumerable<TrxArchiveMovementDetail> detail, dynamic FromData, dynamic ReceivedData)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace("[CompanyName]", data.ArchiveUnitIdFromNavigation.Company.CompanyName, false, true);
        document.Replace("[DocumentNo]", data.DocumentCode, false, true);
        document.Replace("[ApproveDate]", ((DateTime)data.UpdatedDate).ToString("dd-MM-yyyy"), false, true);
        document.Replace("[Title]", data.MovementName, false, true);
        document.Replace("[Day]", data.CreatedDate.ToString("dddd", culture), false, true);
        document.Replace("[Date]", data.CreatedDate.Day.ToString(), false, true);
        document.Replace("[Month]", data.CreatedDate.ToString("MMMM", culture), false, true);
        document.Replace("[Year]", data.CreatedDate.ToString("yyyy"), false, true);
        document.Replace("[CompanyAddress]", data.ArchiveUnitIdFromNavigation.Company.Address, false, true);
        document.Replace("[CreatedBy]", FromData.Name, false, true);
        document.Replace("[PositionName]", FromData.Position.Name, false, true);
        document.Replace("[ArchiveUnitFrom]", data.ArchiveUnitIdFromNavigation.ArchiveUnitName, false, true);
        document.Replace("[ReceivedBy]", ReceivedData.Name, false, true);
        document.Replace("[ReceivedPositionName]", ReceivedData.Position.Name, false, true);
        document.Replace("[ArchiveUnitDes]", data.ArchiveUnitIdDestinationNavigation.ArchiveUnitName, false, true);
        document.Replace("[CountArchive]", detail.Count().ToString(), false, true);

        var file = QRCodeExtension.Generate(FromData.Nik + " - " + FromData.Name, 2);
        int index = 0;
        TextRange range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRFrom", true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        file = QRCodeExtension.Generate(ReceivedData.Nik + " - " + ReceivedData.Name, 2);
        index = 0;
        range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRTo", true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
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
        foreach (TrxArchiveMovementDetail item in detail)
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

        return toArray;
    }
    public static byte[] GenerateBARent(string template, TrxArchiveRent data, IEnumerable<VwArchiveRent> detail)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);
        document.Replace("[no_ba]", data.RentCode, false, true);
        document.Replace("[dd]", ((DateTime)data.ApprovalDate).ToString("dd"), false, true);
        document.Replace("[dddd]", ((DateTime)data.ApprovalDate).ToString("dddd", culture), false, true);
        document.Replace("[ddString]", Global.Terbilang(((DateTime)data.ApprovalDate).Day), false, true);
        document.Replace("[MMString]", Global.Terbilang(((DateTime)data.ApprovalDate).Month), false, true);
        document.Replace("[dd-MM-yyyy]", ((DateTime)data.ApprovalDate).ToString("dd-MM-yyyy"), false, true);
        document.Replace("[archive_unit]", "", false, true);
        document.Replace("[company]", data.ApprovedByNavigation.Employee.Company.CompanyName, false, true);
        document.Replace("[company_address]", data.ApprovedByNavigation.Employee.Company.Address, false, true);
        document.Replace("[bor_name]", data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName, false, true);
        document.Replace("[bor_nip]", data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerIdentityNumber, false, true);
        document.Replace("[bor_pos]", data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerPosition, false, true);
        document.Replace("[bor_uker]", data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerArchiveUnit, false, true);
        document.Replace("[bor_company]", data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerCompany, false, true);
        document.Replace("[apr_name]", data.ApprovedByNavigation.Employee.Name, false, true);
        document.Replace("[apr_nip]", data.ApprovedByNavigation.Employee.Nik, false, true);
        document.Replace("[apr_pos]", data.ApprovedByNavigation.Employee.Position.Name, false, true);
        document.Replace("[apr_uker]", "", false, true);
        document.Replace("[apr_company]", data.ApprovedByNavigation.Employee.Company.CompanyName, false, true);
        document.Replace("[company_city]", data.ApprovedByNavigation.Employee.Company.City, false, true);
        document.Replace("[description]", data.Description, false, true);
        document.Replace("[dd MMMM yyyy]", ((DateTime)data.ApprovalDate).ToString("dd MMMM yyyy", culture), false, true);

        string QR1 = data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerIdentityNumber + " - " + data.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName;
        string QR2 = data.ApprovedByNavigation.Employee.Nik + " - " + data.ApprovedByNavigation.Employee.Name;
        if (QR1.Length > QR2.Length)
        {
            for (int i = 0; i < QR1.Length - QR2.Length; i++)
            {
                QR2 += " ";
            }
        }
        else
        {
            for (int i = 0; i < QR2.Length - QR1.Length; i++)
            {
                QR1 += " ";
            }
        }

        var file = QRCodeExtension.Generate(QR1, 2);
        int index = 0;
        TextRange range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QR1", true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        file = QRCodeExtension.Generate(QR2, 2);
        index = 0;
        range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QR2", true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        file = QRCodeExtension.Generate(data.TrxArchiveRentId.ToString(), 2);
        index = 0;
        range = null;

        using (Image image = Image.FromFile(file))
        {
            TextSelection[] selections = document.FindAllString("QRRent", true, true);
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

        if (File.Exists(file))
        {
            File.Delete(file);
        }

        Section section = document.Sections[0];
        TextSelection selection2 = document.FindString("[list_archive]", true, true);
        range = selection2.GetAsOneRange();
        Paragraph paragraph = range.OwnerParagraph;
        Body body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        string[] Header = { "No", "Kode Media Penyimpanan", "Urutan", "Jenis Penyimpanan", "Klasifikasi", "Judul Arsip", "Pencipta", "Unit Kearsipan" };

        Table table = section.AddTable(true);
        table.ResetCells(data.TrxArchiveRentDetails.Count() + 1, Header.Length);

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
            TR.CharacterFormat.FontSize = 8;
            TR.CharacterFormat.Bold = true;
        }

        int x = 1;
        foreach (TrxArchiveRentDetail item in data.TrxArchiveRentDetails)
        {
            TableRow DataRow = table.Rows[x];
            DataRow.Height = 20;

            SetRowData(0, DataRow, x.ToString());
            SetRowData(1, DataRow, item.MediaStorageInActive.MediaStorageInActiveCode);
            SetRowData(2, DataRow, item.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().Sort.ToString());
            SetRowData(3, DataRow, item.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().SubTypeStorage.SubTypeStorageName);
            SetRowData(4, DataRow, item.MediaStorageInActive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName);
            SetRowData(5, DataRow, item.Archive.TitleArchive);
            SetRowData(6, DataRow, item.Archive.Creator.CreatorName);
            SetRowData(7, DataRow, item.MediaStorageInActive.TypeStorage.ArchiveUnit.ArchiveUnitName);

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

        return toArray;
    }
    public static byte[] GenerateLabelInActive(string template, TrxMediaStorageInActive data, IEnumerable<TrxMediaStorageInActiveDetail> detail)
    {
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        byte[] toArray;
        Document document = new();
        document.LoadFromFile(template);

        int index = 0;
        TextRange range = null;

        Section section = document.Sections[0];
        TextSelection selection2 = document.FindString("LokasiSimpan", true, true);
        range = selection2.GetAsOneRange();
        Paragraph paragraph = range.OwnerParagraph;
        Body body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        string storageLoc = data.Row.Level.Rack.RackCode + "-" + data.Row.Level.LevelCode + data.Row.RowName;
        char[] Header = storageLoc.ToCharArray();

        Table table = section.AddTable(true);
        table.ResetCells(1, Header.Length);

        TableRow FRow = table.Rows[0];
        FRow.IsHeader = true;

        FRow.Height = 50;
        for (int i = 0; i < Header.Length; i++)
        {
            Paragraph p = FRow.Cells[i].AddParagraph();
            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            FRow.Cells[i].Width = 50;
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange TR = p.AppendText(Header[i].ToString());
            TR.CharacterFormat.FontName = "Calibri";
            TR.CharacterFormat.FontSize = 40;
            TR.CharacterFormat.Bold = true;
        }

        //table.AutoFit(AutoFitBehaviorType.AutoFitToContents);
        table.TableFormat.HorizontalAlignment = RowAlignment.Center;
        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);

        selection2 = document.FindString("KodeUker", true, true);
        range = selection2.GetAsOneRange();
        paragraph = range.OwnerParagraph;
        body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        string creator = data.SubSubjectClassification.Creator.CreatorCode;
        Header = creator.ToCharArray();

        table = section.AddTable(true);
        table.ResetCells(1, Header.Length);

        FRow = table.Rows[0];
        FRow.IsHeader = true;

        FRow.Height = 50;
        for (int i = 0; i < Header.Length; i++)
        {
            Paragraph p = FRow.Cells[i].AddParagraph();
            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            FRow.Cells[i].Width = 50;
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange TR = p.AppendText(Header[i].ToString());
            TR.CharacterFormat.FontName = "Calibri";
            TR.CharacterFormat.FontSize = 40;
            TR.CharacterFormat.Bold = true;
        }

        //table.AutoFit(AutoFitBehaviorType.AutoFitToContents);
        table.TableFormat.HorizontalAlignment = RowAlignment.Center;
        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);

        selection2 = document.FindString("NomerBerkas", true, true);
        range = selection2.GetAsOneRange();
        paragraph = range.OwnerParagraph;
        body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        var start = detail.OrderBy(x => x.Sort).FirstOrDefault();
        var end = detail.OrderByDescending(x => x.Sort).FirstOrDefault();
        string nomorberkas = start.Sort.ToString("D2") + (start == end ? start.Sort.ToString("D2") : end.Sort.ToString("D2")) + data.ArchiveYear.Substring(2, 2);
        Header = nomorberkas.ToCharArray();

        table = section.AddTable(true);
        table.ResetCells(1, Header.Length);

        FRow = table.Rows[0];
        FRow.IsHeader = true;

        FRow.Height = 50;
        for (int i = 0; i < Header.Length; i++)
        {
            Paragraph p = FRow.Cells[i].AddParagraph();
            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            FRow.Cells[i].Width = 50;
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange TR = p.AppendText(Header[i].ToString());
            TR.CharacterFormat.FontName = "Calibri";
            TR.CharacterFormat.FontSize = 40;
            TR.CharacterFormat.Bold = true;
        }

        //table.AutoFit(AutoFitBehaviorType.AutoFitToContents);
        table.TableFormat.HorizontalAlignment = RowAlignment.Center;
        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);

        selection2 = document.FindString("KodeBOX", true, true);
        range = selection2.GetAsOneRange();
        paragraph = range.OwnerParagraph;
        body = paragraph.OwnerTextBody;
        index = body.ChildObjects.IndexOf(paragraph);

        string kode = data.MediaStorageInActiveCode;
        string[] Headers = new string[] { kode };

        table = section.AddTable(true);
        table.ResetCells(1, Headers.Length);

        FRow = table.Rows[0];
        FRow.IsHeader = true;

        FRow.Height = 120;
        for (int i = 0; i < Headers.Length; i++)
        {
            Paragraph p = FRow.Cells[i].AddParagraph();
            FRow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
            FRow.Cells[i].Width = 120;
            p.Format.HorizontalAlignment = HorizontalAlignment.Center;

            TextRange TR = p.AppendText(Headers[i].ToString());
            TR.CharacterFormat.FontName = "Calibri";
            TR.CharacterFormat.FontSize = 60;
            TR.CharacterFormat.Bold = true;
        }

        table.TableFormat.HorizontalAlignment = RowAlignment.Center;
        body.ChildObjects.Remove(paragraph);
        body.ChildObjects.Insert(index, table);

        using (MemoryStream memoryStream = new())
        {
            document.SaveToStream(memoryStream, FileFormat.PDF);
            toArray = memoryStream.ToArray();
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
        TR2.CharacterFormat.FontSize = 8;
    }
}
