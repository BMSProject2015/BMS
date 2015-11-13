 <%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
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
            font: bold 10px "Museo 700";
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
    
     .style5
     {
         width: 27px;
     }
     .style6
     {
         width: 73px;
     }
     .style7
     {
         width: 24px;
     }
     .style8
     {
         width: 153px;
     }
     .style9
     {
         width: 235px;
     }
    
 </style>
</head>
<body>
    <%   
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String iID_MaDanhMuc = Convert.ToString(ViewData["iID_MaDanhMuc"]);
    String sLNS = Convert.ToString(ViewData["sLNS"]);
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    String Check = Convert.ToString(ViewData["Check"]);
    if (String.IsNullOrEmpty(Check))
    {
        Check = "off";
    }
    //trang thai 
    DataTable dtTrangThai = rptDT_ChiNganSachSD_II_02Controller.tbTrangThai();
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
 //Ma don vi
    DataTable dtNguonDonVi = rptDT_ChiNganSachSD_II_02Controller.DanhSachDonVi(MaND,iID_MaDanhMuc,sLNS, iID_MaTrangThaiDuyet);
    SelectOptionList slDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "TenDV");
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtNguonDonVi.Rows.Count > 0)
        {
            iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
        }
        else
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
    }
   // String[] arrDonVi = iID_MaDonVi.Split(',');
        //Nhom don vi
    DataTable dtNhomDV = rptDT_ChiNganSachSD_II_02Controller.DS_NhomDonVi(MaND,sLNS, iID_MaTrangThaiDuyet);

    if (String.IsNullOrEmpty(iID_MaDanhMuc))
    {
        if (dtNhomDV.Rows.Count > 0)
        {
            iID_MaDanhMuc = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDanhMuc"]);
        }
        else
        {
            iID_MaDanhMuc = Guid.Empty.ToString();
        }
    }

    SelectOptionList slNhomDonVi = new SelectOptionList(dtNhomDV, "iID_MaDanhMuc", "TenDM");
    dtNguonDonVi.Dispose();
    dtNhomDV.Dispose();
        //Loai ngan sach
    String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
    DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
    if (String.IsNullOrEmpty(sLNS))
    {
        if (dtLoaiNganSach.Rows.Count > 0)
        {
            sLNS = Convert.ToString(dtLoaiNganSach.Rows[0]["sLNS"]);
        }
        else
        {
            sLNS = Guid.Empty.ToString();
        }
    }
    String[] arrLNS = sLNS.Split(',');
    dtLoaiNganSach.Dispose();
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String UrlReport = "";
    if (PageLoad == "1")
        UrlReport = Url.Action("ViewPDF", "rptDT_ChiNganSachSD_II_02", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, iID_MaDanhMuc = iID_MaDanhMuc, sLNS = sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Check = Check });
        
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="1"});
    String urlExport = Url.Action("ExportToExcel", "rptDT_ChiNganSachSD_II_02", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, iID_MaDanhMuc = iID_MaDanhMuc, sLNS = sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Check = Check });
    using (Html.BeginForm("EditSubmit", "rptDT_ChiNganSachSD_II_02", new { ParentID = ParentID}))
        {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo DT chi ngân sách sử dụng năm</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                      <div class="login1" style=" width:50px; height:20px; text-align:left;">
                      <a style="cursor:pointer;"></a></div>
                           <%--<div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>--%>
                </td>
                </tr>
            </table>
        </div>
      <div id="Div1" style="background-color:#F0F9FE;">
   <div id="rptMain" style="margin:0 auto;background-color:#F0F9FE;">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td style="width: 50%;" >
                            <div style="width: 100%; height:300px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="LNS" onclick="ChonLNS()" onchange="Chon()"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                      
                                    <%
                                        String TenLNS = ""; String LNS = "";
                                        String _Checked = "checked=\"checked\"";
                                        for (int i = 0; i < dtLoaiNganSach.Rows.Count; i++)
                                        {
                                            _Checked = "";
                                            TenLNS = Convert.ToString(dtLoaiNganSach.Rows[i]["TenHT"]);
                                            LNS = Convert.ToString(dtLoaiNganSach.Rows[i]["sLNS"]);
                                            for (int j = 0; j < arrLNS.Length; j++)
                                            {
                                                if (LNS == arrLNS[j])
                                                {
                                                    _Checked = "checked=\"checked\"";
                                                    break;
                                                }
                                            }                                                                                 
                                    %>
                                <tr>
                                    <td style="width: 15%;">
                                        <input type="checkbox" value="<%=LNS %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="Chon()" name="sLNS" />
                                    </td>
                                    <td>
                                        <%=TenLNS%>
                                    </td>
                                </tr>
                                    <%}%>
                                </table>
                            </div>
                        </td>
                        <td style="width: 50%;">
                            <table style=" width:100%;">
                                <tr style=" height : 50px;">
                                <td class="td_form2_td1" style="width: 13%;">
                                    <div><%=NgonNgu.LayXau("Chọn trạng thái:")%></div>
                                </td>
                                <td style="width: 10%;">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 85%\"onchange=Chon()")%>                                
                                    </div>
                                </td>   
                                </tr>
                                
                                <tr style=" height : 50px;">
                                <td class="td_form2_td1" style="width: 13%;">
                                    <div><%=NgonNgu.LayXau("Chọn nhóm đơn vị")%></div>
                                </td>
                                <td style="width: 15%;" id="<%=ParentID %>_tdNhomDonVi">
                                <%rptDT_ChiNganSachSD_II_02Controller rpt = new rptDT_ChiNganSachSD_II_02Controller();
                                  rptDT_ChiNganSachSD_II_02Controller.LNSdata _Data = new rptDT_ChiNganSachSD_II_02Controller.LNSdata();
                                  _Data = rpt.obj_DonVi(ParentID, MaND, iID_MaDonVi, iID_MaDanhMuc, sLNS, iID_MaTrangThaiDuyet);
                                 %>
                                 <%=_Data.iID_MaDanhMuc %>
                                </td>
                                </tr>
                                <tr style=" height : 50px;">                
                                <td class="td_form2_td1" style="width: 13%;">
                                    <div><%=NgonNgu.LayXau("Chọn đơn vị")%></div>
                                </td>
                                <td style="width: 15%;"id="<%=ParentID %>_tdDonVi">
                                   <%=_Data.iID_MaDonVi %>
                                </td>
                                </tr>
                                <tr style=" height : 50px;"> <td class="td_form2_td1" style="width: 30%;"> 
                                <div class="div"><%=MyHtmlHelper.CheckBox(ParentID, Check, "Check", "")%> &nbsp;&nbsp;&nbsp; Có cột năm trước
                                </div></td>
                                </tr>
                        </table>
                       
                        </td>                       
                    </tr>
                          <tr>
                       <td>
                          <%=MyHtmlHelper.ActionLink(urlExport, "Xuất Excel") %>
                          </td>
                        <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
                 </table>
            </div>
        </div>
       
    </div>
    <%} %>
     <script type="text/javascript">
         function ChonLNS() {
             var sLNS = document.getElementById("LNS").checked;
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (sLNS) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }
             });
             //Chon();
         }                                            
    </script>
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
   <script type="text/javascript">
       function Chon() {
           var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
           var iID_MaDanhMuc = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;

           var sLNS = "";
           $("input:checkbox[check-group='sLNS']").each(function (i) {
               if (this.checked) {
                   if (sLNS != "") sLNS += ",";
                   sLNS += this.value;
               }
           });
           jQuery.ajaxSetup({ cache: false });
           var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaDanhMuc=#3&sLNS=#4&iID_MaTrangThaiDuyet=#5", "rptDT_ChiNganSachSD_II_02") %>');
           url = unescape(url.replace("#0", "<%= ParentID %>"));
           url = unescape(url.replace("#1", "<%=MaND %>"));
           url = unescape(url.replace("#2", "<%=iID_MaDonVi %>"));
           url = unescape(url.replace("#3", "<%=iID_MaDanhMuc %>"));
           url = unescape(url.replace("#4", sLNS));
           url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
           $.getJSON(url, function (data) {
               document.getElementById("<%= ParentID %>_tdNhomDonVi").innerHTML = data.iID_MaDanhMuc;
               document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
           });
       }                                            
     </script>
      <script type="text/javascript">
          function ChonDV() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              var iID_MaDanhMuc = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;

              var sLNS = "";
              $("input:checkbox[check-group='sLNS']").each(function (i) {
                  if (this.checked) {
                      if (sLNS != "") sLNS += ",";
                      sLNS += this.value;
                  }
              });
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaDanhMuc=#3&sLNS=#4&iID_MaTrangThaiDuyet=#5", "rptDT_ChiNganSachSD_II_02") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%=MaND %>"));
              url = unescape(url.replace("#2", "<%=iID_MaDonVi %>"));
              url = unescape(url.replace("#3", iID_MaDanhMuc));
              url = unescape(url.replace("#4", sLNS));
              url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
              });
          }                                            
     </script>
          <script type="text/javascript">
              function Huy() {
                  window.location.href = '<%=BackURL%>';
              }
    </script>
  <%

%>

    <iframe src="<%=UrlReport%>" height="600px" width="100%">
    </iframe>
</body>
</html>
