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
            String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
            String Thang = "0";
            String Quy = "0";
            String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
            if (String.IsNullOrEmpty(Thang_Quy))
            {
                Thang_Quy = "1";
            }
            if (String.IsNullOrEmpty(LoaiThang_Quy))
            {
                LoaiThang_Quy = "0";
            }
            if (LoaiThang_Quy == "0")
            {
                Thang = Thang_Quy;
                Quy = "0";
            }
            else
            {
                Thang = "0";
                Quy = Thang_Quy;
            }

           
            String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
            DataTable dtTrangThai = rptQuyetToan_ThuongXuyen_34CController.tbTrangThai();
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

            DataTable dtQuy = DanhMucModels.DT_Quy();
            SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
            dtQuy.Dispose();

            DataTable dtThang = DanhMucModels.DT_Thang();
            SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
            dtThang.Dispose();

            DataTable dtNhomDonVi = rptQuyetToan_ThuongXuyen_23_2Controller.DanhSachNhomDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy,MaND);

            String iID_MaDanhMuc = Convert.ToString(ViewData["iID_MaDanhMuc"]);
            if (String.IsNullOrEmpty(iID_MaDanhMuc))
            {
                if(dtNhomDonVi.Rows.Count>2)
                {
                    iID_MaDanhMuc = Convert.ToString(dtNhomDonVi.Rows[2]["iID_MaDanhMuc"]);
                }
                else
                {
                    iID_MaDanhMuc = Guid.Empty.ToString();
                }
            }
            dtNhomDonVi.Dispose();

           
        
           
            String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
            if (String.IsNullOrEmpty(LoaiIn))
            {
                LoaiIn = "ChiTiet";
            }
            DataTable dtDonVi = rptQuyetToan_ThuongXuyen_23_2Controller.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, iID_MaDanhMuc, MaND);

            String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
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
          String  chkNgayNguoi = Convert.ToString(ViewData["chkNgayNguoi"]);
            String URL = Url.Action("Index", "QuyetToan_Report", new { Loai="0"});
            String urlReport = "";
            String PageLoad = Convert.ToString(ViewData["PageLoad"]);
            if (PageLoad.Equals("1"))
            {
                MaDonVi = iID_MaDonVi;
                urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_23_2", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = MaDonVi, iID_MaDanhMuc = iID_MaDanhMuc, LoaiIn = LoaiIn, chkNgayNguoi = chkNgayNguoi });
            }
            using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_23_2", new { ParentID = ParentID, ChiSo = ChiSo }))
        {                        
    %>
     <div class="box_tong">
            <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                      <td width="47.9%">
                        <span>Báo cáo quyết toán theo nhóm</span>
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
                            <td class="td_form2_td1" width="8%"><div>
                                <%=NgonNgu.LayXau("Trạng Thái :")%></div></td>
                            <td width="10%">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 50%\" onChange=ChonNDV()")%>
                                </div>
                            </td>
                            <td width="5%" rowspan="6">Đơn vị:</td>
                            <td width="50%" rowspan="6" ><div id="<%= ParentID %>_tdDonVi" style="height:220px; overflow:scroll;"></div></td>
                            <td></td>
                        </tr>
                        <tr>
                        <td></td>
                            <td class="td_form2_td1" width="10%"><div>
                                <%=NgonNgu.LayXau("Tháng Quý: ")%></div></td>
                            <td width="28%">
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%> Tháng:&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:24%;\" onChange=ChonNDV()")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%> Quý:&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:24%;\" onChange=ChonNDV()")%>                           
                            </td>
                                <td></td>
                            </tr>
                            <tr>
                            <td></td>
                            <td class="td_form2_td1" width="8%">Nhóm đơn vị:&nbsp;&nbsp; </td>
                            <td width="10%" id="<%= ParentID %>_tdNhomDonVi">                            
                         
                             </td>                      
                             <td></td>
                             </tr>
                             <tr>
                            <td></td>
                            <td class="td_form2_td1" width="8%">Tổng hợp dữ liệu theo:&nbsp;&nbsp; </td>
                              <td class="style2" > &nbsp;
        <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Chi tiết&nbsp;&nbsp;
        <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tổng hợp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td>                      
                             <td></td>
                             </tr>
                           <tr>
                           <td></td>
                           <td class="td_form2_td1"> Có ngày/người: </td>
                           <td> &nbsp;&nbsp;&nbsp;<%=MyHtmlHelper.CheckBox(ParentID, chkNgayNguoi, "chkNgayNguoi", "", "")%></td>
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
    <%} %>
     <script type="text/javascript">
         ChonNDV('<%=iID_MaDanhMuc %>');
         function CheckAll(value) {
             $("input:checkbox[check-group='DonVi']").each(function (i) {
                 this.checked = value;
             });
         }
        
         $(function () {
             $('div.login1 a').click(function () {
                 $('div#rptMain').slideToggle('normal');
                 $(this).toggleClass('active');
                 return false;
             });
         });    
          
         function ChonNDV(value) {
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
             var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
             var LoaiThang_Quy;
             var Thang_Quy;
             var i = 0;
             for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                 if (TenLoaiThang_Quy[i].checked) {
                     LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                 }
             }
             if (LoaiThang_Quy == 0) {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
             }
             else {
                 Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
             }
             var iID_MaDanhMuc;
             var objDM=document.getElementById("<%=ParentID %>_iID_MaDanhMuc");
             if (objDM != undefined) {                 
                 iID_MaDanhMuc = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;
             }
             else {
                 iID_MaDanhMuc = value;
             }
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&iID_MaDanhMuc=#5", "rptQuyetToan_ThuongXuyen_23_2") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#2", Thang_Quy));
             url = unescape(url.replace("#3", LoaiThang_Quy));
             url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
             url = unescape(url.replace("#5", iID_MaDanhMuc));
             $.getJSON(url, function (data) {           
                 document.getElementById("<%= ParentID %>_tdNhomDonVi").innerHTML = data.iID_MaDanhMuc;
                 document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
             });
         }                                            
  
              function ChonDV() {
                  var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                  var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
                  var LoaiThang_Quy;
                  var Thang_Quy;
                  var i = 0;
                  for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                      if (TenLoaiThang_Quy[i].checked) {
                          LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                      }
                  }
                  if (LoaiThang_Quy == 0) {
                      Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
                  }
                  else {
                      Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
                  }
                  var iID_MaDanhMuc = document.getElementById("<%=ParentID %>_iID_MaDanhMuc").value;
                  jQuery.ajaxSetup({ cache: false });
                  var url = unescape('<%= Url.Action("ds_NhomDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&iID_MaDanhMuc=#5", "rptQuyetToan_ThuongXuyen_23_2") %>');
                  url = unescape(url.replace("#0", "<%= ParentID %>"));
                  url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
                  url = unescape(url.replace("#2", Thang_Quy));
                  url = unescape(url.replace("#3", LoaiThang_Quy));
                  url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
                  url = unescape(url.replace("#5", iID_MaDanhMuc));
                  $.getJSON(url, function (data) {
                      document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.iID_MaDonVi;
                  });
              }                                            
   
         function Huy() {
             window.location.href = '<%=URL %>';
         }
     </script>
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_23_2", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, iID_MaDanhMuc = iID_MaDanhMuc, LoaiIn = LoaiIn, chkNgayNguoi = chkNgayNguoi }), "Xuất ra Excel")%> 
     <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
