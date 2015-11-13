<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
  

      <%
        String iID_MaDonVi = "";
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";
        String iID_MaDotPhanBo = Request.QueryString["iID_MaDotPhanBo"];
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        }
        
        String sLNS = Request.QueryString["sLNS"];
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = Convert.ToString(ViewData["sLNS"]);
        }

        String MaND = User.Identity.Name;
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = new DataTable();
        dtNam.Columns.Add("MaNam", typeof(String));
        dtNam.Columns.Add("TenNam", typeof(String));
        DataRow R;
        for (int i = NamMin; i < NamMax; i++)
        {
            R = dtNam.NewRow();
            dtNam.Rows.Add(R);
            R[0] = Convert.ToString(i);
            R[1] = Convert.ToString(i);
        }
        dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
        dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();
         
        //Loai ngan sach
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "sLNS");
        String[] arrDonVi = sLNS.Split(',');     
        using (Html.BeginForm("EditSubmit", "rptTongHopNganSach", new { ParentID = ParentID, sLNS = sLNS, iID_MaDotPhanBo = iID_MaDotPhanBo,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet }))
        {%>
<%--    <%=MyHtmlHelper.Hidden(ParentID,NamLamViec,"iNamLamViec","") %>
    <%=MyHtmlHelper.Hidden(ParentID, sLNS, "sLNS", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDotPhanBo, "iID_MaDotPhanBo", "")%>--%>
   
    <div>
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                    
                        <span>Báo cáo tổng hợp ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
     <%} %>
    <iframe src="<%=Url.Action("ViewPDF","rptTongHopNganSach", new{sLNS=sLNS,iID_MaDotPhanBo=iID_MaDotPhanBo,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet})%>" height="600px" width="100%">
    </iframe>
</body>
</html>
