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
        String ParentID = "Edit";
        string ID = Convert.ToString(ViewData["ID"]);
        string sHoatDong = string.Empty;
        string iID_MaLoaiThuMuc = string.Empty;
        string sTen = string.Empty;
        DataTable dt;
        if (Convert.ToString(ViewData["DuLieuMoi"]).Equals("0"))
        {
            dt = TuLieuLichSuModels.GetThuMuc(ID);
            if (dt != null && dt.Rows.Count != 0)
            {
                DataRow dataRow = dt.Rows[0];
                iID_MaLoaiThuMuc = Convert.ToString(dataRow["iID_MaLoaiThuMuc"]);
                sHoatDong = Convert.ToString(dataRow["bHoatDong"]);
                sTen = Convert.ToString(dataRow["sTen"]);
            }
        }
       
        //lấy danh mục loại vay vốn

        DataTable dtLoaiThuMuc = DanhMucModels.DT_DanhMuc("LoaiThuMuc", false, "--- Chọn loại thư mục ---");
        SelectOptionList optLoaiThuMuc = new SelectOptionList(dtLoaiThuMuc, "iID_MaDanhMuc", "sTen");
        if (dtLoaiThuMuc != null) dtLoaiThuMuc.Dispose();
    %>
    <% using (Html.BeginForm("EditSubmitThuMuc", "TuLieu_ThuMuc", new { ParentID = ParentID }))
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
                            <%=NgonNgu.LayXau("Thêm mới thư mục")%>
                            <%
                                }
                                else
                                {
                            %>
                            <%=NgonNgu.LayXau("Sửa thông tin thư mục")%>
                            <%
                                }
                            %>&nbsp; &nbsp; </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <div style="width: 50%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Đường dẫn&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" tab-index='-1'")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Loại thư mục&nbsp;<span style="color: Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, optLoaiThuMuc, iID_MaLoaiThuMuc, "iID_MaLoaiThuMuc", "", "class=\"input1_2\" ")%>
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
                                    <%=MyHtmlHelper.CheckBox(ParentID, sHoatDong, "bHoatDong", "")%>
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
