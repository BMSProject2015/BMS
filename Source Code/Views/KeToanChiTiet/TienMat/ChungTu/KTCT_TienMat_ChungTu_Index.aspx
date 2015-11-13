<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "KTCT_TienMat_ChungTu";
    String MaND = User.Identity.Name;
    String iSoChungTu = Request.QueryString["SoChungTu"];
    String dTuNgay = Request.QueryString["TuNgay"];
    String dDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String page = Request.QueryString["page"];

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(KeToanTongHopModels.iID_MaPhanHe, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(KeToanTongHopModels.iID_MaPhanHe);
    DataTable dt = KTCT_TienMat_ChungTuModels.Get_DanhSachChungTu(MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet, CurrentPage, Globals.PageSize);

    double nums = KTCT_TienMat_ChungTuModels.Get_DanhSachChungTu_Count(MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { MaND = MaND, SoChungTu = iSoChungTu, TuNgay = dTuNgay, DenNgay = dDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));

    String strThemMoi = Url.Action("Edit", "KTCT_TienMat_ChungTu");

    using (Html.BeginForm("SearchSubmit", "KTCT_TienMat_ChungTu", new { ParentID = ParentID}))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienMat_ChungTu"), "Danh sách chứng từ ghi sổ")%>
            </div>
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
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"",2)%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày chứng từ từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>        
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td colspan="2" align="center" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input type="submit" class="button" value="Tìm kiếm"/>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <%
                                        if(LuongCongViecModel.NguoiDung_DuocThemChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND))
                                        {
                                        %>
                                            <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                        <%
                                        } %>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
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
                    <span>Danh sách chứng từ ghi sổ</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 8%;" align="center">Tháng</th>
            <th style="width: 10%;" align="center">Số chứng từ</th>
            <th style="width: 35%;" align="center">Nội dung</th>
            <th style="width: 20%;" align="center">Trạng thái</th>
            <th style="width: 10%;" align="center">Ngày tạo</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
            String sTrangThai = "";
            String strColor = "";
            for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
            {
                if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                {
                    sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                    strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                    break;
                }
            }            
            String strEdit = "";
            String strDelete = "";
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(KeToanTongHopModels.iID_MaPhanHe, MaND) &&
                                LuongCongViecModel.KiemTra_TrangThaiKhoiTao(KeToanTongHopModels.iID_MaPhanHe, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
            {
                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_TienMat_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"]}).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "KTCT_TienMat_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienMat_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"]}).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                       
            %>
            <tr <%=strColor %>>
                <td align="center"><%=STT%></td>    
                <td align="center"><%=R["iThang"] %></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienMat_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"]}).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center"><%=sTrangThai %></td>
                <td align="center"><%=NgayChungTu %></td>
                <td align="center">
                    <%=strURL %>
                </td>
                <td align="center">
                    <%=strEdit%>                   
                </td>
                <td align="center">
                    <%=strDelete%>                                       
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="9" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
dtTrangThai_All.Dispose();
dtTrangThai.Dispose();
%>
</asp:Content>





