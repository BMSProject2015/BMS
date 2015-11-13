<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String MaND = User.Identity.Name;
    String sSoQuyetDinh = Request.QueryString["sSoQuyetDinh"];
    String dTuNgay = Request.QueryString["dTuNgay"];
    String dDenNgay = Request.QueryString["dDenNgay"];
    String sNguoiDung = Request.QueryString["sNguoiDung"];
  
    String page = Request.QueryString["page"];

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtNguoiDung = QLDA_TongDauTuModels.Get_NguoiTaoQuyetDinhTongDauTu();
    SelectOptionList slNguoiDung = new SelectOptionList(dtNguoiDung, "sID_MaNguoiDungTao", "sTen");
    dtNguoiDung.Dispose();
    //Xác định là nhóm trợ lý phòng ban là người tạo mới trong NS_PhanHe_TrangThaiDuyet
    bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeVonDauTu, MaND);
    if (isModify) sNguoiDung = User.Identity.Name;
    DataTable dt = QLDA_TongDauTuModels.Get_DanhSachTongDauTu_QuyetDinh(sSoQuyetDinh, dTuNgay, dDenNgay, sNguoiDung, CurrentPage, Globals.PageSize);

    double nums = QLDA_TongDauTuModels.Get_DanhSachTongDauTu_QuyetDinh_Count(sSoQuyetDinh, dTuNgay, dDenNgay, sNguoiDung);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", "QLDA_TongDauTu", new { sSoQuyetDinh = sSoQuyetDinh, TuNgay = dTuNgay, DenNgay = dDenNgay, sNguoiDung = sNguoiDung, page = x }));

    String strThemMoi = Url.Action("Edit", "QLDA_TongDauTu");

    using (Html.BeginForm("SearchSubmit", "QLDA_TongDauTu", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_TongDauTu"), "Danh sách quyết định tổng mức đầu tư")%>
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
                	<span>Thông tin tìm kiếm</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="50%">
                <tr>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Số quyết định</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <script type="text/javascript">
                                function sSoPheDuyet_UpperCase(ctl) {
                                    ctl.value = ctl.value.toUpperCase();
                                }
                            </script>
                            <%=MyHtmlHelper.TextBox(ParentID, sSoQuyetDinh, "sSoQuyetDinh", "", "onblur=\"sSoPheDuyet_UpperCase(this);\" class=\"input1_2\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 10%;">
                    <%if(!isModify)
                      {%>
                            <div><b>Người nhập</b></div></td>
                            <%} %>
                    <td class="td_form2_td5" style="width: 40%;">
                     <%if (!isModify)
                       {%>
                        <div>
                             <%=MyHtmlHelper.DropDownList(ParentID, slNguoiDung, sNguoiDung, "sNguoiDung", "", "class=\"input1_2\"")%>
                        </div>
                        <%} %>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm từ ngày</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm đến ngày</b></div></td>
                    <td class="td_form2_td5" style="width: 40%;">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="td_form2_td1" align="right">
            	        <div style="text-align:right; float:right; width:100%">
                            <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	        </div> 
            	    </td>
                </tr>
                <tr><td class="td_form2_td1" align="right" colspan="4"></td></tr>
            </table>
        </div>
    </div>
</div>
<%  } %>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Danh sách quyết định tổng mức đầu tư</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="TaoMoi" type="button" class="button_title" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 5%;" align="center">Số quyết định</th>
            <th style="width: 5%;" align="center">Số quyết định trước</th>
            <th style="width: 10%;" align="center">Ngày quyết định</th>
            <th style="width: 10%;" align="center">Cấp phê duyệt</th>
            <th style="width: 30%;" align="center">Nội dung</th>
            <th style="width: 10%;" align="center">Ngày tạo</th>
            <th style="width: 10%;" align="center">Người tạo</th>
            <th style="width: 5%;" align="center">Sửa quyết định</th>
            <th style="width: 5%;" align="center">Xóa quyết định</th>
        </tr>
        <%
        int i;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            String sTrangThai = "";
            String sClasstr = "";
                        
            //Ngày tạo tổng đầu tư 
            String dNgayQuyetDinh = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQuyetDinh"]));
            String dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]));
            if (i % 2 == 0) sClasstr = "alt";
            String strEdit = "";
            String strDelete = "";
            strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_TongDauTu", new { iID_MaTongDauTu_QuyetDinh = R["iID_MaTongDauTu_QuyetDinh"], sSoQuyetDinh = R["sSoQuyetDinh"].ToString() }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_TongDauTu", new { iID_MaTongDauTu_QuyetDinh = R["iID_MaTongDauTu_QuyetDinh"], sSoQuyetDinh = R["sSoQuyetDinh"].ToString() }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            %>
            <tr <%=sClasstr %>>
                <td align="center"><%=R["rownum"]%></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_TongDauTu", new { iID_MaTongDauTu_QuyetDinh = R["iID_MaTongDauTu_QuyetDinh"].ToString(), sSoQuyetDinh = R["sSoQuyetDinh"].ToString() }).ToString(), R["sSoQuyetDinh"].ToString(), "Detail", "")%></b>
                </td>    
                <td align="center"><b><%=R["sSoQuyetDinh_Cu"]%></b></td>
                <td align="left"><%=dNgayQuyetDinh%></td>
                <td align="left"><%=R["sCapPheDuyet"]%></td>
                 <td align="left"><%=R["sNoiDung"]%></td>
                <td align="left"><%=dNgayLap%></td>
                <td align="left"><%=R["sID_MaNguoiDungTao"]%></td>
                <td align="center"><%=strEdit%></td>
                <td align="center"><%=strDelete%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="10" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
%>
<script type="text/javascript">
    $(document).ready(function () {
        //Hide the div tag when page is loading
        $('#dvText').hide();

        //For Show the div or any HTML element
        $("#btnSearch").click(function () {
            $('#dvText').show();
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
        });

        //For hide the div or any HTML element
        $("#aHide").click(function () {
            $('#dvText').hide();
        });

        $(window).resize(function () {
            $('.popup_block').css({
                position: 'absolute',
                left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                top: ($(window).height() - $('.popup_block').outerHeight()) / 2
            });
        });
        // To initially run the function:
        $(window).resize();
        //Fade in Background
    });                                 
</script>
<div id="dvText" class="popup_block">
    <img src="../../../Content/ajax-loader.gif"/><br />
    <p>Hệ thống đang thực hiện yêu cầu...</p>
</div>
</asp:Content>

