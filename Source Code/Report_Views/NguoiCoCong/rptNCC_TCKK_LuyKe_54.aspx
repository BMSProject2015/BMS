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
<head runat="server">
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
         width: 21px;
     }
     .style2
     {
         width: 10%;
     }
     .style3
     {
         width: 25%;
     }
     .style4
     {
         width: 30%;
     }
     .style5
     {
         width: 88px;
     }
    
 </style>
</head>
<body>
     <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "NguoiCoCong";
        String MaND = User.Identity.Name;
        String sLNS=Convert.ToString(ViewData["sLNS"]);
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }

        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
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
        //dt Tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        //dt Quý
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptNCC_BaoCaoQTTroCapKhoKhan_5Controller.tbTrangThai();
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
        String TongHop = Convert.ToString(ViewData["TongHop"]);
        //dt Loại Ngân Sách
        DataTable dtLNS = ReportModels.NS_LoaiNganSachNguoiCoCong();
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
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
        // dt đơn vị
        DataTable dtDonVi = rptNCC_TCCK_LuyKe_54Controller.dtDanhsach_DonVi(MaND, Thang_Quy, LoaiThang_Quy, sLNS,iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
        String iThang = "", iQuy = "";
        if (LoaiThang_Quy == "0")
        {
            iThang = Thang_Quy;
            iQuy = "-1";
        }
        else
        {
            iThang = "-1";
            iQuy = Thang_Quy;
        }   
             
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptNCC_TCCK_LuyKe_54", new { Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay ,TongHop=TongHop});
        String BackURL = Url.Action("Index", "NguoiCoCong_Report", new { iLoai = 1 });
        String urlExport = Url.Action("ExportToExcel", "rptNCC_TCCK_LuyKe_54", new {Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet ,KhoGiay=KhoGiay,TongHop=TongHop});
        using (Html.BeginForm("EditSubmit", "rptNCC_TCCK_LuyKe_54", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo người có công</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="background-color:#F0F9FE;">
            <div id="rptMain">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 110px">
  <tr>
    <td class="style1">&nbsp;</td>
    <td align="right" class="style2"><b>Loại ngân sách : </b></td>
    <td class="style3"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 100%;\" onchange=Chon()")%></td>
    <td width="140" align="right"><b>Tháng / quý : </b></td>
    <td class="style4">&nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "onchange=Chon()")%>&nbsp;&nbsp;Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang, "iThang", "", "class=\"input1_2\" style=\"width:27%;\" onchange=Chon()")%>
                                &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "onchange=Chon()")%>&nbsp;&nbsp;Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "class=\"input1_2\" style=\"width:27%;\" onchange=Chon()")%><br /></td>
    <td class="style5">&nbsp;</td>
    <td width="268" rowspan="2"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "")%> &nbsp;&nbsp;A4 - Giấy nhỏ<br />
                            &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "")%> &nbsp;&nbsp;A3 - Giấy to
                           </fieldset><br />
      <b>Chọn Toàn bộ đơn vị :<%=MyHtmlHelper.CheckBox(ParentID,TongHop,"TongHop","") %></b></td>
  </tr>
  <tr>
    <td class="style1">&nbsp;</td>
    <td align="right" class="style2"><b>Trạng thái : </b></td>
    <td class="style3"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=Chon()")%></td>
    <td align="right"><b>Đơn vị : </b></td>
    <td id="<%= ParentID %>_tdDonVi" class="style4"><%rptNCC_TCCK_LuyKe_54Controller rptTB1 = new rptNCC_TCCK_LuyKe_54Controller();                          
                              
                            %>
                            <%=rptTB1.obj_DonVi(ParentID, MaND, Thang_Quy, LoaiThang_Quy, sLNS, iID_MaDonVi,iID_MaTrangThaiDuyet)%>   </td>
    <td class="style5">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="7" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
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
        function Chon() {
            var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThang_Quy").checked;
            var sLNS = document.getElementById("<%=ParentID %>_sLNS").value
            var LoaiThang_Quy
            var Thang
            if (LoaiThang_QuyCheck == true) {
                Thang = document.getElementById("<%= ParentID %>_iThang").value;
                LoaiThang_Quy = 0;
            }
            else {
                Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                LoaiThang_Quy = 1;
            }
            
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&Thang_Quy=#1&LoaiThang_Quy=#2&sLNS=#3&iID_MaDonVi=#4&iID_MaTrangThaiDuyet=#5", "rptNCC_TCCK_LuyKe_54") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", Thang));
            url = unescape(url.replace("#2", LoaiThang_Quy));
            url = unescape(url.replace("#3", sLNS));
            url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
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
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
