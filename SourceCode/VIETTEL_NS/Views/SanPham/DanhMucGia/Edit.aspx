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
        String iID_MaSanPham = Convert.ToString(Request.QueryString["iID_MaSanPham"]);
        String iID_MaDanhMucGia = Convert.ToString(ViewData["iID_MaDanhMucGia"]);
        String iID_MaDanhMucGia_Cha = Convert.ToString(ViewData["iID_MaDanhMucGia_Cha"]);
        String iID_MaLoaiHinh = Convert.ToString(ViewData["iID_MaLoaiHinh"]);
        String ParentID = "Edit";
        String sKyHieu = "", sTen = "", iLoai = "";
        bool bLaHangCha = false;
        
        //chi tiết chỉ tiêu nếu trong trường hợp sửa
        DataTable dt = SanPham_DanhMucGiaModels.Get_ChiTietDanhMucGia_Row(iID_MaDanhMucGia);
        DataRow R;
        if (dt.Rows.Count > 0 && iID_MaDanhMucGia != null && iID_MaDanhMucGia != "")
        {
            R = dt.Rows[0];
            sKyHieu = HamChung.ConvertToString(R["sKyHieu"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
            iLoai = Convert.ToString(R["iLoai"]);
            bLaHangCha = Convert.ToBoolean(R["bLaHangCha"]);
        }
        String tgLaHangCha = "";
        if (bLaHangCha == true)
        {
            tgLaHangCha = "on";
        }
        //lấy dữ liệu đưa vào Combobox
        //
        SqlCommand cmd;
        cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
        dt = Connection.GetDataTable(cmd);
        R = dt.NewRow();
        R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
        R["sTen"] = "-- Đơn vị tính --";
        dt.Rows.InsertAt(R, 0);
        SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
        cmd.Dispose();
        String iDM_MaDonViTinh = Convert.ToString(ViewData["iDM_MaDonViTinh"]);


        using (Html.BeginForm("EditSubmit", "SanPham_DanhMucGia", new { ParentID = ParentID, iID_MaDanhMucGia = iID_MaDanhMucGia }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>
    <%= Html.Hidden(ParentID + "_iID_MaLoaiHinh", iID_MaLoaiHinh)%>
    <%= Html.Hidden(ParentID + "_iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMucGia"), "Cấu hình danh mục")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span>Nhập thông tin khoản mục</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin khoản mục</span>
                    <% } %>
                       
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã khoản mục</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%
                                String strReadonly = "";
                                if (ViewData["DuLieuMoi"] == "0") {
                                    strReadonly = "readonly=\"readonly\""; 
                                }    
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", " " + strReadonly + " class=\"input1_2\" tab-index='-1'", 2)%><br />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên khoản mục</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Đơn vị tính</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"><div><b>Là hàng cha</b></div></td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.CheckBox(ParentID, tgLaHangCha, "bLaHangCha", String.Format("value='{0}'", bLaHangCha))%></div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
</asp:Content>
