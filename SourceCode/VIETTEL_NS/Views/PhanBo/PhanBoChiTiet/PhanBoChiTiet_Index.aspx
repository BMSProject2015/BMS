<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsPhanBo.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>

    <%
    String iID_MaPhanBo = Request.QueryString["iID_MaPhanBo"];
    if (String.IsNullOrEmpty(iID_MaPhanBo)) iID_MaPhanBo = Convert.ToString(ViewData["iID_MaPhanBo"]);
    NameValueCollection data = PhanBo_PhanBoModels.LayThongTin(iID_MaPhanBo);
    %>
    <div style="width: 100%; float: left; margin-top: 10px;">
        <div style="width: 100%; float:left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thông tin chứng từ</span>
                            </td>
                            <td align="right"><span>F2 - Thêm</span></td>
                            <td align="right" style="width:100px;"><span>Delete - Xóa</span></td>                        
                            <td align="left"><span>F10 - Lưu</span></td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%">
                                    <div><b>Đợt chỉ tiêu</b></div>
                                </td>
                                <td class="td_form2_td5" style="width: 20%"><div>
                                    <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayDotPhanBo"]))%></div>
                                </td>
                                <td class="td_form2_td1" style="width: 15%">
                                    <div><b>Ngày chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5" style="width: 20%"><div>
                                    <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%"><div><b>Số phân bổ</b></div></td>
                                <td class="td_form2_td5" style="width: 20%"><div><%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></div></td>                            
                                <td class="td_form2_td1" style="width: 15%"><div><b>Nội dung</b></div></td>
                                <td class="td_form2_td5" style="width: 20%"><div><%=data["sNoiDung"]%></div></td>
                            </tr>
                        </table>
                        <%Html.RenderPartial("~/Views/PhanBo/PhanBoChiTiet/PhanBoChiTiet_Index_DanhSach.ascx", new { ControlID = "PhanBoChiTiet", MaND = User.Identity.Name }); %>    
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            jsPhanBo_Url_Frame = '<%=Url.Action("PhanBoChiTiet_Frame", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = iID_MaPhanBo })%>';
            jsPhanBo_Url = '<%=Url.Action("Index", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = iID_MaPhanBo})%>';
        });
	</script>   
</asp:Content>
