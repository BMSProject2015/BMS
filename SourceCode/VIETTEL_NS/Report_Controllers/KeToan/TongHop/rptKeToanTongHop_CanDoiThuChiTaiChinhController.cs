using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKeToanTongHop_CanDoiThuChiTaiChinhController : Controller
    {
        //
        // GET: /rptGia_1C/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_CanDoiThuChiTaiChinhA3.xls";
        public static String NameFile = "";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_CanDoiThuChiTaiChinh.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                ViewData["srcFile"] = NameFile;
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }
        /// <summary>
        /// hàm lấy các giá trị khi thực hiện submit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String iNgay = Convert.ToString(Request.Form[ParentID + "_iNgay"]);
            String iThang = Convert.ToString(Request.Form[ParentID + "_iThang"]);
            String iID_MaTrangThaiDuyet = Convert.ToString(Request.Form[ParentID + "_iID_MaTrangThaiDuyet"]);
            String DonViTinh = Convert.ToString(Request.Form[ParentID + "_DonViTinh"]);
            String iQuy = Convert.ToString(Request.Form[ParentID + "_iQuy"]);
            ViewData["PageLoad"] = "1";
            ViewData["iNgay"] = iNgay;
            ViewData["iThang"] = iThang;
            ViewData["iID_MaTrangThaiDuyet"] = iID_MaTrangThaiDuyet;
            ViewData["DonViTinh"] = DonViTinh;
            ViewData["iQuy"] = iQuy;
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_CanDoiThuChiTaiChinh.aspx";
            return View(sViewPath + "ReportView.aspx");
          // return RedirectToAction("Index", new { DenNgay = DenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, DuocXem = 1 });
        }

        /// <summary>
        /// Hàm khởi tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh,  String iQuy)
        {
           
            String iNamLamViec = DateTime.Now.ToString();
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            {
                iNamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            XlsFile Result = new XlsFile(true);
            // Result.Open(Server.MapPath(path));
            Result.Open(path);
            String BoQuocPhong = ReportModels.CauHinhTenDonViSuDung(1);
            String CucTaiChinh = ReportModels.CauHinhTenDonViSuDung(2);
            String ngay = ReportModels.Ngay_Thang_Nam_HienTai();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKeToanTongHop_CanDoiThuChiTaiChinh");
            LoadData(fr, MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, iNamLamViec, iQuy);
            if (DonViTinh == "0")
            {
                fr.SetValue("DVT", "Đồng");
            }
            else if (DonViTinh == "1")
            {
                fr.SetValue("DVT", "Nghìn đồng");
            }
            else
            {
                fr.SetValue("DVT", "Triệu đồng");
            }

            fr.SetValue("iNam", iNamLamViec);
            fr.SetValue("iThang", iThang);
            fr.SetValue("iNgay", iNgay);
            fr.SetValue("ngay", ngay);
            fr.SetValue("LoaiBaoCao", DonViTinh);
            fr.SetValue("BoQuocPhong", BoQuocPhong);
            fr.SetValue("CucTaiChinh", CucTaiChinh);
            fr.Run(Result);
            return Result;

        }
        public DataTable rptKeToanTongHop_CanDoiThuChiTaiChinh_PhanThu(String iID_MaTrangThaiDuyet, String iNgay, String iThang, String iNamLamViec, String DVT, String iQuy)
        {
            DataTable dt = null;
            String DKDVT = "";
            if (DVT == "0")
            {
                DKDVT = "";
            }
            else if (DVT == "1")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;
            String SQL = String.Format(@"select iID_MaThuChi,ISNULL(iID_MaThuChi_Cha,0) as iID_MaThuChi_Cha,bLaHangCha,bTuDongTinh,bTuTinhTong,sNoiDung, ISNULL(sKyHieu_Cha,'') as sKyHieu_Cha, ISNULL(sTenTaiKhoan_TienViet,'') as sTenTaiKhoan_TienViet,ISNULL(sTenTaiKhoan_NgoaiTe,'') as sTenTaiKhoan_NgoaiTe,ISNULL(sTenTaiKhoan_Tong,'') as sTenTaiKhoan_Tong,bCoTKGT_NgoaiTe,bCoTKGT_TienViet,bCoTKGT_Tong
 from KT_CanDoiThuChiTaiChinh cd where cd.iNam=@iNamLamViec and cd.iLoaiThuChi=1 and cd.iTrangThai=1 and cd.bHienThi=1 and iQuy=@iQuy order by iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
            //cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
          //  cmd.Parameters.AddWithValue("@iThangCT", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //return dt;
            int ThuTu = 0;
            DataTable tbl = GetData_PhanThu(dt, 0, ref ThuTu, false, DenNgay, iThangSoDuDauNam, iID_MaTrangThaiDuyet, DVT, Convert.ToInt32(iQuy));
            dt.Dispose();
            return tbl;
        }
        public DataTable rptKeToanTongHop_CanDoiThuChiTaiChinh_PhanChi(String iID_MaTrangThaiDuyet, String iNgay, String iThang, String iNamLamViec, String DVT, String iQuy)
        {
            DataTable dt = null;
            String DKDVT = "";
            if (DVT == "0")
            {
                DKDVT = "";
            }
            else if (DVT == "1")
            {
                DKDVT = "/1000 ";
            }
            else
            {
                DKDVT = "/1000000 ";
            }
            if (iID_MaTrangThaiDuyet == "0")
            {
                iID_MaTrangThaiDuyet = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
            }
            else
            {
                iID_MaTrangThaiDuyet = " ";
            }
            String DenNgay = iNamLamViec + "/" + iThang + "/" + iNgay;


            String SQL = String.Format(@"select iID_MaThuChi,ISNULL(iID_MaThuChi_Cha,0) as iID_MaThuChi_Cha,bLaHangCha,bTuDongTinh,bTuTinhTong,sNoiDung, ISNULL(sKyHieu_Cha,'') as sKyHieu_Cha, ISNULL(sTenTaiKhoan_TienViet,'') as sTenTaiKhoan_TienViet,ISNULL(sTenTaiKhoan_NgoaiTe,'') as sTenTaiKhoan_NgoaiTe,ISNULL(sTenTaiKhoan_Tong,'') as sTenTaiKhoan_Tong,bCoTKGT_NgoaiTe,bCoTKGT_TienViet,bCoTKGT_Tong
from KT_CanDoiThuChiTaiChinh cd where cd.iNam=@iNamLamViec and cd.iLoaiThuChi=2 and cd.iTrangThai=1 and cd.bHienThi=1 and iQuy=@iQuy order by iSTT");
            SqlCommand cmd = new SqlCommand(SQL);
           // cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
            cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            if (iID_MaTrangThaiDuyet != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            int iThangSoDuDauNam = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
            //  cmd.Parameters.AddWithValue("@iThangCT", iThangSoDuDauNam);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            //if (dt.Rows.Count == 0)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}
            //return dt;
            int ThuTu = 0;
            DataTable tbl = GetData_PhanChi(dt, 0, ref ThuTu, false, DenNgay, iThangSoDuDauNam, iID_MaTrangThaiDuyet, DVT, Convert.ToInt32(iQuy));
            dt.Dispose();
            return tbl;
        }
        public static DataTable GetData_PhanThu(DataTable dtTemp, int Cap, ref int ThuTu, Boolean isKhoangTrang, String DenNgay, int iThangCT, String iID_MaTrangThaiDuyet, String DVT, int iQuy)
        {
            String strDoanTrang = "";
            DataTable dt = new DataTable();

            dt.Columns.Add("iSTT", typeof(String));
            dt.Columns.Add("iID_MaThuChi", typeof(int));
            dt.Columns.Add("iID_MaThuChi_Cha", typeof(int));
            dt.Columns.Add("bLaHangCha", typeof(int));


            dt.Columns.Add("sNoiDung", typeof(String));
          
            dt.Columns.Add("rSoTienViet", typeof(decimal));
            dt.Columns.Add("rSoTienNgoai", typeof(decimal));


            dt.Columns.Add("rTongSoTien", typeof(decimal));

            if (dtTemp.Rows.Count > 0 && dtTemp != null)
            {

                for (int i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "     ";
                }
                foreach (DataRow dr in dtTemp.Rows)
                {
                    String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(dr["iID_MaThuChi_Cha"]);
                    if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet_Cha) == false &&
                        iID_MaTaiKhoanDanhMucChiTiet_Cha != "" && iID_MaTaiKhoanDanhMucChiTiet_Cha != "0")
                    {
                    }
                    else
                    {
                        ThuTu++;
                        Decimal rSoTienNgoai = 0, rTongSoTien = 0;
                        DataRow drMain = dt.NewRow();
                        drMain["iSTT"] = ThuTu.ToString();
                        drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                        drMain["iID_MaThuChi_Cha"] = Convert.ToInt32((dr["iID_MaThuChi_Cha"]));
                        if (Convert.ToBoolean(dr["bLaHangCha"]) == true)
                        {
                            drMain["bLaHangCha"] = 1;
                        }
                        else
                        {
                            drMain["bLaHangCha"] = 0;
                        }

                        if (isKhoangTrang == true)
                        {
                            drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                        }
                        else
                        {
                            drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                        }
                        // drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                        //drMain["rSoTienViet"] = 0;
                        int kt = KiemTra_LaHangCha(Convert.ToInt32(dr["iID_MaThuChi"]));
                        Boolean bTuDongTinh = Convert.ToBoolean(dr["bTuDongTinh"]);
                        String sTenTaiKhoan_NgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();
                        String sTenTaiKhoan_Tong = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                        Boolean bCoTKGT_NgoaiTe = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                        Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                        Boolean bTuTinhTong = Convert.ToBoolean(dr["bTuTinhTong"]);
                        //String DenNgay = "2013/12/31";
                        if (kt > 0)
                        {
                            if (bTuDongTinh == false)
                            {
                                // rSoTienNgoai = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                // rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                if (bCoTKGT_NgoaiTe)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            //String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            //String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            //    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            //    arrMaNgoaiTe[i].Trim().Length -
                                            //    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();

                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu("", "", "", "", iThangCT,
                                                                                         DenNgay,
                                                                                         false, iID_MaTrangThaiDuyet,
                                                                                         arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rSoTienNgoai = getTien_ByTK_ChiTiet_PhanThu("", "", "", "", iThangCT, DenNgay,
                                                                                bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet,
                                                                                Convert.ToString(
                                                                                    dr["sTenTaiKhoan_NgoaiTe"]));
                                }

                               
                            }
                            else
                            {
                                //if (dr["iID_MaThuChi"].ToString() == "23")
                                //{
                                    TinhTongTienThu_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoai,
                                               bTuDongTinh,
                                               sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                               DenNgay, iID_MaTrangThaiDuyet);
                                //}                            
                            }
                            ///tinh tong
                            if (bTuTinhTong == false)
                            {
                                if (bCoTKGT_Tong)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            //String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            //String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            //    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            //    arrMaNgoaiTe[i].Trim().Length -
                                            //    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi,iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "","", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rTongSoTien = getTien_ByTK_ChiTiet_PhanThu("", "","", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                }

                            }
                            else
                            {
                                TinhTongTienThu_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), 
                                           ref rTongSoTien, bTuTinhTong,
                                           sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                           DenNgay, iID_MaTrangThaiDuyet);
                            }
                            if (DVT == "0")
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai;
                                drMain["rTongSoTien"] = rTongSoTien;
                                drMain["rSoTienViet"] = rTongSoTien - rSoTienNgoai;
                            }
                            else if (DVT == "1")
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai / 1000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000;

                            }
                            else
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai / 1000000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000000;
                            }

                        }
                        else
                        {
                            if (bTuDongTinh == false)
                            {
                                if (bCoTKGT_NgoaiTe)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            //String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            //String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            //    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            //    arrMaNgoaiTe[i].Trim().Length -
                                            //    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rSoTienNgoai = getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                }

                                
                              
                            }
                            else
                            {
                                getTien_ThuCon_ByCha_NgoaiTe(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 1,
                                                     ref rSoTienNgoai,iID_MaTrangThaiDuyet, iQuy);
                               
                            }
                            if (bTuTinhTong == false)
                            {
                                if (bCoTKGT_Tong)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            //String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            //String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            //    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            //    arrMaNgoaiTe[i].Trim().Length -
                                            //    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rTongSoTien = getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                }
                            }
                            else
                            {
                                getTien_ThuCon_ByCha_TongTien(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 1,
                                                     ref rTongSoTien, iID_MaTrangThaiDuyet, iQuy);

                            }

                            if (DVT == "0")
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai;
                                drMain["rTongSoTien"] = rTongSoTien;
                                drMain["rSoTienViet"] = rTongSoTien - rSoTienNgoai;
                            }
                            else if (DVT == "1")
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai / 1000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000;

                            }
                            else
                            {
                                drMain["rSoTienNgoai"] = rSoTienNgoai / 1000000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000000;
                            }

                        }
                        
                        
                        dt.Rows.Add(drMain);
                        addchildren_PhanThu(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap,
                                            ref ThuTu, isKhoangTrang, DenNgay, iThangCT, iID_MaTrangThaiDuyet, DVT, iQuy);
                    }
                }
                dtTemp.Dispose();

            }
            else {
                dt.Rows.Add(dt.NewRow());
            }

            return dt;
        }

        private static void addchildren_PhanThu(DataTable dtTemp, int menuid, ref  DataTable dt, int Cap, ref int ThuTu, Boolean isKhoangTrang, String DenNgay, int iThangCT, String iID_MaTrangThaiDuyet, String DVT, int iQuy)
        {
            String strDoanTrang = "";
            for (int i = 1; i <= Cap; i++)
            {
                strDoanTrang += "     ";
            }
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ThuTu++;
                    Decimal rSoTienNgoai = 0, rTongSoTien = 0;
                    DataRow drMain = dt.NewRow();
                    drMain["iSTT"] = ThuTu.ToString();
                    drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                    drMain["iID_MaThuChi_Cha"] = Convert.ToInt32((dr["iID_MaThuChi_Cha"]));
                
                    if (Convert.ToBoolean(dr["bLaHangCha"]) == true)
                    {
                        drMain["bLaHangCha"] = 2;
                    }
                    else
                    {
                        if (Cap > 0)
                        {
                            drMain["bLaHangCha"] = 3;
                        }
                        else
                        {
                            drMain["bLaHangCha"] = 0;
                        }
                    }
                    if (isKhoangTrang == true)
                    {
                        drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                    }
                    else
                    {
                        drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                    }
                    // drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                   // drMain["rSoTienViet"] = 0;
                    int kt = KiemTra_LaHangCha(Convert.ToInt32(dr["iID_MaThuChi"]));
                    Boolean bTuDongTinh = Convert.ToBoolean(dr["bTuDongTinh"]);
                    String sTenTaiKhoan_NgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();
                    String sTenTaiKhoan_Tong = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                    Boolean bCoTKGT_NgoaiTe = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    Boolean bTuTinhTong = Convert.ToBoolean(dr["bTuTinhTong"]);

                    if (kt > 0)
                    {
                        if (bTuDongTinh == false)
                        {
                            // rSoTienNgoai = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            // rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            if (bCoTKGT_NgoaiTe)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                        String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                        String iID_MaChiTiet = arrChuoi[1].Trim();
                                        String iID_MaPhongBan = "";
                                        String iID_MaDonVi = "";
                                        if (arrChuoi.Count() > 2)
                                        {
                                            iID_MaPhongBan = arrChuoi[2].Trim();
                                        }
                                        if (arrChuoi.Count() > 3)
                                        {
                                            iID_MaDonVi = arrChuoi[3].Trim();
                                        }
                                        rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi,iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rSoTienNgoai = getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            }


                        }
                        else
                        {
                            TinhTongTienThu_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoai,
                                       bTuDongTinh,
                                       sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                       DenNgay, iID_MaTrangThaiDuyet);
                        }
                        ///tinh tong
                        if (bTuTinhTong == false)
                        {
                            if (bCoTKGT_Tong)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                        String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                        String iID_MaChiTiet = arrChuoi[1].Trim();
                                        String iID_MaPhongBan = "";
                                        String iID_MaDonVi = "";
                                        if (arrChuoi.Count() > 2)
                                        {
                                            iID_MaPhongBan = arrChuoi[2].Trim();
                                        }
                                        if (arrChuoi.Count() > 3)
                                        {
                                            iID_MaDonVi = arrChuoi[3].Trim();
                                        }
                                        rTongSoTien += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "", "", "", iThangCT, DenNgay,
                                                                                    false, iID_MaTrangThaiDuyet,
                                                                                    arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rTongSoTien = getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            }

                        }
                        else
                        {
                            TinhTongTienThu_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()),
                                       ref rTongSoTien, bTuTinhTong,
                                       sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                       DenNgay, iID_MaTrangThaiDuyet);
                        }
                        if (DVT == "0")
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai;
                            drMain["rTongSoTien"] = rTongSoTien;
                            drMain["rSoTienViet"] = rTongSoTien - rSoTienNgoai;
                        }
                        else if (DVT == "1")
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai / 1000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000;

                        }
                        else
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai / 1000000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000000;
                        }

                    }
                    else
                    {
                        if (bTuDongTinh == false)
                        {
                            if (bCoTKGT_NgoaiTe)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                        String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                        String iID_MaChiTiet = arrChuoi[1].Trim();
                                        String iID_MaPhongBan = "";
                                        String iID_MaDonVi = "";
                                        if (arrChuoi.Count() > 2)
                                        {
                                            iID_MaPhongBan = arrChuoi[2].Trim();
                                        }
                                        if (arrChuoi.Count() > 3)
                                        {
                                            iID_MaDonVi = arrChuoi[3].Trim();
                                        }
                                        rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rSoTienNgoai += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rSoTienNgoai = getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            }



                        }
                        else
                        {
                            getTien_ThuCon_ByCha_NgoaiTe(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 1,
                                                 ref rSoTienNgoai, iID_MaTrangThaiDuyet, iQuy);

                        }

                        if (bTuTinhTong == false)
                        {
                           // if (Convert.ToString(dr["iID_MaThuChi"]) == "342")
                            //{
                                if (bCoTKGT_Tong)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "", "", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rTongSoTien = getTien_ByTK_ChiTiet_PhanThu("", "", "", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                }
                         // }
                        }
                        else
                        {
                            getTien_ThuCon_ByCha_TongTien(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 1,
                                                 ref rTongSoTien, iID_MaTrangThaiDuyet, iQuy);

                        }

                        if (DVT == "0")
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai;
                            drMain["rTongSoTien"] = rTongSoTien;
                            drMain["rSoTienViet"] = rTongSoTien - rSoTienNgoai;
                        }
                        else if (DVT == "1")
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai / 1000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000;

                        }
                        else
                        {
                            drMain["rSoTienNgoai"] = rSoTienNgoai / 1000000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienNgoai) / 1000000;
                        }
                   
                    }

                  

                    dt.Rows.Add(drMain);
                    
                    addchildren_PhanThu(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap + 1,
                                       ref ThuTu, isKhoangTrang, DenNgay, iThangCT, iID_MaTrangThaiDuyet, DVT, iQuy);
                }
                else
                {
                }
            }
        }
        public static void TinhTongTienThu_NgoaiTe(DataTable dtTemp, int menuid, ref  Decimal rSoTienNgoaiTe, Boolean bTuDongTinh,
       string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ////Tinh Tien NgoaiTe
                   
                        if (Convert.ToBoolean(dr["bTuDongTinh"]) == true)
                        {
                            TinhTongTienThu_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoaiTe,
                                            Convert.ToBoolean(dr["bTuDongTinh"]),
                                            sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                            DenNgay, iID_MaTrangThaiDuyet);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim()) == false)
                            {
                                if (Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]))
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rSoTienNgoaiTe += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi, iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rSoTienNgoaiTe += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rSoTienNgoaiTe += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                    //rSoTienNgoaiTe += HamChung.ConvertToDecimal(dr["rSoTienNgoaiTe"]);
                                }
                            }
                    }
                   
                    //Neu tu dong tinh
                  

                }
            }
        }
        public static void TinhTongTienThu_TongTien(DataTable dtTemp, int menuid, ref Decimal rTongSoTien, Boolean bTuDongTinh,
           string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                   
                    ////Tinh Tien Tong
                    //if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_Tong"])) == false)
                   // {
                        if (Convert.ToBoolean(dr["bTuTinhTong"]) == true)
                        {
                            TinhTongTienThu_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rTongSoTien,
                                            Convert.ToBoolean(dr["bTuTinhTong"]),
                                            sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                            DenNgay, iID_MaTrangThaiDuyet);
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim()) == false)
                            {
                                if (Convert.ToBoolean(dr["bCoTKGT_Tong"]))
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                            String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                            String iID_MaChiTiet = arrChuoi[1].Trim();
                                            String iID_MaPhongBan = "";
                                            String iID_MaDonVi = "";
                                            if (arrChuoi.Count() > 2)
                                            {
                                                iID_MaPhongBan = arrChuoi[2].Trim();
                                            }
                                            if (arrChuoi.Count() > 3)
                                            {
                                                iID_MaDonVi = arrChuoi[3].Trim();
                                            }
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay,
                                                                                 Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                                        }
                                        else  if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }

                                    }
                                }
                                else
                                {
                                    rTongSoTien += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                    // rTongSoTien += HamChung.ConvertToDecimal(dr["rSoTienTong"]);
                                }
                            }
                        }
                  //  }
                    

                }
            }
        }
        public static void TinhTongTienThu(DataTable dtTemp, int menuid, ref  Decimal rSoTienNgoaiTe, ref Decimal rTongSoTien, Boolean bTuDongTinh,
            string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ////Tinh Tien NgoaiTe
                    if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim()) == false)
                    {
                        if (Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]))
                        {
                            String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                            for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                            {
                                if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                {
                                    String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                    String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                        arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                        arrMaNgoaiTe[i].Trim().Length -
                                        arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                    rSoTienNgoaiTe += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                                }

                            }
                        }
                        else
                        {
                            rSoTienNgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            //rSoTienNgoaiTe += HamChung.ConvertToDecimal(dr["rSoTienNgoaiTe"]);
                        }
                    }
                    ////Tinh Tien Tong
                    if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim()) == false)
                    {
                        if (Convert.ToBoolean(dr["bCoTKGT_Tong"]))
                        {
                            String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                            for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                            {
                                if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                {
                                    String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                    String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                        arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                        arrMaNgoaiTe[i].Trim().Length -
                                        arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                    rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay,
                                                                         Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                                }

                            }
                        }
                        else
                        {
                            rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            // rTongSoTien += HamChung.ConvertToDecimal(dr["rSoTienTong"]);
                        }
                    }
                    //Neu tu dong tinh
                    if (bTuDongTinh)
                    {
                        TinhTongTienThu(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoaiTe,
                                        ref rTongSoTien, bTuDongTinh,
                                        sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                        DenNgay,iID_MaTrangThaiDuyet);
                    }
                   
                }
            }
        }
        public static Decimal getTien_ByTK_ChiTiet_PhanThu(String iID_MaTaiKhoan, String iID_MaChiTiet, String iID_MaPhongBan, String iID_MaDonVi, int iThangCT, String DenNgay, Boolean bCoTKGT, String iID_MaTrangThaiDuyet, String SChuoi)
        {
            Decimal vR = 0;
            if (bCoTKGT == true)
            {
                String DK_Co = "", DK_No = "";
                if (String.IsNullOrEmpty(iID_MaPhongBan) == false)
                {
                    DK_Co += " AND iID_MaPhongBan_Co=@iID_MaPhongBan";
                    DK_No += " AND iID_MaPhongBan_No=@iID_MaPhongBan";
                }
                if (String.IsNullOrEmpty(iID_MaDonVi) == false)
                {
                    DK_Co += " AND iID_MaDonVi_Co=@iID_MaDonVi";
                    DK_No += " AND iID_MaDonVi_No=@iID_MaDonVi";
                }
                string sql = "";
                if (String.IsNullOrEmpty(iID_MaChiTiet) == false)
                {
                    sql = String.Format(
                        @"select rSoTien=SUM(rSoTienNo)+SUM(rSoTienCo) from (
                select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT and ct.iTrangThai=1 {0}{1}
                AND ct.iID_MaTaiKhoan_No !=''
                                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay
                				and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan
                				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
                          union
                                select  rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT  and ct.iTrangThai=1 {0}{2}
                AND ct.iID_MaTaiKhoan_Co !='' AND ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan 		
                                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay			
                				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
                		union
                				select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1 {1} 		
                				AND ct.iID_MaTaiKhoan_No !='' and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan 	
                				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
                        union
                				select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1 {2}
                AND ct.iID_MaTaiKhoan_Co !='' and ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan  			
                			    and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%') as CT",
                        iID_MaTrangThaiDuyet, DK_No, DK_Co);
                }
                else
                {
                    sql = String.Format(
                       @"select rSoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo))ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1 END from (
                select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and ct.iTrangThai=1 {0}{1}
                AND ct.iID_MaTaiKhoan_No !=''
                                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay
                				and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan
                				
                          union
                                select  rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec   and ct.iTrangThai=1 {0}{2}
                AND ct.iID_MaTaiKhoan_Co !='' AND ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan 		
                                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay) as CT",
                       iID_MaTrangThaiDuyet, DK_No, DK_Co);
                }
                SqlCommand cmd =
                    new SqlCommand(sql);

                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                if (String.IsNullOrEmpty(iID_MaChiTiet) == false)
                {
                    cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
                }
                cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                if (String.IsNullOrEmpty(iID_MaChiTiet) == false)
                {
                    cmd.Parameters.AddWithValue("@iID_MaChiTiet", iID_MaChiTiet);
                }
                if (String.IsNullOrEmpty(iID_MaPhongBan) == false)
                {
                    cmd.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                }
                if (String.IsNullOrEmpty(iID_MaDonVi) == false)
                {
                    cmd.Parameters.AddWithValue("@iID_MaDonVi", iID_MaDonVi);
                }
                vR = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                // }
            }
            else
            {
                if (String.IsNullOrEmpty(SChuoi) == false)
                {
                    //if (SChuoi == "33382")
                    //{
                    SqlCommand cmd = new SqlCommand();
                    String[] arriID_MaTaiKhoan = SChuoi.Trim().Split(',');
                    String DK_Co = "", DK_No = "";
                    String MaTaiKhoan_No = " AND (";
                    String MaTaiKhoan_Co = " AND (";
                    if (arriID_MaTaiKhoan.Count() > 1)
                    {
                        for (int i = 0; i < arriID_MaTaiKhoan.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arriID_MaTaiKhoan[i]) == false)
                            {
                                if (arriID_MaTaiKhoan[i].IndexOf('-') > 0)
                                {
                                    String[] arrChuoi = arriID_MaTaiKhoan[i].Trim().Split('-');
                                    if (MaTaiKhoan_No == " AND (")
                                    {
                                        MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    else
                                    {
                                        MaTaiKhoan_No += " OR ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    if (MaTaiKhoan_Co == " AND (")
                                    {
                                        MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    else
                                    {
                                        MaTaiKhoan_Co += " OR ct.iID_MaTaiKhoan_Co LIKE  @iID_MaTaiKhoan" + i;
                                    }
                                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arrChuoi[0].Trim() + "%");
                                    ///
                                    if (arrChuoi.Count() > 1)
                                    {
                                        DK_Co += " AND iID_MaPhongBan_Co=@iID_MaPhongBan";
                                        DK_No += " AND iID_MaPhongBan_No=@iID_MaPhongBan";
                                        cmd.Parameters.AddWithValue("@iID_MaPhongBan" + i, arrChuoi[1].Trim());
                                    }
                                    if (arrChuoi.Count() > 2)
                                    {
                                        DK_Co += " AND iID_MaDonVi_Co=@iID_MaDonVi";
                                        DK_No += " AND iID_MaDonVi_No=@iID_MaDonVi";
                                        cmd.Parameters.AddWithValue("@iID_MaDonVi" + i, arrChuoi[2].Trim());
                                    }
                                }
                                else
                                {
                                    if (MaTaiKhoan_No == " AND (")
                                    {
                                        MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    else
                                    {
                                        MaTaiKhoan_No += " OR ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    if (MaTaiKhoan_Co == " AND (")
                                    {
                                        MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan" + i;
                                    }
                                    else
                                    {
                                        MaTaiKhoan_Co += " OR ct.iID_MaTaiKhoan_Co LIKE  @iID_MaTaiKhoan" + i;
                                    }
                                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arriID_MaTaiKhoan[i] + "%");
                                }
                            }
                        }
                        MaTaiKhoan_No += " )";
                        MaTaiKhoan_Co += " )";
                    }
                    else
                    {
                        if (SChuoi.IndexOf('-') < 0)
                        {
                            MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan + '%')";
                            MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan + '%')";
                            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", SChuoi);
                        }
                        else
                        {
                            String[] arrChuoi = SChuoi.Trim().Split('-');
                            MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan + '%')";
                            MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan + '%')";
                            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", arrChuoi[0].Trim());
                            if (arrChuoi.Count() > 1)
                            {
                                DK_Co += " AND iID_MaPhongBan_Co=@iID_MaPhongBan";
                                DK_No += " AND iID_MaPhongBan_No=@iID_MaPhongBan";
                                cmd.Parameters.AddWithValue("@iID_MaPhongBan", arrChuoi[1].Trim());
                            }
                            if (arrChuoi.Count() > 2)
                            {
                                DK_Co += " AND iID_MaDonVi_Co=@iID_MaDonVi";
                                DK_No += " AND iID_MaDonVi_No=@iID_MaDonVi";
                                cmd.Parameters.AddWithValue("@iID_MaDonVi", arrChuoi[2].Trim());
                            }
                        }
                    }
                   
                  
                    String SQL = String.Format(
                        @"select rSoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo))ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1 END
							from (select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iThangCT>0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_No !='' 
and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay {1} {3}
--and ct.iID_MaTaiKhoan_No in (@iID_MaTaiKhoan)
union all
select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iThangCT=0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_No !='' {1} {3}
--and ct.iID_MaTaiKhoan_No in (@iID_MaTaiKhoan)
union
							select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iThangCT=0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_Co !='' {2} {4}
--and ct.iID_MaTaiKhoan_Co in (@iID_MaTaiKhoan)
								union all
							select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iThangCT>0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_Co !=''
and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay {2} {4}
--and ct.iID_MaTaiKhoan_Co in (@iID_MaTaiKhoan)
								) as T",
                        iID_MaTrangThaiDuyet, MaTaiKhoan_No, MaTaiKhoan_Co, DK_No, DK_Co);
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                    cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                 
                    vR = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    // }
                }
            }

            return vR;
        }
        public static Decimal getTien_ByTK_ChiTiet(String iID_MaTaiKhoan, String iID_MaChiTiet, int iThangCT, String DenNgay, Boolean bCoTKGT, String iID_MaTrangThaiDuyet, String SChuoi)
        {
            Decimal vR = 0;
            if (bCoTKGT == true)
            {

//                SqlCommand cmd =
//                    new SqlCommand(
//                        String.Format(
//                            @"select rSoTien=SUM(rSoTienNo)+SUM(rSoTienCo) from (
//select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT and ct.iTrangThai=1 {0}
//AND ct.iID_MaTaiKhoan_No !=''
//                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay
//				and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan
//				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
//          union
//                select  rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT  and ct.iTrangThai=1 {0}
//AND ct.iID_MaTaiKhoan_Co !='' AND ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan 		
//                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay			
//				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
//		union
//				select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1 		
//				AND ct.iID_MaTaiKhoan_No !='' and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan 	
//				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
//        union
//				select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1
//AND ct.iID_MaTaiKhoan_Co !='' and ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan  			
//			    and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%') as CT",
//                            iID_MaTrangThaiDuyet));

                SqlCommand cmd =
                    new SqlCommand(
                        String.Format(
                            @"select rSoTien=(CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo)) ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1 END) from (
select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT and ct.iTrangThai=1 {0}
AND ct.iID_MaTaiKhoan_No !=''
                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay
				and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan
				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
          union
                select  rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iNamLamViec=@iNamLamViec and  ct.iThangCT>@iThangCT  and ct.iTrangThai=1 {0}
AND ct.iID_MaTaiKhoan_Co !='' AND ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan 		
                and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay			
				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
		union
				select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1 		
				AND ct.iID_MaTaiKhoan_No !='' and ct.iID_MaTaiKhoan_No=@iID_MaTaiKhoan 	
				and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_No) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%'
        union
				select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_SoDuTaiKhoanGiaiThich ct where ct.iNamLamViec=@iNamLamViec  and ct.iTrangThai=1
AND ct.iID_MaTaiKhoan_Co !='' and ct.iID_MaTaiKhoan_Co=@iID_MaTaiKhoan  			
			    and (select RTRIM(LTRIM(sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanGiaiThich_Co) like (select RTRIM(LTRIM(ct.sXauNoiMa_Cha)) AS sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet ct, KT_TaiKhoanGiaiThich tk where ct.iTrangThai=1 and tk.iTrangThai=1 and ct.iID_MaTaiKhoanDanhMucChiTiet=tk.iID_MaTaiKhoanDanhMucChiTiet and  tk.iID_MaTaiKhoan=@iID_MaTaiKhoan and ct.sKyHieu=@iID_MaChiTiet)  + '%') as CT",
                            iID_MaTrangThaiDuyet));
                cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                cmd.Parameters.AddWithValue("@iThangCT", iThangCT);
                cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                cmd.Parameters.AddWithValue("@iID_MaChiTiet", iID_MaChiTiet);
                vR = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                cmd.Dispose();
                // }
            }
            else
            {
                if (String.IsNullOrEmpty(SChuoi) == false)
                {
                    //if (SChuoi == "33382")
                    //{
                    SqlCommand cmd = new SqlCommand();
                    String[] arriID_MaTaiKhoan = SChuoi.Trim().Split(',');
                    String MaTaiKhoan_No = " AND (";
                    String MaTaiKhoan_Co = " AND (";
                    if (arriID_MaTaiKhoan.Count() > 1)
                    {
                        for (int i = 0; i < arriID_MaTaiKhoan.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arriID_MaTaiKhoan[i]) == false)
                            {
                                if (MaTaiKhoan_No == " AND (")
                                {
                                    MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                }
                                else
                                {
                                    MaTaiKhoan_No += " OR ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan" + i;
                                }
                                if (MaTaiKhoan_Co == " AND (")
                                {
                                    MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan" + i;
                                }
                                else
                                {
                                    MaTaiKhoan_Co += " OR ct.iID_MaTaiKhoan_Co LIKE  @iID_MaTaiKhoan" + i;
                                }
                                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan" + i, arriID_MaTaiKhoan[i] + "%");
                            }
                        }
                        MaTaiKhoan_No += " )";
                        MaTaiKhoan_Co += " )";
                    }
                    else
                    {
                        MaTaiKhoan_No += " ct.iID_MaTaiKhoan_No LIKE @iID_MaTaiKhoan + '%')";
                        MaTaiKhoan_Co += " ct.iID_MaTaiKhoan_Co LIKE @iID_MaTaiKhoan + '%')";
                        cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", SChuoi);
                    }

                    String SQL = String.Format(
                        @"select rSoTien=CASE WHEN (SUM(rSoTienNo)-SUM(rSoTienCo)>=0) THEN (SUM(rSoTienNo)-SUM(rSoTienCo))ELSE (SUM(rSoTienNo)-SUM(rSoTienCo))*-1 END
							from (select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iThangCT>0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_No !='' 
and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay {1}
--and ct.iID_MaTaiKhoan_No in (@iID_MaTaiKhoan)
union all
select SUM(rSoTien) as rSoTienNo, rSoTienCo=0 from KT_ChungTuChiTiet ct where ct.iThangCT=0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_No !='' {1}
--and ct.iID_MaTaiKhoan_No in (@iID_MaTaiKhoan)
union
							select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iThangCT=0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_Co !='' {2}
--and ct.iID_MaTaiKhoan_Co in (@iID_MaTaiKhoan)
								union all
							select rSoTienNo=0, SUM(rSoTien) as rSoTienCo from KT_ChungTuChiTiet ct where ct.iThangCT>0 AND ct.iTrangThai=1 {0} and ct.iNamLamViec=@iNamLamViec and ct.iID_MaTaiKhoan_Co !=''
and CONVERT(Datetime, CONVERT(varchar, ct.iNamLamViec) + '/' + CONVERT(varchar, ct.iThangCT) + '/' + CONVERT(varchar, ct.iNgayCT), 111)<=@DenNgay {2}
--and ct.iID_MaTaiKhoan_Co in (@iID_MaTaiKhoan)
								) as T",
                        iID_MaTrangThaiDuyet, MaTaiKhoan_No, MaTaiKhoan_Co);
                    cmd.CommandText = SQL;
                    cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                    cmd.Parameters.AddWithValue("@DenNgay", DenNgay);
                    //cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", SChuoi);
                    vR = Convert.ToDecimal(Connection.GetValue(cmd, 0));
                    cmd.Dispose();
                    // }
                }
            }
           
            return vR;
        }
        public static void getTien_ThuCon_ByCha_NgoaiTe(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienNgoaiTe, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con == "25")
            //{
            Decimal rSoTienThuCha_NgoaiTe = 0, rSoTienThuCha_Tong = 0;
            Decimal rSoThienThuCon_NgoaiTe = 0, rSoThienThuCon_Tong = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_NgoaiTe = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_NgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                    // ngoai te
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]))
                    {
                        String[] arrMaNgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                String iID_MaChiTiet = arrChuoi[1].Trim();
                                String iID_MaPhongBan = "";
                                String iID_MaDonVi = "";
                                if (arrChuoi.Count() > 2)
                                {
                                    iID_MaPhongBan = arrChuoi[2].Trim();
                                }
                                if (arrChuoi.Count() > 3)
                                {
                                    iID_MaDonVi = arrChuoi[3].Trim();
                                }
                                rSoTienThuCha_NgoaiTe += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_NgoaiTe += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }

                        }
                    }
                    else
                    {
                        rSoTienThuCha_NgoaiTe = getTien_ByTK_ChiTiet_PhanThu("", "","","", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_NgoaiTe);
                    }
                  
                }
            }

            string sql = @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                    Boolean bCoTKGT_NgoaiTe = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    String sTenTaiKhoanCon_NgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();

                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    String sTenTaiKhoanCon_Tong_CT = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                    if (bCoTKGT_NgoaiTe)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_NgoaiTe.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                String iID_MaChiTiet = arrChuoi[1].Trim();
                                String iID_MaPhongBan = "";
                                String iID_MaDonVi = "";
                                if (arrChuoi.Count() > 2)
                                {
                                    iID_MaPhongBan = arrChuoi[2].Trim();
                                }
                                if (arrChuoi.Count() > 3)
                                {
                                    iID_MaDonVi = arrChuoi[3].Trim();
                                }
                                rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet,iID_MaPhongBan,iID_MaDonVi, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet_PhanThu("", "","","", 0, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                    }
                   

                }
            }
            cmd.Dispose();
            rSoTienNgoaiTe = rSoTienThuCha_NgoaiTe - rSoThienThuCon_NgoaiTe;
           
        }
        public static void getTien_ThuCon_ByCha_TongTien(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienTong, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con == "25")
            //{
            Decimal rSoTienThuCha_NgoaiTe = 0, rSoTienThuCha_Tong = 0;
            Decimal rSoThienThuCon_NgoaiTe = 0, rSoThienThuCon_Tong = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_NgoaiTe = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_NgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                    
                    //tong tien
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]))
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoan_Tong.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                String iID_MaChiTiet = arrChuoi[1].Trim();
                                String iID_MaPhongBan = "";
                                String iID_MaDonVi = "";
                                if (arrChuoi.Count() > 2)
                                {
                                    iID_MaPhongBan = arrChuoi[2].Trim();
                                }
                                if (arrChuoi.Count() > 3)
                                {
                                    iID_MaDonVi = arrChuoi[3].Trim();
                                }
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                            }

                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }

                        }
                    }
                    else
                    {
                        rSoTienThuCha_Tong = getTien_ByTK_ChiTiet_PhanThu("", "","","", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                    }

                    //rSoTienThuCha_NgoaiTe = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_NgoaiTe);
                    // rSoTienThuCha_Tong = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                }
            }

            string sql = @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                    Boolean bCoTKGT_NgoaiTe = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    String sTenTaiKhoanCon_NgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();

                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    String sTenTaiKhoanCon_Tong_CT = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                   
                    if (bCoTKGT_Tong)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_Tong_CT.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String[] arrChuoi = arrMaNgoaiTe[i].Trim().Split('-');
                                String iID_MaTaiKhoan = arrChuoi[0].Trim();
                                String iID_MaChiTiet = arrChuoi[1].Trim();
                                String iID_MaPhongBan = "";
                                String iID_MaDonVi = "";
                                if (arrChuoi.Count() > 2)
                                {
                                    iID_MaPhongBan = arrChuoi[2].Trim();
                                }
                                if (arrChuoi.Count() > 3)
                                {
                                    iID_MaDonVi = arrChuoi[3].Trim();
                                }
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet_PhanThu(iID_MaTaiKhoan, iID_MaChiTiet, iID_MaPhongBan, iID_MaDonVi, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet_PhanThu("", "","","", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_Tong += getTien_ByTK_ChiTiet_PhanThu("", "","","", 0, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                    }

                }
            }
            cmd.Dispose();
          
            rSoTienTong = rSoTienThuCha_Tong - rSoThienThuCon_Tong;
            //}
        }
        public static void getTien_ThuCon_ByCha(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienNgoaiTe, ref  Decimal rSoTienTong, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con == "25")
            //{
            Decimal rSoTienThuCha_NgoaiTe = 0, rSoTienThuCha_Tong = 0;
            Decimal rSoThienThuCon_NgoaiTe = 0, rSoThienThuCon_Tong = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_NgoaiTe = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_NgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                    // ngoai te
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]))
                    {
                        String[] arrMaNgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_NgoaiTe += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_NgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }

                        }
                    }
                    else
                    {
                        rSoTienThuCha_NgoaiTe = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_NgoaiTe);
                    }
                    //tong tien
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]))
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoan_Tong.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }


                        }
                    }
                    else
                    {
                        rSoTienThuCha_Tong = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                    }

                    //rSoTienThuCha_NgoaiTe = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_NgoaiTe);
                    // rSoTienThuCha_Tong = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                }
            }

            string sql = @"select iID_MaThuChi,bCoTKGT_NgoaiTe,sTenTaiKhoan_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                    Boolean bCoTKGT_NgoaiTe = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    String sTenTaiKhoanCon_NgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();

                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    String sTenTaiKhoanCon_Tong_CT = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                    if (bCoTKGT_NgoaiTe)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_NgoaiTe.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_NgoaiTe += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                    }
                    if (bCoTKGT_Tong)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_Tong_CT.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_NgoaiTe);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                    }

                }
            }
            cmd.Dispose();
            rSoTienNgoaiTe = rSoTienThuCha_NgoaiTe - rSoThienThuCon_NgoaiTe;
            rSoTienTong = rSoTienThuCha_Tong - rSoThienThuCon_Tong;
            //}
        }
        #region "Phan chi"

        public static void getTien_ChiCon_ByCha(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienViet, ref  Decimal rSoTienTong, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con=="118")
            //{
                
            
            Decimal rSoTienThuCha_TienViet = 0, rSoTienThuCha_Tong = 0;
            Decimal rSoThienThuCon_TienViet = 0, rSoThienThuCon_Tong = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi, sTenTaiKhoan_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_NgoaiTe,bCoTKGT_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_TienViet = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_TienViet = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                    // ngoai te
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]))
                    {
                        String[] arrMaNgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_TienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_TienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoTienThuCha_TienViet = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_TienViet);
                    }
                    //tong tien
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]))
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoan_Tong.Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoTienThuCha_Tong = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                    }
                   
                }
                dt.Dispose();
            }

            string sql = @"select bCoTKGT_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_NgoaiTe,sTenTaiKhoan_Tong, sNoiDung  from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                    Boolean bCoTKGT_TienViet = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    String sTenTaiKhoanCon_TienViet = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();

                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    String sTenTaiKhoanCon_Tong_CT = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                    if (bCoTKGT_TienViet)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_TienViet.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_TienViet, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_TienViet);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_TienViet);
                    }
                    if (bCoTKGT_Tong)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_Tong_CT.Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                    }

                }
                tbl.Dispose();
            }
            cmd.Dispose();
            rSoTienViet = rSoTienThuCha_TienViet - rSoThienThuCon_TienViet;
            rSoTienTong = rSoTienThuCha_Tong - rSoThienThuCon_Tong;
            //}
        }
        public static void getTien_ChiCon_ByCha_TongTien(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienTong, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con=="118")
            //{


            Decimal  rSoTienThuCha_Tong = 0;
            Decimal  rSoThienThuCon_Tong = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi, sTenTaiKhoan_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_NgoaiTe,bCoTKGT_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_TienViet = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_TienViet = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                   
                    //tong tien
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]))
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoan_Tong.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoTienThuCha_Tong = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_Tong);
                    }

                }
                dt.Dispose();
            }

            string sql = @"select bCoTKGT_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_NgoaiTe,sTenTaiKhoan_Tong, sNoiDung  from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                   

                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    String sTenTaiKhoanCon_Tong_CT = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                   
                    if (bCoTKGT_Tong)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_Tong_CT.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_Tong += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_Tong_CT);
                    }

                }
                tbl.Dispose();
            }
            cmd.Dispose();
          
            rSoTienTong = rSoTienThuCha_Tong - rSoThienThuCon_Tong;
            //}
        }
        public static void getTien_ChiCon_ByCha_NgoaiTe(String iID_MaThuChi_Con, int iThangCT, String DenNgay, int iLoaiThuChi, ref  Decimal rSoTienViet, String iID_MaTrangThaiDuyet, int iQuy)
        {
            //if (iID_MaThuChi_Con=="118")
            //{


            Decimal rSoTienThuCha_TienViet = 0;
            Decimal rSoThienThuCon_TienViet = 0;
            SqlCommand cmd =
                new SqlCommand(
                    @"select iID_MaThuChi, sTenTaiKhoan_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_NgoaiTe,bCoTKGT_Tong, sNoiDung from KT_CanDoiThuChiTaiChinh where iTrangThai=1 and iNam=@iNam and iID_MaThuChi IN (SELECT TOP 1 iID_MaThuChi_Cha from  KT_CanDoiThuChiTaiChinh where iTrangThai=1 AND iNam=@iNam AND iID_MaThuChi=@iID_MaThuChi)");
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            cmd.Parameters.Clear();
            Decimal iID_MaThuChi_Cha = 0;
            String sTenTaiKhoan_TienViet = "", sTenTaiKhoan_Tong = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                iID_MaThuChi_Cha = Convert.ToDecimal(dt.Rows[0]["iID_MaThuChi"]);
                sTenTaiKhoan_TienViet = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim();
                sTenTaiKhoan_Tong = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_Tong"]).Trim();
                if (iID_MaThuChi_Cha > 0)
                {
                    // ngoai te
                    if (Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]))
                    {
                        String[] arrMaNgoaiTe = Convert.ToString(dt.Rows[0]["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoTienThuCha_TienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoTienThuCha_TienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoTienThuCha_TienViet = getTien_ByTK_ChiTiet("", "", 0, DenNgay, Convert.ToBoolean(dt.Rows[0]["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, sTenTaiKhoan_TienViet);
                    }
                   
                }
                dt.Dispose();
            }

            string sql = @"select bCoTKGT_NgoaiTe,bCoTKGT_Tong,sTenTaiKhoan_NgoaiTe,sTenTaiKhoan_Tong, sNoiDung  from KT_CanDoiThuChiTaiChinh where iQuy=@iQuy AND iID_MaThuChi !=@iID_MaThuChi AND iID_MaThuChi_Cha=@iID_MaThuChi_Cha and bHienThi=1 and iTrangThai=1 and iNam=@iNam and iLoaiThuChi=@iLoaiThuChi";
            cmd = new SqlCommand(sql);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@iQuy", iQuy);
            cmd.Parameters.AddWithValue("@iNam", NguoiDungCauHinhModels.iNamLamViec);
            cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi_Con);
            cmd.Parameters.AddWithValue("@iID_MaThuChi_Cha", iID_MaThuChi_Cha);
            cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            DataTable tbl = Connection.GetDataTable(cmd);
            if (tbl != null && tbl.Rows.Count > 0)
            {

                for (int j = 0; j < tbl.Rows.Count; j++)
                {
                    DataRow dr = tbl.Rows[j];
                    Boolean bCoTKGT_TienViet = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    String sTenTaiKhoanCon_TienViet = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();
                    if (bCoTKGT_TienViet)
                    {
                        String[] arrMaNgoaiTe = sTenTaiKhoanCon_TienViet.Split(',');
                        for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                        {
                            if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                            {
                                String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                    arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                    arrMaNgoaiTe[i].Trim().Length -
                                    arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT,
                                                                               DenNgay,
                                                                               bCoTKGT_TienViet, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_TienViet);
                            }
                            else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                            {
                                rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    false, iID_MaTrangThaiDuyet,
                                                                    arrMaNgoaiTe[i]);
                            }
                        }
                    }
                    else
                    {
                        rSoThienThuCon_TienViet += getTien_ByTK_ChiTiet("", "", 0, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, sTenTaiKhoanCon_TienViet);
                    }
                    
                }
                tbl.Dispose();
            }
            cmd.Dispose();
            rSoTienViet = rSoTienThuCha_TienViet - rSoThienThuCon_TienViet;
           
            //}
        }
        public static DataTable GetData_PhanChi(DataTable dtTemp, int Cap, ref int ThuTu, Boolean isKhoangTrang, String DenNgay, int iThangCT, String iID_MaTrangThaiDuyet, String DVT, int iQuy)
        {
            String strDoanTrang = "";
            DataTable dt = new DataTable();

            dt.Columns.Add("iSTT", typeof(String));
            dt.Columns.Add("iID_MaThuChi", typeof(int));
            dt.Columns.Add("iID_MaThuChi_Cha", typeof(int));
            dt.Columns.Add("bLaHangCha", typeof(int));


            dt.Columns.Add("sNoiDung", typeof(String));

            dt.Columns.Add("rSoTienViet", typeof(decimal));
            dt.Columns.Add("rSoTienNgoai", typeof(decimal));


            dt.Columns.Add("rTongSoTien", typeof(decimal));

            if (dtTemp.Rows.Count > 0 && dtTemp != null)
            {

                for (int i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "     ";
                }
                foreach (DataRow dr in dtTemp.Rows)
                {
                    String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(dr["iID_MaThuChi_Cha"]);
                    if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet_Cha) == false &&
                        iID_MaTaiKhoanDanhMucChiTiet_Cha != "" && iID_MaTaiKhoanDanhMucChiTiet_Cha != "0")
                    {
                    }
                    else
                    {
                        ThuTu++;
                        Decimal rSoTienViet = 0, rTongSoTien = 0;
                        DataRow drMain = dt.NewRow();
                        drMain["iSTT"] = ThuTu.ToString();
                        drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                        drMain["iID_MaThuChi_Cha"] = Convert.ToInt32((dr["iID_MaThuChi_Cha"]));
                        if (Convert.ToBoolean(dr["bLaHangCha"]) == true)
                        {
                            drMain["bLaHangCha"] = 1;
                        }
                        else
                        {
                            drMain["bLaHangCha"] = 0;
                        }

                        if (isKhoangTrang == true)
                        {
                            drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                        }
                        else
                        {
                            drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                        }
                        // drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                        //drMain["rSoTienNgoai"] = 0;
                        int kt = KiemTra_LaHangCha(Convert.ToInt32(dr["iID_MaThuChi"]));
                        Boolean bTuDongTinh = Convert.ToBoolean(dr["bTuDongTinh"]);
                        String sTenTaiKhoan_TienViet = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();
                        String sTenTaiKhoan_Tong = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                        Boolean bCoTKGT_TienViet = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                        Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                        Boolean bTuTinhTong = Convert.ToBoolean(dr["bTuTinhTong"]);
                        //String DenNgay = "2013/12/31";
                        if (kt > 0)
                        {
                            if (bTuDongTinh == false)
                            {
                                // rSoTienNgoai = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                // rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                if (bCoTKGT_TienViet)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                                arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                                arrMaNgoaiTe[i].Trim().Length -
                                                arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            rSoTienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rSoTienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }

                                    }
                                }
                                else
                                {
                                    rSoTienViet = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                }


                            }
                            else
                            {
                              
                                    TinhTongTienChi_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienViet,
                                               bTuDongTinh,
                                               sTenTaiKhoan_TienViet, bCoTKGT_TienViet, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                               DenNgay, iID_MaTrangThaiDuyet);
                              
                            }
                            ///tinh tong
                            if (bTuTinhTong == false)
                            {
                                if (bCoTKGT_Tong)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                                arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                                arrMaNgoaiTe[i].Trim().Length -
                                                arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                }

                            }
                            else
                            {
                                TinhTongTienChi_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()),
                                           ref rTongSoTien, bTuTinhTong,
                                           sTenTaiKhoan_TienViet, bCoTKGT_TienViet, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                           DenNgay, iID_MaTrangThaiDuyet);
                            }
                            if (DVT == "0")
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet;
                                drMain["rTongSoTien"] = rTongSoTien;
                                drMain["rSoTienViet"] = rTongSoTien - rSoTienViet;
                            }
                            else if (DVT == "1")
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet / 1000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000;

                            }
                            else
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet / 1000000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000000;
                            }

                        }
                        else
                        {
                            if (bTuDongTinh == false)
                            {
                                if (bCoTKGT_TienViet)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                                arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                                arrMaNgoaiTe[i].Trim().Length -
                                                arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            rSoTienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rSoTienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rSoTienViet = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                }



                            }
                            else
                            {
                                getTien_ChiCon_ByCha_NgoaiTe(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 2,
                                                     ref rSoTienViet, iID_MaTrangThaiDuyet, iQuy);

                            }
                            if (bTuTinhTong == false)
                            {
                                if (bCoTKGT_Tong)
                                {
                                    String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                    for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                    {
                                        if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                        {
                                            String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                            String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                                arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                                arrMaNgoaiTe[i].Trim().Length -
                                                arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                            rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                        }
                                        else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                        {
                                            rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                                false, iID_MaTrangThaiDuyet,
                                                                                arrMaNgoaiTe[i]);
                                        }
                                    }
                                }
                                else
                                {
                                    rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                                }
                            }
                            else
                            {
                                getTien_ChiCon_ByCha_TongTien(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 2,
                                                     ref rTongSoTien, iID_MaTrangThaiDuyet, iQuy);

                            }

                            if (DVT == "0")
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet;
                                drMain["rTongSoTien"] = rTongSoTien;
                                drMain["rSoTienViet"] = rTongSoTien - rSoTienViet;
                            }
                            else if (DVT == "1")
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet / 1000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000;

                            }
                            else
                            {
                                drMain["rSoTienNgoai"] = rSoTienViet / 1000000;
                                drMain["rTongSoTien"] = rTongSoTien / 1000000;
                                drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000000;
                            }

                        }

                        dt.Rows.Add(drMain);
                        addchildren_PhanChi(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap,
                                            ref ThuTu, isKhoangTrang, DenNgay, iThangCT, iID_MaTrangThaiDuyet, DVT, iQuy);
                    }
                }
                dtTemp.Dispose();

            }
            else
            {
                dt.Rows.Add(dt.NewRow());
            }
            return dt;
        }

        private static void addchildren_PhanChi(DataTable dtTemp, int menuid, ref  DataTable dt, int Cap, ref int ThuTu, Boolean isKhoangTrang, String DenNgay, int iThangCT, String iID_MaTrangThaiDuyet, String DVT, int iQuy)
        {
            String strDoanTrang = "";
            for (int i = 1; i <= Cap; i++)
            {
                strDoanTrang += "     ";
            }
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ThuTu++;
                    Decimal rSoTienViet = 0, rTongSoTien = 0;
                    DataRow drMain = dt.NewRow();
                    drMain["iSTT"] = ThuTu.ToString();
                    drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                    drMain["iID_MaThuChi_Cha"] = Convert.ToInt32((dr["iID_MaThuChi_Cha"]));
                    if (Convert.ToBoolean(dr["bLaHangCha"]) == true)
                    {
                        drMain["bLaHangCha"] = 2;
                    }
                    else
                    {
                        if (Cap > 0)
                        {
                            drMain["bLaHangCha"] = 3;
                        }
                        else
                        {
                            drMain["bLaHangCha"] = 0;
                        }
                    }

                    if (isKhoangTrang == true)
                    {
                        drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                    }
                    else
                    {
                        drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                    }
                    // drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                   // drMain["rSoTienNgoai"] = 0;
                    int kt = KiemTra_LaHangCha(Convert.ToInt32(dr["iID_MaThuChi"]));
                    Boolean bTuDongTinh = Convert.ToBoolean(dr["bTuDongTinh"]);
                    String sTenTaiKhoan_TienViet = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim();
                    String sTenTaiKhoan_Tong = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim();
                    Boolean bCoTKGT_TienViet = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    Boolean bCoTKGT_Tong = Convert.ToBoolean(dr["bCoTKGT_Tong"]);
                    Boolean bTuTinhTong = Convert.ToBoolean(dr["bTuTinhTong"]);
                    //String DenNgay = "2013/12/31";
                    if (kt > 0)
                    {
                        if (bTuDongTinh == false)
                        {
                            // rSoTienNgoai = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_NgoaiTe, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            // rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            if (bCoTKGT_TienViet)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rSoTienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rSoTienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rSoTienViet = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            }


                        }
                        else
                        {

                            TinhTongTienChi_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienViet,
                                       bTuDongTinh,
                                       sTenTaiKhoan_TienViet, bCoTKGT_TienViet, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                       DenNgay, iID_MaTrangThaiDuyet);

                        }
                        ///tinh tong
                        if (bTuTinhTong == false)
                        {
                            if (bCoTKGT_Tong)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            }

                        }
                        else
                        {
                            TinhTongTienChi_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()),
                                       ref rTongSoTien, bTuTinhTong,
                                       sTenTaiKhoan_TienViet, bCoTKGT_TienViet, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                       DenNgay, iID_MaTrangThaiDuyet);
                        }
                        if (DVT == "0")
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet;
                            drMain["rTongSoTien"] = rTongSoTien;
                            drMain["rSoTienViet"] = rTongSoTien - rSoTienViet;
                        }
                        else if (DVT == "1")
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet / 1000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000;

                        }
                        else
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet / 1000000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000000;
                        }

                    }
                    else
                    {
                        if (bTuDongTinh == false)
                        {
                            if (bCoTKGT_TienViet)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rSoTienViet += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rSoTienViet += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rSoTienViet = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_TienViet, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            }



                        }
                        else
                        {
                            getTien_ChiCon_ByCha_NgoaiTe(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 2,
                                                 ref rSoTienViet, iID_MaTrangThaiDuyet, iQuy);

                        }
                        if (bTuTinhTong == false)
                        {
                            if (bCoTKGT_Tong)
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }
                                }
                            }
                            else
                            {
                                rTongSoTien = getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, bCoTKGT_Tong, iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            }
                        }
                        else
                        {
                            getTien_ChiCon_ByCha_TongTien(Convert.ToString(dr["iID_MaThuChi"]), iThangCT, DenNgay, 2,
                                                 ref rTongSoTien, iID_MaTrangThaiDuyet, iQuy);

                        }

                        if (DVT == "0")
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet;
                            drMain["rTongSoTien"] = rTongSoTien;
                            drMain["rSoTienViet"] = rTongSoTien - rSoTienViet;
                        }
                        else if (DVT == "1")
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet / 1000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000;

                        }
                        else
                        {
                            drMain["rSoTienNgoai"] = rSoTienViet / 1000000;
                            drMain["rTongSoTien"] = rTongSoTien / 1000000;
                            drMain["rSoTienViet"] = (rTongSoTien - rSoTienViet) / 1000000;
                        }

                    }

                    dt.Rows.Add(drMain);
                    addchildren_PhanChi(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap + 1,
                                        ref ThuTu, isKhoangTrang, DenNgay, iThangCT, iID_MaTrangThaiDuyet, DVT, iQuy);
                }
                else
                {
                }
            }
        }

        public static void TinhTongTienChi(DataTable dtTemp, int menuid, ref  Decimal rSoTienNgoaiTe, ref Decimal rTongSoTien, Boolean bTuDongTinh,
            string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ////Tinh Tien NgoaiTe
                    if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim()) == false)
                    {
                        if (Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]))
                        {
                            String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                            for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                            {
                                if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                {
                                    String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                    String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                        arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                        arrMaNgoaiTe[i].Trim().Length -
                                        arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                    rSoTienNgoaiTe += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                                }

                            }
                        }
                        else
                        {
                            rSoTienNgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                            // rSoTienNgoaiTe += HamChung.ConvertToDecimal(dr["rSoTienViet"]);
                        }
                    }
                    ////Tinh Tien Tong
                    if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim()) == false)
                    {
                        if (Convert.ToBoolean(dr["bCoTKGT_Tong"]))
                        {
                            String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                            for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                            {
                                if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                {
                                    String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                    String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                        arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                        arrMaNgoaiTe[i].Trim().Length -
                                        arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                    rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay,
                                                                        Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                                }

                            }
                        }
                        else
                        {
                            rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                        }
                    }
                    //Neu tu dong tinh
                    if (bTuDongTinh)
                    {
                        TinhTongTienChi(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoaiTe,
                                        ref rTongSoTien, bTuDongTinh,
                                        sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                        DenNgay, iID_MaTrangThaiDuyet);
                    }

                }
            }
        }
        public static void TinhTongTienChi_TongTien(DataTable dtTemp, int menuid, ref Decimal rTongSoTien, Boolean bTuDongTinh,
           string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {

                    ////Tinh Tien Tong

                    if (Convert.ToBoolean(dr["bTuTinhTong"]) == true)
                    {
                        TinhTongTienChi_TongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rTongSoTien,
                                        Convert.ToBoolean(dr["bTuDongTinh"]),
                                        sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                        DenNgay, iID_MaTrangThaiDuyet);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_Tong"])) == false)
                        {
                            if (Convert.ToBoolean(dr["bCoTKGT_Tong"]))
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_Tong"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rTongSoTien += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay,
                                                                            Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }

                                }
                            }
                            else
                            {
                                rTongSoTien += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                    Convert.ToBoolean(dr["bCoTKGT_Tong"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_Tong"]));
                            }
                        }
                    }


                }
            }
        }
        public static void TinhTongTienChi_NgoaiTe(DataTable dtTemp, int menuid, ref  Decimal rSoTienNgoaiTe,  Boolean bTuDongTinh,
           string sTenTaiKhoan_NgoaiTe, Boolean bCoTKGT_NgoaiTe, string sTenTaiKhoan_Tong, Boolean bCoTKGT_Tong, int iThangCT, String DenNgay, String iID_MaTrangThaiDuyet)
        {
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ////Tinh Tien NgoaiTe

                    if (Convert.ToBoolean(dr["bTuDongTinh"]) == true)
                    {
                        TinhTongTienChi_NgoaiTe(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienNgoaiTe,
                                         Convert.ToBoolean(dr["bTuDongTinh"]),
                                        sTenTaiKhoan_NgoaiTe, bCoTKGT_NgoaiTe, sTenTaiKhoan_Tong, bCoTKGT_Tong, iThangCT,
                                        DenNgay, iID_MaTrangThaiDuyet);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim()) == false)
                        {
                            if (Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]))
                            {
                                String[] arrMaNgoaiTe = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]).Trim().Split(',');
                                for (int i = 0; i < arrMaNgoaiTe.Count(); i++)
                                {
                                    if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') > 0)
                                    {
                                        String iID_MaTaiKhoan = arrMaNgoaiTe[i].Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Trim();
                                        String iID_MaChiTiet = arrMaNgoaiTe[i].Substring(
                                            arrMaNgoaiTe[i].Trim().IndexOf('-') + 1,
                                            arrMaNgoaiTe[i].Trim().Length -
                                            arrMaNgoaiTe[i].Trim().Substring(0, arrMaNgoaiTe[i].Trim().IndexOf('-')).Length - 1).Trim();
                                        rSoTienNgoaiTe += getTien_ByTK_ChiTiet(iID_MaTaiKhoan, iID_MaChiTiet, iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, "");
                                    }
                                    else if (String.IsNullOrEmpty(arrMaNgoaiTe[i]) == false && arrMaNgoaiTe[i].IndexOf('-') < 0)
                                    {
                                        rSoTienNgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay,
                                                                            false, iID_MaTrangThaiDuyet,
                                                                            arrMaNgoaiTe[i]);
                                    }

                                }
                            }
                            else
                            {
                                rSoTienNgoaiTe += getTien_ByTK_ChiTiet("", "", iThangCT, DenNgay, Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]), iID_MaTrangThaiDuyet, Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]));
                                // rSoTienNgoaiTe += HamChung.ConvertToDecimal(dr["rSoTienViet"]);
                            }
                        }
                    }

                    //Neu tu dong tinh


                }
            }
        }
#endregion
        public static int KiemTra_LaHangCha(int iID_MaThuChi)
          {
              SqlCommand cmd =
                  new SqlCommand(
                      "SELECT COUNT(*) FROM KT_CanDoiThuChiTaiChinh WHERE iTrangThai=1  AND iID_MaThuChi_Cha=@iID_MaThuChi");
              cmd.Parameters.AddWithValue("@iID_MaThuChi", iID_MaThuChi);
              int vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
              cmd.Dispose();
              return vR;
          }
          public static void TinhTongTien(DataTable dtTemp, int menuid, ref Decimal rSoTienThu, ref  Decimal rSoTienThuNgoaiTe,ref Decimal rSoTienChi, ref  Decimal rSoTienNgoaiTeChi)
          {
              foreach (DataRow dr in dtTemp.Rows)
              {
                  if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                  {
                      rSoTienThu += HamChung.ConvertToDecimal(dr["rSoTienThu"]);
                      rSoTienThuNgoaiTe += HamChung.ConvertToDecimal(dr["rSoTienNgoaiTeThu"]);

                      rSoTienChi += HamChung.ConvertToDecimal(dr["rSoTienChi"]);
                      rSoTienNgoaiTeChi += HamChung.ConvertToDecimal(dr["rSoTienNgoaiTeChi"]);
                      TinhTongTien(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref rSoTienThu, ref rSoTienThuNgoaiTe, ref rSoTienChi, ref rSoTienNgoaiTeChi);
                  }
              }
          }

        /// <summary>
        /// hàm hiển thị dữ liệu ra báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        private void LoadData(FlexCelReport fr, String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String iNamLamViec, String iQuy)
        {
            DataTable data = rptKeToanTongHop_CanDoiThuChiTaiChinh_PhanThu(iID_MaTrangThaiDuyet, iNgay, iThang, iNamLamViec, DonViTinh, iQuy);
            data.TableName = "ChiTietThu";
            //fr.AddTable("ChiTietThu", data);
          
            //data.Dispose();

            DataTable data1 = rptKeToanTongHop_CanDoiThuChiTaiChinh_PhanChi(iID_MaTrangThaiDuyet, iNgay, iThang, iNamLamViec, DonViTinh, iQuy);
            data1.TableName = "ChiTietChi";

            if (data.Rows.Count < data1.Rows.Count)
            {
                int Count = data1.Rows.Count - data.Rows.Count;
                for (int i = 0; i < Count; i++)
                {
                    DataRow dr = data.NewRow();
                    data.Rows.Add(dr);
                    dr = null;
                }
            }
            else
            {
                int Count = data.Rows.Count - data1.Rows.Count;
                for (int i = 0; i < Count; i++)
                {
                    DataRow dr = data1.NewRow();
                    data1.Rows.Add(dr);
                    dr = null;
                }
            }
            fr.AddTable("ChiTietThu", data);

            data.Dispose();
            fr.AddTable("ChiTietChi", data1);

            data1.Dispose();
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file PDF
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String iQuy)
        {
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, iQuy);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "AA");
                    pdf.EndExport();
                    ms.Position = 0;
                    clsResult.FileName = "Test.pdf";
                    clsResult.type = "pdf";
                    clsResult.ms = ms;
                    return clsResult;
                }

            }
        }
        /// <summary>
        /// hàm xuất dữ liệu ra file Excel
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public clsExcelResult ExportToExcel(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String iQuy)
        {
            clsExcelResult clsResult = new clsExcelResult();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh,iQuy);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "CanDoiThuChi.xls";
                clsResult.type = "xls";
                return clsResult;
            }

        }
        /// <summary>
        /// Hàm View PDF
        /// </summary>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iID_MaTrangThaiDuyet"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaND, String iID_MaTrangThaiDuyet, String iNgay, String iThang, String DonViTinh, String iQuy)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), MaND, iID_MaTrangThaiDuyet, iNgay, iThang, DonViTinh, iQuy);
            using (FlexCelPdfExport pdf = new FlexCelPdfExport())
            {
                pdf.Workbook = xls;
                using (MemoryStream ms = new MemoryStream())
                {
                    pdf.BeginExport(ms);
                    pdf.ExportAllVisibleSheets(false, "BaoCao");
                    pdf.EndExport();
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf");
                }
            }
            return null;
        }

        /// <summary>
        /// Dt Trang Thai Duyet
        /// </summary>
        /// <returns></returns>
        public static DataTable tbTrangThai()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("iID_MaTrangThaiDuyet", (typeof(string)));
            dt.Columns.Add("TenTrangThai", (typeof(string)));

            DataRow dr = dt.NewRow();

            dr["iID_MaTrangThaiDuyet"] = "0";
            dr["TenTrangThai"] = "Đã Duyệt";
            dt.Rows.InsertAt(dr, 0);

            DataRow dr1 = dt.NewRow();
            dr1["iID_MaTrangThaiDuyet"] = "1";
            dr1["TenTrangThai"] = "Tất Cả";
            dt.Rows.InsertAt(dr1, 1);

            return dt;
        }
        public DataTable tendonvi(String ID)
        {
            DataTable dt;
            if (String.IsNullOrEmpty(ID)) return null;
            SqlCommand cmd = new SqlCommand("SELECT  sTen FROM NS_DonVi WHERE iID_MaDonVi=@ID");
            cmd.Parameters.AddWithValue("@ID", ID);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        public static DataTable Danhsach_DonVi()
        {
            DataTable dt = new DataTable();
            String SQL = string.Format(@"SELECT SP.iID_MaDonVi,DV.sTen
                                            FROM DM_SanPham AS SP
                                            INNER JOIN NS_DonVi AS DV ON SP.iID_MaDonVi=DV.iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            dt = Connection.GetDataTable(cmd);
            return dt;
        }
    }
}

