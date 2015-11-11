<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <div>
     <%
        String ParentID = "PhanBo";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String MaND = User.Identity.Name;
        //Loai ngan sach
         String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
         DataTable dtLNS = rptQuyetToan_NghiepVu_38_1aController.NS_LoaiNganSachNghiepVu(false, iID_MaPhongBan);
        String sLNS = Convert.ToString(ViewData["isLNS"]);
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
        String Thang_Quy = Convert.ToString(ViewData["iQuy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
         DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        String TruongTien = Convert.ToString(ViewData["iTruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            PageLoad = "0";
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptQuyetToan_NghiepVu_38_1aController.LayDSDonVi(iID_MaTrangThaiDuyet, Thang_Quy, "1", sLNS, TruongTien, MaND);

        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Guid.Empty.ToString();
        }
        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
        {
            LoaiIn = "ChiTiet";
        }
         String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if(ChiSo==arrMaDonVi.Length)
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
                MaDonVi = arrMaDonVi[0];
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
            if (LoaiIn == "TongHop")
            {
                ChiSo = 0;
                MaDonVi = iID_MaDonVi;
            }
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptQuyetToan_NghiepVu_38_1a", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, iID_MaDonVi = MaDonVi, sLNS = sLNS, TruongTien = TruongTien,MaND=MaND,LoaiIn=LoaiIn });
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_NghiepVu_38_1a", new { ParentID = ParentID,ChiSo=ChiSo }))
        {%>
         <div class="box_tong">
          <div class="title_tong">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                         <td width="47.9%">
                            <span>Báo cáo thông báo ngân sách- thông báo lũy kê</span>
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
            <td style="width: 5%;"></td>
            <td style="width: 30%;" rowspan="10">
                            <div style="width: 99%; height: 400px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)"></td>
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
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="Chon()"
                                                name="sLNS" />                                        </td>
                                        <td>
                                            <%=TenLNS%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div>                            </td>
                         <td style="width: 30%;" rowspan="10" id="<%= ParentID %>_divDonVi">
                             <div >                              
                        <% rptQuyetToan_NghiepVu_38_1aController rpt = new rptQuyetToan_NghiepVu_38_1aController(); %>
                        <%=rpt.obj_DSDonVi(iID_MaTrangThaiDuyet, Thang_Quy,sLNS,TruongTien,MaND,iID_MaDonVi)%>
                        </div>
                            </td>
                            <td style="width: 10%;" class="td_form2_td1"> <div>
                                Trạng Thái:
                                </div></td>
                                <td style="width: 10%;">
                                <div>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\" onchange=Chon()")%></div></td>
                           
                            <td></td>
        </tr>
        <tr>
        <td>
        
        </td>
      
        <td style="width: 10%;" class="td_form2_td1"> <div>
                                Quý:
                                </div></td>
                     <td style="width: 10%;">
                                <div>
                                 <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "Thang_Quy", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\" onchange=Chon()")%></div></td>
      <td></td>
        </tr>
          <tr>
        <td>
        
        </td>
      
        <td style="width: 10%;" class="td_form2_td1"> <div>
                                Trường tiền:
                                </div></td>
                     <td style="width: 10%;">
                                <div>
                                <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "","onchange=Chon()")%>Tự Chi&nbsp;&nbsp;&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=Chon()")%>Hiện Vật&nbsp;&nbsp;&nbsp;</div></td>
      <td></td>
        </tr>
          <tr>
        <td>
        
        </td>
     <td align="right" class="style4">Tổng hợp dữ liệu theo: </td>
    <td class="style2" > &nbsp;
        <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Chi tiết&nbsp;&nbsp;
        <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tổng hợp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td>
      <td></td>
        </tr>
        <tr>
                        <td>
                        </td>
                         <td></td>
                        <td colspan="2"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
              window.location.href = '<%=BackURL%>';
          }
    </script>
     <script type="text/javascript">
         function Chon() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
             var iThang_Quy = document.getElementById("<%=ParentID %>_Thang_Quy").value;
         
             var sLNS = "";
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (this.checked) {
                     if (sLNS != "") sLNS += ",";
                     sLNS += this.value;
                 }
             });
           
             var TruongTien="rTuChi";
             var bTruongTien = document.getElementById("<%=ParentID %>_TruongTien").checked;
             if (bTruongTien == false) TruongTien = "rHienVat";
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("ds_DonVi?iID_MaTrangThaiDuyet=#0&Thang_Quy=#1&sLNS=#2&TruongTien=#3&MaND=#4&iID_MaDonVi=#5", "rptQuyetToan_NghiepVu_38_1a") %>');
             url = unescape(url.replace("#0", iID_MaTrangThaiDuyet));
             url = unescape(url.replace("#1", iThang_Quy));
             url = unescape(url.replace("#2", sLNS));
             url = unescape(url.replace("#3", TruongTien));
             url = unescape(url.replace("#4", "<%= MaND %>"));
             url = unescape(url.replace("#5", "<%= iID_MaDonVi %>"));
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
             });
         }                                            
     </script>
     

     <script type="text/javascript">
         function Chonall(sLNS) {
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (sLNS) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }

             });
             Chon();
         }
         function ChonallDV(sLNS) {
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
      
    <%}%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
