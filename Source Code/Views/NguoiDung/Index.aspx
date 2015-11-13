<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%
    String MaNhomNguoiDung = Request.QueryString["MaNhomNguoiDung"];
    String TenNhomNguoiDung = "";
    SqlCommand cmd;
    cmd = new SqlCommand("SELECT * FROM QT_NguoiDung WHERE iID_MaNhomNguoiDung = @iID_MaNhomNguoiDung AND bHoatDong=1 ORDER BY sID_MaNguoiDung");
    cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
    DataTable dtNguoiDung = Connection.GetDataTable(cmd);



    cmd = new SqlCommand("SELECT * FROM QT_NhomNguoiDung WHERE iID_MaNhomNguoiDung = @iID_MaNhomNguoiDung");
    cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
    DataTable dtNhomNguoiDung = Connection.GetDataTable(cmd);
    TenNhomNguoiDung = Convert.ToString(dtNhomNguoiDung.Rows[0]["sTen"]);
    cmd.Dispose();
    dtNhomNguoiDung.Dispose();
    

    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, "QT_NguoiDung");
%>
    Thư mục: <%=MyHtmlHelper.ActionLink(Url.Action("Index", "NhomNguoiDung"), TenNhomNguoiDung)%>
    <h3>Danh sách người dùng</h3>
    
    <table cellpadding="0"  cellspacing="0" border="0" class="table_form3" >
        <tr>
            <td bgcolor="#2e6e9e">
                <%=MyHtmlHelper.ButtonLink(Url.Action("Register", "Account"), "Đăng ký người dùng mới")%>
            </td>
        </tr>
    </table>
    
    <table cellpadding="0"  cellspacing="0" border="0" class="table_form3" >
        <tr class="tr_form3">
            <td width="20px" align="center"><b>STT</b></td>
            <td align="center"><b>Tài khoản</b></td>
            <td width="150px" align="center"><b>Mật khẩu</b></td>
            <td width="100px" align="center"><b>Đã kích hoạt</b></td>
            <td width="100px" align="center"><b>Phân quyền</b></td>
            <td width="100px" align="center"><b>&nbsp;</b></td>
        </tr>
<%
    Boolean HoatDong;
    int i;
    string strTG;
    for (i = 0; i < dtNguoiDung.Rows.Count; i++)
    {
        %>
        <tr>
            <td><%=i+1 %></td>
            <td><%=dtNguoiDung.Rows[i]["sID_MaNguoiDung"]%></td>
            <td>
                <%=MyHtmlHelper.ActionLink(Url.Action("PasswordReset", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"] }).ToString(), "Thiết lập lại mật khẩu", "Edit", sDanhSachChucNangCam)%>
            </td>
            <td>
                <%
                    HoatDong = Convert.ToBoolean(dtNguoiDung.Rows[i]["bHoatDong"]);
                    if (HoatDong)
                    {
                        strTG = MyHtmlHelper.ActionLink(Url.Action("CapNhapKichHoat", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"], HoatDong = false , MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Hủy kích hoạt", "Edit", sDanhSachChucNangCam);
                    }   
                    else
                    {
                        strTG = MyHtmlHelper.ActionLink(Url.Action("CapNhapKichHoat", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"], HoatDong = true, MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Kích hoạt", "Edit", sDanhSachChucNangCam);
                    }   
                %>
                <%=strTG %>
            </td>
            <td>
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BangMau_NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"] }).ToString(), "Phân quyền", "Detail", sDanhSachChucNangCam)%>
            </td>
            <td>
                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"], MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Sửa", "Edit", sDanhSachChucNangCam)%>
                <%
                    strTG = "";
                    strTG = MyHtmlHelper.ActionLink(Url.Action("Delete", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"], MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "Xóa", "Delete", sDanhSachChucNangCam);
                    if (strTG != "")
                    {
                        %>
                            <br />
                            <%=strTG %>
                        <%
                    }   
                %>
                
            </td>
        </tr>
        <%
    }
    dtNguoiDung.Dispose();
%>
    </table>
</asp:Content>
