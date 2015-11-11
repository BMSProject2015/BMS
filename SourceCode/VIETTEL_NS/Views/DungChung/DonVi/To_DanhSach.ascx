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
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String sMaDonVi = Convert.ToString(props["sMaDonVi"].GetValue(Model));
    DataTable dtDonVi = new DataTable();
    if (props.IndexOf(props["dtDonVi"])>0)
        dtDonVi = (DataTable)props["dtDonVi"].GetValue(Model);
    else
        dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
    int SoCot =1;
    if (props.IndexOf(props["iSoCot"])>0)        
        SoCot= Convert.ToInt16(props["iSoCot"].GetValue(Model));
    else
    {
        if (dtDonVi.Rows.Count < 10)
            SoCot = 1;
        else
            SoCot = 1;                
    }
    
    String[] arrMaDonVi=sMaDonVi.Split(',');
    
    
 %>
   <table  class="mGrid" style="width: 100%">
        <tr>
            <th align="center" style="width: 40px;"> <input type="checkbox"  id="abc" onclick="CheckAllTO(this.checked)" /></th>
            <%for (int c = 0; c < SoCot*2-1; c++)
              {%>
            <th></th>
            <%} %>
        </tr>
            <%
                
                String strsTen = "",MaDonVi="",strChecked="";
                for (int i = 0; i < dtDonVi.Rows.Count; i=i+SoCot)
                {
                    
                    
                %>
            <tr>
                <%for (int c = 0; c < SoCot; c++)
                  {
                      if (i + c < dtDonVi.Rows.Count)
                      {
                          strChecked = "";
                          strsTen = Convert.ToString(dtDonVi.Rows[i + c]["TenTo"]);
                          MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["MaTo"]);
                          for (int j = 0; j < arrMaDonVi.Length; j++)
                          {
                              if (MaDonVi.Equals(arrMaDonVi[j]))
                              {
                                  strChecked = "checked=\"checked\"";
                                  break;
                              }
                          }
                      %>
               <td align="center" style="width: 40px;">
                    <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="To" id="MaTo" name="MaTo" />
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
     function CheckAllTO(value) {
         $("input:checkbox[check-group='To']").each(function (i) {
             this.checked = value;
         });
     }                                            
 </script>