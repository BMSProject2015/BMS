<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
    <%
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String iNamLamViec = ReportModels.LayNamLamViec(MaND);
        
        //lấy danh sách quý
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        //Lấy năm ngân sách
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();
        
        //lấy danh sách phòng ban
        String iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTenPhongBan");
        dtPhongBan.Dispose();
        
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        if (String.IsNullOrEmpty(iThang_Quy))
        {
            ReportModels.LayQuyHienTai();
        }
        
        //lấy danh sách loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaPhongBanND = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBanND);

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
        
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });

        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        String[] arrLNS = sLNS.Split(',');
        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrMaDonVi.Length];
        String Chuoi = "";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        if (String.IsNullOrEmpty(PageLoad))
            PageLoad = "0";

        //Nếu không chọn loại ngân sách thì không cho xuất báo cáo
        if (String.IsNullOrEmpty(sLNS))
        {
            PageLoad = "0";
            sLNS = Guid.Empty.ToString();
        }

        //Nếu không chọn đơn vị không cho xuất báo cáo
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            PageLoad = "0";
        } 
        
        if (PageLoad == "1")
        {
            for (int i = 0; i < arrMaDonVi.Length; i++)
            {
                arrView[i] = String.Format(@"/rptQuyetToan_TongQuyetToan_LNS_DonVi/viewpdf?iID_MaDonVi={0} &sLNS={1} &iThang_Quy={2} &iID_MaNamNganSach={3} &iID_MaPhongBan={4} &MaND={5}", arrMaDonVi[i], sLNS, iThang_Quy, iID_MaNamNganSach, iID_MaPhongBan, MaND);
                Chuoi += arrView[i];
                if (i < arrMaDonVi.Length - 1)
                    Chuoi += "+";
            }
        }

        int SoCot = 1;
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_TongQuyetToan_LNS_DonVi", new { ParentID = ParentID }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, MaND, "MaND", "")%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp tình hình quyết toán ngân sách năm
                            <%=iNamLamViec%></span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain" style="background-color: #F0F9FE;">
            <div id="Div2" style="margin-left: 10px;" class="table_form2">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <div>
                                <b>Chọn quý </b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <b>Loại ngân sách &nbsp;&nbsp; </b>
                        </td>
                        <td class="td_form2_td5" style="width: 25%" rowspan="25">
                             <div  style="overflow: scroll; height: 400px">
                            
                            <table class="mGrid" style="width: 100%">
                                <tr>
                                    <th align="center" style="width: 40px;">
                                        <input type="checkbox" id="abc" onclick="CheckAllDV(this.checked)" />
                                    </th>
                                    <%for (int c = 0; c < SoCot * 2 - 1; c++)
                                      {%>
                                    <th>
                                    </th>
                                    <%} %>
                                </tr>
                                <%
                                String strsTen = "", MaDonVi = "", strChecked = "";
                                for (int i = 0; i < dtLNS.Rows.Count; i = i + SoCot)
                                {
                                %>
                                <tr>
                                    <%for (int c = 0; c < SoCot; c++)
                                      {
                                          if (i + c < dtLNS.Rows.Count)
                                          {
                                              strChecked = "";
                                              strsTen = Convert.ToString(dtLNS.Rows[i + c]["TenHT"]);
                                              MaDonVi = Convert.ToString(dtLNS.Rows[i + c]["sLNS"]);
                                              for (int j = 0; j < arrLNS.Length; j++)
                                              {
                                                  if (MaDonVi.Equals(arrLNS[j]))
                                                  {
                                                      strChecked = "checked=\"checked\"";
                                                      break;
                                                  }
                                              }
                                    %>
                                    <td align="center" style="width: 40px;">
                                        <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="LNS"
                                            onclick="Chon()" id="sLNS" name="sLNS" />
                                    </td>
                                    <td align="left">
                                        <%=strsTen%>
                                    </td>
                                    <%} %>
                                    <%} %>
                                </tr>
                                <%}%>
                            </table>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 10px">
                            <b>Chọn đơn vị &nbsp;&nbsp;</b>
                        </td>
                        <td class="td_form2_td5" rowspan="25" style="width: 30%;">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn năm  </b></div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slNamNganSach, iID_MaNamNganSach, "iID_MaNamNganSach", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn phòng ban  </b></div>
                        </td>
                        <td>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, iID_MaPhongBan, "iID_MaPhongBan", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon() ")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                       
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <table cellpadding="0" cellspacing="0" border="0" align="center">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
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
        <script type="text/javascript">
            function CheckAllDV(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
                Chon();
            }                                            
        </script>
        
         <script type="text/javascript">
             function CheckAll(value) {
                 $("input:checkbox[check-group='DonVi']").each(function (i) {
                     this.checked = value;
                 });
             }                                            
         </script>
        <script type="text/javascript">
            $(document).ready(function () {
                   $('.title_tong a').click(function () {
                    $('div#rptMain').slideToggle('normal');
                    $(this).toggleClass('active');
                    return false;
                });
                var count = <%=arrView.Length%>;
                var Chuoi = '<%=Chuoi%>';
                var Mang=Chuoi.split("+");
                   var pageLoad = <%=PageLoad %>;
                   if(pageLoad=="1") {
                var siteArray = new Array(count);
                for (var i = 0; i < count; i++) {
                    siteArray[i] = Mang[i];
                }
                    for (var i = 0; i < count; i++) {
                        window.open(siteArray[i], '_blank');
                    }
                } 
            });
            
            Chon();

            function Chon() {
                  var sLNS = "";
             $("input:checkbox[check-group='LNS']").each(function (i) {
                 if (this.checked) {
                     if (sLNS != "") sLNS += ",";
                     sLNS += this.value;
                 }
             });
                Thang = document.getElementById("<%= ParentID %>_iThang_Quy").value;
                 var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
                 var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachDonVi?ParentID=#0&Thang_Quy=#1&iID_MaDonVi=#2&sLNS=#3&iID_MaNamNganSach=#4&iID_MaPhongBan=#5", "rptQuyetToan_TongQuyetToan_LNS_DonVi") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Thang));
                url = unescape(url.replace("#2","<%= iID_MaDonVi %>"));
                url = unescape(url.replace("#3",sLNS));
                url = unescape(url.replace("#4", iID_MaNamNganSach));
                url = unescape(url.replace("#5", iID_MaPhongBan));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data;
                });
            }
                                                     
        </script>
        <script type="text/javascript">
            function Huy() {
                window.location.href = '<%=BackURL%>';
            }
        </script>
    </div>
    <%} %>
</body>
</html>
