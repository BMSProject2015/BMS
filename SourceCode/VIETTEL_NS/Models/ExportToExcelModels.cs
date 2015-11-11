using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Office;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using VIETTEL.Controllers;
namespace VIETTEL.Models
{
    public class ExportToExcelModels
    {
        public static void CopyFile(String Path1, String Path2)
        {
            // Create the file and clean up handles.
            // FileStream fs = File.Create(Path1);

            // Ensure that the target does not exist.
            // Delete a file by using File class static method...
            if (File.Exists(Path2))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    File.Delete(Path2);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
            // Try to copy the same file again, which should succeed.
            File.Copy(Path1, Path2, true);
        }
        public static clsExcelResult exportDataToExcel(string tieude, DataTable dt, String FileName, String[] ColName)
        {
            clsExcelResult excelResult = new clsExcelResult();
            //khoi tao cac doi tuong Com Excel de lam viec
            Excel.Application xlApp;
            Excel.Worksheet xlSheet;
            Excel.Workbook xlBook;
            xlApp = new Excel.Application();
            //doi tuong Trống để thêm  vào xlApp sau đó lưu lại sau
            object missValue = System.Reflection.Missing.Value;
            //khoi tao doi tuong Com Excel moi            
            xlBook = xlApp.Workbooks.Add(missValue);

            //su dung Sheet dau tien de thao tac
            xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            //không cho hiện ứng dụng Excel lên để tránh gây đơ máy
            xlApp.Visible = false;
            int socot = dt.Columns.Count;
            int sohang = dt.Rows.Count;
            int i, j;

            //set thuoc tinh cho tieu de
            xlSheet.get_Range("A1", Convert.ToChar(socot + 65) + "1").Merge(false);
            Excel.Range caption = xlSheet.get_Range("A1", Convert.ToChar(socot + 65) + "1");
            caption.Select();
            caption.FormulaR1C1 = tieude;
            //căn lề cho tiêu đề
            caption.HorizontalAlignment = Excel.Constants.xlCenter;
            caption.Font.Bold = true;
            caption.VerticalAlignment = Excel.Constants.xlCenter;
            caption.Font.Size = 15;
            //màu nền cho tiêu đề
            caption.Interior.ColorIndex = 20;
            caption.RowHeight = 30;
            //set thuoc tinh cho cac header
            Excel.Range header = xlSheet.get_Range("A2", Convert.ToChar(socot + 65) + "2");
            header.Select();

            header.HorizontalAlignment = Excel.Constants.xlCenter;
            header.Font.Bold = true;
            header.Font.Size = 10;
            //điền tiêu đề cho các cột trong file excel
            for (i = 0; i < socot; i++)
                xlSheet.Cells[2, i + 2] = ColName[i];
            //dien cot stt
            xlSheet.Cells[2, 1] = "STT";
            for (i = 0; i < sohang; i++)
                xlSheet.Cells[i + 3, 1] = i + 1;
            //dien du lieu vao sheet

            for (i = 0; i < sohang; i++)
                for (j = 0; j < socot; j++)
                {
                    xlSheet.Cells[i + 3, j + 2] = Convert.ToString(dt.Rows[i][j]);

                }
            //autofit độ rộng cho các cột 
            for (i = 0; i < sohang; i++)
                ((Excel.Range)xlSheet.Cells[1, i + 1]).EntireColumn.AutoFit();



            //save file
            xlBook.SaveAs("C:\\Users\\Tuan\\Downloads\\" + FileName, Excel.XlFileFormat.xlWorkbookNormal, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
            xlBook.Close(true, missValue, missValue);
            xlApp.Quit();

            // release cac doi tuong COM
            releaseObject(xlSheet);
            releaseObject(xlBook);
            releaseObject(xlApp);
            excelResult.FileName = FileName;
            excelResult.Path = "C:\\Users\\Tuan\\Downloads\\";
            return excelResult;
        }
        static public void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }

}