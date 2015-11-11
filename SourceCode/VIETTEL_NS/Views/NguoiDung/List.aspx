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
    //String MaNhomNguoiDung = Request.QueryString["MaNhomNguoiDung"];
    SqlCommand cmd;
    cmd = new SqlCommand("SELECT * FROM QT_NguoiDung WHERE iTrangThai=1 AND ( iID_MaNhomNguoiDung = '1' OR LEFT(iID_MaNhomNguoiDung,2)='1-') ORDER BY iID_MaNhomNguoiDung");
    DataTable dtNguoiDung = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, "QT_NguoiDung");
%>
      
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách đơn vị</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <%=MyHtmlHelper.ButtonLink(Url.Action("Register", "Account"), "Đăng ký người dùng mới", null, null, "class=\"button_title\"")%>
                </td>
            </tr>
        </table>
    </div>   
    <table class="mGrid">
        <tr>
            <th width="3%" align="center">STT</th>
            <th align="center">Nhóm người dùng</th>
            <th width="250px" align="center">Tài khoản</th>
            <th width="150px" align="center">Mật khẩu</th>
            <th width="100px" align="center">Đã kích hoạt</th>
            <%--<td width="100px" align="center"><b>Phân quyền</b></td>--%>
            <th width="5%" align="center">Sửa</th>
            <th width="5%" align="center">Xóa</th>
        </tr>
<%
    Boolean HoatDong;
    int i;
    string strTG;
    for (i = 0; i < dtNguoiDung.Rows.Count; i++)
    {
        String TenNhomNguoiDung = "", TenNhomNguoiDungLay = "" ;
        String MaNhomNguoiDung = Convert.ToString(dtNguoiDung.Rows[i]["iID_MaNhomNguoiDung"]);
        cmd = new SqlCommand("SELECT sTen FROM QT_NhomNguoiDung WHERE iID_MaNhomNguoiDung = @iID_MaNhomNguoiDung");
        cmd.Parameters.AddWithValue("@iID_MaNhomNguoiDung", MaNhomNguoiDung);
        TenNhomNguoiDung = Convert.ToString(Connection.GetValue(cmd, ""));
        cmd.Dispose();

        int j;
        int itg1 = 0, itg2;
        //itg1 = CString.DemKyTu(MaNhomNguoiDung, '-');
        string strDoanTrang = "";
        strDoanTrang = "";
        itg2 = CString.DemKyTu(MaNhomNguoiDung, '-');
        for (j = itg1 + 1; j <= itg2; j++)
        {
            strDoanTrang += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        TenNhomNguoiDungLay = strDoanTrang + TenNhomNguoiDung;

        String classtr = "";
        int STT = i + 1;
        if (i % 2 == 0)
        {
            classtr = "class=\"alt\"";
        }
        %>
        <tr <%=classtr %>>
            <td><%=STT%></td>
            <td><%=TenNhomNguoiDungLay%></td>
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
            <%--<td>
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BangMau_NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"] }).ToString(), "Phân quyền", "Detail", sDanhSachChucNangCam)%>
            </td>--%>
            <td align="center">
                <%
                    strTG = "";
                    strTG = MyHtmlHelper.ActionLink(Url.Action("Edit", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", sDanhSachChucNangCam);
                    if (strTG != "")
                    {
                        %>
                            <%=strTG %>
                        <%
                    }   
                %>          
            </td>
            <td align="center">
                <%
                    strTG = "";
                    strTG = MyHtmlHelper.ActionLink(Url.Action("Delete", "NguoiDung", new { MaNguoiDung = dtNguoiDung.Rows[i]["sID_MaNguoiDung"], MaNhomNguoiDung = MaNhomNguoiDung }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", sDanhSachChucNangCam);
                    if (strTG != "")
                    {
                        %>
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
</div>
</asp:Content>
