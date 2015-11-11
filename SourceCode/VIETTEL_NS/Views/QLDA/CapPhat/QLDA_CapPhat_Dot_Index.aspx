<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    
    String ParentID = "QLDA_CapPhat";
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

    int iDotMoi = 0;
    if (QLDA_CapPhatModels.Get_Max_Dot(NamLamViec) != "") { 
        iDotMoi = Convert.ToInt32(QLDA_CapPhatModels.Get_Max_Dot(NamLamViec)) + 1;
    };
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }

    using (Html.BeginForm("AddNewSubmit", "QLDA_CapPhat", new { ParentID = ParentID }))
    {
%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_CapPhat"), "Đợt cấp phát")%>
            </div>
        </td>
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Chọn đợt hoặc thêm mới đợt cấp phát</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" width="50%" class="table_form2">
                <tr>
                    <td style="width: 50%">
                        <table cellpadding="0" cellspacing="0" width="50%" class="table_form2">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Thêm đợt mới")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.CheckBox(ParentID, iThemMoi, "iThemMoi", "", "onclick=\"CheckThemMoi(this.checked)\"")%></div>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2" id="tb_DotNganSach">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Số cấp phát")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.Label(iDotMoi,"iDot")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Ngày")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayLap", "", "class=\"input1_2\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Nội dung cấp phát")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextArea(ParentID, "", "sNoiDungCapPhat", "", "class=\"input1_2\" style=\"height:60px\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDungCapPhat")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;"><div></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                                <td width="30%" align="right" class="td_form2_td5">
                                                    <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thêm mới")%>" />
                                                </td>          
                                                    <td width="5px">&nbsp;</td>          
                                                <td class="td_form2_td5">
                                                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%} %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách đợt cấp phát</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="<%= ParentID %>_divDotNganSach">
        <%=QLDA_CapPhatController.get_NgayDotNganSach(ParentID, NamLamViec, User.Identity.Name)%>
    </div>
</div>
<script type="text/javascript">
    CheckThemMoi(false);
    function CheckThemMoi(value) {
        if (value == true) {
            document.getElementById('tb_DotNganSach').style.display = ''
        } else {
            document.getElementById('tb_DotNganSach').style.display = 'none'
        }
    }

    function Chon_LNS(sLNS) {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("get_objDotNganSach?ParentID=#0&NamLamViec=#1MaND=#2", "QLDA_CapPhat")%>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", <%=NamLamViec %>));
        url = unescape(url.replace("#2", <%=User.Identity.Name%>));
        $.getJSON(url, function (data) {
            document.getElementById("<%= ParentID %>_divDotNganSach").innerHTML = data;
        });
    }        
        
</script>
</asp:Content>
