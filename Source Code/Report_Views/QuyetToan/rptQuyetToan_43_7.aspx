<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
    <style type="text/css">
        .style1
        {
            width: 6%;
        }
    </style>
</head>
<body>
    <%        
        
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_43_7Controller.tbTrangThai();
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
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(iID_MaPhongBan);
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
        String[] arrLNS = sLNS.Split(',');

        dtLNS.Dispose();
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        String Quy = Convert.ToString(ViewData["ThangQuy"]);
        if (String.IsNullOrEmpty(Quy))
        {
            Quy = "1";
        }
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptQuyetToan_43_7Controller.LayDSDonVi(iID_MaTrangThaiDuyet, sLNS, Quy, TruongTien, MaND);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
      
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_43_7", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, ThangQuy = Quy, iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_43_7", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                     <td width="47.9%">
                        <span>Báo cáo tổng hợp quyết toán năm-quý</span>
                    </td>
                         <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                </tr>
            </table>
        </div>
        <div id="rptMain">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="style1">&nbsp;</td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                <%=NgonNgu.LayXau("Tất cả : ")%></div>
                        </td>
                        <td style="width: 25%;">
                            <div>
                                <input type="checkbox" id="checkAll" onclick="ChonLNS(this.checked)" />
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                               Trạng Thái :                             
                            </div>
                        </td>
                        <td style="width: 15%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                        <td width="20%">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">&nbsp;</td>
                        <td rowspan="21" class="td_form2_td1"> <div>Loại ngân sách:</div> </td>
                        <td rowspan="21">
                            <div style="width: 99%; height: 500px; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                    <%
                                    String TenLNS = ""; String sLNS1 = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtLNS.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                                        sLNS1 = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                                        for (int j = 0; j < arrLNS.Length; j++)
                                        {
                                            if (sLNS1 == arrLNS[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 15%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="ChonQuy()"
                                                name="sLNS" />
                                        </td>
                                        <td>
                                            <%=TenLNS%>
                                        </td>
                                    </tr>
                                  <%}%>
                                </table>
                            </div>
                        </td>
                        <td class="td_form2_td1">
                            <div>
                               Chọn Quý: </div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                      <td></td>
                    </tr>
                    <tr>
                    <td class="style1"></td>                 
                         <td class="td_form2_td1">
                            <div>
                                <%=NgonNgu.LayXau("Chọn Đơn Vị:")%>
                            </div>
                        </td>
                        <td> <div id="<%=ParentID %>_divDonVi">
                                <%rptQuyetToan_43_7Controller rptTB1 = new rptQuyetToan_43_7Controller();%>
                                <%=rptTB1.obj_DSDonVi(ParentID,iID_MaTrangThaiDuyet,sLNS,Quy,TruongTien,MaND,iID_MaDonVi)%>
                            </div> 
                        </td>
                         <td></td>
                    </tr>
                    <tr>
                  
                     <td class="style1">&nbsp;</td>
                            <td class="td_form2_td1">
                        <div>
                               Trường Tiền:
                            </div>
                        </td>
                        <td>
                            <div>
                               <div>
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "","onchange=\"ChonQuy()\"")%>Tự Chi&nbsp;&nbsp;&nbsp;&nbsp
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=\"ChonQuy()\"")%>Hiện Vật&nbsp;&nbsp;&nbsp;&nbsp
                            </div>
                            </div>
                        </td>
                         <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                      
                        <td>&nbsp;</td>
                    </tr>                    
                    <tr>
                    <td class="style1">&nbsp;</td>
                         <td colspan="3">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 45%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="10%">
                                    </td>
                                    <td style="width: 45%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                       <td>&nbsp;</td> 
                    </tr>
               </table>            
         </div>
</div>
</div>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_43_7", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, ThangQuy = Quy, iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien }), "Xuất ra Excels")%>
  <script type="text/javascript">
      $(function () {
          $("div#rptMain").hide();
          $('div.login1 a').click(function () {
              $('div#rptMain').slideToggle('normal');
              $(this).toggleClass('active');
              return false;
          });
      });       
    </script>
 <script type="text/javascript">
     function Huy() {
         window.location.href = '<%=URL %>';
     }
 </script>
<script type="text/javascript">
    function ChonLNS(sLNS) {
        $("input:checkbox[check-group='sLNS']").each(function (i) {
            if (sLNS) {
                this.checked = true;
            }
            else {
                this.checked = false;
            }
        });
        ChonQuy(); 

    }                                            
    </script>
    <script type="text/javascript">
        function ChonQuy() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var Quy = document.getElementById("<%=ParentID %>_Quy").value;
            var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
            var TruongTien = "";
            if (bTruongTien) TruongTien = "rTuChi";
            else TruongTien = "rHienVat";
            var sLNS = "";
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (this.checked) {
                    if (sLNS != "") sLNS += ",";
                    sLNS += this.value;
                }
            });    
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&sLNS=#2&Thang_Quy=#3&TruongTien=#4&MaND=#5&iID_MaDonVi=#6", "rptQuyetToan_43_7") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", sLNS));
            url = unescape(url.replace("#3", Quy));
            url = unescape(url.replace("#4", TruongTien));
            url = unescape(url.replace("#5", "<%= MaND %>"));
            url = unescape(url.replace("#6", "<%= iID_MaDonVi %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }                                            
        </script>
        <%} %>
      <div>
        <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
        <%--<iframe src="<%=Url.Action("ViewPDF", "rptKTTH_ChiTietPhaiThu", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, ThangQuy = Quy, iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien })%>" height="600px" width="100%"></iframe>--%>
    </div>
</body>
</html>
