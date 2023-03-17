using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace DataTableExportService
{
    public static class ExcelExporter<T>
    {
        public static void ExportDataToExcel(List<T> result)
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var excel = new Application();
            var excelworkBook = excel.Workbooks.Add();
            var excelSheet = (Worksheet)excelworkBook.ActiveSheet;
            excelSheet.Name = "DataSheet";

            try
            {
                int col = 1;

                //create the column(s) header(s)                
                foreach (var propInfo in result[0].GetType().GetProperties())
                {
                    excelSheet.Cells[1, col] = propInfo.Name;
                    excelSheet.Cells[1, col].Font.Bold = true;
                    col++;
                }

                //put the actual data
                int fillingRow = 0;
                foreach (var item in result)
                {
                    int fillingColumn = 1;

                    foreach (var propInfo in item.GetType().GetProperties())
                    {
                        try
                        {
                            excelSheet.Cells[fillingRow + 2, fillingColumn].Value = propInfo.GetValue(item);
                            fillingColumn++;
                        }
                        catch (System.Runtime.InteropServices.COMException comex)
                        {
                            Console.WriteLine(string.Format("{0}, caused for exporting value - {1}", comex.Message, propInfo.GetValue(item)));
                            excelSheet.Cells[fillingRow + 2, fillingColumn].Value = $"'{propInfo.GetValue(item)}'";
                            fillingColumn++;
                            continue;
                        }
                    }

                    fillingRow++;
                }
            }
            catch (Exception ex)
            {
                excelworkBook.Close(false);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Export Failed.");
            }
        }
    }
}
