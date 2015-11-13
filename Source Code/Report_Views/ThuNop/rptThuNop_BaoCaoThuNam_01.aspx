<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCao_ThuNam";
    String Nam = Request.QueryString["Nam"];
    if (String.IsNullOrEmpty(Nam))
    {
        Nam = DateTime.Now.Year.ToString();
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
    DataTable dtNguonDonVi = rptThuNop_BaoCaoThuNam_01Controller.DonVi(Nam);    
    String MaDV = Request.QueryString["MaDV"];
    if (String.IsNullOrEmpty(MaDV))
    {
        if (dtNguonDonVi.Rows.Count > 0)
            MaDV = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
        else
            MaDV = Guid.Empty.ToString();
    }
    String BackURL = Url.Action("Index", "ThuNop_Report");
    using (Html.BeginForm("EditSubmit", "rptThuNop_BaoCaoThuNam_01", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo kết quả hoạt động có thu năm</span>
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
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNamLamViec()\"")%>                                
                            </div>
                        </td>                   
                        <td class="td_form2_td1" style="width: 13%;">
                            <div><%=NgonNgu.LayXau("Chọn đơn vị")%></div>
                        </td>
                        <td style="width: 15%;">
                            <div id="<%=ParentID %>_divDonVi">
                                     <%rptThuNop_BaoCaoThuNam_01Controller rptTB1 = new rptThuNop_BaoCaoThuNam_01Controller();%>                                    
                                     <%=rptTB1.obj_DSDonVi(ParentID,Nam,MaDV)%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                          <tr>
                        <td>
                        </td>
                         
                        <td colspan="4"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 5px auto 10px auto;" width="100%">
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
    <script type="text/javascript">
        function ChonNamLamViec() {
        var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value

        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamLamViec=#1&iID_MaDonVi=#2", "rptThuNop_BaoCaoThuNam_01") %>');
        url = unescape(url.replace("#0", "<%= ParentID %>"));
        url = unescape(url.replace("#1", NamLamViec));
        url = unescape(url.replace("#2", "<%= MaDV %>"));
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
    dtNam.Dispose();
    dtNguonDonVi.Dispose();
  %>
  <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNop_BaoCaoThuNam_01", new { Nam = Nam, MaDV = MaDV }), "ExportToExcel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptThuNop_BaoCaoThuNam_01",new{Nam=Nam,MaDV=MaDV})%>" height="600px" width="100%">
   </iframe>
</body>
</html>
