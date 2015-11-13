<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    
    String ParentID = "QLDA_CapPhat";
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

    int iDotMoi = 0;
    if (QLDA_CapPhatModels.Get_Max_Dot(NamLamViec) != "") { 
        iDotMoi = Convert.ToInt32(QLDA_CapPhatModels.Get_Max_Dot(NamLamViec)) + 1;
    };
    Boolean bThemMoi = false;
    String iThemMoi = "";
    if (ViewData["bThemMoi"] != null)
    {
        bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
        if (bThemMoi)
            iThemMoi = "on";
    }
    String iID_MaDotCapPhat="",sNoiDungCapPhat="",dNgayLap="";
    iID_MaDotCapPhat = Convert.ToString(ViewData["iID_MaDotCapPhat"]);
    String SQL = @"SELECT CONVERT(varchar,dNgayLap,103) as NgayLap,* FROM QLDA_CapPhat_Dot
                WHERE iID_MaDotCapPhat=@iID_MaDotCapPhat";
    SqlCommand cmd= new SqlCommand(SQL);
    cmd.Parameters.AddWithValue("@iID_MaDotCapPhat",iID_MaDotCapPhat);
    DataTable dt = Connection.GetDataTable(cmd);
    if(dt.Rows.Count>0)
        {
            sNoiDungCapPhat = Convert.ToString(dt.Rows[0]["sNoiDungCapPhat"]);
            dNgayLap = Convert.ToString(dt.Rows[0]["NgayLap"]);
           
        }
    using (Html.BeginForm("SuaSubmit", "QLDA_CapPhat", new { ParentID = ParentID, iID_MaDotCapPhat = iID_MaDotCapPhat }))
    {
%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_CapPhat"), "Đợt cấp phát")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Sửa đợt cấp phát</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" width="50%" class="table_form2">
                <tr>
                    <td style="width: 50%">
                        <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2" id="tb_DotNganSach">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Số cấp phát")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.Label(iDotMoi,"iDot")%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Ngày")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayLap, "dNgayLap", "", "class=\"input1_2\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div><%=NgonNgu.LayXau("Nội dung cấp phát")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDungCapPhat, "sNoiDungCapPhat", "", "class=\"input1_2\" style=\"height:60px\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDungCapPhat")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;"><div></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                                <td width="30%" align="right" class="td_form2_td5">
                                                    <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Lưu")%>" />
                                                </td>          
                                                    <td width="5px">&nbsp;</td>          
                                                <td class="td_form2_td5">
                                                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%} %>

</asp:Content>
