<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String MaDonViCha = Convert.ToString(ViewData["iID_MaDonViCha"]);
        String ParentID = "Edit";
        NameValueCollection data = (NameValueCollection)ViewData["data"];
        DataTable dtLoaiDonVi = DanhMucModels.DT_DanhMuc("LoaiDonVi", true, "--- Chọn loại đơn vị ---");
        DataTable dtNhomDonVi = DanhMucModels.DT_DanhMuc("NhomDonVi", true, "--- Chọn nhóm đơn vị ---");
        DataTable dtKhoiDonVi = DanhMucModels.DT_DanhMuc("KhoiDonVi", true, "--- Chọn khối đơn vị ---");
        DataTable dt = DanhMucModels.Get_OneRow_DonVi(MaDonVi);

        SelectOptionList optLoaiDonVi = new SelectOptionList(dtLoaiDonVi, "iID_MaDanhMuc", "sTen");
        SelectOptionList optNhomDonVi = new SelectOptionList(dtNhomDonVi, "iID_MaDanhMuc", "sTen");
        SelectOptionList optKhoiDonVi = new SelectOptionList(dtKhoiDonVi, "iID_MaDanhMuc", "sTen");

        String sTen = "", sMoTa = "", iID_Ma="", iID_MaDonViCha = "", iID_MaPhongBan = "", MaKhoiDonVi = "", MaLoaiDonVi = "", MaNhomDonVi = "";
       // int iCapNS = 1;
        if (dt.Rows.Count > 0)
        {
            sTen = dt.Rows[0]["sTen"].ToString();
            sMoTa = dt.Rows[0]["sMoTa"].ToString();
            iID_Ma = dt.Rows[0]["iID_Ma"].ToString();
            iID_MaDonViCha = dt.Rows[0]["iID_MaDonViCha"].ToString();
            iID_MaPhongBan = dt.Rows[0]["iID_MaPhongBan"].ToString();
            MaLoaiDonVi = dt.Rows[0]["iID_MaLoaiDonVi"].ToString();
            MaNhomDonVi = dt.Rows[0]["iID_MaNhomDonVi"].ToString();
            MaKhoiDonVi = dt.Rows[0]["iID_MaKhoiDonVi"].ToString();
         //   iCapNS = Convert.ToInt16(dt.Rows[0]["iID_MaKhoiDonVi"].ToString());
        }
        //tao datatable iCapNS
        DataTable dtCapNS = new DataTable();
        dtCapNS.Columns.Add("iCapNS", typeof(String));
        DataRow r = dtCapNS.NewRow();
        r[0] = "1";
        dtCapNS.Rows.Add(r);

        r = dtCapNS.NewRow();
        r[0] = "2";
        dtCapNS.Rows.Add(r);

        r = dtCapNS.NewRow();
        r[0] = "3";
        dtCapNS.Rows.Add(r);

        SelectOptionList slCapNS = new SelectOptionList(dtCapNS, "iCapNS", "iCapNS");
        dtCapNS.Dispose();
        String strReadOnlyMa = "";
        String strIcon = "";
        if (ViewData["DuLieuMoi"] == "0")
        {
            strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }

        
        using (Html.BeginForm("EditSubmit", "DonVi", new { ParentID = ParentID, MaDonVi = MaDonVi }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>  
    <%= Html.Hidden(ParentID + "_iID_MaDonViCha", iID_MaDonViCha)%>
    <%= Html.Hidden(ParentID + "_iID_Ma", iID_Ma)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
       
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Nhập thông tin đơn vị</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="70%" class="table_form2">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Khối đơn vị</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optKhoiDonVi, data, "iID_MaKhoiDonVi", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKhoiDonVi")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Loại đơn vị</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optLoaiDonVi, data, "iID_MaLoaiDonVi", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaLoaiDonVi")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Nhóm đơn vị</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optNhomDonVi, data, "iID_MaNhomDonVi", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNhomDonVi")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Mã đơn vị (tối đa 5 ký tự)</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaDonVi", "", "style=\"width:20%;\" " + strReadOnlyMa + "", 2)%><%=strIcon %>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iMaDonVi")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Tên đơn vị</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sTen", "", "style=\"width:80%;\"")%><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Mô tả</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sMoTa", "", "style=\"width:80%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="70%" class="table_form2">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Mã quan hệ ngân sách</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sMaSo", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaSo")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Số tài khoản</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sSoTaiKhoan", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sSoTaiKhoan")%>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Địa chỉ</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sDiaChi", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sDiaChi")%>
                                        </div>
                                    </td>
                                    </tr>
                                   <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            Kho bạc</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, data, "sKhoBac", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_sKhoBac")%>
                                        </div>
                                    </td>
                                </tr>
                                  <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                         Hưởng lương</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, data, "iHuongLuong", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iHuongLuong")%>
                                        </div>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                         Cấp ngân sách</div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slCapNS,data, "iCapNS", null, "style=\"width: 49%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iCapNS")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
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
                            <input type="submit" class="button4" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
}
    %>
</asp:Content>
