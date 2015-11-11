<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo_Tong" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        
        String ParentID = "TongHopChiTieu";
        
        //Loai ngan sach
        String sLNS = Convert.ToString(ViewData["sLNS"]);       
        DataTable dtLoaiNganSach = DanhMucModels.NS_LoaiNganSach();
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "TenHT");
        if (String.IsNullOrEmpty(sLNS))
        {
            if(dtLoaiNganSach.Rows.Count>0)
            {
                sLNS = dtLoaiNganSach.Rows[0]["sLNS"].ToString();
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        String TruongTien = Convert.ToString(ViewData["TruongTien"]); 
        if(String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }

        String MaND = User.Identity.Name;
        //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]); 
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        }
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHePhanBo);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
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
        dtNam.Dispose();
        String iID_MaDotPhanBo = Convert.ToString(ViewData["DotPhanBo"]);
        DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND, iID_MaTrangThaiDuyet, sLNS);
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            if (dtDotPhanBo.Rows.Count > 1)
            {
                iID_MaDotPhanBo = dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"].ToString();
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
        }
        String URL = Url.Action("Index", "PhanBo_Report");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptPBTong5", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, DotPhanBo = iID_MaDotPhanBo, TruongTien = TruongTien });
        using (Html.BeginForm("EditSubmit", "rptPBTong5", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp chỉ tiêu</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">                           
                            <tr>
                                <td style="width: 1%;"></td>
                                <td class="td_form2_td1" style="width: 10%;">
                                    <div>
                                        <%=NgonNgu.LayXau("Trạng Thái:")%></div>
                                </td>
                                <td width="10%">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "","class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNLV()\"")%>
                                    </div>
                                </td>                            
                          
                                <td class="td_form2_td1" style="width: 10%;">
                                    <div>
                                        <%=NgonNgu.LayXau("Loại Ngân sách:")%></div>
                                </td>
                                <td width="20%">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slLoaiNganSach, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNLV()\"")%>
                                    </div>
                                </td>

                                <td class="td_form2_td1" style="width: 10%;">
                                    <div>
                                        
                                        <%=NgonNgu.LayXau("Đơt phân bổ")%></div>
                                </td>
                                <td width="10%" >                                                               
                                     <div id="<%=ParentID %>_divDotPhanBo">
                                     <%rptPBTong5Controller rptTH = new rptPBTong5Controller(); %>
                                     <%=rptTH.obj_DSDotPhanBo(ParentID, MaND,iID_MaTrangThaiDuyet,sLNS,iID_MaDotPhanBo)%>
                                     </div>                                  
                                </td>
                         
                                <td class="td_form2_td1" width="10%"><div>
                                <%=NgonNgu.LayXau("Trường Tiền:")%></div></td>
                                <td>                           
                                Tư chi:
                                <%=MyHtmlHelper.Option(ParentID,"rTuChi",TruongTien,"TruongTien","")
                                 %>
                                 Hiện vật
                                  <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "")
                                 %>
                                </td>   
                                <td></td>
                                </tr>   
                                <tr>
                        <td>
                        </td>
                         
                        <td colspan="8"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
     <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptPBTong5", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sLNS = sLNS, DotPhanBo = iID_MaDotPhanBo, TruongTien = TruongTien }), "Xuất ra Excel")%>
       <script type="text/javascript">
           function Huy() {
               window.location.href = '<%=URL %>';
           }
            </script>
    <script type="text/javascript">
        function ChonNLV() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&iID_MaTrangThaiDuyet=#1&sLNS=#2&iID_MaDotPhanBo=#3&MaND=#4", "rptPBTong5") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", sLNS));
            url = unescape(url.replace("#3", "<%= iID_MaDotPhanBo %>"));
            url = unescape(url.replace("#4", "<%= MaND %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>   
    <iframe src="<%=URLView%>" height="600px" width="100%">
    </iframe>
</body>
</html>