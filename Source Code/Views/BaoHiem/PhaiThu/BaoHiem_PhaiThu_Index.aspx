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
    String ParentID = "BaoHiem_PhaiThu";
    String MaND = User.Identity.Name;
    String iSoChungTu = Request.QueryString["SoChungTu"];
    String iThang_Quy = Request.QueryString["iThang_Quy"];    
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];    
    String page = Request.QueryString["page"];
    
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeBaoHiem, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    DataTable dtDonVi = NguoiDung_DonViModels.DS_NguoiDung_DonVi(MaND, true);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    
    DataTable dtThang = DanhMucModels.DT_Thang(true);
    //DataRow Row;
    //Row = dtThang.NewRow();
    //Row[0] = "";
    //Row[1] = "-- Chọn tháng --";
    //dtThang.Rows.InsertAt(Row, 0);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
    
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeBaoHiem);
    DataTable dt = BaoHiem_PhaiThuModels.Get_DanhSachChungTu(MaND, iSoChungTu,iThang_Quy, iID_MaTrangThaiDuyet,iID_MaDonVi, CurrentPage, Globals.PageSize);

    double nums = BaoHiem_PhaiThuModels.Get_DanhSachChungTu_Count(MaND, iSoChungTu, iThang_Quy, iID_MaTrangThaiDuyet, iID_MaDonVi);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { SoChungTu = iSoChungTu, iThang_Quy = iThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));

    String strThemMoi = Url.Action("Edit", "BaoHiem_PhaiThu");

    using (Html.BeginForm("SearchSubmit", "BaoHiem_PhaiThu", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThu"), "Danh sách chứng từ ")%>
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
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "sSoChungTu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr> 
                             <tr>
                                <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
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
                                <td class="td_form2_td1">
                                    <div><b>Tháng</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:98%;\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iThang_Quy")%>
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
                                        if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND))
                                        {
                                        %>
                                            <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                        <%
                                        }%>
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
                        Danh sách chứng từ chi bảo hiểm
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 10%;" align="center">Đơn vị</th>
            <th style="width: 10%;" align="center">Số chứng từ</th>
            <th style="width: 10%;" align="center">Thời gian</th>
            <th style="width: 20%;" align="center">Nội dung</th>
            <th style="width: 10%;" align="center">Trạng thái</th>
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
                     
            
            //Lấy thông tin loại ngân sách
            String strLNS = DanhMucModels.NS_TenLoaiNganSach(Convert.ToString(R["sDSLNS"]));
            String sTen_DonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            String strThoiGianBH = "";
        
                    strThoiGianBH = "Tháng: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
          
            String strEdit = "";
            String strDelete = "";
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeBaoHiem, MaND) &&
                                LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeBaoHiem, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
            {
                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "BaoHiem_PhaiThu", new { iID_MaBaoHiemPhaiThu = R["iID_MaBaoHiemPhaiThu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "BaoHiem_PhaiThu", new { iID_MaBaoHiemPhaiThu = R["iID_MaBaoHiemPhaiThu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThuChiTiet", new { iID_MaBaoHiemPhaiThu = R["iID_MaBaoHiemPhaiThu"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                       
            %>
            <tr <%=strColor %>>
                <td align="center"><%=STT%></td>  
                <td align="left"><%=sTen_DonVi%></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThuChiTiet", new { iID_MaBaoHiemPhaiThu = R["iID_MaBaoHiemPhaiThu"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="center"><%=HttpUtility.HtmlEncode(strThoiGianBH)%></td>
                <td align="left"><%= HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%></td>
                <td align="center"><%=HttpUtility.HtmlEncode(sTrangThai) %></td>
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
            <td colspan="10" align="right">
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



