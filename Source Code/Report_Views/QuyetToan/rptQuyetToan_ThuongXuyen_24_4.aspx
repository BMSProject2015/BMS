<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%              
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);  
        //String Thang_Quy =Convert.ToString(Request.QueryString["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        ////String NamLamViec = Request.QueryString["NamLamViec"];
        //if (String.IsNullOrEmpty(NamLamViec))
        //{
        //    NamLamViec = DateTime.Now.Year.ToString();
        //}
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();         

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
            LoaiIn = "ChiTiet";
        DataTable dtDonVi = rptQuyetToan_ThuongXuyen_24_4Controller.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, "0", MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        //String iID_MaDonVi = Convert.ToString(Request.QueryString["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            PageLoad = "0";
        }
        dtDonVi.Dispose();
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
        if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if (ChiSo == arrMaDonVi.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrMaDonVi.Length - 1)
            {
                MaDonVi = arrMaDonVi[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
        dtDonVi.Dispose();
        if (LoaiIn == "TongHop")
        {
            ChiSo = 0;
            MaDonVi = iID_MaDonVi;
        }
        if (LoaiIn == "DonVi")
        {
            ChiSo = 0;
            MaDonVi = iID_MaDonVi;
        }
       
        DataTable dtTrangThai = rptQuyetToan_ThuongXuyen_24_4Controller.tbTrangThai();
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
       
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String urlReport = "";
      
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_24_4", new { Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,LoaiIn=LoaiIn });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_24_4", new { ParentID = ParentID,ChiSo=ChiSo }))
        {       
    %>
     <div class="box_tong">
            <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                     <td width="47.9%">
                        <span>Báo cáo so sánh chỉ tiêu quyết toán</span>
                      </td>
                         <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                        </td>
                </tr>
            </table>
            </div>
            <div id="rptMain">
                <div id="Di2">              
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                        <tr>
                            <td width="5%">&nbsp;</td>
                             <td class="td_form2_td1" width="13%"><div>
                                <%=NgonNgu.LayXau("Tháng:")%></div></td>
                            <td width="20%">                          
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:100%;\" onChange=ChonDV()")%>                             
                            </td>
                              <td width="5%" rowspan="4">Đơn vị:</td>
                            <td width="50%" rowspan="4" ><div id="<%= ParentID %>_tdDonVi" style="height:220px; overflow:scroll;"></div></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Trạng thái:")%></div>
                            </td>
                            <td>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width:100%;\" onChange=ChonDV()")%>                             
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                        <td></td>
                        <td class="td_form2_td1"><div>
                                <%=NgonNgu.LayXau("Tổng hợp dữ liệu theo:")%></div>
                        
                        
                        </td>
                              <td class="style2" >
                              <table border="0" cellspacing="0" cellpadding="0" width="100%">
                              	<tr>
                              		<td><%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Từng đơn vị chọn&nbsp;&nbsp;</td>
                              	</tr>
                                <tr>
                                    <td><%=MyHtmlHelper.Option(ParentID, "DonVi", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tất cả chi tiết&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                      <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tổng hợp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                </tr>
                              </table>
    </td>                      <td></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            
                            <td colspan="2"> <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                                <tr>
                                                    <td width="49%" align="right"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                                    <td width="2%">&nbsp;</td>
                                                    <td width="49%"> <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                                </tr>
                                             </table></td>
                          
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
    </div>    
   <script type="text/javascript">
       function Huy() {
           window.location.href = '<%=URL %>';
       }
 </script>
          <script type="text/javascript">

              $(function () {
                  $('div.login1 a').click(function () {
                      $('div#rptMain').slideToggle('normal');
                      $(this).toggleClass('active');
                      return false;
                  });
              });
              function CheckAll(value) {
                  $("input:checkbox[check-group='DonVi']").each(function (i) {
                      this.checked = value;
                  });
              }
              ChonDV();
              function ChonDV() {
                  //var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
                  var Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
                  var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                  jQuery.ajaxSetup({ cache: false });
                  var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&Thang_Quy=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4", "rptQuyetToan_ThuongXuyen_24_4") %>');
                  url = unescape(url.replace("#0", "<%= ParentID %>"));
                  url = unescape(url.replace("#1", "<%= MaND %>"));
                  url = unescape(url.replace("#2", Thang_Quy));
                  url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
                  url = unescape(url.replace("#4",  iID_MaTrangThaiDuyet));
                 
                  $.getJSON(url, function (data) {
                      document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                  });
              }                                            
          </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_24_4", new { MaND = MaND, Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra Excel")%> 
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
