using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel.Controls;
using DomainModel;
using System.Collections.Specialized;
using VIETTEL.Models;
using System.Data.OleDb;
using System.Text;
namespace VIETTEL.Controllers.KeToanTongHop
{
    public class ConvertToFoxController : Controller
    {
        //
        public string sViewPath = "~/Views/KeToanTongHop/ChungTuChiTiet/";

        [Authorize]
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                return View(sViewPath + "KeToanTongHop_ConvertToFox.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit()
        {
            String iThang = Request.Form["CauHinh_iThangLamViec"];
            String iNam = Request.Form["CauHinh_iNamLamViec"];
            String iMaTrangThaiDuyet = Request.Form["CauHinh_TrangThai"];
            String iThangHienThi = "", iNamHienThi = "";
            if (Convert.ToInt32(iThang) < 10)
            {
                iThangHienThi = "0" + iThang;
            }
            else
            {
                iThangHienThi = iThang;
            }
            if (iNam.Length > 3)
            {
                iNamHienThi = iNam.Substring(2);
            }

            String FileServerPath = "";
            String strTen = "SL" + iThangHienThi + "" + iNamHienThi + ".txt";
            if (string.IsNullOrEmpty(FileServerPath))
            {
                FileServerPath = "../Libraries/DataKT";
            }

            string fileName = FileServerPath + "/" + strTen;
            

            //-----------------//
           String SQL = "DELETE KT_TaiLieu WHERE iThang=@iThang AND iNam=@iNam; " +
                  "INSERT INTO KT_TaiLieu(sTenTaiLieu,iThang,iNam,sDuongDan,iTrangThai) VALUES(@sTenTaiLieu,@iThang,@iNam,@DuongDan,1)";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTenTaiLieu", "Dữ liệu tháng " + iThang + " năm " + iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@DuongDan", fileName.Replace("..", ""));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            //---------------------/
            String DK = "";
            if (iMaTrangThaiDuyet != "0")
            {
                DK = " AND kt.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ct.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            SQL =
                String.Format(
                    @"SELECT ISNULL(kt.sSoChungTu,'') as sSoChungTuGhiSo, kt.sNoiDung, kt.sDonVi, kt.iTapSo, ct.iNgayCT, ct.iThangCT, ct.iNamLamViec,
ct.iNgay, ct.iThang,ISNULL(ct.sSoChungTuChiTiet,'') as sSoChungTuChiTiet,
ct.sNoiDung as sNoiDungCT,ct.sGhiChu, ISNULL(ct.iID_MaPhongBan_Co,'') as iID_MaPhongBan_Co, ISNULL(ct.iID_MaDonVi_Co,'') as iID_MaDonVi_Co, ISNULL(ct.iID_MaTaiKhoan_Co,'') as iID_MaTaiKhoan_Co,
ISNULL(ct.iID_MaPhongBan_No,'') as iID_MaPhongBan_No, ISNULL(ct.iID_MaDonVi_No,'') as iID_MaDonVi_No, ISNULL(ct.iID_MaTaiKhoan_No,'') as iID_MaTaiKhoan_No, ct.rSoTien

 FROM KT_ChungTuChiTiet ct, KT_ChungTu kt where ct.iTrangThai=1 and kt.iTrangThai=1 and kt.iID_MaChungTu=ct.iID_MaChungTu and ct.iThangCT=@iThang  and ct.iNamLamViec=@iNam {0}",
                    DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            if (iMaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}\t", "Tap");
            builder.AppendFormat("{0}\t", "Loai");
            builder.AppendFormat("{0}\t", "Ngayct");
            builder.AppendFormat("{0}\t", "Thangct");
            builder.AppendFormat("{0}\t", "Chung_tu");
            builder.AppendFormat("{0}\t", "Ch_tu_gs");
            builder.AppendFormat("{0}\t", "Ngay");
            builder.AppendFormat("{0}\t", "Thang");
            builder.AppendFormat("{0}\t", "Dvi");
            builder.AppendFormat("{0}\t", "Dvn");
            builder.AppendFormat("{0}\t", "Dvc");
            builder.AppendFormat("{0}\t", "Sotien");
            builder.AppendFormat("{0}\t", "Tkno");
            builder.AppendFormat("{0}\t", "Tkco");
            builder.AppendFormat("{0}\t", "Noidung");
            builder.AppendFormat("{0}", "Dien_giai");
            builder.AppendLine();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (System.IO.File.Exists(Server.MapPath(fileName)))
                    {
                        System.IO.File.Delete(Server.MapPath(fileName));
                    }
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iTapSo"]).Trim());
                    builder.AppendFormat("{0}\t", "");
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iNgayCT"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iThangCT"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["sSoChungTuChiTiet"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["sSoChungTuGhiSo"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iNgay"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iThang"]).Trim());
                    builder.AppendFormat("{0}\t", NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sDonVi"])).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iID_MaPhongBan_No"]).Trim() + "" + Convert.ToString(dr["iID_MaDonVi_No"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iID_MaPhongBan_Co"]).Trim() + "" + Convert.ToString(dr["iID_MaDonVi_Co"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToDouble(dr["rSoTien"]));
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iID_MaTaiKhoan_No"]).Trim());
                    builder.AppendFormat("{0}\t", Convert.ToString(dr["iID_MaTaiKhoan_Co"]).Trim());
                    builder.AppendFormat("{0}\t", NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sNoiDungCT"])).Trim());
                    builder.AppendFormat("{0}", NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sNoiDung"])).Trim());
                    builder.AppendLine();
                    dr = null;
                }

            }
            System.IO.File.WriteAllText(Server.MapPath(fileName), builder.ToString(), Encoding.Default);
            builder.Clear();
            builder = null;
            return RedirectToAction("Index", "ConvertToFox");

        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_DBF()
        {
            String iThang = Request.Form["CauHinh_iThangLamViec"];
            String iNam = Request.Form["CauHinh_iNamLamViec"];
            String iMaTrangThaiDuyet = Request.Form["CauHinh_TrangThai"];
            String iThangHienThi = "", iNamHienThi = "";
            if (Convert.ToInt32(iThang) < 10)
            {
                iThangHienThi = "0" + iThang;
            }
            else
            {
                iThangHienThi = iThang;
            }
            if (iNam.Length > 3)
            {
                iNamHienThi = iNam.Substring(2);
            }

            String FileServerPath = "";
            String strTen = "SL" + iThangHienThi + "" + iNamHienThi + ".DBF";
            if (string.IsNullOrEmpty(FileServerPath))
            {
                FileServerPath = "../Libraries/DataKT";
            }

            string fileName = FileServerPath + "/" + strTen;
            try
            {
                if (System.IO.File.Exists(Server.MapPath(fileName)))
                {
                    System.IO.File.Delete(Server.MapPath(fileName));
                }
            }
            catch
            {

            }

            String connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(FileServerPath) +
            ";Extended Properties=dBASE IV;User ID=Admin;Password=";

            String SQL = "CREATE TABLE " + strTen +
                       " (Tap Numeric(2),Loai Character(1), Ngayct Integer, Thangct Integer,Chung_tu Character(10), Ch_tu_gs Integer, Ngay Integer, Thang Numeric(2,0), Dvi Varchar(20),Dvn Character(4), Dvc Character(4), Sotien Double, Tkno Character(5), Tkco Character(5),  Noidung Varchar(254), Dien_giai Character(254))";
            ExecuteNonQuery(connectionString, SQL);
            //-----------------//
            SQL = "DELETE KT_TaiLieu WHERE iThang=@iThang AND iNam=@iNam; " +
                  "INSERT INTO KT_TaiLieu(sTenTaiLieu,iThang,iNam,sDuongDan,iTrangThai) VALUES(@sTenTaiLieu,@iThang,@iNam,@DuongDan,1)";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTenTaiLieu", "Dữ liệu tháng " + iThang + " năm " + iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@DuongDan", fileName.Replace("..", ""));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            //---------------------/
            String DK = "";
            if (iMaTrangThaiDuyet != "0")
            {
                DK = " AND kt.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ct.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            SQL =
                String.Format(
                    @"SELECT ISNULL(kt.sSoChungTu,'') as sSoChungTuGhiSo, kt.sNoiDung, kt.sDonVi, kt.iTapSo, ct.iNgayCT, ct.iThangCT, ct.iNamLamViec,
ct.iNgay, ct.iThang,ISNULL(ct.sSoChungTuChiTiet,'') as sSoChungTuChiTiet,
ct.sNoiDung as sNoiDungCT,ct.sGhiChu, ISNULL(ct.iID_MaPhongBan_Co,'') as iID_MaPhongBan_Co, ISNULL(ct.iID_MaDonVi_Co,'') as iID_MaDonVi_Co, ISNULL(ct.iID_MaTaiKhoan_Co,'') as iID_MaTaiKhoan_Co,
ISNULL(ct.iID_MaPhongBan_No,'') as iID_MaPhongBan_No, ISNULL(ct.iID_MaDonVi_No,'') as iID_MaDonVi_No, ISNULL(ct.iID_MaTaiKhoan_No,'') as iID_MaTaiKhoan_No, ct.rSoTien

 FROM KT_ChungTuChiTiet ct, KT_ChungTu kt where ct.iTrangThai=1 and kt.iTrangThai=1 and kt.iID_MaChungTu=ct.iID_MaChungTu and ct.iThangCT=@iThang  and ct.iNamLamViec=@iNam {0}",
                    DK);

            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            if (iMaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    OleDbCommand command = new OleDbCommand();

                    SQL = "INSERT INTO " + strTen +
                          " (Tap,Loai, Ngayct, Thangct,Chung_tu, Ch_tu_gs, Ngay, Thang, Dvi,Dvn, Dvc, Sotien, Tkno, Tkco,  Noidung, Dien_giai) VALUES (@Tap,@Loai, @Ngayct, @Thangct,@Chung_tu, @Ch_tu_gs, @Ngay, @Thang, @Dvi,@Dvn, @Dvc, @Sotien, @Tkno, @Tkco,  @Noidung, @Dien_giai)";
                    command.CommandText = SQL;
                    command.Parameters.AddWithValue("@Tap", Convert.ToString(dr["iTapSo"]));
                    command.Parameters.AddWithValue("@Loai", "");
                    command.Parameters.AddWithValue("@Ngayct", Convert.ToString(dr["iNgayCT"]));
                    command.Parameters.AddWithValue("@Thangct", Convert.ToString(dr["iThangCT"]));
                    command.Parameters.AddWithValue("@Chung_tu", Convert.ToString(dr["sSoChungTuChiTiet"]));
                    command.Parameters.AddWithValue("@Ch_tu_gs", Convert.ToString(dr["sSoChungTuGhiSo"]));
                    command.Parameters.AddWithValue("@Ngay", Convert.ToString(dr["iNgay"]));
                    command.Parameters.AddWithValue("@Thang", Convert.ToString(dr["iThang"]));
                    command.Parameters["@Thang"].OleDbType = System.Data.OleDb.OleDbType.SmallInt;

                    command.Parameters.AddWithValue("@Dvi",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sDonVi"])));
                    command.Parameters["@Dvi"].OleDbType = System.Data.OleDb.OleDbType.VarChar;
                    command.Parameters.AddWithValue("@Dvn",
                                                   Convert.ToString(dr["iID_MaPhongBan_No"]) + "" +
                                                   Convert.ToString(dr["iID_MaDonVi_No"]));
                    command.Parameters.AddWithValue("@Dvc", Convert.ToString(dr["iID_MaPhongBan_Co"]) + "" +
                                                            Convert.ToString(dr["iID_MaDonVi_Co"]));
                    command.Parameters.AddWithValue("@Sotien", Convert.ToDouble(dr["rSoTien"]));
                    command.Parameters.AddWithValue("@Tkno", Convert.ToString(dr["iID_MaTaiKhoan_No"]));
                    command.Parameters.AddWithValue("@Tkco", Convert.ToString(dr["iID_MaTaiKhoan_Co"]));
                    command.Parameters.AddWithValue("@Noidung",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sNoiDungCT"])));
                    command.Parameters["@Noidung"].OleDbType = System.Data.OleDb.OleDbType.Char;
                    command.Parameters.AddWithValue("@Dien_giai",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sGhiChu"])));

                    UpdateDatabase(command, connectionString);
                    command.Dispose();
                }

            }

            return RedirectToAction("Index", "ConvertToFox");

        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSubmit_Excel()
        {
            String iThang = Request.Form["CauHinh_iThangLamViec"];
            String iNam = Request.Form["CauHinh_iNamLamViec"];
            String iMaTrangThaiDuyet = Request.Form["CauHinh_TrangThai"];
            String iThangHienThi = "", iNamHienThi = "";
            if (Convert.ToInt32(iThang) < 10)
            {
                iThangHienThi = "0" + iThang;
            }
            else
            {
                iThangHienThi = iThang;
            }
            if (iNam.Length > 3)
            {
                iNamHienThi = iNam.Substring(2);
            }

            String FileServerPath = "";
            
            String strTen = "SL" + iThangHienThi + "" + iNamHienThi + ".XLS";
            if (string.IsNullOrEmpty(FileServerPath))
            {
                FileServerPath = "../Libraries/DataKT";
            }

            string fileName = FileServerPath + "/" + strTen;
            //try
            //{
            //    if (System.IO.File.Exists(Server.MapPath(fileName)))
            //    {
            //        System.IO.File.Delete(Server.MapPath(fileName));
            //    }
            //}
            //catch
            //{
              
            //}
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Viettel_Project\CTC_BQP\Project 29082013\VIETTEL\Libraries\DataKT\SL0210.XLS;Extended Properties='Excel 8.0;HDR=Yes'";
           // String connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(FileServerPath) +
                                     // ";Extended Properties=dBASE IV;User ID=Admin;Password=";

            //String SQL = "CREATE TABLE " + strTen +
            //           " (Tap Numeric(2),Loai Character(1), Ngayct Integer, Thangct Integer,Chung_tu Character(10), Ch_tu_gs Integer, Ngay Integer, Thang Numeric(2,0), Dvi Character(20),Dvn Character(4), Dvc Character(4), Sotien Double, Tkno Character(5), Tkco Character(5),  Noidung Character(254), Dien_giai Character(254))";
            //ExecuteNonQuery(connectionString, SQL);
            //-----------------//
            String SQL = "DELETE KT_TaiLieu WHERE iThang=@iThang AND iNam=@iNam; " +
                  "INSERT INTO KT_TaiLieu(sTenTaiLieu,iThang,iNam,sDuongDan,iTrangThai) VALUES(@sTenTaiLieu,@iThang,@iNam,@DuongDan,1)";
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sTenTaiLieu", "Dữ liệu tháng " + iThang + " năm " + iNam);
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            cmd.Parameters.AddWithValue("@DuongDan", fileName.Replace("..", ""));
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            //---------------------/
            String DK = "";
            if (iMaTrangThaiDuyet!="0")
            {
                DK = " AND kt.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet AND ct.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            SQL =
                String.Format(
                    @"SELECT ISNULL(kt.sSoChungTu,'') as sSoChungTuGhiSo, kt.sNoiDung, kt.sDonVi, kt.iTapSo, ct.iNgayCT, ct.iThangCT, ct.iNamLamViec,
ct.iNgay, ct.iThang,ISNULL(ct.sSoChungTuChiTiet,'') as sSoChungTuChiTiet,
ct.sNoiDung as sNoiDungCT,ct.sGhiChu, ISNULL(ct.iID_MaPhongBan_Co,'') as iID_MaPhongBan_Co, ISNULL(ct.iID_MaDonVi_Co,'') as iID_MaDonVi_Co, ISNULL(ct.iID_MaTaiKhoan_Co,'') as iID_MaTaiKhoan_Co,
ISNULL(ct.iID_MaPhongBan_No,'') as iID_MaPhongBan_No, ISNULL(ct.iID_MaDonVi_No,'') as iID_MaDonVi_No, ISNULL(ct.iID_MaTaiKhoan_No,'') as iID_MaTaiKhoan_No, ct.rSoTien

 FROM KT_ChungTuChiTiet ct, KT_ChungTu kt where ct.iTrangThai=1 and kt.iTrangThai=1 and kt.iID_MaChungTu=ct.iID_MaChungTu and ct.iThangCT=@iThang  and ct.iNamLamViec=@iNam {0}",
                    DK);
           
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iThang", iThang);
            cmd.Parameters.AddWithValue("@iNam", iNam);
            if (iMaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    OleDbCommand command = new OleDbCommand();

                    //SQL = "INSERT INTO " + strTen +
                    SQL = "INSERT INTO [SL0210$]" +
                          " (Tap,Loai, Ngayct, Thangct,Chung_tu, Ch_tu_gs, Ngay, Thang, Dvi,Dvn, Dvc, Sotien, Tkno, Tkco,  Noidung, Dien_giai) VALUES (@Tap,@Loai, @Ngayct, @Thangct,@Chung_tu, @Ch_tu_gs, @Ngay, @Thang, @Dvi,@Dvn, @Dvc, @Sotien, @Tkno, @Tkco,  @Noidung, @Dien_giai)";
                    command.CommandText = SQL;
                    command.Parameters.AddWithValue("@Tap", Convert.ToString(dr["iTapSo"]));
                    command.Parameters.AddWithValue("@Loai", "");
                    command.Parameters.AddWithValue("@Ngayct", Convert.ToString(dr["iNgayCT"]));
                    command.Parameters.AddWithValue("@Thangct", Convert.ToString(dr["iThangCT"]));
                    command.Parameters.AddWithValue("@Chung_tu", Convert.ToString(dr["sSoChungTuChiTiet"]));
                    command.Parameters.AddWithValue("@Ch_tu_gs", Convert.ToString(dr["sSoChungTuGhiSo"]));
                    command.Parameters.AddWithValue("@Ngay", Convert.ToString(dr["iNgay"]));
                    command.Parameters.AddWithValue("@Thang", Convert.ToString(dr["iThang"]));
                    command.Parameters["@Thang"].OleDbType = System.Data.OleDb.OleDbType.SmallInt;
                
                    command.Parameters.AddWithValue("@Dvi",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sDonVi"])));
                   command.Parameters["@Dvi"].OleDbType = System.Data.OleDb.OleDbType.Char;
                    command.Parameters.AddWithValue("@Dvn",
                                                   Convert.ToString(dr["iID_MaPhongBan_No"]) + "" +
                                                   Convert.ToString(dr["iID_MaDonVi_No"]));
                    command.Parameters.AddWithValue("@Dvc", Convert.ToString(dr["iID_MaPhongBan_Co"]) + "" +
                                                            Convert.ToString(dr["iID_MaDonVi_Co"]));
                    command.Parameters.AddWithValue("@Sotien", Convert.ToDouble(dr["rSoTien"]));
                    command.Parameters.AddWithValue("@Tkno", Convert.ToString(dr["iID_MaTaiKhoan_No"]));
                    command.Parameters.AddWithValue("@Tkco", Convert.ToString(dr["iID_MaTaiKhoan_Co"]));
                    command.Parameters.AddWithValue("@Noidung",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sNoiDungCT"])));
                    command.Parameters["@Noidung"].OleDbType = System.Data.OleDb.OleDbType.Char;
                    command.Parameters.AddWithValue("@Dien_giai",
                                                    NgonNgu.ChuyenUnicodeSangTCVN3(Convert.ToString(dr["sGhiChu"])));

                    UpdateDatabase(command, connectionString);
                    command.Dispose();
                }

            }
            
            return RedirectToAction("Index", "ConvertToFox");
          
        }
        public ActionResult Download(String MaTaiLieu)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM KT_TaiLieu WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
            cmd.Parameters.AddWithValue("@iID_MaTaiLieu", MaTaiLieu);
            DataTable dtTaiLieu = Connection.GetDataTable(cmd);
            String tg = Convert.ToString(dtTaiLieu.Rows[0]["sDuongDan"]);
            //String strExt = "";
            //int i = tg.LastIndexOf('.');
            //if (i >= 0)
            //{
            //    strExt = tg.Substring(i);
            //}

            String fileName = "";
            int j = tg.LastIndexOf('/');
            if (j >= 0)
            {
                fileName = tg.Substring(j);
            }

            String strURL = String.Format("~/{0}", dtTaiLieu.Rows[0]["sDuongDan"]);
            String strTen = String.Format("{0}", fileName.Replace("/", ""));
            //String strTen = String.Format("{0}{1}", fileName, "TXT");
            strTen = NgonNgu.LayXauKhongDauTiengViet(strTen);
            cmd.Dispose();
            dtTaiLieu.Dispose();

           //strTen = strTen.Replace(' ', '_');
            return new DownloadResult { VirtualPath = strURL, FileDownloadName = strTen };
        }
        [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Delete(string iID_MaTaiLieu)
        {
            //string SQL =
            //    "DELETE KT_TaiLieu WHERE iID_MaTaiLieu=@iID_MaTaiLieu";
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = SQL;
            //cmd.Parameters.AddWithValue("@iID_MaTaiLieu", iID_MaTaiLieu);
            //Connection.DeleteRecord(cmd);
            //cmd.Dispose();
            //cmd.Parameters.Clear();
            Connection.DeleteRecord("KT_TaiLieu", "iID_MaTaiLieu", iID_MaTaiLieu);
            return RedirectToAction("Index", "ConvertToFox");
        }
        private int ExecuteNonQuery(String connectionString, String SQL)
        {
           
            OleDbConnection connection = new OleDbConnection(connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = SQL;
            int Result = command.ExecuteNonQuery();
            connection.Close();
            connection.Dispose();
            command.Dispose();
            return Result;
        }
        public int UpdateDatabase(OleDbCommand cmd, String connectionString)
        {
            int vR = 0;
            OleDbConnection conn = null;
            conn = new OleDbConnection(connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            vR = cmd.ExecuteNonQuery();
            conn.Dispose();
            conn.Close();
            return vR;
        }
        public static DataTable Get_dtTaiLieu(int Trang = 1, int SoBanGhi = 0)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM KT_TaiLieu WHERE iTrangThai=1");
            cmd.CommandText = SQL;
            vR = CommonFunction.dtData(cmd, "dNgayTao DESC", Trang, SoBanGhi);
            cmd.Dispose();
            return vR;
        }

        public static int Get_CountdtTaiLieu()
        {
            String SQL = "SELECT COUNT(*) FROM KT_TaiLieu WHERE iTrangThai=1";
            return Convert.ToInt16(Connection.GetValue(SQL, 0));
        }

    }
    public class DownloadResult : ActionResult
    {

        public DownloadResult()
        {
        }

        public DownloadResult(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public string FileDownloadName
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!String.IsNullOrEmpty(FileDownloadName))
            {
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + this.FileDownloadName);
            }
            string filePath = context.HttpContext.Server.MapPath(this.VirtualPath);
            context.HttpContext.Response.TransmitFile(filePath);
        }
    }

}
