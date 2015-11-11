<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">     

    <% 
        String sKyHieu = Request.QueryString["sKyHieu"];
        String ControllerName = Request.QueryString["ControllerName"];
       
        String iID_MaTaiKhoan = rptKeToan_DanhMucTaiKhoanGiaiThichController.sThamSo(sKyHieu);
        String[] arrTaiKhoan = iID_MaTaiKhoan.Split(',');

        DataTable dtTaiKhoan = rptKeToan_DanhMucTaiKhoanGiaiThichController.DanhSachTaiKhoan();
        String MaTK = "", TenTK = "";
        using (Html.BeginForm("EditSubmit", "rptKeToan_DanhMucTaiKhoanGiaiThich", new { sKyHieu = sKyHieu, ControllerName = ControllerName }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
        </div>
        <div id="table_form2" class="table_form2">
            <div id="rptMain" style="width: 100%;  margin: 0px auto; padding: 0px 0px;
                overflow: visible;">
                <table cellpadding="0"  cellspacing=""="0" border="0" width="100%"  class="table_form2">
                 <tr>
                 <td width="10%"></td>
                 <td class="td_form2_td1" width="20%"><div>Tài khoản: &nbsp;</div></td>

                 <td>
                  <div style="width: 50%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">                                                                                                          
                                    <%
                                    String _Checked = "checked=\"checked\"";
                                    for (int i = 0; i < dtTaiKhoan.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenTK = Convert.ToString(dtTaiKhoan.Rows[i]["sTen"]);
                                        MaTK = Convert.ToString(dtTaiKhoan.Rows[i]["sKyHieu"]);
                                        TenTK = MaTK + "-" + TenTK;
                                        for (int j = 0; j < arrTaiKhoan.Length; j++)
                                        {
                                            if (MaTK == arrTaiKhoan[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=MaTK %>" <%=_Checked %> check-group="sMaTK" id="MaTK" 
                                                name="MaTK" />                                        </td>
                                        <td>
                                              <%=TenTK%>                                </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div> 

                 
                 </td>

                 </tr>
                 <tr>
                       <td></td>
                  <td colspan="2"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />                                    </td>
                                </tr>
                           </table></td> 
                             <td></td> 
                 </tr>
            </table>
            </div>
        </div>
    </div>
    <%} %>

</asp:Content>
