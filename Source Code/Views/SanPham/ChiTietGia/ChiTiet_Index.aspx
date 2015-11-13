<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.SanPham" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%     
    String iID_MaSanPham = Request.QueryString["iID_MaSanPham"];
    if (String.IsNullOrEmpty(iID_MaSanPham)) iID_MaSanPham = Convert.ToString(ViewData["iID_MaSanPham"]);
    String iID_MaChiTietGia = Request.QueryString["iID_MaChiTietGia"];
    if (String.IsNullOrEmpty(iID_MaChiTietGia)) iID_MaChiTietGia = Convert.ToString(ViewData["iID_MaChiTietGia"]);
    DataTable dt = SanPham_DanhMucGiaModels.Get_ChiTietGia(iID_MaChiTietGia);
    String sMa = "", sTen = "", rSoLuong = "", sQuyCach = "", iID_MaLoaiHinh = "", strTen = "", strDonVi = "", iID_LoaiDonVi = "", iID_MaDonVi = ""
        , rThueGTGT = "0", rLoiNhuan = "0", rTyLe311 = "0", rTyLe312 = "0", rTyLe411 = "0", rTyLe412 = "0", rTyLe511 = "0", rTyLe512 = "0"
        , rHeSo_211 = "0", rHeSo_212 = "0", rHeSo_311 = "0", rHeSo_312 = "0", rHeSo_411 = "0", rHeSo_412 = "0", rHeSo_511 = "0", rHeSo_512 = "0";
    DataRow R;
    if (dt.Rows.Count > 0)
    {
        R = dt.Rows[0];
        sTen = HamChung.ConvertToString(R["sTen"]);
        sMa = HamChung.ConvertToString(R["sMa"]);
        rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
        sQuyCach = HamChung.ConvertToString(R["sQuyCach"]);
        iID_MaLoaiHinh = HamChung.ConvertToString(R["iID_MaLoaiHinh"]);
        iID_MaDonVi = HamChung.ConvertToString(R["iID_MaDonVi"]);
        strDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
        iID_LoaiDonVi = HamChung.ConvertToString(R["iID_LoaiDonVi"]);
        if (iID_LoaiDonVi == "3")
        {
            strDonVi = "Cục tài chính";
            iID_MaDonVi = "0";
        }
        rThueGTGT = HamChung.ConvertToString(R["rThueGTGT"]);
        rLoiNhuan = HamChung.ConvertToString(R["rLoiNhuan"]);
        DataTable dtCauHinh = SanPham_CauHinhDonViController.LayCauHinh(iID_MaDonVi);
        if (dtCauHinh.Rows.Count > 0)
        {
            DataRow _R = dtCauHinh.Rows[0];
            rTyLe311 = HamChung.ConvertToString(_R["rTyLe311_211"]);
            rTyLe312 = HamChung.ConvertToString(_R["rTyLe312_212"]);
            rTyLe411 = HamChung.ConvertToString(_R["rTyLe411_211"]);
            rTyLe412 = HamChung.ConvertToString(_R["rTyLe412_212"]);
            rTyLe511 = HamChung.ConvertToString(_R["rTyLe511_211"]);
            rTyLe512 = HamChung.ConvertToString(_R["rTyLe512_212"]);

            rHeSo_211 = HamChung.ConvertToString(_R["rHeSo_211"]);
            rHeSo_212 = HamChung.ConvertToString(_R["rHeSo_212"]);
            rHeSo_311 = HamChung.ConvertToString(_R["rHeSo_311"]);
            rHeSo_312 = HamChung.ConvertToString(_R["rHeSo_312"]);
            rHeSo_411 = HamChung.ConvertToString(_R["rHeSo_411"]);
            rHeSo_412 = HamChung.ConvertToString(_R["rHeSo_412"]);
            rHeSo_511 = HamChung.ConvertToString(_R["rHeSo_511"]);
            rHeSo_512 = HamChung.ConvertToString(_R["rHeSo_512"]);
        }
        switch (iID_MaLoaiHinh)
        {
            case "1":
                strTen = "Sửa chữa lớn " + sTen;
                break;
            case "2":
                strTen = "Sửa chữa vừa " + sTen;
                break;
            case "3":
                strTen = "Sửa chữa nhỏ " + sTen;
                break;
        }
    }
%>
<input type="hidden" id="rTyLe_311" value="<%=rTyLe311 %>" />
<input type="hidden" id="rTyLe_312" value="<%=rTyLe312 %>" />
<input type="hidden" id="rTyLe_411" value="<%=rTyLe411 %>" />
<input type="hidden" id="rTyLe_412" value="<%=rTyLe412 %>" />
<input type="hidden" id="rTyLe_511" value="<%=rTyLe511 %>" />
<input type="hidden" id="rTyLe_512" value="<%=rTyLe512 %>" />

<input type="hidden" id="rHeSo_211" value="<%=rHeSo_211 %>" />
<input type="hidden" id="rHeSo_212" value="<%=rHeSo_212 %>" />
<input type="hidden" id="rHeSo_311" value="<%=rHeSo_311 %>" />
<input type="hidden" id="rHeSo_312" value="<%=rHeSo_312 %>" />
<input type="hidden" id="rHeSo_411" value="<%=rHeSo_411 %>" />
<input type="hidden" id="rHeSo_412" value="<%=rHeSo_412 %>" />
<input type="hidden" id="rHeSo_511" value="<%=rHeSo_511 %>" />
<input type="hidden" id="rHeSo_512" value="<%=rHeSo_512 %>" />
<script src="<%= Url.Content("~/Scripts/jsBang_GiaSanPham" + iID_LoaiDonVi + ".js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script type="text/javascript">

    function getThueGTGT() {
        return parseFloat("<%=rThueGTGT %>")/100;
    }
    function getLoiNhuan() {
        return parseFloat("<%=rLoiNhuan %>") / 100;
    }
    function getTyLe(Ma) {
        if (document.getElementById("rTyLe_" + Ma) == null) return 0;
        var TyLe = document.getElementById("rTyLe_" + Ma).value;
        if (TyLe == "") {
            return 0;
        } else {
            return parseFloat(TyLe) / 100;
        }
    }
    function getHeSoBH(Ma) {
        if (document.getElementById("rHeSo_" + Ma) == null) return 0;
        var HeSo = document.getElementById("rHeSo_" + Ma).value;
        if (HeSo == "") {
            return 0;
        } else {
            return parseFloat(HeSo) / 100;
        }
    }
</script>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham", new { }), "Danh sách sản phẩm")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMucGia", new { iID_MaSanPham = iID_MaSanPham }), "Cấu hình danh mục")%>
            </div>
        </td>
    </tr>
</table>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Bảng giải trình chi tiết tính giá sản phẩm</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                            <td class="td_form2_td5"><div><b><%=strDonVi%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã sản phẩm</b></div></td>
                            <td class="td_form2_td5"><div><b><%=sMa%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên sản phẩm</b></div></td>
                            <td class="td_form2_td5"><div><b><%=strTen%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Số lượng</b></div></td>
                            <td class="td_form2_td5"><div><b><%=rSoLuong%></b></div></td>
                        </tr>
                      
                        <tr>
                            <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                            <td class="td_form2_td5"><div><b><%=sQuyCach%></b></div></td>
                        </tr>
                    </table>
                    <%
                        Html.RenderPartial("~/Views/SanPham/ChiTietGia/ChiTiet_Index_DanhSach.ascx", new { ControlID = "SP_ChiTietGia", iID_MaSanPham = iID_MaSanPham, iID_LoaiDonVi = iID_LoaiDonVi, iID_MaChiTietGia = iID_MaChiTietGia, MaND = User.Identity.Name });
                    %>  
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>
