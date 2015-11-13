<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.DotNganSach" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   Chọn đợt hoặc thêm mới đợt ngân sách
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    
    String ParentID = "DuToan";
    String bChiNganSach = Convert.ToString(Request.QueryString["ChiNganSach"]);
    String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
    String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iXoa = Convert.ToString(Request.QueryString["iXoa"]);    
       
    DataTable dtLoaiNganSach_All = DanhMucModels.NS_LoaiNganSach();
    String sLNS = Convert.ToString(dtLoaiNganSach_All.Rows[0]["sLNS"]);
    SelectOptionList slLoaiNganSach_All = new SelectOptionList(dtLoaiNganSach_All, "sLNS", "TenHT");
    dtLoaiNganSach_All.Dispose();

    DataTable dtLoaiNganSach = NganSach_HamChungModels.DSLNSCuaPhongBan(User.Identity.Name);
    dtLoaiNganSach.Rows.InsertAt(dtLoaiNganSach.NewRow(), 0);
    dtLoaiNganSach.Rows[0]["sLNS"] = "";
    dtLoaiNganSach.Rows[0]["sTen"] = "---Danh sách loại ngân sách---";
    SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "sTen");
    dtLoaiNganSach.Dispose();
     
    DataTable dtTrangThai = CommonFunction.Lay_dtDanhMuc("TrangThaiDotNganSach");
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaDanhMuc", "sTen");
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }
    using (Html.BeginForm("AddNewSubmit", "DuToan_DotNganSach", new { ParentID = ParentID, ChiNganSach = bChiNganSach }))
    {
%>
<%if (iXoa == "-1")
{ %>
<script type="text/javascript" language="javascript">
    alert("Đợt dự toán đã có chứng từ được duyệt nên không thể xóa đợt!");
</script>
<%} %>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Chọn đợt hoặc thêm mới đợt ngân sách</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                <tr>
                    <td class="td_form2_td1" style="width: 15%;">
                        <div><%=NgonNgu.LayXau("Chọn loại ngân sách")%></div>
                    </td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slLoaiNganSach, "", "sLNS", "", "class=\"input1_2\" onchange=\"Chon_LNS(this.value)\"")%></div>
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sLNS")%>
                    </td>
                </tr>                
                <tr>
                    <td class="td_form2_td1">
                        <div><%=NgonNgu.LayXau("Thêm đợt mới")%></div>
                    </td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.CheckBox(ParentID, iThemMoi, "iThemMoi", "", "onclick=\"CheckThemMoi(this.checked)\"")%></div>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tb_DotNganSach">
                <tr>
                    <td class="td_form2_td1" style="width: 15%;">
                        <div><%=NgonNgu.LayXau("Ngày") %></div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DatePicker(ParentID,"","dNgayDotNganSach","","class=\"input1_2\"") %>
                             <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayDotNganSach")%>
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
        </div>
    </div>
</div>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách đợt ngân sách</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="<%= ParentID %>_divDotNganSach">
        <%=DuToan_DotNganSachController.get_NgayDotNganSach(ParentID, NamLamViec, Convert.ToInt32(bChiNganSach),NguonNganSach, NamNganSach, sLNS)%>
    </div>
</div>
<%
    }
    dtLoaiNganSach.Dispose();      
%>
<script type="text/javascript">
    CheckThemMoi('<%=bThemMoi %>');
    function CheckThemMoi(value) {
        if (value==true||value=='True') {
            document.getElementById('tb_DotNganSach').style.display = ''
        } else {
            document.getElementById('tb_DotNganSach').style.display = 'none'
        }
    }

    function Chon_LNS(sLNS) {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("get_objDotNganSach?ParentID=#0&NamLamViec=#1&bChiNganSach=#2&MaNguonNamNganSach=#3&MaNamNganSach=#4&sLNS=#5", "DuToan_DotNganSach")%>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", <%=NamLamViec %>));
        url = unescape(url.replace("#2", <%=bChiNganSach %>));
        url = unescape(url.replace("#3", <%=NguonNganSach %>));
        url = unescape(url.replace("#4", <%=NamNganSach %>));
        url = unescape(url.replace("#5", sLNS));
        $.getJSON(url, function (data) {
            document.getElementById("<%= ParentID %>_divDotNganSach").innerHTML = data;
        });
    }        
        
</script>
</asp:Content>
