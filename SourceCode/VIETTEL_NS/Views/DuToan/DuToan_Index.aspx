<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
 <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        String ParentID = "DuToan";        
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);


        DataTable dtNam = DanhMucModels.DT_Nam();
        
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        
        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "iID_MaNamNganSach", "sTen");
        DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
        SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "iID_MaNguonNganSach", "sTen");

        DataTable dtCH = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
        String NamNS="1", NguonNS="2";
        if (dtCH.Rows.Count > 0)
        {
            NamHienTai = Convert.ToString(dtCH.Rows[0]["iNamLamViec"]);
            NamNS = Convert.ToString(dtCH.Rows[0]["iID_MaNamNganSach"]);
            NguonNS = Convert.ToString(dtCH.Rows[0]["iID_MaNguonNganSach"]);
        }
        
        using (Html.BeginForm("EditSubmit", "DuToan", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Dự toán</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div><%=NgonNgu.LayXau("Chọn năm làm việc")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "MaNam", "", "class=\"input1_2\" style=\"width: 50%\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_MaNam")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div><%=NgonNgu.LayXau("Chọn năm ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, NamNS, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width: 50%\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNamNganSach")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn nguồn ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNguonNganSach,NguonNS, "iID_MaNguonNganSach", "", "class=\"input1_2\" style=\"width: 50%\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%
        }
        dtNamNganSach.Dispose();
        dtNguonNganSach.Dispose();         
    %>
</asp:Content>
