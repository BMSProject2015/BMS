using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using DomainModel;
using DomainModel.Abstract;
using System.Text;
namespace VIETTEL.Models
{
    public class TaiKhoanDanhMucChiTietModels
    {

        public static NameValueCollection LayThongTinChiTietTaiKhoanDanhMuc(String iID_MaTaiKhoanDanhMucChiTiet)
        {

            NameValueCollection Data = new NameValueCollection();
            DataTable dt = Get_ChiTietTaiKhoanDanhMuc(iID_MaTaiKhoanDanhMucChiTiet);
            String colName = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                colName = dt.Columns[i].ColumnName;
                Data[colName] = Convert.ToString(dt.Rows[0][i]);
            }
            dt.Dispose();
            return Data;
        }
        public static DataTable Get_ChiTietTaiKhoanDanhMuc(String iID_MaTaiKhoanDanhMucChiTiet)
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet ORDER BY sKyHieu";
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }
        public static DataTable Get_DanhSachTaiKhoanDanhMucChiTiet()
        {
            DataTable dt = null;
            SqlCommand cmd = new SqlCommand();
            String SQL = "SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 ORDER BY sKyHieu";
            cmd.CommandText = SQL;
            dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            return dt;
        }


        public static void CapNhapLai()
        {
            //if (String.IsNullOrEmpty(iID_MaTaiKhoan))
            //{
                SqlCommand cmd = new SqlCommand();
                String SQL =
                  String.Format(
                      @"DELETE KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sKyHieu is null or sKyHieu='';
UPDATE KT_TaiKhoanDanhMucChiTiet  set iID_MaTaiKhoanDanhMucChiTiet_Cha=(select top 1 iID_MaTaiKhoanDanhMucChiTiet from 
KT_TaiKhoanDanhMucChiTiet kt where  kt.sKyHieu=KT_TaiKhoanDanhMucChiTiet.sXauNoiMa and kt.iTrangThai=1)
WHERE iTrangThai=1 and sXauNoiMa!='' and sXauNoiMa is not null;
UPDATE KT_TaiKhoanDanhMucChiTiet  set iID_MaTaiKhoanDanhMucChiTiet_Cha=NULL, sXauNoiMa_Cha=sKyHieu WHERE iTrangThai=1 and sXauNoiMa ='' OR sXauNoiMa is null;
UPDATE KT_TaiKhoanDanhMucChiTiet set bLaHangCha=1 where bLaHangCha=0 and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai=1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha=KT_TaiKhoanDanhMucChiTiet.iID_MaTaiKhoanDanhMucChiTiet);
UPDATE KT_TaiKhoanDanhMucChiTiet set bLaHangCha=0 where bLaHangCha=1 and not exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai=1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha=KT_TaiKhoanDanhMucChiTiet.iID_MaTaiKhoanDanhMucChiTiet);");
                cmd.CommandText = SQL;
               // cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
//            }
//            else
//            {
//                SqlCommand cmd = new SqlCommand();

//                String SQL =
//                  String.Format(
//                      @"DELETE KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sKyHieu is null or sKyHieu='';
//UPDATE KT_TaiKhoanDanhMucChiTiet  set iID_MaTaiKhoanDanhMucChiTiet_Cha=(select top 1 kt.iID_MaTaiKhoanDanhMucChiTiet from 
//KT_TaiKhoanDanhMucChiTiet kt, KT_TaiKhoanGiaiThich CT where CT.iTrangThai=1 AND CT.iID_MaTaiKhoanDanhMucChiTiet=kt.iID_MaTaiKhoanDanhMucChiTiet 
//and  kt.sKyHieu=KT_TaiKhoanDanhMucChiTiet.sXauNoiMa and kt.iTrangThai=1 and CT.iID_MaTaiKhoan=@iID_MaTaiKhoan) WHERE iTrangThai=1 and sXauNoiMa!='' and sXauNoiMa is not null;
//UPDATE KT_TaiKhoanDanhMucChiTiet  set iID_MaTaiKhoanDanhMucChiTiet_Cha=NULL, sXauNoiMa_Cha=sKyHieu WHERE iTrangThai=1 and sXauNoiMa ='' OR sXauNoiMa is null;
//UPDATE KT_TaiKhoanDanhMucChiTiet set bLaHangCha=1 where bLaHangCha=0 and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai=1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha=KT_TaiKhoanDanhMucChiTiet.iID_MaTaiKhoanDanhMucChiTiet);
//UPDATE KT_TaiKhoanDanhMucChiTiet set bLaHangCha=0 where bLaHangCha=1 and not exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanDanhMucChiTiet ct where ct.iTrangThai=1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha=KT_TaiKhoanDanhMucChiTiet.iID_MaTaiKhoanDanhMucChiTiet);");
//                cmd.CommandText = SQL;
                
//                cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
//                Connection.UpdateDatabase(cmd);
//                cmd.Dispose();
//            }
            

        }

        public static void CapNhapLai_XauNoiMa(String sXauNoiMa_Cha, String iID_MaTaiKhoanDanhMucChiTiet)
        {
            SqlCommand cmd = new SqlCommand();
            String SQL =
                String.Format(
                    @"update KT_TaiKhoanDanhMucChiTiet set sXauNoiMa_Cha=@sXauNoiMa_Cha where iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet");
            cmd.CommandText = SQL;
            cmd.Parameters.AddWithValue("@sXauNoiMa_Cha", sXauNoiMa_Cha);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
            Connection.UpdateDatabase(cmd);
            cmd.Dispose();
        }

        public static void DetailSubmit(String UserID, String IPSua,String iID_MaTaiKhoan, NameValueCollection Values = null)
        {
            String TenBangChiTiet = "KT_TaiKhoanDanhMucChiTiet";
            string idXauMaCacHang = Values["idXauMaCacHang"];
            string idXauMaCacCot = Values["idXauMaCacCot"];
            string idXauGiaTriChiTiet = Values["idXauGiaTriChiTiet"];
            string idXauCacHangDaXoa = Values["idXauCacHangDaXoa"];
            string idXauDuLieuThayDoi = Values["idXauDuLieuThayDoi"];
            String[] arrMaHang = idXauMaCacHang.Split(',');
            String[] arrHangDaXoa = idXauCacHangDaXoa.Split(',');
            String[] arrMaCot = idXauMaCacCot.Split(',');
            String[] arrHangGiaTri = idXauGiaTriChiTiet.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);

            String iID_MaChungTuChiTiet;
            //Luu cac hang sua
            String[] arrHangThayDoi = idXauDuLieuThayDoi.Split(new string[] { BangDuLieu.DauCachHang }, StringSplitOptions.None);
            for (int i = 0; i < arrMaHang.Length; i++)
            {
                iID_MaChungTuChiTiet = arrMaHang[i];
                if (arrHangDaXoa[i] == "1")
                {
                    //Lưu các hàng đã xóa
                    if (iID_MaChungTuChiTiet != "")
                    {
                        //Dữ liệu đã có
                        if (Is_TaiKhoan_ChiTiet(Convert.ToInt32(iID_MaChungTuChiTiet)) == false)
                        {
                            Bang bang = new Bang(TenBangChiTiet);
                            bang.DuLieuMoi = false;
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.CmdParams.Parameters.AddWithValue("@iTrangThai", 0);
                            bang.MaNguoiDungSua = UserID;
                            bang.IPSua = IPSua;
                            bang.Save();
                        }
                    }
                }
                else
                {
                    String[] arrGiaTri = arrHangGiaTri[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    String[] arrThayDoi = arrHangThayDoi[i].Split(new string[] { BangDuLieu.DauCachO }, StringSplitOptions.None);
                    Boolean okCoThayDoi = false;
                    for (int j = 0; j < arrMaCot.Length; j++)
                    {
                        if (arrThayDoi[j] == "1")
                        {
                            okCoThayDoi = true;
                            break;
                        }
                    }
                    if (okCoThayDoi)
                    {
                        Bang bang = new Bang(TenBangChiTiet);
                        iID_MaChungTuChiTiet = arrMaHang[i];
                        if (iID_MaChungTuChiTiet == "")
                        {
                            //Du Lieu Moi
                            bang.DuLieuMoi = true;
                        }
                        else
                        {
                            //Du Lieu Da Co
                            bang.GiaTriKhoa = iID_MaChungTuChiTiet;
                            bang.DuLieuMoi = false;
                        }
                        bang.MaNguoiDungSua = UserID;
                        bang.IPSua = IPSua;
                        bang.CmdParams.Parameters.AddWithValue("@iSTT", i);
                        //Them tham so
                        String sKyHieu = "";
                        String sXauNoiMa = "";
                        for (int j = 0; j < arrMaCot.Length; j++)
                        {
                            if (arrThayDoi[j] == "1")
                            {
                                String Truong = "@" + arrMaCot[j];
                                if (arrMaCot[j].StartsWith("b"))
                                {
                                    //Nhap Kieu checkbox
                                    if (arrGiaTri[j] == "1")
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, true);
                                    }
                                    else
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, false);
                                    }
                                }
                                else if (arrMaCot[j].StartsWith("r") || (arrMaCot[j].StartsWith("i") && arrMaCot[j].StartsWith("iID") == false))
                                {
                                    //Nhap Kieu so
                                    if (CommonFunction.IsNumeric(arrGiaTri[j]))
                                    {
                                        bang.CmdParams.Parameters.AddWithValue(Truong, Convert.ToDouble(arrGiaTri[j]));
                                    }
                                }
                                else
                                {
                                  //Nhap kieu xau
                                    bang.CmdParams.Parameters.AddWithValue(Truong, arrGiaTri[j]);
                                    if (arrMaCot[j].StartsWith("sKyHieu"))
                                    {
                                        sKyHieu = arrGiaTri[j];
                                    }
                                    if (arrMaCot[j].StartsWith("sXauNoiMa"))
                                    {
                                        sXauNoiMa = arrGiaTri[j];
                                    }
                                }
                            }
                        }
                      //  bang.CmdParams.Parameters.AddWithValue("@sXauNoiMa_Cha",
                                                              // Lay_sXauNoiMa_Cha(sXauNoiMa, sKyHieu, iID_MaChungTuChiTiet, iID_MaTaiKhoan));
                        bang.Save();
                        CapNhapLai();
                        String sXauNoiMa_Cha = Lay_sXauNoiMa_Cha(sXauNoiMa, sKyHieu, iID_MaChungTuChiTiet,
                                                                 iID_MaTaiKhoan);
                        CapNhapLai_XauNoiMa(sXauNoiMa_Cha, iID_MaChungTuChiTiet);
                        Update_sXauNoiMa_Con(iID_MaChungTuChiTiet);
                        if (bang.DuLieuMoi == true && String.IsNullOrEmpty(iID_MaTaiKhoan) == false &&
                            iID_MaTaiKhoan != Guid.Empty.ToString() && String.IsNullOrEmpty(sKyHieu)==false)
                        {
                            SqlCommand cmd;
                            String SQL =
                               "INSERT INTO KT_TaiKhoanGiaiThich(iID_MaTaiKhoan,iID_MaTaiKhoanDanhMucChiTiet) SELECT @iID_MaTaiKhoan, MAX(iID_MaTaiKhoanDanhMucChiTiet) FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1";
                            cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                            Connection.UpdateDatabase(cmd);
                            cmd.Dispose();

                        }
                        else
                        {
                            SqlCommand cmd;
                            String SQL =
                               "UPDATE KT_ChungTuChiTiet SET sTenTaiKhoanGiaiThich_No=(SELECT TOP 1 sKyHieu + '-' + sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet) WHERE iTrangThai=1 AND iID_MaTaiKhoanGiaiThich_No=@iID_MaTaiKhoanDanhMucChiTiet; " +
                               "UPDATE KT_ChungTuChiTiet SET sTenTaiKhoanGiaiThich_Co=(SELECT TOP 1 sKyHieu + '-' + sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet) WHERE iTrangThai=1 AND iID_MaTaiKhoanGiaiThich_Co=@iID_MaTaiKhoanDanhMucChiTiet; " +
                               "UPDATE KT_SoDuTaiKhoanGiaiThich SET sTenTaiKhoanGiaiThich_No=(SELECT TOP 1 sKyHieu + '-' + sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet) WHERE iTrangThai=1 AND iID_MaTaiKhoanGiaiThich_No=@iID_MaTaiKhoanDanhMucChiTiet; " +
                               "UPDATE KT_SoDuTaiKhoanGiaiThich SET sTenTaiKhoanGiaiThich_Co=(SELECT TOP 1 sKyHieu + '-' + sTen FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet) WHERE iTrangThai=1 AND iID_MaTaiKhoanGiaiThich_Co=@iID_MaTaiKhoanDanhMucChiTiet; ";
                            cmd = new SqlCommand(SQL);
                            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaChungTuChiTiet);
                            Connection.UpdateDatabase(cmd);
                            cmd.Dispose();  
                        }
                     
                    }
                }
            }
            //CapNhapLai();
        }
        private static Boolean Is_TaiKhoan_ChiTiet(int iID_MaTaiKhoanDanhMucChiTiet)
        {
            int vR = 0;
            SqlCommand cmd;
            //String SQL =
            //    "SELECT iID_MaTaiKhoanDanhMucChiTiet FROM KT_TaiKhoanGiaiThich WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
            String SQL =
                String.Format(
                    @"SELECT iID_MaTaiKhoanDanhMucChiTiet FROM KT_TaiKhoanGiaiThich WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet
union
SELECT iID_MaTaiKhoanGiaiThich_No as iID_MaTaiKhoanDanhMucChiTiet FROM KT_ChungTuChiTiet WHERE iID_MaTaiKhoanGiaiThich_No=@iID_MaTaiKhoanDanhMucChiTiet
union
SELECT iID_MaTaiKhoanGiaiThich_Co as iID_MaTaiKhoanDanhMucChiTiet FROM KT_ChungTuChiTiet WHERE iID_MaTaiKhoanGiaiThich_Co=@iID_MaTaiKhoanDanhMucChiTiet
union
SELECT iID_MaTaiKhoanGiaiThich_No as iID_MaTaiKhoanDanhMucChiTiet FROM KT_SoDuTaiKhoanGiaiThich WHERE iID_MaTaiKhoanGiaiThich_No=@iID_MaTaiKhoanDanhMucChiTiet
union
SELECT iID_MaTaiKhoanGiaiThich_Co as iID_MaTaiKhoanDanhMucChiTiet FROM KT_SoDuTaiKhoanGiaiThich WHERE iID_MaTaiKhoanGiaiThich_Co=@iID_MaTaiKhoanDanhMucChiTiet");
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
            vR = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();
            if (vR > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private static  void Update_sXauNoiMa_Con(String iID_MaTaiKhoanDanhMucChiTiet)
        {
            // lay ma noi cua chung tu
            SqlCommand cmd;
            String SQL = "select sXauNoiMa_Cha from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
            String sXauNoiMa_Cha = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            // update cac chug tu con
            SQL = "select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 and iID_MaTaiKhoanDanhMucChiTiet_Cha=@iID_MaTaiKhoanDanhMucChiTiet";
            cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
             DataTable vR = Connection.GetDataTable(cmd);
             if (vR != null && vR.Rows.Count > 0)
             {
                 for (int i = 0; i < vR.Rows.Count; i++)
                 {
                     DataRow dr = vR.Rows[i];
                     int iID_MaTaiKhoanDanhMucChiTiet_Con = Convert.ToInt32(dr["iID_MaTaiKhoanDanhMucChiTiet"]);
                     SQL = "UPDATE KT_TaiKhoanDanhMucChiTiet set sXauNoiMa_Cha=@sXauNoiMa_Cha + '' +  sKyHieu WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet_Con";
                     cmd = new SqlCommand(SQL);
                     cmd.Parameters.AddWithValue("@sXauNoiMa_Cha", sXauNoiMa_Cha);
                     cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet_Con", iID_MaTaiKhoanDanhMucChiTiet_Con);
                     Connection.UpdateDatabase(cmd);
                     Update_sXauNoiMa_Con(Convert.ToString(iID_MaTaiKhoanDanhMucChiTiet_Con));
                 }
             }
        }
        /// <summary>
        /// Lay xau noi ma cha
        /// </summary>
        /// <param name="sKyHieu_Cha"></param>
        /// <returns></returns>
        private static String Lay_sXauNoiMa_Cha(String sKyHieu_Cha, String sKyHieu, String iID_MaTaiKhoanDanhMucChiTiet, String iID_MaTaiKhoan)
        {
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) == true || iID_MaTaiKhoan == Guid.Empty.ToString())
            {
                String vR = "";
                SqlCommand cmd;
                String SQL = "";
                if (String.IsNullOrEmpty(sKyHieu_Cha) == false && String.IsNullOrEmpty(sKyHieu) == false)
                {
                    SQL =
                        "SELECT TOP 1 sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet WHERE sKyHieu=@sKyHieu";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu_Cha);
                    String Value = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value + "" + sKyHieu;
                }
                else if (String.IsNullOrEmpty(sKyHieu_Cha) == true && String.IsNullOrEmpty(sKyHieu) == false)
                {
                    SQL =
                        "SELECT TOP 1 sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=(SELECT iID_MaTaiKhoanDanhMucChiTiet_Cha from  KT_TaiKhoanDanhMucChiTiet where iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet)";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value + "" + sKyHieu;
                }
                else if (String.IsNullOrEmpty(sKyHieu_Cha) == false && String.IsNullOrEmpty(sKyHieu) == true)
                {
                    SQL =
                        "SELECT TOP 1 sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet WHERE sKyHieu=@sKyHieu";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu_Cha);
                    String Value_sKyHieu_Cha = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();

                    SQL =
                        "SELECT TOP 1 sKyHieu FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value_sKyHieu_Cha + "" + Value_sKyHieu;
                }
                else
                {
                    SQL =
                        "SELECT TOP 1 sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=(SELECT iID_MaTaiKhoanDanhMucChiTiet_Cha from  KT_TaiKhoanDanhMucChiTiet where iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet)";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu_Cha = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();


                    SQL =
                        "SELECT TOP 1 sKyHieu FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value_sKyHieu_Cha + "" + Value_sKyHieu;
                }
                return vR;
            }
            else
            {
                // truong hop co tai khoan
                String vR = "";
                SqlCommand cmd;
                String SQL = "";
                if (String.IsNullOrEmpty(sKyHieu_Cha) == false && String.IsNullOrEmpty(sKyHieu) == false)
                {
                    SQL =
                        "SELECT TOP 1 dm.sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet AS dm, KT_TaiKhoanGiaiThich AS tk WHERE dm.iTrangThai=1 AND  tk.iTrangThai=1 AND" +
                        "  dm.iID_MaTaiKhoanDanhMucChiTiet = tk.iID_MaTaiKhoanDanhMucChiTiet AND tk.iID_MaTaiKhoan=@iID_MaTaiKhoan AND dm.sKyHieu=@sKyHieu";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                    cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu_Cha);
                    String Value = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value + "" + sKyHieu;
                }
                else if (String.IsNullOrEmpty(sKyHieu_Cha) == true && String.IsNullOrEmpty(sKyHieu) == false)
                {
                    SQL =
                        "SELECT TOP 1 dm.sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet AS dm, KT_TaiKhoanGiaiThich AS tk WHERE dm.iTrangThai=1 AND  tk.iTrangThai=1 AND" +
                        "  dm.iID_MaTaiKhoanDanhMucChiTiet = tk.iID_MaTaiKhoanDanhMucChiTiet AND tk.iID_MaTaiKhoan=@iID_MaTaiKhoan AND dm.iID_MaTaiKhoanDanhMucChiTiet=(SELECT iID_MaTaiKhoanDanhMucChiTiet_Cha from  KT_TaiKhoanDanhMucChiTiet where iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet)";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value + "" + sKyHieu;
                }
                else if (String.IsNullOrEmpty(sKyHieu_Cha) == false && String.IsNullOrEmpty(sKyHieu) == true)
                {
                    SQL =
                        "SELECT TOP 1 dm.sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet AS dm, KT_TaiKhoanGiaiThich AS tk WHERE dm.iTrangThai=1 AND  tk.iTrangThai=1 AND" +
                        "  dm.iID_MaTaiKhoanDanhMucChiTiet = tk.iID_MaTaiKhoanDanhMucChiTiet AND tk.iID_MaTaiKhoan=@iID_MaTaiKhoan AND dm.sKyHieu=@sKyHieu";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoan", iID_MaTaiKhoan);
                    cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu_Cha);
                    String Value_sKyHieu_Cha = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();

                    SQL =
                        "SELECT TOP 1 sKyHieu FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value_sKyHieu_Cha + "" + Value_sKyHieu;
                }
                else
                {
                    SQL =
                        "SELECT TOP 1 sXauNoiMa_Cha FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=(SELECT iID_MaTaiKhoanDanhMucChiTiet_Cha from  KT_TaiKhoanDanhMucChiTiet where iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet)";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu_Cha = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();


                    SQL =
                        "SELECT TOP 1 sKyHieu FROM KT_TaiKhoanDanhMucChiTiet WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
                    cmd = new SqlCommand(SQL);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
                    String Value_sKyHieu = Convert.ToString(Connection.GetValue(cmd, ""));
                    cmd.Dispose();
                    vR = Value_sKyHieu_Cha + "" + Value_sKyHieu;
                }
                return vR.Replace(" ", "");
            }
        }
        public static String LayXauTaiKhoanDanhMucChiTiet(string Path, string XauHanhDong, string XauSapXep, String MaTaiKhoanDanhMucChiTiet_Cha, int Cap, ref int ThuTu, String  iID_MaTaiKhoan)
        {
            String vR = "";
            String SQL = "";
            if (String.IsNullOrEmpty(iID_MaTaiKhoan) || iID_MaTaiKhoan==Guid.Empty.ToString())
            {
                if (MaTaiKhoanDanhMucChiTiet_Cha != null && MaTaiKhoanDanhMucChiTiet_Cha != "")
                {
                    SQL = string.Format("SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai = 1 AND iID_MaTaiKhoanDanhMucChiTiet_Cha= '{0}' ORDER BY sKyHieu", MaTaiKhoanDanhMucChiTiet_Cha);
                }
                else
                {
                    SQL = string.Format("SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai = 1 AND (iID_MaTaiKhoanDanhMucChiTiet_Cha is null OR iID_MaTaiKhoanDanhMucChiTiet_Cha='0') ORDER BY sKyHieu", MaTaiKhoanDanhMucChiTiet_Cha);
                }  
            }
            else
            {
                if (MaTaiKhoanDanhMucChiTiet_Cha != null && MaTaiKhoanDanhMucChiTiet_Cha != "")
                {
                    SQL =
                        string.Format(
                            "SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND iID_MaTaiKhoanDanhMucChiTiet_Cha= '{0}' and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet and iID_MaTaiKhoan='{1}') ORDER BY ct.sKyHieu",
                            MaTaiKhoanDanhMucChiTiet_Cha, iID_MaTaiKhoan);
                }
                else
                {
                    SQL = string.Format("SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND (ct.iID_MaTaiKhoanDanhMucChiTiet_Cha is null OR ct.iID_MaTaiKhoanDanhMucChiTiet_Cha='0') and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet and iID_MaTaiKhoan='{0}') ORDER BY ct.sKyHieu", iID_MaTaiKhoan);
                }   
            }
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauCon, strDoanTrang = "";
                StringBuilder strb = new StringBuilder();
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];

                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaTaiKhoanDanhMucChiTiet"].ToString());
                    strXauCon = LayXauTaiKhoanDanhMucChiTiet(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaTaiKhoanDanhMucChiTiet"]), Cap + 1, ref ThuTu, iID_MaTaiKhoan);

                    if (strXauCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaTaiKhoanDanhMucChiTiet"].ToString());
                    }
                    //strPG += string.Format("<tr " + classtr + ">");
                    String TaiKhoan_GiaiThich =
                      DS_TaiKhoan_GiaiThich(Convert.ToString(Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                    Boolean bLaHangCha = Convert.ToBoolean(Row["bLaHangCha"]);
                    strb.Append(string.Format("<tr " + classtr + ">"));
                    if (Cap == 0 || bLaHangCha == true)
                    {

                        strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}<b>{1}</b></td>", "", tgThuTu));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;display:none;\">{0}<b>{1}</b></td>", strDoanTrang, Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sKyHieu"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sXauNoiMa"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));

                    }
                    else
                    {
                       
                        //if (tgThuTu % 2 == 0)
                        //{

                        //    strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}{1}</td>", "", tgThuTu));
                        //    strb.Append(string.Format("<td style=\"padding: 3px 3px;display:none;\">{0}{1}</td>", strDoanTrang, Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                        //    strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]));
                        //    strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]));
                        //    strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));
                        //}
                        //else
                        //{

                            strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}{1}</td>", "", tgThuTu));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;display:none;\">{0}{1}</td>", strDoanTrang, Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", Row["sXauNoiMa"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));
                        //}
                    }
                    //if (tgThuTu % 2 == 0)
                    //{
                       
                    //    strb.Append(string.Format("<td style=\"padding: 3px 3px;\" align=\"center\">{0}</td>", strHanhDong));
                    //}
                    //else
                    //{
                      
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\" align=\"center\">{0}</td>", strHanhDong));
                   //}

                    //strPG += string.Format("</tr>");
                    //strPG += strXauCon;
                    strb.Append(string.Format("</tr>"));
                    strb.Append(strXauCon);
                }
                //vR = String.Format("{0}", strPG);
                vR = strb.ToString();
            }
            dt.Dispose();
            return vR;
        }

        public static String LayXauTaiKhoanDanhMucChiTiet_TaiKhoan(string Path, string XauHanhDong, string XauSapXep, String MaTaiKhoanDanhMucChiTiet_Cha, int Cap, ref int ThuTu, DataTable dtGiaiThich, int iTrangThai)
        {
            String vR = "";
            String SQL = "";
            if (MaTaiKhoanDanhMucChiTiet_Cha != null && MaTaiKhoanDanhMucChiTiet_Cha != "")
            {
                if (iTrangThai == 0)
                {
                    SQL =
                     string.Format(
                         "SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha= '{0}' and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet) ORDER BY ct.sKyHieu",
                         MaTaiKhoanDanhMucChiTiet_Cha);
                }
                else if (iTrangThai == 1)
                {
                    SQL =
                     string.Format(
                         "SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND ct.iID_MaTaiKhoanDanhMucChiTiet_Cha= '{0}' and  not exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet) ORDER BY ct.sKyHieu",
                         MaTaiKhoanDanhMucChiTiet_Cha);
                }
                else
                {
                    SQL =
                        string.Format(
                            "SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai = 1 AND iID_MaTaiKhoanDanhMucChiTiet_Cha= '{0}' ORDER BY sKyHieu",
                            MaTaiKhoanDanhMucChiTiet_Cha);
                }
            }
            else
            {
                if (iTrangThai == 0)
                {
                    SQL =
                     string.Format(
                         "SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND (ct.iID_MaTaiKhoanDanhMucChiTiet_Cha is null OR ct.iID_MaTaiKhoanDanhMucChiTiet_Cha='0') and  exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet) ORDER BY ct.sKyHieu");
                }
                else if (iTrangThai == 1)
                {
                    SQL =
                      string.Format(
                          "SELECT * FROM KT_TaiKhoanDanhMucChiTiet ct WHERE ct.iTrangThai = 1 AND (ct.iID_MaTaiKhoanDanhMucChiTiet_Cha is null OR ct.iID_MaTaiKhoanDanhMucChiTiet_Cha='0') and  not exists (select iID_MaTaiKhoanDanhMucChiTiet from KT_TaiKhoanGiaiThich tk where tk.iTrangThai=1 and tk.iID_MaTaiKhoanDanhMucChiTiet=ct.iID_MaTaiKhoanDanhMucChiTiet) ORDER BY ct.sKyHieu");
                   
                }
                else
                {
                    SQL =
                        string.Format(
                            "SELECT * FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai = 1 AND (iID_MaTaiKhoanDanhMucChiTiet_Cha is null OR iID_MaTaiKhoanDanhMucChiTiet_Cha='0') ORDER BY sKyHieu");
                }
            }
            DataTable dt = Connection.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                int i, tgThuTu;

                string strPG = "", strXauCon, strDoanTrang = "";
                StringBuilder strb = new StringBuilder();
                for (i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    ThuTu++;
                    tgThuTu = ThuTu;
                    DataRow Row = dt.Rows[i];

                    String strHanhDong = XauHanhDong.Replace("%23%23", Row["iID_MaTaiKhoanDanhMucChiTiet"].ToString());
                    strXauCon = LayXauTaiKhoanDanhMucChiTiet_TaiKhoan(Path, XauHanhDong, XauSapXep, Convert.ToString(Row["iID_MaTaiKhoanDanhMucChiTiet"]), Cap + 1, ref ThuTu, dtGiaiThich, iTrangThai);

                    if (strXauCon != "")
                    {
                        strHanhDong += XauSapXep.Replace("%23%23", Row["iID_MaTaiKhoanDanhMucChiTiet"].ToString());
                    }
                    //strPG += string.Format("<tr " + classtr + ">");
                    strb.Append(string.Format("<tr " + classtr + ">"));

                    String check = "";
                    for (int j = 0; j < dtGiaiThich.Rows.Count; j++)
                    {
                        check = "";
                        String sKyHieu1 = Convert.ToString(dtGiaiThich.Rows[j]["sKyHieu"]);
                        if (Row["sKyHieu"].Equals(sKyHieu1))
                        {
                            check = "checked=\"checked\"";
                            break;
                        }
                    }
                    String TaiKhoan_GiaiThich =
                       DS_TaiKhoan_GiaiThich(Convert.ToString(Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                    if (Cap == 0)
                    {

                        //strb.Append(
                        //    string.Format(
                        //        "<td style=\"padding: 3px 3px;width:20px;\" align=\"center\"> <input type=\"checkbox\" id=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" name=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" value=\"{0}\" {1} check-group=\"iID_MaTaiKhoan\"/></td>",
                        //        Row["iID_MaTaiKhoanDanhMucChiTiet"], check));
                        strb.Append(
       string.Format(
           "<td style=\"padding: 3px 3px;width:20px;\" align=\"center\"> <input type=\"checkbox\" id=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" name=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" value=\"{0}\" {1} check-group=\"iID_MaTaiKhoan\"/> " +
           "<input type=\"hidden\" id=\"sKyHieu_{0}\" name=\"sKyHieu_{0}\" value=\"{2}\"/>" +
            "<input type=\"hidden\" id=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" name=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" value=\"{0}\"/>" +
           "<input type=\"hidden\" id=\"sTen_{0}\" name=\"sTen_{0}\" value=\"{3}\"/></td>", Row["iID_MaTaiKhoanDanhMucChiTiet"], check, Row["sKyHieu"], Row["sTen"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}<b>{1}</b></td>", "", tgThuTu));

                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sKyHieu"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}<b>{1}</b></td>", strDoanTrang, Row["sTen"]));
                        strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));
                      
                    }
                    else
                    {
                        if (tgThuTu % 2 == 0)
                        {

                            strb.Append(
                                string.Format(
                                    "<td style=\"padding: 3px 3px;width:20px;\" align=\"center\"> <input type=\"checkbox\" id=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" name=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" value=\"{0}\" {1} check-group=\"iID_MaTaiKhoan\"/> " +
                                    "<input type=\"hidden\" id=\"sKyHieu_{0}\" name=\"sKyHieu_{0}\" value=\"{2}\"/>" +
                                                "<input type=\"hidden\" id=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" name=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" value=\"{0}\"/>" +
                                    "<input type=\"hidden\" id=\"sTen_{0}\" name=\"sTen_{0}\" value=\"{3}\"/></td>", Row["iID_MaTaiKhoanDanhMucChiTiet"], check, Row["sKyHieu"], Row["sTen"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}{1}</td>", "", tgThuTu));
                            //strb.Append(string.Format("<td style=\"padding: 3px 3px;display:none;\">{0}{1}</td>", strDoanTrang, Row["iID_MaTaiKhoanDanhMucChiTiet"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));
                         
                        }
                        else
                        {

                            //strb.Append(
                            //    string.Format(
                            //        "<td style=\"padding: 3px 3px;width:20px;\" align=\"center\"> <input type=\"checkbox\" id=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" name=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" value=\"{0}\" {1} check-group=\"iID_MaTaiKhoan\"/></td>",
                            //        Row["iID_MaTaiKhoanDanhMucChiTiet"], check));
                            strb.Append(
       string.Format(
           "<td style=\"padding: 3px 3px;width:20px;\" align=\"center\"> <input type=\"checkbox\" id=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" name=\"Loai_iID_MaTaiKhoanDanhMucChiTiet\" value=\"{0}\" {1} check-group=\"iID_MaTaiKhoan\"/> " +
           "<input type=\"hidden\" id=\"sKyHieu_{0}\" name=\"sKyHieu_{0}\" value=\"{2}\"/>" +
                       "<input type=\"hidden\" id=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" name=\"iID_MaTaiKhoanDanhMucChiTiet_{0}\" value=\"{0}\"/>" +
           "<input type=\"hidden\" id=\"sTen_{0}\" name=\"sTen_{0}\" value=\"{3}\"/></td>", Row["iID_MaTaiKhoanDanhMucChiTiet"], check, Row["sKyHieu"], Row["sTen"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;width:20px;\" align=\"center\">{0}{1}</td>", "", tgThuTu));

                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sKyHieu"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}{1}</td>", strDoanTrang, Row["sTen"]));
                            strb.Append(string.Format("<td style=\"padding: 3px 3px;\">{0}</td>", TaiKhoan_GiaiThich));
                          
                        }
                    }
                 
                    strb.Append(string.Format("</tr>"));
                    strb.Append(strXauCon);
                }
                vR = strb.ToString();
            }
            dt.Dispose();
            return vR;
        }
        /// <summary>
        /// Lay danh sach tai khoan giai thich
        /// </summary>
        /// <param name="iID_MaTaiKhoanGiaiThich"></param>
        /// <returns></returns>
        public static String DS_TaiKhoan_GiaiThich(String iID_MaTaiKhoanDanhMucChiTiet)
        {
            String vR = "";
            DataTable dt;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "select iID_MaTaiKhoan from KT_TaiKhoanGiaiThich  WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet);
            dt = Connection.GetDataTable(cmd);
            if (dt!=null && dt.Rows.Count>0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vR += "," + dt.Rows[i]["iID_MaTaiKhoan"];
                }
            }
            if (dt != null) dt.Dispose();
            cmd.Dispose();
            if (vR.Length>1)
            {
                vR = vR.Substring(1, vR.Length - 1);
            }
            return vR;
        }
        public static Boolean CheckTaiKhoan(String iID_MaTaiKhoanGiaiThich)
        {
            Boolean vR = false;
            DataTable dt;
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT iID_MaTaiKhoanGiaiThich FROM KT_TaiKhoanGiaiThich WHERE iTrangThai=1 AND iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanDanhMucChiTiet";
            cmd.Parameters.AddWithValue("@iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanGiaiThich);
            dt = Connection.GetDataTable(cmd);
            if (dt.Rows.Count > 0) vR = true;
            if (dt != null) dt.Dispose();
            cmd.Dispose();
          
            return vR;
        }
        public static Boolean DeleteTaiKhoan(String iID_MaTaiKhoanGiaiThich)
        {
            Boolean vR = false;
            if (CheckTaiKhoan(iID_MaTaiKhoanGiaiThich)==false)
            {
                SqlCommand cmd;
                cmd =
                    new SqlCommand(
                        "UPDATE KT_TaiKhoanDanhMucChiTiet SET iTrangThai=1 WHERE iID_MaTaiKhoanDanhMucChiTiet=@iID_MaTaiKhoanGiaiThich");
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanGiaiThich", iID_MaTaiKhoanGiaiThich);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                cmd =
                    new SqlCommand(
                        "UPDATE KT_TaiKhoanDanhMucChiTiet SET iID_MaTaiKhoanDanhMucChiTiet_Cha=null,bLaHangCha=0  WHERE iID_MaTaiKhoanDanhMucChiTiet_Cha=@iID_MaTaiKhoanGiaiThich");
                cmd.Parameters.AddWithValue("@iID_MaTaiKhoanGiaiThich", iID_MaTaiKhoanGiaiThich);
                Connection.UpdateDatabase(cmd);
                cmd.Dispose();
                vR = true;
            }
            return vR;
        }
        public static String getiID_MaTaiKhoanDanhMucChiTiet(String sKyHieu)
        {
            String vR = "";
            SqlCommand cmd;
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT TOP 1 iID_MaTaiKhoanDanhMucChiTiet FROM KT_TaiKhoanDanhMucChiTiet WHERE iTrangThai=1 AND sKyHieu=@sKyHieu";
            cmd.Parameters.AddWithValue("@sKyHieu", sKyHieu);
            vR = Convert.ToString(Connection.GetValue(cmd, "0"));
            cmd.Dispose();
            return vR;
        }


        public static DataTable getstring(DataTable dtTemp, int Cap, ref int ThuTu, Boolean isKhoangTrang)
        {
            String strDoanTrang = "";
            DataTable dt = new DataTable();
            
           dt.Columns.Add("iSTT", typeof(String));
           dt.Columns.Add("iID_MaTaiKhoanDanhMucChiTiet", typeof(int));
           dt.Columns.Add("iID_MaTaiKhoanDanhMucChiTiet_Cha", typeof(String));
            dt.Columns.Add("sKyHieu", typeof(String));
            dt.Columns.Add("sTen", typeof(String));
            dt.Columns.Add("rSoTienNgoaiTe", typeof(decimal));
            dt.Columns.Add("sTenNgoaiTe", typeof(String));
            dt.Columns.Add("iID_MaNgoaiTe", typeof(int));
            dt.Columns.Add("bLaHangCha", typeof (int));
            dt.Columns.Add("sXauNoiMa", typeof(String));
            if (dtTemp.Rows.Count > 0 && dtTemp != null)
            {

                for (int i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "   ";
                }
                foreach (DataRow dr in dtTemp.Rows)
                {
                    String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(dr["iID_MaTaiKhoanDanhMucChiTiet_Cha"]);
                    if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet_Cha) == false && iID_MaTaiKhoanDanhMucChiTiet_Cha != "" && iID_MaTaiKhoanDanhMucChiTiet_Cha != "0")
                    {
                    }
                    else
                    {
                        ThuTu++;
                        DataRow drMain = dt.NewRow();
                        drMain["iSTT"] = ThuTu.ToString();
                        drMain["iID_MaTaiKhoanDanhMucChiTiet"] = Convert.ToInt32(dr["iID_MaTaiKhoanDanhMucChiTiet"]);
                        drMain["iID_MaTaiKhoanDanhMucChiTiet_Cha"] = iID_MaTaiKhoanDanhMucChiTiet_Cha;
                        drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                        if (isKhoangTrang == true)
                        {
                            drMain["sTen"] = strDoanTrang + Convert.ToString(dr["sTen"]);
                        }
                        else
                        {
                            drMain["sTen"] =  Convert.ToString(dr["sTen"]);
                        }
                        drMain["rSoTienNgoaiTe"] = Convert.ToDecimal(dr["rSoTienNgoaiTe"]);
                        drMain["sTenNgoaiTe"] = Convert.ToString(dr["sTenNgoaiTe"]);
                        drMain["iID_MaNgoaiTe"] = Convert.ToInt32(dr["iID_MaNgoaiTe"]);
                        if (Convert.ToBoolean(dr["bLaHangCha"])==true)
                        {
                            drMain["bLaHangCha"] = 1;
                        }
                        else
                        {
                            drMain["bLaHangCha"] = 0;
                        }
                        drMain["sXauNoiMa"] = Convert.ToString(dr["sXauNoiMa"]);
                        dt.Rows.Add(drMain);
                        addchildren(dtTemp, Int32.Parse(dr["iID_MaTaiKhoanDanhMucChiTiet"].ToString()), ref dt, Cap + 1,
                                   ref ThuTu, isKhoangTrang);

                    }
                }
                dtTemp.Dispose();
               
            }
           
            return dt;
        }
        private static void addchildren(DataTable dtTemp, int menuid, ref  DataTable dt, int Cap, ref int ThuTu, Boolean isKhoangTrang)
        {
            String strDoanTrang = "";
            for (int i = 1; i <= Cap; i++)
            {
                strDoanTrang += "   ";
            }
            foreach (DataRow dr1 in dtTemp.Rows)
            {
                if (dr1["iID_MaTaiKhoanDanhMucChiTiet_Cha"].ToString() == menuid.ToString())
                {
                    ThuTu++;
                    DataRow drMain = dt.NewRow();
                    drMain["iSTT"] = ThuTu.ToString();
                    drMain["iID_MaTaiKhoanDanhMucChiTiet"] = Convert.ToInt32(dr1["iID_MaTaiKhoanDanhMucChiTiet"]);
                    drMain["iID_MaTaiKhoanDanhMucChiTiet_Cha"] = Convert.ToString(dr1["iID_MaTaiKhoanDanhMucChiTiet_Cha"]);
                    drMain["sKyHieu"] = Convert.ToString(dr1["sKyHieu"]);
                   
                    if (isKhoangTrang == true)
                    {
                        drMain["sTen"] = strDoanTrang + Convert.ToString(dr1["sTen"]);
                    }
                    else
                    {
                        drMain["sTen"] = Convert.ToString(dr1["sTen"]);
                    }
                    drMain["rSoTienNgoaiTe"] = Convert.ToDecimal(dr1["rSoTienNgoaiTe"]);
                    drMain["sTenNgoaiTe"] = Convert.ToString(dr1["sTenNgoaiTe"]);
                    drMain["iID_MaNgoaiTe"] = Convert.ToInt32(dr1["iID_MaNgoaiTe"]);
                    if (Convert.ToBoolean(dr1["bLaHangCha"]) == true)
                    {
                        drMain["bLaHangCha"] = 1;
                    }
                    else
                    {
                        drMain["bLaHangCha"] = 0;
                    }
                    drMain["sXauNoiMa"] = Convert.ToString(dr1["sXauNoiMa"]);
                    dt.Rows.Add(drMain);
                    addchildren(dtTemp, Int32.Parse(dr1["iID_MaTaiKhoanDanhMucChiTiet"].ToString()), ref dt, Cap + 1, ref ThuTu, isKhoangTrang);
                }
                else
                {
                }
            }
           

        }
        public static DataTable DT_DS_NgoaiTe(Boolean ThemDongTieuDe = false, String sDongTieuDe = "--- Chọn tất cả ---")
        {
            DataTable vR = new DataTable();
            String SQL = "SELECT iID_MaNgoaiTe, sTen " +
                        "FROM QLDA_NgoaiTe " +
                        "WHERE iTrangThai = 1 ORDER BY sTen";
            SqlCommand cmd = new SqlCommand(SQL);
            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (ThemDongTieuDe)
            {
                DataRow R = vR.NewRow();
                R["iID_MaNgoaiTe"] = 0;
                R["sTen"] = sDongTieuDe;
                vR.Rows.InsertAt(R, 0);
            }
            return vR;
        }

        public static String getTen_NgoaiTe(int iID_MaNgoaiTe)
        {
            String vR = "";
            String SQL = "SELECT Ten " +
                        "FROM QLDA_NgoaiTe " +
                        "WHERE iTrangThai = 1 AND iID_MaNgoaiTe=@iID_MaNgoaiTe";
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", iID_MaNgoaiTe);
            vR = Connection.GetValueString(cmd, "");
            cmd.Dispose();
            return vR;
        }
    }

}