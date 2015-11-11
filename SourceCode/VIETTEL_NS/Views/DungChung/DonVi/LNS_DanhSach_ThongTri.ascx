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
    {
        String iID_MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
       dtDonVi = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
    }
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
            <th align="center" style="width: 40px;"> <input type="checkbox"  id="abc" onclick="CheckAllLNS(this.checked)" /></th>
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
                          strsTen = Convert.ToString(dtDonVi.Rows[i + c]["TenHT"]);
                          MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["sLNS"]);
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
                    <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="LNS" id="sLNS" name="sLNS" onclick="ChonLNS()"/>
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
         $("input:checkbox[check-group='LNS']").each(function (i) {
             this.checked = value;
         });
     }                                            
 </script>