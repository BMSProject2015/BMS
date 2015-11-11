<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<%@ Import Namespace="FlexCel.Core" %>
<%@ Import Namespace="FlexCel.Render" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
   
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]); ;
    DataTable dtTrangThai = rptDTCNSQP_ChiChoDoanhNghiepController.tbTrangThai();
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
    String ToSo = Request.QueryString["ToSo"];
    if (String.IsNullOrEmpty(ToSo))
    {
        ToSo = "1";
    }
    DataTable dtToSo = rptDuToan_1020500Controller.DanhSachToIn(MaND,iID_MaTrangThaiDuyet, ToSo);
        SelectOptionList slTo = new SelectOptionList(dtToSo, "MaTo", "TenTo"); 
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptDuToan_1020500", new { MaND = MaND,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet, ToSo = ToSo });
    using (Html.BeginForm("EditSubmit", "rptDuToan_1020500", new { ParentID = ParentID, MaND = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, ToSo = ToSo }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách quốc phòng 1020500</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td  style="width: 25%;"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng Thái : ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                             </div>   
                       </td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Tờ số: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div id="<%=ParentID %>_divDonVi">
                            <%rptDuToan_1020500Controller rptTB1 = new rptDuToan_1020500Controller();%>                                   
                                     <%=rptTB1.obj_DSDonVi(ParentID,MaND, iID_MaTrangThaiDuyet, ToSo)%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>                       
                        <td>
                        </td>                        
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
    </script>
    </div>
    <%}
         %>
    <div>    
      <script type="text/javascript">
          function Chon() {
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
              
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&ToSo=#3", " rptDuToan_1020500") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%= MaND %>"));
              url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));            
              url = unescape(url.replace("#3", "<%= ToSo %>"));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;

              });
          }                                            
      </script>
      
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
