﻿using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Text;

namespace Ardita.Extensions
{
    public static class Global
    {
        public static IWorkbook GetExcelTemplate(string templateName, List<DataTable> listData, string type)
        {
            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            JObject excelTemplate = GlobalConst.ExcelTemplate(templateName);
            if (excelTemplate.Count > 0)
            {
                var sheetCount = excelTemplate[type].Count();
                for (int i = 0; i < sheetCount; i++)
                {
                    ISheet excelSheetx = workbook.CreateSheet(excelTemplate[type][i.ToString()]![GlobalConst.name]!.ToString());
                    int columnCount = excelTemplate[type][i.ToString()]![GlobalConst.column]!.Count();
                    if (columnCount > 0)
                    {
                        //Create Header
                        IRow rowHeader = excelSheetx.CreateRow(0);
                        for (int c = 0; c < columnCount; c++)
                        {
                            rowHeader.CreateCell(c).SetCellValue(excelTemplate[type][i.ToString()]![GlobalConst.column]![c.ToString()]!.ToString());
                        }
                        //Create Detail
                        var dt = listData[i];
                        if (dt.Rows.Count > 0)
                        {
                            int no = 1;
                            foreach (DataRow dr in dt.Rows)
                            {
                                IRow rowData = excelSheetx.CreateRow(no);
                                for (int c = 0; c < columnCount; c++)
                                {
                                    if (c > 0)
                                        rowData.CreateCell(c).SetCellValue(dr[c].ToString());
                                    else
                                        rowData.CreateCell(c).SetCellValue(no);
                                }
                                no++;
                            }
                        }
                    }
                }
            }
            return workbook;
        }
        public static DataTable ImportExcel(IFormFile file, string folderName, string path)
        {
            var results = new DataTable();

            string newPath = Path.Combine(path, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;


                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        results.Columns.Add(cell.ToString().Replace(" ", ""), typeof(string));
                    }

                    DataRow dataRow;
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        dataRow = results.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {

                            if (row.GetCell(j) != null)
                            {
                                dataRow[j] = row.GetCell(j).ToString();

                            }
                        }
                        results.Rows.Add(dataRow.ItemArray);
                    }
                }
            }
            return results;
        }
        public static void WriteExcelToResponse(this IWorkbook book, HttpContext httpContext, string templateName)
        {
            templateName = $"{templateName}.xlsx";

            var response = httpContext.Response;
            response.ContentType = "application/vnd.ms-excel";
            if (!string.IsNullOrEmpty(templateName))
            {
                var contentDisposition = new Microsoft.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                contentDisposition.SetHttpFileName(templateName);
                response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }
            book.Write(response.Body);
        }
        public static string ToDateTimeStringNow(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddhhmmss");
        }
        public static string ToFileNameDateTimeStringNow(this string fileString, string fileName)
        {
            DateTime dateTime = DateTime.Now;
            string result = $"{fileName} - {dateTime.ToString("yyyyMMddhhmmss")}";
            return result;
        }
        public static string Encode(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string Decode(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}
