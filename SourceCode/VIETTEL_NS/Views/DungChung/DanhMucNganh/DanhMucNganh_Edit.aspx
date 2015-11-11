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
         String ParentID = "DanhMucNganh";
         String iID = Convert.ToString(ViewData["iID"]);
         String sTenNganh = "", iiD_MaNganhMLNS="",sMaNguoiQuanLy="",iID_MaNganh="";
         DataTable dt = MucLucNganSach_NganhModels.Get_dtMucLucNganSach_Nganh(iID);
         if (dt.Rows.Count > 0)
         {
             iID_MaNganh = Convert.ToString(dt.Rows[0]["iID_MaNganh"]);
             sTenNganh =Convert.ToString(dt.Rows[0]["sTenNganh"]);
             iiD_MaNganhMLNS = Convert.ToString(dt.Rows[0]["iiD_MaNganhMLNS"]);
             sMaNguoiQuanLy = Convert.ToString(dt.Rows[0]["sMaNguoiQuanLy"]);
             
         }
         
         DataTable dtMLNS = MucLucNganSach_NganhModels.LayDanhSachMLNS_Nganh();
         String[] arrMLNS_Nganh = iiD_MaNganhMLNS.Split(',');
         String[] arrMaNguoiQuanLy = sMaNguoiQuanLy.Split(',');
         DataTable dtNguoiDung = DanhMucModels.getNguoiDung();
         String BackURL = Url.Action("Index", "DanhMucNganh");
         String readOnly="readonly=\"readonly\"";
         if (ViewData["DuLieuMoi"] == "1") readOnly = "";
         using (Html.BeginForm("EditSubmit", "DanhMucNganh", new { ParentID = ParentID, iID = iID }))
    {
     %>
     <%=MyHtmlHelper.Hidden("",ViewData["DuLieuMoi"],"DuLieuMoi","") %>
     <div class="box_tong">
         <div class="title_tong">
             <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	<td>              
                	<span>Cấu hình MLNS-Ngành</span>                                                         
                </td>
               
                </tr>
              </table>
        </div>
         <div id="Div1">
         <div id="Div2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="100%"  class="table_form2">
                 <tr>
                 <td width="30%"></td>
                 <td class="td_form2_td1" width="10%"><div>Mã Ngành: &nbsp;</div></td>
                 <td width="30%"><div><%=MyHtmlHelper.TextBox(ParentID, iID_MaNganh, "iID_MaNganh", null, "style=\"width: 50%;\"" + readOnly + "")%>                         
                        
                         <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNganh")%>
                         </div>
                        </td>
                 <td></td>
                 </tr>
                 <tr>
                 <td></td>
                 <td class="td_form2_td1"><div>Tên Ngành: &nbsp;</div></td>
                 <td><div><%=MyHtmlHelper.TextBox(ParentID,sTenNganh, "sTenNganh", null, "style=\"width: 50%;\"")%>   </div>
                   </td>
                   <td></td>
                 </tr>
                 <tr>
                 <td></td>
                 <td class="td_form2_td1"><div>Mã Ngành MLNS: &nbsp;</div></td>

                 <td>
                  <div style="width: 50%; height: 200px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">                                                                                                          
                                    <%
                                    String TenLNS = ""; String sLNS1 = "";
                                    String _Checked = "checked=\"checked\"";
                                    for (int i = 0; i < dtMLNS.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtMLNS.Rows[i]["sNG"]);
                                        sLNS1 = Convert.ToString(dtMLNS.Rows[i]["sNG"]);
                                        for (int j = 0; j < arrMLNS_Nganh.Length; j++)
                                        {
                                            if (sLNS1 == arrMLNS_Nganh[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox1" 
                                                name="iID_MaNganhMLNS" />                                        </td>
                                        <td>
                                            <%=TenLNS%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div> 

                 
                 </td>
                 <td></td>
                 </tr>
                  <tr>
                 <td></td>
                 <td class="td_form2_td1"><div>Người quản lý ngành: &nbsp;</div></td>

                 <td>
                  <div style="width: 50%; height: 200px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">                                                                                                          
                                    <%
                                     TenLNS = "";  sLNS1 = "";
                                     _Checked = "checked=\"checked\"";
                                     for (int i = 0; i < dtNguoiDung.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtNguoiDung.Rows[i]["sID_MaNguoiDung"]);
                                        sLNS1 = Convert.ToString(dtNguoiDung.Rows[i]["sID_MaNguoiDung"]);
                                        for (int j = 0; j < arrMaNguoiQuanLy.Length; j++)
                                        {
                                            if (sLNS1 == arrMaNguoiQuanLy[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox2" 
                                                name="sMaNguoiQuanLy" />                                        </td>
                                        <td>
                                            <%=TenLNS%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div> 

                 
                 </td>
                 <td></td>
                 </tr>
                 <tr>
                       <td></td>
                  <td colspan="2"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
                             <td></td> 
                 </tr>
            </table>
        </div>
        </div>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        
        </script>

    </div>
     <%} %>
 </asp:Content>
