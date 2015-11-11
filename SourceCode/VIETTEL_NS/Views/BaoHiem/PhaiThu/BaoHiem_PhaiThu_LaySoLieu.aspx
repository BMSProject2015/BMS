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
    String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    
    String    sTienToChungTu = PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeBaoHiem);
    int iSoChungTu = BaoHiem_PhaiThuModels.GetMaxChungTu() + 1;
    
        
    DataTable dtThang = DanhMucModels.DT_Thang(false);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

    using (Html.BeginForm("EditSubmit_LaySLieu", "BaoHiem_PhaiThu", new { ParentID = ParentID, iID_MaPhongBan = iID_MaPhongBan }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>


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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThu"), "Bảo hiểm")%>
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
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Lấy dữ liệu từ quyết toán")%>
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
                            <div><b>Từ Tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>                                                         
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, "", "iTuThang_Quy", "", " onchange=\"validmonth();\" class=\"input1_2\" style=\"width:17%;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iTuThang_Quy")%>
                            </div>
                        </td>
                    </tr>                   
                    <tr>
                       <td class="td_form2_td1">
                            <div><b>Đến Tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>                                                         
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, "", "iDenThang_Quy", "", "onchange=\"validmonth();\" class=\"input1_2\" style=\"width:17%;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iDenThang_Quy")%>
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
<script type="text/javascript">
    function validmonth() {
        var TuThang = document.getElementById('<%=ParentID %>_iTuThang_Quy').value;
        var DenThang = document.getElementById('<%=ParentID %>_iDenThang_Quy').value;
        if (TuThang > DenThang) {
            alert("Từ tháng phải nhỏ hơn đến tháng!");
            document.getElementById('<%=ParentID %>_iTuThang_Quy').value = 1;
        }
    }
</script>
<%
    }       
%>
</asp:Content>



