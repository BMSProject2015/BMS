using System;
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
using DomainModel.Abstract;
using DomainModel.Controls;
using VIETTEL.Models;
using VIETTEL.Controllers;
using System.IO;
using System.Text;
namespace VIETTEL.Report_Controllers.QLDA
{
    public class rptQLDA_ChiTiet_DanhMucCongTrinh_1Controller : Controller
    {
        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_1.xls";
        private const String sFilePath_HMCT = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_HMCT_1.xls";
        private const String sFilePath_CT = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_CT_1.xls";
        private const String sFilePath_DATP = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_DATP_1.xls";
        private const String sFilePath_DuAn = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_DuAn_1.xls";
        private const String sFilePath_DeAn = "/Report_ExcelFrom/QLDA/rptQLDA_BAOCAO_CT_DM_CONGTRINH_DeAn_1.xls";
        String NgayPheDuyet_DauTu = "", NgayPheDuyet_DuToan = "";
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ChiTiet_DanhMucCongTrinh_1.aspx";
            ViewData["PageLoad"] = "0";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// EditSubmit
        /// </summary>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public ActionResult EditSubmit(String ParentID)
        {
            String sSoPheDuyetDauTu = Convert.ToString(Request.Form[ParentID + "_sSoDauTu"]);
            String sSoPheDuyetDuToan = Convert.ToString(Request.Form[ParentID + "_sSoDuToan"]);
            String sDeAn = Convert.ToString(Request.Form["sDeAn"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
            ViewData["sSoPheDuyetDauTu"] = sSoPheDuyetDauTu;
            ViewData["sSoPheDuyetDuToan"] = sSoPheDuyetDuToan;
            ViewData["sDeAn"] = sDeAn;
            ViewData["MaTien"] = MaTien;
            ViewData["iCapTongHop"] = iCapTongHop;
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_ChiTiet_DanhMucCongTrinh_1.aspx";
            ViewData["PageLoad"] = "1";
            return View(sViewPath + "ReportView.aspx");
        }
        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Lấy dữ liệu"
        public DataTable ChiTiet_DMCT(String MaTien, String sDeAn,String sSoPheDuyetDauTu,String sSoPheDuyetDuToan)
        {
            DataTable dt_DuAn;
            DataTable dt_DuToan;
            String DKNgoaiTe = "";
            String DKLoaiNgoaiTe = "";
            //VND
            if (MaTien == "0")
            {
                DKNgoaiTe = "rSoTien/1000000";
            }
            else
            {
                DKNgoaiTe = "rNgoaiTe";
                DKLoaiNgoaiTe = " iID_MaNgoaiTe=@iID_MaNgoaiTe AND";
            }

            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR ";
            }
            //dau tu
            if (sSoPheDuyetDauTu == "TatCa")
            {

                #region tạo dt dự án
                String SQL_DuAn = String.Format(@" SELECT * FROM(
                                               SELECT DISTINCT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS
                                               FROM QLDA_TongDauTu
                                               WHERE iTrangThai=1 
                                                     AND ABS({1})<>0
                                                     AND ({0})
                                                     AND {2} iID_MaLoaiDieuChinh=1
                                                     AND (sM IN(9200,9250,9300,9350)
                                                     OR (sM=9400)
                                                     OR (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01))
                                                    ) as a
                                               INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM
                                            ", DKDeAn, DKNgoaiTe, DKLoaiNgoaiTe);

                SqlCommand cmd_DuAn = new SqlCommand(SQL_DuAn);
                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuAn.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuAn.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                dt_DuAn = Connection.GetDataTable(cmd_DuAn);
                cmd_DuAn.Dispose();
                #endregion
                #region tạo dt dự án điều chỉnh
                String SQL_DuAn_DieuChinh = String.Format(@"SELECT iID_MaDanhMucDuAn,sTenDuAn,iID_MaLoaiDieuChinh,iID_MaMucLucNganSach, 
                                                        DAUTU_9200=SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END),
                                                        DAUTU_9250=SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END),
                                                        DAUTU_9300=SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END),
                                                        DAUTU_9350=SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END),
                                                        DAUTU_9400=SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END),
                                                        DAUTU_9400_1=SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END),
                                                        dNgayPheDuyet,
                                                         sSoPheDuyet as sSoPheDuyet_DuAn,
                                                         CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuAn  
                                                        FROM QLDA_TongDauTu
                                                        WHERE iTrangThai=1
                                                         AND ({0})
                                                         AND {2} iID_MaLoaiDieuChinh <>0 
                                                         GROUP BY iID_MaDanhMucDuAn,sTenDuAn,iID_MaLoaiDieuChinh, dNgayPheDuyet,sSoPheDuyet,iID_MaMucLucNganSach
                                                        HAVING ABS(SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END))<>0
	                                                          OR ABS(SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END))<>0
	                                                          OR ABS(SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END))<>0 
	                                                          OR ABS(SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END))<>0
	                                                          OR ABS(SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END))<>0
	                                                          OR ABS(SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END))>=0.5
                                                        ORDER BY dNgayPheDuyet DESC", DKDeAn, DKNgoaiTe, DKLoaiNgoaiTe);
                SqlCommand cmd_DuAn_DieuChinh = new SqlCommand(SQL_DuAn_DieuChinh);
                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuAn_DieuChinh.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuAn_DieuChinh.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                DataTable dt_DuAn_DieuChinh = Connection.GetDataTable(cmd_DuAn_DieuChinh);
                cmd_DuAn_DieuChinh.Dispose();
                dt_DuAn.Columns.Add("DAUTU_9200", typeof(Decimal));
                dt_DuAn.Columns.Add("DAUTU_9250", typeof(Decimal));
                dt_DuAn.Columns.Add("DAUTU_9300", typeof(Decimal));
                dt_DuAn.Columns.Add("DAUTU_9350", typeof(Decimal));
                dt_DuAn.Columns.Add("DAUTU_9400", typeof(Decimal));
                dt_DuAn.Columns.Add("DAUTU_9400_1", typeof(Decimal));
                dt_DuAn.Columns.Add("sSoPheDuyet_DuAn", typeof(String));
                dt_DuAn.Columns.Add("dNgayPheDuyet_DuAn", typeof(String));
                Double DAUTU_9200 = 0, DAUTU_9250 = 0, DAUTU_9300 = 0, DAUTU_9350 = 0, DAUTU_9400 = 0, DAUTU_9400_1 = 0;
                for (int i = 0; i < dt_DuAn_DieuChinh.Rows.Count; i++)
                {
                    for (int j = 0; j < dt_DuAn.Rows.Count; j++)
                    {
                        if (dt_DuAn.Rows[j]["iID_MaDanhMucDuAn"].ToString() == dt_DuAn_DieuChinh.Rows[i]["iID_MaDanhMucDuAn"].ToString())
                        {
                            String DK = "iID_MaDanhMucDuAn='" + dt_DuAn.Rows[j]["iID_MaDanhMucDuAn"].ToString() + "'" + " AND iID_MaMucLucNganSach='" + dt_DuAn.Rows[j]["iID_MaMucLucNganSach"].ToString() + "'";
                            DataRow[] dr = dt_DuAn_DieuChinh.Select(DK);
                            DataRow[] dr_DuAn = dt_DuAn.Select(DK);
                            if ((dr.Length == 1))
                            {
                                dr_DuAn[0]["DAUTU_9200"] = dr[0]["DAUTU_9200"];
                                dr_DuAn[0]["DAUTU_9250"] = dr[0]["DAUTU_9250"];
                                dr_DuAn[0]["DAUTU_9300"] = dr[0]["DAUTU_9300"];
                                dr_DuAn[0]["DAUTU_9350"] = dr[0]["DAUTU_9350"];
                                dr_DuAn[0]["DAUTU_9400"] = dr[0]["DAUTU_9400"];
                                dr_DuAn[0]["DAUTU_9400_1"] = dr[0]["DAUTU_9400_1"];
                                dr_DuAn[0]["sSoPheDuyet_DuAn"] = dr[0]["sSoPheDuyet_DuAn"];
                                dr_DuAn[0]["dNgayPheDuyet_DuAn"] = dr[0]["dNgayPheDuyet_DuAn"];

                            }
                            else if (Convert.ToInt32(dr[0]["iID_MaLoaiDieuChinh"]) == 3)
                            {
                                dr_DuAn[0]["DAUTU_9200"] = dr[0]["DAUTU_9200"];
                                dr_DuAn[0]["DAUTU_9250"] = dr[0]["DAUTU_9250"];
                                dr_DuAn[0]["DAUTU_9300"] = dr[0]["DAUTU_9300"];
                                dr_DuAn[0]["DAUTU_9350"] = dr[0]["DAUTU_9350"];
                                dr_DuAn[0]["DAUTU_9400"] = dr[0]["DAUTU_9400"];
                                dr_DuAn[0]["DAUTU_9400_1"] = dr[0]["DAUTU_9400_1"];
                                dr_DuAn[0]["sSoPheDuyet_DuAn"] = dr[0]["sSoPheDuyet_DuAn"];
                                dr_DuAn[0]["dNgayPheDuyet_DuAn"] = dr[0]["dNgayPheDuyet_DuAn"];
                            }
                            else
                            {
                                DAUTU_9200 = 0; DAUTU_9250 = 0; DAUTU_9300 = 0; DAUTU_9350 = 0; DAUTU_9400 = 0; DAUTU_9400_1 = 0;
                                for (int z = 0; z < dr.Length; z++)
                                {
                                    DAUTU_9200 += Convert.ToDouble(dr[z]["DAUTU_9200"]);
                                    DAUTU_9250 += Convert.ToDouble(dr[z]["DAUTU_9250"]);
                                    DAUTU_9350 += Convert.ToDouble(dr[z]["DAUTU_9350"]);
                                    DAUTU_9300 += Convert.ToDouble(dr[z]["DAUTU_9300"]);
                                    DAUTU_9400 += Convert.ToDouble(dr[z]["DAUTU_9400"]);
                                    DAUTU_9400_1 += Convert.ToDouble(dr[z]["DAUTU_9400_1"]);
                                    if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3 || Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 1)
                                    {
                                        dr_DuAn[0]["sSoPheDuyet_DuAn"] = dr[z]["sSoPheDuyet_DuAn"];
                                        dr_DuAn[0]["dNgayPheDuyet_DuAn"] = dr[z]["dNgayPheDuyet_DuAn"];
                                    }
                                    if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3)
                                    {
                                        break;
                                    }
                                }
                                dr_DuAn[0]["DAUTU_9200"] = DAUTU_9200;
                                dr_DuAn[0]["DAUTU_9250"] = DAUTU_9250;
                                dr_DuAn[0]["DAUTU_9300"] = DAUTU_9300;
                                dr_DuAn[0]["DAUTU_9350"] = DAUTU_9350;
                                dr_DuAn[0]["DAUTU_9400"] = DAUTU_9400;
                                dr_DuAn[0]["DAUTU_9400_1"] = DAUTU_9400_1;
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                String SQLDu_An = String.Format(@"SELECT * FROM(
                                               SELECT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,
	                                           DAUTU_9200=SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END),
                                                        DAUTU_9250=SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END),
                                                        DAUTU_9300=SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END),
                                                        DAUTU_9350=SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END),
                                                        DAUTU_9400=SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END),
                                                        DAUTU_9400_1=SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END),
                                                        sSoPheDuyet as sSoPheDuyet_DuAn,
                                                        CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuAn 
                                               FROM QLDA_TongDauTu
                                               WHERE iTrangThai=1 
													 AND {2} sSoPheDuyet=@sSoPheDuyet 
                                                     AND {1}<>0
                                                     AND ({0})
                                                     AND sM IN(9200,9250,9300,9350,9400)
                                              GROUP BY iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000),
	                                           SUBSTRING(sLNS,1,1),
	                                           sSoPheDuyet,dNgayPheDuyet
                                                    ) as a
                                               INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM", DKDeAn,DKNgoaiTe,DKLoaiNgoaiTe);
                SqlCommand cmd_DuAn = new SqlCommand(SQLDu_An);
                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuAn.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuAn.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                if (sSoPheDuyetDauTu != "TatCa")
                {
                    cmd_DuAn.Parameters.AddWithValue("@sSoPheDuyet", sSoPheDuyetDauTu);
                }
                dt_DuAn = Connection.GetDataTable(cmd_DuAn);
                if (dt_DuAn.Rows.Count > 0)
                    NgayPheDuyet_DauTu = dt_DuAn.Rows[0]["dNgayPheDuyet_DuAn"].ToString();
                cmd_DuAn.Dispose();

            }

            //du toan
            if (sSoPheDuyetDuToan == "TatCa")
            {
                #region tạo dt dự toán
                String SQL_DuToan = String.Format(@"SELECT * FROM( SELECT DISTINCT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS
                                               FROM QLDA_TongDuToan
                                               WHERE iTrangThai=1 
                                                     AND ABS({1})<>0
                                                     AND ({0})
                                                     AND  {2} iID_MaLoaiDieuChinh=1
                                                     AND sM IN(9200,9250,9300,9350,9400)
                                                    ) as a 
                                               INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM
                                            ", DKDeAn, DKNgoaiTe, DKLoaiNgoaiTe);

                SqlCommand cmd_DuToan = new SqlCommand(SQL_DuToan);

                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuToan.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuToan.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                dt_DuToan = Connection.GetDataTable(cmd_DuToan);
                cmd_DuToan.Dispose();
                #endregion
                #region tạo dt dự toán điều chỉnh
                String SQL_DuToan_DieuChinh = String.Format(@"SELECT iID_MaDanhMucDuAn,sTenDuAn,iID_MaLoaiDieuChinh,iID_MaMucLucNganSach, 
                                                        DUTOAN_9200=SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END),
                                                        DUTOAN_9250=SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END),
                                                        DUTOAN_9300=SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END),
                                                        DUTOAN_9350=SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END),
                                                        DUTOAN_9400=SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END),
                                                        DUTOAN_9400_1=SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END),
                                                        dNgayPheDuyet,
                                                         sSoPheDuyet as sSoPheDuyet_DuToan,
                                                         CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuToan 
                                                        FROM QLDA_TongDuToan
                                                         WHERE iTrangThai=1
                                                         AND ({0})
                                                         AND  {2} iID_MaLoaiDieuChinh <>0 
                                                         GROUP BY iID_MaDanhMucDuAn,sTenDuAn,iID_MaLoaiDieuChinh, dNgayPheDuyet,sSoPheDuyet,iID_MaMucLucNganSach
                                                        HAVING ABS(SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END))>0
	                                                          OR ABS(SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END))>0
	                                                          OR ABS(SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END))>0 
	                                                          OR ABS(SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END))>0
	                                                          OR ABS(SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END))>0
	                                                          OR ABS(SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END))>0
                                                        ORDER BY dNgayPheDuyet DESC", DKDeAn, DKNgoaiTe, DKLoaiNgoaiTe);
                SqlCommand cmd_DuToan_DieuChinh = new SqlCommand(SQL_DuToan_DieuChinh);
                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuToan_DieuChinh.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuToan_DieuChinh.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                DataTable dt_DuToan_DieuChinh = Connection.GetDataTable(cmd_DuToan_DieuChinh);
                cmd_DuToan_DieuChinh.Dispose();
                dt_DuToan.Columns.Add("DUTOAN_9200", typeof(Decimal));
                dt_DuToan.Columns.Add("DUTOAN_9250", typeof(Decimal));
                dt_DuToan.Columns.Add("DUTOAN_9300", typeof(Decimal));
                dt_DuToan.Columns.Add("DUTOAN_9350", typeof(Decimal));
                dt_DuToan.Columns.Add("DUTOAN_9400", typeof(Decimal));
                dt_DuToan.Columns.Add("DUTOAN_9400_1", typeof(Decimal));
                dt_DuToan.Columns.Add("sSoPheDuyet_DuToan", typeof(String));
                dt_DuToan.Columns.Add("dNgayPheDuyet_DuToan", typeof(String));
                Double DUTOAN_9200 = 0, DUTOAN_9250 = 0, DUTOAN_9300 = 0, DUTOAN_9350 = 0, DUTOAN_9400 = 0, DUTOAN_9400_1 = 0;
                for (int i = 0; i < dt_DuToan_DieuChinh.Rows.Count; i++)
                {
                    for (int j = 0; j < dt_DuToan.Rows.Count; j++)
                    {
                        if (dt_DuToan.Rows[j]["iID_MaDanhMucDuAn"].ToString() == dt_DuToan_DieuChinh.Rows[i]["iID_MaDanhMucDuAn"].ToString())
                        {
                            String DK = "iID_MaDanhMucDuAn='" + dt_DuToan.Rows[j]["iID_MaDanhMucDuAn"].ToString() +"'"+ " AND iID_MaMucLucNganSach='" + dt_DuToan.Rows[j]["iID_MaMucLucNganSach"].ToString() + "'";
                            DataRow[] dr = dt_DuToan_DieuChinh.Select(DK);
                            DataRow[] dr_DuAn = dt_DuToan.Select(DK);
                            if ((dr.Length == 1))
                            {
                                dr_DuAn[0]["DUTOAN_9200"] = dr[0]["DUTOAN_9200"];
                                dr_DuAn[0]["DUTOAN_9250"] = dr[0]["DUTOAN_9250"];
                                dr_DuAn[0]["DUTOAN_9300"] = dr[0]["DUTOAN_9300"];
                                dr_DuAn[0]["DUTOAN_9350"] = dr[0]["DUTOAN_9350"];
                                dr_DuAn[0]["DUTOAN_9400"] = dr[0]["DUTOAN_9400"];
                                dr_DuAn[0]["DUTOAN_9400_1"] = dr[0]["DUTOAN_9400_1"];
                                dr_DuAn[0]["sSoPheDuyet_DuToan"] = dr[0]["sSoPheDuyet_DuToan"];
                                dr_DuAn[0]["dNgayPheDuyet_DuToan"] = dr[0]["dNgayPheDuyet_DuToan"];
                            }
                            else if (Convert.ToInt32(dr[0]["iID_MaLoaiDieuChinh"]) == 3)
                            {
                                dr_DuAn[0]["DUTOAN_9200"] = dr[0]["DUTOAN_9200"];
                                dr_DuAn[0]["DUTOAN_9250"] = dr[0]["DUTOAN_9250"];
                                dr_DuAn[0]["DUTOAN_9300"] = dr[0]["DUTOAN_9300"];
                                dr_DuAn[0]["DUTOAN_9350"] = dr[0]["DUTOAN_9350"];
                                dr_DuAn[0]["DUTOAN_9400"] = dr[0]["DUTOAN_9400"];
                                dr_DuAn[0]["DUTOAN_9400_1"] = dr[0]["DUTOAN_9400_1"];
                                dr_DuAn[0]["sSoPheDuyet_DuToan"] = dr[0]["sSoPheDuyet_DuToan"];
                                dr_DuAn[0]["dNgayPheDuyet_DuToan"] = dr[0]["dNgayPheDuyet_DuToan"];
                            }
                            else
                            {
                                DUTOAN_9200 = 0; DUTOAN_9250 = 0; DUTOAN_9300 = 0; DUTOAN_9350 = 0; DUTOAN_9400 = 0; DUTOAN_9400_1 = 0;
                                for (int z = 0; z < dr.Length; z++)
                                {
                                    DUTOAN_9200 += Convert.ToDouble(dr[z]["DUTOAN_9200"]);
                                    DUTOAN_9250 += Convert.ToDouble(dr[z]["DUTOAN_9250"]);
                                    DUTOAN_9300 += Convert.ToDouble(dr[z]["DUTOAN_9300"]);
                                    DUTOAN_9350 += Convert.ToDouble(dr[z]["DUTOAN_9350"]);
                                    DUTOAN_9400 += Convert.ToDouble(dr[z]["DUTOAN_9400"]);
                                    DUTOAN_9400_1 += Convert.ToDouble(dr[z]["DUTOAN_9400_1"]);
                                    if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3 || Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 1)
                                    {
                                        dr_DuAn[0]["sSoPheDuyet_DuToan"] = dr[z]["sSoPheDuyet_DuToan"];
                                        dr_DuAn[0]["dNgayPheDuyet_DuToan"] = dr[z]["dNgayPheDuyet_DuToan"];
                                    }
                                    if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3)
                                    {
                                        break;
                                    }
                                }
                                dr_DuAn[0]["DUTOAN_9200"] = DUTOAN_9200;
                                dr_DuAn[0]["DUTOAN_9250"] = DUTOAN_9250;
                                dr_DuAn[0]["DUTOAN_9300"] = DUTOAN_9300;
                                dr_DuAn[0]["DUTOAN_9350"] = DUTOAN_9350;
                                dr_DuAn[0]["DUTOAN_9400"] = DUTOAN_9400;
                                dr_DuAn[0]["DUTOAN_9400_1"] = DUTOAN_9400_1;
                            }
                        }
                    }
                }
               
                #endregion
                
            }
            else
            {
                String SQLDu_Toan = String.Format(@"SELECT * FROM(
                                               SELECT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,
	                                                    DUTOAN_9200=SUM(CASE WHEN sM=9200 THEN {1} ELSE 0 END),
                                                        DUTOAN_9250=SUM(CASE WHEN sM=9250 THEN {1} ELSE 0 END),
                                                        DUTOAN_9300=SUM(CASE WHEN sM=9300 THEN {1} ELSE 0 END),
                                                        DUTOAN_9350=SUM(CASE WHEN sM=9350 THEN {1} ELSE 0 END),
                                                        DUTOAN_9400=SUM(CASE WHEN sM=9400 THEN {1} ELSE 0 END),
                                                        DUTOAN_9400_1=SUM(CASE WHEN (sM=9400 AND sTM=9449 AND sTTM=00 AND sNG=01) THEN {1} ELSE 0 END),
                                                        sSoPheDuyet as sSoPheDuyet_DuToan,
                                                        CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuToan 
                                               FROM QLDA_TongDuToan
                                               WHERE iTrangThai=1 
													 AND {2} sSoPheDuyet=@sSoPheDuyet
                                                     AND {1}<>0
                                                     AND ({0})
                                                     AND sM IN(9200,9250,9300,9350,9400)
                                              GROUP BY iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000),
	                                           SUBSTRING(sLNS,1,1),
	                                           sSoPheDuyet,dNgayPheDuyet
                                                    ) as a
                                               INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM", DKDeAn,DKNgoaiTe,DKLoaiNgoaiTe);
                SqlCommand cmd_DuToan = new SqlCommand(SQLDu_Toan);
                for (int i = 0; i < arrDeAn.Length; i++)
                {
                    cmd_DuToan.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
                }
                if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
                {
                    cmd_DuToan.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
                }
                if (sSoPheDuyetDuToan != "TatCa")
                {
                    cmd_DuToan.Parameters.AddWithValue("@sSoPheDuyet", sSoPheDuyetDuToan);
                }
                dt_DuToan = Connection.GetDataTable(cmd_DuToan);
                if (dt_DuToan.Rows.Count > 0)
                    NgayPheDuyet_DuToan = dt_DuToan.Rows[0]["dNgayPheDuyet_DuToan"].ToString();
                cmd_DuToan.Dispose();
            }
          

            #region ghép dt dự toán vào dt dự án
            DataRow addR, R2;
            String sCol = "iID_MaDanhMucDuAn,iID_MaMucLucNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,NguonNS,sTienDo,DM,sSoPheDuyet_DuToan,dNgayPheDuyet_DuToan,DUTOAN_9200,DUTOAN_9250,DUTOAN_9300,DUTOAN_9350,DUTOAN_9400,DUTOAN_9400_1";
            String[] arrCol = sCol.Split(',');
            dt_DuAn.Columns.Add("sSoPheDuyet_DuToan", typeof(String));
            dt_DuAn.Columns.Add("dNgayPheDuyet_DuToan", typeof(String));
            dt_DuAn.Columns.Add("DUTOAN_9200", typeof(Decimal));
            dt_DuAn.Columns.Add("DUTOAN_9250", typeof(Decimal));
            dt_DuAn.Columns.Add("DUTOAN_9300", typeof(Decimal));
            dt_DuAn.Columns.Add("DUTOAN_9350", typeof(Decimal));
            dt_DuAn.Columns.Add("DUTOAN_9400", typeof(Decimal));
            dt_DuAn.Columns.Add("DUTOAN_9400_1", typeof(Decimal));


            for (int i = 0; i < dt_DuToan.Rows.Count; i++)
            {
                String xauTruyVan = String.Format(@"iID_MaDanhMucDuAn='{0}' AND sDeAn='{1}' AND sDuAn='{2}' AND sDuAnThanhPhan='{3}' AND sCongTrinh='{4}' AND sHangMucCongTrinh='{5}' AND sHangMucChiTiet='{6}' AND NguonNS='{7}' AND iID_MaMucLucNganSach='{8}'",
                                                  dt_DuToan.Rows[i]["iID_MaDanhMucDuAn"], dt_DuToan.Rows[i]["sDeAn"], dt_DuToan.Rows[i]["sDuAn"], dt_DuToan.Rows[i]["sDuAnThanhPhan"], dt_DuToan.Rows[i]["sCongTrinh"],
                                                  dt_DuToan.Rows[i]["sHangMucCongTrinh"], dt_DuToan.Rows[i]["sHangMucChiTiet"], dt_DuToan.Rows[i]["NguonNS"], dt_DuToan.Rows[i]["iID_MaMucLucNganSach"]
                                                  );
                DataRow[] R = dt_DuAn.Select(xauTruyVan);

                if (R == null || R.Length == 0)
                {
                    addR = dt_DuAn.NewRow();
                    for (int j = 0; j < arrCol.Length; j++)
                    {
                        addR[arrCol[j]] = dt_DuToan.Rows[i][arrCol[j]];
                    }
                    dt_DuAn.Rows.Add(addR);
                }
                else
                {
                    foreach (DataRow R1 in dt_DuToan.Rows)
                    {

                        for (int j = 0; j < dt_DuAn.Rows.Count; j++)
                        {
                            Boolean okTrung = true;
                            R2 = dt_DuAn.Rows[j];

                            for (int c = 0; c < arrCol.Length - 8; c++)
                            {
                                if (R2[arrCol[c]].Equals(R1[arrCol[c]]) == false)
                                {
                                    okTrung = false;
                                    break;
                                }
                            }
                            if (okTrung)
                            {
                                dt_DuAn.Rows[j]["sSoPheDuyet_DuToan"] = R1["sSoPheDuyet_DuToan"];
                                dt_DuAn.Rows[j]["dNgayPheDuyet_DuToan"] = R1["dNgayPheDuyet_DuToan"];
                                dt_DuAn.Rows[j]["DUTOAN_9200"] = R1["DUTOAN_9200"];
                                dt_DuAn.Rows[j]["DUTOAN_9250"] = R1["DUTOAN_9250"];
                                dt_DuAn.Rows[j]["DUTOAN_9300"] = R1["DUTOAN_9300"];
                                dt_DuAn.Rows[j]["DUTOAN_9350"] = R1["DUTOAN_9350"];
                                dt_DuAn.Rows[j]["DUTOAN_9400"] = R1["DUTOAN_9400"];
                                dt_DuAn.Rows[j]["DUTOAN_9400_1"] = R1["DUTOAN_9400_1"];
                                break;
                            }
                        }
                    }
                }
            }
            //sắp xếp datatable sau khi ghép
            DataView dv = dt_DuAn.DefaultView;
            dv.Sort = "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,NguonNS,dNgayPheDuyet_DuToan";
            dt_DuAn = dv.ToTable();
            #endregion
            return dt_DuAn;
        }
        #endregion

        /// <summary>
        /// Tạo báo cáo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        #region "Tạo báo cáo"
        public ExcelFile CreateReport(String path, String MaTien,String sDeAn, String sSoPheDuyetDauTu, String sSoPheDuyetDuToan)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Ngay = "ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            String _SoPheDuyet_DauTu = "", _SoPheDuyet_DuToan = ""; 
            if (sSoPheDuyetDauTu != "TatCa")
            {
                _SoPheDuyet_DauTu = sSoPheDuyetDauTu;
            }
            if (sSoPheDuyetDuToan != "TatCa")
            {
                _SoPheDuyet_DuToan = sSoPheDuyetDuToan;
            }
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_ChiTiet_DanhMucCongTrinh_1");
            DataTable dtDVT = QLDA_ReportModel.getdtTien();
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count; i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTen"].ToString();
                }
            }
            dtDVT.Dispose();
            LoadData(fr,MaTien, sDeAn,sSoPheDuyetDauTu,sSoPheDuyetDuToan);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("SoPheDuyet_DauTu", _SoPheDuyet_DauTu);
            fr.SetValue("SoPheDuyet_DuToan", _SoPheDuyet_DuToan);
            fr.SetValue("NgayPheDuyet_DuToan", NgayPheDuyet_DuToan);
            fr.SetValue("NgayPheDuyet_DauTu", NgayPheDuyet_DauTu);
            fr.SetValue("DVT", DVT);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
        #endregion
        /// <summary>
        /// Đổ dữ liệu xuống file báo cáo
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="Thang"></param>
        /// <param name="Nam"></param>
        #region "Đổ dữ liệu xuống file báo cáo"
        private void LoadData(FlexCelReport fr, String MaTien, String sDeAn, String sSoPheDuyetDauTu, String sSoPheDuyetDuToan)
        {
            DataTable data = ChiTiet_DMCT(MaTien, sDeAn, sSoPheDuyetDauTu, sSoPheDuyetDuToan);
            // Hạng mục công trình
            DataTable dtHangMucCongTrinh = HamChung.SelectDistinct_QLDA("HMCT", data, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet");
            //Công trình
            DataTable dtCongTrinh = HamChung.SelectDistinct_QLDA("CongTrinh", dtHangMucCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh");
            //Dự án thành phần
            DataTable dtDuAnThanhPhan = HamChung.SelectDistinct_QLDA("DATP", dtCongTrinh, "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan", "NguonNS,sDeAn,sDuAn,sDuAnThanhPhan,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh");
            //Dự án
            DataTable dtDuAn = HamChung.SelectDistinct_QLDA("DuAn", dtDuAnThanhPhan, "NguonNS,sDeAn,sDuAn", "NguonNS,sDeAn,sDuAn,sTenDuAn,sTienDo", "sDeAn,sDuAn,sDuAnThanhPhan");
            //Đề án
            DataTable dtDeAn = HamChung.SelectDistinct_QLDA("DeAn", dtDuAn, "NguonNS,sDeAn", "NguonNS,sDeAn,sTenDuAn,sTienDo", "sDeAn,sDuAn");
            //Nguồn
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS", "NguonNS,sTenDuAn", "", "NguonNS");
            data.TableName = "Chitiet";
            fr.AddTable("Chitiet", data);
            fr.AddTable("HMCT", dtHangMucCongTrinh);
            fr.AddTable("CongTrinh", dtCongTrinh);
            fr.AddTable("DATP", dtDuAnThanhPhan);
            fr.AddTable("DuAn", dtDuAn);
            fr.AddTable("DeAn", dtDeAn);
            fr.AddTable("Nguon", dtNguon);
            dtDeAn.Dispose();
            dtDuAn.Dispose();
            dtDuAnThanhPhan.Dispose();
            dtCongTrinh.Dispose();
            dtNguon.Dispose();
            data.Dispose();
        }
        #endregion
        /// <summary>
        /// ViewPDF
        /// </summary>
        /// <param name="Quy"></param>
        /// <param name="Nam"></param>
        /// <returns></returns>
        public ActionResult ViewPDF(String MaTien, String sDeAn, String sSoPheDuyetDauTu, String sSoPheDuyetDuToan,String iCapTongHop)
        {
            String DuongDan = "";
            if (iCapTongHop == "0") DuongDan = sFilePath_DeAn;
            else if (iCapTongHop == "1") DuongDan = sFilePath_DuAn;
            else if (iCapTongHop == "2") DuongDan = sFilePath_DATP;
            else if (iCapTongHop == "3") DuongDan = sFilePath_CT;
            else if (iCapTongHop == "4") DuongDan = sFilePath_HMCT;
            else DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaTien, sDeAn, sSoPheDuyetDauTu, sSoPheDuyetDuToan);
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

        public static DataTable GetDanhSachQD_TongDauTu(String sDeAn)
        {
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length;i++)
            {
                DKDeAn += "sDeAn=@sDeAn"+i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR";
            }
            String SQL = String.Format(@"SELECT DISTINCT sSoPheDuyet, sSoPheDuyet as TenHT
                                        FROM QLDA_TongDauTu
                                        WHERE iTrangThai=1
                                              AND sM IN(9200,9250,9300,9350,9400)
                                              AND ({0})", DKDeAn);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i=0;i<arrDeAn.Length;i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count !=0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "TatCa";
                dr[1] = "Chọn tất cả";
                dt.Rows.InsertAt(dr, 0);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr[0] = "-1111111";
                dr[1] = "Không có quyết định";
                dt.Rows.InsertAt(dr, 0);
            }
            dt.Dispose();
            return dt;
        }
        public static DataTable GetDanhSachQD_TongDuToan(String sDeAn)
        {
            String[] arrDeAn = sDeAn.Split(',');
            String DKDeAn = "";
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                DKDeAn += "sDeAn=@sDeAn" + i;
                if (i < arrDeAn.Length - 1)
                    DKDeAn += " OR";
            }
            String SQL = String.Format(@"SELECT DISTINCT sSoPheDuyet, sSoPheDuyet as TenHT
                                        FROM QLDA_TongDuToan
                                        WHERE iTrangThai=1 AND sM IN(9200,9250,9300,9350,9400) AND ({0})", DKDeAn);
            SqlCommand cmd = new SqlCommand(SQL);
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            DataTable dt = Connection.GetDataTable(cmd);
            cmd.Dispose();
            if (dt.Rows.Count != 0)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "TatCa";
                dr[1] = "Chọn tất cả";
                dt.Rows.InsertAt(dr, 0);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr[0] = "-1111111";
                dr[1] = "Không có quyết định";
                dt.Rows.InsertAt(dr, 0);
            }
            
            dt.Dispose();
            return dt;
        }
        public class QLDA_DMCT
        {
            public String DeAn { get; set; }
            public String SoDauTu { get; set; }
            public String SoDuToan { get; set; }
            public String NgoaiTe { get; set; }
        }
        [HttpGet]
        public JsonResult ds_QLDA(String ParentID,String sDeAn,String sSoDauTu,String sSoDuToan,String MaTien)
        {
            return Json(obj_QLDA(ParentID, sDeAn, sSoDauTu, sSoDuToan, MaTien), JsonRequestBehavior.AllowGet);
        }
        public QLDA_DMCT obj_QLDA(String ParentID, String sDeAn, String sSoDauTu, String sSoDuToan,String MaTien)
        {
            QLDA_DMCT data = new QLDA_DMCT();

            #region đề án
            String input = "";
            DataTable dtDeAn = QLDA_ReportModel.dt_DeAn();
            StringBuilder stbDeAn = new StringBuilder();
            stbDeAn.Append("<fieldset>");
            stbDeAn.Append("<legend><b>Đề án</b></legend>");
            stbDeAn.Append("<div style=\"width: 99%; height: 150px; overflow: scroll; border:1px solid black;\">");
            stbDeAn.Append("<table class=\"mGrid\">");
            stbDeAn.Append("<tr>");
            stbDeAn.Append("<td><input type=\"checkbox\" id=\"checkAll\" onclick=\"Chonall(this.checked)\"></td><td> Chọn tất cả đề án </td>");
            stbDeAn.Append("</fieldset>");
            String TenDeAn = "", MaDeAn = "";
            String[] arrDeAn = sDeAn.Split(',');
            String _Checked = "checked=\"checked\"";
            for (int i = 1; i <= dtDeAn.Rows.Count; i++)
            {
                MaDeAn = Convert.ToString(dtDeAn.Rows[i - 1]["sDeAn"]);
                TenDeAn = Convert.ToString(dtDeAn.Rows[i - 1]["sTenDuAn"]);
                _Checked = "";
                for (int j = 1; j <= arrDeAn.Length; j++)
                {
                    if (MaDeAn == arrDeAn[j - 1])
                    {
                        _Checked = "checked=\"checked\"";
                        break;
                    }
                }

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDeAn\" id=\"sDeAn\" name=\"sDeAn\" onchange=\"ChonDeAn()\" />", MaDeAn, _Checked);
                stbDeAn.Append("<tr>");
                stbDeAn.Append("<td style=\"width: 15%;\">");
                stbDeAn.Append(input);
                stbDeAn.Append("</td>");
                stbDeAn.Append("<td>" + TenDeAn + "</td>");

                stbDeAn.Append("</tr>");
            }
            stbDeAn.Append("</table>");
            stbDeAn.Append("</div>");
            dtDeAn.Dispose();
            String DeAn = stbDeAn.ToString();
            data.DeAn = DeAn;
            #endregion
            #region ngoại tệ
            DataTable dtNgoaiTe = QLDA_ReportModel.getdtTien();
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTen");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            data.NgoaiTe = NgoaiTe;
            #endregion
            #region sSoDauTu
            DataTable dtDauTu = GetDanhSachQD_TongDauTu(sDeAn);
            SelectOptionList slDauTu = new SelectOptionList(dtDauTu, "sSoPheDuyet", "TenHT");
            String DauTu = MyHtmlHelper.DropDownList(ParentID, slDauTu, sSoDauTu, "sSoDauTu", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtDauTu.Dispose();
            data.SoDauTu = DauTu;
            #endregion
            #region sSoDuToan
            DataTable dtDuToan = GetDanhSachQD_TongDuToan(sDeAn);
            SelectOptionList slDuToan = new SelectOptionList(dtDuToan, "sSoPheDuyet", "TenHT");
            String DuToan = MyHtmlHelper.DropDownList(ParentID, slDuToan, sSoDuToan, "sSoDuToan", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtDuToan.Dispose();
            data.SoDuToan = DuToan;
            #endregion
            return data;
        }
        public clsExcelResult ExportToExcel(String MaTien, String sDeAn, String sSoPheDuyetDauTu, String sSoPheDuyetDuToan, String iCapTongHop)
        {

            String DuongDan = "";
            if (iCapTongHop == "0") DuongDan = sFilePath_DeAn;
            else if (iCapTongHop == "1") DuongDan = sFilePath_DuAn;
            else if (iCapTongHop == "2") DuongDan = sFilePath_DATP;
            else if (iCapTongHop == "3") DuongDan = sFilePath_CT;
            else if (iCapTongHop == "4") DuongDan = sFilePath_HMCT;
            else DuongDan = sFilePath;
            clsExcelResult clsResult = new clsExcelResult();
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), MaTien, sDeAn, sSoPheDuyetDauTu, sSoPheDuyetDuToan);

            using (MemoryStream ms = new MemoryStream())
            {
                xls.Save(ms);
                ms.Position = 0;
                clsResult.ms = ms;
                clsResult.FileName = "QLDA_ChiTiet_DanhMucCongTrinh.xls";
                clsResult.type = "xls";
                return clsResult;
            }
        }
    }
}
