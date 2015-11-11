<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CongSan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form2
        {
            height: 89px;
        }
        </style>
      <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 12px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
    
     .style7
     {
         height: 37px;
     }
     .style8
     {
         height: 29px;
     }
    
 </style>
</head>
<body>
  <%
      String ParentID = "KeToanCongSan";
      
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
      dtNam.Rows[0]["TenNam"] = "-- Năm chứng từ kế toán --";
      SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        
      String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
      DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
      SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
      if (string.IsNullOrEmpty(KhoGiay))
      {
          KhoGiay = "1";
      }
      dtKhoGiay.Dispose();
      String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
      if (String.IsNullOrEmpty(iNamLamViec)) iNamLamViec = DateTime.Now.Year.ToString();
      String PageLoad = Convert.ToString(ViewData["PageLoad"]);
      String URL = "";
      if (PageLoad == "1")
      {
          URL = Url.Action("ViewPDF", "rptCongSan_PhuLuc3", new { iNamLamViec = iNamLamViec, KhoGiay = KhoGiay });
      }
      
       String BackURL = Url.Action("Index", "CongSan_Report");
       String urlExport = Url.Action("ExportToExcel", "rptCongSan_PhuLuc3", new { iNamLamViec = iNamLamViec, KhoGiay = KhoGiay });
       using (Html.BeginForm("EditSubmit", "rptCongSan_PhuLuc3", new { ParentID = ParentID }))
    {
  %>
<div class="box_tong" >
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>BÁO CÁO TỔNG HỢP  TÀI SẢN CỐ ĐỊNH</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">

<div id="rptMain" style="background-color:#F0F9FE;margin:0 auto;">
<div style="margin:0 auto;">                       
 <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                <tr><td colspan="2" style="height:5px;"></td></tr>
                    <tr>
                        <td class="td_form2_td1" style="text-align:right; width:30%;">
                            <div><%=NgonNgu.LayXau("Chọn năm:")%></div>
                        </td>
                        <td class="td_form2_td1" style="text-align:left; width:20%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 50%\"")%>
                            </div>
                        </td>
                        <%-- <td class="td_form2_td1" style="text-align:right; width:10%;">
                            <div><%=NgonNgu.LayXau("Khổ giấy:")%></div>
                        </td>
                        <td class="td_form2_td1" style="text-align:left; width:20%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>--%>
                        <td style=" width:20%;"></td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" colspan="5" style="text-align:center";>                       
                            <table cellpadding="0" cellspacing="0" border="0" style="width:250px; margin: 3px auto;">
                                <tr>
                                    <td style="text-align:right;"><input style="display:inline-block" type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="10px"></td>
                                    <td style="text-align:left;"><input style="display:inline-block; margin-left:5px;" class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                 </table>
           
   </div>
</div>
<script type="text/javascript">
    function Huy() {
        window.location.href = '<%=BackURL%>';
    }
    </script>
      <script type="text/javascript">
          $(function () {
              $("div#rptMain").show();
              $('div.login1 a').click(function () {
                  $('div#rptMain').slideToggle('fast');
                  $(this).toggleClass('active');
                  return false;
              });
          });
    </script>
 </div>
 <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
</div>
<%  } %>
<br />
   <iframe src="<%=URL%>" height="600px" width="100%"></iframe>
</body>
</html>
