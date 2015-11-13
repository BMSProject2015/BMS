<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String Loai = Convert.ToString(Request.QueryString["Loai"]);
    String MaQuanSoRaQuan = Convert.ToString(ViewData["MaQuanSoRaQuan"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    
    DataRow R;    
    String sLNS = "", dNgayRaQuan = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "", sThang = "", iID_MaDonVi = "";
    sThang = DateTime.Now.Month.ToString();
    
    DataTable dtThang = new DataTable();
    dtThang.Columns.Add("MaThang", typeof(String));
    dtThang.Columns.Add("TenThang", typeof(String));
    DataRow Row;
    for (int i = 0; i < 13; i++)
    {
        Row = dtThang.NewRow();
        dtThang.Rows.Add(Row);
        Row[0] = Convert.ToString(i);
        Row[1] = Convert.ToString(i);
    }
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

    using (Html.BeginForm("EditSubmit", "QuyetToan_QuanSo_RaQuan", new { ParentID = ParentID, MaQuanSoRaQuan = MaQuanSoRaQuan}))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_QuanSo_RaQuan"), "Chứng từ quyết toán")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 60%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                  
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang", "", "class=\"input1_2\" style=\"width:98%;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iThang")%>
                            </div>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
</asp:Content>



