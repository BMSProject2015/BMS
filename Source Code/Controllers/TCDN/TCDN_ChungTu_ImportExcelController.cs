using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using DomainModel.Abstract;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using VIETTEL.Models;

namespace VIETTEL.Controllers
{
    public class TCDN_ChungTu_ImportExcelController : Controller
    {
        //
        // GET: /ImportExcelVatTu/
        public string sViewPath = "~/Views/TCDN/ChungTu_Import/";
        public static DataTable dtImportResult;
        public static int CountRecord = 0;
        public static String ResultSheetName = "", sLoi = "";

        public ActionResult Index()
        {
            ViewData["DuLieuMoi"] = "1";
            return View(sViewPath + "Index.aspx");
        }


        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit()
        {
            
            String ParentID = "Edit";
            String MaChungTu = Request.Form[ParentID + "iID_MaChungTu"];
            String MaND = User.Identity.Name;
            Bang bang = new Bang("TCDN_ChungTu");
            String MaChungTuAddNew = "";
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(TCDNModels.iID_MaPhanHe, MaND) == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(MaND, bang.TenBang, "Create") == false)
            {
                return RedirectToAction("Index", "PermitionMessage");
            }


            HttpPostedFileBase hpf = Request.Files[ParentID + "_sFileName"] as HttpPostedFileBase;
            String FileName = hpf.FileName;
            FileName = FileName.Replace(' ', '_');
            String strURLPath = String.Format("Libraries/Excels/{0}", DateTime.Now.ToString("yyyy/MM/dd"));
            String FilePath = Server.MapPath("~/" + strURLPath);
            HamChung.CreateDirectory(FilePath);

            String[] arrFN = FileName.Split('\\');
            FileName = arrFN[arrFN.Length - 1];
            FileName = NgonNgu.LayXauKhongDauTiengViet(FileName);
            arrFN = FileName.Split('.');
            if (arrFN.Length >= 2)
            {
                FileName = String.Format("{0}.{1}", arrFN[0], arrFN[arrFN.Length - 1]);
            }
            FileName = String.Format("{0}_{1}_{2}", DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"), DateTime.Now.Millisecond, FileName);
            hpf.SaveAs(String.Format("{0}\\{1}", FilePath, FileName));
            FilePath = String.Format(FilePath + "\\\\{0}", FileName);


            int i;
            NameValueCollection arrLoi = new NameValueCollection();
            String iID_MaDoanhNghiep = Convert.ToString(Request.Form[ParentID + "_iID_MaDoanhNghiep"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            String NgayChungTu = Convert.ToString(Request.Form[ParentID + "_vidNgayChungTu"]);
            String sFileName = FileName;
            
            if (iID_MaDoanhNghiep == Convert.ToString(Guid.Empty) || iID_MaDoanhNghiep == "" || iID_MaDoanhNghiep == null)
            {
                arrLoi.Add("err_iID_MaDonVi", "Bạn chưa chọn đơn vị!");
            }
            if (iQuy == string.Empty || iQuy == "" || iQuy == null)
            {
                arrLoi.Add("err_iQuy", "Bạn chưa chọn tháng cho chứng từ!");
            }
            if (NgayChungTu == string.Empty || NgayChungTu == "" || NgayChungTu == null)
            {
                arrLoi.Add("err_dNgayChungTu", "Bạn chưa nhập ngày chứng từ!");
            }
            if (String.IsNullOrEmpty(sFileName))
            {
                arrLoi.Add("err_sFileName", "Bạn chưa chọn file excel!");
            }
            if (arrLoi.Count > 0)
            {
                for (i = 0; i <= arrLoi.Count - 1; i++)
                {
                    ModelState.AddModelError(ParentID + "_" + arrLoi.GetKey(i), arrLoi[i]);
                }
                ViewData["DuLieuMoi"] = "0";
                if (String.IsNullOrEmpty(MaChungTu))
                {
                    ViewData["DuLieuMoi"] = "1";
                }
                ViewData["MaChungTu"] = MaChungTu;
                return View(sViewPath + "Index.aspx");
            }
            else
            {
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                DataRow R = dtCauHinh.Rows[0];

                String sLoai = "0;1";

                bang.MaNguoiDungSua = User.Identity.Name;
                bang.IPSua = Request.UserHostAddress;
                bang.TruyenGiaTri(ParentID, Request.Form);

                bang.CmdParams.Parameters.AddWithValue("@sTienToChungTu", PhanHeModels.LayTienToChungTu(TCDNModels.iID_MaPhanHe));
                bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", R["iNamLamViec"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", R["iID_MaNguonNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", R["iID_MaNamNganSach"]);
                bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(TCDNModels.iID_MaPhanHe));
                MaChungTuAddNew = Convert.ToString(bang.Save());

                //String FilePath = Server.MapPath("~/" + Request.Form[ParentID + "_sDuongDan"] + "");
                ImportChiTiet(MaChungTuAddNew, sLoai, MaND, Request.UserHostAddress, FilePath);
                TCDN_ChungTuModels.InsertDuyetChungTu(MaChungTuAddNew, "Mới mới", User.Identity.Name, Request.UserHostAddress);
                dtCauHinh.Dispose();
            }
            return RedirectToAction("Index", "TCDN_ChungTuChiTiet", new { iID_MaChungTu = MaChungTuAddNew });
        }
        private void ImportChiTiet(String iID_MaChungTu, String iLoai, String MaND, String IPSua, String FilePath)
        {
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dt = new DataTable();
            SheetData data = new SheetData();

            cmdExcel.CommandText = "SELECT * From [CanDoiKeToan$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            DataTable dtCauHinh = new DataTable();
            cmdExcel.CommandText = "SELECT * From [CauHinh$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtCauHinh);
            DataRow CauHinhDinhDang = dtCauHinh.Rows[0];
            DataRow CauHinhCanLe = dtCauHinh.Rows[1];

            DataTable dtChungTu = TCDN_ChungTuModels.GetChungTu(iID_MaChungTu);

            int iNamLamViec = Convert.ToInt32(dtChungTu.Rows[0]["iNamLamViec"]);
            int iID_MaNguonNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
            int iID_MaNamNganSach = Convert.ToInt32(dtChungTu.Rows[0]["iID_MaNamNganSach"]);
            String iID_MaDoanhNghiep = Convert.ToString(dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);
            int iQuy = Convert.ToInt32(dtChungTu.Rows[0]["iQuy"]);
            String[] arrLoai = iLoai.Split(';');
            int Nam = iNamLamViec;
            int Quy = iQuy;
            if (iQuy == 1)
            {
                Nam = iNamLamViec - 1;
                Quy = 4;
            }
            else
            {
                Quy = iQuy - 1;
            }
            DataTable dtDauKy = TCDN_ChungTuChiTietModels.Get_dtQuyNam(Nam, Quy, iID_MaDoanhNghiep);

            Bang bang = new Bang("TCDN_ChungTuChiTiet");
            bang.GiaTriKhoa = null;
            bang.DuLieuMoi = true;
            bang.MaNguoiDungSua = MaND;
            bang.IPSua = IPSua;
            for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    bang.CmdParams.Parameters.Clear();
                    bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                    bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 1);

                    String sKyHieu = Convert.ToString(row[dtCauHinh.Columns.IndexOf("sKyHieu")]);
                    DataTable dtChiTieu = TCDN_ChiTieuModels.Get_ChiTieu_Theo_KyHieu(sKyHieu);
                    if (dtChiTieu.Rows.Count > 0)
                    {
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu", dtChiTieu.Rows[0]["iID_MaChiTieu"]);
                        bang.CmdParams.Parameters.AddWithValue("@iID_MaChiTieu_Cha", dtChiTieu.Rows[0]["iID_MaChiTieu_Cha"]);
                        bang.CmdParams.Parameters.AddWithValue("@sTen", dtChiTieu.Rows[0]["sTen"]);
                    }
                    else
                    {
                        String sSTT = Convert.ToString(row[dtCauHinh.Columns.IndexOf("sSTT")]);
                        String sNoiDung = Convert.ToString(row[dtCauHinh.Columns.IndexOf("sNoiDung")]);
                        String sTen = "";
                        if (String.IsNullOrEmpty(sSTT))
                        {
                            sTen = sNoiDung;
                        }
                        else
                        {
                            if (CommonFunction.IsNumeric(sSTT))
                            {
                                sTen = sSTT + ". " + sNoiDung;
                            }else{
                                sTen = sSTT + " - " + sNoiDung;
                            }
                        }
                        bang.CmdParams.Parameters.AddWithValue("@sTen", sTen);
                    }
                    bang.CmdParams.Parameters.AddWithValue("@sKyHieu", sKyHieu);
                    bang.CmdParams.Parameters.AddWithValue("@bLaHangCha", row[dtCauHinh.Columns.IndexOf("bLaHangCha")]);
                    bang.CmdParams.Parameters.AddWithValue("@sThuyetMinh", row[dtCauHinh.Columns.IndexOf("sThuyetMinh")]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoDauNam", row[dtCauHinh.Columns.IndexOf("rSoDauNam")]);
                    bang.CmdParams.Parameters.AddWithValue("@rSoCuoiNam", row[dtCauHinh.Columns.IndexOf("rSoCuoiNam")]);

                    bang.CmdParams.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaPhongBan", dtChungTu.Rows[0]["iID_MaPhongBan"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", dtChungTu.Rows[0]["iID_MaTrangThaiDuyet"]);
                    bang.CmdParams.Parameters.AddWithValue("@iNamLamViec", dtChungTu.Rows[0]["iNamLamViec"]);
                    bang.CmdParams.Parameters.AddWithValue("@iQuy", dtChungTu.Rows[0]["iQuy"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNguonNganSach", dtChungTu.Rows[0]["iID_MaNguonNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaNamNganSach", dtChungTu.Rows[0]["iID_MaNamNganSach"]);
                    bang.CmdParams.Parameters.AddWithValue("@iID_MaDoanhNghiep", dtChungTu.Rows[0]["iID_MaDoanhNghiep"]);
                    bang.Save();

                }
            connExcel.Close();
        }
        public class SheetData
        {
            public string sData { get; set; }
        }

        public JsonResult get_dtSheet(String ParentID, String DuongDan)
        {
            String FilePath = Server.MapPath("~/" + DuongDan + "");
            return Json(get_objSheet(ParentID, FilePath), JsonRequestBehavior.AllowGet);
        }

        public static SheetData get_objSheet(String ParentID, String FilePath)
        {
            String vR = string.Empty;
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            conStr = String.Format(conStr, FilePath);
            OleDbConnection connExcel = new OleDbConnection(conStr);
            OleDbCommand cmdExcel = new OleDbCommand();
            OleDbDataAdapter oda = new OleDbDataAdapter();
            cmdExcel.Connection = connExcel;
            connExcel.Open();

            DataTable dt = new DataTable();
            SheetData data = new SheetData();

            cmdExcel.CommandText = "SELECT * From [CanDoiKeToan$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dt);

            DataTable dtCauHinh = new DataTable();
            cmdExcel.CommandText = "SELECT * From [CauHinh$]";
            oda.SelectCommand = cmdExcel;
            oda.Fill(dtCauHinh);
            DataRow CauHinhDinhDang = dtCauHinh.Rows[0];
            DataRow CauHinhCanLe = dtCauHinh.Rows[1];

            connExcel.Close();

            vR += "<table cellpadding='0' cellspacing='0' border='0' class='table_form3'>";
            vR += "<tr class='tr_form3'>";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    vR += "<td align = 'center' >";
                    vR += "<b>";
                    vR += dt.Columns[i].ColumnName;
                    vR += "</b>";
                    vR += "</td>";
                }
                vR += "</tr>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataRow = dt.Rows[i];
                    String laHangCha = Convert.ToString(dataRow[0]);
                    vR += "<tr>";
                    for (int j = 1; j < dataRow.Table.Columns.Count; j++)
                    {
                        String strCanLe = Convert.ToString(CauHinhCanLe[j]);
                        switch (strCanLe)
                        {
                            case "c":
                                strCanLe = "align = 'center'";
                                break;
                            case "l":
                                strCanLe = "align = 'left'";
                                break;
                            case "r":
                                strCanLe = "align = 'right'";
                                break;
                            default:
                                strCanLe = "";
                                break;
                        }

                        String strDinhDang = Convert.ToString(CauHinhDinhDang[j]);
                        string dataRowText = dataRow[j].ToString().Trim();
                        if (strDinhDang == "r") dataRowText = CommonFunction.DinhDangSo(dataRowText);
                        String laCha = Convert.ToString(dataRow[0]);
                        if (laCha == "1") laCha = "style = 'font-weight:bold'";
                        else laCha = "";
                        vR += "<td " + strCanLe + " >";
                        vR += MyHtmlHelper.Label(dataRowText, dt.Columns[j].ColumnName,"", laCha);
                        vR += "</td>";
                    }
                    vR += "</tr>";
                }
                dt.Dispose();
            }
            vR += "</table>";
            data.sData = vR;
            return data;
        }
    }
}
