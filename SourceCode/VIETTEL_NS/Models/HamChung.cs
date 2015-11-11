using System;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Security;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Web.Routing;
using System.Web;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.Mvc;
namespace VIETTEL.Models
{
    public class HamChung
    {


        public static void MessageBox(Control page, string msg)
        {
            string myScript = String.Format("alert('{0}');", msg);
            ScriptManager.RegisterStartupScript(page, page.GetType(),
              "MyScript", myScript, true);
        }
        /// <summary>
        /// lay du lieu tu datatable
        /// </summary>
        /// <param name="sTenBang"> Tên bảng cần lấy</param>
        /// <param name="sTenTruongLayra">danh sách field lấy ra: a,b,c </param>
        /// <param name="sTruongDK">trường đk lọc dữ liệu: a</param>
        /// <param name="sDK">giá trị lọc: a</param>
        /// <param name="orderby">trường sắp xếp: a,b,c </param>
        /// <returns>trả về kiểu Datatable</returns>
        public static DataTable GetDataTable(string sTenBang, string sTenTruongLayra, string sTruongDK, string sDK, bool groupby,string orderby)
        {
            DataTable dt;
            string sSql = "Select " + (sTenTruongLayra == "" ? "*" : sTenTruongLayra) + " from " + sTenBang + " where iTrangThai =1 ";
            if(sTruongDK != "")
            {
                sSql += " and " + sTruongDK + "=@dk";
            }

            if (groupby)
            {
                sSql += " Group by  " + sTenTruongLayra ;
            }

            if (orderby != "")
            {
                sSql += " order by " + orderby;
            }

            SqlCommand cmd = new SqlCommand(sSql);
            if (sTruongDK != "")
            {
                cmd.Parameters.AddWithValue("@dk", sDK);
            }
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

        /// <summary>
        /// Tạo thư mục mới nếu chưa có 
        /// </summary>
        /// <param name="Dir"></param>
        /// <returns>True khi chưa có thư mục cần tạo; False khi đã có thư mục trên server</returns>
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

      
        public static Boolean isDate(String strDate)
        {
           if (String.IsNullOrEmpty(strDate)) return false;
           Object objDate= CommonFunction.LayNgayTuXau(strDate);
           if (objDate == null)
               return false;
           try
           {
               DateTime d = Convert.ToDateTime(objDate);
           }
           catch
           {
               return false;
           }
           return true;

        }


        /// <summary>     
        ///Nếu sửa kiểm tra sau khi sửa mã của bản ghi có trùng với mã truyền vào nếu trùng thì hợp lệ nếu khác là trùng. Trùng trả về giá trị True    
     /// </summary>
     /// <param name="TenBang">Tên bảng dữ liệu</param>
     /// <param name="TenTruongKhoa">Tên trường khóa</param>
     /// <param name="GiaTriKhoa">Giá trị khóa</param>
     /// <param name="DanhSachTenTruong">Danh sách tên trường check trùng ','</param>
        /// <param name="DanhSachGiaTri">Danh sách giá trị cần check</param>
     /// <param name="DuLieuMoi">DuLieuMoi=True là thêm mới,=False là sửa</param>
     /// <returns>Trùng trả về giá trị True</returns>
        public static Boolean Check_Trung(String TenBang, String TenTruongKhoa, Object GiaTriKhoa, String DanhSachTenTruong, String DanhSachGiaTri, Boolean DuLieuMoi)
        {
            String DK="";
            SqlCommand cmd = new SqlCommand();
            String[] arrTenTruong,arrGiaTri;
            arrTenTruong = DanhSachTenTruong.Split(',');
            arrGiaTri=DanhSachGiaTri.Split(',');

            if (String.IsNullOrEmpty(DanhSachTenTruong) == false)
            {
                DK += " AND ";
            }
            for (int i = 0; i < arrTenTruong.Length; i++)
            {
                DK += arrTenTruong[i] + "=@" + arrTenTruong[i];
                if (i < arrTenTruong.Length - 1)
                    DK += " AND ";
                if (arrTenTruong[i].StartsWith("d"))
                {
                    DateTime d= new DateTime();
                    if(isDate(arrGiaTri[i]))
                    {
                        d =Convert.ToDateTime(CommonFunction.LayNgayTuXau(arrGiaTri[i]));
                    }
                    cmd.Parameters.AddWithValue("@" + arrTenTruong[i],d.ToString("yyyy/MM/dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + arrTenTruong[i], arrGiaTri[i]);
                }
            }

            Boolean Ok = false;
            String SQL = String.Format("SELECT {0} FROM {1} WHERE iTrangThai=1 {2}", TenTruongKhoa, TenBang, DK);

            cmd.CommandText = SQL;
            DataTable dt = Connection.GetDataTable(cmd);
            if (DuLieuMoi)
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    Ok = true;
                }
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {

                    String Ma2 = CommonFunction.ValueToString(dt.Rows[0][TenTruongKhoa]);
                    String Ma1 = CommonFunction.ValueToString(GiaTriKhoa);
                    if (Ma1.Equals(Ma2))
                    {
                        Ok = false;
                    }
                    else Ok = true;

                }
            }
            dt.Dispose();
            cmd.Dispose();
            return Ok;
        }
        /// <summary>
        /// Hàm kiẻm tra trước khi xóa xem giá trị có trong bảng cần truy vấn không
        /// </summary>
        /// <param name="TenBang">bảng cấn truy vấn</param>
        /// <param name="TenTruong">tên trường</param>
        /// <param name="GiaTri">giá trị</param>
        /// <returns>true được xóa; false không được xóa</returns>
        public static Boolean Checked_Delete(String TenBang, String TenTruong, String GiaTri)
        {
            Boolean Ok = true;
            String DK = "";
            SqlCommand cmd = new SqlCommand();
            String[] arrTenTruong, arrGiaTri;
            arrTenTruong = TenTruong.Split(',');
            arrGiaTri = GiaTri.Split(',');
            if (String.IsNullOrEmpty(TenTruong) == false)
            {
                DK += " AND ";
            }
            for (int i = 0; i < arrTenTruong.Length; i++)
            {
                DK += arrTenTruong[i] + "=@" + arrTenTruong[i];
                if (i < arrTenTruong.Length - 1)
                    DK += " AND ";
                if (arrTenTruong[i].StartsWith("d"))
                {
                    DateTime d = new DateTime();
                    if (isDate(arrGiaTri[i]))
                    {
                        d = Convert.ToDateTime(CommonFunction.LayNgayTuXau(arrGiaTri[i]));
                    }
                    cmd.Parameters.AddWithValue("@" + arrTenTruong[i], d.ToString("yyyy/MM/dd"));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@" + arrTenTruong[i], arrGiaTri[i]);
                }
            }
            String SQL = String.Format("SELECT * FROM {0} WHERE iTrangThai=1  {1}", TenBang, DK);
            
            cmd.CommandText = SQL;            
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                Ok = false;
                dt.Dispose();
            }
            cmd.Dispose();
            return Ok;
        }

        public static DataTable Lay_dtDanhMuc(String TenLoaiDanhMuc)
        {
            String SQL = String.Format("SELECT iID_MaDanhMuc, DC_DanhMuc.sTen,DC_DanhMuc.sTenKhoa FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'{0}' ORDER BY iSTT", TenLoaiDanhMuc);
            return Connection.GetDataTable(SQL);
        }

        /// <summary>
        /// Lấy ký tự của chỉ số cột trong Excel; chỉ số bắt đầu từ 1 tương ứng với Cột A
        /// </summary>
        /// <param name="ChiSoCot">chỉ số của cột</param>
        /// <returns>Ký tự của cột tương ứng với chỉ số</returns>
       public static String ExportExcel_MaCot(int ChiSoCot)
        {
            String vR = "";
            int Du;
            Boolean ok = false;
            ChiSoCot = ChiSoCot - 1;
            do
            {
                Du = ChiSoCot % 26;
                ChiSoCot /= 26;
                if (ok)
                {
                    Du = Du - 1;
                }
                vR = Convert.ToChar(Du + 65) + vR;
                ok = true;
            } while (ChiSoCot > 0);
            return vR;
        }
        /// <summary>
        /// SELECT DISTINCT Tu DATATABLE
        /// </summary>
        /// <param name="TableName">Tên Truyền vào</param>
        /// <param name="SourceTable">TABLE Dữ liệu</param>
        /// <param name="FieldName">Trường cần distinct</param>
        /// <param name="sFieldAdd">Danh sách của Table mới</param>
       /// <param name="sField_DK">Tên trường điều kiện để lấy mô tả</param>
       /// <param name="sField_DK_Value">Tên trường để lấy giá trị khi lấy thông tin mô tả (trường hợp lấy mô tả của nguồn ngân sách)</param>
        /// <returns>Dat</returns>
        public static DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName, String sFieldAdd, String sField_DK="",String sField_DK_Value="",String strSort="")
        {
            DataTable dt = new DataTable(TableName);
            String[] arrFieldAdd = sFieldAdd.Split(',');
            String[] arrFieldName = FieldName.Split(',');
            for (int i = 0; i < arrFieldAdd.Length; i++)
            {
                dt.Columns.Add(arrFieldAdd[i], SourceTable.Columns[arrFieldAdd[i]].DataType);
            }
            if (SourceTable.Rows.Count > 0)
            {
                object[] LastValue = new object[arrFieldName.Length];
                for (int i = 0; i < LastValue.Length; i++)
                {
                    LastValue[i] = null;
                }


                foreach (DataRow dr in SourceTable.Select("", FieldName +" "+ strSort))
                {
                    Boolean ok = true;
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (LastValue[i] != null && (ColumnEqual(LastValue[i], dr[arrFieldName[i]])))
                        {
                            ok = false;
                        }
                        else
                        {
                            ok = true;
                            break;
                        }
                    }
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (ok)
                        {
                            LastValue[i] = dr[arrFieldName[i]];
                        }
                    }
                    if (ok)
                    {
                        DataRow R = dt.NewRow();
                        for (int j = 0; j < arrFieldAdd.Length; j++)
                        {
                            R[arrFieldAdd[j]] = dr[arrFieldAdd[j]];
                        }
                        if ((String.IsNullOrEmpty(sField_DK) == false && arrFieldAdd[arrFieldAdd.Length - 1] == "sMoTa") || String.IsNullOrEmpty(sField_DK_Value) == false)
                            R[arrFieldAdd[arrFieldAdd.Length - 1]] = LayMoTa(dr, sField_DK, sField_DK_Value);                        
                        dt.Rows.Add(R);
                    }
                }
            }
            return dt;
        }

        private static String LayMoTa(DataRow dr, String sField_DK,String sField_DK_Value="")
        {
            String MoTa = "";
            if (String.IsNullOrEmpty(sField_DK) == false)
            {
                String[] arrDK = sField_DK.Split(',');
                String DK = "";
                for (int i = 0; i < arrDK.Length; i++)
                {
                    DK += arrDK[i] + "=@" + arrDK[i];
                    if (i < arrDK.Length - 1)
                        DK += " AND ";

                }
                if (String.IsNullOrEmpty(DK) == false) DK = " WHERE " + DK;
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach " + DK;
                SqlCommand cmd = new SqlCommand(SQL);
                for (int i = 0; i < arrDK.Length; i++)
                {
                    if (i < arrDK.Length - 1)
                        cmd.Parameters.AddWithValue("@" + arrDK[i], dr[arrDK[i]]);
                    else
                        cmd.Parameters.AddWithValue("@" + arrDK[i], "");
                }
                if (String.IsNullOrEmpty(sField_DK_Value) == false)
                {
                    cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("@" + arrDK[0]));
                    cmd.Parameters.AddWithValue("@" + arrDK[0], dr[sField_DK_Value]);
                }
                MoTa = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            else
            {
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sLNS=@sLNS";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", dr[sField_DK_Value]);
                MoTa = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return MoTa;
        }
        public static DataTable SelectDistinct_QLDA(string TableName, DataTable SourceTable, string FieldName, String sFieldAdd, String sField_DK = "", String sField_DK_Value = "", String strSort = "",String sTienDo="")
        {
            DataTable dt = new DataTable(TableName);
            String[] arrFieldAdd = sFieldAdd.Split(',');
            String[] arrFieldName = FieldName.Split(',');
            for (int i = 0; i < arrFieldAdd.Length; i++)
            {
                dt.Columns.Add(arrFieldAdd[i], SourceTable.Columns[arrFieldAdd[i]].DataType);
            }
            if (SourceTable.Rows.Count > 0)
            {
                object[] LastValue = new object[arrFieldName.Length];
                for (int i = 0; i < LastValue.Length; i++)
                {
                    LastValue[i] = null;
                }


                foreach (DataRow dr in SourceTable.Select("", FieldName + " " + strSort))
                {
                    Boolean ok = true;
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (LastValue[i] != null && (ColumnEqual(LastValue[i], dr[arrFieldName[i]])))
                        {
                            ok = false;
                        }
                        else
                        {
                            ok = true;
                            break;
                        }
                    }
                    for (int i = 0; i < arrFieldName.Length; i++)
                    {
                        if (ok)
                        {
                            LastValue[i] = dr[arrFieldName[i]];
                        }
                    }
                    if (ok)
                    {
                        DataRow R = dt.NewRow();
                        for (int j = 0; j < arrFieldAdd.Length; j++)
                        {
                            R[arrFieldAdd[j]] = dr[arrFieldAdd[j]];
                        }
                        if (String.IsNullOrEmpty(sField_DK) == false && arrFieldAdd[arrFieldAdd.Length - 2] == "sTenDuAn")
                            R[arrFieldAdd[arrFieldAdd.Length - 2]] = LayMoTa_QLDA(dr, sField_DK, sField_DK_Value);
                        if (String.IsNullOrEmpty(sField_DK) == false && arrFieldAdd[arrFieldAdd.Length - 1] == "sTienDo")
                            R[arrFieldAdd[arrFieldAdd.Length - 1]] = LayTienDo_QLDA(dr, sField_DK, sField_DK_Value);
                        dt.Rows.Add(R);
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// Lấy mô tả quản lý dự án
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="sField_DK"></param>
        /// <param name="sField_DK_Value"></param>
        /// <returns></returns>
        private static String LayMoTa_QLDA(DataRow dr, String sField_DK, String sField_DK_Value = "")
        {
            String MoTa = "";
            if (String.IsNullOrEmpty(sField_DK) == false)
            {
                String[] arrDK = sField_DK.Split(',');
                String DK = "";
                for (int i = 0; i < arrDK.Length; i++)
                {
                    DK += arrDK[i] + "=@" + arrDK[i];
                    if (i < arrDK.Length - 1)
                        DK += " AND ";
                }
                if (String.IsNullOrEmpty(DK) == false) DK = " WHERE " + DK;
                String SQL = "SELECT sTenDuAn FROM QLDA_DanhMucDuAn " + DK;
                SqlCommand cmd = new SqlCommand(SQL);
                for (int i = 0; i < arrDK.Length; i++)
                {
                    if (i < arrDK.Length - 1)
                        cmd.Parameters.AddWithValue("@" + arrDK[i], dr[arrDK[i]]);
                    else
                        cmd.Parameters.AddWithValue("@" + arrDK[i], "");
                }
                if (String.IsNullOrEmpty(sField_DK_Value) == false)
                {
                    cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("@" + arrDK[0]));
                    cmd.Parameters.AddWithValue("@" + arrDK[0], dr[sField_DK_Value]);
                }
                MoTa = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return MoTa;
        }
        private static String LayTienDo_QLDA(DataRow dr, String sField_DK, String sField_DK_Value = "")
        {
            String TienDo = "";
            if (String.IsNullOrEmpty(sField_DK) == false)
            {
                String[] arrDK = sField_DK.Split(',');
                String DK = "";
                for (int i = 0; i < arrDK.Length; i++)
                {
                    DK += arrDK[i] + "=@" + arrDK[i];
                    if (i < arrDK.Length - 1)
                        DK += " AND ";
                }
                if (String.IsNullOrEmpty(DK) == false) DK = " WHERE " + DK;
                String SQL = "SELECT sTienDo FROM QLDA_DanhMucDuAn " + DK;
                SqlCommand cmd = new SqlCommand(SQL);
                for (int i = 0; i < arrDK.Length; i++)
                {
                    if (i < arrDK.Length - 1)
                        cmd.Parameters.AddWithValue("@" + arrDK[i], dr[arrDK[i]]);
                    else
                        cmd.Parameters.AddWithValue("@" + arrDK[i], "");
                }
                if (String.IsNullOrEmpty(sField_DK_Value) == false)
                {
                    cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("@" + arrDK[0]));
                    cmd.Parameters.AddWithValue("@" + arrDK[0], dr[sField_DK_Value]);
                }
                TienDo = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            return TienDo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        private static bool ColumnEqual(object A, object B)
        {

            // Compares two values to see if they are equal. Also compares DBNULL.Value.
            // Note: If your DataTable contains object fields, then you must extend this
            // function to handle them in a meaningful way if you intend to group on them.

            if ((A == DBNull.Value || A == null) && (B == DBNull.Value || B == null)) //  both are DBNull.Value
                return true;
            if ((A == DBNull.Value || A == null) || (B == DBNull.Value || B == null)) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
        public static String LayTenNguoiDungTuTaiKhoan(Object TaiKhoan)
        {
            String SQL = String.Format("SELECT sHoTen FROM QT_NguoiDung WHERE sID_MaNguoiDung='{0}'", TaiKhoan);
            return Connection.GetValueString(SQL, "");
        }
        public static String LayTenTaiKhoan(string username)
        {
            if (String.IsNullOrEmpty(username)) username = "";
            String HoTen = "";
            SqlCommand cmd = new SqlCommand("SELECT * FROM aspnet_Users WHERE UserName=@UserName");
            cmd.Parameters.AddWithValue("@UserName", username);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                cmd = new SqlCommand("SELECT * FROM aspnet_Membership WHERE UserId=@UserId");
                cmd.Parameters.AddWithValue("@UserId", dt.Rows[0]["UserId"]);
                DataTable dt1 = Connection.GetDataTable(cmd);
                if (dt1.Rows.Count > 0)
                {
                    HoTen = Convert.ToString(dt1.Rows[0]["FullName"]);
                }
                dt1.Dispose();
            }
            dt.Dispose();
            return HoTen;
        }
        //Kiểm tra quyền xem theo menu tránh trường hợp gõ trực tiếp trên url--- Tuannn
        public static Boolean CoQuyenXemTheoMenu(String URL, String MaNguoiDung)
        {

            Boolean CoQuyen = true;
            if (URL.Length > 0)
            {
                URL = URL.Remove(0, 1);
            }
            URL = URL.ToLower();
            String MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaNguoiDung);
            String SQL = "SELECT sURL FROM MENU_MenuItem WHERE iID_MaMenuItem  IN (SELECT iID_MaMenuItem FROM PQ_MenuItem_Cam WHERE iID_MaLuat IN " +
                         "(SELECT iID_MaLuat FROM PQ_NhomNguoiDung_Luat WHERE iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung))";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt != null && dt.Rows.Count > 0)
            {
                String menuUrl = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    menuUrl = Convert.ToString(dt.Rows[i]["sURL"]).ToLower();
                    //if (URL.Equals(menuUrl) && menuUrl != "" || URL.IndexOf(menuUrl) >= 0)
                    //if ((URL.Equals(menuUrl)  || URL.IndexOf(menuUrl) >= 0) && menuUrl != "" ) 
                    if (URL.Equals(menuUrl) && menuUrl != "")
                    {
                        CoQuyen = false;
                        break;
                    }
                }
            }
            if (dt != null)
            {
                dt.Dispose();
            }
            return CoQuyen;
        }

        //phuonglt85
        public static DataTable getYear(DateTime date, Boolean All = false, string NoiDung = "")
        {
            int NamMin = Convert.ToInt32(date.Year) - 10;
            int NamMax = Convert.ToInt32(date.Year) + 10;
            DataTable dtNam = new DataTable();
            dtNam.Columns.Add("MaNam", typeof(int));
            dtNam.Columns.Add("TenNam", typeof(String));
            DataRow Row;
            for (int i = NamMin; i < NamMax; i++)
            {
                Row = dtNam.NewRow();
                dtNam.Rows.Add(Row);
                Row[0] = Convert.ToString(i);
                Row[1] = Convert.ToString(i);
            }
            if (All)
            {
                DataRow R = dtNam.NewRow();
                R[0] = 0;
                R[1] = NoiDung;
                dtNam.Rows.InsertAt(R, 0);
            }
            return dtNam;
        }
        public static DataTable getMonth(DateTime date, Boolean All = false, string NoiDung = "", string Title = "")
        {
            DataTable dtThang = new DataTable();
            dtThang.Columns.Add("MaThang", typeof(int));
            dtThang.Columns.Add("TenThang", typeof(String));
            DataRow RThang;
            for (int i = 1; i <= 12; i++)
            {
                RThang = dtThang.NewRow();
                dtThang.Rows.Add(RThang);
                RThang[0] = Convert.ToString(i);
                RThang[1] = Title + " " + Convert.ToString(i);
            }
            if (All)
            {
                DataRow R = dtThang.NewRow();
                R[0] = 0;
                R[1] = NoiDung;
                dtThang.Rows.InsertAt(R, 0);
            }
            return dtThang;
        }
        public static DataTable getDaysInMonths(int month, int year, Boolean All = false, string NoiDung = "", string Title = "")
        {
            DataTable dtThang = new DataTable();
            dtThang.Columns.Add("MaNgay", typeof(int));
            dtThang.Columns.Add("TenNgay", typeof(String));
            DataRow RThang;
            int ngay = GetDaysInMonth(month, year);
            for (int i = 1; i <= ngay; i++)
            {
                RThang = dtThang.NewRow();
                dtThang.Rows.Add(RThang);
                RThang[0] = Convert.ToString(i);
                RThang[1] = Title + " " + Convert.ToString(i);
            }
            if (All)
            {
                DataRow R = dtThang.NewRow();
                R[0] = 0;
                R[1] = NoiDung;
                dtThang.Rows.InsertAt(R, 0);
            }
            return dtThang;
        }
        public static int GetDaysInMonth(int month, int year)
        {
            if (month < 1 || month > 12)
            {

            }
            if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||
            10 == month || 12 == month)
            {
                return 31;
            }
            else if (2 == month)
            {

                if (0 == (year % 4))
                {

                    if (0 == (year % 400))
                    {
                        return 29;
                    }
                    else if (0 == (year % 100))
                    {
                        return 28;
                    }


                    return 29;
                }

                return 28;
            }
            return 30;
        }
        public static string ConvertToString(object obj)
        {

            try
            {
                if (Convert.ToString(obj) == "00000000-0000-0000-0000-000000000000")
                {
                    return "";
                }
                else
                {
                    return Convert.ToString(obj); 
                }
               
            }
            catch
            {
                return "";
            }
        }
        public static Double ConvertToDouble(object obj)
        {

            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0;
            }
        }
        public static decimal ConvertToDecimal(object obj)
        {

            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0;
            }
        }
        public static Boolean ConvertBoolean(object obj)
        {

            try
            {
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }
        public static string getStringNull(string obj)
        {
            if (obj == "01/01/0001") return "---";
            else return obj;
        }
        public static DateTime ConvertDateTime(object date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        public static string LayChuoiNgay(int Day, int Month, int Year)
        {
            string strDay = "";
            string strMonth = "";
            if (Day < 10) return strDay = "0" + Day.ToString();
            if (Month < 10) return strMonth = "0" + Month.ToString();
            return strDay + "/" + strMonth + "/" + Year.ToString();
        }

        /// <summary>
        /// Lay gia tri
        /// </summary>
        /// <param name="GiaTri"></param>
        /// <param name="HienThi"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string getGiaTri(String GiaTri, String HienThi, DataTable dt)
        {
            String vR = "";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                builder.Append("<option value=" + Convert.ToString(dr[GiaTri]) + ">" + HttpUtility.HtmlEncode(Convert.ToString(dr[HienThi])) + "</option>");
            }
            vR = builder.ToString();
            return vR;
        }
        /// <summary>
        /// Lấy danh sách trạng thái duyệt
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTrangThai(String maPhanHe, String maTrangThaiDuyet)
        {
            String SQL = String.Format(@"SELECT PH.iID_MaTrangThaiDuyet,PH.sTen
                                         FROM NS_PhanHe_TrangThaiDuyet AS PH
                                         WHERE PH.iTrangThai=1
                                           AND PH.iID_MaPhanHe=@iID_MaPhanHe
                                           AND PH.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", maPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", maTrangThaiDuyet);
            DataTable dt=Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }

     /// <summary>
     /// Lấy dánh sách trạng thái duyệt
     /// </summary>
     /// <param name="MaPhanHe"></param>
     /// <param name="MaTrangThaiDuyet"></param>
     /// <param name="ChonTatCa"></param>
     /// <param name="TenMacDinh"></param>
     /// <returns></returns>
        public static DataTable GetTrangThai(int MaPhanHe, int MaTrangThaiDuyet, Boolean ChonTatCa=true, String TenMacDinh = "--Tất cả--")
        {
            String SQL = String.Format(@"SELECT PH.iID_MaTrangThaiDuyet,PH.sTen
                                         FROM NS_PhanHe_TrangThaiDuyet AS PH
                                         WHERE PH.iTrangThai=1
                                           AND PH.iID_MaPhanHe=@iID_MaPhanHe
                                           AND PH.iID_MaTrangThaiDuyet=@iID_MaTrangThaiDuyet");
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaPhanHe", MaPhanHe);
            cmd.Parameters.AddWithValue("@iID_MaTrangThaiDuyet", MaTrangThaiDuyet);
            DataTable dt=Connection.GetDataTable(cmd);
            if (ChonTatCa)
            {
                DataRow dR = dt.NewRow();
                dR["iID_MaTrangThaiDuyet"] = "0";
                dR["sTen"] = TenMacDinh;
                dt.Rows.InsertAt(dR, 0);
            }
            cmd.Dispose();
            return dt;
        }
        public static void Language()
        {
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
        }


        public static string RenderPartialViewToStringLoad(string ViewName, object Model, Controller controller)
        {
            if (string.IsNullOrEmpty(ViewName))
                ViewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = Model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, ViewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                String tg = sw.GetStringBuilder().ToString();
                //String strJ = "";
                //BocTachDuLieu(ref tg, ref strJ);                
                //tg = tg.Trim();
                //strJ = strJ.Trim();
                //strJ = JavaScriptEncode(strJ);
                //strJ = String.Format("{0} <script>{1}</script>;", tg, strJ);
                return tg;
            }
        }
        private static string JavaScriptEncode(String str)
        {
            // Encode certain characters, or the JavaScript expression could be invalid

            return new JavaScriptSerializer().Serialize(str);
            //return "";
        }

        public static void BocTachDuLieu(ref string str1, ref string str2)
        {
            Boolean ok = true;
            str1 = str1.Replace("\r", "");
            str1 = str1.Replace("\n", "");

            while (ok)
            {
                int cs1 = str1.IndexOf("<script");

                if (cs1 >= 0)
                {
                    int cs2 = str1.IndexOf("</script>");
                    string tg = str1.Substring(cs1, cs2 - cs1 + 9);
                    str1 = str1.Remove(cs1, cs2 - cs1 + 9);
                    cs1 = tg.IndexOf(">");
                    tg = tg.Substring(cs1 + 1, tg.Length - cs1 - 10);
                    //tg = tg.Replace("\r", "");
                    //tg = tg.Replace("\n", "");
                    str2 += tg;
                }
                else
                {
                    ok = false;
                }
            }
        }
        public static string getPhanHe(String MaNguoiDung)
        {
            String VR = "";
            String MaNhomNguoiDung = BaoMat.LayMaNhomNguoiDung(MaNguoiDung);
            String SQL = "SELECT sTen,sIP FROM MENU_MenuItem WHERE iID_MaMenuItem !=1 AND iID_MaMenuItemCha=0 AND iID_MaMenuItem NOT IN (SELECT iID_MaMenuItem FROM PQ_MenuItem_Cam WHERE iID_MaLuat IN " +
                         "(SELECT iID_MaLuat FROM PQ_NhomNguoiDung_Luat WHERE iID_MaNhomNguoiDung=@iID_MaNhomNguoiDung))";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
            DataTable dt = Connection.GetDataTable(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    VR += ", <a href=\"" + Convert.ToString(dr["sIP"]) + "\"/>" + Convert.ToString(dr["sTen"]) + "</a>";
                }
                dt.Dispose();
            } 
            if (VR.Length > 1)
            {
                VR = VR.Substring(1, VR.Length - 1) + ".";
            }
            
            return VR;
        }
    }
    public class DanhSachDonViModels
    {
        public DanhSachDonViModels(String MaND, String sMaDonVi, DataTable dtDonVi, String ControlID, int iSoCot = 1)
        {
            this.sMaDonVi = sMaDonVi;
            this.MaND = MaND;
            this.dtDonVi = dtDonVi;
            this.ControlID = ControlID;
            this.iSoCot = iSoCot;
        }
        public String sMaDonVi { get; set; }
        public String MaND { get; set; }
        public DataTable dtDonVi { get; set; }
        public String ControlID { get; set; }
        public int iSoCot { get; set; }

    }
}
