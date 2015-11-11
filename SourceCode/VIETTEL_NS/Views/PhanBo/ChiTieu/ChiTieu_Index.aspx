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
    String MaND = User.Identity.Name;
    String ParentID = "PhanBo_ChiTieu";
    String ChiNganSach = Request.QueryString["ChiNganSach"];
    String MaDotPhanBo = Convert.ToString(ViewData["MaDotPhanBo"]);
    String iSoChungTu = Request.QueryString["SoChiTieu"];
    String sTuNgay = Request.QueryString["TuNgay"];
    String sDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String page = Request.QueryString["page"];

    if (HamChung.isDate(sTuNgay) == false) sTuNgay = "";
    if (HamChung.isDate(sDenNgay) == false) sDenNgay = "";
    
    int CurrentPage = 1;
    SqlCommand cmd;

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";
    
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanBoModels.iID_MaPhanHeChiTieu);

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanBoModels.iID_MaPhanHeChiTieu, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = PhanBo_ChiTieuModels.Get_DanhSachChiTieu(MaDotPhanBo, MaND, "", "", "", iID_MaTrangThaiDuyet, sTuNgay, sDenNgay,true, CurrentPage, Globals.PageSize);

    double nums = PhanBo_ChiTieuModels.Get_DanhSachChiTieu_Count(MaDotPhanBo, MaND, "", "", "", iID_MaTrangThaiDuyet, sTuNgay, sDenNgay,true);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay=sDenNgay, iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet, page  = x}));
    String strThemMoi = Url.Action("Edit", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo });

    using (Html.BeginForm("SearchSubmit", "PhanBo_ChiTieu", new { ParentID = ParentID, MaDotPhanBo = MaDotPhanBo }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_DotPhanBo"), "Đợt phân bổ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo }), "Chỉ tiêu phân bổ")%>
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
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày chứng từ từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\" onblur=isDate(this); ")%>        
                                         
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
                                        <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\" onblur=isDate(this)")%>
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
                                        if(LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHeChiTieu, MaND))
                                        {
                                        %>
                                            <%--<div style="width: 10%; float: right;" onclick="OnInit_CT();">      
                                                <%= Ajax.ActionLink(" ", "Index", "NhapNhanh", new { id = "CHUNGTU_THEMMOI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", MaDotPhanBo = MaDotPhanBo, sLNS = sLNS, ChiNganSach = ChiNganSach, idDiv = "divCongThuc" }, new AjaxOptions { }, new { @class = "icon_them" })%>                                
                                            </div> 

                                            <script type="text/javascript">
                                               function CallSuccess_CT(CongThuc, MaDiv) {
                                                   document.getElementById(MaDiv).innerHTML = CongThuc;
                                                   return false;
                                               }
                                               function OnInit_CT() {
                                                   $("#idDialog").dialog("destroy");
                                                   document.getElementById("idDialog").title = '<%=NgonNgu.LayXauChuHoa("Thêm mới chứng từ")%>';
                                                   document.getElementById("idDialog").innerHTML = "";
                                                   $("#idDialog").dialog({
                                                       resizeable: false,
                                                       height: 280,
                                                       width: 600,
                                                       modal: true
                                                   });
                                               }
                                               function OnLoad_CT(v) {
                                                   document.getElementById("idDialog").innerHTML = v;
                                               }                                    
                                            </script>
                                            <div id="idDialog" style="display: none;"></div>--%>
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
                	<span>Danh sách chỉ tiêu</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 2%;" align="center">STT</th>
            <th style="width: 10%;" align="center">Ngày</th>
            <th style="width: 5%;" align="center">Số CT</th>
            <th align="center">Nội dung</th>
            <th style="width: 10%;" align="center">Phân bổ ngành</th>
            <th style="width: 10%;" align="center">Phân bổ</th>
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
            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanBoModels.iID_MaPhanHeChiTieu, MaND) &&
                                LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanBoModels.iID_MaPhanHeChiTieu, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
            {
                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "PhanBo_ChiTieu", new { iID_MaChiTieu = R["iID_MaChiTieu"], MaDotPhanBo = MaDotPhanBo }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "PhanBo_ChiTieu", new { iID_MaChiTieu = R["iID_MaChiTieu"], MaDotPhanBo = MaDotPhanBo }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }
            
            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieuChiTiet", new { ChiNganSach = ChiNganSach, iID_MaChiTieu = R["iID_MaChiTieu"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");

            String strURL_PhanBo = MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_PhanBo", new { ChiNganSach = ChiNganSach, iID_MaChiTieu = R["iID_MaChiTieu"] }).ToString(), "Phân bổ", "Detail", null, "title=\"Xem phân bổ của chỉ tiêu\"");

            String strURL_PhanBoNganh = MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_PhanBoNganh", new { ChiNganSach = ChiNganSach, iID_MaChiTieu = R["iID_MaChiTieu"] }).ToString(), "Phân bổ ngành", "Detail", null, "title=\"Xem phân bổ của chỉ tiêu\"");
            String strURLTuChoi = "", strTex = "";
          
            %>
            <tr <%=strColor %>>
                <td align="center"><%=R["rownum"]%></td>            
                <td align="center"><%=NgayChungTu %></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = R["iID_MaChiTieu"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center"><%=strURL_PhanBoNganh%></td>
                <td align="center"><%=strURL_PhanBo%></td>
                <td align="center"><%=sTrangThai %></td>
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
dtTrangThai.Dispose();
dtTrangThai_All.Dispose();
%>


</asp:Content>



