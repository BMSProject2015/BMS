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
    public class KeToanTongHopListModels
    {
        public KeToanTongHopListModels(String iID_MaChungTu, String MaND)
        {
            this.iID_MaChungTu = iID_MaChungTu;
            this.MaND = MaND;
        }
        public String iID_MaChungTu { get; set; }
        public String MaND { get; set; }
    }

    public class KeToanTongHopModels
    {
        public static int iID_MaPhanHe = PhanHeModels.iID_MaPhanHeKeToanTongHop;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="iNam"></param>
        /// <param name="TenTruongTaiKhoan"></param>
        /// <param name="CanDoiTaiKhoan">=true đưa hêt tài khoan trong danh mục/ false chỉ đưa tài khoản phát sinh</param>
        /// <returns></returns>
        public static DataTable DienTaiKhoanCha(DataTable dtData, String iNam, String TenTruongTaiKhoan, Boolean CanDoiTaiKhoan = false)
        {
            String SQL = String.Format("SELECT iID_MaTaiKhoan,sTen,bHienThi FROM KT_TaiKhoan WHERE iNam='{0}' ORDER By iID_MaTaiKhoan ", iNam);
            DataTable dtTK = Connection.GetDataTable(SQL);
            DataTable dtKQ = new DataTable();
            String TenTruong = "";
            dtKQ.Columns.Add("TK_Cha", typeof(String));
            dtKQ.Columns.Add("TK_Con", typeof(String));
            dtKQ.Columns.Add("bHienThi", typeof(Boolean));
            List<String> arrTruongTien = new List<String>();
            for (int c = 0; c < dtData.Columns.Count; c++)
            {
                TenTruong = dtData.Columns[c].ColumnName;
                dtKQ.Columns.Add(TenTruong, dtData.Columns[c].DataType);
                if (TenTruong.StartsWith("r"))
                {
                    arrTruongTien.Add(TenTruong);
                }

            }
            String TK = "", TK1 = "";
            // Điền tài khoản cha vào dtKQ
            int i = 0;
            DataRow R;
            int cs = 0;
            Boolean bHienThi = true; ;
            while (i < dtTK.Rows.Count)
            {
                TK = Convert.ToString(dtTK.Rows[i]["iID_MaTaiKhoan"]).Trim();
                bHienThi=Convert.ToBoolean(dtTK.Rows[i]["bHienThi"]);
                for (int j = cs; j < dtData.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtData.Rows[j][TenTruongTaiKhoan]).Trim();
                    if (TK1.StartsWith(TK))
                    {


                        if (TK1 == TK)
                        {

                            if (bHienThi == true && CanDoiTaiKhoan)
                            {
                                R = dtKQ.NewRow();
                                for (int c = 0; c < dtData.Columns.Count; c++)
                                {
                                    TenTruong = dtData.Columns[c].ColumnName;
                                    R[TenTruong] = dtData.Rows[j][TenTruong];
                                }
                                R["bHienThi"] = bHienThi;
                                dtKQ.Rows.Add(R);
                            }
                            else if(CanDoiTaiKhoan==false)
                            {
                                R = dtKQ.NewRow();
                                for (int c = 0; c < dtData.Columns.Count; c++)
                                {
                                    TenTruong = dtData.Columns[c].ColumnName;
                                    R[TenTruong] = dtData.Rows[j][TenTruong];
                                }
                                R["bHienThi"] = bHienThi;
                                dtKQ.Rows.Add(R);
                            }
                            //dtData.Rows.RemoveAt(j);
                            cs = j + 1;

                            break;
                        }
                        else
                        {
                            if (bHienThi== true && CanDoiTaiKhoan)
                            {
                                R = dtKQ.NewRow();
                                for (int c = 0; c < dtData.Columns.Count; c++)
                                {
                                    TenTruong = dtData.Columns[c].ColumnName;
                                    if (TenTruong.StartsWith("r"))
                                    {
                                        R[TenTruong] = 0;
                                    }
                                    else if (TenTruong == "sTen")
                                    {
                                        R[TenTruong] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(TK);//Lấy tên tài khoản cha
                                    }
                                    else
                                    {
                                        R[TenTruong] = dtData.Rows[j][TenTruong];
                                    }

                                }
                                R["bHienThi"] = bHienThi;
                                //gán lại trường tài khoản = tài khoản cha
                                R[TenTruongTaiKhoan] = TK;
                                dtKQ.Rows.Add(R);
                            }
                            else if (CanDoiTaiKhoan == false)
                            {
                                R = dtKQ.NewRow();
                                for (int c = 0; c < dtData.Columns.Count; c++)
                                {
                                    TenTruong = dtData.Columns[c].ColumnName;
                                    if (TenTruong.StartsWith("r"))
                                    {
                                        R[TenTruong] = 0;
                                    }
                                    else if (TenTruong == "sTen")
                                    {
                                        R[TenTruong] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(TK);//Lấy tên tài khoản cha
                                    }
                                    else
                                    {
                                        R[TenTruong] = dtData.Rows[j][TenTruong];
                                    }

                                }
                                R["bHienThi"] = bHienThi;
                                //gán lại trường tài khoản = tài khoản cha
                                R[TenTruongTaiKhoan] = TK;
                                dtKQ.Rows.Add(R);
                            }

                            break;
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(dtTK.Rows[i]["bHienThi"]) == true && CanDoiTaiKhoan)
                        {
                            R = dtKQ.NewRow();
                            for (int c = 0; c < dtData.Columns.Count; c++)
                            {
                                TenTruong = dtData.Columns[c].ColumnName;
                                if (TenTruong.StartsWith("r"))
                                {
                                    R[TenTruong] = 0;
                                }
                                else if (TenTruong.StartsWith("i"))
                                {
                                    R[TenTruong] = dtData.Rows[j][TenTruong];
                                }
                                if (TenTruong == "sTen")
                                {
                                    R[TenTruong] = TaiKhoanModels.LayTenTaiKhoanKhongGhepMa(TK);//Lấy tên tài khoản cha
                                }
                                else
                                {
                                    R[TenTruong] = dtData.Rows[j][TenTruong];
                                }
                            }
                            R["bHienThi"] = bHienThi;
                            //gán lại trường tài khoản = tài khoản cha
                            R[TenTruongTaiKhoan] = TK;
                            dtKQ.Rows.Add(R);
                        }                      
                        break;
                    }

                }
                if (CanDoiTaiKhoan && cs == dtData.Rows.Count - 1)
                {
                    if (Convert.ToBoolean(dtTK.Rows[i]["bHienThi"]) == true && CanDoiTaiKhoan)
                    {
                        R = dtKQ.NewRow();
                        for (int c = 0; c < dtData.Columns.Count; c++)
                        {
                            TenTruong = dtData.Columns[c].ColumnName;
                            if (TenTruong.StartsWith("i"))
                            {
                                R[TenTruong] = dtData.Rows[0][TenTruong];// lấy trường tháng năm
                            }
                        }
                        R["bHienThi"] = bHienThi;
                        //gán lại trường tài khoản = tài khoản cha
                        R[TenTruongTaiKhoan] = TK;
                        dtKQ.Rows.Add(R);
                    }
                    else if (CanDoiTaiKhoan == false)
                    {
                        R = dtKQ.NewRow();
                        for (int c = 0; c < dtData.Columns.Count; c++)
                        {
                            TenTruong = dtData.Columns[c].ColumnName;
                            if (TenTruong.StartsWith("i"))
                            {
                                R[TenTruong] = dtData.Rows[0][TenTruong];// lấy trường tháng năm
                            }
                        }
                        R["bHienThi"] = bHienThi;
                        //gán lại trường tài khoản = tài khoản cha
                        R[TenTruongTaiKhoan] = TK;
                        dtKQ.Rows.Add(R);
                    }

                }
                else
                {
                    if (cs == dtData.Rows.Count)
                    {
                        i = dtTK.Rows.Count;
                    }
                }
                i = i + 1;
            }
            //xác định lại tài khoản cha và điền chỉ số tài khoản cha
            Boolean CoTaiKhoanCon;
            for (i = 0; i < dtKQ.Rows.Count; i++)
            {
                CoTaiKhoanCon = false;
                TK = Convert.ToString(dtKQ.Rows[i][TenTruongTaiKhoan]);
                for (int j = i + 1; j < dtKQ.Rows.Count; j++)
                {
                    TK1 = Convert.ToString(dtKQ.Rows[j][TenTruongTaiKhoan]);
                    if (TK1.StartsWith(TK))
                    {
                        CoTaiKhoanCon = true;
                        dtKQ.Rows[j]["TK_Cha"] = i;
                        dtKQ.Rows[j]["TK_Con"] = 1;
                    }
                }
                //if (CoTaiKhoanCon==false && String.IsNullOrEmpty(Convert.ToString(dtKQ.Rows[i]["TK_Con"])))
                //{
                //    dtKQ.Rows[i]["TK_Cha"] = i;
                //}


            }
            //Tính tổng lên tài khoản cha
            Double TongCong = 0, Tong_Co = 0, Tong_No = 0;
            String CScha = "", CScha_Cu = "";
            DataRow[] arrR;
            for (int j = 0; j < arrTruongTien.Count; j++)
            {
                for (i = dtKQ.Rows.Count - 1; i >= 0; i--)
                {
                    CScha = Convert.ToString(dtKQ.Rows[i]["TK_Cha"]);
                    if (String.IsNullOrEmpty(CScha) == false)
                    {
                        arrR = dtKQ.Select("TK_Cha='" + CScha + "'");
                        for (int c = 0; c < arrR.Length; c++)
                        {
                            if (arrR[c][arrTruongTien[j]] != DBNull.Value)
                            {
                                TongCong = TongCong + Convert.ToDouble(arrR[c][arrTruongTien[j]]);
                            }
                            if (arrTruongTien[j].StartsWith("rCK_"))
                            {
                                if (arrR[c]["rCK_No"] != DBNull.Value)
                                {
                                    Tong_No = Tong_No + Convert.ToDouble(arrR[c]["rCK_No"]);
                                }
                                if (arrR[c]["rCK_Co"] != DBNull.Value)
                                {
                                    Tong_Co = Tong_Co + Convert.ToDouble(arrR[c]["rCK_Co"]);
                                }

                            }
                        }

                        if (arrTruongTien[j].StartsWith("rCK_"))
                        {
                            TongCong = Tong_No - Tong_Co;
                            if (TongCong > 0)
                            {
                                dtKQ.Rows[Convert.ToInt32(CScha)]["rCK_No"] = TongCong;
                                dtKQ.Rows[Convert.ToInt32(CScha)]["rCK_Co"] = 0;//tuannn thêm ngày 12/6/2012
                            }
                            else
                            {
                                dtKQ.Rows[Convert.ToInt32(CScha)]["rCK_Co"] = TongCong * (-1);
                                dtKQ.Rows[Convert.ToInt32(CScha)]["rCK_No"] = 0;//tuannn thêm ngày 12/6/2012
                            }
                        }
                        else
                        {
                            dtKQ.Rows[Convert.ToInt32(CScha)][arrTruongTien[j]] = TongCong;
                        }
                        TongCong = 0; Tong_Co = 0; Tong_No = 0;
                    }

                }
            }
            dtData.Dispose();
            dtTK.Dispose();
            return dtKQ;
        }
        /// <summary>
        /// Đưa ra danh sách tài khoản có phát sinh từ ngày đến ngày
        /// </summary>
        /// <param name="TuNgay">từ ngày số</param>
        /// <param name="DenNgay">Đến ngày số</param>
        /// <param name="Thang">Tháng số</param>
        /// <param name="Nam">năm</param>
        /// <returns></returns>
        public static DataTable Get_dtTaiKhoan(String TuNgay, String DenNgay, String Thang, String Nam)
        {
            String sTuNgay = Nam + "/" + Thang + "/" + TuNgay;
            String sDenNgay = Nam + "/" + Thang + "/" + DenNgay;
            String SQL = "SELECT iID_MaTaiKhoan_No as iID_MaTaiKhoan ,SUM(rPS_No) AS rPS_No,SUM(rPS_Co) AS rPS_Co";
            SQL += " FROM (";
            SQL += " SELECT ";
            SQL += " iID_MaTaiKhoan_No, iID_MaTaiKhoan_Co=iID_MaTaiKhoan_No,SUM(rSoTien) as rPS_No,rPS_Co=0.0";
            SQL += " FROM KT_ChungTuChiTiet";
            SQL += " WHERE iTrangThai=1 AND iThangCT >0 AND iNamLamViec={3} ";
            SQL += " AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= '{0}'";
            SQL += " AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) <= '{1}'";
            SQL += " AND iID_MaTrangThaiDuyet={2}";
            SQL += " GROUP BY iID_MaTaiKhoan_No";
            SQL += " UNION";
            SQL += " SELECT ";
            SQL += " iID_MaTaiKhoan_No=iID_MaTaiKhoan_Co,iID_MaTaiKhoan_Co,rPS_No=0.0,SUM(rSoTien) as rPS_Co";
            SQL += " FROM KT_ChungTuChiTiet";
            SQL += " WHERE iTrangThai=1 AND iThangCT > 0 AND iNamLamViec={3} ";
            SQL += " AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT) + '/' + CONVERT(varchar, iNgayCT), 111) >= '{0}'";
            SQL += " AND CONVERT(Datetime, CONVERT(varchar, iNamLamViec) + '/' + CONVERT(varchar, iThangCT)+ '/' + CONVERT(varchar, iNgayCT), 111) < '{1}'";
            SQL += " AND iID_MaTrangThaiDuyet={2}";
            SQL += " GROUP BY iID_MaTaiKhoan_Co";
            SQL += " ) KT";
            SQL += " GROUP BY iID_MaTaiKhoan_No,iID_MaTaiKhoan_Co";
            SQL += " Having (SUM(rPS_No)>0 OR SUM(rPS_co)>0) AND iID_MaTaiKhoan_No<>''";
            String strSQL=String.Format(SQL, sTuNgay, sDenNgay, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), Nam);
            DataTable dt = Connection.GetDataTable(strSQL);

            return dt;
        }

        public static DataTable Get_dtCanDoiThuChiTaiChinh(String iNam, String iQuy, String iLoaiThuChi, Dictionary<String, String> arrGiaTriTimKiem)
        {
            DataTable vR;
            String SQL, DK = "";
            SqlCommand cmd = new SqlCommand();
            if (arrGiaTriTimKiem != null)
            {
                if (String.IsNullOrEmpty(arrGiaTriTimKiem["TrangThai"]) == false)
                {
                    if (arrGiaTriTimKiem["TrangThai"] == "1")
                    {
                        DK += " AND bDongY=@TrangThai";
                        cmd.Parameters.AddWithValue("@TrangThai", true);
                    }
                    else
                    {
                        DK += " AND bDongY=@TrangThai";
                        cmd.Parameters.AddWithValue("@TrangThai", false);
                    }
                }
            }
            if (!String.IsNullOrEmpty(iNam))
            {
                DK += " AND iNam=@Nam";
                cmd.Parameters.AddWithValue("@Nam", iNam);
            }
            if (!String.IsNullOrEmpty(iQuy))
            {
                DK += " AND iQuy=@iQuy";
                cmd.Parameters.AddWithValue("@iQuy", iQuy);
            }
            if (!String.IsNullOrEmpty(iLoaiThuChi) && iLoaiThuChi!="0")
            {
                DK += " AND iLoaiThuChi=@iLoaiThuChi";
                cmd.Parameters.AddWithValue("@iLoaiThuChi", iLoaiThuChi);
            }
            SQL = String.Format("SELECT * FROM KT_CanDoiThuChiTaiChinh WHERE iTrangThai = 1 {0} ORDER BY iLoaiThuChi,iSTT", DK);
            cmd.CommandText = SQL;

            vR = Connection.GetDataTable(cmd);
            cmd.Dispose();

            //return vR;
            int ThuTu = 0;
            DataTable dt = GetData(vR, 0, ref ThuTu, false);
            vR.Dispose();
            return dt;

        }
        public static DataTable GetData(DataTable dtTemp, int Cap, ref int ThuTu, Boolean isKhoangTrang)
        {
            String strDoanTrang = "";
            DataTable dt = new DataTable();

            dt.Columns.Add("iSTT", typeof(String));
            dt.Columns.Add("iID_MaThuChi", typeof(int));
            dt.Columns.Add("iID_MaThuChi_Cha", typeof(int));
            dt.Columns.Add("bLaHangCha", typeof(String));
            dt.Columns.Add("iLoaiThuChi", typeof(int));
            dt.Columns.Add("iNam", typeof(int));
            dt.Columns.Add("sXauNoiMa", typeof(String));
            dt.Columns.Add("sKyHieu", typeof(String));
            dt.Columns.Add("sKyHieu_Cha", typeof(String));
            dt.Columns.Add("sNoiDung", typeof(String));

            dt.Columns.Add("sTenTaiKhoan_TienViet", typeof(String));
            dt.Columns.Add("bCoTKGT_TienViet", typeof(Boolean));
            dt.Columns.Add("sTenTaiKhoan_NgoaiTe", typeof(String));
            dt.Columns.Add("bCoTKGT_NgoaiTe", typeof(Boolean));

            dt.Columns.Add("sTenTaiKhoan_Tong", typeof(String));
            dt.Columns.Add("bCoTKGT_Tong", typeof(Boolean));
            dt.Columns.Add("bHienThi", typeof(Boolean));
            dt.Columns.Add("bTuDongTinh", typeof(Boolean));
            dt.Columns.Add("bTuTinhTong", typeof(Boolean));

            //dt.Columns.Add("iID_MaTaiKhoan_No_Thu", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_No_Thu", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_No_Thu", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_No_Thu", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_Co_Thu", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_Co_Thu", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_Co_Thu", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_Co_Thu", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_No_Thu_NgoaiTe", typeof(String));



            //dt.Columns.Add("sTenTaiKhoan_No_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_No_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_No_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_Co_Thu_NgoaiTe", typeof(String));

            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_Co_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_Co_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_Co_Thu_NgoaiTe", typeof(String));
            //dt.Columns.Add("sKyHieu_Chi", typeof(String));
            //dt.Columns.Add("sKyHieu_Chi_Cha", typeof(String));
            //dt.Columns.Add("sNoiDung_Chi", typeof(String));

            //dt.Columns.Add("iID_MaTaiKhoan_No_Chi", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_No_Chi", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_No_Chi", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_No_Chi", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_Co_Chi", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_Co_Chi", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_Co_Chi", typeof(String));


            //dt.Columns.Add("sTenTaiKhoanGiaiThich_Co_Chi", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_No_Chi_NgoaiTe", typeof(String));

            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_No_Chi_NgoaiTe", typeof(String));

            //dt.Columns.Add("sTenTaiKhoan_No_Chi_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_No_Chi_NgoaiTe", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoan_Co_Chi_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoan_Co_Chi_NgoaiTe", typeof(String));
            //dt.Columns.Add("iID_MaTaiKhoanGiaiThich_Co_Chi_NgoaiTe", typeof(String));
            //dt.Columns.Add("sTenTaiKhoanGiaiThich_Co_Chi_NgoaiTe", typeof(String));
            dt.Columns.Add("sID_MaNguoiDungTao", typeof(String));
            if (dtTemp.Rows.Count > 0 && dtTemp != null)
            {

                for (int i = 1; i <= Cap; i++)
                {
                    strDoanTrang += "   ";
                }
                foreach (DataRow dr in dtTemp.Rows)
                {
                    String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(dr["iID_MaThuChi_Cha"]);
                    if (String.IsNullOrEmpty(iID_MaTaiKhoanDanhMucChiTiet_Cha) == false && iID_MaTaiKhoanDanhMucChiTiet_Cha != "" && iID_MaTaiKhoanDanhMucChiTiet_Cha != "0")
                    {
                    }
                    else
                    {
                        ThuTu++;
                        DataRow drMain = dt.NewRow();
                        drMain["iSTT"] = ThuTu.ToString();
                        drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                        drMain["iID_MaThuChi_Cha"] = HamChung.ConvertToDouble((dr["iID_MaThuChi_Cha"]));
                        drMain["bLaHangCha"] = Convert.ToString(dr["bLaHangCha"]);
                        drMain["iLoaiThuChi"] = Convert.ToInt32(dr["iLoaiThuChi"]);
                        drMain["iNam"] = Convert.ToInt32(dr["iNam"]);
                        drMain["sXauNoiMa"] = Convert.ToString(dr["sXauNoiMa"]);
                        drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                        drMain["sKyHieu_Cha"] = Convert.ToString(dr["sKyHieu_Cha"]);
                        if (isKhoangTrang == true)
                        {
                            drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                        }
                        else
                        {
                            drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                        }

                        drMain["sTenTaiKhoan_TienViet"] = Convert.ToString(dr["sTenTaiKhoan_TienViet"]);
                        drMain["bCoTKGT_TienViet"] = Convert.ToBoolean(dr["bCoTKGT_TienViet"]);
                        drMain["sTenTaiKhoan_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]);
                        drMain["bCoTKGT_NgoaiTe"] = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);

                        drMain["sTenTaiKhoan_Tong"] = Convert.ToString(dr["sTenTaiKhoan_Tong"]);
                        drMain["bCoTKGT_Tong"] = Convert.ToString(dr["bCoTKGT_Tong"]);
                        drMain["bHienThi"] = Convert.ToBoolean(dr["bHienThi"]);

                        drMain["bTuDongTinh"] = Convert.ToBoolean(dr["bTuDongTinh"]);
                        drMain["bTuTinhTong"] = Convert.ToBoolean(dr["bTuTinhTong"]);

                        //drMain["iID_MaTaiKhoan_No_Thu"] = Convert.ToString(dr["iID_MaTaiKhoan_No_Thu"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_No_Thu"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_No_Thu"]);
                        
                        //drMain["sTenTaiKhoan_No_Thu"] = Convert.ToString(dr["sTenTaiKhoan_No_Thu"]);
                        //drMain["sTenTaiKhoanGiaiThich_No_Thu"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_No_Thu"]);
                        //drMain["iID_MaTaiKhoan_Co_Thu"] = Convert.ToString(dr["iID_MaTaiKhoan_Co_Thu"]);
                        
                        //drMain["iID_MaTaiKhoanGiaiThich_Co_Thu"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_Co_Thu"]);
                        //drMain["sTenTaiKhoan_Co_Thu"] = Convert.ToString(dr["sTenTaiKhoan_Co_Thu"]);
                        
                        //drMain["sTenTaiKhoanGiaiThich_Co_Thu"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_Co_Thu"]);
                        
                        //drMain["iID_MaTaiKhoan_No_Thu_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoan_No_Thu_NgoaiTe"]);
                  
                       
                        //drMain["sTenTaiKhoan_No_Thu_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_No_Thu_NgoaiTe"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_No_Thu_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_No_Thu_NgoaiTe"]);
                        //drMain["sTenTaiKhoanGiaiThich_No_Thu_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_No_Thu_NgoaiTe"]);
                        //drMain["iID_MaTaiKhoan_Co_Thu_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoan_Co_Thu_NgoaiTe"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_Co_Thu_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_Co_Thu_NgoaiTe"]);
                        //drMain["sTenTaiKhoan_Co_Thu_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_Co_Thu_NgoaiTe"]);
                        //drMain["sTenTaiKhoanGiaiThich_Co_Thu_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_Co_Thu_NgoaiTe"]);
                        //drMain["sKyHieu_Chi"] = Convert.ToString(dr["sKyHieu_Chi"]);
                        //drMain["sKyHieu_Chi_Cha"] = Convert.ToString(dr["sKyHieu_Chi_Cha"]);

                      
                        //if (isKhoangTrang == true)
                        //{
                        //    drMain["sNoiDung_Chi"] = strDoanTrang + Convert.ToString(dr["sNoiDung_Chi"]);
                        //}
                        //else
                        //{
                        //    drMain["sNoiDung_Chi"] = Convert.ToString(dr["sNoiDung_Chi"]);
                        //}
                        //drMain["iID_MaTaiKhoan_No_Chi"] = Convert.ToString(dr["iID_MaTaiKhoan_No_Chi"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_No_Chi"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_No_Chi"]);
                        //drMain["sTenTaiKhoan_No_Chi"] = Convert.ToString(dr["sTenTaiKhoan_No_Chi"]);
                        //drMain["sTenTaiKhoanGiaiThich_No_Chi"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_No_Chi"]);
                        //drMain["iID_MaTaiKhoan_Co_Chi"] = Convert.ToString(dr["iID_MaTaiKhoan_Co_Chi"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_Co_Chi"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_Co_Chi"]);
                        //drMain["sTenTaiKhoan_Co_Chi"] = Convert.ToString(dr["sTenTaiKhoan_Co_Chi"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_No_Chi_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_No_Chi_NgoaiTe"]);

                        //drMain["sTenTaiKhoanGiaiThich_Co_Chi"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_Co_Chi"]);
                        //drMain["iID_MaTaiKhoan_No_Chi_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoan_No_Chi_NgoaiTe"]);
                        //drMain["sTenTaiKhoan_No_Chi_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_No_Chi_NgoaiTe"]);
                        //drMain["sTenTaiKhoanGiaiThich_No_Chi_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_No_Chi_NgoaiTe"]);
                        //drMain["iID_MaTaiKhoan_Co_Chi_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoan_Co_Chi_NgoaiTe"]);
                        //drMain["sTenTaiKhoan_Co_Chi_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_Co_Chi_NgoaiTe"]);
                        //drMain["iID_MaTaiKhoanGiaiThich_Co_Chi_NgoaiTe"] = Convert.ToString(dr["iID_MaTaiKhoanGiaiThich_Co_Chi_NgoaiTe"]);
                        //drMain["sTenTaiKhoanGiaiThich_Co_Chi_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoanGiaiThich_Co_Chi_NgoaiTe"]);
                        drMain["sID_MaNguoiDungTao"] = Convert.ToString(dr["sID_MaNguoiDungTao"]);
                    
                        dt.Rows.Add(drMain);
                        addchildren(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap + 1,
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
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["iID_MaThuChi_Cha"].ToString() == menuid.ToString())
                {
                    ThuTu++;
                    DataRow drMain = dt.NewRow();
                    drMain["iSTT"] = ThuTu.ToString();
                    drMain["iID_MaThuChi"] = Convert.ToInt32(dr["iID_MaThuChi"]);

                    drMain["iID_MaThuChi_Cha"] = HamChung.ConvertToDouble((dr["iID_MaThuChi_Cha"]));
                    drMain["bLaHangCha"] = Convert.ToString(dr["bLaHangCha"]);
                    drMain["iLoaiThuChi"] = Convert.ToInt32(dr["iLoaiThuChi"]);
                    drMain["iNam"] = Convert.ToInt32(dr["iNam"]);
                    drMain["sXauNoiMa"] = Convert.ToString(dr["sXauNoiMa"]);
                    drMain["sKyHieu"] = Convert.ToString(dr["sKyHieu"]);
                    drMain["sKyHieu_Cha"] = Convert.ToString(dr["sKyHieu_Cha"]);
                    if (isKhoangTrang == true)
                    {
                        drMain["sNoiDung"] = strDoanTrang + Convert.ToString(dr["sNoiDung"]);
                    }
                    else
                    {
                        drMain["sNoiDung"] = Convert.ToString(dr["sNoiDung"]);
                    }

                    drMain["sTenTaiKhoan_TienViet"] = Convert.ToString(dr["sTenTaiKhoan_TienViet"]);
                    drMain["bCoTKGT_TienViet"] = Convert.ToBoolean(dr["bCoTKGT_TienViet"]);
                    drMain["sTenTaiKhoan_NgoaiTe"] = Convert.ToString(dr["sTenTaiKhoan_NgoaiTe"]);
                    drMain["bCoTKGT_NgoaiTe"] = Convert.ToBoolean(dr["bCoTKGT_NgoaiTe"]);
                    drMain["sTenTaiKhoan_Tong"] = Convert.ToString(dr["sTenTaiKhoan_Tong"]);
                    drMain["bCoTKGT_Tong"] = Convert.ToString(dr["bCoTKGT_Tong"]);
                    drMain["bHienThi"] = Convert.ToBoolean(dr["bHienThi"]);

                    drMain["bTuDongTinh"] = Convert.ToBoolean(dr["bTuDongTinh"]);
                    drMain["bTuTinhTong"] = Convert.ToBoolean(dr["bTuTinhTong"]);
                    drMain["sID_MaNguoiDungTao"] = Convert.ToString(dr["sID_MaNguoiDungTao"]);
                    dt.Rows.Add(drMain);
                    addchildren(dtTemp, Int32.Parse(dr["iID_MaThuChi"].ToString()), ref dt, Cap + 1, ref ThuTu, isKhoangTrang);
                }
                else
                {
                }
            }


        }
    }
}