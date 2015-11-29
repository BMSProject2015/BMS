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
    String ParentID = "CapPhat_ChungTu";
    String DonVi = Convert.ToString(ViewData["DonVi"]);
    String sLNS = Convert.ToString(ViewData["sLNS"]);
    String iSoCapPhat = Request.QueryString["SoCapPhat"];
    String sTuNgay = Request.QueryString["TuNgay"];
    String sDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String iDM_MaLoaiCapPhat = Request.QueryString["iDM_MaLoaiCapPhat"];
    String page = Request.QueryString["page"];
    String Loai = Request.QueryString["Loai"];
    String iID_MaTinhChatCapThu = Request.QueryString["iID_MaTinhChatCapThu"];
    String sLNSQuocPhong = Request.QueryString["sLNSQuocPhong"];
    //String 
    if (HamChung.isDate(sTuNgay) == false) sTuNgay = "";
    if (HamChung.isDate(sDenNgay) == false) sDenNgay = "";
    int CurrentPage = 1;
    SqlCommand cmd;

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";
    
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeCapPhat);
    
    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeCapPhat, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "--Chọn trạng thái--";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = CapPhat_ChungTuModels.LayDanhSachChungTuCuc("", MaND, iSoCapPhat, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat,iID_MaTinhChatCapThu,sLNSQuocPhong, true, CurrentPage, Globals.PageSize);

    double nums = CapPhat_ChungTuModels.LayDanhSachChungTuCapPhatCucCount("", MaND, iSoCapPhat, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, iDM_MaLoaiCapPhat, iID_MaTinhChatCapThu, sLNSQuocPhong, true);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {MaND = MaND, SoCapPhat = iSoCapPhat, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));
    String strThemMoi = Url.Action("SuaChungTu", "CapPhat_ChungTu", new { sLNS = sLNS, DonVi = DonVi, Loai = Loai });
    DataTable dtLoaiCapPhat = DanhMucModels.DT_DanhMuc("LoaiCapPhat",true,"--Chọn loại cấp phát--");
    SelectOptionList slLoaiCapPhat = new SelectOptionList(dtLoaiCapPhat, "iID_MaDanhMuc", "sTen");

    //START Lấy danh sách loại ngân sách quốc phòng
    String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
    DataTable dtLNSQuocPhong = DanhMucModels.NS_LoaiNganSachQuocPhong(iID_MaPhongBan);
    SelectOptionList slLNSQuocPhong = new SelectOptionList(dtLNSQuocPhong, "sLNS", "TenHT");
    //END
    
    //START Lấy danh sách tính chất cấp thu
    DataTable dtTinhChatCapThu = TinhChatCapThuModels.Get_dtTinhChatCapThu();
    SelectOptionList slTinhChatCapThu = new SelectOptionList(dtTinhChatCapThu, "iID_MaTinhChatCapThu", "sTen");
    DataRow R2 = dtTinhChatCapThu.NewRow();
    R2["iID_MaTinhChatCapThu"] = "-1";
    R2["sTen"] = "--Chọn tính chất cấp thu--";
    dtTinhChatCapThu.Rows.InsertAt(R2, 0);
    //END
    
    using (Html.BeginForm("TimKiemChungTu", "CapPhat_ChungTu", new { ParentID = ParentID, DonVi = DonVi, Loai = Loai }))
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
                                <td class="td_form2_td1"><div><b>Số cấp phát</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoCapPhat, "iSoCapPhat", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                              <tr>
                                <td class="td_form2_td1"><div><b>Loại cấp phát</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiCapPhat, iDM_MaLoaiCapPhat, "iDM_MaLoaiCapPhat", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                              <tr>
                                <td class="td_form2_td1"><div><b>Tính chất cấp thu</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTinhChatCapThu, iID_MaTinhChatCapThu, "iID_MaTinhChatCapThu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Loại ngân sách</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLNSQuocPhong, sLNSQuocPhong, "sLNSQuocPhong", "", "class=\"input1_2\"")%>
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
                                <td class="td_form2_td1" ; width="10%" ><div><b>Từ ngày</b></div></td>
                                <td class="td_form2_td5" ; width="35%" >
                                    <div style="width: 35%">
                                        <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\" onblur=isDate(this);")%>        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div style="width: 35%">
                                        <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\" onblur=isDate(this);")%>
                                    </div>
                                </td>
                            </tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
                            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
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
                                        if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND))                                        {
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
                	<span>Danh sách cấp phát</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 7.5%;" align="center">Ngày cấp phát</th>
            <th style="width: 7.5%;" align="center">Số cấp phát</th>
            <th style="width: 7.5%;" align="center">Loại cấp phát</th>
            <th style="width: 7.5%;" align="center">Tính chất cấp thu</th>
            <th style="width: 7.5%;" align="center">Loại Ngân sách</th>
            <% if (Loai == "1")
               {%>
                <th style="width: 10%;" align="center">Chi tiết Đến</th>
                <th style="width: 27.5%;" align="center">Nội dung</th>
             <%}
               else
               {%> 
                <th style="width: 37.5%;" align="center">Nội dung</th>
               <%}
             %>
            
            <th style="width: 10%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayCapPhat"]));
            String sTrangThai = "",strColor="";
            int TrangThaiDuyet = Convert.ToInt16(R["iID_MaTrangThaiDuyet"]);
            int DaDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeCapPhat);
            String iID_MaCapPhat = Convert.ToString(R["iID_MaCapPhat"]);
            for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
            {
                if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                {
                    sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                    strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                    break;
                }
            }

            String LoaiCapPhat = "";
            for (int j = 0; j < dtLoaiCapPhat.Rows.Count; j++)
            {
                if(Convert.ToString(R["iDM_MaLoaiCapPhat"])==Convert.ToString(dtLoaiCapPhat.Rows[j]["iID_MaDanhMuc"]))
                {
                    LoaiCapPhat=Convert.ToString(dtLoaiCapPhat.Rows[j]["sTen"]);
                    break;
                }
            }
            String tinhChatCapThu = "";
            for (int j = 0; j < dtTinhChatCapThu.Rows.Count; j++)
            {
                if (Convert.ToString(R["iID_MaTinhChatCapThu"]) == "-1")
                    break;
                if (Convert.ToString(R["iID_MaTinhChatCapThu"]) == Convert.ToString(dtTinhChatCapThu.Rows[j]["iID_MaTinhChatCapThu"]))
                {
                    tinhChatCapThu = Convert.ToString(dtTinhChatCapThu.Rows[j]["sTen"]);
                    break;
                }
            }

            DataTable dtLoaiChiTietDen = CapPhat_ChungTuModels.LayLoaiNganSachCon();
            String chiTietDen = "";
            for (int j = 0; j < dtLoaiChiTietDen.Rows.Count; j++)
            {
                if (Convert.ToString(R["sLoai"]) == Convert.ToString(dtLoaiChiTietDen.Rows[j]["iID_Loai"]))
                {
                    chiTietDen = Convert.ToString(dtLoaiChiTietDen.Rows[j]["TenHT"]);
                    break;
                }
            }
            dtLoaiChiTietDen.Dispose();
            
            String LoaiNganSach = (Convert.ToString(R["sDSLNS"]).Length < 7)? "": Convert.ToString(R["sDSLNS"]);
            
            String strEdit = "";
            String strDelete = "";

            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeCapPhat, MaND) &&
                                LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeCapPhat, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
            {
                strEdit = MyHtmlHelper.ActionLink(Url.Action("SuaChungTu", "CapPhat_ChungTu", new { iID_MaCapPhat = R["iID_MaCapPhat"], Loai = Loai }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strDelete = MyHtmlHelper.ActionLink(Url.Action("XoaChungTu", "CapPhat_ChungTu", new { iID_MaCapPhat = R["iID_MaCapPhat"], Loai = Loai}).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            }
            
            %>
            <tr <%=strColor %>>
                <td align="center"><%=R["rownum"]%></td>            
                <td align="center"><%=NgayChungTu %></td>
                <td align="center"><b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "CapPhat_ChungTuChiTiet", new { iID_MaCapPhat = R["iID_MaCapPhat"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoCapPhat"]), "Detail", "")%></b></td>
                <td><%=LoaiCapPhat %></td>
                <td><%=tinhChatCapThu %></td>
                <td><%=LoaiNganSach%></td>
                <td><%=chiTietDen%></td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center"><%=sTrangThai %></td>
                <td align="center">
                    <%=strEdit%>                   
                </td>
                <td align="center">
                    <%=strDelete%>                                       
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="11" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%
dt.Dispose();
dtTrangThai.Dispose();
dtLoaiCapPhat.Dispose();
dtTrangThai_All.Dispose();
dtTinhChatCapThu.Dispose();
dtLNSQuocPhong.Dispose();
%>
<script type="text/javascript">
    function CallSuccess_CT() {     
        location.reload();
        return false;
    }
    function OnInit_CT() {
        $("#idDialog").dialog("destroy");
        document.getElementById("idDialog").title = 'Thông tri';
        document.getElementById("idDialog").innerHTML = "";
        $("#idDialog").dialog({
            resizeable: false,
            width: 400,            
            modal: true
        });
    }
    function OnLoad_CT(v) {
        document.getElementById("idDialog").innerHTML = v;
    }
</script>

<div id="idDialog" style="display: none;">    
</div>
</asp:Content>

