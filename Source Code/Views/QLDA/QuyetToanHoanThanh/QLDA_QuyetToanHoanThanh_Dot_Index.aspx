<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
    String ParentID = "QLDA_QuyetToanHoanThanh";
    String NamLamViec = Request.QueryString["NamLamViec"];
    String page = Request.QueryString["page"];
    String sNguoiDung = "";
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    if(NamLamViec == null || NamLamViec == ""){
        NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    }
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    //Xác định là nhóm trợ lý phòng ban là người tạo mới trong NS_PhanHe_TrangThaiDuyet
    bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeVonDauTu, User.Identity.Name);
    if (isModify) sNguoiDung = User.Identity.Name;
    DataTable dt = QLDA_QuyetToanHoanThanhModels.Get_DanhSachQuyetToan_SoPhieu(NamLamViec, sNguoiDung, CurrentPage, Globals.PageSize);

    double nums = QLDA_QuyetToanHoanThanhModels.Get_DanhSachQuyetToan_SoPhieu_Count(NamLamViec, sNguoiDung);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "QLDA_QuyetToanHoanThanh", new { NamLamViec = NamLamViec, sNguoiDung = sNguoiDung, page = x }));

    int iDotMoi = 0;
    if (QLDA_QuyetToanHoanThanhModels.Get_Max_Dot(NamLamViec) != "") { 
        iDotMoi = Convert.ToInt32(QLDA_QuyetToanHoanThanhModels.Get_Max_Dot(NamLamViec)) + 1;
    };
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }

    using (Html.BeginForm("AddNewSubmit", "QLDA_QuyetToanHoanThanh", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_QuyetToanHoanThanh"), "Danh sách quyết toán hoàn thành")%>
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
                    <span>Chọn đợt hoặc thêm mới quyết toán</span>
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
                                    <div><%=NgonNgu.LayXau("Thêm quyết toán mới")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.CheckBox(ParentID, iThemMoi, "iThemMoi", "", "onclick=\"CheckThemMoi(this.checked)\"")%></div>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2" id="tb_DotNganSach">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Số quyết toán")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.Label(iDotMoi, "iSoQuyetToan")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Ngày quyết toán")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayQuyetToan", "", "class=\"input1_2\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayQuyetToan")%>
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
                    <span>Danh sách quyết toán</span>
                </td>
            </tr>
        </table>
    </div>
    <div>
    <table class='mGrid'>
        <tr>
            <th style="width: 3%" align="center">STT</th>
            <th style="width: 20%;" align="center">Năm làm việc</th>
            <th style="width: 25%;" align="center">Ngày quyết toán</th>               
            <th style="width: 20%;" align="center">Số quyết toán</th>
            <th style="width: 27%;" align="center">Người tạo</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        int i;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String sClasstr = "";
            int STT = i + 1;
            
            //Ngày tạo tổng đầu tư 
            String dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQuyetToan"]));
            if (i % 2 == 0) sClasstr = "alt";
            String strEdit = "";
            String strDelete = "";

            strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_QuyetToanHoanThanh", new { iID_MaQuyetToanHoanThanh_SoPhieu = R["iID_MaQuyetToanHoanThanh_SoPhieu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_QuyetToanHoanThanh", new { iID_MaQuyetToanHoanThanh_SoPhieu = R["iID_MaQuyetToanHoanThanh_SoPhieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            
            %>
            <tr class='<%=sClasstr %>'>
                <td align="center"><%=STT%></td>
                <td align="center"><b><%=R["iNamLamViec"]%></b></td>
                <td align="center"><b>
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QLDA_QuyetToanHoanThanh", new { iID_MaQuyetToanHoanThanh_SoPhieu = R["iID_MaQuyetToanHoanThanh_SoPhieu"].ToString() }).ToString(), dNgayLap.ToString(), "Detail", "")%></b></td>
                <td align="center"><b>
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QLDA_QuyetToanHoanThanh", new { iID_MaQuyetToanHoanThanh_SoPhieu = R["iID_MaQuyetToanHoanThanh_SoPhieu"].ToString() }).ToString(), R["iSoQuyetToan"].ToString(), "Detail", "")%></b></td>
                <td align="center"><b><%=R["sID_MaNguoiDungTao"]%></b></td>
                <td align="center"><%=strDelete%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="6" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
    </div>
</div>
<%dtCauHinh.Dispose();
  dt.Dispose(); %>
<script type="text/javascript">
    CheckThemMoi(false);
    function CheckThemMoi(value) {
        if (value == true) {
            document.getElementById('tb_DotNganSach').style.display = ''
        } else {
            document.getElementById('tb_DotNganSach').style.display = 'none'
        }
    }  
</script>
</asp:Content>

