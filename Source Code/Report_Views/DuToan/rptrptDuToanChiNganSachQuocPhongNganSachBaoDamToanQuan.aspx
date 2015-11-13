<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
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
    String sNG = Request.QueryString["sNG"];

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
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToanNganSachNhaNuoc5aController.tbTrangThai();
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
    
    DataTable dtNguonDonVi = rptDuToanNganSachNhaNuoc5aController.Get_sNG();
    SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "sTenKhoa", "sTen");

    if (String.IsNullOrEmpty(sNG))
    {
        if (dtNguonDonVi.Rows.Count > 0)
            sNG = Convert.ToString(dtNguonDonVi.Rows[0]["sTenKhoa"]);
        else
            sNG = Guid.Empty.ToString();
    }
    String URL = Url.Action("Index", "DuToan_Report", new { sLoai = "0" });
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan", new { ParentID = ParentID, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sNG = sNG }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách quốc phòng (Ngân sách đảm bảo toàn quân)</span>
                    </td>
                </tr>
            </table>
        </div>        
        <div id="Div3">
            <div id="Div4">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td class="td_form2_td1" style="width: 113px"></td>
                    <td class="td_form2_td1" style="width: 113px"></td>
                        <td class="td_form2_td1" style="width: 70px">
                            <div>
                                <%=NgonNgu.LayXau("Trạng Thái : ")%>
                            </div>
                        </td>
                        <td class="td_form2_td5" style="width: 115px">
                            <div style="width:100%; padding:0;">
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 70px">
                            <div>
                                <%=NgonNgu.LayXau("Chọn ngành")%></div>
                        </td>
                        <td class="td_form2_td5" style="width: 113px">
                            <div style="width:100%; padding:0;">                               
                                <%=MyHtmlHelper.DropDownList(ParentID, slTenDonVi, sNG, "sNG", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sLNS")%>
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
    <%} %>
    <div ></div>    
    <%
        dtNam.Dispose();
        dtNguonDonVi.Dispose();
    %>
<script type="text/javascript">
    function Huy() {
        window.location.href = '<%=URL %>';
    }
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sNG = sNG }), "Xuất ra file Excel")%>
   <iframe src="<%=Url.Action("ViewPDF","rptDuToanChiNganSachQuocPhongNganSachBaoDamToanQuan",new{iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet,sNG=sNG})%>" height="600px" width="100%"></iframe>
</body>
</html>
