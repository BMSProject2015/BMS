<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>

      <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";
        String iID_MaDotPhanBo = Request.QueryString["iID_MaDotPhanBo"];
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        }
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String NamLamViec = Request.QueryString["NamLamViec"];
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DateTime.Now.Year.ToString();
        }
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

        //dot Phan Bo
        DataTable dtDotPhanBo = rptThongBaoCapChiTieuNganSachController.LayDotPhanBo(MaND,iID_MaTrangThaiDuyet);
        SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            if (dtDotPhanBo.Rows.Count > 0)
            {
                iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }

        }
        dtDotPhanBo.Dispose();
        DataTable dtNguonDonVi = DonViModels.Get_dtDonVi();
        SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtNguonDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtNguonDonVi.Dispose();
        using (Html.BeginForm("EditSubmit", "rptThongBaoCapChiTieuNganSach", new { ParentID = ParentID, iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaDonVi = iID_MaDonVi }))
        {%>
    <%=MyHtmlHelper.Hidden(ParentID,NamLamViec,"iNamLamViec","") %>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDotPhanBo, "iID_MaDotPhanBo", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
   
    <div>
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                    
                        <span>Báo cáo Thông báo cấp chỉ tiêu ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
     <%} %>
     <iframe src="<%=Url.Action("ViewPDF","rptThongBaoCapChiTieuNganSach",new{NamLamViec=NamLamViec,iID_MaDotPhanBo=iID_MaDotPhanBo,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%"></iframe>
</body>
</html>
