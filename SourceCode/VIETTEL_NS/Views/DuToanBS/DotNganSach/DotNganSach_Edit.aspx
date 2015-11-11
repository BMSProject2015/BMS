<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "Edit";
    String bChiNganSach = Convert.ToString(Request.QueryString["ChiNganSach"]);
    String MaDotNganSach = Convert.ToString(ViewData["MaDotNganSach"]);

    DataTable dtDotNganSach = DuToan_DotNganSachModels.GetDotNganSach(MaDotNganSach);
    DataRow R;
    String iNamLamViec = "", NgayDotNganSach = "", iID_MaNamNganSach = "", iID_MaNguonNganSach = "";
    String TenNamNganSach = "", TenNguonNganSach = "";
    if (dtDotNganSach.Rows.Count > 0) {
        R = dtDotNganSach.Rows[0];
        iNamLamViec = Convert.ToString(R["iNamLamViec"]);
        iID_MaNamNganSach = Convert.ToString(R["iID_MaNamNganSach"]);
        iID_MaNguonNganSach = Convert.ToString(R["iID_MaNguonNganSach"]);
        NgayDotNganSach = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayDotNganSach"]));
        
        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        for (i = 0; i < dtNamNganSach.Rows.Count; i++) {
            if (iID_MaNamNganSach == Convert.ToString(dtNamNganSach.Rows[i]["iID_MaNamNganSach"]))
            {
                TenNamNganSach = Convert.ToString(dtNamNganSach.Rows[i]["sTen"]);
                break;
            }
        }
        DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
        for (i = 0; i < dtNguonNganSach.Rows.Count; i++)
        {
            if (iID_MaNguonNganSach == Convert.ToString(dtNguonNganSach.Rows[i]["iID_MaNguonNganSach"]))
            {
                TenNguonNganSach = Convert.ToString(dtNguonNganSach.Rows[i]["sTen"]);
                break;
            }
        }
    }

    using (Html.BeginForm("EditSubmit", "DuToan_DotNganSach", new { ParentID = ParentID, MaDotNganSach = MaDotNganSach, ChiNganSach = bChiNganSach }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>Sửa thông tin đợt ngân sách</span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 50%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 35%;">
                            <div>Năm làm việc</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn năm ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextBox(ParentID, TenNamNganSach, "NamNganSach", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn nguồn ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, TenNguonNganSach, "NguonNganSach", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Ngày đợt ngân sách") %></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, NgayDotNganSach, "dNgayDotNganSach", "", "class=\"input1_2\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayDotNganSach")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Sửa" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
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
<%
    }       
%>
</asp:Content>
