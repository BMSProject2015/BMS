<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_QuanLyDauTu.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String iID_MaDanhMucDuAn = Request.QueryString["iID_MaDanhMucDuAn"];
    String dNgayLap = Request.QueryString["dNgayLap"];

    if (iID_MaDanhMucDuAn == null) iID_MaDanhMucDuAn = "";
    if (dNgayLap == null) dNgayLap = CommonFunction.LayXauNgay(DateTime.Now); 

    DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
    SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
    dtDuAn.Dispose();

    NameValueCollection data = QLDA_TongDauTuDuToanModels.LayThongTin(iID_MaDanhMucDuAn, dNgayLap);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienGui_ChungTu"), "Danh sách chứng từ ghi sổ")%>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
    });            
</script>
<div id="ContainerPanel" class="ContainerPanel">
    <div id="pHeader" class="collapsePanelHeader"> 
        <div id="dvHeaderText" class="HeaderContent" style="width: 97%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Tìm kiếm thông tin tổng đầu tư tổng dự toán")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                            using (Html.BeginForm("SearchSubmit", "QLDA_TongDauTuDuToan", new { ParentID = ParentID }))
                            {       
                            %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="50%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 20%;"><div><b>Công trình dự án</b></div></td>
                                        <td class="td_form2_td5" style="width: 80%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slDuAn, iID_MaDanhMucDuAn, "iID_MaDanhMucDuAn_Search", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 20%;"><div><b>Tìm từ ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 80%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 20%;"><div><b>Tìm đến ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 80%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                            <td class="td_form2_td1" align="right"></td>
            	                             <td class="td_form2_td5">
            	                                <div style="text-align:right; float:right; width:100%">
                                                    <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	                                </div> 
            	                            </td>
                                        </tr>
                                        <tr><td class="td_form2_td1" align="right" colspan="2"></td></tr>
                                </table>
                            <%} %>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin tổng mức đầu tư - Tổng dự toán</span>
                        </td>
                        <td align="right">
                            <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table cellpadding="5" cellspacing="5" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 10%">
                                <div>
                                    <b>Mã công trình dự án</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5"  style="width: 50%">
                                <div>
                                    <script type="text/javascript">
                                        function ddlDuan_SelectedValueChanged(ctl) {
                                            var url = "<%=Url.Action("Index", "QLDA_TongDauTuDuToan", new { dNgayLap = dNgayLap})%>";
                                            if(ctl.selectedIndex>=0)
                                            {
                                                var value = ctl.options[ctl.selectedIndex].value;
                                                if(value!="")
                                                {
                                                    url += "&iID_MaDanhMucDuAn=" + value;
                                                }
                                            }
                                            location.href = url;
                                        }
                                    </script>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slDuAn, iID_MaDanhMucDuAn, "iID_MaDanhMucDuAn", "", "onChange=\"ddlDuan_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDanhMucDuAn")%>
                                </div>
                            </td>
                            <td class="td_form2_td1"  style="width: 10%">
                                <div>
                                    <b>Ngày lập</b>&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5"  style="width: 30%">
                                <div>
                                    <script type="text/javascript">
                                        function dNgayLap_SelectedValueChanged(ctl) {
                                            var url = "<%=Url.Action("Index", "QLDA_TongDauTuDuToan", new { iID_MaDanhMucDuAn = iID_MaDanhMucDuAn})%>";
                                            var value = document.getElementById("Edit_vidNgayLap").value;
                                            if(value!="")
                                            {
                                                url += "&dNgayLap=" + value;
                                            }
                                            location.href = url;
                                        }
                                    </script>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayLap, "dNgayLap", "", "onchange=\"dNgayLap_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                </div>
                            </td>
                        </tr>
                    </table>                    
                    <%Html.RenderPartial("~/Views/QLDA/TongDauTuDuToan/QLDA_TongDauTuDuToan_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>


