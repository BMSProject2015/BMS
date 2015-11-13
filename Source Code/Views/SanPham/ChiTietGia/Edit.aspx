<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string ParentID = "SanPham";
        String UserID = User.Identity.Name;
        int i;
        string iID_MaSanPham = HamChung.ConvertToString(ViewData["iID_MaSanPham"]);
        string iID_MaChiTietGia = HamChung.ConvertToString(ViewData["iID_MaChiTietGia"]);
        DataTable dt = new DataTable();
        if(String.IsNullOrEmpty(iID_MaChiTietGia)) dt = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
        else dt = SanPham_DanhMucGiaModels.Get_ChiTietGia(iID_MaChiTietGia);
        DataRow R;
        string sTen = "", sMa = "", rSoLuong = "", iID_MaDonVi = "", strTenDonVi = "", sQuyCach = "", iDM_MaDonViTinh = "", sTen_DonViTinh = "", iID_MaLoaiHinh = "", iID_LoaiDonVi = ""
            , rThueGTGT = "", rLoiNhuan = "";
        if (dt.Rows.Count > 0) {
            R = dt.Rows[0];
            sTen = HamChung.ConvertToString(R["sTen"]);
            sMa = HamChung.ConvertToString(R["sMa"]);
            rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
            sQuyCach = HamChung.ConvertToString(R["sQuyCach"]);
            iID_MaDonVi = HamChung.ConvertToString(R["iID_MaDonVi"]);
            strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            iDM_MaDonViTinh = HamChung.ConvertToString(R["iDM_MaDonViTinh"]);
            sTen_DonViTinh = SanPham_VatTuModels.Get_TenDonViTinh(iDM_MaDonViTinh);
            iID_MaLoaiHinh = HamChung.ConvertToString(R["iID_MaLoaiHinh"]);
            if (!String.IsNullOrEmpty(iID_MaChiTietGia)) { 
                iID_LoaiDonVi = HamChung.ConvertToString(R["iID_LoaiDonVi"]);
                rThueGTGT = HamChung.ConvertToString(R["rThueGTGT"]);
                rLoiNhuan = HamChung.ConvertToString(R["rLoiNhuan"]);
            }
        }
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        //đoạn code để khi chọn thêm mới
        using (Html.BeginForm("EditSubmit", "SP_ChiTietGia", new { ParentID = ParentID, iID_MaChiTietGia = iID_MaChiTietGia }))
        {
    %>
    <%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>
    <div id="idDialog" style="display: none;">
    </div>
    <script type="text/javascript">
        function ChonLoaiDV(gt) {
            if (gt == 3) {
                document.getElementById("row_don_vi").style.display = "none";
                document.getElementById("row_don_vi_dh").style.display = "";
            } else {
                document.getElementById("row_don_vi").style.display = "";
                document.getElementById("row_don_vi_dh").style.display = "none";
            }
            if (gt == 2) {
                document.getElementById("row_don_vi_sx").style.display = "";
            } else {
                document.getElementById("row_don_vi_sx").style.display = "none";
            }
            ChonDonVi();
        }
        function ChonDonVi() {
            if (LoaiDonVi != 1) {
                var LoaiDonVi = document.getElementById("<%=ParentID %>_iID_LoaiDonVi").value;
                var DonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;
                var iID_MaDonVi_SX = document.getElementById("<%=ParentID %>_iID_MaDonVi_SX").value;
                var iID_MaDonVi_DH = document.getElementById("<%=ParentID %>_iID_MaDonVi_DH").value;
                var iID_MaLoaiHinh = document.getElementById("<%=ParentID %>_iID_MaLoaiHinh").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Get_dtThongTinChiTietGia?ParentID=#0&iID_LoaiDonVi=#1&iID_MaDonVi=#2&iID_MaSanPham=#3&iID_MaLoaiHinh=#4", "SP_ChiTietGia") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", LoaiDonVi));
                if (LoaiDonVi == 2) {
                    url = unescape(url.replace("#2", iID_MaDonVi_SX));
                } else if (LoaiDonVi == 3) {
                    url = unescape(url.replace("#2", iID_MaDonVi_DH));
                }
                url = unescape(url.replace("#3", "<%= iID_MaSanPham %>"));
                url = unescape(url.replace("#4", iID_MaLoaiHinh));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_rSoLuong").value = data.rSoLuong;
                    document.getElementById("<%= ParentID %>_rLoiNhuan").value = data.rLoiNhuan;
                    document.getElementById("<%= ParentID %>_rThueGTGT").value = data.rThueGTGT;
                    document.getElementById("<%= ParentID %>_sQuyCach").value = data.sQuyCach;

                    document.getElementById("<%= ParentID %>_rSoLuong_show").value = data.rSoLuong;
                    document.getElementById("<%= ParentID %>_rLoiNhuan_show").value = data.rLoiNhuan;
                    document.getElementById("<%= ParentID %>_rThueGTGT_show").value = data.rThueGTGT;
                });
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham"), "Danh sách sản phẩm")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin sản phẩm</span>
                    </td>
                    <td align="right">
                        <input id="Button3" type="submit" class="button_title" value="Lưu" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Mã sản phẩm:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div style="float:left"><%=NgonNgu.LayXau(sMa)%></div>
                            <%= Html.Hidden(ParentID + "_sMa", sMa)%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tên sản phẩm:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div style="float:left"><%=NgonNgu.LayXau(sTen)%></div>
                            <%= Html.Hidden(ParentID + "_sTen", sTen)%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Loại đơn vị:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <select id="<%=ParentID %>_iID_LoaiDonVi" name="<%=ParentID %>_iID_LoaiDonVi" onchange = "ChonLoaiDV(this.value);" class="input1_2" style="width:99%" <%if(!String.IsNullOrEmpty(iID_MaChiTietGia)){ %> disabled="disabled" <%} %>>
                                <option value = "1" <%if(iID_LoaiDonVi == "1"){ %> selected="selected" <%} %> >Sản xuất</option>
                                <option value = "2" <%if(iID_LoaiDonVi == "2"){ %> selected="selected" <%} %> >Đặt hàng</option>
                                <option value = "3" <%if(iID_LoaiDonVi == "3"){ %> selected="selected" <%} %> >Cục tài chính</option>
                            </select>
                            <%if(!String.IsNullOrEmpty(iID_MaChiTietGia)){ %> 
                            <%= Html.Hidden(ParentID + "_iID_LoaiDonVi", iID_LoaiDonVi)%>
                            <%} %>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr id = "row_don_vi" <%if(iID_LoaiDonVi == "3"){ %> style="display:none" <%} %>>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Đơn vị:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "onchange = \"ChonDonVi()\" class=\"input1_2\"")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr id = "row_don_vi_sx" style="display:none">
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Đơn vị sản xuất:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi_SX", "", "onchange = \"ChonDonVi()\" class=\"input1_2\"")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr id = "row_don_vi_dh" style="display:none">
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Đơn vị đặt hàng:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi_DH", "", "onchange = \"ChonDonVi()\" class=\"input1_2\"")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Loại sửa chữa:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <select id="<%=ParentID %>_iID_MaLoaiHinh" name="<%=ParentID %>_iID_MaLoaiHinh" class="input1_2" style="width:99%" onchange = "ChonDonVi();" <%if(!String.IsNullOrEmpty(iID_MaChiTietGia)){ %> disabled="disabled" <%} %>>
                                <option value = "1" <%if(iID_MaLoaiHinh == "1"){ %> selected="selected" <%} %> >Sửa chữa lớn</option>
                                <option value = "2" <%if(iID_MaLoaiHinh == "2"){ %> selected="selected" <%} %> >Sửa chữa vừa</option>
                                <option value = "3" <%if(iID_MaLoaiHinh == "3"){ %> selected="selected" <%} %> >Sửa chữa nhỏ</option>
                            </select>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Số lượng sản phẩm:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rSoLuong, "rSoLuong", "", "class=\"input1_2\" tab-index='-1' ")%>
                            <%= Html.Hidden(ParentID + "_iDM_MaDonViTinh", iDM_MaDonViTinh)%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Lợi nhuận dự kiến (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rLoiNhuan, "rLoiNhuan", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Thuế giá trị gia tăng (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rThueGTGT, "rThueGTGT", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Quy cách phẩm chất:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextArea(ParentID, sQuyCach, "sQuyCach", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        ChonDonVi();
    </script>
    <%  } %>
    <br />
</asp:Content>
