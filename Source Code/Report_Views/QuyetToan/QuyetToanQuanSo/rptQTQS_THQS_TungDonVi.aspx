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
         DataTable dtDonVi = QuyetToan_ReportModels.dtDonVi_QS(MaND);

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
         
        
         String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
         if (String.IsNullOrEmpty(iID_MaPhongBan)) iID_MaPhongBan = "-2";
         String iThang = Convert.ToString(ViewData["iThang"]);
         if (String.IsNullOrEmpty(iThang))
             iThang = "-1";
         DataTable dtThang = DanhMucModels.DT_Quy(true);
         SelectOptionList slThang = new SelectOptionList(dtThang, "MaQuy", "TenQuy");
         dtThang.Dispose();
         DataTable dtPhongBan = QuyetToanModels.getDSPhongBan_QuanSo(iNamLamViec, MaND);
         DataRow dr = dtPhongBan.NewRow();
         dr["iID_MaPhongBan"] = "-2";
         dr["sTenPhongBan"] = "--Chọn phòng ban--";
         dtPhongBan.Rows.InsertAt(dr, 0);
         SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
         if (String.IsNullOrEmpty(iID_MaPhongBan))
         {
             if (dtDonVi.Rows.Count > 0)
             {
                 iID_MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["iID_MaPhongBan"]);
             }
             else
             {
                 iID_MaPhongBan = "-1";
             }
         }
         dtPhongBan.Dispose();
         String Chuoi = "";
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
         if (String.IsNullOrEmpty(PageLoad))
             PageLoad = "0";
         String[] arrMaDonVi = iID_MaDonVi.Split(',');
         if (iID_MaPhongBan != "-2")
         {
             arrMaDonVi = "-1".Split(',');
         }
       
         String[] arrView = new String[arrMaDonVi.Length];
         if (String.IsNullOrEmpty(iID_MaDonVi)) PageLoad = "0";
         if (PageLoad == "1")
         {

             for (int i = 0; i < arrMaDonVi.Length; i++)
             {
                 arrView[i] =
                     String.Format(
                         @"/rptQTQS_THQS_TungDonVi/viewpdf?iID_MaDonVi={0}&MaND={1}&iThang={2}&iID_MaPhongBan={3}",
                         arrMaDonVi[i], MaND,iThang,iID_MaPhongBan);
                 Chuoi += arrView[i];
                 if (i < arrMaDonVi.Length - 1)
                     Chuoi += "+";
             }

         }
         String URL = Url.Action("Index", "QuyetToan_QuanSo_Report");
         using (Html.BeginForm("EditSubmit", "rptQTQS_THQS_TungDonVi", new {ParentID=ParentID}))
        {
    %>
     <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Tổng hợp tình hình quân số năm <%=iNamLamViec %> 
                           </span>
                    </td>
                   
                </tr>
            </table>
        </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td class="td_form2_td1" style="width: 10%">
                <div>
                    Chọn phòng ban:</div>
            </td>
            <td class="td_form2_td1" style="width: 15%">
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>
                </div>
            </td>
               <td class="td_form2_td1" style="width: 10%" rowspan="2" >
                            <div> Chọn đơn vị:</div> 
                        </td>
          <td style="width: 40%" rowspan="2">
                             <div  style="overflow: scroll; height: 400px" >
                            
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
            <td class="td_form2_td1" style="width: 10%;">
                <div>
                    Chọn quý:</div>
            </td>
            <td class="td_form2_td1" style="width: 20%">
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>
                </div>
            </td>
            <td></td>
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
                                <input type="button" class="button" value="Hủy"  onclick="Huy()" />
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
               window.location.href = '<%=URL %>';
           }
          </script>
    </table>
    <%} %>
</body>
</html>
