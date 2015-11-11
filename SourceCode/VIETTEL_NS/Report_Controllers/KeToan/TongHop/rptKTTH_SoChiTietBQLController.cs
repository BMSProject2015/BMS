using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Collections.Specialized;

namespace VIETTEL.Report_Controllers.KeToan.TongHop
{
    public class rptKTTH_SoChiTietBQLController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        public string Count = "";
        public decimal Tien = 0;
        public  decimal PS_No = 0;
        public  decimal PS_Co = 0;
        private const String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.xls";
        public ActionResult Index()
        {
            if (HamChung.CoQuyenXemTheoMenu(Request.Url.AbsolutePath, User.Identity.Name))
            {
                HamRiengModels.UserDefault(User.Identity.Name);
                ViewData["PageLoad"] = "0";
                ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.aspx";
                ViewData["FilePath"] = Server.MapPath(sFilePath);
                return View(sViewPath + "ReportView.aspx");
            }
            else
            {
                return RedirectToAction("Index", "PermitionMessage");
            }
        }

        /// <summary>
/// Tìm kiếm
/// </summary>
/// <param name="ParentID"></param>
/// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String MaTaiKhoan = Request.Form[ParentID + "_MaTaiKhoan"];
            String TuThang = Request.Form[ParentID + "_TuThang"];
            String DenThang = Request.Form[ParentID + "_DenThang"];
            String Nam = Request.Form[ParentID + "_Nam"];
            String PhongBan = Request.Form[ParentID + "_iID_MaPhongBan"];
            String DonVi = Request.Form["iID_MaDonVi"];
            String iID_MaTrangThaiDuyet = Request.Form[ParentID + "_iID_MaTrangThaiDuyet"];
            String iDisplay = Request.Form[ParentID + "_iDisplay"];
            String FileName = DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
            ViewData["PageLoad"] = "1";
            ViewData["MaTaiKhoan"] = MaTaiKhoan;
            ViewData["TuThang"] = TuThang;
            ViewData["DenThang"] = DenThang;
            ViewData["Nam"] = Nam;
            ViewData["PhongBan"] = PhongBan;
            ViewData["DonVi"] = DonVi;
            ViewData["iDisplay"] = iDisplay;
            ViewData["iTrangThai"] = iID_MaTrangThaiDuyet;
            ViewData["FilePath"] = Server.MapPath(sFilePath);
            ViewData["path"] = "~/Report_Views/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.aspx";
            return View(sViewPath + "ReportView.aspx");
            // return RedirectToAction("Index", new { MaTaiKhoan = MaTaiKhoan, Thang = Thang, Nam = Nam, PhongBan = PhongBan, DonVi = DonVi });

        }
        /// <summary>
        /// Tao báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        public ExcelFile CreateReport(String path, String MaTaiKhoan = "", String TuThang = "",String DenThang="", String iNamLamViec = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0", String iDisplay="")
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);

            String TenDVCapTren = ReportModels.CauHinhTenDonViSuDung(1);
            String tendv = ReportModels.CauHinhTenDonViSuDung(2);
            String TenTK = TaiKhoanModels.getTenTK(MaTaiKhoan);
            String BQL = DanhMucModels.getTenPB(PhongBan);

            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptKTTH_SoChiTietBQL");
            LoadData(fr, MaTaiKhoan, TuThang,DenThang, iNamLamViec, PhongBan, DonVi, iTrangThai, iDisplay);
        

            fr.SetValue("TaiKhoan", TenTK);
            fr.SetValue("ThoiGian", "Từ tháng " + TuThang +"đến tháng "+DenThang + " năm " + iNamLamViec);
            fr.SetValue("TenDVCapTren", TenDVCapTren.ToUpper());
            fr.SetValue("TenDV", tendv.ToUpper());
            fr.SetValue("BQL", BQL.ToUpper());
            fr.SetValue("Ngay", DateTime.Now.Day);
            fr.SetValue("Thang", DateTime.Now.Month);
            fr.SetValue("Nam", DateTime.Now.Year);
           
     
            fr.Run(Result);
            fr.Dispose();
            return Result;
          
        }
     
        /// <summary>
        /// Lấy danh sách chứng từ
        /// </summary>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <returns></returns>
        private DataTable get_DanhSach_ChungTu(String MaTaiKhoan = "", String TuThang = "",String DenThang = "", String iNamLamViec = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0", String iDisplay = "")
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            String DK = "";
            if (iTrangThai == "0")
            {
                DK = " AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet";
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
            }
          
            SQL =
                String.Format(
                    @"select sSoChungTuGhiSo as sSoChungTu,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_No as iID_MaDonVi,sTenDonVi_No as sTenDonVi,sTenTomTat=(select top 1 sTenTomTat from NS_DonVi where iNamLamViec_DonVi=@iNamLamViec AND iID_MaDonVi=iID_MaDonVi_No),iID_MaPhongBan_No as MaPhongBan, sNoiDung,'  ' + iID_MaTaiKhoan_Co as MaTaiKhoan, sTenTaiKhoan_Co as TKDoiUng, SUm(rSoTienNo) as SoTienNo, SUm(rSoTienCo) as SoTienCo
from
(SELECT sSoChungTuGhiSo,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_No,SUBSTRING(sTenDonVi_No,5,1000) as sTenDonVi_No,iID_MaPhongBan_No, sNoiDung,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, sTenTaiKhoan_Co, SUm(rSoTien) as rSoTienNo, rSoTienCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL {0}
              --AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@iThangCT AND iThangCT>=@TuThang 
            and iThangCT>0 
              AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) and iID_MaDonVi_No IN ({1}) and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       
       GROUP BY sSoChungTuGhiSo,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_No,sTenDonVi_No,iID_MaTaiKhoan_No,iID_MaPhongBan_No, sNoiDung,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, sTenTaiKhoan_Co, rSoTien
       
            
       UNION 
       
       SELECT sSoChungTuGhiSo,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_Co,SUBSTRING(sTenDonVi_Co,5,1000) as sTenDonVi_Co,iID_MaPhongBan_Co, sNoiDung,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, sTenTaiKhoan_No, rSoTienNo=0,SUm(rSoTien) as rSoTienCo
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL {0}
            -- AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@iThangCT and iThangCT>0  AND iThangCT>=@TuThang
             
              AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) and iID_MaDonVi_Co IN ({1}) and iID_MaTaiKhoan_Co=@iID_MaTaiKhoan
       GROUP BY sSoChungTuGhiSo,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_Co,sTenDonVi_Co,iID_MaPhongBan_Co, sNoiDung,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, sTenTaiKhoan_No, rSoTien) a
       group by sSoChungTuGhiSo,iNgayCT,iThangCT, sSoChungTuChiTiet,iNgay,iThang, iID_MaDonVi_No,sTenDonVi_No,iID_MaPhongBan_No, sNoiDung,iID_MaTaiKhoan_Co, sTenTaiKhoan_Co,rSoTienNo,rSoTienCo Order by iThangCT asc,iNgayCT asc, convert(int,sSoChungTuGhiSo) asc",
                    DK, DonVi);
            
            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@iThangCT", DenThang);
            cmd.Parameters.AddWithValue("@TuThang", TuThang);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", PhongBan);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", DonVi);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            // return dt;
            if (iDisplay == "on")
            {
                DataTable dtMain = new DataTable();
                dtMain.Columns.Add("sSoChungTu", typeof(string));
                dtMain.Columns.Add("iNgayCT", typeof(string));
                dtMain.Columns.Add("iThangCT", typeof(string));
                dtMain.Columns.Add("sSoChungTuChiTiet", typeof(string));
                dtMain.Columns.Add("iNgay", typeof(string));
                dtMain.Columns.Add("iThang", typeof(string));
                dtMain.Columns.Add("MaDonVi", typeof(string));
                dtMain.Columns.Add("DonVi", typeof(string));
                dtMain.Columns.Add("sTenTomTat", typeof(string));
                dtMain.Columns.Add("MaPhongBan", typeof(string));
                dtMain.Columns.Add("sNoiDung", typeof(string));
                dtMain.Columns.Add("MaTaiKhoan", typeof(string));
                dtMain.Columns.Add("TKDoiUng", typeof(string));
                dtMain.Columns.Add("SoTienNo", typeof(double));
                dtMain.Columns.Add("SoTienCo", typeof(double));
                string arrsSoChungTu = "";      
                if (dt.Rows.Count > 0 && dt != null)
                {
                    for (int i = 0; i <dt.Rows.Count; i++)
                    {
                                      
                        DataRow dr = dt.Rows[i];
                        string sSoChungTu = Convert.ToString(dr["sSoChungTu"]);
                        if (arrsSoChungTu.IndexOf(sSoChungTu) == -1)
                        {

                            DataRow drMain = dtMain.NewRow();
                            drMain["sSoChungTu"] = sSoChungTu;
                            drMain["iNgayCT"] = Convert.ToString(dr["iNgayCT"]);
                            drMain["iThangCT"] = Convert.ToString(dr["iThangCT"]);
                            drMain["sSoChungTuChiTiet"] = Convert.ToString(dr["sSoChungTuChiTiet"]);
                            drMain["iNgay"] = Convert.ToString(dr["iNgay"]);
                            drMain["iThang"] = Convert.ToString(dr["iThang"]);
                            drMain["MaDonVi"] = Convert.ToString(dr["MaDonVi"]);
                            drMain["DonVi"] = Convert.ToString(dr["DonVi"]);
                            drMain["sTenTomTat"] = Convert.ToString(dr["sTenTomTat"]);
                            drMain["MaPhongBan"] = Convert.ToString(dr["MaPhongBan"]);
                            drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                            drMain["MaTaiKhoan"] = Convert.ToString(dr["MaTaiKhoan"]);
                            drMain["TKDoiUng"] = Convert.ToString(dr["TKDoiUng"]);
                            double SoTienNo = Convert.ToDouble(dr["SoTienNo"]);
                            double SoTienCo = Convert.ToDouble(dr["SoTienCo"]);
                            //string row = "";
                            for (int j = i + 1; j < dt.Rows.Count; j++)
                            {
                                if (sSoChungTu == Convert.ToString(dt.Rows[j]["sSoChungTu"]))
                                {
                                    SoTienNo += Convert.ToDouble(dt.Rows[j]["SoTienNo"]);
                                    SoTienCo += Convert.ToDouble(dt.Rows[j]["SoTienCo"]);                                                             
                                }
                            }                           
                            drMain["SoTienNo"] = SoTienNo;
                            drMain["SoTienCo"] = SoTienCo;
                            dtMain.Rows.Add(drMain);
                            arrsSoChungTu += "," + sSoChungTu;
                        }
                      
                    }
                }
                 return dtMain;

            }
            else
            {
                
                 return dt;
            }

        }
        private DataTable TinhLuyKe(String MaTaiKhoan = "", String Thang = "", String iNamLamViec = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0")
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (iTrangThai == "0")
            {
                SQL = String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,SUm(rSoTienNo) as DuDauKy_No, SUm(rSoTienCo) as DuDauKy_Co
from
(SELECT iID_MaDonVi_No,iID_MaPhongBan_No,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, SUm(rSoTien) as rSoTienNo, rSoTienCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
              AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<=@TuThang AND iThangCT<>0
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       
       GROUP BY iID_MaDonVi_No,iID_MaTaiKhoan_No,iID_MaPhongBan_No,iID_MaTaiKhoan_Co, rSoTien
       UNION        
       SELECT iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTienNo=0,SUm(rSoTien) as rSoTienCo
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@TuThang AND iThangCT<>0           
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan
       GROUP BY iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTien) a
       group by iID_MaDonVi_No,iID_MaPhongBan_No", DonVi);
                int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            else
            {
                SQL = String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,SUm(rSoTienNo) as DuDauKy_No, SUm(rSoTienCo) as DuDauKy_Co
from
(SELECT iID_MaDonVi_No,iID_MaPhongBan_No,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, SUm(rSoTien) as rSoTienNo, rSoTienCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL             
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<=@TuThang AND iThangCT<>0
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       
       GROUP BY iID_MaDonVi_No,iID_MaTaiKhoan_No,iID_MaPhongBan_No,iID_MaTaiKhoan_Co, rSoTien
       UNION        
       SELECT iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTienNo=0,SUm(rSoTien) as rSoTienCo
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@TuThang AND iThangCT<>0             
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan
       GROUP BY iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTien) a
       group by iID_MaDonVi_No,iID_MaPhongBan_No", DonVi);
            }

            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", Thang);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", PhongBan);
            //cmd.Parameters.AddWithValue("@iID_MaDonVi", DonVi);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;


        }
        private DataTable TinhSoDuDauKy(String MaTaiKhoan = "", String Thang = "", String iNamLamViec = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0")
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (iTrangThai == "0")
            {
                SQL =String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,SUm(rSoTienNo) as DuDauKy_No, SUm(rSoTienCo) as DuDauKy_Co
from
(SELECT iID_MaDonVi_No,iID_MaPhongBan_No,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, SUm(rSoTien) as rSoTienNo, rSoTienCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
              AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<@TuThang
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       
       GROUP BY iID_MaDonVi_No,iID_MaTaiKhoan_No,iID_MaPhongBan_No,iID_MaTaiKhoan_Co, rSoTien
       UNION        
       SELECT iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTienNo=0,SUm(rSoTien) as rSoTienCo
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@TuThang           
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan
       GROUP BY iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTien) a
       group by iID_MaDonVi_No,iID_MaPhongBan_No", DonVi);
                int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            else
            {
                SQL = String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,SUm(rSoTienNo) as DuDauKy_No, SUm(rSoTienCo) as DuDauKy_Co
from
(SELECT iID_MaDonVi_No,iID_MaPhongBan_No,iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co, SUm(rSoTien) as rSoTienNo, rSoTienCo=0
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL             
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<@TuThang
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       
       GROUP BY iID_MaDonVi_No,iID_MaTaiKhoan_No,iID_MaPhongBan_No,iID_MaTaiKhoan_Co, rSoTien
       UNION        
       SELECT iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTienNo=0,SUm(rSoTien) as rSoTienCo
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL           
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<@TuThang             
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan
       GROUP BY iID_MaDonVi_Co,iID_MaPhongBan_Co,iID_MaTaiKhoan_Co,iID_MaTaiKhoan_No, rSoTien) a
       group by iID_MaDonVi_No,iID_MaPhongBan_No", DonVi);
            }

            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", Thang);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", PhongBan);
            //cmd.Parameters.AddWithValue("@iID_MaDonVi", DonVi);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;


        }

        private DataTable dtDonVi(String MaTaiKhoan = "", String Thang = "", String iNamLamViec = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0")
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "";
            if (iTrangThai == "0")
            {
                SQL = String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,REPLACE(sTenDonVi_No,iID_MaDonVi_No+' - ','') as sTenDonVi
from
(SELECT DISTINCT iID_MaDonVi_No,sTenDonVi_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
              AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<=@TuThang
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       UNION        
       SELECT  DISTINCT iID_MaDonVi_Co,sTenDonVi_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@TuThang           
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan) a
       group by iID_MaDonVi_No,sTenDonVi_No", DonVi);
                int iID_MaTrangThaiDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop);
                cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", iID_MaTrangThaiDuyet);
            }
            else
            {
                SQL = String.Format(@"
select iID_MaDonVi_No as iID_MaDonVi,REPLACE(sTenDonVi_No,iID_MaDonVi_No+' - ','') as sTenDonVi
from
(SELECT DISTINCT iID_MaDonVi_No,sTenDonVi_No
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_No <>''
             AND iID_MaDonVi_No IS NOT NULL
             AND iNamLamViec=@iNamLamViec
               AND iThangCT<=@TuThang
            AND iID_MaPhongBan_No=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan)
             and iID_MaDonVi_No IN ({0})
              and iID_MaTaiKhoan_No=@iID_MaTaiKhoan
       UNION        
       SELECT  DISTINCT iID_MaDonVi_Co,sTenDonVi_Co
       FROM KT_ChungTuChiTiet
       WHERE iTrangThai=1  
           
             AND iID_MaDonVi_Co <>''
             AND iID_MaDonVi_Co IS NOT NULL
             AND iNamLamViec=@iNamLamViec
             AND iThangCT<=@TuThang           
             AND iID_MaPhongBan_Co=(select top 1 sKyHieu from NS_PhongBan where iID_MaPhongBan =@iID_MaPhongBan) 
              and iID_MaDonVi_Co IN ({0}) 
             and iID_MaTaiKhoan_Co= @iID_MaTaiKhoan) a
       group by iID_MaDonVi_No,sTenDonVi_No", DonVi);
            }

            cmd.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
            cmd.Parameters.AddWithValue("@TuThang", Thang);
            cmd.Parameters.AddWithValue("@iID_MaPhongBan", PhongBan);
            //cmd.Parameters.AddWithValue("@iID_MaDonVi", DonVi);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", MaTaiKhoan);
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            return dt;


        }
        /// <summary>
        /// Load ra danh sách chứng từ đổ dữ liệu vào bảng chi tiết
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        private void LoadData(FlexCelReport fr, String MaTaiKhoan = "", String TuThang = "", String DenThang = "", String Nam = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0", String iDisplay = "")
        {
            DataTable dtDonVi1 = dtDonVi(MaTaiKhoan, DenThang, Nam, PhongBan, DonVi, iTrangThai);
            fr.AddTable("DonVi", dtDonVi1);
            DataTable data = get_DanhSach_ChungTu(MaTaiKhoan, TuThang,DenThang, Nam, PhongBan, DonVi, iTrangThai, iDisplay);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            DataTable dtDuDauKy = TinhSoDuDauKy(MaTaiKhoan, TuThang, Nam, PhongBan, DonVi, iTrangThai);
            DataTable dtLuyKe = TinhLuyKe(MaTaiKhoan, DenThang, Nam, PhongBan, DonVi, iTrangThai);

            DataTable dtDonViCopyLK = dtDonVi1.Copy();
            for (int i = dtDonVi1.Rows.Count - 1; i >= 0; i--)
            {
                for (int j = dtLuyKe.Rows.Count - 1; j >= 0; j--)
                {
                    if (Convert.ToString(dtDonVi1.Rows[i]["iID_MaDonVi"]) == Convert.ToString(dtLuyKe.Rows[j]["iID_MaDonVi"]))
                    {
                        dtDonViCopyLK.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
            for (int i = 0; i < dtDonViCopyLK.Rows.Count; i++)
            {
                DataRow r = dtLuyKe.NewRow();
                r["iID_MaDonVi"] = dtDonViCopyLK.Rows[i]["iID_MaDonVi"];
                r["DuDauKy_No"] = 0;
                r["DuDauKy_Co"] = 0;
                dtLuyKe.Rows.InsertAt(r, 0);
            }
            fr.AddTable("LuyKe", dtLuyKe);
            DataTable dtDonViCopy = dtDonVi1.Copy();
            for (int i =  dtDonVi1.Rows.Count-1; i >=0; i--)
            {
                for (int j = dtDuDauKy.Rows.Count-1; j >=0 ; j--)
                {
                    if (Convert.ToString(dtDonVi1.Rows[i]["iID_MaDonVi"]) == Convert.ToString(dtDuDauKy.Rows[j]["iID_MaDonVi"]))
                    {
                        dtDonViCopy.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
            for (int i = 0; i < dtDonViCopy.Rows.Count; i++)
            {
                DataRow r = dtDuDauKy.NewRow();
                r["iID_MaDonVi"] = dtDonViCopy.Rows[i]["iID_MaDonVi"];
                r["DuDauKy_No"] = 0;
                r["DuDauKy_Co"] = 0;
                dtDuDauKy.Rows.InsertAt(r,0);
            }
            fr.AddTable("DuDauKy", dtDuDauKy);
           
            dtDonVi1.Dispose();
            dtDuDauKy.Dispose();
            dtLuyKe.Dispose();
            DataTable dtSoDu = TinhSoDuDauKy(MaTaiKhoan, TuThang, Nam, PhongBan, DonVi, iTrangThai);
            double DuDauKy_No = 0, DuDauKy_Co = 0, rTongNo = 0, rTongCo = 0;
            double SoDuNo = 0, SoDuCo = 0;
            if (dtSoDu.Rows.Count > 0 && dtSoDu!=null)
            {
                for (int i = 0; i < dtSoDu.Rows.Count; i++)
                {
                    DuDauKy_No += Convert.ToDouble(dtSoDu.Rows[i]["DuDauKy_No"]);
                    DuDauKy_Co += Convert.ToDouble(dtSoDu.Rows[i]["DuDauKy_Co"]);
                }
               
            }
            double CuoiKyNo = 0, CuoiKyCo = 0;
            if (data.Rows.Count > 0 && data != null)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    rTongNo += Convert.ToDouble(data.Rows[i]["SoTienNo"]);
                    rTongCo += Convert.ToDouble(data.Rows[i]["SoTienCo"]);
                }

            }
            if (DuDauKy_No - DuDauKy_Co >= 0)
            {
                SoDuNo = DuDauKy_No - DuDauKy_Co;
                CuoiKyNo = DuDauKy_No - DuDauKy_Co + rTongNo - rTongCo;
            }
            else
            {
                SoDuCo = DuDauKy_Co - DuDauKy_No;
                CuoiKyCo = DuDauKy_Co - DuDauKy_No + rTongCo - rTongNo;
            }
            fr.SetValue("DuDauKy_No", SoDuNo);
            fr.SetValue("DuDauKy_Co", SoDuCo);
            fr.SetValue("CuoiKyNo", CuoiKyNo);
            fr.SetValue("CuoiKyCo", CuoiKyCo);
            if (dtSoDu != null)
            {
                dtSoDu.Dispose();   
            }

            //dtSoDu.TableName = "GiaiThich";
            //fr.AddTable("GiaiThich", dtSoDu);
            //dtSoDu.Dispose();
           
            if (data != null)
            {
                data.Dispose();
            }
        }
        /// <summary>
        /// Kết xuất báo cáo ra PDF
        /// </summary>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="PhongBan"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        public clsExcelResult ExportToPDF(String MaTaiKhoan = "", String TuThang = "", String DenThang = "", String Nam = "", String PhongBan = "", String DonVi = "", String iDisplay = "")
        {
            HamChung.Language();
            String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.xls";
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, TuThang, DenThang,Nam, PhongBan, DonVi, iDisplay);
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
        /// Hiển thị báo cáo
        /// </summary>
        /// <param name="MaTaiKhoan"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <param name="PhongBan"></param>
        /// <param name="DonVi"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaTaiKhoan = "", String TuThang = "", String DenThang = "", String Nam = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0", String iDisplay = "")
        {
            HamChung.Language();
            String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.xls";

            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, TuThang,DenThang, Nam, PhongBan, DonVi, iTrangThai, iDisplay);
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
        public clsExcelResult ExportToExcel(String MaTaiKhoan = "", String Thang = "", String Nam = "", String PhongBan = "", String DonVi = "", String iTrangThai = "0", String iDisplay = "")
        {
            HamChung.Language();
            String sFilePath = "/Report_ExcelFrom/KeToan/TongHop/rptKeToanTongHop_SoChiTietBQL.xls";

            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(sFilePath), MaTaiKhoan, Thang, Nam, PhongBan, DonVi, iTrangThai, iDisplay);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "rptKeToanTongHop_SoChiTietBQL.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
      
        public JsonResult getDonVi(String Id)
        {
            var dt = PhongBan_DonViModels.getDonViByBQL(Id, true, "--- Chọn đơn vị ---");
            JsonResult value = Json(HamChung.getGiaTri("iID_MaDonVi", "sTen", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
        public JsonResult ObjDanhSachDonVi(String Id, String iID_MaDonVi)
        {
            return Json(get_sDanhSachDonVi(Id, iID_MaDonVi), JsonRequestBehavior.AllowGet);
        }

        public String get_sDanhSachDonVi(String Id,String iID_MaDonVi)
        {
            var dt = PhongBan_DonViModels.getDonViByBQL(Id, false, "--- Chọn đơn vị ---");
            String MaDonVi1 = "", MaDonVi2 = "", TenDonVi = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                MaDonVi1 = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    MaDonVi2 = Convert.ToString(dt.Rows[j]["iID_MaDonVi"]);
                    TenDonVi = Convert.ToString(dt.Rows[j]["sTen"]);
                    if (MaDonVi1.Equals(MaDonVi2))
                    {
                        dt.Rows[j]["sTen"] = TenDonVi ;
                    }
                }
            }
            int SoCot = 2;
            if (dt.Rows.Count >= 10)
                SoCot = 5;

            StringBuilder stb = new StringBuilder();
            String[] arrMaDonVi = iID_MaDonVi.Split(',');
            stb.Append("<table  class=\"mGrid\">");
            String strsTen = "", MaDonVi = "", strChecked = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                stb.Append("<tr>");
                strChecked = "";
                strsTen = Convert.ToString(dt.Rows[i]["sTen"]);
                MaDonVi = Convert.ToString(dt.Rows[i]["iID_MaDonVi"]);
                for (int j = 0; j < arrMaDonVi.Length; j++)
                {
                    if (MaDonVi.Equals(arrMaDonVi[j]))
                    {
                        strChecked = "checked=\"checked\"";
                        break;
                    }
                }
                stb.Append("<td align=\"center\" style=\"width:25px;\">");
                stb.Append("<input type=\"checkbox\" " + strChecked + " value=\"" + MaDonVi + "\"check-group=\"DonVi\" id=\"iID_MaDonVi\" name=\"iID_MaDonVi\" />");
                stb.Append("</td>");
                stb.Append("<td align=\"left\">"  + strsTen + "</td>");
                stb.Append("</tr>");
            }
            stb.Append("</table>");
            stb.Append("<script type=\"text/javascript\">");
            stb.Append("function CheckAll(value) {");
            stb.Append("$(\"input:checkbox[check-group='DonVi']\").each(function (i) {");
            stb.Append("this.checked = value;");
            stb.Append("});");
            stb.Append("}");
            stb.Append("</script>");
            return stb.ToString(); ;
        }
        public JsonResult getTaiKhoan(string MaPhongBan, string MaDonVi, string MaTrangThai, string iThang)
        {
            var dt = TaiKhoanModels.DSTaiKhoan_BQL_DVi(MaPhongBan, MaDonVi, MaTrangThai, iThang, false, "--- Chọn đơn vị ---", User.Identity.Name);
            JsonResult value = Json(HamChung.getGiaTri("iID_MaTaiKhoan", "sTenTaiKhoan", dt), JsonRequestBehavior.AllowGet);
            if (dt != null) dt.Dispose();
            return value;
        }
    }
}
