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
    
     .style4
     {
         width: 8%;
     }
     .style5
     {
         width: 27%;
     }
    
     .style6
     {
         width: 25%;
     }
    
 </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "NguoiCoCong";
        String NamLamViec = Request.QueryString["iNamLamViec"];
        String MaND = User.Identity.Name;
        //Trang Thai Duyet
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptNCC_ThongTriTongHop_2Controller.tbTrangThai();
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
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = "0";
        }
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DateTime.Now.Year.ToString();
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
        // dt Tháng
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            if (dtThang.Rows.Count > 0)
            {
                
            }
        }
        dtThang.Dispose();
        //Dt Quý
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        //dt Loại Ngân Sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        DataTable dtLNS = ReportModels.NS_LoaiNganSachNguoiCoCong(false);
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
        // dt Đơn vị
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptNCC_ThongTriTongHop_2Controller.DSDonVi(MaND,LoaiThangQuy,Thang_Quy,sLNS,iID_MaTrangThaiDuyet);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
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

        String Loai = Convert.ToString(ViewData["Loai"]);
        if (String.IsNullOrEmpty(Loai))
        {
            Loai = Convert.ToString(ViewData["Loai"]);
        }
        String[] arrLoai = { "1", "2"};
        if (String.IsNullOrEmpty(Loai))
            Loai = arrLoai[0];


        String Kieu = Convert.ToString(ViewData["Kieu"]);
        if (String.IsNullOrEmpty(Kieu))
        {
            Kieu = Convert.ToString(ViewData["Kieu"]);
        }
        String[] arrKieu = { "1", "2" };
        if (String.IsNullOrEmpty(Kieu))
            Kieu = arrKieu[0];
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptNCC_ThongTriTongHop_2", new { MaND = MaND, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Loai = Loai, Kieu = Kieu });
        String BackURL = Url.Action("Index", "NguoiCoCong_Report", new { iLoai=1});
        String urlExport = Url.Action("ExportToExcel", "rptNCC_ThongTriTongHop_2", new { MaND = MaND, LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Loai = Loai, Kieu = Kieu });
        using (Html.BeginForm("EditSubmit", "rptNCC_ThongTriTongHop_2", new { ParentID = ParentID}))
        {
    %>
   
<div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Thông Tri Tổng Hợp quyết toán</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="rptMain"> 
<div style="background-color:#F0F9FE;margin:0 0 0 0;padding-top:8px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="10%" align="right"><b>Tháng / quý : </b></td>
    <td class="style5"><%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "onchange=Chon()", "class=\"input1_2\" style=\"width:25%;\"onchange=Chon() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:25%;\"onchange=Chon()")%><br /></td>
    <td align="right" class="style4"><b>Ngân sách: </b></td>
    <td class="style6"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 90%;\"onchange=Chon()")%></td>
    <td width="30%" rowspan="2"><fieldset style="height:70px;padding-left:5px;width:240px;text-align:left;padding-left:3px;-moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
            <legend><b>Tổng hợp theo</b></legend>
             <table width="250" border="0" cellpadding="0" cellspacing="0" 
                style="height: 41px">
  <tr>
    <td><%=MyHtmlHelper.Option(ParentID, "1", Loai, "Loai", "")%> &nbsp;&nbsp;<b>Từng đơn vị</b> </td>
    <td><%=MyHtmlHelper.Option(ParentID, "1", Kieu, "Kieu", "")%> &nbsp;&nbsp;<b>Chi Tiết </b> </td>
  </tr>
  <tr>
    <td><%=MyHtmlHelper.Option(ParentID, "2", Loai, "Loai", "")%> &nbsp;&nbsp;<b>Tổng Hợp đơn vị</b> </td>
    <td><%=MyHtmlHelper.Option(ParentID, "2", Kieu, "Kieu", "")%> &nbsp;&nbsp;<b>Tổng hợp </b></td>
  </tr>
</table>

         
    </fieldset></td>
  </tr>
  <tr>
    <td align="right" width="10%"><b>Trạng thái : </b></td>
    <td class="style5"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 83%;heigh:23px;\"onchange=Chon()")%> </td>
    <td align="right" class="style4"><b>Đơn vị : </b></td>
    <td id="<%= ParentID %>_tdDonVi" class="style6"><%rptNCC_ThongTriTongHop_2Controller rptTB1 = new rptNCC_ThongTriTongHop_2Controller();%>                                    
                                <%=rptTB1.obj_DonViTheoNam(ParentID, MaND, LoaiThangQuy, Thang_Quy, sLNS, iID_MaDonVi, iID_MaTrangThaiDuyet)%></td>
  </tr>
  <tr>
    <td colspan="5" align="center"><table cellpadding="0" cellspacing="0" border="0" style="margin: 10px;text-align:center;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Thực hiện" />
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
 <script type="text/javascript">
     function Chonall(sLNS) {
         $("input:checkbox[check-group='MaDonVi']").each(function (i) {
             if (sLNS) {
                 this.checked = true;
             }
             else {
                 this.checked = false;
             }
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
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
        <script type="text/javascript">
            function Chon() {
                var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThangQuy").checked;
                var LoaiThangQuy
                var Thang
                if (LoaiThang_QuyCheck == true) {
                    Thang = document.getElementById("<%= ParentID %>_iThang").value;
                    LoaiThangQuy = 0;
                }
                else {
                    Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                    LoaiThangQuy = 1;
                }

                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
              
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&LoaiThangQuy=#2&Thang_Quy=#3&sLNS=#4&iID_MaDonVi=#5&iID_MaTrangThaiDuyet=#6", "rptNCC_ThongTriTongHop_2") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%= MaND %>"));
                url = unescape(url.replace("#2", LoaiThangQuy));
                url = unescape(url.replace("#3", Thang));
                url = unescape(url.replace("#4", sLNS));
                url = unescape(url.replace("#5", "<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#6", iID_MaTrangThaiDuyet));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }                                            
     </script>
     <div>
     </div>
     </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    <%}%>
    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
