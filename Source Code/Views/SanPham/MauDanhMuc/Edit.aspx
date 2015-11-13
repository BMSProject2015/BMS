<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String iID_MaDanhMuc = Convert.ToString(ViewData["iID_MaDanhMuc"]);
        String iID_MaDanhMucCha = Convert.ToString(ViewData["iID_MaDanhMucCha"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "";
        
        //chi tiết chỉ tiêu nếu trong trường hợp sửa
        SqlCommand cmd = new SqlCommand("SELECT * FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
        cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iID_MaDanhMuc);
        DataTable dt = Connection.GetDataTable(cmd);
        DataRow R;
        if (dt.Rows.Count > 0 && iID_MaDanhMuc != null && iID_MaDanhMuc != "")
        {
            R = dt.Rows[0];
            sKyHieu = HamChung.ConvertToString(R["sTenKhoa"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
        }

        using (Html.BeginForm("EditSubmit", "SanPham_MauDanhMuc", new { ParentID = ParentID, iID_MaDanhMuc = iID_MaDanhMuc }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaDanhMucCha", iID_MaDanhMucCha)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_MauDanhMuc"), "Cấu hình danh mục")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span>Nhập thông tin khoản mục</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin khoản mục</span>
                    <% } %>
                       
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã khoản mục</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%
                                String strReadonly = "";
                                //if (ViewData["DuLieuMoi"] == "0") {
                                //    strReadonly = "readonly=\"readonly\""; 
                                //}    
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sTenKhoa", "", " " + strReadonly + " class=\"input1_2\" tab-index='-1'", 2)%><br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên khoản mục</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose();};    
    %>
</asp:Content>
