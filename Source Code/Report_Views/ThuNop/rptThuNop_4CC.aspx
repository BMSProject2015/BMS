<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }

    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
    int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMin; i < NamMax; i++)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm ngân sách --";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    DataTable dtNguonDonVi = rptThuNop_4CC_Controller.DonVi(NamLamViec);   
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtNguonDonVi.Rows.Count > 0)
            iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
        else
            iID_MaDonVi = Guid.Empty.ToString();
    }
    String BackURL = Url.Action("Index", "ThuNop_Report");
    using (Html.BeginForm("EditSubmit", "rptThuNop_4CC_", new { ParentID = ParentID, iID_MaDonVi = iID_MaDonVi }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo tổng hợp dự toán ngân sách năm-Phần thu</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="Div1">
            <div id="Div2" class="table_form2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                         <td width="25%"></td>
                        <td class="td_form2_td1" style="width: 13%;">
                            <div><%=NgonNgu.LayXau("Chọn năm làm việc:")%></div>
                        </td>
                        <td style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNamLamViec()\"")%>                                
                            </div>
                        </td>                   
                        <td class="td_form2_td1" style="width: 13%;">
                            <div><%=NgonNgu.LayXau("Chọn đơn vị")%></div>
                        </td>
                        <td style="width: 15%;">
                            <div id="<%=ParentID %>_divDonVi">
                                     <%rptThuNop_4CC_Controller rptTB1 = new rptThuNop_4CC_Controller();%>                                    
                                     <%=rptTB1.obj_DSDonVi(ParentID,NamLamViec,iID_MaDonVi)%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                          <tr>
                        <td>
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
    <div ></div>   
  <%
    dtNam.Dispose();
   dtNguonDonVi.Dispose();
%>
    <script type="text/javascript">
        function ChonNamLamViec() {
        var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value

        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamLamViec=#1&iID_MaDonVi=#2", "rptThuNop_4CC_") %>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", NamLamViec));
        url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
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
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNop_4CC_", new { NamLamViec = NamLamViec, iID_MaDonVi = iID_MaDonVi }), "Xuất ra excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptThuNop_4CC_",new{NamLamViec=NamLamViec,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%"></iframe>
</body>
</html>
