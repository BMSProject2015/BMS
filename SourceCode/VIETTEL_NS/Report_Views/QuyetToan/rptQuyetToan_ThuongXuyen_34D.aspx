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
</head>
<body>
    <%        
        
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
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
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        DataTable dtKhoGiay = ReportModels.LoaiKhoGiay();
        SelectOptionList slKhoGiay = new SelectOptionList(dtKhoGiay, "MaKhoGiay", "TenKhoGiay");
        if(string.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "2";
        }
        dtKhoGiay.Dispose();
        String RutGon = Convert.ToString(ViewData["RutGon"]);
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if(String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
        String ToDaXem = "";
        ToDaXem = Convert.ToString(Request.QueryString["ToDaXem"]);
        ToDaXem += ToSo + ",";
        if (ToDaXem == ",") ToDaXem = "";
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });
        String urlReport = "";
      
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_34D", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, RutGon = RutGon, KhoGiay = KhoGiay,ToSo=ToSo,MaND=MaND });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_34D", new { ParentID = ParentID,ToDaXem=ToDaXem }))
        {
        %>
        <div class="box_tong">
             <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo quyết toán thường xuyên theo nhóm đơn vị</span>
                    </td>
                </tr>
             </table>
                </div>
                <div id="Div1">
                <div id="Di2">              
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td width="2%">&nbsp;</td>
                    <td class="td_form2_td1" style="width: 8%;">
                                <div>
                                Trạng Thái : 
                                </div>
                            </td>
                    <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>                      
                            </div></td>
                    <td class="td_form2_td1" style="width: 8%;">
                                <div>
                                Chọn Tháng: 
                                </div>
                            </td>
                    <td width="10%">
                    <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "Thang_Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>                      
                            </div></td>
                    <td width="8%" class="td_form2_td1"><div>Mẫu rút gọn: </div></td>
                    <td width="4%"><div>
                                <%=MyHtmlHelper.CheckBox(ParentID, RutGon, "RutGon", "", "onchange=\"ChoniThang()\"")%>
                                </div></td>
<td width="7%" class="td_form2_td1"><div>Khổ giấy in : </div></td>
                    <td width="10%"><div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slKhoGiay, KhoGiay, "KhoGiay", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChoniThang()\"")%>                      
                                </div></td>
                    <td width="7%" class="td_form2_td1"><div>Chọn tờ: </div></td>
                    <td width="10%"><div id="<%=ParentID %>_divDonVi">
                    <% rptQuyetToan_ThuongXuyen_34DController rpt = new rptQuyetToan_ThuongXuyen_34DController(); %>
                                <%=rpt.obj_To(ParentID,iID_MaTrangThaiDuyet,Thang_Quy,RutGon,MaND,KhoGiay,ToSo)%>
                                </div></td>
                                 <td width="8%" class="td_form2_td1"><div>
                               Tờ đã xem:</div></td>
                    <td width="8%">
                    <%=ToDaXem %>
                    <td></td>
                 </tr>
                <tr>
                     <td>
                        </td>
                         
                        <td colspan="10"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
         function Huy() {
             window.location.href = '<%=URL %>';
         }
 </script>
  <script type="text/javascript">
      function ChoniThang() {
          var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
          var Thang_Quy = document.getElementById("<%=ParentID %>_Thang_Quy").value;
          var KhoGiay = document.getElementById("<%=ParentID %>_KhoGiay").value;
          var bRutGon = document.getElementById("<%=ParentID %>_RutGon").checked;
          var RutGon = "";
          if (bRutGon) RutGon = "on";
          jQuery.ajaxSetup({ cache: false });
          var url = unescape('<%= Url.Action("DS_To?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&RutGon=#3&MaND=#4&KhoGiay=#5&ToSo=#6", "rptQuyetToan_ThuongXuyen_34D") %>');
          url = unescape(url.replace("#0", "<%= ParentID %>"));
          url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
          url = unescape(url.replace("#2", Thang_Quy));
          url = unescape(url.replace("#3", RutGon));
          url = unescape(url.replace("#4", "<%= MaND %>"));
          url = unescape(url.replace("#5", KhoGiay));
          url = unescape(url.replace("#6", "<%= ToSo %>"));
          $.getJSON(url, function (data) {
              document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
          });
      }                                            
      </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_34D", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, RutGon = RutGon, KhoGiay = KhoGiay, ToSo = ToSo, MaND = MaND }), "Xuất ra Excel")%>
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
