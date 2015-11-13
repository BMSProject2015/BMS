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
using System.Text;
using FlexCel.Core;
using FlexCel.Report;
using FlexCel.Render;
using FlexCel.XlsAdapter;
using VIETTEL.Controllers;
using System.IO;

namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_01_CPController : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_01_CP.xls";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01_CP.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        public ActionResult EditSubmit(String ParentID)
        {
            String iID_MaDotCapPhat = Convert.ToString(Request.Form[ParentID + "_iID_MaDotCapPhat"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            ViewData["iID_MaDotCapPhat"] = iID_MaDotCapPhat;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01_CP.aspx";
            ViewData["MaTien"] = MaTien;
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dt_rptQLDA_01_CP(String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            String NamLamViec = "2000";
            DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
            if (dtCauHinh.Rows.Count > 0)
            {
                NamLamViec = dtCauHinh.Rows[0]["iNamLamViec"].ToString();
            }
            dtCauHinh.Dispose();
            #region dtcapphat đợt trước
            
            String DKNgoaiTe_ThanhToan_Truoc = "";
            String DKNgoaiTe_TamUngChuaThuHoi_Truoc = "";
            String DKLoaiNgoaiTe = "";
            String DKNgoaiTe_HopDong = "";
            String DKLoaiNgoaiTe_HopDong = "";
            //VND
            if (MaTien == "0")
            {
                DKNgoaiTe_ThanhToan_Truoc = "SUM(rDeNghiPheDuyetThanhToan)/1000000";
                DKNgoaiTe_TamUngChuaThuHoi_Truoc = "(SUM(rDeNghiPheDuyetTamUng)-SUM(rDeNghiPheDuyetThuKhac))/1000000";
                DKNgoaiTe_HopDong = "rSoTien/1000000";
            }
            else
            {
                DKNgoaiTe_ThanhToan_Truoc = "SUM(rNgoaiTe_DeNghiPheDuyetThanhToan)";
                DKNgoaiTe_TamUngChuaThuHoi_Truoc = "SUM(rNgoaiTe_DeNghiPheDuyetTamUng)-SUM(rNgoaiTe_DeNghiPheDuyetThuKhac)";
                DKLoaiNgoaiTe = "(iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe OR iID_MaNgoaiTe_DeNghiPheDuyetThuKhac=@iID_MaNgoaiTe) AND";
                DKNgoaiTe_HopDong = "rNgoaiTe_SoTien";
                DKLoaiNgoaiTe_HopDong = "iID_MaNgoaiTe_SoTien=@iID_MaNgoaiTe AND ";
            }
            String SQLTruoc = String.Format(@"SELECT a.iID_MaHopDong,a.iID_MaDanhMucDuAn,
	                                           sSoHopDong,
	                                           sTenDuAn,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
                                               NguonNS,sLNS,
	                                           iID_MaDonViThiCong,
	                                           SUBSTRING(sTenDonViThiCong,5,100000) as sTenDonViThiCong,
	                                           rSoTienHopDong,
	                                           rDaThanhToan,
	                                           rTamUngChuaThuHoi
                                        FROM(
                                        SELECT iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDonViThiCong,sTenDonViThiCong,SUM({0}) as rSoTienHopDong
                                        FROM QLDA_HopDongChiTiet
                                        WHERE iNamLamViec<=@iNamLamViec AND {1} iTrangThai=1
                                        GROUP BY iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDonViThiCong,sTenDonViThiCong
                                        ) as a
                                        RIGHT JOIN (
                                        SELECT iID_MaHopDong,
                                               iID_MaDanhMucDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
                                               {2} as rDaThanhToan,
                                               {3} as rTamUngChuaThuHoi				    
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
											  AND iNamLamViec<=@iNamLamViec
											  AND {4} iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat
																        FROM QLDA_CapPhat_Dot
																       WHERE dNgayLap<@dNgay)
                                        GROUP BY iID_MaHopDong,
												  iID_MaDanhMucDuAn,
	                                           SUBSTRING(sLNS,1,1),sLNS
                                        HAVING ABS({2})>=0.5 OR ABS({3})>=0.5
                                        ) as b
                                        ON a.iID_MaHopDong=b.iID_MaHopDong 
                                        INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as d
                                        ON b.iID_MaDanhMucDuAn=d.iID_MaDanhMucDuAn
                                        LEFT JOIN (SELECT iID_MaHopDong,sSoHopDong FROM QLDA_HopDong WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec) as e
                                        ON a.iID_MaHopDong=e.iID_MaHopDong", DKNgoaiTe_HopDong,DKLoaiNgoaiTe_HopDong,DKNgoaiTe_ThanhToan_Truoc,DKNgoaiTe_TamUngChuaThuHoi_Truoc,DKLoaiNgoaiTe);
            SqlCommand cmdTruoc = new SqlCommand(SQLTruoc);
            cmdTruoc.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmdTruoc.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmdTruoc.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dtTruoc = Connection.GetDataTable(cmdTruoc);
            cmdTruoc.Dispose();
            #endregion

            #region dtcapphat đợt được chọn

            String DKNgoaiTe_ChuDauTuTamUng = "";
            String DKNgoaiTe_ChuDauTuThanhToan = "";
            String DKNgoaiTe_ChuDauTuThuTamUng = "";
            String DKNgoaiTe_DeNghiPheDuyetTamUng = "";
            String DKNgoaiTe_DeNghiPheDuyetThanhToan = "";
            String DKNgoaiTe_DeNghiPheDuyetThuTamUng = "";
            String DKNgoaiTe_DeNghiPheDuyetThuKhac = "";
            String DKLoaiNgoaiTe_DotNay = "";
            //VND
            if (MaTien == "0")
            {
                DKNgoaiTe_ChuDauTuTamUng = "SUM(rChuDauTuTamUng)/1000000";
                DKNgoaiTe_ChuDauTuThanhToan = "SUM(rChuDauTuThanhToan)/1000000";
                DKNgoaiTe_ChuDauTuThuTamUng = "SUM(rChuDauTuThuTamUng)/1000000";
                DKNgoaiTe_DeNghiPheDuyetTamUng = "SUM(rDeNghiPheDuyetTamUng)/1000000";
                DKNgoaiTe_DeNghiPheDuyetThanhToan = "SUM(rDeNghiPheDuyetThanhToan)/1000000";
                DKNgoaiTe_DeNghiPheDuyetThuTamUng = "SUM(rDeNghiPheDuyetThuTamUng)/1000000";
                DKNgoaiTe_DeNghiPheDuyetThuKhac = "SUM(rDeNghiPheDuyetThuKhac)/1000000";
            }
            else
            {
                DKNgoaiTe_ChuDauTuTamUng = "SUM(rNgoaiTe_ChuDauTuTamUng)";
                DKNgoaiTe_ChuDauTuThanhToan = "SUM(rNgoaiTe_ChuDauTuThanhToan)";
                DKNgoaiTe_ChuDauTuThuTamUng = "SUM(rNgoaiTe_ChuDauTuThuTamUng)";
                DKNgoaiTe_DeNghiPheDuyetTamUng = "SUM(rNgoaiTe_DeNghiPheDuyetTamUng)";
                DKNgoaiTe_DeNghiPheDuyetThanhToan = "SUM(rNgoaiTe_DeNghiPheDuyetThanhToan)";
                DKNgoaiTe_DeNghiPheDuyetThuTamUng = "SUM(rNgoaiTe_DeNghiPheDuyetThuTamUng)";
                DKNgoaiTe_DeNghiPheDuyetThuKhac = "SUM(rNgoaiTe_DeNghiPheDuyetThuKhac)";
                DKLoaiNgoaiTe_DotNay = @"(iID_MaNgoaiTe_ChuDauTuTamUng=@iID_MaNgoaiTe 
                                    OR iID_MaNgoaiTe_ChuDauTuThanhToan=@iID_MaNgoaiTe
                                    OR iID_MaNgoaiTe_ChuDauTuThuTamUng=@iID_MaNgoaiTe
                                    OR iID_MaNgoaiTe_DeNghiPheDuyetTamUng=@iID_MaNgoaiTe
                                    OR iID_MaNgoaiTe_DeNghiPheDuyetThanhToan=@iID_MaNgoaiTe
                                    OR iID_MaNgoaiTe_DeNghiPheDuyetThuTamUng=@iID_MaNgoaiTe
                                    OR iID_MaNgoaiTe_DeNghiPheDuyetThuKhac=@iID_MaNgoaiTe
                                    ) AND";
            }
            String SQL = String.Format(@"SELECT a.iID_MaHopDong,a.iID_MaDanhMucDuAn,
	                                           sSoHopDong,
	                                           sTenDuAn,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
                                               NguonNS,sLNS,
	                                           iID_MaDonViThiCong,
	                                            SUBSTRING(sTenDonViThiCong,8,100000) as sTenDonViThiCong,
	                                           rSoTienHopDong,
	                                           rChuDauTuTamUng,
	                                           rChuDauTuThanhToan,
                                               rChuDauTuThuTamUng,
                                               rDeNghiPheDuyetTamUng,
                                               rDeNghiPheDuyetThanhToan,
                                               rDeNghiPheDuyetThuTamUng,
                                               rDeNghiPheDuyetThuKhac
                                        FROM(
                                        SELECT iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDonViThiCong,sTenDonViThiCong,SUM({0}) as rSoTienHopDong
                                        FROM QLDA_HopDongChiTiet
                                        WHERE iNamLamViec=@iNamLamViec  AND {1} iTrangThai=1
                                        GROUP BY iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDonViThiCong,sTenDonViThiCong
                                        ) as a
                                        RIGHT JOIN (
                                        SELECT iID_MaHopDong,
                                               iID_MaDanhMucDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,sLNS,
                                               {2} as rChuDauTuTamUng,
                                               {3} as rChuDauTuThanhToan,
                                               {4} as rChuDauTuThuTamUng,
                                               {5} as rDeNghiPheDuyetTamUng,
                                               {6} as rDeNghiPheDuyetThanhToan,
                                               {7} as rDeNghiPheDuyetThuTamUng,
                                               {8} as rDeNghiPheDuyetThuKhac			    
                                        FROM QLDA_CapPhat
                                        WHERE iTrangThai=1 
											  AND iNamLamViec<=@iNamLamViec
											  AND {9} iID_MaDotCapPhat IN(SELECT iID_MaDotCapPhat
																        FROM QLDA_CapPhat_Dot
																       WHERE dNgayLap=@dNgay)
                                        GROUP BY iID_MaHopDong,
												  iID_MaDanhMucDuAn,
	                                           SUBSTRING(sLNS,1,1),sLNS
                                        HAVING ABS({2})>=0.5 OR ABS({3})>=0.5 OR ABS({4})>=0.5 OR ABS({5})>=0.5 OR ABS({6})>=0.5 OR ABS({7})>=0.5 OR ABS({8})>=0.5
                                        ) as b
                                        ON a.iID_MaHopDong=b.iID_MaHopDong AND a.iID_MaDanhMucDuAn=b.iID_MaDanhMucDuAn
                                        INNER JOIN (SELECT iID_MaDanhMucDuAn,sTenDuAn,sDeAn,sDuAn,sDuAnThanhPhan,
                                                                                       sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet 
                                                                                FROM QLDA_DanhMucDuAn 
                                                                                WHERE iTrangThai=1 AND sHangMucChiTiet<>'') as d
                                        ON b.iID_MaDanhMucDuAn=d.iID_MaDanhMucDuAn
                                        LEFT JOIN (SELECT iID_MaHopDong,sSoHopDong FROM QLDA_HopDong WHERE iTrangThai=1 AND iNamLamViec<=@iNamLamViec) as e
                                        ON a.iID_MaHopDong=e.iID_MaHopDong", DKNgoaiTe_HopDong, DKLoaiNgoaiTe_HopDong, DKNgoaiTe_ChuDauTuTamUng, DKNgoaiTe_ChuDauTuThanhToan, DKNgoaiTe_ChuDauTuThuTamUng, DKNgoaiTe_DeNghiPheDuyetTamUng, DKNgoaiTe_DeNghiPheDuyetThanhToan, DKNgoaiTe_DeNghiPheDuyetThuTamUng, DKNgoaiTe_DeNghiPheDuyetThuKhac, DKLoaiNgoaiTe_DotNay);
            SqlCommand cmd = new SqlCommand(SQL);
            cmd.Parameters.AddWithValue("@iNamLamViec", NamLamViec);
            cmd.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgayLap));
            if (MaTien != "0")
            {
                cmd.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            #endregion
            //ghep 2 dt
            #region  //Ghép dt với dtTruoc
            DataRow addR, R2;
            String sCol = "iID_MaHopDong,iID_MaDanhMucDuAn,iID_MaDonViThiCong,";
            sCol += "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,rSoTienHopDong,sSoHopDong,sTenDonViThiCong,NguonNS,sLNS,sTenDuAn,rDaThanhToan,rTamUngChuaThuHoi";
            String[] arrCol = sCol.Split(',');
            dt.Columns.Add("rDaThanhToan", typeof(Decimal));
            dt.Columns.Add("rTamUngChuaThuHoi", typeof(Decimal));
            for (int i = 0; i < dtTruoc.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"iID_MaHopDong='{0}' AND iID_MaDanhMucDuAn='{1}' AND iID_MaDonViThiCong='{2}'
                                                    AND sDeAn='{3}' AND sDuAn='{4}' AND sDuAnThanhPhan='{5}' AND sCongTrinh='{6}' AND sHangMucCongTrinh='{7}' AND sHangMucChiTiet='{8}' AND rSoTienHopDong='{9}' AND NguonNS='{10}' AND sLNS='{11}'",
                                                  dtTruoc.Rows[i]["iID_MaHopDong"], dtTruoc.Rows[i]["iID_MaDanhMucDuAn"], dtTruoc.Rows[i]["iID_MaDonViThiCong"], dtTruoc.Rows[i]["sDeAn"], dtTruoc.Rows[i]["sDuAn"],
                                                  dtTruoc.Rows[i]["sDuAnThanhPhan"], dtTruoc.Rows[i]["sCongTrinh"], dtTruoc.Rows[i]["sHangMucCongTrinh"], dtTruoc.Rows[i]["sHangMucChiTiet"], dtTruoc.Rows[i]["rSoTienHopDong"], dtTruoc.Rows[i]["NguonNS"], dtTruoc.Rows[i]["sLNS"]);
                DataRow[] R = dt.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dtTruoc.Rows[i][arrCol[j]];
                    }
                    dt.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dtTruoc.Rows)
                    {

                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt.Rows[j];

                            for (int c = 0; c < arrCol.Length - 2; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt.Rows[j]["rDaThanhToan"] = R1["rDaThanhToan"];
                                dt.Rows[j]["rTamUngChuaThuHoi"] = R1["rTamUngChuaThuHoi"];
                                break;
                            }

                        }
                    }

                }

            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt.DefaultView;
            dv.Sort = "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sSoHopDong";
            dt = dv.ToTable();
            #endregion
            return dt;
        }
        private void LoadData(FlexCelReport fr, String iID_MaDotCapPhat, String MaND,String MaTien)
        {

            DataTable data = dt_rptQLDA_01_CP(iID_MaDotCapPhat, MaND, MaTien);
            data.TableName = "ChiTiet";
            fr.AddTable("ChiTiet", data);
            data.Columns.Add("sTienDo", typeof(String));
            //Hạng mục chi tiết
            DataTable dtHangMucChiTiet = HamChung.SelectDistinct_QLDA("HMChiTiet", data, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,sTienDo");
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", dtHangMucChiTiet, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sLNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sLNS,sDeAn,sDuAn", "NguonNS,sLNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sLNS,sDeAn", "NguonNS,sLNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //sLNS
            DataTable dtLNS = HamChung.SelectDistinct("sLNS", dtDeAn, "NguonNS,sLNS", "NguonNS,sLNS,sTenDuAn", "", "");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");

            //Thêm tên Loại ngân sách của dtLNS
            for (int i = 0; i < dtLNS.Rows.Count; i++)
            {
                String sLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                DataRow r = dtLNS.Rows[i];
                String SQL = "SELECT sMoTa FROM NS_MucLucNganSach WHERE iTrangThai=1 AND sL='' AND sLNS=@sLNS";
                SqlCommand cmd = new SqlCommand(SQL);
                cmd.Parameters.AddWithValue("@sLNS", sLNS);
                String sMoTa = "";
                sMoTa = Connection.GetValueString(cmd, "");
                r["sTenDuAn"] = sMoTa;
            }

            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMChiTiet", dtHangMucChiTiet);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("sLNS", dtLNS);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        public ActionResult ViewPDF(String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            HamChung.Language();
            String DuongDanFile = sFilePath;

            ExcelFile xls = CreateReport(Server.MapPath(DuongDanFile), iID_MaDotCapPhat, MaND, MaTien);
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
        public ExcelFile CreateReport(String path, String iID_MaDotCapPhat, String MaND,String MaTien)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            String Ngay = "";
            Ngay = "Ngày " + dNgayLap.Substring(0, 2) + " tháng " + dNgayLap.Substring(3, 2) + " năm " + dNgayLap.Substring(6, 4);
            DataTable dtDVT=QLDA_ReportModel.dt_LoaiTien_CP(dNgayLap,MaND);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count;i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                }

            }
            dtDVT.Dispose();
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_01_CP");
            LoadData(fr, iID_MaDotCapPhat, MaND,MaTien);
            fr.SetValue("Ngay", Ngay);
             fr.SetValue("DVT", DVT);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        public JsonResult ds_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            return Json(obj_QLDA(ParentID, iID_MaDotCapPhat, MaND, MaTien), JsonRequestBehavior.AllowGet);
        }
        public String obj_QLDA(String ParentID, String iID_MaDotCapPhat, String MaND, String MaTien)
        {
            String dNgayLap = "01/01/2000";
            DataTable dtDotCapPhat = QLDA_ReportModel.dt_DotCapPhat(MaND);
            for (int i = 0; i < dtDotCapPhat.Rows.Count; i++)
            {
                if (iID_MaDotCapPhat == dtDotCapPhat.Rows[i]["iID_MaDotCapPhat"].ToString())
                {
                    dNgayLap = dtDotCapPhat.Rows[i]["dNgayCapPhat"].ToString();
                    break;
                }
            }
            dtDotCapPhat.Dispose();
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien_CP(dNgayLap, MaND);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            return NgoaiTe;
        }
    }
}
