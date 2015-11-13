<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<body>
    <%
        String ParentID = "DuToan";
         int SoCot = 1;
         //dt Loại ngân sách
         String MaND = User.Identity.Name;
         String iNamLamViec = ReportModels.LayNamLamViec(MaND);
         String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
         DataTable dtDonVi = DuToan_ReportModels.dtDonVi(MaND,"1020500");

         if (String.IsNullOrEmpty(iID_MaDonVi))
         {
             if (dtDonVi.Rows.Count > 0)
             {
                 iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
             }
             else
             {
                 iID_MaDonVi = Guid.Empty.ToString();
             }
         }
         dtDonVi.Dispose();
         String[] arrMaDonVi = iID_MaDonVi.Split(',');
         String[] arrView = new String[arrMaDonVi.Length];
         String Chuoi = "";
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         if (String.IsNullOrEmpty(PageLoad))
             PageLoad = "0";
         if (String.IsNullOrEmpty(iID_MaDonVi)) PageLoad = "0";
         if (PageLoad == "1")
         {

             for (int i = 0; i < arrMaDonVi.Length; i++)
             {
                 arrView[i] =
                     String.Format(
                         @"/rptDuToan_1020500_TungDonVi/viewpdf?iID_MaDonVi={0}&MaND={1}",
                         arrMaDonVi[i], MaND);
                 Chuoi += arrView[i];
                 if (i < arrMaDonVi.Length - 1)
                     Chuoi += "+";
             }

         }
         String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "1" });
         using (Html.BeginForm("EditSubmit", "rptDuToan_1020500_TungDonVi", new {ParentID=ParentID}))
        {
    %>
     <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo dự toán chi ngân sách quốc phòng (phần Hỗ trợ các đoàn KTQP & Đơn vị sự nghiệp công lập) năm <%=iNamLamViec %>
                           </span>
                    </td>
                   
                </tr>
            </table>
        </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%" class="td_form2_td1"></td>
               <td class="td_form2_td1" style="width: 20%" >
                            <div> Chọn đơn vị:</div> 
                        </td>
          <td style="width: 40%">
                             <div  style="overflow: scroll; height: 400px">
                            
                            <table class="mGrid" style="width: 100%">
                                <tr>
                                    <th align="center" style="width: 40px;">
                                        <input type="checkbox" id="abc" onclick="CheckAllDV(this.checked)" />
                                    </th>
                                    <%for (int c = 0; c < SoCot * 2 - 1; c++)
                                      {%>
                                    <th>
                                    </th>
                                    <%} %>
                                </tr>
                                <%
                
String strsTen = "", MaDonVi = "", strChecked = "";
for (int i = 0; i < dtDonVi.Rows.Count; i = i + SoCot)
{
                    
                    
                                %>
                                <tr>
                                    <%for (int c = 0; c < SoCot; c++)
                                      {
                                          if (i + c < dtDonVi.Rows.Count)
                                          {
                                              strChecked = "";
                                              strsTen = Convert.ToString(dtDonVi.Rows[i + c]["TenHT"]);
                                              MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["iID_MaDonVi"]);
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
                                        <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="DonVi"
                                             id="iID_MaDonVi" name="iID_MaDonVi" />
                                    </td>
                                    <td align="left">
                                        <%=strsTen%>
                                    </td>
                                    <%} %>
                                    <%} %>
                                </tr>
                                <%}%>
                            </table>
                            </div>
                        </td>
            <td class="td_form2_td1"></td>
        </tr>
                           
        <tr>
            <td colspan="4">
                <div style="margin-top: 10px;">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 40%">
                            </td>
                            <td align="right">
                                <input type="submit" class="button" value="Tiếp tục" />
                            </td>
                            <td style="width: 1%">
                                &nbsp;
                            </td>
                            <td align="left">
                                <input type="button" class="button" value="Hủy" onclick="Huy()" />
                            </td>
                            <td style="width: 40%">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
          <script type="text/javascript">
                var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang=Chuoi.split("+");
                   var pageLoad = <%=PageLoad %>;
                   if(pageLoad=="1") {
                var siteArray = new Array(count);
                for (var i = 0; i < count; i++) {
                    siteArray[i] = Mang[i];
                }
                    for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
              function CheckAllDV(value) {
                  $("input:checkbox[check-group='DonVi']").each(function (i) {
                      this.checked = value;
                  });
              } function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
          </script>
    </table>
    <%} %>
</body>
</html>
