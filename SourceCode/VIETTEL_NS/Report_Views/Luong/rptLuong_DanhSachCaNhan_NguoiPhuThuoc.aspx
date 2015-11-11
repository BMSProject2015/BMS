<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DANH SÁCH CÁ NHÂN VÀ SỐ NGƯỜI PHỤ THUỘC</title>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "Luong";
        String iThangLuong = Request.QueryString["Thang"];
        String UserID = User.Identity.Name;
        if (String.IsNullOrEmpty(iThangLuong))
        {
            iThangLuong = DanhMucModels.ThangLamViec(UserID).ToString();
        }
        //tháng     
        var dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        String iNamLuong = Request.QueryString["Nam"];

        if (String.IsNullOrEmpty(iNamLuong))
        {
            iNamLuong = DanhMucModels.NamLamViec(UserID).ToString();
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
        String URL = Url.Action("Index", "Luong_Report");
        String MaBangLuong = Convert.ToString(Request.QueryString["iID_MaBangLuong"]);
        if (String.IsNullOrEmpty(MaBangLuong))
        {
            MaBangLuong = Guid.Empty.ToString();
        }
        using (Html.BeginForm("EditSubmit", "rptLuong_DanhSachCaNhan_NguoiPhuThuoc", new { ParentID = ParentID }))
        {
    %>
    <div id="Div1">
        <div id="Div2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2"
                style="display: none;">
                <tr>
                    <td class="td_form2_td1" style="width: 113px">
                    </td>
                    <td class="td_form2_td1" style="width: 113px">
                    </td>
                    <td class="td_form2_td1" style="width: 70px">
                        <div>
                            <%=NgonNgu.LayXau("Chọn tháng")%>
                        </div>
                    </td>
                    <td class="td_form2_td5" style="width: 115px">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangLuong, "iThangLuong", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 70px">
                        <div>
                            <%=NgonNgu.LayXau("Chọn năm")%></div>
                    </td>
                    <td class="td_form2_td5" style="width: 113px">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLuong, "iNamLuong", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 113px">
                    </td>
                    <td class="td_form2_td1" style="width: 113px">
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="text-align: center;" colspan="8">
                        <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 2px auto;
                            width: 200px;">
                            <tr>
                                <td>
                                    <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                </td>
                                <td width="2px">
                                </td>
                                <td>
                                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptLuong_DanhSachCaNhan_NguoiPhuThuoc", new { MaBL = MaBangLuong}), "Xuất ra file Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptLuong_DanhSachCaNhan_NguoiPhuThuoc", new{MaBL=MaBangLuong})%>"
        height="600px" width="100%"></iframe>
</body>
</html>
