<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
    String ParentID = "ThuNop";
    String MaND = User.Identity.Name;
    String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);

    String iLoaiBaoCao = Convert.ToString(ViewData["iLoaiBaoCao"]);
    
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptThuNop_Thu02", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan, iLoaiBaoCao = iLoaiBaoCao });
    String iNamLamViec = ReportModels.LayNamLamViec(MaND);
    DataTable dtPhongBan = ThuNopModels.getDSPhongBan(iNamLamViec, MaND);
    SelectOptionList slPhongBan= new SelectOptionList(dtPhongBan,"iID_MaPhongBan","sTenPhongBan");
    dtPhongBan.Dispose();

    DataTable dtLoaiBaoCao = ThuNopModels.getDSTO();
    SelectOptionList slLoaiBaoCao = new SelectOptionList(dtLoaiBaoCao, "MaLoai", "sTen");
    dtLoaiBaoCao.Dispose();

    String BackURL = Url.Action("Index", "Home");
    using (Html.BeginForm("EditSubmit", "rptThuNop_Thu02", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo các khoản thu nộp ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>   
        <div id="Div1">
            <div id="Div2" class="table_form2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                 <tr>
                 <td class="td_form2_td1" style="width: 40%" >
                            <div> Chọn phòng ban:</div> 
                        </td>
                        <td  style="width: 10%" >
                                    <div>
                                         <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width: 100%\"")%> 
                                    </div>
                               </td>
                                  <td class="td_form2_td1"  style="width: 10%" >
                            <div> Chọn Tờ:</div> 
                        </td>
                        <td style="width: 10%" >
                                    <div>
                                         <%=MyHtmlHelper.DropDownList(ParentID, slLoaiBaoCao, iLoaiBaoCao, "iLoaiBaoCao", "", "class=\"input1_2\" style=\"width: 100%\"")%> 
                                    </div>
                               </td>
                               <td></td>
                               </tr>
                                <tr>
                   <td  colspan="5"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td style="width: 2%;" align="left">
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
                            </tr>
                 </table>
            </div>
        </div>     
    </div>
    <%} %>
    <div ></div>   
          <script type="text/javascript">
              function Huy() {
                  window.location.href = '<%=BackURL%>';
              }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNop_Thu02", new { MaND = MaND, iID_MaPhongBan = iID_MaPhongBan, iLoaiBaoCao = iLoaiBaoCao }), "Xuất ra excel")%>
   <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
