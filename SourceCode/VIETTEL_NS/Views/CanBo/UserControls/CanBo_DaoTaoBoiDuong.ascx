<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    int i;
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    string iID_MaCanBo = Convert.ToString(props["iID_MaCanBo"].GetValue(Model));
    var dt = CanBo_QuaTrinhDaoTaoModels.Get_DanhSach(iID_MaCanBo);
   
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách quá trình đào tạo</span>
                </td>
                <td align="right">
                    <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Thêm quá trình đào tạo');">
                        <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "NS_QUATRINHDAOTAO", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo }, new AjaxOptions { }, new { @class = "button_title" })%>
</div> </td> </tr> </table> </div>
<table class="mGrid">
    <tr>
        <th style="width: 3%;" align="center">
            STT
        </th>
        <th style="width: 7%;" align="center">
            Nơi đào tạo
        </th>
        <th style="width: 13%;" align="center">
            Chuyên ngành
        </th>
        <th style="width: 7%;" align="center">
            Từ ngày
        </th>
        <th style="width: 5%;" align="center">
            Đến ngày
        </th>
        <th align="center">
            Hình thức đào tạo
        </th>
        <th style="width: 10%;" align="center">
            Văn bằng
        </th>
        <th style="width: 10%;" align="center">
            Kết quả đào tạo
        </th>
        <th style="width: 10%;" align="center">
            Trong/ngoài nước
        </th>
        <th style="width: 3%;" align="center">
            Sửa
        </th>
        <th style="width: 3%;" align="center">
            Xóa
        </th>
    </tr>
    <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;



            String strDelete = "";
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeNhanSu, MaND))
            {
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_DaoTaoBoiDuong", new { iID_MaQuaTrinhCongTac = R["iID_MaQuaTrinhCongTac"], iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }

            //gioi tinh
            String MaChuyenNganh = "", MaHinhThucDaoTao = "", MaVanBang = "", MaKetQuaDaoTao = "", iID_MaKetQuaDaoTao = "",
                  iID_MaChuyenNganh = "", iID_MaHinhThucDaoTao = "", iID_MaVanBang = "", TuNgay = "", DenNgay = "", TrongNuoc = "";
            //Đào tạo trong hay ngoài nước
            Boolean bTrongNuoc = Convert.ToBoolean(R["bTrongNuoc"]);
            if (bTrongNuoc) TrongNuoc = "Trong nước";
            else TrongNuoc = "Ngoài nước";


            MaChuyenNganh = HamChung.ConvertToString(R["iID_MaChuyenNganh"]);
            MaHinhThucDaoTao = HamChung.ConvertToString(R["iID_MaHinhThucDaoTao"]);
            MaVanBang = HamChung.ConvertToString(R["iID_MaVanBang"]);
            MaKetQuaDaoTao = HamChung.ConvertToString(R["iID_MaKetQuaDaoTao"]);
            if (String.IsNullOrEmpty(MaChuyenNganh) == false && MaChuyenNganh != "" && MaChuyenNganh != Convert.ToString(Guid.Empty))
            {
                iID_MaChuyenNganh = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaChuyenNganh)).Rows[0]["sTen"]);
            }
            if (String.IsNullOrEmpty(MaHinhThucDaoTao) == false && MaHinhThucDaoTao != "" && MaHinhThucDaoTao != Convert.ToString(Guid.Empty))
            {
                iID_MaHinhThucDaoTao = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaHinhThucDaoTao)).Rows[0]["sTen"]);
            }
            if (String.IsNullOrEmpty(MaVanBang) == false && MaVanBang != "" && MaVanBang != Convert.ToString(Guid.Empty))
            {
                iID_MaVanBang = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaVanBang)).Rows[0]["sTen"]);
            }
            if (String.IsNullOrEmpty(MaKetQuaDaoTao) == false && MaKetQuaDaoTao != "" && MaKetQuaDaoTao != Convert.ToString(Guid.Empty))
            {
                iID_MaKetQuaDaoTao = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaKetQuaDaoTao)).Rows[0]["sTen"]);
            }

            String dTuNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dTuNgay"]));
            if (dTuNgay == "01/01/0001") TuNgay = "";
            else TuNgay = dTuNgay;

            String dDenNgay = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dDenNgay"]));
            if (dDenNgay == "01/01/0001") DenNgay = "";
            else DenNgay = dDenNgay;

            String iID_MaQuaTrinhCongTac = HamChung.ConvertToString(R["iID_MaQuaTrinhCongTac"]);
    %>
    <tr <%=classtr %>>
        <td align="center">
            <%=STT%>
        </td>
        <td align="left">
            <%= HttpUtility.HtmlEncode(R["sNoiDaoTao"])%>
        </td>
        <td align="left">
            <%=HttpUtility.HtmlEncode(iID_MaChuyenNganh)%>
        </td>
        <td align="center">
            <%=TuNgay%>
        </td>
        <td align="center">
            <%=DenNgay%>
        </td>
        <td align="left">
            <%=HttpUtility.HtmlEncode(iID_MaHinhThucDaoTao)%>
        </td>
        <td align="left">
            <%=HttpUtility.HtmlEncode(iID_MaVanBang)%>
        </td>
        <td align="left">
            <%=HttpUtility.HtmlEncode(iID_MaKetQuaDaoTao)%>
        </td>
        <td align="left">
            <%= TrongNuoc%>
        </td>
        <td align="center">
            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT('Sửa quá trình đào tạo');">
                <%= Ajax.ActionLink("s", "Index", "NhapNhanh", new { id = "NS_QUATRINHDAOTAO", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaCanBo = iID_MaCanBo, iID_MaQuaTrinhCongTac = iID_MaQuaTrinhCongTac }, new AjaxOptions { }, new { @class = "icon_them" })%>
            </div>
        </td>
        <td align="center">
            <%=strDelete%>
        </td>
    </tr>
    <%} %>
</table>
</div>
<script type="text/javascript">
    function CallSuccess_CT() {
        $('#dvText').show();
        $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
        $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
        location.reload();
        return false;
    }
    function OnInit_CT(title) {
        $("#idDialog").dialog("destroy");
        document.getElementById("idDialog").title = title;
        document.getElementById("idDialog").innerHTML = "";
        $("#idDialog").dialog({
            resizeable: false,
            width: 600,
            modal: true
        });
    }
    function OnLoad_CT(v) {
        document.getElementById("idDialog").innerHTML = v;
    }

    $(document).ready(function () {
        //Hide the div tag when page is loading
        $('#dvText').hide();

        //For Show the div or any HTML element
        $("#btnLuu").click(function () {
            $('#dvText').show();
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
            alert(0);
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
<div id="idDialog" style="display: none;">
</div>
<div id="dvText" class="popup_block">
    <img src="../../../Content/ajax-loader.gif" /><br />
    <p>
        Hệ thống đang thực hiện yêu cầu...</p>
</div>
<% if (dt != null) dt.Dispose(); %>