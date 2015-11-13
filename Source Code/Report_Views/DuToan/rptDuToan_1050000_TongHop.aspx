<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<%@ Import Namespace="FlexCel.Core" %>
<%@ Import Namespace="FlexCel.Render" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
   
    String ParentID = "BaoCaoNganSachNam";
    int SoCot = 1;
    String MaND = User.Identity.Name;
    String iNamLamViec = ReportModels.LayNamLamViec(MaND);

   
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
   
    String ToSo = Request.QueryString["ToSo"];
        String Nganh = Request.QueryString["Nganh"];
        if (String.IsNullOrEmpty(Nganh))
        {
            Nganh = "-1";
        }
    if (String.IsNullOrEmpty(ToSo))
    {
        ToSo = "1";
    }

    DataTable dtTo = rptDuToan_1050000_TongHopController.DanhSachToIn(MaND, Nganh, ToSo);
    String MaTo = Convert.ToString(ViewData["MaTo"]);
    String[] arrMaDonVi = MaTo.Split(',');
    String[] arrMaTo = MaTo.Split(',');
    String Chuoi = "";
    String[] arrView = new String[arrMaTo.Length];
    if (String.IsNullOrEmpty(PageLoad))
        PageLoad = "0";
    if (String.IsNullOrEmpty(MaTo)) PageLoad = "0";
    if (PageLoad == "1")
    {

        for (int i = 0; i < arrMaTo.Length; i++)
        {
            arrView[i] =
                String.Format(
                    @"/rptDuToan_1050000_TongHop/viewpdf?ToSo={0}&MaND={1}",
                    arrMaTo[i], MaND);
            Chuoi += arrView[i];
            if (i < arrMaTo.Length - 1)
                Chuoi += "+";
        }

    }  
       
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});
    using (Html.BeginForm("EditSubmit", "rptDuToan_1050000_TongHop", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách quốc phòng năm <%=iNamLamViec%> (Phần chi cho doanh nghiệp)</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
               <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 20%" class="td_form2_td1"></td>
               <td class="td_form2_td1" style="width: 20%" >
                            <div> Chọn tờ:</div> 
                        </td>
          <td style="width: 30%">
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
for (int i = 0; i < dtTo.Rows.Count; i = i + SoCot)
{
                    
                    
                                %>
                                <tr>
                                    <%for (int c = 0; c < SoCot; c++)
                                      {
                                          if (i + c < dtTo.Rows.Count)
                                          {
                                              strChecked = "";
                                              strsTen = Convert.ToString(dtTo.Rows[i + c]["TenTo"]);
                                              MaDonVi = Convert.ToString(dtTo.Rows[i + c]["MaTo"]);
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
                                        <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="MaTo"
                                             id="MaTo" name="MaTo" />
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
                  $("input:checkbox[check-group='MaTo']").each(function (i) {
                      this.checked = value;
                  });
              }
               function Huy() {
              window.location.href = '<%=BackURL%>';
          }
          </script>
    </table>
            </div>
        </div>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
    </script>
    </div>
    <%}
        
         %>
    <div>    
       <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_1050000_TongHop", new { MaND = MaND, Nganh = Nganh, ToSo = ToSo }), "Xuất ra Excels")%>
    </div>
</body>
</html>
