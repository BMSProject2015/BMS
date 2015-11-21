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
    string ParentID = "Edit";
    string UserID = User.Identity.Name;
    string ChiNganSach = Convert.ToString(ViewData["ChiNganSach"]);
    string MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
    string MaDotNganSach = Convert.ToString(ViewData["MaDotNganSach"]);
    string iKyThuat = Convert.ToString(ViewData["iKyThuat"]);
    string MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    string sLNS1 = "104";
    DataTable dtChungTu = DuToanBS_ChungTuModels.LayChungTu(MaChungTu);
    DataRow R;
    string iSoChungTu = "", dNgayChungTu = "", sNoiDung = "";
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToString(R["iSoChungTu"]);
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    string backURL = Url.Action("Index", "DuToanBS_ChungTu_BaoDam");
    using (Html.BeginForm("ThemSuaChungTu", "DuToanBS_ChungTu", new { ParentID = ParentID, MaChungTu = MaChungTu, sLNS1 = sLNS1, iKyThuat = iKyThuat }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", 0)%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span><%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>&nbsp; &nbsp;</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 50%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>Số chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=iSoChungTu%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Ngày chứng từ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>Nội dung</div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
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
                                            <input class="button" type="button" value="Hủy" onclick="Huy()" />
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
    dtChungTu.Dispose();
    }       
%>
<script type="text/javascript">
         function Huy() {
        window.parent.location.href = '<%=backURL %>';
</script>
</asp:Content>

