<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Globalization" %>
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
        String ParentID = "Edit";
        string errorKey = Convert.ToString(ViewData["ErrorKey"]);
        string ID = Convert.ToString(ViewData["ID"]);
        string sTenNoiDung = string.Empty;
        string sMoTaChung = string.Empty;
        string dNgayTao = string.Empty;
        string sLoai = string.Empty;
        string sHoatDong = string.Empty;
        string iID_MaNoiDung = string.Empty;
        DataTable dtNoiDung = VayNoModels.LayThongTinNoiDung(ID);
        if (dtNoiDung.Rows.Count != 0)
        {
            DataRow dataRow = dtNoiDung.Rows[0];
            iID_MaNoiDung = Convert.ToString(dataRow["iID_MaNoiDung"]);
            sHoatDong = Convert.ToString(dataRow["bHoatDong"]);
            sLoai = Convert.ToString(dataRow["iLoai"]);
            sTenNoiDung = Convert.ToString(dataRow["sTenNoiDung"]);
            sMoTaChung = Convert.ToString(dataRow["sMoTaChung"]);
            if (dataRow["dNgayTao"] != DBNull.Value)
            {
                DateTime objDate = Convert.ToDateTime(dataRow["dNgayTao"]);
                dNgayTao = objDate.ToString("dd/MM/yyyy");
            }
        }

        DataTable dtLoaiVayVon = DanhMucModels.DT_DanhMuc("LoaiVayVon", false, "--- Chọn loại vay vốn ---");
        SelectOptionList optLoaiVayVon = new SelectOptionList(dtLoaiVayVon, "iID_MaDanhMuc", "sTen");
        if (dtLoaiVayVon != null) dtLoaiVayVon.Dispose();
    %>
    <% using (Html.BeginForm("EditSubmitNoiDung", "VayVon_NoiDung", new { ParentID = ParentID }))
       {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_ID", ID)%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>
                            <%
                                if (ViewData["DuLieuMoi"] == "1")
                                {
                            %>
                            <%=NgonNgu.LayXau("Thêm mới nội dung")%>
                            <%
                                }
                                else
                                {
                            %>
                            <%=NgonNgu.LayXau("Sửa thông tin nội dung")%>
                            <%
                                }
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <% if (errorKey.Equals("1"))
{
    %>
    <script type="text/javascript">
        alert("Mã nội dung đã tồn tại");
    </script>
    <%
}
            %>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 50%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Mã nội dung</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iID_MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNoiDung")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Tên nội dung</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sTenNoiDung, "sTenNoiDung", "", "class=\"input1_2\"")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sTenNoiDung")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Loại</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                   <%=MyHtmlHelper.DropDownList(ParentID, optLoaiVayVon, MaLoaiVayVon, "iID_Loai", "", "class=\"input1_3\" ")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Mô tả</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextArea(ParentID, sMoTaChung, "sMoTaChung", "", "class=\"input1_2\" style=\"height: 40px;\"")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sMoTaChung")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Ngày tạo</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DatePicker(ParentID, dNgayTao, "dNgayTao", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayTao")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Hoạt động</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.CheckBox(ParentID, sHoatDong, "bPublic", "")%>
                                    <%--<%=MyHtmlHelper.CheckBox(ParentID, sHoatDong, "sHoatDong", "sHoatDong", "class=\"input1_2\"")%><br />--%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_bHoatDong")%>
                                     <%= Html.ListBox(ParentID + "_" + "err_bHoatDong")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">
                                                &nbsp;
                                            </td>
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="Submit1" value="Lưu" />
                                            </td>
                                            <td width="5px">
                                                &nbsp;
                                            </td>
                                            <td class="td_form2_td5">
                                                <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
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
    </div>
    <%} %>
</asp:Content>
