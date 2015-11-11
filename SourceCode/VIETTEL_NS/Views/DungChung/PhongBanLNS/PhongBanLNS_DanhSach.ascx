<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
   // String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String sLNS = Convert.ToString(props["sLNS"].GetValue(Model));
    String LoaiNS = Convert.ToString(props["LNS"].GetValue(Model));
    DataTable dtLNS = new DataTable();
    if (props.IndexOf(props["dtLNS"]) > 0)
        dtLNS = (DataTable)props["dtLNS"].GetValue(Model);
    else
        dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(false, LoaiNS);
    int SoCot =1;
    if (props.IndexOf(props["iSoCot"])>0)        
        SoCot= Convert.ToInt16(props["iSoCot"].GetValue(Model));
    else
    {
        if (dtLNS.Rows.Count < 10)
            SoCot = 1;
        else
            SoCot = 1;                
    }
    String[] arrLoaiNS = sLNS.Split(',');
    
 %>
   <table  class="mGrid">
       
            <%
                String strsTen = "", LNS = "", strChecked = "";
                for (int i = 0; i < dtLNS.Rows.Count; i = i + SoCot)
                {
                    
                    
                %>
            <tr>
                <%for (int c = 0; c < SoCot; c++)
                  {
                      if (i + c < dtLNS.Rows.Count)
                      {
                          strChecked = "";
                          strsTen = Convert.ToString(dtLNS.Rows[i + c]["TenHT"]);
                          LNS = Convert.ToString(dtLNS.Rows[i + c]["sLNS"]);
                          for (int j = 0; j < arrLoaiNS.Length; j++)
                          {
                              if (LNS.Equals(arrLoaiNS[j]))
                              {
                                  strChecked = "checked=\"checked\"";
                                  break;
                              }
                          }
                      %>
                 <td align="center" style="width: 40px;">
                    <input type="checkbox" value="<%=LNS %>" <%=strChecked %> check-group="DonVi" id="sLNS" name="sLNS" />
                </td>
                <td align="left">
                    <%=strsTen%>
                </td>
                <%} %>
                <%} %>
            </tr>
            <%}%>
 </table>
 <script type="text/javascript">
     function CheckAll(value) {
         $("input:checkbox[check-group='DonVi']").each(function (i) {
             this.checked = value;
         });
     }                                            
 </script>