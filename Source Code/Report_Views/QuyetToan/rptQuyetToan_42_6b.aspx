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
        String ParentID = "QuyetToanNganSach";
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_42_6bController.tbTrangThai();
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
        String sLNS = "", TruongTien = "", Thang_Quy = "", LoaiThang_Quy = "", iID_MaDanhMuc = "", TongHop = "";
        
        TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);

        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }

        TongHop = Convert.ToString(ViewData["TongHop"]);
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
        dtNam.Dispose();
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
        sLNS = Convert.ToString(ViewData["sLNS"]);

        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNghiepVuNhaNuoc_PhongBan(iID_MaPhongBan);
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");      
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = Convert.ToString(dtLNS.Rows[0]["sLNS"]);
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        dtLNS.Dispose();
        
        DataTable dtNDV = HamChung.Lay_dtDanhMuc("Nhomdonvi");
        SelectOptionList slNDV = new SelectOptionList(dtNDV, "iID_MaDanhMuc", "sTen");
        iID_MaDanhMuc = Convert.ToString(ViewData["iID_MaDanhMuc"]);
        if (String.IsNullOrEmpty(iID_MaDanhMuc))
        {
            if (dtNDV.Rows.Count > 0)
            {
                iID_MaDanhMuc = dtNDV.Rows[0]["iID_MaDanhMuc"].ToString();
            }
            else
            {

                iID_MaDanhMuc = Guid.Empty.ToString();
            }
        }
        dtNDV.Dispose();
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptQuyetToan_42_6b", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, TruongTien = TruongTien, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDanhMuc = iID_MaDanhMuc, TongHop = TongHop });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_42_6b", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp chuẩn quyết toán nhóm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td width="15%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                Trạng thái:</div>
                        </td>
                        <td style="width: 10%;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                Tháng/Quý:</div>
                        </td>
                        <td style="width: 20%;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang_Quy, "iThang", "", "class=\"input1_2\" style=\"width:30%;\"")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Thang_Quy, "iQuy", "", "class=\"input1_2\" style=\"width:30%;\"")%><br />
                            </div>
                        </td>
                       <td class="td_form2_td1" style="width: 10%;">
                            <div>
                                Chọn trường tiền:</div>
                        </td>
                        <td style="width: 10%;">
                            <div>
                                <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "")%>Tự Chi
                                <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "")%>Hiện
                                Vật
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="td_form2_td1" >
                            <div>
                                Chọn nhóm đơn vị:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNDV, iID_MaDanhMuc, "iID_MaDanhMuc", "", "class=\"input1_2\" style=\"width: 100%\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" >
                            <div>
                                Loại ngân sách:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 94%\"")%><br />
                            </div>
                        </td>
                         
                         <td class="td_form2_td1" >
                            <div>
                                Tổng hợp:</div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, TongHop, "iTongHop","","")%></div>
                        </td>
                        <td></td>
                    </tr> 
                       <tr>
                    <td></td>
                    <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td>
                           <td></td> 
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
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_42_6b", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, TruongTien = TruongTien, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDanhMuc = iID_MaDanhMuc, TongHop = TongHop }), "Xuất ra Excels")%>
    <%} %>
    <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
