<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NguoiCoCong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
         width: 13px;
     }
     .style2
     {
         width: 15px;
     }
     .style3
     {
         width: 185px;
     }
     .style4
     {
         width: 26%;
     }
    
    
     .style5
     {
         width: 13%;
     }
    
    
 </style>
</head>
<body>
     <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopNguoiCoCong";
       
        String MaND = User.Identity.Name;
        String RutGon = Convert.ToString(ViewData["RutGon"]);
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }

        String ThangQuy = Convert.ToString(ViewData["ThangQuy"]);
        if (String.IsNullOrEmpty(ThangQuy))
        {
            ThangQuy = "1";
        }
        String sLNS=Convert.ToString(ViewData["sLNS"]);
        if(String.IsNullOrEmpty(sLNS))
        {
            sLNS = Guid.Empty.ToString();
        }
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        String Solieu = Convert.ToString(ViewData["Solieu"]);
        if (String.IsNullOrEmpty(Solieu))
        {
            Solieu = "2";
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
        dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        //dt Quý
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        // dt Tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptNCC_TongHopNganSach_57Controller.tbTrangThai();
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
        DataTable dtLNS = ReportModels.NS_LoaiNganSachNguoiCoCong();
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["TenHT"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        dtLNS.Dispose();
        String UrlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptNCC_TongHopTCKK_62", new { MaND = MaND, sLNS = sLNS, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay, RutGon = RutGon, Solieu = Solieu, ToSo = ToSo });
        String BackURL = Url.Action("Index", "NguoiCoCong_Report", new { iLoai=1});
        String urlExport = Url.Action("ExportToExcel", "rptNCC_TongHopTCKK_62", new { MaND = MaND, sLNS = sLNS, ThangQuy = ThangQuy, LoaiThang_Quy = LoaiThang_Quy, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay, RutGon = RutGon, Solieu = Solieu, ToSo = ToSo });
        using (Html.BeginForm("EditSubmit", "rptNCC_TongHopTCKK_62", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Tổng hợp Người có công</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
         <div id="Div1" style="background-color:#F0F9FE;margin:0 0 0 0;">
            <div id="rptMain">

<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 103px">
  <tr>
    <td width="105" align="right"><b>Loại ngân sách :</b> </td>
    <td class="style3"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 100%;\"  onchange=ChonTo()")%></td>
    <td class="style2">&nbsp;</td>
    <td width="96" align="right"><b>Tháng / Quý : </b></td>
    <td class="style4"><%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", " class=\"input1_2\" style=\"width:10%;\" onchange=ChonTo() ")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangQuy, "iThang", "", "class=\"input1_2\" style=\"width:20%;\" onchange=ChonTo() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "class=\"input1_2\" style=\"width:10%;\" onchange=ChonTo() ")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, ThangQuy, "iQuy", "", "class=\"input1_2\" style=\"width:20%;\" onchange=ChonTo() ")%><br /></td>
    <td rowspan="2" align="left" class="style5"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:140px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In số liệu ") %></b></legend>
                            <%=MyHtmlHelper.Option(ParentID, "1", Solieu, "Solieu", "")%>&nbsp;&nbsp;Toàn bộ danh mục<br />
                            <%=MyHtmlHelper.Option(ParentID, "2", Solieu, "Solieu", "")%>&nbsp;&nbsp;Theo số liệu có
                           </fieldset> </td>
    <td width="15%" rowspan="2" align="right"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                           <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "", "onchange=ChonTo()")%>&nbsp;&nbsp;A4 - Giấy nhỏ<br />
                            <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "onchange=ChonTo()")%>&nbsp;&nbsp;A3 - Giấy to
                           </fieldset></td>
    <td width="2%" rowspan="2" align="right">&nbsp;</td>
  </tr>
  <tr>
    <td align="right"><b>Mẫu rút gọn : </b></td>
    <td class="style3"><%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onchange=ChonTo()")%></td>
    <td class="style2">&nbsp;</td>
    <td align="right"><b>Trạng thái :  </b></td>
    <td class="style4"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 84%;heigh:23px;\" onchange=ChonTo()")%>
        
    
    </td>
  </tr>
  <tr>
  <td align="right"></td>
    <td class="style3"></td>
    <td class="style2">&nbsp;</td>
    <td align="right"><b>Chọn tờ: </b></td>
    <td class="style4">
       <div id="<%=ParentID %>_divDonVi">
                                 <% rptNCC_TongHopTCKK_62Controller rpt = new rptNCC_TongHopTCKK_62Controller(); %>
                                <%=rpt.obj_To(ParentID,iID_MaTrangThaiDuyet,ThangQuy,LoaiThang_Quy, RutGon,MaND,KhoGiay,ToSo,sLNS)%>
    </div>
    </td>
  </tr>
  <tr>
    <td colspan="7" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
    <td align="center">&nbsp;</td>
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
        <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
  <script type="text/javascript">
      ChonTo();
      function ChonTo() {
          var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
          var bLoaiThang_QuyCheck = document.getElementById("<%=ParentID %>_LoaiThang_Quy").checked;
          var Thang_Quy = "", bLoaiThang_Quy = "";
          if (bLoaiThang_QuyCheck == false) {
              Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
              bLoaiThang_Quy = "1";
          }
          else {
              Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
              bLoaiThang_Quy = "0";
          }
          var KhoGiayCheck = document.getElementById("<%=ParentID %>_KhoGiay").checked;
          var KhoGiay = "";
          if (KhoGiayCheck == false) KhoGiay = "1";
          else KhoGiay = "2";
          var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
          var RutGon = "";
          if (bRutGon) RutGon = "on";
          var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
          jQuery.ajaxSetup({ cache: false });
          var url = unescape('<%= Url.Action("DS_To?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&RutGon=#4&MaND=#5&KhoGiay=#6&ToSo=#7&sLNS=#8", "rptNCC_TongHopTCKK_62") %>');
          url = unescape(url.replace("#0", "<%= ParentID %>"));
          url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
          url = unescape(url.replace("#2", Thang_Quy));
          url = unescape(url.replace("#3", bLoaiThang_Quy));
          url = unescape(url.replace("#4", RutGon));
          url = unescape(url.replace("#5", "<%= MaND %>"));
          url = unescape(url.replace("#6", KhoGiay));
          url = unescape(url.replace("#7", "<%= ToSo %>"));
          url = unescape(url.replace("#8", sLNS));
          $.getJSON(url, function (data) {
              document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
          });
      }                                            
    </script>