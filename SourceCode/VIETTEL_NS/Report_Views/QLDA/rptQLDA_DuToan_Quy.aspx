<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>DỰ TOÁN QUÝ</title>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QLDA";
        String Quy = Request.QueryString["Quy"];
         var dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        if (dtQuy != null) dtQuy.Dispose();
        if (String.IsNullOrEmpty(Quy))
        {
            Quy = dtQuy.Rows[1]["MaQuy"].ToString();
        }
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
        String URL = Url.Action("Index", "QLDA_Report");
        using (Html.BeginForm("EditSubmit", "rptQLDA_DuToan_Quy", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 113px"></td>
                        <td class="td_form2_td1" style="width: 113px"></td>
                        <td class="td_form2_td1" style="width: 70px">
                            <div>
                                <%=NgonNgu.LayXau("Chọn quý")%>
                            </div>
                        </td>
                        <td class="td_form2_td5" style="width: 115px">
                            <div style="padding:0px 0px;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 70px">
                            <div><%=NgonNgu.LayXau("Chọn năm")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 113px">
                            <div style="padding:0px 0px;">                               
                                <%=MyHtmlHelper.DropDownList(ParentID, slNam, Nam, "iNam", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 113px"></td>
                        <td class="td_form2_td1" style="width: 113px"></td>
                    </tr>             
                    
                     <tr>                           
                        <td class="td_form2_td1" style="text-align:center; " colspan="8">  
                           <table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 2px auto; width:200px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2px">
                                    </td>
                                    <td>
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
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
    </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQLDA_DuToan_Quy", new { Quy = Quy, Nam = Nam }), "Xuất ra file Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptQLDA_DuToan_Quy", new{Quy=Quy, Nam = Nam})%>" height="600px" width="100%">
    </iframe>
</body>
</html>
