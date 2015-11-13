<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String MaND = User.Identity.Name;
        String iThangLamViec = DanhMucModels.ThangLamViec(MaND).ToString();
        Object objNam = NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec");
        int iNam = DateTime.Now.Year;
        if (objNam != null) iNam = Convert.ToInt16(objNam);
        String iNamLamViec = Convert.ToString(iNam);

        String iID = Convert.ToString(ViewData["iID_MaDonVi"]);

        String iNgay1 = Convert.ToString(ViewData["iNgay1"]);
        String iNgay2 = Convert.ToString(ViewData["iNgay2"]);
        String iID_MaTaiKhoan = Convert.ToString(ViewData["DSMaTaiKhoan"]);
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            iID_MaTaiKhoan = "-1";
        }
        String iThang1 = Convert.ToString(ViewData["iThang1"]);
        String iThang2 = Convert.ToString(ViewData["iThang2"]);
        if (String.IsNullOrEmpty(iThang1)) iThang1 = iThangLamViec;
        if (String.IsNullOrEmpty(iThang2)) iThang2 = iThangLamViec;
        String sLoaiBaoCao = Convert.ToString(ViewData["LoaiBaoCao"]);
        if (String.IsNullOrEmpty(sLoaiBaoCao)) sLoaiBaoCao = "0";
        int ThangHienTai = DateTime.Now.Month;
        //Chọn từ tháng
        DataTable dtThang = DanhMucModels.DT_Thang(false);

        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        //Chọn từ ngày
        DataTable dtNgay = DanhMucModels.DT_Ngay(Convert.ToUInt16(iThang2), iNam, false);
        SelectOptionList slNgay = new SelectOptionList(dtNgay, "MaNgay", "TenNgay");
        if (String.IsNullOrEmpty(iNgay1))
        {
            iNgay1 = Convert.ToString(dtNgay.Rows[0]["TenNgay"]);
        }
        dtNgay.Dispose();
        iNgay2 = Convert.ToString(dtNgay.Rows[dtNgay.Rows.Count - 1]["TenNgay"]);
        String ParentID = "KeToan";
       String BackURL = Url.Action("SoDoLuong", "KeToanTongHop");
        String Cap = "3";
        using (Html.BeginForm("Loc_Submit", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc", new { ParentID = ParentID, iNamLamViec = iNamLamViec }))
        { 
    %>
    
    <script src="../../../Scripts/Report/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/jquery-ui-1.8.23.custom.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/Report/ShowDialog.js" type="text/javascript"></script>
    
     <style type="text/css">          
        fieldset{padding:3px;border:1px solid #dedede; border-radius:3px; -webkit-border-radius:3px; -moz-border-radius:3px;}
        fieldset legend{padding:3px; font-size:14px;font-family:Tahoma Arial;}    
        fieldset p{padding:2px;}
        fieldset p span{padding:2px; font-size:12px; font-weight:bold;}
        div#td_TaiKhoan.mGrid tr:even{background-color:#dedede;}
    </style>
    <div class="box_tong" id="div_Tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>                    
                    <td width="47.9%">
                        <span>Báo cáo chi tiết + tổng hợp tài khoản</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td colspan="3" class="td_form2_td1">
                            <div style="float: left; font-weight: bold;">
                                <%=NgonNgu.LayXau("Danh sách tài khoản")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" width="500px">
                            <div id="td_TaiKhoan" style="width: 100%; height: 400px; overflow: scroll; border: 1px solid black;">
                                <% rptKTTongHop_ChiTiet_TongHopTaiKhoanController rpt = new rptKTTongHop_ChiTiet_TongHopTaiKhoanController();           
                                %>
                                <%=rpt.sDanhSachTaiKhoan("3", iThang1, iThang2, iNamLamViec)%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px; font-weight: bold;" valign="top">
                            <div>
                                <fieldset>
                                    <legend><b>Chọn tài khoản đến cấp</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Cấp 1
                                                </div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID,"3",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Cấp 2</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "4", Cap, "CapTK", "", " style=\"width:30px\" onclick=\"ChonCap(this.value)\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Cấp 3</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "5", Cap, "CapTK", "", "style=\"width:30px\" onclick=\"ChonCap(this.value)\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    Cấp 4</div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "6", Cap, "CapTK", "", " style=\"width:30px\" onclick=\"ChonCap(this.value)\"")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div>
                                <fieldset>
                                    <legend><b>Hình thức báo cáo</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Chi tiết tài khoản</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "0", sLoaiBaoCao, "iLoaiBaoCao", "", " style=\"width:30px\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" style="padding-right: 10px;">
                                                <div>
                                                    <b>Tổng hợp tài khoản</b></div>
                                            </td>
                                            <td class="td_form2_td1">
                                                <div>
                                                    <%=MyHtmlHelper.Option(ParentID, "1", sLoaiBaoCao, "iLoaiBaoCao", "", " style=\"width:30px\" ")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                            <div>
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="right">
                                            <div>
                                                <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                            </div>
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <div>
                                                <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="padding-left: 20px;" valign="top">
                            <div>
                                <fieldset>
                                    <legend><b>Từ Ngày ... tháng Đến Ngày ... tháng</b></legend>
                                    <table>
                                        <tr>
                                            <td class="td_form2_td1" width="80px" style="padding-right: 10px;">
                                                <div>
                                                    Từ Ngày</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div id="td_iNgay1">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay1, "iNgay1", "","style=\"width:50px\"")%></div>
                                            </td>
                                            <td class="td_form2_td1" width="50px" style="padding-right: 10px;">
                                                <div>
                                                    Tháng</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:50px\" onchange=\"ChonThang(this.value,'iNgay1')\"")%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1" width="80px" style="padding-right: 10px;">
                                                <div>
                                                    Đến Ngày</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div id="td_iNgay2">
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay2, "iNgay2", "","style=\"width:50px\"")%></div>
                                            </td>
                                            <td class="td_form2_td1" width="50px" style="padding-right: 10px;">
                                                <div>
                                                    Tháng</div>
                                            </td>
                                            <td class="td_form2_td1" align="left">
                                                <div>
                                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:50px\" onchange=\"ChonThang(this.value,'iNgay2')\"")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>--%>
        <div id="confirmBox" title="Báo cáo chi tiết + tổng hợp tài khoản" style="width:650px; min-height:200px;">
            <div>
                <div id="left" style="float:left; width:305px; text-align:left; padding:3px;">
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Từ ngày&nbsp;&nbsp;&nbsp;Tháng&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đến ngày&nbsp;&nbsp;Tháng")%></legend>
                        <p style="margin-bottom:5px;">
                            <label id="td_iNgay1"><%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay1, "iNgay1", "","style=\"width:60px\"")%></label>
                            <label><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang1, "iThang1", "", "style=\"width:60px\" onchange=\"ChonThang(this.value,'iNgay1')\"")%></label>
                            <label id="td_iNgay2"><%=MyHtmlHelper.DropDownList(ParentID, slNgay, iNgay2, "iNgay2", "","style=\"width:60px\"")%></label>
                            <label><%=MyHtmlHelper.DropDownList(ParentID, slThang, iThang2, "iThang2", "", "style=\"width:60px\" onchange=\"ChonThang(this.value,'iNgay2')\"")%></label>
                        </p>
                    </fieldset>
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Chọn đến TK cấp:") %></legend>
                        <p><%=MyHtmlHelper.Option(ParentID,"3",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 1") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"4",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 2") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"5",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 3") %></span></p>
                        <p><%=MyHtmlHelper.Option(ParentID,"6",Cap,"CapTK",""," style=\"width:30px\" onclick=\"ChonCap(this.value)\"") %><span><%=NgonNgu.LayXau("Cấp 4") %></span></p>                        
                    </fieldset>
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Hình thức báo cáo:") %></legend>
                        <p style="margin-bottom:5px;">
                            <span><%=MyHtmlHelper.Option(ParentID, "0", sLoaiBaoCao, "iLoaiBaoCao", "", " style=\"width:30px\"")%></span><span>Chi tiết tài khoản</span>
                            <span><%=MyHtmlHelper.Option(ParentID, "1", sLoaiBaoCao, "iLoaiBaoCao", "", " style=\"width:30px\" ")%></span><span>Tổng hợp tài khoản</span>
                        </p>
                    </fieldset>
                </div><!--End #left-->
                <div id="right" style="float:right; width:305px; text-align:left; padding:3px;">
                    <fieldset>
                        <legend><%=NgonNgu.LayXau("Danh sách tài khoản:") %></legend>
                         <table class="mGrid">
                            <tr>
                                <th align="center" style="width: 25px;">
                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                </th>
                                <th align="left" style="font-size: 12px;">
                                    Tài khoản
                                </th>
                            </tr>
                        </table>
                        <div id="td_TaiKhoan" style="max-height:200px; height:200px;min-height:200px; overflow:scroll; margin-bottom:2px;">
                            <% rptKTTongHop_ChiTiet_TongHopTaiKhoan_CucController rpt = new rptKTTongHop_ChiTiet_TongHopTaiKhoan_CucController(); %>
                            <%=rpt.sDanhSachTaiKhoan("3", iThang1, iThang2, iNamLamViec)%> 
                        </div>
                    </fieldset>
                </div><!--End #right-->
            </div><!--End div--->
            <p style="text-align:center; padding:4px; clear:both;"><input type="submit" class="button" id="Submit2" onclick='clicks()' value="<%=NgonNgu.LayXau("Thực hiện")%>" style="display:inline-block; margin-right:5px;" /><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>"  onclick="Huy()" style="display:inline-block; margin-left:5px;" /></p>
        </div><!--End #confirmBox-->
    </div>
    <script type="text/javascript">     
           function ChonCap(value)
           {
                var TuThang=document.getElementById("<%=ParentID %>_iThang1").value;
                var DenThang=document.getElementById("<%=ParentID %>_iThang2").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("Get_objTaiKhoan?CapTK=#0&TuThang=#1&DenThang=#2&iNamLamViec=#3", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc") %>');
                    url = unescape(url.replace("#0",value));
                    url = unescape(url.replace("#1",TuThang));
                    url = unescape(url.replace("#2",DenThang));
                    url = unescape(url.replace("#3", <%=iNamLamViec %>));
                    $.getJSON(url, function (data) {
                        document.getElementById("td_TaiKhoan").innerHTML = data;
                    });                          
           }
           function ChonThang(Thang,TenTruong) {
               var cap=document.getElementById("<%=ParentID %>_CapTK").value;
               var Ngay = document.getElementById("<%=ParentID %>_" + TenTruong).value;
               jQuery.ajaxSetup({ cache: false });
               var url = unescape('<%= Url.Action("Get_objNgayThang?ParentID=#0&TenTruong=#1&Ngay=#2&Thang=#3&iNam=#4", "rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc") %>');
               url = unescape(url.replace("#0", "<%= ParentID %>"));
               url = unescape(url.replace("#1", TenTruong));
               url = unescape(url.replace("#2", Ngay));
               url = unescape(url.replace("#3", Thang));
               url = unescape(url.replace("#4", <%=iNamLamViec %>));
               $.getJSON(url, function (data) {
                   document.getElementById("td_"+TenTruong).innerHTML = data;                    
                    ChonCap(cap);
               });
               
           }         
            $(function(){
                ShowDialog(640);
                 $('*').keyup(function (e) {
                if (e.keyCode == '27') {
                        Hide();
                    }
                });
                $('div.login1 a').click(function () {
                    ShowDialog(640);
                });                      
            });        
                                      
    </script>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='iID_MaTaiKhoan']").each(function (i) {
                this.checked = value;                    
            });
        }                                            
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
    <%} %>
</asp:Content>