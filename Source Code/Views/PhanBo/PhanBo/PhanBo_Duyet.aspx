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
    int i,j;
    String MaND = User.Identity.Name;
    String ParentID = "PhanBo_ChiTieu";
    String MaPhongBan = Request.QueryString["MaPhongBan"];
    String MaLoaiNganSach = Request.QueryString["MaLoaiNganSach"];
    String MaDotNganSach =  Request.QueryString["MaDotNganSach"];
    String NgayDotNganSach = Request.QueryString["NgayDotNganSach"];
    String iSoChungTu = Request.QueryString["SoChungTu"];
    String sTuNgay = Request.QueryString["TuNgay"];
    String sDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = Convert.ToString(LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DuocSua(PhanBoModels.iID_MaPhanHePhanBo, MaND));
    }

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanBoModels.iID_MaPhanHePhanBo);
    
    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHePhanBo, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtPhongBan = DanhMucModels.NS_PhongBan();
    dtPhongBan.Rows.InsertAt(dtPhongBan.NewRow(), 0);
    dtPhongBan.Rows[0]["iID_MaPhongBan"] = Guid.Empty;
    dtPhongBan.Rows[0]["sTen"] = "-- Chọn phòng ban --";
     MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
    SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");

    DataTable dt = PhanBo_PhanBoModels.Get_DanhSachPhanBo(MaPhongBan, NgayDotNganSach, MaDotNganSach, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, MaND, false,CurrentPage, Globals.PageSize);

    double nums = PhanBo_PhanBoModels.Get_DanhSachPhanBo_Count(MaPhongBan, NgayDotNganSach, MaDotNganSach, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, MaND,false);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Duyet", new {MaPhongBan=MaPhongBan,SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay=sDenNgay, iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet, page  = x}));
    String strThemMoi = Url.Action("Edit", "PhanBo_PhanBo", new { MaDotNganSach = MaDotNganSach });

    using (Html.BeginForm("SearchDuyetSubmit", "PhanBo_PhanBo", new { ParentID = ParentID }))
    {
%>
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
                                <td class="td_form2_td1"><div><b>Ban quản lý</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" disabled=\"disabled\"")%></div>
                                </td>
                            </tr>                           
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày chứng từ từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày đợt ngân sách</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, NgayDotNganSach, "sNgayDotNganSach", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>                            
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày chứng từ đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\"")%>
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
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%} %>
<br />
<%
    using (Html.BeginForm("TrinhDuyetSubmit", "PhanBo_PhanBo", new { ParentID = ParentID, MaDotNganSach = MaDotNganSach}))
{
    %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách dự toán</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <%--<input id="Button1" type="submit" class="button_title" value="Trình duyệt" onclick="javascript:location.href=''" />--%>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 5%;" align="center">B Quản lý</th>
            <th style="width: 5%;" align="center">Đơn vị</th>
            <th style="width: 10%;" align="center">Đợt phân bổ</th>
            <th style="width: 10%;" align="center">Người lập</th>
            <th style="width: 7%;" align="center">Ngày phân bổ</th>
            <th style="width: 7%;" align="center">Số chứng từ</th>
            <th style="width: 22%;" align="center">Nội dung</th>
            <th style="width: 6%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Xử lý</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            
            String TenPhongBan = Convert.ToString(DanhMucModels.GetRow_PhongBan(Convert.ToString(R["iID_MaPhongBan"])).Rows[0]["sTen"]);
            String TenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            String NgayDotChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayDotPhanBo"]));
            String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
            String NguoiLap = Convert.ToString(R["sID_MaNguoiDungTao"]);
            String sTrangThai = "";
            String strColor = "";
            String strCheck = "";

            for (j = 0; j < dtTrangThai_All.Rows.Count; j++)
            {
                if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                {
                    sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                    strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                    break;
                }
            }

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = R["iID_MaPhanBo"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");

            %>
            <tr <%=strColor%>>
                <td align="center" style="padding: 3px 2px;"><%=STT%></td>  
                <td align="center"><%=TenPhongBan%></td>
                <td align="center"><%=TenDonVi%></td>
                <td align="center"><%=NgayDotChungTu %></td>
                <td align="center"><%=NguoiLap%></td>
                <td align="center"><%=NgayChungTu %></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_PhanBoChiTiet", new { iID_MaPhanBo = R["iID_MaPhanBo"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"]), "Detail", null, "title=\"Xem chi tiết chứng từ\"")%></b>
                </td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center">
                    <%=sTrangThai %>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <%
              String sSoCot = "11";
            %>
            <td colspan="<%=sSoCot %>" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  } 
    dtTrangThai.Dispose();
%>
</asp:Content>
