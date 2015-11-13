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
    String ParentID = "CapPhat_ChungTu";
    String MaPhongBan = Request.QueryString["MaPhongBan"];
    String iSoCapPhat = Request.QueryString["SoCapPhat"];
    String sTuNgay = Request.QueryString["TuNgay"];
    String sDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String iDM_MaLoaiCapPhat = Request.QueryString["iDM_MaLoaiCapPhat"];
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = Convert.ToString(LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DuocSua(PhanHeModels.iID_MaPhanHeCapPhat, MaND));
    }

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeCapPhat);
    
    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtPhongBan = NganSach_HamChungModels.DSPhongBanCuaNguoiDung(MaND);// DanhMucModels.NS_PhongBan();
    if (LuongCongViecModel.KiemTra_ThuTruong(MaND))
    {
        dtPhongBan = DanhMucModels.NS_PhongBan();
        dtPhongBan.Rows.InsertAt(dtPhongBan.NewRow(), 0);
        dtPhongBan.Rows[0]["iID_MaPhongBan"] = Guid.Empty;
        dtPhongBan.Rows[0]["sTen"] = "-- Chọn phòng ban --";
    }    
    SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
    
    DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach();
    dtLoaiNganSach.Rows.InsertAt(dtLoaiNganSach.NewRow(), 0);
    dtLoaiNganSach.Rows[0]["sLNS"] = "";
    dtLoaiNganSach.Rows[0]["TenHT"] = "-- Chọn loại ngân sách --";
    SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "TenHT");

    DataTable dtLoaiCapPhat = DanhMucModels.DT_DanhMuc("LoaiCapPhat", true, "---Chọn loại cấp phát---");
    SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");

    DataTable dt = CapPhat_ChungTuModels.Get_DanhSachChungTu(MaPhongBan, MaND, iSoCapPhat, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat,false,CurrentPage, Globals.PageSize);

    double nums = CapPhat_ChungTuModels.Get_DanhSachChungTu_Count(MaPhongBan, MaND, iSoCapPhat, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat,false);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Duyet", new { MaND = MaND, SoCapPhat = iSoCapPhat, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));

    using (Html.BeginForm("SearchDuyetSubmit", "CapPhat_ChungTu", new { ParentID = ParentID }))
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
                                <td class="td_form2_td1"><div><b>Phòng ban</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>                      
                            <tr>
                                <td class="td_form2_td1"><div><b>Số cấp phát</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoCapPhat, "iSoCapPhat", "", "class=\"input1_2\"")%>
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
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Loại cấp phát</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiCapPhat, iDM_MaLoaiCapPhat, "iDM_MaLoaiCapPhat", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>                           
                            <tr>
                                <td class="td_form2_td1"><div><b>Từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
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
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách cấp phát</span>
                </td>
                <td align="right" style="padding-right: 10px;">                    
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 7%;" align="center">B Quản lý</th>
            <th style="width: 15%;" align="center">Ngày cấp phát</th>
            <th style="width: 10%;" align="center">Số cấp phát</th>
            <th style="width: 15%;" align="center">Loại cấp phát</th>
            <th style="width: 35%;" align="center">Nội dung</th>
            <th style="width: 10%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Thao tác</th>
            <th style="width: 5%;" align="center">Xử lý</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            
            String TenPhongBan = Convert.ToString(DanhMucModels.GetRow_PhongBan(Convert.ToString(R["iID_MaPhongBan"])).Rows[0]["sTen"]);
            String NgayCapPhat = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayCapPhat"]));
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
            
            String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayCapPhat"]));
            
            String LoaiCapPhat = "";
            for (j = 0; j < dtLoaiCapPhat.Rows.Count; j++)
            {
                if (Convert.ToString(R["iDM_MaLoaiCapPhat"]) == Convert.ToString(dtLoaiCapPhat.Rows[j]["iID_MaDanhMuc"]))
                {
                    LoaiCapPhat = Convert.ToString(dtLoaiCapPhat.Rows[j]["sTen"]);
                    break;
                }
            }
            
            //Lấy trạng thái duyệt của chi tiết chứng từ
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM CP_CapPhatChiTiet WHERE bDongY = 1 AND iID_MaCapPhat = @iID_MaCapPhat";
            cmd.Parameters.AddWithValue("@iID_MaCapPhat", R["iID_MaCapPhat"]);
            int SoLuongTuChoi = Convert.ToInt32(Connection.GetValue(cmd, 0));
            cmd.Dispose();

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "CapPhat_ChungTuChiTiet", new { iID_MaCapPhat = R["iID_MaCapPhat"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
            String strURLTuChoi = "", strTex = "";
            if (LuongCongViecModel.KiemTra_NguoiDungDuocDuyet(MaND, PhanHeModels.iID_MaPhanHeCapPhat) && Convert.ToInt16(R["iID_MaTrangThaiDuyet"]) == LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeCapPhat))
            {
                strURLTuChoi = Url.Action("TuChoi", "CapPhat_ChungTu", new { iID_MaCapPhat = R["iID_MaCapPhat"] });
                strTex = "Từ chối";
            }
            %>
            <tr <%=strColor%>>
                <td align="center" style="padding: 3px 2px;"><%=STT%></td>  
                <td align="center"><%=TenPhongBan%></td>
                <td align="center"><%=NgayChungTu %></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "CapPhat_ChungTuChiTiet", new { iID_MaCapPhat = R["iID_MaCapPhat"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoCapPhat"]), "Detail", "")%></b>
                </td>
                <td><%=LoaiCapPhat %></td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center"><%=sTrangThai %></td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(strURLTuChoi, strTex)%>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <%
              String sSoCot = "8";
            %>
            <td colspan="<%=sSoCot %>" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<% 
    dtTrangThai.Dispose();
    dtLoaiCapPhat.Dispose();
%>
</asp:Content>


