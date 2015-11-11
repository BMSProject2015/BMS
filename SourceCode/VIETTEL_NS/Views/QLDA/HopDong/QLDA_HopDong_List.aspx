<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    String sSoHopDong = Request.QueryString["sSoHopDong"];
    String dTuNgay = Request.QueryString["dTuNgay"];
    String dDenNgay = Request.QueryString["dDenNgay"];
    String page = Request.QueryString["page"];
    
    String ParentID = "QLDA_HopDong";
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

    int iDotMoi = 0;
    if (QLDA_CapPhatModels.Get_Max_Dot(NamLamViec) != "") { 
        iDotMoi = Convert.ToInt32(QLDA_CapPhatModels.Get_Max_Dot(NamLamViec)) + 1;
    };
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }
    
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    String sNguoiDung = "";
    //Xác định là nhóm trợ lý phòng ban là người tạo mới trong NS_PhanHe_TrangThaiDuyet
    bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeVonDauTu, User.Identity.Name);
    if (isModify) sNguoiDung = User.Identity.Name;
    DataTable dt = QLDA_HopDongModels.Get_DanhSachHopDong(sSoHopDong,dTuNgay, dDenNgay,sNguoiDung, CurrentPage, Globals.PageSize);

    double nums = QLDA_HopDongModels.Get_DanhSachHopDong_Count(sSoHopDong,dTuNgay, dDenNgay);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);

    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", "QLDA_HopDong", new { sSoHopDong = sSoHopDong,TuNgay = dTuNgay, DenNgay = dDenNgay, page = x }));

    using (Html.BeginForm("AddNewSubmit", "QLDA_HopDong", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_HopDong"), "Danh sách hợp đồng")%>
            </div>
        </td>
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
        <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Tìm kiếm thông tin hợp đồng")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                            using (Html.BeginForm("SearchSubmit", "QLDA_HopDong", new { ParentID = ParentID }))
                            {       
                            %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="100%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Số hợp đồng</b></div></td>
                                        <td class="td_form2_td5" style="width: 24%;">
                                            <div>
                                                <%=MyHtmlHelper.TextBox(ParentID, sSoHopDong, "sSoHopDong_Search", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm từ ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 23%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Tìm đến ngày</b></div></td>
                                        <td class="td_form2_td5" style="width: 23%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
            	                        <td class="td_form2_td1" colspan="6">
            	                            <div style="text-align:right; float:right; width:100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	                            </div> 
            	                        </td>
                                    </tr>
                                    <tr><td class="td_form2_td1" align="right" colspan="6"></td></tr>
                                </table>
                            <%} %>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Chọn hợp đồng hoặc thêm hợp đồng mới</span>
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
                                    <div><%=NgonNgu.LayXau("Thêm hợp đồng mới")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.CheckBox(ParentID, iThemMoi, "iThemMoi", "", "onclick=\"CheckThemMoi(this.checked)\"")%></div>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2" id="tb_DotNganSach">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Số hợp đồng")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextBox(ParentID, " ", "sSoHopDong", "", "class=\"input1_2\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sSoHopDong")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Ngày hợp đồng")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayHopDong", "", "class=\"input1_2\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayHopDong")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Ngày lập")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayLap", "", "class=\"input1_2\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                    </div>
                                </td>
                            </tr>
                             <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Nội dung")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextArea(ParentID, "", "sNoiDung", "", "class=\"input1_2\"")%><br />
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
                    <span>Danh sách hợp đồng</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid">
                <tr>
                    <th style="width: 3%;" align="center">STT</th>
                    <th style="width: 10%;" align="center">Số hợp đồng</th>
                      <th style="width: 40%;" align="center">Nội dung</th>
                    <th style="width: 20%;" align="center">Ngày hợp đồng</th>
                    <th style="width: 13%;" align="center">Ngày lập</th>
                    <th style="width: 7%;" align="center">Sửa</th>
                     <th style="width: 7%;" align="center">Xóa</th>
                </tr>
                <%
                int i;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String sClasstr = "";
                        
                    //Ngày tạo tổng đầu tư 
                    String dNgayHopDong = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayHopDong"]));
                    String dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]));
                    if (i % 2 == 0) sClasstr = "alt";
                    String strEdit = "";
                    String strDelete = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_HopDong", new { iID_MaHopDong = R["iID_MaHopDong"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                    String SQL = "SELECT * FROM QLDA_CapPhat WHERE iTrangThai=1 AND iID_MaHopDong='"+R["iID_MaHopDong"]+"'";
                    DataTable dtCP=Connection.GetDataTable(SQL);
                    if(dtCP.Rows.Count<=0)
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_HopDong", new { iID_MaHopDong = R["iID_MaHopDong"]}).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    %>
                    <tr <%=sClasstr %>>
                        <td align="center"><%=R["rownum"]%></td>
                        <td align="left">
                            <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_HopDong", new { iID_MaHopDong = R["iID_MaHopDong"].ToString() }).ToString(), R["sSoHopDong"].ToString(), "Detail", "")%></b>
                        </td> 
                         <td align="left">
                            <%=R["sNoiDung"]%>
                        </td>     
                        <td align="center"><%=dNgayHopDong%></td>
                        <td align="center"><%=dNgayLap%></td>
                        <td align="center"><%=strEdit%></td>
                         <td align="center"><%=strDelete%></td>
                    </tr>
                <%} %>
                <tr class="pgr">
                    <td colspan="7" align="right">
                        <%=strPhanTrang%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
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
