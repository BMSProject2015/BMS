<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%  
        String ParentID = "QuyetToan";
        String MaND = User.Identity.Name;
        String Thang = "0";
        String Quy = "0";
        String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
        if (String.IsNullOrEmpty(LoaiThang_Quy))
        {
            LoaiThang_Quy = "0";
        }
        if (LoaiThang_Quy == "0")
        {
            Thang = Thang_Quy;
            Quy = "0";
        }
        else
        {
            Thang = "0";
            Quy = Thang_Quy;
        }
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_ThuongXuyen_22_1Controller.tbTrangThai();
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
        DataTable dtNam = DanhMucModels.DT_Nam();
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();



        DataTable dtDonVi = QuyetToan_ReportModels.DanhSachDonVi(iID_MaTrangThaiDuyet, Thang_Quy, LoaiThang_Quy, MaND);

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 1)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[1]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        dtDonVi.Dispose();
        ;
        String URL = Url.Action("Index", "QuyetToan_Report", new { Loai = "0" });
        String urlReport = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String LoaiBaoCao1 = "", LoaiBaoCao2 = "", LoaiBaoCao3 = "", chkNgayNguoi = ""; ;
        LoaiBaoCao1 = Convert.ToString(ViewData["LoaiBaoCao1"]);
        LoaiBaoCao2 = Convert.ToString(ViewData["LoaiBaoCao2"]);
        LoaiBaoCao3 = Convert.ToString(ViewData["LoaiBaoCao3"]);
        chkNgayNguoi = Convert.ToString(ViewData["chkNgayNguoi"]);

        String LoaiBaoCao = "";
        if (LoaiBaoCao1 == "on")
            LoaiBaoCao = "1,";
        if (LoaiBaoCao2 == "on")
            LoaiBaoCao += "2,";
        if (LoaiBaoCao3 == "on")
            LoaiBaoCao += "3";
        if (String.IsNullOrEmpty(LoaiBaoCao))
        {
            LoaiBaoCao = "1";
            LoaiBaoCao1 = "on";
        }
        if (LoaiBaoCao.Length % 2 == 0)
        {
            LoaiBaoCao = LoaiBaoCao.Substring(0, LoaiBaoCao.Length - 1);
        }
        String[] arrLBC = LoaiBaoCao.Split(',');
        String MaBaoCao = "1";
        int ChiSo = 0;
        if (String.IsNullOrEmpty(LoaiBaoCao) == false)
        {
            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if (ChiSo >= arrLBC.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrLBC.Length - 1)
            {
                MaBaoCao = arrLBC[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
            }
        }
        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThuongXuyen_22_1", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy, iID_MaDonVi = iID_MaDonVi, LoaiBaoCao = MaBaoCao, chkNgayNguoi = chkNgayNguoi });
        }

        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThuongXuyen_22_1", new { ParentID = ParentID, ChiSo = ChiSo }))
        {                        
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 45%">
                        <span>Báo cáo nhập dữ liệu</span>
                    </td>
                    <%--<td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>--%>
                </tr>
            </table>
        </div>
        <div id="table_form2" class="table_form2">
            <%--  <div id="rptMain" style=" margin:5px auto; padding:5px 5px; overflow:visible; text-align:center">
               <ul class="inlineBlock">
               
               <li style=" height:30px;">
                   
                        <p>
                           
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:50px;\" onchange=\"Chon()\"")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:80px;\" onchange=\"Chon()\"")%>
                            
                               <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:50px;\" onchange=\"Chon()\"")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:80px;\" onchange=\"Chon()\"")%>
                        </p>
                  
               
               </li>
               <li style=" height:30px;  text-align:inherit">
               
                   
                    
                            <%=NgonNgu.LayXau("Đơn vị")%></li ><li style=" height:30px;">
                          <div id="<%=ParentID %>_divDonVi" style = " width:150px;">
                             
                            </div> 
                            
            
               </li>
               
               <li style=" height:30px;">
               
                   
                  
                            <span style="text-align:right;padding:2px 1px; line-height:28px;" ><label class="label" style="min-width:132px; margin-right:7px;">
                            <%=NgonNgu.LayXau("Trạng thái")%>&nbsp;&nbsp;
                            </label> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:200px\"")%></span>                                           
                                               
                   
               </li>
              <li style=" height:30px; width:200px;">
                
                                                                     
                            <div style="float:left;">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>Số QT:</td>
                                    <td id="lblSoQT"></td>
                                </tr>
                                <tr>
                                    <td>Tổng số:</td>
                                    <td id="lblTongSo"></td>
                                </tr>
                            </table>
                            </div>
                    
                    </li>
                     <li style=" height:30px;">
               
                   
                  
                            <span style="text-align:right;padding:2px 1px; line-height:28px;" ><label class="label" style="min-width:132px; margin-right:7px;">
                            <%=NgonNgu.LayXau("Trạng thái")%>&nbsp;&nbsp;
                            </label> <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:200px\"")%></span>                                           
                                            
                   
               </li>
               </ul>
               
                  
                    <p style="text-align:center; padding:4px;"><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>                    
                </div>--%>
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td width="10%">
                    </td>
                    <td width="28%">
                        <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:50px;\" onchange=\"Chon()\"")%>Tháng
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:80px;\" onchange=\"Chon()\"")%>
                        <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:50px;\" onchange=\"Chon()\"")%>Quý
                        <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:80px;\" onchange=\"Chon()\"")%>
                    </td>
                    <td width="8%" align="right">
                        <span style="text-align: right; padding: 2px 1px; line-height: 28px;">
                            <label class="label" style="min-width: 132px; margin-right: 7px;">
                                <%=NgonNgu.LayXau(" Đơn vị:              ")%>&nbsp;&nbsp;
                            </label>
                        </span>
                    </td>
                    <td width="15%">
                        <div id="<%=ParentID %>_divDonVi" style="width: 150px;">
                        </div>
                    </td>
                    <td align="left">
                        
                            <label class="label" style="min-width: 132px; margin-right: 7px;">
                                <%=NgonNgu.LayXau("Trạng thái:")%>&nbsp;&nbsp;
                            </label>
                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "style=\"width:200px\" onchange=\"Chon()\"")%></span>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <div style="float: left; margin-left: 20px">
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        Số QT:
                                    </td>
                                    <td id="lblSoQT">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Tổng số:
                                    </td>
                                    <td id="lblTongSo">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td align="right">
                        <span style="text-align: right; padding: 2px 1px; line-height: 28px;">
                            <label class="label" style="min-width: 132px; margin-right: 7px;">
                                <%=NgonNgu.LayXau("Loại báo cáo:")%>&nbsp;&nbsp;
                            </label>
                        </span>
                    </td>
                    <td>
                        <table border="0" cellspacing="0" cellpadding="0" width="100%">
                            <tr>
                                <td>
                                    <%=MyHtmlHelper.CheckBox(ParentID,LoaiBaoCao1,"LoaiBaoCao1","","")%>
                                </td>
                                <td>
                                    Tờ số liệu
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=MyHtmlHelper.CheckBox(ParentID,LoaiBaoCao2,"LoaiBaoCao2","","")%>
                                </td>
                                <td>
                                    Tờ giải thích bằng số
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%=MyHtmlHelper.CheckBox(ParentID,LoaiBaoCao3,"LoaiBaoCao3","","")%>
                                </td>
                                <td>
                                    Tờ giải thích bằng lời
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                     Có ngày/người: <%=MyHtmlHelper.CheckBox(ParentID, chkNgayNguoi, "chkNgayNguoi", "", "")%>
                    </td>
                </tr>
                <tr>
                <td></td>
                    <td  align="right" rowspan="3">
                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                    </td>
                    <td >
                    </td>
                    <td align="left">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        //          $(document).ready(function () {

        //              $('.title_tong a').click(function () {
        //                  $('div#rptMain').slideToggle('normal');
        //                  $(this).toggleClass('active');
        //                  return false;
        //              });
        //          });   
    </script>
    <script type="text/javascript">
        Chon();
        function Chon() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%= ParentID %>_iID_MaTrangThaiDuyet").value;

            var TenLoaiThang_Quy = document.getElementsByName("<%=ParentID %>_LoaiThang_Quy");
            var LoaiThang_Quy;
            var Thang_Quy;
            var i = 0;
            for (i = 0; i < TenLoaiThang_Quy.length; i++) {
                if (TenLoaiThang_Quy[i].checked) {
                    LoaiThang_Quy = TenLoaiThang_Quy[i].value;
                }
            }
            if (LoaiThang_Quy == 0) {
                Thang_Quy = document.getElementById("<%=ParentID %>_iThang").value;
            }
            else {
                Thang_Quy = document.getElementById("<%=ParentID %>_iQuy").value;
            }

            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
            var iID_MaDonVi = '-1111';
            if (objDonVi != null) {
                iID_MaDonVi = objDonVi.options[objDonVi.selectedIndex].value;
            }
            else {
                iID_MaDonVi = '<%=iID_MaDonVi %>';
            }

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&iID_MaTrangThaiDuyet=#1&Thang_Quy=#2&LoaiThang_Quy=#3&iID_MaDonVi=#4&MaND=#5", "rptQuyetToan_ThuongXuyen_22_1") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", Thang_Quy));
            url = unescape(url.replace("#3", LoaiThang_Quy));
            url = unescape(url.replace("#4", iID_MaDonVi));
            url = unescape(url.replace("#5", "<%= MaND%>"));
            $.getJSON(url, function (item) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = item.dsDonVi;
                document.getElementById("lblSoQT").innerHTML = item.sSoQT;
                document.getElementById("lblTongSo").innerHTML = item.sTongQT;
            });
        }                                            
    </script>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptQuyetToan_ThuongXuyen_22_1", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, LoaiThang_Quy = LoaiThang_Quy, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi, LoaiBaoCao = MaBaoCao, chkNgayNguoi = chkNgayNguoi }), "Xuất ra Excel")%>
    <iframe src="<%=urlReport%>" height="600px" width="100%"></iframe>
</body>
</html>
