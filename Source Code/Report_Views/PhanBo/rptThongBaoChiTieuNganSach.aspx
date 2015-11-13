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
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "TongHopNganSach";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    String iID_MaDotPhanBo = Request.QueryString["iID_MaDotPhanBo"];
    String MaND = User.Identity.Name;
    String sLNS = Request.QueryString["sLNS"];
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
    // Dot phan bo
    DataTable dtDotPhanBo = rptTongHopNganSachController.LayDotPhanBo(NamLamViec);
    SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
    if (String.IsNullOrEmpty(iID_MaDotPhanBo))
    {
        iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
    }
    //Loai ngan sach
    DataTable dtLoaiNganSach = ReportModels.DtLoaiNganSach(MaND);
    SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "TenHT");
    if (String.IsNullOrEmpty(sLNS))
    {
        sLNS = Convert.ToString(dtLoaiNganSach.Rows[0]["TenHT"]);
    }
    using (Html.BeginForm("EditSubmit", "rptThongBaoChiTieuNganSach", new { ParentID = ParentID, iID_MaDotPhanBo = iID_MaDotPhanBo, sLNS = sLNS }))
    {
    %>   
     <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn năm làm việc")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Loại Ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiNganSach, sLNS, "TenHT", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <%=NgonNgu.LayXau("Chọn đọt phân bổ")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                               
                                <%=MyHtmlHelper.DropDownList(ParentID, slTenDotPhanBo, iID_MaDotPhanBo, "iID_MaDotPhanBo", "", "class=\"input1_2\" style=\"width: 30%\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                        </td>
                        <td class="td_form2_td5">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <div>
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%
        dtNam.Dispose();
        dtLoaiNganSach.Dispose();
        dtDotPhanBo.Dispose();
    %>
</body>
</html>
