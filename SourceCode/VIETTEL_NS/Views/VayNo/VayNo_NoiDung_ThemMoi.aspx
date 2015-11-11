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
        string errorKey = Convert.ToString(ViewData["ErrorKey"]);
        string ID = Convert.ToString(ViewData["ID"]);
        string sTenNoiDung = string.Empty;
        string sMoTaChung = string.Empty;
        string dNgayTao = string.Empty;
        string sLoai = string.Empty;
        string sHoatDong = string.Empty;
        string iID_MaNoiDung = string.Empty;
        string MaLoaiVayVon = string.Empty;
        DataTable dtNoiDung = VayNoModels.LayThongTinNoiDung(ID);
        if (dtNoiDung.Rows.Count != 0)
        {
            DataRow dataRow = dtNoiDung.Rows[0];
            iID_MaNoiDung = Convert.ToString(dataRow["iID_MaNoiDung"]);
            sHoatDong = Convert.ToString(dataRow["bPublic"]);
            MaLoaiVayVon = Convert.ToString(dataRow["iID_Loai"]);
            sTenNoiDung = Convert.ToString(dataRow["sTenNoiDung"]);
            sMoTaChung = Convert.ToString(dataRow["sMoTaChung"]);
            dNgayTao = Convert.ToString(dataRow["dNgayTao"]);
        }
        //lấy danh mục loại vay vốn

        DataTable dtLoaiVayVon = DanhMucModels.DT_DanhMuc("LoaiNoiDungVayVon", false, "--- Chọn loại vay vốn ---");
        SelectOptionList optLoaiVayVon = new SelectOptionList(dtLoaiVayVon, "iID_MaDanhMuc", "sTen");
        if (dtLoaiVayVon != null) dtLoaiVayVon.Dispose();
    %>
    <% using (Html.BeginForm("EditSubmitNoiDung", "VayNo_NoiDung", new { ParentID = ParentID }))
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
      
        <div id="nhapform">
            <div id="form2">
                <div style="width: 50%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Tên nội dung&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, sTenNoiDung, "sTenNoiDung", "", "class=\"input1_2\" tab-index='-1'")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sTenNoiDung")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" style="width: 15%;">
                                <div>
                                    Mã nội dung&nbsp;<span  style="color:Red;">*</span></div>
                            </td>
                            <td class="td_form2_td5">
                                <%
                                    if (ViewData["DuLieuMoi"] == "1")
                                    {
                                %>
                                <div>
                                    <%=MyHtmlHelper.TextBox(ParentID, iID_MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNoiDung")%>
                                </div>
                                <%
                                    }
                                else
                                {
                                %>
                                <div>
                                    <b>
                                        <%=iID_MaNoiDung%>
                                    </b>
                                </div>
                                <%
                                    }
                                %>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div>
                                    Loại</div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                   <%=MyHtmlHelper.DropDownList(ParentID, optLoaiVayVon, MaLoaiVayVon, "iID_Loai", "", "class=\"input1_2\" ")%>
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
