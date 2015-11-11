<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/QLDA/jsBang_QLDA_KeHoachVon.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        String ParentID = "Edit";
        String iID_KeHoachVon_QuyetDinh = Request.QueryString["iID_KeHoachVon_QuyetDinh"];
        String dNgayKeHoachVon = Request.QueryString["dNgayKeHoachVon"];
        String iLoaiKeHoachVon = Request.QueryString["iLoaiKeHoachVon"];

        // if (dNgayKeHoachVon == null) dNgayKeHoachVon = CommonFunction.LayXauNgay(DateTime.Now);
        if (iLoaiKeHoachVon == null) iLoaiKeHoachVon = "";



        DataTable dtLoaiKeHoachVon = QLDA_KeHoachVonModels.DT_LoaiKeHoachVon();
        SelectOptionList slLoaiKeHoachVon = new SelectOptionList(dtLoaiKeHoachVon, "iID_MaLoaiKeHoachVon", "sTen");
        dtLoaiKeHoachVon.Dispose();

        NameValueCollection data = QLDA_KeHoachVonModels.LayThongTin(iID_KeHoachVon_QuyetDinh);
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_KeHoachVon"), "Danh sách Kế hoạch vốn")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
    <div id="ContainerPanel" class="ContainerPanel" style="display: none;">
        <div id="pHeader" class="collapsePanelHeader">
            <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
                <div style="width: 100%; float: left;">
                    <span>
                        <%=NgonNgu.LayXau("Tìm kiếm thông tin hợp đồng")%></span>
                </div>
            </div>
            <div id="dvArrow" class="ArrowExpand">
            </div>
        </div>
        <div id="dvContent" class="Content" style="display: none">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%;">
                        <div id="nhapform">
                            <div id="form2">
                                <%
                                    using (Html.BeginForm("SearchSubmit", "QLDA_KeHoachVon", new { ParentID = ParentID }))
                                    {       
                                %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="100%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Loại kế hoạch vốn</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKeHoachVon, "", "iLoaiKeHoachVon_Search", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b></b>
                                            </div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm từ ngày</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm đến ngày</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td5" colspan="4">
                                            <div style="text-align: right; float: right; width: 100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float: right; margin-left: 10px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" align="right" colspan="4">
                                        </td>
                                    </tr>
                                </table>
                                <%} %>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thông tin kế hoạch vốn</span>
                            </td>
                            <td align="right">
                                <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin -- Space: Sửa thông tin</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <div>
                            <table cellpadding="5" cellspacing="5" width="100%" border="0">
                                <tr>
                                    <td class="td_form2_td1" style="width: 120px;">
                                        <div>
                                            <b>Số quyết định</b>&nbsp;<span style="color: Red;">*</span>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 100px;">
                                        <div>
                                            <%=data["sSoQuyetDinh"]%>
                                        </div>
                                    </td>
                                    <td class="td_form2_td1" style="width: 120px;">
                                        <div>
                                            <b>Ngày quyết định</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td  class="td_form2_td5" style="width: 90px;">
                                        <div>
                                            <%=CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayQD"]))%>
                                        </div>
                                    </td>
                                    <td class="td_form2_td1" style="width: 80px;">
                                        <div>
                                            <b>Ngày lập</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td  class="td_form2_td5" style="width: 90px;">
                                        <div>
                                            <%=CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayKeHoachVon"]))%>
                                        </div>
                                    </td>
                                    <td class="td_form2_td1" style="width: 130px;">
                                        <div>
                                            <b>Loại kế hoạch vốn</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td  class="td_form2_td5" style="width: 130px;">
                                        <div>
                                            <%=data["sTen"]%>
                                        </div>
                                    </td>
                               
                                    <td class="td_form2_td1" style="width: 80px;">
                                        <div>
                                            <b>Nội dung</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td   class="td_form2_td5" style="width: 250px;">
                                        <div>
                                            <%=data["sNoiDung"]%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table cellpadding="5" cellspacing="5" width="100%" style="display: none;">
                            <tr>
                                <td class="td_form2_td1" style="width: 10%">
                                    <div>
                                        <b>Loại kế hoạch vốn</b>&nbsp;<span style="color: Red;">*</span></div>
                                </td>
                                <td class="td_form2_td5" style="width: 40%">
                                    <div>
                                        <script type="text/javascript">
                                        function ddlLoaiKeHoachVon_SelectedValueChanged(ctl) {
                                            var url = "<%=Url.Action("Index", "QLDA_KeHoachVon")%>";
                                            if(ctl.selectedIndex>=0)
                                            {
                                                var value = ctl.options[ctl.selectedIndex].value;
                                                if(value!="")
                                                {
                                                    url += "?iLoaiKeHoachVon=" + value;
                                                }
                                            }
                                             var NgayKHV = document.getElementById("Edit_vidNgayKeHoachVon").value;
                                            if(NgayKHV!="")
                                                {
                                                     url += "&dNgayKeHoachVon=" + NgayKHV;
                                                 location.href = url;

                                                }
                                        }
                                        </script>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKeHoachVon, iLoaiKeHoachVon, "iLoaiKeHoachVon", "", "onChange=\"ddlLoaiKeHoachVon_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                    </div>
                                </td>
                                <td class="td_form2_td1" style="width: 10%">
                                    <div>
                                        <b>Ngày lập</b>&nbsp;<span style="color: Red;">*</span></div>
                                </td>
                                <td class="td_form2_td5" style="width: 40%">
                                    <div>
                                        <script type="text/javascript">
                                        function dNgayLap_SelectedValueChanged(ctl) {
                                            var iLoaiKHV=document.getElementById("Edit_iLoaiKeHoachVon").value;
                                            if(iLoaiKHV=="-1")
                                            {
                                                alert("Bạn chưa chọn loại kế hoạch vốn");
                                               document.getElementById("Edit_vidNgayKeHoachVon").value="";
                                                return;
                                             }
                                            var url = "<%=Url.Action("Index", "QLDA_KeHoachVon")%>";
                                            var value = document.getElementById("Edit_vidNgayKeHoachVon").value;
                                            if(value!="")
                                            {
                                                url += "?dNgayKeHoachVon=" + value;
                                            }

                                            if(iLoaiKHV!="")
                                                {
                                                    url += "&iLoaiKeHoachVon=" + iLoaiKHV;
                                                }
                                            location.href = url;
                                        }
                                        </script>
                                        <%=MyHtmlHelper.DatePicker(ParentID, dNgayKeHoachVon, "dNgayKeHoachVon", "", "onchange=\"dNgayLap_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <%Html.RenderPartial("~/Views/QLDA/KeHoachVon/QLDA_KeHoachVon_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
