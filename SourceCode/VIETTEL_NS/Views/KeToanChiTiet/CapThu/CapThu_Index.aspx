<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "KTTG_ChungTuCapThu_Duyet";
    String NamLamViec = Request.QueryString["NamLamViec"];
    String page = Request.QueryString["page"];
    String sNguoiDung = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sNguoiDung);
    if(NamLamViec == null || NamLamViec == ""){
        NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    }
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_DanhSachChungTu(NamLamViec, sNguoiDung, CurrentPage, Globals.PageSize);

    double nums = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_DanhSachChungTu_Count(NamLamViec, User.Identity.Name);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "KTCT_CapThu", new { NamLamViec = NamLamViec, sNguoiDung = sNguoiDung, page = x }));

    int iDotMoi = 0;
    if (KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec) != "")
    {
        iDotMoi = Convert.ToInt32(KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec)) + 1;
    };
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }

    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(User.Identity.Name);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    DataTable dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoan(true, "--- Tài khoản kế toán ---","");
    SelectOptionList stTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
    dtTaiKhoan.Dispose();

    using (Html.BeginForm("Search", "KTCT_CapThu", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
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
        <div id="dvHeaderText" class="HeaderContent" style="width: 90%;">
            <div style="width: 100%; float: left;">
                <span>1. Thông tin tìm kiếm chứng từ</span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 100%;">
                    <div id="nhapform">
                        <div id="form2">
                            <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                <tr>
                                    <td style="width: 100%">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2" id="tb_DotNganSach">
                                            <tr>
                                                <td valign="top" style="width: 50%">
                                                    <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                        <tr>
                                                            <td class="td_form2_td1" style="width: 15%;">
                                                                <div><%=NgonNgu.LayXau("Đơn vị")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                                                </div>
                                                            </td>
                                                        </tr>   
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Từ ngày")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\" style=\"height:18px;\" onblur=isDate(this);")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Tài khoản nợ")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_No", "", "class=\"input1_2\"")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Đơn vị nợ")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_No", "", "class=\"input1_2\"")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" style="width: 50%">
                                                    <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                        <tr>
                                                            <td class="td_form2_td1" style="width: 15%;">
                                                                <div><%=NgonNgu.LayXau("Số ủy nhiệm chi")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.TextBox(ParentID, "", "sSoChungTu", "", "class=\"input1_2\" style=\"height:18px;\"")%>
                                                                </div>
                                                            </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Đến ngày")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\" style=\"height:18px;\" onblur=isDate(this);")%>
                                                                </div>
                                                            </td>
                                                        </tr>   
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Tài khoản có")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_Co", "", "class=\"input1_2\"")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="td_form2_td1">
                                                                <div><%=NgonNgu.LayXau("Đơn vị có")%></div>
                                                            </td>
                                                            <td class="td_form2_td5">
                                                                <div>
                                                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_Co", "", "class=\"input1_2\"")%>
                                                                </div>
                                                            </td>
                                                        </tr> 
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1" colspan="2" align="right" width="100%">
                                                    <div style="float: right;">
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>  
                                                                <td align="right" class="td_form2_td1">
                                                                    <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Tìm kiếm")%>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<%}
    DataTable dtChungTuCapPhat = CapPhat_ChungTuModels.GetDanhSachCapPhat(NamLamViec);
    SelectOptionList slChungTuCapPhat = new SelectOptionList(dtChungTuCapPhat, "iID_MaCapPhat", "TENHT");
    dtChungTuCapPhat.Dispose();
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 48%">
                    <span>2. Danh sách chứng từ duyệt cấp thu</span>
                </td>
                <td align="right" style="width: 50%;">
                    <%
                    String strThemMoi = Url.Action("Edit", "KTCT_CapThu");
                    using (Html.BeginForm("Edit", "KTCT_CapThu", new { ParentID = ParentID }))
                    {
                    %>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 85%"><%=MyHtmlHelper.DropDownList(ParentID, slChungTuCapPhat, "", "iID_MaCapPhat", "", "class=\"input1_2\"")%></td>
                            <td style="width: 15%"><input id="TaoMoi" type="submit" class="button" value="Tạo mới"/></td>
                        </tr>
                    </table>
                    <%} %>
                </td>
            </tr>
        </table>
    </div>
    <div>
    <table class='mGrid'>
        <tr>
            <th style="width: 3%" align="center">STT</th>
            <th style="width: 10%;" align="center">Năm làm việc</th>       
            <th style="width: 10%;" align="center">Số UNC/RDT</th>
            <th style="width: 10%;" align="center">Tổng cấp</th>
            <th style="width: 10%;" align="center">Tổng thu</th>
            <th style="width: 10%;" align="center">Số tiền còn</th>
            <th style="width: 10%;" align="center">Đã rút dự toán</th>
            <th style="width: 10%;" align="center">Đã nhận ủy nhiệm chi</th>
            <th style="width: 12%;" align="center">Người tạo</th>
            <th style="width: 10%;" align="center">Chi tiết chứng từ</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        int i;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String sClasstr = "";
            int STT = i + 1;
            
            if (i % 2 == 0) sClasstr = "alt";

            String strRutDuToan = "";
            strRutDuToan = CommonFunction.DinhDangSo(KTCT_TienGui_DuyetChungTuModels.CheckRutDuToan(Convert.ToString(R["iID_MaChungTu_Duyet"])));
            String strUyNhiemChi = "";
            strUyNhiemChi = CommonFunction.DinhDangSo(KTCT_TienGui_DuyetChungTuModels.CheckUyNhiemChi(Convert.ToString(R["iID_MaChungTu_Duyet"])));

            String strDuyet = "";
            String strChiTiet = "";
            String strEdit = "";
            String strDelete = "";

            String sTongCap = "0", sTongThu = "0", sSoTien = "0";
            sTongCap = CommonFunction.DinhDangSo(Convert.ToString(R["rTongCap"]));
            sTongThu = CommonFunction.DinhDangSo(Convert.ToString(R["rTongThu"]));
            sSoTien = CommonFunction.DinhDangSo(Convert.ToString(R["rSoTien"]));

            strDuyet = MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_CapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            strChiTiet = MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_CapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Edit", "");
            
            strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_CapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            if (strRutDuToan == "" && strUyNhiemChi == "") {
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "KTCT_CapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }

            %>
            <tr class='<%=sClasstr %>'>
                <td align="center"><%=STT%></td>
                <td align="center"><b><%=R["iNamLamViec"]%></b></td>
                <td align="center"><b>
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_CapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"].ToString() }).ToString(), R["sSoChungTu"].ToString(), "Detail", "")%></b></td>
                <td align="center"><b><%=sTongCap%></b></td>
                <td align="center"><b><%=sTongThu%></b></td>
                <td align="center"><b><%=sSoTien%></b></td>
                <td align="center"><b><%=strRutDuToan%></b></td>
                <td align="center"><b><%=strUyNhiemChi%></b></td>
                <td align="center"><b><%=R["sID_MaNguoiDungTao"]%></b></td>
                <td align="center"><%=strChiTiet%></td>
                <td align="center"><%=strDelete%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="11" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
    </div>
</div>
<%dtCauHinh.Dispose();
  dt.Dispose(); %>
</asp:Content>
