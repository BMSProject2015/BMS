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
    public class rptQLDA_01CTController : Controller
    {
        //
        // GET: /rptQLDA_01CT/

        public string sViewPath = "~/Report_Views/";
        private const String sFilePath = "/Report_ExcelFrom/QLDA/rptQLDA_01CT.xls";
        private const String sFilePath_HMCT = "/Report_ExcelFrom/QLDA/rptQLDA_01CT_HMCT.xls";
        private const String sFilePath_CT = "/Report_ExcelFrom/QLDA/rptQLDA_01CT_CT.xls";
        private const String sFilePath_DATP = "/Report_ExcelFrom/QLDA/rptQLDA_01CT_DATP.xls";
        private const String sFilePath_DuAn = "/Report_ExcelFrom/QLDA/rptQLDA_01CT_DuAn.xls";
        private const String sFilePath_DeAn = "/Report_ExcelFrom/QLDA/rptQLDA_01CT_DeAn.xls";
        /// <summary>
        /// Index
        /// </summary>        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01CT.aspx";
            ViewData["PageLoad"]="0";
            return View(sViewPath + "ReportView.aspx");
        }
        
        public ActionResult EditSubmit(String ParentID)
        {
            String dNgay = Convert.ToString(Request.Form[ParentID + "_vidNgay"]);
            String sDeAn = Convert.ToString(Request.Form["sDeAn"]);
            String MaTien = Convert.ToString(Request.Form[ParentID + "_MaTien"]);
            String iCapTongHop = Convert.ToString(Request.Form[ParentID + "_iCapTongHop"]);
            ViewData["dNgay"] = dNgay;
            ViewData["sDeAn"] = sDeAn;
            ViewData["MaTien"] = MaTien;
            ViewData["iCapTongHop"] = iCapTongHop;
             ViewData["path"] = "~/Report_Views/QLDA/rptQLDA_01CT.aspx";
            ViewData["PageLoad"]="1";
            return View(sViewPath + "ReportView.aspx");
        }
        public static DataTable dtQLDA_01CT(String dNgay, String MaTien, String sDeAn)
        {
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
            #region tạo dt dự án
            String SQL_DuAn = String.Format(@"SELECT *
                                                FROM (SELECT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                           sDeAn,
	                                           sDuAn,
	                                           sDuAnThanhPhan,
	                                           sCongTrinh,
	                                           sHangMucCongTrinh,
	                                           sHangMucChiTiet,
	                                           SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                           SUBSTRING(sLNS,1,1) as NguonNS,sLNS,sL,sK,sM,sTM,sTTM,sNG
                                               ,{1} as rSoTien_DauTu_DuAn
                                               FROM QLDA_TongDauTu
                                               WHERE iTrangThai=1 AND sM IN(9200,9250,9300,9350,9400) AND 
                                                      {2}
                                                     ABS({1})>0 AND
                                                     iID_MaLoaiDieuChinh=1 AND
                                                     dNgayPheDuyet<=@dNgay AND
                                                        ({0})) as a
                                              INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM
                                              ", DKDeAn,DKNgoaiTe,DKLoaiNgoaiTe);
            SqlCommand cmd_DuAn = new SqlCommand(SQL_DuAn);
            cmd_DuAn.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd_DuAn.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
            {
                cmd_DuAn.Parameters.AddWithValue("@iID_MaNgoaiTe",MaTien);
            }
            DataTable dt_DuAn = Connection.GetDataTable(cmd_DuAn);
            cmd_DuAn.Dispose();
            #endregion
            #region Tiền điều chỉnh dự án
            String SQL_DieuChinh_DuAn = String.Format(@"SELECT sLNS,sL,sK,sM,sTM,sTTM,sNG,iID_MaMucLucNganSach,iID_MaDanhMucDuAn,sTenDuAn,iID_MaLoaiDieuChinh, {0} as rSoTien, rNgoaiTe, sTenNgoaiTe,dNgayPheDuyet, sSoPheDuyet as sSoPheDuyet_DuAn,CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuAn  
                                                        FROM QLDA_TongDauTu
                                                        WHERE iTrangThai=1 AND sM IN(9200,9250,9300,9350,9400) AND {1} iID_MaLoaiDieuChinh <>0 AND dNgayPheDuyet<=@dNgay AND ABS({0})>0
                                                        ORDER BY sLNS,sL,sK,sM,sTM,sTTM,sNG,dNgayPheDuyet DESC", DKNgoaiTe, DKLoaiNgoaiTe);
            SqlCommand cmd_DieuChinh_DuAn = new SqlCommand(SQL_DieuChinh_DuAn);
            cmd_DieuChinh_DuAn.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
            {
                cmd_DieuChinh_DuAn.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt_DieuChinhDuAn = Connection.GetDataTable(cmd_DieuChinh_DuAn);
            cmd_DieuChinh_DuAn.Dispose();
            dt_DuAn.Columns.Add("rSoTien_DieuChinh_DuAn",typeof(Decimal));
            dt_DuAn.Columns.Add("sSoPheDuyet_DuAn", typeof(String));
            dt_DuAn.Columns.Add("dNgayPheDuyet_DuAn", typeof(String));
            Double rSoTien = 0, rNgoaiTe = 0;
            for (int i = 0; i < dt_DieuChinhDuAn.Rows.Count;i++)
            {
                for (int j = 0; j < dt_DuAn.Rows.Count;j++)
                {
                    if (dt_DuAn.Rows[j]["iID_MaDanhMucDuAn"].ToString() == dt_DieuChinhDuAn.Rows[i]["iID_MaDanhMucDuAn"].ToString() && dt_DuAn.Rows[j]["iID_MaMucLucNganSach"].ToString() == dt_DieuChinhDuAn.Rows[i]["iID_MaMucLucNganSach"].ToString())
                    {
                        String DK = "iID_MaDanhMucDuAn='" + dt_DuAn.Rows[j]["iID_MaDanhMucDuAn"].ToString() + "'" + " AND iID_MaMucLucNganSach='" + dt_DuAn.Rows[j]["iID_MaMucLucNganSach"].ToString() + "'";
                        DataRow[] dr = dt_DieuChinhDuAn.Select(DK);
                        DataRow[] dr_DuAn = dt_DuAn.Select(DK);
                        if ((dr.Length == 1))
                        {
                            dr_DuAn[0]["rSoTien_DieuChinh_DuAn"] = dr[0]["rSoTien"];
                            dr_DuAn[0]["sSoPheDuyet_DuAn"] = dr[0]["sSoPheDuyet_DuAn"];
                            dr_DuAn[0]["dNgayPheDuyet_DuAn"] = dr[0]["dNgayPheDuyet_DuAn"];
                        }
                        else if (Convert.ToInt32(dr[0]["iID_MaLoaiDieuChinh"]) == 3)
                        {
                            dr_DuAn[0]["rSoTien_DieuChinh_DuAn"] = dr[0]["rSoTien"];
                            dr_DuAn[0]["sSoPheDuyet_DuAn"] = dr[0]["sSoPheDuyet_DuAn"];
                            dr_DuAn[0]["dNgayPheDuyet_DuAn"] = dr[0]["dNgayPheDuyet_DuAn"];
                        }
                        else
                        {
                            rSoTien = 0; rNgoaiTe = 0;
                            for (int z = 0; z < dr.Length; z++)
                            {
                                rSoTien += Convert.ToDouble(dr[z]["rSoTien"]);
                                rNgoaiTe += Convert.ToDouble(dr[z]["rNgoaiTe"]);
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
                            dr_DuAn[0]["rSoTien_DieuChinh_DuAn"] = rSoTien;
                        }
                    }
                }
            }
            #endregion
            #region tạo dt dự toán
            String SQL_DuToan = String.Format(@"SELECT * FROM(
                                        SELECT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,
	                                   sDeAn,
	                                   sDuAn,
	                                   sDuAnThanhPhan,
	                                   sCongTrinh,
	                                   sHangMucCongTrinh,
	                                   sHangMucChiTiet,
	                                   SUBSTRING(sTenDuAn,19,10000) as sTenDuAn,
	                                   SUBSTRING(sLNS,1,1) as NguonNS,
                                       {1} as rSoTien_DauTu_DuToan
                                       FROM QLDA_TongDuToan
                                       WHERE iTrangThai=1 AND
                                             ABS({1})>0.5
                                             AND  {2} dNgayPheDuyet<=@dNgay AND
                                             iID_MaLoaiDieuChinh=1 AND
                                              ({0})) as a
                                        INNER JOIN (SELECT iID_MaDanhMucDuAn as DM,sTienDo FROM QLDA_DanhMucDuAn WHERE iTrangThai=1) as b
                                               ON  a.iID_MaDanhMucDuAn=b.DM
                                       ", DKDeAn,DKNgoaiTe,DKLoaiNgoaiTe);
            SqlCommand cmd_DuToan = new SqlCommand(SQL_DuToan);
            cmd_DuToan.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            for (int i = 0; i < arrDeAn.Length; i++)
            {
                cmd_DuToan.Parameters.AddWithValue("@sDeAn" + i, arrDeAn[i]);
            }
            if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
            {
                cmd_DuToan.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt_DuToan = Connection.GetDataTable(cmd_DuToan);
            cmd_DuToan.Dispose();
            #endregion
            #region Tiền điều chỉnh dự toán
            String SQL_DieuChinh_DuToan = String.Format(@"SELECT iID_MaDanhMucDuAn,iID_MaMucLucNganSach,sTenDuAn,iID_MaLoaiDieuChinh, {0} as rSoTien, rNgoaiTe, sTenNgoaiTe,dNgayPheDuyet, sSoPheDuyet as sSoPheDuyet_DuToan,CONVERT(varchar(10),dNgayPheDuyet,103) as dNgayPheDuyet_DuToan  
                                                        FROM QLDA_TongDuToan
                                                        WHERE iTrangThai=1 AND {1} iID_MaLoaiDieuChinh <>0 AND dNgayPheDuyet<=@dNgay AND ABS({0})>0.5
                                                        ORDER BY dNgayPheDuyet DESC",DKNgoaiTe,DKLoaiNgoaiTe);
            SqlCommand cmd_DieuChinh_DuToan = new SqlCommand(SQL_DieuChinh_DuToan);
            cmd_DieuChinh_DuToan.Parameters.AddWithValue("@dNgay", CommonFunction.LayNgayTuXau(dNgay));
            if (!String.IsNullOrEmpty(DKLoaiNgoaiTe))
            {
                cmd_DieuChinh_DuToan.Parameters.AddWithValue("@iID_MaNgoaiTe", MaTien);
            }
            DataTable dt_DieuChinhDuToan = Connection.GetDataTable(cmd_DieuChinh_DuToan);
            cmd_DieuChinh_DuToan.Dispose();
            dt_DuToan.Columns.Add("rSoTien_DieuChinh_DuToan", typeof(Decimal));
            dt_DuToan.Columns.Add("sSoPheDuyet_DuToan", typeof(String));
            dt_DuToan.Columns.Add("dNgayPheDuyet_DuToan", typeof(String));
            Double rSoTien_DuToan = 0, rNgoaiTe_DuToan = 0;
            for (int i = 0; i < dt_DieuChinhDuToan.Rows.Count; i++)
            {
                for (int j = 0; j < dt_DuToan.Rows.Count; j++)
                {
                    if (dt_DuToan.Rows[j]["iID_MaDanhMucDuAn"].ToString() == dt_DieuChinhDuToan.Rows[i]["iID_MaDanhMucDuAn"].ToString())
                    {
                        String DK = "iID_MaDanhMucDuAn='" + dt_DuToan.Rows[j]["iID_MaDanhMucDuAn"].ToString() + "'" + " AND iID_MaMucLucNganSach='" + dt_DuToan.Rows[j]["iID_MaMucLucNganSach"].ToString() + "'";
                        DataRow[] dr = dt_DieuChinhDuToan.Select(DK);
                        DataRow[] dr_DuToan = dt_DuToan.Select(DK);
                        if ((dr.Length == 1))
                        {
                            dr_DuToan[0]["rSoTien_DieuChinh_DuToan"] = dr[0]["rSoTien"];
                            dr_DuToan[0]["sSoPheDuyet_DuToan"] = dr[0]["sSoPheDuyet_DuToan"];
                            dr_DuToan[0]["dNgayPheDuyet_DuToan"] = dr[0]["dNgayPheDuyet_DuToan"];
                        }
                        else if (Convert.ToInt32(dr[0]["iID_MaLoaiDieuChinh"]) == 3)
                        {
                            dr_DuToan[0]["rSoTien_DieuChinh_DuToan"] = dr[0]["rSoTien"];
                            dr_DuToan[0]["sSoPheDuyet_DuToan"] = dr[0]["sSoPheDuyet_DuToan"];
                            dr_DuToan[0]["dNgayPheDuyet_DuToan"] = dr[0]["dNgayPheDuyet_DuToan"];
                        }
                        else
                        {
                            rSoTien_DuToan = 0; rNgoaiTe_DuToan = 0;
                            for (int z = 0; z < dr.Length; z++)
                            {
                                rSoTien_DuToan += Convert.ToDouble(dr[z]["rSoTien"]);
                                rNgoaiTe_DuToan += Convert.ToDouble(dr[z]["rNgoaiTe"]);
                                if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3 || Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 1)
                                {
                                    dr_DuToan[0]["sSoPheDuyet_DuToan"] = dr[z]["sSoPheDuyet_DuToan"];
                                    dr_DuToan[0]["dNgayPheDuyet_DuToan"] = dr[z]["dNgayPheDuyet_DuToan"];
                                }
                                if (Convert.ToInt32(dr[z]["iID_MaLoaiDieuChinh"]) == 3)
                                {
                                    break;
                                }
                            }
                            dr_DuToan[0]["rSoTien_DieuChinh_DuToan"] = rSoTien_DuToan;
                        }
                    }
                }
            }
            #endregion
            #region ghép dt dự toán vào dt dự án
            DataRow addR, R2;
            String sCol = "iID_MaDanhMucDuAn,iID_MaMucLucNganSach,sDeAn,sDuAn,sDuAnThanhPhan,sCongTrinh,sHangMucCongTrinh,sHangMucChiTiet,sTenDuAn,NguonNS,sTienDo,sSoPheDuyet_DuToan,dNgayPheDuyet_DuToan,rSoTien_DauTu_DuToan,rSoTien_DieuChinh_DuToan";
            String[] arrCol = sCol.Split(',');
            dt_DuAn.Columns.Add("sSoPheDuyet_DuToan", typeof(String));
            dt_DuAn.Columns.Add("dNgayPheDuyet_DuToan", typeof(String));
            dt_DuAn.Columns.Add("rSoTien_DauTu_DuToan", typeof(Decimal));
            dt_DuAn.Columns.Add("rSoTien_DieuChinh_DuToan", typeof(Decimal));

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

                            for (int c = 0; c < arrCol.Length - 4; c++)
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
                                dt_DuAn.Rows[j]["rSoTien_DauTu_DuToan"] = R1["rSoTien_DauTu_DuToan"];
                                dt_DuAn.Rows[j]["rSoTien_DieuChinh_DuToan"] = R1["rSoTien_DieuChinh_DuToan"];
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
       public class QLDA_01CT
        {
            public String DeAn { get; set;}
            public String NgoaiTe { get; set; }
        }
         [HttpGet]
       public JsonResult ds_QLDA(String ParentID, String dNgay, String MaTien, String sDeAn)
        {
            return Json(obj_QLDA(ParentID, dNgay, MaTien, sDeAn), JsonRequestBehavior.AllowGet);
        }
        public QLDA_01CT obj_QLDA(String ParentID, String dNgay, String MaTien, String sDeAn)
         {
             QLDA_01CT data= new QLDA_01CT();

             #region đề án
             String input = "";
            DataTable dtDeAn = QLDA_ReportModel.dt_DeAn(dNgay);
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

                input = String.Format("<input type=\"checkbox\" value=\"{0}\" {1} check-group=\"MaDeAn\" id=\"sDeAn\" name=\"sDeAn\" onchange=\"ChonThang()\" />", MaDeAn, _Checked);
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
            DataTable dtNgoaiTe = QLDA_ReportModel.dt_LoaiTien(sDeAn, dNgay);
            SelectOptionList slNgoaiTe = new SelectOptionList(dtNgoaiTe, "iID_MaNgoaiTe", "sTenNgoaiTe");
            String NgoaiTe = MyHtmlHelper.DropDownList(ParentID, slNgoaiTe, MaTien, "MaTien", "", "class=\"input1_2\" style=\"width: 80%\"");
            dtNgoaiTe.Dispose();
            data.NgoaiTe = NgoaiTe;
            #endregion
            return data;
         }
        private void LoadData(FlexCelReport fr, String dNgay, String MaTien, String sDeAn)
        {
            DataTable data = dtQLDA_01CT(dNgay, MaTien,sDeAn);
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
            DataTable dtNguon = HamChung.SelectDistinct("Nguon", dtDeAn, "NguonNS","NguonNS,sTenDuAn", "", "NguonNS");

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
        public ActionResult ViewPDF(String dNgay, String MaTien, String sDeAn, String iCapTongHop)
        {
            String DuongDan = "";
            if (iCapTongHop == "0") DuongDan = sFilePath_DeAn;
            else if (iCapTongHop == "1") DuongDan = sFilePath_DuAn;
            else if (iCapTongHop == "2") DuongDan = sFilePath_DATP;
            else if (iCapTongHop == "3") DuongDan = sFilePath_CT;
            else if (iCapTongHop == "4") DuongDan = sFilePath_HMCT;
            else DuongDan = sFilePath;
            ExcelFile xls = CreateReport(Server.MapPath(DuongDan), dNgay, MaTien, sDeAn);
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
        public ExcelFile CreateReport(String path, String dNgay, String MaTien, String sDeAn)
        {
            XlsFile Result = new XlsFile(true);
            Result.Open(path);
            String Ngay = "ngày " + dNgay.Substring(0, 2) + " tháng " + dNgay.Substring(3, 2) + " năm "+dNgay.Substring(6,4);
            FlexCelReport fr = new FlexCelReport();
            fr = ReportModels.LayThongTinChuKy(fr, "rptQLDA_01CT");
           DataTable dtDVT=QLDA_ReportModel.dt_LoaiTien(sDeAn, dNgay);
            String DVT = " triệu đồng";
            for (int i = 1; i < dtDVT.Rows.Count;i++)
            {
                if (MaTien == dtDVT.Rows[i]["iID_MaNgoaiTe"].ToString())
                {
                    DVT = dtDVT.Rows[i]["sTenNgoaiTe"].ToString();
                }

            }
            dtDVT.Dispose();
            LoadData(fr, dNgay, MaTien, sDeAn);
            fr.SetValue("Ngay", Ngay);
            fr.SetValue("DVT", DVT);
            fr.SetValue("NgayThang", ReportModels.Ngay_Thang_Nam_HienTai());
            fr.SetValue("Cap2", ReportModels.CauHinhTenDonViSuDung(2).ToUpper());
            fr.SetValue("Cap3", ReportModels.CauHinhTenDonViSuDung(3).ToUpper());
            fr.Run(Result);
            return Result;
        }
    }
}
