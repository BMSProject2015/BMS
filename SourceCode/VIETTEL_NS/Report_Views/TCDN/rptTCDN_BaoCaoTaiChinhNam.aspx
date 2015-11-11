<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
   <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoTaiChinhNam";
    String NamLamViec = Convert.ToString(ViewData["NamLamViec"]);
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
    String To = Convert.ToString(ViewData["To"]);
    if (String.IsNullOrEmpty(To))
    {
        To = "";
    }
    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMin; i < NamMax; i++)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    DataTable dtTo = new DataTable();
    dtTo.Columns.Add("MaTo", typeof(String));
    dtTo.Columns.Add("TenTo", typeof(String));
    DataRow dR;
    for (int i = 1; i <= 4; i++)
    {
        dR = dtTo.NewRow();
        dtTo.Rows.Add(dR);
        dR[0] = Convert.ToString(i);
        dR[1] = Convert.ToString(i);
    }
    dtTo.Rows.InsertAt(dtTo.NewRow(), 0);
    dtTo.Rows[0]["TenTo"] = "-- Tất cả --";
    SelectOptionList slTo = new SelectOptionList(dtTo, "MaTo", "TenTo");
    String BackURL = Url.Action("Index", "TCDN_Report", new { sLoai="0"});
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptTCDN_BaoCaoTaiChinhNam", new { NamLamViec = NamLamViec,To = To});
    using (Html.BeginForm("EditSubmit", "rptTCDN_BaoCaoTaiChinhNam", new { ParentID = ParentID, To = To }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo tài chính năm của các công ty cổ phần</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="text-align:center;">
                            <%--<table cellpadding="0" cellspacing="0" border="0" style="margin-left:500px;">
                                <tr>
                                    <td align="right" width="50%"><b>Chọn năm làm việc :</b></td>
                                    <td align="left" width="50%"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
                                </tr>
                                <tr>
                                    <td align="right" width="50%"><b>Chọn tờ :</b></td>
                                    <td align="left" width="50%"><%=MyHtmlHelper.DropDownList(ParentID, slTo, To, "iTo", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
                                </tr>
                            </table>--%>
                            <div><b>Chọn năm làm việc :</b>   <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>
                                
                            </div>
                             <div><b>Chọn tờ :</b>   <%=MyHtmlHelper.DropDownList(ParentID, slTo, To, "iTo", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%>
                                
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="margin:0 auto;">
                            <table cellpadding="0" cellspacing="0" border="0" style="margin-left:500px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
    <%}
        dtNam.Dispose();    
         %>
 
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
    </script> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptTCDN_BaoCaoTaiChinhNam", new { NamLamViec = NamLamViec }), "Export to Excel")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
