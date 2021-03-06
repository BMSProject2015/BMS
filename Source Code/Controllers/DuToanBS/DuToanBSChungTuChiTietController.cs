﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DomainModel.Abstract;
using DomainModel;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using VIETTEL.Models;
using System.IO;
using System.Data.OleDb;
using VIETTEL.Models.DuToanBS;

namespace VIETTEL.Controllers.DuToanBS
{
    public class DuToanBSChungTuChiTietController : Controller
    {
        //
        // GET: /DuToanBS_ChungTuChiTiet/
        private string sViewPath = "~/Views/DuToanBS/ChungTuChiTiet/";
        private const string DETAIL = "Detail";
        private const string EDIT = "Edit";
        private const string CREATE = "Create";
        private const string BANG_CHUNGTU = "";
        private const string BANG_CHUNGTU_CHITIET = "DTBS_ChungTuChiTiet";
        private const string PERMITION_MESSAGE = "PermitionMessage";
        private const string VIEW_CHUNGTU_CHITIET_INDEX = "ChungTuChiTiet_Index.aspx";
        private const string VIEW_CHUNGTU_CHITIET_FRAME = "ChungTuChiTiet_Index_DanhSach_Frame.aspx";

        #region Index
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="iID_MaChungTu"></param>
        /// <param name="sLNS"></param>
        /// <param name="iID_MaDonVi"></param>
        /// <param name="iLoai"></param>
        /// <param name="iChiTapTrung"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index(string iID_MaChungTu, string sLNS, string iID_MaDonVi, string iLoai, string iChiTapTrung)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, BANG_CHUNGTU_CHITIET, DETAIL) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE);
            }
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["sLNS"] = sLNS;
            ViewData["iLoai"] = iLoai;
            ViewData["iID_MaDonVi"] = iID_MaDonVi;
            ViewData["iChiTapTrung"] = iChiTapTrung;

            return View(sViewPath + VIEW_CHUNGTU_CHITIET_INDEX);
        } 
        #endregion

        #region Lưới chứng từ chi tiết
        /// <summary>
        /// Lưới chứng từ chi tiết
        /// </summary>
        /// <param name="sLNS"></param>
        /// <param name="MaLoai"></param>
        /// <param name="iLoai"></param>
        /// <param name="iChiTapTrung"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChungTuChiTietFrame(string MaLoai, string iLoai, string iChiTapTrung)
        {
            //Kiểm tra quyền của người dùng với chức năng
            if (BaoMat.ChoPhepLamViec(User.Identity.Name, BANG_CHUNGTU_CHITIET, DETAIL) == false)
            {
                return RedirectToAction("Index", PERMITION_MESSAGE);
            }
            string iID_MaChungTu = Request.Form["DuToan_iID_MaChungTu"];

            MaLoai = Request.Form["DuToan_MaLoai"];
            ViewData["iID_MaChungTu"] = iID_MaChungTu;
            ViewData["MaLoai"] = MaLoai;
            ViewData["iLoai"] = iLoai;
            ViewData["iChiTapTrung"] = iChiTapTrung;
            return View(sViewPath + VIEW_CHUNGTU_CHITIET_FRAME);
        } 
        #endregion

        #region Cập nhật chứng từ chi tiết
        /// <summary>
        /// Cập nhật chứng từ chi tiết
        /// </summary>
        /// <param name="ChiNganSach">Chi ngân sách</param>
        /// <param name="iID_MaChungTu">Mã chứng từ</param>
        /// <param name="sLNS">Loại ngân sách</param>
        /// <param name="iLoai">Loại</param>
        /// <param name="iChiTapTrung">Chi tập trung</param>
        /// <returns></returns>
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CapNhatChungTuChiTiet(string ChiNganSach, string iID_MaChungTu, string sLNS, string iLoai, string iChiTapTrung)
        {
            NameValueCollection data;
            if (iLoai == "4")
            {
                data = DuToanBSChungTuModels.LayThongTinChungTuKyThuatLan2(iID_MaChungTu);
            }
            else
            {
                data = DuToanBSChungTuModels.LayThongTinChungTu(iID_MaChungTu);
            }

            string MaND = User.Identity.Name;
            string ipAddress = Request.UserHostAddress;
            //Lấy thông tin của Bang
            string idXauMaCacHang = Request.Form["idXauMaCacHang"];
            string idXauMaCacCot = Request.Form["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Request.Form["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Request.Form["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Request.Form["idXauDuLieuThayDoi"];
            string idMaMucLucNganSach = Request.Form["idMaMucLucNganSach"];
            //Cập nhật vào database
            DuToanBSChungTuChiTietModels.CapNhatChungTuChiTiet(iID_MaChungTu, iLoai, iChiTapTrung, MaND, idXauDuLieuThayDoi, ipAddress, idMaMucLucNganSach, idXauMaCacHang, idXauCacHangDaXoa, idXauMaCacCot, idXauGiaTriChiTiet);

            //Kiểm tra trình duyệt/từ chối
            string idAction = Request.Form["idAction"];
            //Từ chối chứng từ.
            string sLyDo = Request.Form["sLyDo"];
            if (idAction == "1")
            {
                return RedirectToAction("TuChoiChungTu", "DuToanBSChungTu", new { maChungTu = iID_MaChungTu, sLNS = sLNS, iKyThuat = data["iKyThuat"], sLyDo = sLyDo, chiTiet = "1" });
            }
            //Trình duyệt chứng từ.
            else if (idAction == "2")
            {
                return RedirectToAction("TrinhDuyetChungTu", "DuToanBSChungTu", new { maChungTu = iID_MaChungTu, sLNS = sLNS, iKyThuat = data["iKyThuat"], sLyDo = sLyDo, chiTiet = "1" });
            }
            return RedirectToAction("ChungTuChiTietFrame", new { iID_MaChungTu = iID_MaChungTu, iLoai = iLoai, iChiTapTrung = iChiTapTrung });
        } 
        #endregion

        #region Upload excel
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpLoadExcel(string iID_MaChungTu, string iLoai)
        {
            string path = string.Empty;
            string sTenKhoa = "TMTL";
            path = TuLieuLichSuModels.ThuMucLuu(sTenKhoa);
            String sFileName = "";
            string newPath = AppDomain.CurrentDomain.BaseDirectory + path;
            //string newPath = path + dateString;
            if (Directory.Exists(newPath) == false)
            {
                Directory.CreateDirectory(newPath);
            }
            sFileName = Path.GetFileName(Request.Files["uploadFile"].FileName);
            sFileName = Path.Combine(newPath, sFileName);
            String ConnectionString = String.Format(ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'", sFileName);
            OleDbConnection connExcel = new OleDbConnection(ConnectionString);
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = null;
            conn = new OleDbConnection(ConnectionString);
            //try
            //{
            Request.Files["uploadFile"].SaveAs(Path.Combine(newPath, sFileName));
            OleDbCommand cmdExcel = new OleDbCommand();
            cmdExcel.Connection = connExcel;
            connExcel.Open();


            conn.Open();

            DataTable dtSheet = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            String Sheetname = Convert.ToString(dtSheet.Rows[0]["TABLE_NAME"]);

            cmd.CommandText = String.Format(@"SELECT * FROM [{0}]", Sheetname);
            cmd.Connection = conn;
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            DataTable dt = new DataTable();

            adapter.Fill(dt);
            dt.Columns[0].ColumnName = "STT";
            dt.Columns[1].ColumnName = "B";
            dt.Columns[2].ColumnName = "DV";
            dt.Columns[3].ColumnName = "TenDV";
            dt.Columns[4].ColumnName = "sLNS";
            dt.Columns[5].ColumnName = "sL";
            dt.Columns[6].ColumnName = "sK";
            dt.Columns[7].ColumnName = "sM";
            dt.Columns[8].ColumnName = "sTM";
            dt.Columns[9].ColumnName = "sTTM";
            dt.Columns[10].ColumnName = "sNG";
            dt.Columns[11].ColumnName = "TC";
            dt.Columns[12].ColumnName = "HV";
            SqlCommand cmd1;
            if (iLoai == "PC")
            {

                String SQL = String.Format(@"SELECT *
                            FROM DTBS_ChungTuChiTiet
                            WHERE iTrangThai=1 AND iID_MaChungTuChiTiet=@iID_MaChungTu
                            AND( rTuChi<>0 OR rPhanCap<>0 OR rHienVat<>0 OR rHangNhap<>0 OR rHangMua<>0 OR rDuPhong<>0)");
                cmd1 = new SqlCommand(SQL);
                cmd1.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                DataTable dtChungTu = Connection.GetDataTable(cmd1);

                String MaND = User.Identity.Name;
                String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                if (dtCauHinh.Rows.Count > 0)
                {
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                    dtCauHinh.Dispose();
                }
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                    sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                    dtPhongBan.Dispose();
                }
                for (int i = 0; i < dtChungTu.Rows.Count; i++)
                {
                    DataRow R = dtChungTu.Rows[i];
                    String sXau = String.Format(@"sL='{0}' AND sK='{1}' AND sM='{2}' AND sTM='{3}' AND sTTM='{4}' AND sNG='{5}'", R["sL"], R["sK"], R["sM"], R["sTM"], R["sTTM"], R["sNG"]);
                    DataRow[] dr = dt.Select(sXau);
                    //check neu da co du lieu se xoa cai cu sau do them cai moi
                    if (dr.Length > 0)
                    {
                        //Check bang DTBS_ChungTuchitiet_codulieu
                        SQL = String.Format(@"SELECT COUNT(*) FROM DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                        cmd1 = new SqlCommand(SQL);
                        cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);


                        int count = Convert.ToInt32(Connection.GetValue(cmd1, 0));
                        //neu co du lieu se xoa
                        if (count > 0)
                        {
                            SQL = String.Format(@"DELETE DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                            Connection.UpdateDatabase(cmd1);
                        }
                    }
                    String sXauNoiMa = "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"];

                    SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE  sXauNoiMa=@sXauNoiMa AND iTrangThai=1");
                    cmd1 = new SqlCommand(SQL);
                    cmd1.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                    DataTable dtMucLuc = Connection.GetDataTable(cmd1);
                    for (int j = 0; j < dr.Length; j++)
                    {
                        SQL = String.Format(@"INSERT INTO DTBS_ChungTuChiTiet_PhanCap(iID_MaChungTu,iID_MaPhongBan,sTenPhongBan,iID_MaPhongBanDich,iID_MaTrangThaiDuyet,
                        iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDonVi,sTenDonVi,iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,
                        sMoTa,rTuChi,rHienVat,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua) VALUES (@iID_MaChungTu,@iID_MaPhongBan,@sTenPhongBan,@iID_MaPhongBanDich,@iID_MaTrangThaiDuyet,@iNamLamViec,@iID_MaNguonNganSach,@iID_MaNamNganSach,
                                @iID_MaDonVi,@sTenDonVi,@iID_MaMucLucNganSach,@iID_MaMucLucNganSach_Cha,@sXauNoiMa,@sLNS,@sL,@sK,@sM,@sTM,@sTTM,@sNG, @sMoTa,@rTuChi,@rHienVat,@iID_MaNhomNguoiDung_DuocGiao,@sID_MaNguoiDung_DuocGiao,@sID_MaNguoiDungTao,@sIPSua,@sID_MaNguoiDungSua)");
                        cmd1 = new SqlCommand(SQL);
                        cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                        cmd1.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        cmd1.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                        cmd1.Parameters.AddWithValue("@iID_MaPhongBanDich", Convert.ToString(dr[j]["b"]));
                        cmd1.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                        cmd1.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                        cmd1.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                        cmd1.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                        cmd1.Parameters.AddWithValue("@iID_MaDonVi", Convert.ToString(dr[j]["DV"]));
                        String sTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dr[j]["DV"]));
                        cmd1.Parameters.AddWithValue("@sTenDonVi", Convert.ToString(dr[j]["DV"]) + "-" + sTenDonVi);



                        cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLuc.Rows[0]["iID_MaMucLucNganSach"]);
                        cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMucLuc.Rows[0]["iID_MaMucLucNganSach_Cha"]);
                        cmd1.Parameters.AddWithValue("@sXauNoiMa", "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"]);
                        cmd1.Parameters.AddWithValue("@sLNS", "1020100");
                        cmd1.Parameters.AddWithValue("@sL", R["sL"]);
                        cmd1.Parameters.AddWithValue("@sK", R["sK"]);
                        cmd1.Parameters.AddWithValue("@sM", R["sM"]);
                        cmd1.Parameters.AddWithValue("@sTM", R["sTM"]);
                        cmd1.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                        cmd1.Parameters.AddWithValue("@sNG", R["sNG"]);
                        cmd1.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                        if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["TC"])))
                        {
                            cmd1.Parameters.AddWithValue("@rTuChi", Convert.ToDecimal(dr[j]["TC"]) * 1000);
                        }
                        else
                            cmd1.Parameters.AddWithValue("@rTuChi", 0);
                        if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["HV"])))
                        {
                            cmd1.Parameters.AddWithValue("@rHienVat", Convert.ToDecimal(dr[j]["HV"]) * 1000);
                        }
                        else
                            cmd1.Parameters.AddWithValue("@rHienVat", 0);

                        cmd1.Parameters.AddWithValue("@iID_MaNhomNguoiDung_DuocGiao", R["iID_MaNhomNguoiDung_DuocGiao"]);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", MaND);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                        cmd1.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
                        Connection.UpdateDatabase(cmd1);
                    }
                    dtMucLuc.Dispose();
                }
            }
            else
            {
                String SQL = String.Format(@"SELECT *
                    FROM DTBS_ChungTuChiTiet
                    WHERE iTrangThai=1 AND iID_MaChungTu=@iID_MaChungTu
                    AND( rTuChi<>0 OR rPhanCap<>0 OR rHienVat<>0 OR rHangNhap<>0 OR rHangMua<>0 OR rDuPhong<>0)");
                cmd1 = new SqlCommand(SQL);
                cmd1.Parameters.AddWithValue("@iID_MaChungTu", iID_MaChungTu);
                DataTable dtChungTu = Connection.GetDataTable(cmd1);

                String MaND = User.Identity.Name;
                String iNamLamViec = NguoiDungCauHinhModels.iNamLamViec.ToString();
                DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
                String iID_MaNguonNganSach = "", iID_MaNamNganSach = "", iID_MaPhongBan = "", sTenPhongBan = "";
                if (dtCauHinh.Rows.Count > 0)
                {
                    iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
                    iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
                    dtCauHinh.Dispose();
                }
                DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
                if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
                {
                    DataRow drPhongBan = dtPhongBan.Rows[0];
                    iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
                    sTenPhongBan = Convert.ToString(drPhongBan["sTen"]);
                    dtPhongBan.Dispose();
                }
                for (int i = 0; i < dtChungTu.Rows.Count; i++)
                {
                    DataRow R = dtChungTu.Rows[i];
                    String sXau = String.Format(@"sL='{0}' AND sK='{1}' AND sM='{2}' AND sTM='{3}' AND sTTM='{4}' AND sNG='{5}'", R["sL"], R["sK"], R["sM"], R["sTM"], R["sTTM"], R["sNG"]);
                    DataRow[] dr = dt.Select(sXau);
                    //check neu da co du lieu se xoa cai cu sau do them cai moi
                    if (dr.Length > 0)
                    {
                        //Check bang DTBS_ChungTuchitiet_codulieu
                        SQL = String.Format(@"SELECT COUNT(*) FROM DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                        cmd1 = new SqlCommand(SQL);
                        cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);


                        int count = Convert.ToInt32(Connection.GetValue(cmd1, 0));
                        //neu co du lieu se xoa
                        if (count > 0)
                        {
                            SQL = String.Format(@"DELETE DTBS_ChungTuChiTiet_PhanCap WHERE iID_MaChungTu=@iID_MaChungTu");
                            cmd1 = new SqlCommand(SQL);
                            cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                            Connection.UpdateDatabase(cmd1);
                        }
                    }
                    String sXauNoiMa = "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"];

                    SQL = String.Format("SELECT * FROM NS_MucLucNganSach WHERE  sXauNoiMa=@sXauNoiMa AND iTrangThai=1");
                    cmd1 = new SqlCommand(SQL);
                    cmd1.Parameters.AddWithValue("@sXauNoiMa", sXauNoiMa);
                    DataTable dtMucLuc = Connection.GetDataTable(cmd1);
                    for (int j = 0; j < dr.Length; j++)
                    {
                        SQL = String.Format(@"INSERT INTO DTBS_ChungTuChiTiet_PhanCap(iID_MaChungTu,iID_MaPhongBan,sTenPhongBan,iID_MaPhongBanDich,iID_MaTrangThaiDuyet,
                        iNamLamViec,iID_MaNguonNganSach,iID_MaNamNganSach,iID_MaDonVi,sTenDonVi,iID_MaMucLucNganSach,iID_MaMucLucNganSach_Cha,sXauNoiMa,sLNS,sL,sK,sM,sTM,sTTM,sNG,
                        sMoTa,rTuChi,rHienVat,iID_MaNhomNguoiDung_DuocGiao,sID_MaNguoiDung_DuocGiao,sID_MaNguoiDungTao,sIPSua,sID_MaNguoiDungSua) VALUES (@iID_MaChungTu,@iID_MaPhongBan,@sTenPhongBan,@iID_MaPhongBanDich,@iID_MaTrangThaiDuyet,@iNamLamViec,@iID_MaNguonNganSach,@iID_MaNamNganSach,
                                @iID_MaDonVi,@sTenDonVi,@iID_MaMucLucNganSach,@iID_MaMucLucNganSach_Cha,@sXauNoiMa,@sLNS,@sL,@sK,@sM,@sTM,@sTTM,@sNG, @sMoTa,@rTuChi,@rHienVat,@iID_MaNhomNguoiDung_DuocGiao,@sID_MaNguoiDung_DuocGiao,@sID_MaNguoiDungTao,@sIPSua,@sID_MaNguoiDungSua)");
                        cmd1 = new SqlCommand(SQL);
                        cmd1.Parameters.AddWithValue("@iID_MaChungTu", R["iID_MaChungTuChiTiet"]);
                        cmd1.Parameters.AddWithValue("@iID_MaPhongBan", iID_MaPhongBan);
                        cmd1.Parameters.AddWithValue("@sTenPhongBan", sTenPhongBan);
                        cmd1.Parameters.AddWithValue("@iID_MaPhongBanDich", Convert.ToString(dr[j]["b"]));
                        cmd1.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan));
                        cmd1.Parameters.AddWithValue("@iNamLamViec", iNamLamViec);
                        cmd1.Parameters.AddWithValue("@iID_MaNguonNganSach", iID_MaNguonNganSach);
                        cmd1.Parameters.AddWithValue("@iID_MaNamNganSach", iID_MaNamNganSach);
                        cmd1.Parameters.AddWithValue("@iID_MaDonVi", Convert.ToString(dr[j]["DV"]));
                        String sTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(dr[j]["DV"]));
                        cmd1.Parameters.AddWithValue("@sTenDonVi", Convert.ToString(dr[j]["DV"]) + "-" + sTenDonVi);
                        cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach", dtMucLuc.Rows[0]["iID_MaMucLucNganSach"]);
                        cmd1.Parameters.AddWithValue("@iID_MaMucLucNganSach_Cha", dtMucLuc.Rows[0]["iID_MaMucLucNganSach_Cha"]);
                        cmd1.Parameters.AddWithValue("@sXauNoiMa", "1020100-" + R["sL"] + "-" + R["sK"] + "-" + R["sM"] + "-" + R["sTM"] + "-" + R["sTTM"] + "-" + R["sNG"]);
                        cmd1.Parameters.AddWithValue("@sLNS", "1020100");
                        cmd1.Parameters.AddWithValue("@sL", R["sL"]);
                        cmd1.Parameters.AddWithValue("@sK", R["sK"]);
                        cmd1.Parameters.AddWithValue("@sM", R["sM"]);
                        cmd1.Parameters.AddWithValue("@sTM", R["sTM"]);
                        cmd1.Parameters.AddWithValue("@sTTM", R["sTTM"]);
                        cmd1.Parameters.AddWithValue("@sNG", R["sNG"]);
                        cmd1.Parameters.AddWithValue("@sMoTa", R["sMoTa"]);
                        if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["TC"])))
                        {
                            cmd1.Parameters.AddWithValue("@rTuChi", Convert.ToDecimal(dr[j]["TC"]) * 1000);
                        }
                        else
                            cmd1.Parameters.AddWithValue("@rTuChi", 0);
                        if (!String.IsNullOrEmpty(Convert.ToString(dr[j]["HV"])))
                        {
                            cmd1.Parameters.AddWithValue("@rHienVat", Convert.ToDecimal(dr[j]["HV"]) * 1000);
                        }
                        else
                            cmd1.Parameters.AddWithValue("@rHienVat", 0);
                        cmd1.Parameters.AddWithValue("@iID_MaNhomNguoiDung_DuocGiao", R["iID_MaNhomNguoiDung_DuocGiao"]);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDung_DuocGiao", MaND);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDungTao", MaND);
                        cmd1.Parameters.AddWithValue("@sIPSua", Request.UserHostAddress);
                        cmd1.Parameters.AddWithValue("@sID_MaNguoiDungSua", MaND);
                        Connection.UpdateDatabase(cmd1);
                    }
                    dtMucLuc.Dispose();
                }

            }
            cmd.Dispose();
            cmd1.Dispose();

            //}
            //catch (Exception Exception) { throw Exception; }
            //finally
            //{
            conn.Close();
            connExcel.Close();
            conn.Dispose();
            connExcel.Dispose();

            string url = newPath + "/" + Path.GetFileName(Request.Files["uploadFile"].FileName);
            System.IO.File.Delete(url);

            //}
            if (iLoai == "PC")
                return RedirectToAction("ChungTuChiTietFrame", "DuToan_phanCapChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            return RedirectToAction("ChungTuChiTiet_Frame", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
        } 
        #endregion

        #region Lấy 1 hang AJAX: rTongSoNamTruoc
        [Authorize]
        public JsonResult get_GiaTri(String Truong, String GiaTri, String DSGiaTri)
        {
            if (Truong == "rTongSoNamTruoc")
            {
                return get_GiaTriTongSoNamTruoc(GiaTri, DSGiaTri);
            }
            return null;
        }

        private JsonResult get_GiaTriTongSoNamTruoc(String GiaTri, String DSGiaTri)
        {
            String iID_MaChungTu = GiaTri;
            String strDSTruong = "iID_MaDonVi,sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG";
            String[] arrDSTruong = strDSTruong.Split(',');
            String[] arrDSGiaTri = DSGiaTri.Split(',');

            SqlCommand cmd = new SqlCommand();
            String DK = "";

            NameValueCollection data = DuToan_ChungTuModels.LayThongTin(iID_MaChungTu);
            //DK = String.Format("iNamLamViec={0} AND iID_MaNguonNganSach={1} AND iID_MaNamNganSach={2}", Convert.ToInt32(data["iNamLamViec"]) - 1, data["iID_MaNguonNganSach"], data["iID_MaNamNganSach"]);
            DK = "iNamLamViec=@iNamLamViec AND iID_MaNguonNganSach=@iID_MaNguonNganSach AND iID_MaNamNganSach=@iID_MaNamNganSach";
            cmd.Parameters.AddWithValue("@iNamLamViec", Convert.ToInt32(data["iNamLamViec"]) - 1);
            cmd.Parameters.AddWithValue("@iID_MaNguonNganSach", data["iID_MaNguonNganSach"]);
            cmd.Parameters.AddWithValue("@iID_MaNamNganSach", data["iID_MaNamNganSach"]);
            int i = 0;

            for (i = 0; i < arrDSGiaTri.Length; i++)
            {
                DK += String.Format(" AND {0}=@{0}", arrDSTruong[i]);
                cmd.Parameters.AddWithValue("@" + arrDSTruong[i], arrDSGiaTri[i]);
                //DK += String.Format(" AND {0}='{1}'", arrDSTruong[i], arrDSGiaTri[i]);
            }

            String SQL = String.Format("SELECT SUM(rTongSo) FROM DT_ChungTuChiTiet WHERE bLaHangCha=0 AND {0}", DK);
            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();

            Object item = new
            {
                value = "0",
                label = "0"
            };

            if (dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
            {
                item = new
                {
                    value = dt.Rows[0][0],
                    label = dt.Rows[0][0]
                };
            }
            dt.Dispose();

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Lấy 1 hang AJAX: LayPhongBan
        [Authorize]
        public JsonResult GetPhongBanCuaDonVi(String iID_MaDonVi)
        {
            String MaDonVi = iID_MaDonVi.Split('-')[0];
            String iID_MaPhongBan = DonViModels.getPhongBanCuaDonVi(MaDonVi);
            Object item = new
            {
                value = iID_MaPhongBan,
            };
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
