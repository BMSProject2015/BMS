using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Controls;
using DomainModel;
using VIETTEL.Models;
using System.Diagnostics;
using System.Reflection;
namespace VIETTEL.Controllers
{
    public class DuToan_ExportToExcelController : Controller
    {

        public clsExcelResult PhanBoDuToanNganSachNam(String NamLamViec, String sLNS, String MaND, String iID_MaTrangThaiDuyet)// ns Quốc phòng đầu 1 nhà nước đầu 2
        {
            String FileName = "SoPhanBoDuToanNganSachNamDonVi.xls";
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
            DataTable dt = DuToan_ReportModels.DT_rptPhanBoDuToanNganSachNam(MaND, iID_MaTrangThaiDuyet);
            String path1=Server.MapPath("/Report_ExcelFrom/DuToan/")+FileName;
            String path2= Server.MapPath("~/Libraries/DuToan/Excels/" + DateTime.Now.ToString("yyyy/MM/dd")+"/")+FileName;
            ExportToExcelModels.CopyFile(path1, path2);
            xlBook = xlApp.Workbooks.Open(path2, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);            
            xlBook = FillData_ToExcel(xlBook, dt, 4, 2, true);      
            Random randX = new Random();
            string PathSaveAs = Server.MapPath("~/Libraries/DuToan/Excels/" + DateTime.Now.ToString("yyyy/MM/dd"));
            CreateDirectory(PathSaveAs);
            string PathRand = PathSaveAs + "\\" + FileName + "_" + randX.Next().ToString() + ".xls";
            //save file
            xlBook.SaveAs(PathRand, Excel.XlFileFormat.xlWorkbookNormal, missValue, missValue, missValue, missValue, Excel.XlSaveAsAccessMode.xlExclusive, missValue, missValue, missValue, missValue, missValue);
            xlBook.Close(true, missValue, missValue);
            xlApp.Quit();

            // release cac doi tuong COM
            releaseObject(xlSheet);
            releaseObject(xlBook);
            releaseObject(xlApp);
            excelResult.FileName = FileName + ".xls";
            excelResult.Path = PathRand;
            return excelResult;
        }
        
        public static Boolean CreateDirectory(String Dir)
        {
            if (Directory.Exists(Dir) == false)
            {
                try
                {
                    Directory.CreateDirectory(Dir);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }    

        private Excel.Workbook FillData_ToExcel(Excel.Workbook xlBook, DataTable dt, int StartRow, int StartCol, Boolean STT)
        {
            Excel._Worksheet xlSheet = (Excel.Worksheet)xlBook.Worksheets.get_Item(1);
            String TenTruong = "", GiaTri = "";
            Object cell1;
            cell1=xlSheet.Cells[3,3];
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                TenTruong = dt.Columns[j].ColumnName;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow Row = dt.Rows[i];
                    Type type = Row[TenTruong].GetType();
                    //Fill số thứ tự     
                    if(STT)
                        xlSheet.Cells[i + StartRow, 1] = i + 1;
                 
                    switch (type.Name)
                    {
                        case "Double":
                        case "Decimal":
                            GiaTri = Convert.ToString(Row[j]);
                            xlSheet.Cells[i + StartRow, j + StartCol] = GiaTri;

                            xlSheet.get_Range((Object)xlSheet.Cells[i + StartRow, j + StartCol], (Object)xlSheet.Cells[i + StartRow, j + StartCol]).EntireColumn.NumberFormat = "#,###";
                            break;
                        case "Single":
                        case "Int32":
                            GiaTri = Convert.ToString(Row[j]);
                            xlSheet.Cells[i + StartRow, j + StartCol] = GiaTri;
                            xlSheet.get_Range((Object)xlSheet.Cells[i + StartRow, j + StartCol], (Object)xlSheet.Cells[i + StartRow, j + StartCol]).EntireColumn.NumberFormat = "#,###";
                            break;
                        case "String":
                            GiaTri = Convert.ToString(Row[j]);
                            xlSheet.Cells[i + StartRow, j + StartCol] = "'" + GiaTri;
                            break;
                        case "DateTime":
                            GiaTri = String.Format("{0:dd/MM/yyyy}", Row[j]);
                            xlSheet.Cells[i + StartRow, j + StartCol] = "'" + GiaTri;
                            break;
                        default:
                            GiaTri = Convert.ToString(Row[j]);
                            xlSheet.Cells[i + StartRow, j + StartCol] = GiaTri;
                            break;
                    }
                    ((Excel.Range)xlSheet.Cells[i + StartRow, j + StartCol]).EntireColumn.AutoFit();
                }
        
            }
            return xlBook;

        }
        private Excel.Workbook open( ref Excel.Workbook xlBook, String path)
        {
            
            return xlBook;
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

        public string PathPathSaveAs { get; set; }
    }
    public class clsExcelResult : ActionResult
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public MemoryStream ms {get;set;}
        public String type { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                context.HttpContext.Response.Buffer = true;
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                context.HttpContext.Response.ContentType = "application/vnd."+type;
                if (string.IsNullOrEmpty(Path) == false)
                {
                    context.HttpContext.Response.WriteFile(Path);
                }
                else
                {
                    context.HttpContext.Response.BinaryWrite(ms.ToArray());
                }
                context.HttpContext.Response.End();
            }
            catch { }
        }
    }




}
