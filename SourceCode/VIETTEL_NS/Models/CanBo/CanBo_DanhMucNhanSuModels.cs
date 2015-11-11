using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;

namespace VIETTEL.Models
{
    public class CanBo_DanhMucNhanSuModels
    {
        /// <summary>
        /// Lấy ra danh sách tỉnh
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getTinh(Boolean All = false, String TieuDe = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaTinh, sTenTinh FROM CB_DM_Tinh WHERE iTrangThai=1 ORDER BY iSTT, sTenTinh ASC");
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = vR.NewRow();
                R["iID_MaTinh"] = "";
                R["sTenTinh"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
      /// <summary>
      /// Lấy tình trạng cán bộ
      /// </summary>
      /// <param name="All"></param>
      /// <param name="TieuDe"></param>
      /// <returns></returns>
        public static DataTable getTinhTrangCB(Boolean All = false, String TieuDe = "")
        {
            var dtGioiTinh = new DataTable();
            dtGioiTinh.Columns.Add("iID_Ma", typeof(int));
            dtGioiTinh.Columns.Add("sTen", typeof(string));
            DataRow R2 = dtGioiTinh.NewRow();
            R2["iID_Ma"] = 0;
            R2["sTen"] = "Bình thường";
            dtGioiTinh.Rows.Add(R2);

            DataRow R3 = dtGioiTinh.NewRow();
            R3["iID_Ma"] = 1;
            R3["sTen"] = "Tăng";
            dtGioiTinh.Rows.Add(R3);

            DataRow R4 = dtGioiTinh.NewRow();
            R4["iID_Ma"] = -1;
            R4["sTen"] = "Giảm";
            dtGioiTinh.Rows.Add(R4);
            if (All)
            {
                DataRow R = dtGioiTinh.NewRow();
                R["iID_Ma"] = -1;
                R["sTen"] = TieuDe;
                dtGioiTinh.Rows.InsertAt(R, 0);
            }
            return dtGioiTinh;
        }
        /// <summary>
        /// Thiết lập giá trị giới tính
        /// </summary>
        /// <returns></returns>
        public static DataTable getGioiTinh(Boolean All = false, String TieuDe = "")
        {
            var dtGioiTinh = new DataTable();
            dtGioiTinh.Columns.Add("iID_Ma", typeof(int));
            dtGioiTinh.Columns.Add("sTen", typeof(string));          

         

            DataRow R4 = dtGioiTinh.NewRow();
            R4["iID_Ma"] = 1;
            R4["sTen"] = "Nam";
            dtGioiTinh.Rows.Add(R4);

            DataRow R3 = dtGioiTinh.NewRow();
            R3["iID_Ma"] = 0;
            R3["sTen"] = "Nữ";
            dtGioiTinh.Rows.Add(R3);
            if (All)
            {
                DataRow R = dtGioiTinh.NewRow();
                R["iID_Ma"] = -1;
                R["sTen"] = TieuDe;
                dtGioiTinh.Rows.InsertAt(R, 0);
            }
            return dtGioiTinh;
        }
        /// <summary>
        /// LÊy dữ liệu huyện
        /// </summary>
        /// <param name="MaTinh"></param>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getHuyen(String MaTinh = "", Boolean All = false, String TieuDe = "")
        {
            DataTable vR = new DataTable();
            if (String.IsNullOrEmpty(MaTinh) == false && MaTinh != "")
            {
                SqlCommand cmd = new SqlCommand();
                String SQL = String.Format("SELECT iID_MaHuyen, sTenHuyen FROM CB_DM_Huyen WHERE iTrangThai=1");
                SQL += " AND iID_MaTinh=@iID_MaTinh ORDER BY iSTT, sTenHuyen ASC";             
                cmd.Parameters.AddWithValue("@iID_MaTinh", MaTinh);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd); cmd.Dispose();
            }           
            if (All)
            {
                if (vR.Columns.Count == 0)
                {
                    vR.Columns.Add("iID_MaHuyen", typeof(string));
                    vR.Columns.Add("sTenHuyen", typeof(string));
                }
                DataRow R = vR.NewRow();
                R["iID_MaHuyen"] = -1;
                R["sTenHuyen"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Lấy dữ liệu huyện
        /// </summary>
        /// <param name="MaHuyen"></param>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getXaPhuong(String MaHuyen = "", Boolean All = false, String TieuDe = "")
        {
            DataTable vR = new DataTable();
            if (String.IsNullOrEmpty(MaHuyen) == false && MaHuyen != "")
            {
                SqlCommand cmd = new SqlCommand();
                String SQL = String.Format("SELECT iID_MaXaPhuong, sTenXaPhuong FROM CB_DM_XaPhuong WHERE iTrangThai=1");

                SQL += " AND iID_MaHuyen=@iID_MaHuyen ORDER BY iSTT, sTenXaPhuong ASC";

                cmd.Parameters.AddWithValue("@iID_MaHuyen", MaHuyen);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd); cmd.Dispose();
            }
            if (All)
            {
                if (vR.Columns.Count == 0)
                {
                    vR.Columns.Add("iID_MaXaPhuong", typeof(string));
                    vR.Columns.Add("sTenXaPhuong", typeof(string));
                }
                DataRow R = vR.NewRow();
                R["iID_MaXaPhuong"] = -1;
                R["sTenXaPhuong"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }

        /// <summary>
        /// Lấy ngạch lương
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getNgachLuong(Boolean All = false, String TieuDe = "")
        {
            DataTable vR = new DataTable();
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaNgachLuong, sTenNgachLuong FROM L_DanhMucNgachLuong WHERE iTrangThai=1 ORDER BY iSTT, sTenNgachLuong ASC");
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd); cmd.Dispose();
            if (All)
            {
                DataRow R = vR.NewRow();
                R["iID_MaNgachLuong"] = "";
                R["sTenNgachLuong"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Lấy dữ liệu bậc lương theo ngạch
        /// </summary>
        /// <param name="MaNghachLuong"></param>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getBacLuong(String MaNghachLuong = "", Boolean All = false, String TieuDe = "")
        {
            DataTable vR = new DataTable();
            if (String.IsNullOrEmpty(MaNghachLuong) == false && MaNghachLuong != "")
            {
                SqlCommand cmd = new SqlCommand();
                String SQL = String.Format("SELECT iID_MaBacLuong, sTenBacLuong FROM L_DanhMucBacLuong WHERE iTrangThai=1");

                SQL += " AND iID_MaNgachLuong=@MaNghachLuong ORDER BY iSTT, sTenBacLuong ASC";
                cmd.Parameters.AddWithValue("@MaNghachLuong", MaNghachLuong);
                cmd.CommandText = SQL;
                vR = Connection.GetDataTable(cmd); cmd.Dispose();
            }
            if (All)
            {
                if (vR.Columns.Count == 0)
                {
                    vR.Columns.Add("iID_MaBacLuong", typeof(string));
                    vR.Columns.Add("sTenBacLuong", typeof(string));
                }
                DataRow R = vR.NewRow();
                R["iID_MaBacLuong"] = "";
                R["sTenBacLuong"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Lấy nơi đào tạo
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getNoiDaoTao(Boolean All = false, String TieuDe = "")
        {
            var dtGioiTinh = new DataTable();
            dtGioiTinh.Columns.Add("iID_Ma", typeof(int));
            dtGioiTinh.Columns.Add("sTen", typeof(string));

            DataRow R3 = dtGioiTinh.NewRow();
            R3["iID_Ma"] = 1;
            R3["sTen"] = "Trong nước";
            dtGioiTinh.Rows.Add(R3);

            DataRow R4 = dtGioiTinh.NewRow();
            R4["iID_Ma"] = 0;
            R4["sTen"] = "Nước ngoài";
            dtGioiTinh.Rows.Add(R4);
            if (All)
            {
                DataRow R = dtGioiTinh.NewRow();
                R["iID_Ma"] = -1;
                R["sTen"] = TieuDe;
                dtGioiTinh.Rows.InsertAt(R, 0);
            }
            return dtGioiTinh;
        }
        /// <summary>
        /// Lấy thông tin nữ quân nhân
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getNuQuanNhan(Boolean All = false, String TieuDe = "", int Type = 1)
        {
            var dtGioiTinh = new DataTable();
            dtGioiTinh.Columns.Add("iID_Ma", typeof(int));
            dtGioiTinh.Columns.Add("sTen", typeof(string));
            if (Type == 1)
            {
                DataRow R3 = dtGioiTinh.NewRow();
                R3["iID_Ma"] = 1;
                R3["sTen"] = "Nữ chiến sĩ";
                dtGioiTinh.Rows.Add(R3);

                DataRow R4 = dtGioiTinh.NewRow();
                R4["iID_Ma"] = 2;
                R4["sTen"] = "Nữ quân nhân tình nguyện";
                dtGioiTinh.Rows.Add(R4);
            }
            if (All)
            {
                DataRow R = dtGioiTinh.NewRow();
                R["iID_Ma"] = 0;
                R["sTen"] = TieuDe;
                dtGioiTinh.Rows.InsertAt(R, 0);
            }
            return dtGioiTinh;
        }
        /// <summary>
        /// Lấy danh sách quá trình công tác theo cán bộ
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_QuaTrinhCT(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_QuaTrinhCongTac WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT DESC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Lấy chi tiết quá trình công tác
        /// </summary>
        /// <param name="iID_MaQuaTrinhCongTac"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet_QuaTrinhCT(string iID_MaQuaTrinhCongTac)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_QuaTrinhCongTac WHERE iTrangThai=1 AND iID_MaQuaTrinhCongTac=@iID_MaQuaTrinhCongTac");
            cmd.Parameters.AddWithValue("@iID_MaQuaTrinhCongTac", iID_MaQuaTrinhCongTac);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Lấy danh sách người phụ thuộc
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_NguoiPhuThuoc(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaNguoiPhuThuoc, iID_MaCanBo, iID_MaQuanHe, sHoTen, sLyDoGiamTru, dTuNgay, dDenNgay FROM CB_NguoiPhuThuoc WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT, sHoTen ASC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết người phụ thuộc
        /// </summary>
        /// <param name="iID_MaNguoiPhuThuoc"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet_NguoiPhuThuoc(string iID_MaNguoiPhuThuoc)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_NguoiPhuThuoc WHERE iTrangThai=1 AND iID_MaNguoiPhuThuoc=@iID_MaNguoiPhuThuoc");
            cmd.Parameters.AddWithValue("@iID_MaNguoiPhuThuoc", iID_MaNguoiPhuThuoc);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Danh sách quá trình đi nước ngoài
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_DiNuocNgoai(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_DiNuocNgoai WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT, dNgayTao ASC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết 
        /// </summary>
        /// <param name="iID_MaDiNuocNgoai"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet_DiNuocNgoai(string iID_MaDiNuocNgoai)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_DiNuocNgoai WHERE iTrangThai=1 AND iID_MaDiNuocNgoai=@iID_MaDiNuocNgoai");
            cmd.Parameters.AddWithValue("@iID_MaDiNuocNgoai", iID_MaDiNuocNgoai);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }

        /// <summary>
        /// Danh sách quá trình khen thưởng
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_KhenThuong(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_KhenThuong WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT, dNgayTao ASC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết 
        /// </summary>
        /// <param name="iID_MaKhenThuong"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet_KhenThuong(string iID_MaKhenThuong)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_KhenThuong WHERE iTrangThai=1 AND iID_MaKhenThuong=@iID_MaKhenThuong");
            cmd.Parameters.AddWithValue("@iID_MaKhenThuong", iID_MaKhenThuong);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Loại khen thưởng
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns>Khen thưởng hay danh hiệu thi đua</returns>
        public static DataTable getLoaiKhenThuong(Boolean All = false, String TieuDe = "")
        {
            var dtGioiTinh = new DataTable();
            dtGioiTinh.Columns.Add("iID_Ma", typeof(int));
            dtGioiTinh.Columns.Add("sTen", typeof(string));

            DataRow R3 = dtGioiTinh.NewRow();
            R3["iID_Ma"] = 0;
            R3["sTen"] = "Danh hiệu thi đua";
            dtGioiTinh.Rows.Add(R3);

            DataRow R4 = dtGioiTinh.NewRow();
            R4["iID_Ma"] = 1;
            R4["sTen"] = "Khen thưởng";
            dtGioiTinh.Rows.Add(R4);
            if (All)
            {
                DataRow R = dtGioiTinh.NewRow();
                R["iID_Ma"] = -1;
                R["sTen"] = TieuDe;
                dtGioiTinh.Rows.InsertAt(R, 0);
            }
            return dtGioiTinh;
        }
        /// <summary>
        /// Danh sách quá trình kỷ luật
        /// </summary>
        /// <param name="iID_MaCanBo"></param>
        /// <returns></returns>
        public static DataTable Get_DanhSach_KyLuat(String iID_MaCanBo = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT * FROM CB_KyLuat WHERE iTrangThai=1 AND iID_MaCanBo=@iID_MaCanBo ORDER BY iSTT, dNgayTao ASC");
            cmd.Parameters.AddWithValue("@iID_MaCanBo", iID_MaCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Chi tiết 
        /// </summary>
        /// <param name="iID_MaKyLuat"></param>
        /// <returns></returns>
        public static DataTable GetChiTiet_KyLuat(string iID_MaKyLuat)
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand("SELECT * FROM CB_KyLuat WHERE iTrangThai=1 AND iID_MaKyLuat=@iID_MaKyLuat");
            cmd.Parameters.AddWithValue("@iID_MaKyLuat", iID_MaKyLuat);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return vR;
        }
        /// <summary>
        /// Trình độ học vấn
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        ///  <param name="sDK"> dieu kien loc theo id</param>
        /// <returns></returns>
        public static DataTable getHocVan(Boolean All = false, String TieuDe = "", String sDK = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sID_MaTrinhDo, sTen FROM CB_TrinhDo WHERE iTrangThai=1";
            if (sDK != "")
            {
                SQL += " AND sID_MaTrinhDo=@iID_MaTrinhDo";
            }
            SQL += " ORDER BY iSTT, sTen ASC";
            cmd.CommandText = SQL;
            if (sDK != "")
            {
                cmd.Parameters.AddWithValue("@iID_MaTrinhDo", sDK);
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = vR.NewRow();
                R["sID_MaTrinhDo"] = "";
                R["sTen"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }

        /// <summary>
        /// Chức vụ
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <returns></returns>
        public static DataTable getChucVu(Boolean All = false, String TieuDe = "", string sDK = "")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT sID_MaChucVu, sTen FROM CB_ChucVu WHERE iTrangThai=1";
            if (sDK != "")
            {
                SQL += " AND sID_MaChucVu=@iID_MaChucVu";
            }
            SQL += " ORDER BY iSTT, sTen ASC";
            cmd.CommandText = SQL;
            if (sDK != "")
            {
                cmd.Parameters.AddWithValue("@iID_MaChucVu", sDK);
            }
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = vR.NewRow();
                R["sID_MaChucVu"] = "";
                R["sTen"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }

        /// <summary>
        /// Lý do tăng giảm cán bộ
        /// </summary>
        /// <param name="All"></param>
        /// <param name="TieuDe"></param>
        /// <param name="iID_MaTinhTrangCanBo"></param>
        /// <returns></returns>
        public static DataTable getLyDoTangGiam(Boolean All = false, String TieuDe = "", String iID_MaTinhTrangCanBo="")
        {
            DataTable vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT iID_MaLyDoTangGiam, sTen FROM CB_LyDoTangGiam WHERE iTrangThai=1 AND iID_MaTinhTrangCanBo=@iID_MaTinhTrangCanBo ORDER BY iSTT, sTen ASC");
            cmd.Parameters.AddWithValue("@iID_MaTinhTrangCanBo", iID_MaTinhTrangCanBo);
            cmd.CommandText = SQL;
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (All)
            {
                DataRow R = vR.NewRow();
                R["iID_MaLyDoTangGiam"] = -1;
                R["sTen"] = TieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }
        /// <summary>
        /// Lấy hệ số lương
        /// </summary>
        /// <param name="MaNgach"></param>
        /// <param name="MaBac"></param>
        /// <returns></returns>
        public static string getHeSoLuong(string MaNgach, string MaBacLuong)
        {
            string vR;
            SqlCommand cmd = new SqlCommand();
            String SQL = String.Format("SELECT rHeSoLuong FROM L_DanhMucBacLuong WHERE iID_MaBacLuong=@iID_MaBacLuong AND iID_MaNgachLuong=@iID_MaNgachLuong");
            cmd.Parameters.AddWithValue("@iID_MaBacLuong", MaBacLuong);
            cmd.Parameters.AddWithValue("@iID_MaNgachLuong", MaNgach);
            cmd.CommandText = SQL;
            vR = Convert.ToString(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            return vR;
        }


        /// <summary>
        /// lay dy lieu theo dieu kien
        /// </summary>
        /// <param name="MaDanhMuc"> ID Danh muc</param>
        /// <param name="TenBang"> Ten danh muc</param>
        /// <returns></returns>
        public static DataTable GetRow_DanhMuc(String TenBang,String TenTruong,String MaDanhMuc)
        {
            DataTable vR;
            String SQL = String.Format("SELECT * FROM @TenBang WHERE @TenTruong = @iID_MaDanhMuc");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@TenBang", TenBang);
            cmd.Parameters.AddWithValue("@TenTruong", TenTruong);
            cmd.Parameters.AddWithValue("@iID_MaDanhMuc", MaDanhMuc);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
        /// <summary>
        /// lay ten don vi theo dieu kien ma don vi
        /// </summary>
        /// <param name="MaDonVi"> ID don vi</param>
        /// <returns>datatable</returns>
        public static DataTable GetDonVi(String MaDonVi)
        {
            DataTable vR;
            String SQL = String.Format("SELECT * FROM NS_DonVi WHERE iID_MaDonVi = @iID_MaDonVi");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaDonVi", MaDonVi);
            vR = Connection.GetDataTable(cmd);
            return vR;
        }
    }
}