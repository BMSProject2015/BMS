<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NguoiCoCong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
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
    
     .style1
     {
         width: 193px;
     }
        .style2
     {
         width: 210px;
     }
     .style3
     {
         width: 100px;
     }
    
     .style4
     {
         width: 210px;
         height: 19px;
     }
     .style5
     {
         height: 19px;
         width: 302px;
     }
     .style6
     {
         width: 287px;
     }
    
     .style7
     {
         width: 302px;
     }
    
 </style>
</head>
<body>
    <% 
        String UserName="";
        UserName = User.Identity.Name;
        String ParentID = "QuyetToanNganSach";
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptNCC_TCKK_58_4Controller.tbTrangThai();
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "TenTrangThai");
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            if (dtTrangThai.Rows.Count > 0)
            {
                iID_MaTrangThaiDuyet = Convert.ToString(dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"]);
            }
            else
            {
                iID_MaTrangThaiDuyet = Guid.Empty.ToString();
            }
        }
        dtTrangThai.Dispose();

        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        
        DataTable dtLNS = ReportModels.NS_LoaiNganSachNguoiCoCong();
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
       
        dtLNS.Dispose();

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptNCC_TCKK_63_8Controller.dtDanhsach_DonVi(sLNS, iID_MaTrangThaiDuyet,UserName);
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
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        String Loai = Convert.ToString(ViewData["Loai"]);
        if (String.IsNullOrEmpty(Loai))
        {
            Loai = Convert.ToString(ViewData["Loai"]);
        }
        String[] arrLoai = { "1", "2" };
        if (String.IsNullOrEmpty(Loai))
            Loai = arrLoai[0];
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptNCC_TCKK_63_8", new { sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, Loai = Loai, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, UserName = UserName ,KhoGiay=KhoGiay});
        String BackURL = Url.Action("Index", "NguoiCoCong_Report", new { iLoai=2});
        String urlExport = Url.Action("ExportToExcel", "rptNCC_TCKK_63_8", new { iID_MaDonVi = iID_MaDonVi, sLNS = sLNS, Loai = Loai, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, UserName = UserName, KhoGiay = KhoGiay });
        using (Html.BeginForm("EditSubmit", "rptNCC_TCKK_63_8", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng quyết toán</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
            <div id="rptMain">               
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 105px">
  <tr>
    <td rowspan="3" align="center" valign="top" class="style3">&nbsp;</td>
    <td rowspan="3" align="center" valign="top" class="style1"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:280px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Chọn Loại ngân sách ") %></b></legend>
                             <div><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 100%\"size='4' tabindex='-1' onchange=ChonLNS()")%></div>
                           </fieldset></td>
    <td align="right" class="style2"><b>Trạng thái : </b></td>
    <td class="style7"> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"onchange=ChonLNS()")%></td>
    <td rowspan="2" valign="top" align="center" class="style6"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In cho loại") %></b></legend>
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "1", Loai, "Loai", "")%> &nbsp;&nbsp;Từng đơn vị<br />
                           &nbsp;&nbsp;  <%=MyHtmlHelper.Option(ParentID, "2", Loai, "Loai", "")%> &nbsp;&nbsp;Tất cả đơn vị
                           </fieldset></td>
    <td width="170" rowspan="2" valign="top" align="center"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "")%> &nbsp;&nbsp;A4 - Giấy nhỏ<br />
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "")%> &nbsp;&nbsp;A3 - Giấy to
                           </fieldset></td>
    <td width="170" rowspan="2" valign="top" align="center">&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style4"><b>Đơn vị: </b></td>
    <td class="style5"><div id="<%= ParentID %>_tdDonVi"><%rptNCC_TCKK_63_8Controller rptTB1 = new rptNCC_TCKK_63_8Controller();                          
                            %>
                            <%=rptTB1.obj_DonVi(ParentID,sLNS, iID_MaDonVi,iID_MaTrangThaiDuyet,UserName)%>   </div></td>
  </tr>
  <tr>
    <td colspan="4"> <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-top: 10px;margin-left:150px;">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td>
    <td>&nbsp;</td>
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
          function ChonLNS() {
              var sLNS = document.getElementById("<%=ParentID %>_sLNS").value
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&sLNS=#1&iID_MaDonVi=#2&iID_MaTrangThaiDuyet=#3&UserName=#4", "rptNCC_TCKK_63_8") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", sLNS));
              url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
              url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#4", "<%= UserName%>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
              });
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
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
