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
            width: 7%;
        }
    </style>
</head>
<body>
    <%        
      
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_NghiepVu_40_2Controller.tbTrangThai();
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
        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
    
        DataTable dtQuy = DanhMucModels.DT_Thang();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaThang", "TenThang");
        dtQuy.Dispose();


        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(iID_MaPhongBan);
        String sLNS=Convert.ToString(ViewData["sLNS"]);      
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
        String RutGon = Convert.ToString(ViewData["RutGon"]);
        // Khổ giấy
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if (String.IsNullOrEmpty(KhoGiay))
            KhoGiay = "2";
        dtKhoGiay.Dispose();
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = "1" });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
        String ToDaXem = "";
        ToDaXem = Convert.ToString(Request.QueryString["ToDaXem"]);
        ToDaXem += ToSo + ",";
        if (ToDaXem == ",") ToDaXem = "";
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_nghiepvu_46B", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, Thang_Quy = Thang_Quy, TruongTien = TruongTien, RutGon = RutGon, KhoGiay = KhoGiay, MaND = MaND, ToSo = ToSo });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_nghiepvu_46B", new { ParentID = ParentID, ToDaXem = ToDaXem }))
        {
        %>
        <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp quyết toán tháng theo nhóm đơn vị</span>
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
                <div id="Div2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="style1">&nbsp;</td>
                           <td width="5%" class="td_form2_td1"><div>LNS:</div></td>    
                          <td style="width: 40%;" rowspan="23">
                        <div style="width: 99%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="Checkbox1" onclick="ChonLNS(this.checked)"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                           
                               
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
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="ChoniThang()"
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
                         <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                 Trạng Thái :
                            </div>
                        </td>
                        <td style="width:25%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 50%\" onchange=\"ChoniThang()\"")%>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                   
                    <tr>
                        <td class="style1">&nbsp;</td>
                        <td></td>
                         <td class="td_form2_td1">
                            <div>
                               Tháng làm việc: </div>
                        </td>
                        <td>
                            <div>                            
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "Thang_Quy", "", "class=\"input1_2\" style=\"width:50%;\" onchange=\"ChoniThang()\"")%> 
                            </div>
                        </td>
                         <td>&nbsp;</td>
                    </tr>
                    <tr>
                     <td class="style1">&nbsp;</td>
                     <td></td>
                         <td class="td_form2_td1">
                            <div>
                                Rút gọn:                                </div>  
                        </td>
                        <td>
                        <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onchange=\"ChoniThang()\"")%> 
                                </div> 
                        </td>
                         <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">&nbsp;</td>
                        <td></td>
                         <td class="td_form2_td1">
                        <div>
                               Trường Tiền:
                           </div>
                        </td>
                        <td>
                            <div>
                              <div>
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=\"ChoniThang()\"")%>Tự Chi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=\"ChoniThang()\"")%>Hiện Vật&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr> 
                    
                    <tr>
                        <td class="style1">&nbsp;</td>
                        <td></td>
                         <td class="td_form2_td1">
                            <div>
                               Khổ giấy: </div>
                        </td>
                        <td>
                            <div>                            
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width:50%;\" onchange=\"ChoniThang()\"")%> 
                            </div>
                        </td>
                         <td>&nbsp;</td>
                    </tr>   
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td></td>
                 <td class="td_form2_td1"><div>Chọn tờ: </div></td>
                    <td>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                    	<tr>
                    		<td style="width: 50%;"> <div id="<%=ParentID %>_divDonVi">
                    <% rptQuyetToan_NghiepVu_46BController rpt = new rptQuyetToan_NghiepVu_46BController(); %>
                                <%=rpt.obj_To(ParentID,iID_MaTrangThaiDuyet,Thang_Quy,RutGon,MaND,KhoGiay,ToSo,sLNS,TruongTien)%>
                                </div></td>
                                 <td class="td_form2_td1" style="width: 25%;"><div>
                               Tờ đã xem:</div></td>
                              <td style="width: 25%;">
                               <%=ToDaXem %>
                                </td>
                    	</tr>
                    </table>
                    </td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                                 <td class="style1">&nbsp;</td>
                        <td>&nbsp;</td>
                 <td class="td_form2_td1">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                    </tr>                
                    <tr>
                    <td class="style1">&nbsp;</td>
                         <td colspan="2">
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
                ChoniThang();
            }                                            
    </script>
        <%} %>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
    </script>
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
        function ChoniThang() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var Thang_Quy = document.getElementById("<%=ParentID %>_Thang_Quy").value;
            var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
            var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
            var RutGon = "";
            if (bRutGon) RutGon = "on";
            var sLNS = "";
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (this.checked) {
                    if (sLNS != "") sLNS += ",";
                    sLNS += this.value;
                }
            });
            var bTruongTien = document.getElementById("<%=ParentID %>_TruongTien").checked;
            var TruongTien = "";
            if (bTruongTien) TruongTien = "rTuChi";
            else TruongTien = "rHienVat";
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("DS_To?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6&sLNS=#7&TruongTien=#8", "rptQuyetToan_NghiepVu_46B") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", Thang_Quy));
            url = unescape(url.replace("#3", RutGon));
            url = unescape(url.replace("#4", "<%= MaND %>"));
            url = unescape(url.replace("#5", KhoGiay));
            url = unescape(url.replace("#6", "<%= ToSo %>"));
            url = unescape(url.replace("#7", sLNS));
            url = unescape(url.replace("#8", TruongTien));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }                                            
    </script>
        <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_NghiepVu_46B", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, Thang_Quy = Thang_Quy, TruongTien = TruongTien, RutGon = RutGon, KhoGiay = KhoGiay, MaND = MaND, ToSo = ToSo }), "Xuất ra Excels")%>
        <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
    