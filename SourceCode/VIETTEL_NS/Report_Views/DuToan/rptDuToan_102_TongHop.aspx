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
         String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        
         DataTable dtDonVi = DuToan_ReportModels.dtPhongBan(MaND, "102");

         SelectOptionList slPhongBan = new SelectOptionList(dtDonVi, "iID_MaPhongBan", "TenHT");
         if (String.IsNullOrEmpty(iID_MaPhongBan))
         {
             if (dtDonVi.Rows.Count > 0)
             {
                 iID_MaPhongBan = Convert.ToString(dtDonVi.Rows[0]["iID_MaPhongBan"]);
             }
             else
             {
                 iID_MaPhongBan = "-1";
             }
         }
         dtDonVi.Dispose();
         String[] arrView = new String[1];
         String Chuoi = "";
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         if (String.IsNullOrEmpty(PageLoad))
             PageLoad = "0";
         if (String.IsNullOrEmpty(iID_MaPhongBan)) PageLoad = "0";
         if (PageLoad == "1")
         {
                 arrView[0] =
                     String.Format(
                         @"/rptDuToan_102_TongHop/viewpdf?iID_MaPhongBan={0}&MaND={1}",
                         iID_MaPhongBan, MaND);
                 Chuoi += arrView[0];
         }
         String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
         using (Html.BeginForm("EditSubmit", "rptDuToan_102_TongHop", new {ParentID=ParentID}))
        {
    %>
     <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo dự toán chi ngân sách sử dụng (phần nghiệp vụ -Ngành 00) năm <%=iNamLamViec %>
                           </span>
                    </td>
                   
                </tr>
            </table>
        </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%" class="td_form2_td1"></td>
               <td class="td_form2_td1" style="width: 30%" >
                            <div> Chọn phòng ban:</div> 
                        </td>
          <td style="width: 20%">
                             <div>
                             <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>
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
              }
               function Huy() {
              window.location.href = '<%=BackURL%>';
          }
          </script>
    </table>
    <%} %>
</body>
</html>
