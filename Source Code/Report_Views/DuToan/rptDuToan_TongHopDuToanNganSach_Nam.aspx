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
     <style type="text/css">
        .div-floatleft
        {
            float:left;    
            max-height:110px;            
        }
        .div-label
        {           
            font-size:13px;  
            padding-top:2px;
            min-width:100px;     
        }
        .div-txt
        {
            padding-top:5px;
            width:250px;            
        }    
        .p
        {
            height:20px;
            line-height:20px;
            padding:2px 4px 2px 2px;     
            text-align:right;                       
        }
    </style>       
</head>
<body>
    <%   
    String ParentID = "BaoCaoNganSachNam";
    String MaND = User.Identity.Name;
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToan_TongHopDuToanNganSach_NamController.tbTrangThai();
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
  
    DataTable dtNguonDonVi = rptDuToan_TongHopDuToanNganSach_NamController.getDonVi(MaND,iID_MaTrangThaiDuyet);
    SelectOptionList slDonvi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");
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
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="1"});
   

    using (Html.BeginForm("EditSubmit", "rptDuToan_TongHopDuToanNganSach_Nam", new { ParentID = ParentID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo tổng hợp dự toán ngân sách năm</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="table_form2" class="table_form2">
            <div id="" style="width:600px; margin:0px auto; min-height:10px;">                
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn trạng thái")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:160px;">
                    <p class="p"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNamLamViec()\"")%></p>                    
                </div>
                <div class="div-floatleft div-label" style="max-width:160px; text-align:left; padding-right:3px; padding-left:5px; margin-left:10px;">
                    <p class="p"><%=NgonNgu.LayXau("Chọn đơn vị")%></p>                      
                </div>
                <div class="div-floatleft div-txt" style="width:200px; margin:auto">
                    <div id="<%=ParentID %>_divDonVi">
                            <%rptDuToan_TongHopDuToanNganSach_NamController rptTB1 = new rptDuToan_TongHopDuToanNganSach_NamController();%>                                   
                                     <%=rptTB1.obj_DonViTheoNam(ParentID,MaND,iID_MaDonVi,iID_MaTrangThaiDuyet)%>
                            </div>        
                </div>
            </div>
            <div style="height:5px; clear:both;"></div>
            <div id="both" style="clear:both; height:30px; line-height:30px;">
                <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 5px auto; width:200px;">
                    <tr>
                        <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                        <td width="5px"></td>
                        <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                    </tr>
               </table>   
            </div>
        </div>
    </div>
    <%} %>
     <script type="text/javascript">
         function ChonNamLamViec() {
             var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&iID_MaTrangThaiDuyet=#3", "rptDuToan_TongHopDuToanNganSach_Nam") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", "<%= MaND %>"));
             url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
             url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));

             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
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
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_TongHopDuToanNganSach_Nam", new { MaND = MaND, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptDuToan_TongHopDuToanNganSach_Nam",new{MaND=MaND,iID_MaDonVi=iID_MaDonVi,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
</body>
</html>
