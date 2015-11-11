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
    String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtChungTu = TCDN_KinhDoanh_ChungTuModels.GetChungTu(MaChungTu);
    DataRow R;
    int iSoChungTu = 0, iQuy = 0;
    String sLNS = "", sTienToChungTu = "", dNgayChungTu = "", sNoiDung = "", iID_MaTrangThaiDuyet = "", iID_MaDoanhNghiep = "";
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        iID_MaDoanhNghiep = Convert.ToString(R["iID_MaDoanhNghiep"]);
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);
        iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);
        iQuy = Convert.ToInt32(R["iQuy"]);
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    if (ViewData["DuLieuMoi"] == "1")
    {
        sTienToChungTu = PhanHeModels.LayTienToChungTu(TCDNModels.iID_MaPhanHe);
        iSoChungTu = TCDN_KinhDoanh_ChungTuModels.GetMaxChungTu() + 1;
    }
    dtChungTu.Dispose();

    DataTable dtDoanhNghiep = TCSN_DoanhNghiepModels.Get_ListDoanhNghiep(true);
    SelectOptionList slDoanhNghiep = new SelectOptionList(dtDoanhNghiep, "iID_MaDoanhNghiep", "sTenDoanhNghiep");
    dtDoanhNghiep.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    using (Html.BeginForm("EditSubmit", "TCDN_KinhDoanh_ChungTu_ImportExcel", FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<%= Html.Hidden(ParentID + "_iID_MaChungTu", MaChungTu)%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_KinhDoanh_ChungTu"), "Danh sách chứng từ hoạt động kinh doanh")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span><%=NgonNgu.LayXau("Thêm mới chứng từ")%></span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 60%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Đơn vị</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDoanhNghiep, iID_MaDoanhNghiep, "iID_MaDoanhNghiep", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 30%;">
                            <div><b>Số chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <b><%=sTienToChungTu%><%=iSoChungTu%></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Quý chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Convert.ToString(iQuy), "iQuy", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iQuy")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Ngày chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Nội dung</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Tải tệp excel</b></div>
                        </td>
                        <td class="td_form2_td5">
                        <div>
                            <input type="file"   id="<%=ParentID %>_sFileName" name="<%=ParentID %>_sFileName" style="width:100%"/>
                            </div>
                        </td>
                        <%--<td class="td_form2_td5">
                            <% =MyHtmlHelper.UploadFile("upload", "Libraries/Excels", DateTime.Now.ToString("HHmmss"))%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sFileName")%>
                        </td>--%>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                        </td>
                        <td class="td_form2_td5">
                            <a href="../../Libraries/Excels/TCDNMAU/MauNhapExcel-HoatDongKinhDoanh.xls">
                                    Mẫu nhập excel</a>
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
                                            <input type="submit" class="button" id="btnLuu" value="Lưu" />
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
            <div class="box_tong" style="display:none" id="<%= ParentID %>_ChiTiet">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span><%=NgonNgu.LayXauChuHoa("Thông tin chi tiết")%></span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="form_nhap">
                    <div class="form2">
                        <div class="content" style="overflow:auto" id="<%= ParentID %>_tdList">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%--
<%= Html.Hidden(ParentID + "_sFileName")%>
<%= Html.Hidden(ParentID + "_sDuongDan")%>--%>

<script type="text/javascript">                            
//upload.addFilter("Documents (*.xls)", "*.xls");                                        
//upload.addListener(upload.UPLOAD_COMPLETE, <%= ParentID%>_uploadFile);                                            
function <%=ParentID%>_uploadFile(filename, url) {                                                                                        
    document.getElementById("<%= ParentID%>_sFileName").value = filename;
    document.getElementById("<%= ParentID%>_sDuongDan").value = upload.serverPath + "/" + url;
    XemDuLieu();
}
function XemDuLieu() {
    var FilePath, SheetName;
    var url = unescape('<%= Url.Action("get_dtSheet?ParentID=#0&DuongDan=#1", "TCDN_KinhDoanh_ChungTu_ImportExcel") %>');
    FileName = document.getElementById('<%= ParentID %>_sFileName').value;
    if(FileName == ""){
        alert("Bạn chưa chọn file excel");
        return false;
    }
    DuongDan = document.getElementById('<%= ParentID %>_sDuongDan').value;
    url = unescape(url.replace("#0", "<%= ParentID %>"));
    url = unescape(url.replace("#1", DuongDan));
    $.getJSON(url, function (data) {
        document.getElementById("<%=ParentID%>_ChiTiet").style.display = 'block';
        document.getElementById("<%= ParentID %>_tdList").innerHTML = data.sData;
    });
}
</script>
<%
    }       
%>
<script type="text/javascript">
    $(document).ready(function () {
        //Hide the div tag when page is loading
        $('#dvText').hide();

        //For Show the div or any HTML element
        $("#btnLuu").click(function () {
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





