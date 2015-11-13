<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<%
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
    String sLNS = Request.QueryString["sLNS"];
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
    String BackURL = Url.Action("Index", "ThuNop_Report");
    using (Html.BeginForm("EditSubmit", "rptThuNop_TongHopPhanThuNganSach_42C_C", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo tổng hợp phần thu ngân sách năm</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="Div1">
            <div id="Div2" class="table_form2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">                    
                    <tr>                       
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px auto 5px auto;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <div><%=NgonNgu.LayXau("Chọn năm làm việc:")%></div>
                                    </td>
                                    <td width="2%"></td>
                                    <td style="width: 49%;" align="left">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamLamViec, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 30%\" ")%></div>
                                    </td>
                                </tr>
                           </table>
                        </td>                         
                    </tr>
                    <tr>                        
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 5px auto 10px auto;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%"></td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                           </table>
                        </td>                         
                    </tr>
                 </table>
            </div>
        </div>
    </div>    
    <%}
        dtNam.Dispose();
    %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThuNop_TongHopPhanThuNganSach_42C_C", new { NamLamViec = NamLamViec }), "ExportToExcel")%>
<div">    
    <iframe src="<%=Url.Action("ViewPDF","rptThuNop_TongHopPhanThuNganSach_42C_C",new{NamLamViec=NamLamViec})%>" height="600px" width="100%"></iframe>
</div>
