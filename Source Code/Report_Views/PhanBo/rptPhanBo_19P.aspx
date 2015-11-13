<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
     <link href="../../Content/style.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
<%
   
    String ParentID = "PhanBo_19P";
    String MaND = User.Identity.Name;
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    //truong tien
    String TruongTien = Convert.ToString(ViewData["TruongTien"]);
    if(String.IsNullOrEmpty(TruongTien))
    {
        TruongTien = "rTuChi";
    }
    //don vi
    DataTable dtDonVi = NguoiDung_DonViModels.getDS(MaND);
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        PageLoad = "0";
    }
    String[] arrDonVi = iID_MaDonVi.Split(',');
    dtDonVi.Dispose();
    //LNS
    String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
    String sLNS = Convert.ToString(ViewData["sLNS"]);
    if (String.IsNullOrEmpty(sLNS))
    {
        sLNS = "0";
    }
    String[] arrLNS = sLNS.Split(',');
    dtLNS.Dispose();
    //kho giay
    DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
    SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
    String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
    if (String.IsNullOrEmpty(KhoGiay))
    {
        KhoGiay = "1";
    }
    dtKhoGiay.Dispose();
    //Trang Thai 
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }
    DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(3);
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptPhanBo_19P", new { MaND = MaND, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay });
    using (Html.BeginForm("EditSubmit", "rptPhanBo_19P", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
                       <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td width="47.9%">
                        <span>Tổng hợp năm - LNS</span>
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
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td width="5%"></td>
                         <td style="width: 30%;" rowspan="21">
                         <fieldset><b>Chọn LNS:</b>
                            <div style="width: 99%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="checkAll" onclick="ChonallLNS(this.checked)"></td>
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
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox1" onchange="Chon()"
                                                name="sLNS" />                                        </td>
                                        <td>
                                            <%=TenLNS%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div>                    </fieldset>        </td>
                             <td width="5%"></td>
                         <td style="width: 30%;" rowspan="21">
                            <fieldset><b>Chọn đơn vị cần tổng hợp:</b>

                            <div style="width: 99%; height: 500px; overflow: scroll; border:1px solid black;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="Checkbox2" onclick="ChonallDonVi(this.checked)"></td>
                                <td> Chọn tất cả đơn vị </td>
                                </tr>                                                                                 
                                    <%
                                    String TenDonVi = ""; String MaDonVi = "";
                                    String _Checked1 = "checked=\"checked\"";
                                    for (int i = 0; i < dtDonVi.Rows.Count; i++)
                                    {
                                        _Checked1 = "";
                                        TenDonVi = Convert.ToString(dtDonVi.Rows[i]["TenHT"]);
                                        MaDonVi = Convert.ToString(dtDonVi.Rows[i]["iID_MaDonVi"]);
                                        for (int j = 0; j < arrDonVi.Length; j++)
                                        {
                                            if (MaDonVi == arrDonVi[j])
                                            {
                                                _Checked1 = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=MaDonVi %>" <%=_Checked1%> check-group="iID_MaDonVi" id="iID_MaDonVi"
                                                name="iID_MaDonVi" />                                        </td>
                                        <td>
                                            <%=TenDonVi%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div>        
                            </fieldset>                    </td>
                        <td class="td_form2_td1" width="10%">
                            <div> Trạng thái:</div> 
                        </td>
                        <td  width="15%">
                                    <div>
                                         <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%> 
                                    </div>
                               </td>
                      
                         <td></td>
                    </tr>
                    <tr>
                    <td></td>
                    <td></td>
                    <td class="td_form2_td1">
                   <div> Khổ giấy:</div> </td>
                    <td>
                        <div>
                                         <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\"")%> 
                                    </div>
                    </td>
                    <td></td>
                    
                    </tr>
                     <tr>
                    <td></td>
                    <td></td>
                    <td class="td_form2_td1">
                   <div> Loại in(dangHT):</div> </td>
                    <td>
                        <div>
                                         <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, "", "", "", "class=\"input1_2\" style=\"width: 100%\"")%> 
                                    </div>
                    </td>
                    <td></td>
                    
                    </tr>
                       <tr>
                    <td></td>
                    <td></td>
                    <td class="td_form2_td1">
                   <div> Trường Tiền:</div> </td>
                    <td>
                              <div>
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "")%>Tự Chi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "")%>Hiện Vật&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                             </div>
                    </td>
                    <td></td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
                       <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="td_form2_td1">
                        &nbsp;</td>
                    <td>
                              &nbsp;</td>
                    <td>&nbsp;</td>
                    
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                <td style="width: 33%;"></td>
                               <td style="width: 10%;" align="center" onclick="OnInit_CT_NEW(700, 'Tiêu đề');">
                                <%= Ajax.ActionLink("Tiêu đề", "Index", "NhapNhanh", new { id = "PhanBo_19P", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" }, new AjaxOptions { }, new { @class = "button" })%>
                                  </td>
                                    <td style="width: 10%;" align="center">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td style="width: 10%;" align="center">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                    <td></td>
                                </tr>
                            </table></td>
                      <td></td>
                    </tr>
                 </table>
            </div>
        </div>
    
    <%} %> 
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
         function ChonallLNS(sLNS) {
             $("input:checkbox[check-group='sLNS']").each(function (i) {
                 if (sLNS) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }

             });
         }
         function ChonallDonVi(iID_MaDonVi) {
             $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
                 if (iID_MaDonVi) {
                     this.checked = true;
                 }
                 else {
                     this.checked = false;
                 }

             });
             }
             function OnInit_CT_NEW(value, title) {                
                 $("#idDialog").dialog("destroy");
                 document.getElementById("idDialog").title = title;
                 document.getElementById("idDialog").innerHTML = "";
                 $("#idDialog").dialog({
                     resizeable: false,
                     draggable: true,
                     width: value,
                     modal: true
                 });
             }
             function OnLoad_CT(v) {
                 document.getElementById("idDialog").innerHTML = v;
             }
             function CallSuccess_CT() {
                 
             }                                                   
      </script>  
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptPhanBo_19P", new { MaND = MaND, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, KhoGiay = KhoGiay }), "Xuất ra Excel")%>
   <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
   <div id="idDialog" style="display: none;">
    </div>
</body>
</html>
