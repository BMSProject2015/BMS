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
        
        String MaND = User.Identity.Name;
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(iID_MaPhongBan);
        String sLNS = Convert.ToString(ViewData["sLNS"]); 
        //String sLNS = Request.QueryString["sLNS"];
        if(String.IsNullOrEmpty(sLNS))
        {
            if(dtLoaiNganSach.Rows.Count>0)
            {
                sLNS = Convert.ToString(dtLoaiNganSach.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "TenHT");

        
        String ParentID = "QuyetToanNganSach";
        //String TongHop = Request.QueryString["TongHop"];
        String TongHop = Convert.ToString(ViewData["TongHop"]);
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]); 
        //Trang Thai Duyet
        //String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        DataTable dtTrangThai = rptQuyetToan_8bController.tbTrangThai();
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

        DataTable dtQuy = new DataTable();
        dtQuy.Columns.Add("Quy", typeof(String));

        DataRow R1 = dtQuy.NewRow();
        R1["Quy"] = "1";
        dtQuy.Rows.InsertAt(R1, 0);
        DataRow R2 = dtQuy.NewRow();
        R2["Quy"] = "2";
        dtQuy.Rows.InsertAt(R2, 1);
        DataRow R3 = dtQuy.NewRow();
        R3["Quy"] = "3";
        dtQuy.Rows.InsertAt(R3, 2);
        DataRow R4 = dtQuy.NewRow();
        R4["Quy"] = "4";
        dtQuy.Rows.InsertAt(R4, 3);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "Quy", "Quy");
        String Quy = Convert.ToString(ViewData["Quy"]); 
        //String Quy = Request.QueryString["Quy"];
        if (String.IsNullOrEmpty(Quy))
        {
            Quy = "1";
        }
        //String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]); 
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            DataTable dtDonVi = rptQuyetToan_44_8Controller.LayDSDonVi(iID_MaTrangThaiDuyet, Quy, sLNS,MaND);
            if (dtDonVi.Rows.Count > 1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_44_8", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Quy = Quy, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS });
        }
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_44_8", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp quyết toán quý tháng</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2" style="margin-top:5px;">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                         <td width="10%">&nbsp;</td>
                        <td class="td_form2_td1" width="10%">
                            <div>
                               Chọn loại ngân sách:</div>
                        </td>
                        <td width="15%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiNganSach, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%></div>
                        </td>
                         <td class="td_form2_td1" width="10%">
                            <div>
                               Trạng Thái : </div>
                        </td>
                        <td width="7%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" width="10%">
                            <div>
                                Chọn Quý</div>
                        </td>
                        <td width="7%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy,Quy, "Quy", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonQuy()\"")%>
                            </div>
                        </td>
                     
                        <td class="td_form2_td1" width="10%">
                            <div>
                               Chọn đơn vị</div>
                        </td>
                        <td width="15%">
                            <div id="<%=ParentID %>_divDonVi">
                                <%rptQuyetToan_44_8Controller rptTB1 = new rptQuyetToan_44_8Controller();%>
                                <%=rptTB1.obj_DSDonVi(ParentID,iID_MaTrangThaiDuyet,Quy,sLNS,iID_MaDonVi,MaND)%>
                            </div>
                        </td>                                                         
                        <td </td>
                    </tr>
                    <tr>
                     <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                            <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 45%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="10%">
                                    </td>
                                    <td style="width: 45%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td> 
                           <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>                      
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_44_8", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Quy = Quy, iID_MaDonVi = iID_MaDonVi, sLNS = sLNS }), "Xuất ra Excels")%>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=URL %>';
         }
 </script>
    <script type="text/javascript">
        function ChonQuy() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var Quy = document.getElementById("<%=ParentID %>_Quy").value;
            var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Quy=#2&sLNS=#3&iID_MaDonVi=#4", "rptQuyetToan_44_8") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", Quy));
            url = unescape(url.replace("#3", sLNS));
            url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>
    <%dtNam.Dispose();
    %>
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
