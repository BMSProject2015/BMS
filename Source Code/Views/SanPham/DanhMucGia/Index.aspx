<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string ParentID = "DanhMucGia"; 
        string iID_MaSanPham = "", sTenDanhMuc = "", sKyHieu = "", sTen = "", rSoLuong = "", sMa = "", iID_MaLoaiHinh = "";
        int i;
        iID_MaLoaiHinh = HamChung.ConvertToString(ViewData["iID_MaLoaiHinh"]);
        if (String.IsNullOrEmpty(iID_MaLoaiHinh)) iID_MaLoaiHinh = "1";
        iID_MaSanPham =  HamChung.ConvertToString(ViewData["iID_MaSanPham"]);
        DataTable dt = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
        DataRow R;
        if (dt.Rows.Count > 0) {
            R = dt.Rows[0];
            sTen = HamChung.ConvertToString(R["sTen"]);
            rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
            sMa = HamChung.ConvertToString(R["sMa"]);
        }
        //đoạn code để khi chọn thêm mới
        String strThemMoi = Url.Action("Edit", "SanPham_DanhMucGia", new { iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
        //
        String strSort = Url.Action("Sort", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = string.Empty, iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
        //sự kiện tìm kiếm được chọn
        String urlFrame = Url.Action("Frame", "SanPham_DanhMucGia", new { iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
        
        using (Html.BeginForm("SaveSubmit", "SanPham_DanhMucGia", new { ParentID = ParentID, iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }))
        {
    %>
    <%--<%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>--%>
    <script type="text/javascript">
        function reloadFrame() {
            $("#frame_danh_muc").load("<%=urlFrame %>");
        }
        function CallSuccess_CT() {
            Bang_ShowCloseDialog();
            location_reload();
            return false;
        }
        function OnInit_CT() {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = 'Kế toán tổng hợp';
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: 800,
                modal: true
            });
        }
        function OnInit_CT_NEW(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            document.getElementById("idDialog").style.top = "100px";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true
            });
        }
        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }
        function ChonLoaiSuaChua(loai) {
            var url = "<%=Url.Action("Index", "SanPham_DanhMucGia", new { iID_MaSanPham = iID_MaSanPham })%>";
            location.href = url + "&iID_MaLoaiHinh=" + loai;
        }
    </script>
    <div id="idDialog" style="display: none;">
    </div>
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
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td5">
                            <div><b><%=sMa%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tên sản phẩm:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td5">
                            <div><b><%=sTen%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Loại sửa chữa:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td5">
                            <div>
                                <select id="<%=ParentID %>_iID_MaLoaiHinh" name="<%=ParentID %>_iID_MaLoaiHinh" class="input1_2" style="width:100%" onchange="ChonLoaiSuaChua(this.value);">
                                    <option value = "1" <%if(iID_MaLoaiHinh == "1"){ %> selected="selected" <%} %> >Sửa chữa lớn</option>
                                    <option value = "2" <%if(iID_MaLoaiHinh == "2"){ %> selected="selected" <%} %> >Sửa chữa vừa</option>
                                    <option value = "3" <%if(iID_MaLoaiHinh == "3"){ %> selected="selected" <%} %> >Sửa chữa nhỏ</option>
                                </select>
                            </div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
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
                      <span>Danh sách khoản mục</span>
                    </td>
                    <td align="right">
                        <%--<input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                        <input id="Button1" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSort %>'" />--%>
                    </td>
                </tr>
            </table>
            
        </div>
        <div id="frame_danh_muc">
        <table class="mGrid">
            <tr>
                <th style="width: 10%;" align="center">
                    Mã  khoản mục
                </th>
                <th style="width: 50%;" align="center">
                    Tên khoản mục
                </th>
                <th style="width: 10%;" align="center">
                    Đơn vị tính
                </th>
                <th style="width: 30%;" align="center">
                    Hành động
                </th>
            </tr>
            <%
                int ThuTu = 0;
                ArrayList listChiTiet = SanPham_DanhMucGiaModels.LayDanhSachDanhMuc(iID_MaSanPham,iID_MaLoaiHinh, "", 0, ref ThuTu);
                int c = listChiTiet.Count;

                //Cac ma co dinh
                String[] arrKyHieuMucCon = "2.2.1,2.2.2,3.1.1,3.1.2,3.2.1,3.2.2,4.1.1,4.1.2,4.2.1,4.2.2,5.1.1,5.1.2,5.2.1,5.2.2".Split(',');
                Hashtable hsKyHieuMucCon = new Hashtable();
                foreach(String key in arrKyHieuMucCon){
                    hsKyHieuMucCon.Add(key, true);
                }
            %>
            <%--<%=SanPham_DanhMucGiaModels.LayXauChiTieu(sTenDanhMuc, sKyHieu, Url.Action("", ""), XauHanhDong, XauThemVT, XauThemCon, XauSapXep, "", 0, ref ThuTu)%>--%>
        <%
            foreach(Hashtable row in listChiTiet){
                string urlDelete = Url.Action("Delete", "SanPham_DanhMucGia", new { iID_MaDanhMucGia = row["iID_MaDanhMucGia"], iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
                string urlSort = Url.Action("Sort", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"], iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh });
                MvcHtmlString urlThemVatTu = Ajax.ActionLink("Thêm vật tư", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA_VT", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia = row["iID_MaDanhMucGia"], iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }, new AjaxOptions { }, new { });
                MvcHtmlString urlThemNhomCon = Ajax.ActionLink("Thêm nhóm con", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"], iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }, new AjaxOptions { }, new { });
                MvcHtmlString urlSua = Ajax.ActionLink("Sửa", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia = row["iID_MaDanhMucGia"], iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }, new AjaxOptions { }, new { });
                double dKyHieu = 0; 
                try
                {
                    dKyHieu = Convert.ToDouble(row["sKyHieu"]);
                }
                catch (System.Exception ex){}
        %>
            <tr>
                <%if ((bool)row["bLaHangCha"])
                    { %>
                    <td style="background-color:#dff0fb;padding: 3px 3px;"><b><%=row["sKyHieu"]%></b></td>
                    <td style="background-color:#dff0fb;padding: 3px 3px;"><b><%=row["sTen"]%></b></td>
                    <td style="background-color:#dff0fb;padding: 3px 3px;"><b><%=row["sTen_DonVi"]%></b></td>
                <td style="background-color: #dff0fb; padding: 3px 3px;" nowrap>
                    <%if(dKyHieu < 6) {%>
                    <%if ((int)row["laCha"] == 1){ %>
                    <span>
                        <%=MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "")%>|</span>
                    <%}
                        if ((int)row["laCha"] != 0 && hsKyHieuMucCon[(Convert.ToString(row["sKyHieu"])).Replace("&nbsp;","")] == null)
                            {%>
                    <span onclick="OnInit_CT_NEW(800, 'Thêm vật tư');">
                        <%= urlThemVatTu%>|</span>
                    <%}
                            if ((int)row["Cap"] == 0)
                            {
                    %>
                    <span onclick="OnInit_CT_NEW(400, 'Thêm nhóm con');">
                        <%= urlThemNhomCon%>| </span>
                    <%}
                            else if ((int)row["laCha"] == -1 && (int)row["DuocXoa"] == 1)
                        { %>
                    <span>
                        <%=MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "")%></span>|
                    <%} %>
                    <span onclick="OnInit_CT_NEW(400, 'Sửa');">
                        <%= urlSua%>
                    </span>
                    <%} %>
                </td>
                <% }
                    else
                    {     
                %>
                    <td style="padding: 3px 3px;"><%=row["sKyHieu"]%></td>
                    <td style="padding: 3px 3px;"><%=row["sTen"]%></td>
                    <td style="padding: 3px 3px;"><%=row["sTen_DonVi"]%></td>
                    <td style="padding: 3px 3px;">
                        <%if ((int)row["DuocXoa"] == 1)
                        { %>
                        <span><%=MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "")%></span>
                        <%} %>
                    </td>
                <%    }
                %>
            </tr>
        <% 
            }
        %>
        </table>
        </div>
    </div>
</asp:Content>
