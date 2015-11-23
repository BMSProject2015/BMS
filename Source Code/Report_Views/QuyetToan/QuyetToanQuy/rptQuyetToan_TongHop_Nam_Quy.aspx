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
        
        //Tùy chọn chi tiết báo cáo
        String iID_TuyChon = Convert.ToString(ViewData["iID_TuyChon"]);
        DataTable dtTuyChon = new DataTable("dtTuyChon");
        DataColumn IdTuyChon = dtTuyChon.Columns.Add("IdTuyChon", typeof(String));
        DataColumn TenTuyChon = dtTuyChon.Columns.Add("TenTuyChon", typeof(String));
        DataRow R1 = dtTuyChon.NewRow();
        R1["IdTuyChon"] = "1";
        R1["TenTuyChon"] = "Ngành";
        DataRow R2 = dtTuyChon.NewRow();
        R2["IdTuyChon"] = "2";
        R2["TenTuyChon"] = "LNS";
        DataRow R3 = dtTuyChon.NewRow();
        R3["IdTuyChon"] = "3";
        R3["TenTuyChon"] = "M";
        DataRow R4 = dtTuyChon.NewRow();
        R4["IdTuyChon"] = "4";
        R4["TenTuyChon"] = "TM";
        dtTuyChon.Rows.Add(R1);
        dtTuyChon.Rows.Add(R2);
        dtTuyChon.Rows.Add(R3);
        dtTuyChon.Rows.Add(R4);
        SelectOptionList slTuyChon = new SelectOptionList(dtTuyChon, "IdTuyChon", "TenTuyChon");
        
        //Danh sách quý
        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        DataRow Row1 = dtQuy.NewRow();
        Row1["MaQuy"] = "5";
        Row1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(Row1);
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();

        //Danh sách năm ngân sách
        String iID_MaNamNganSach = Convert.ToString(ViewData["iID_MaNamNganSach"]);
        DataTable dtNamNganSach = QuyetToanModels.getDSNamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "MaLoai", "sTen");
        dtNamNganSach.Dispose();

        //Danh sách phòng ban
        string iID_MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
        DataTable dtPhongBan = QuyetToanModels.getDSPhongBan(iNamLamViec, MaND);
        SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan,"iID_MaPhongBan","sTenPhongBan");
        dtPhongBan.Dispose();
        
        String iThang_Quy = Convert.ToString(ViewData["iThang_Quy"]);
        if (String.IsNullOrEmpty(iThang_Quy))
        {
            ReportModels.LayQuyHienTai();
        }
        
        //Danh sách loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "TenHT");
        
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        
        dtDonVi.Dispose();
        
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 0 });

        String LoaiBaoCao = Convert.ToString(ViewData["LoaiTongHop"]);
        if (String.IsNullOrEmpty(LoaiBaoCao)) LoaiBaoCao = "ChiTiet";
        String[] arrDonVi = iID_MaDonVi.Split(',');
        String[] arrView = new String[arrDonVi.Length];
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
        
        // Xuat Bao Cao
        if (PageLoad == "1")
        {
            if (LoaiBaoCao == "ChiTiet")
            {
                for (int i = 0; i < arrDonVi.Length; i++)
                {
                    arrView[i] =
                        String.Format(
                            @"/rptQuyetToan_TongHop_Nam_Quy/viewpdf?iID_MaDonVi={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}&LoaiBaoCao={5}&iID_MaPhongBan={6}&iID_TuyChon={7}",
                            arrDonVi[i], sLNS, iThang_Quy, iID_MaNamNganSach, MaND, LoaiBaoCao, iID_MaPhongBan, iID_TuyChon);
                    Chuoi += arrView[i];
                    if (i < arrDonVi.Length - 1)
                        Chuoi += "+";
                }
            }
            else
            {
                arrView = new string[1];
                arrView[0] =
                       String.Format(
                           @"/rptQuyetToan_TongHop_Nam_Quy/viewpdf?iID_MaDonVi={0}&sLNS={1}&iThang_Quy={2}&iID_MaNamNganSach={3}&MaND={4}&LoaiBaoCao={5}&iID_MaPhongBan={6}&iID_TuyChon={7}",
                           iID_MaDonVi, sLNS, iThang_Quy, iID_MaNamNganSach, MaND, LoaiBaoCao, iID_MaPhongBan, iID_TuyChon);
                Chuoi += arrView[0];

            }
        } 

        int SoCot = 1;
        String[] arrMaDonVi = iID_MaDonVi.Split(',');
        
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_TongHop_Nam_Quy", new { ParentID = ParentID}))
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
                                <b>Chọn Quý :</b></div>
                        </td>
                        <td style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iThang_Quy, "iThang_Quy", "", "class=\"input1_2\" style=\"width:100%;\"onchange=Chon()")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <b>Đơn vị: &nbsp;&nbsp; </b>
                        </td>
                        <td style="width: 25%" rowspan="25">
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
                                      for (int i = 0; i < dtDonVi.Rows.Count; i = i + SoCot)
                                      {
                                %>
                                <tr>
                                    <%for (int c = 0; c < SoCot; c++)
                                      {
                                          if (i + c < dtDonVi.Rows.Count)
                                          {
                                              strChecked = "";
                                              strsTen = Convert.ToString(dtDonVi.Rows[i + c]["TenHT"]);
                                              MaDonVi = Convert.ToString(dtDonVi.Rows[i + c]["iID_MaDonVi"]);
                                              for (int j = 0; j < arrMaDonVi.Length; j++)
                                              {
                                                  if (MaDonVi.Equals(arrMaDonVi[j]))
                                                  {
                                                      strChecked = "checked=\"checked\"";
                                                      break;
                                                  }
                                              }
                                    %>
                                    <td align="center" style="width: 40px;">
                                        <input type="checkbox" value="<%=MaDonVi %>" <%=strChecked %> check-group="DonVi"
                                            onclick="Chon()" id="iID_MaDonVi" name="iID_MaDonVi" />
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
                            <b>Loại ngân sách :</b>
                        </td>
                        <td rowspan="25" style="width: 30%;">
                            <div id="<%= ParentID %>_tdDonVi" style="overflow: scroll; height: 400px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div>
                                <b>Chọn năm :</b></div>
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
                                <b>Chọn phòng ban :</b></div>
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
                        <td class="td_form2_td1" style="width: 10%; height: 20px">
                            <div style="color: red; font-size: medium">
                                <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiBaoCao, "LoaiTongHop", "", "onchange=\" Chon()\"")%>
                                Chi tiết đơn vị
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 15%; height: 20px">
                            <div style="color: red; font-size: medium">
                                <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiBaoCao, "LoaiTongHop", "", "")%>
                                Báo cáo tổng hợp
                            </div>
                        </td>;
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align:right; color:Red; font-size: medium">
                           Đến <%=MyHtmlHelper.DropDownList(ParentID, slTuyChon, iID_TuyChon, "iID_TuyChon", "", "class=\"input1_2\" style=\"width:60%;\"")%>
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
            function CheckAll(value) {
                $("input:checkbox[check-group='LNS']").each(function (i) {
                    this.checked = value;
                });
            }                                            
        </script>
        <script type="text/javascript">
            function CheckAllDV(value) {
                $("input:checkbox[check-group='DonVi']").each(function (i) {
                    this.checked = value;
                });
                Chon();
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
                  var iID_MaDonVi = "";
             $("input:checkbox[check-group='DonVi']").each(function (i) {
                 if (this.checked) {
                     if (iID_MaDonVi != "") iID_MaDonVi += ",";
                     iID_MaDonVi += this.value;
                 }
             });
                Thang = document.getElementById("<%= ParentID %>_iThang_Quy").value;
                 var iID_MaNamNganSach = document.getElementById("<%=ParentID %>_iID_MaNamNganSach").value;
                 var iID_MaPhongBan = document.getElementById("<%=ParentID %>_iID_MaPhongBan").value;

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("LayDanhSachLNS?ParentID=#0&Thang_Quy=#1&iID_MaDonVi=#2&sLNS=#3&iID_MaNamNganSach=#4&iID_MaPhongBan=#5", "rptQuyetToan_TongHop_Nam_Quy") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", Thang));
                url = unescape(url.replace("#2",iID_MaDonVi));
                url = unescape(url.replace("#3","<%= sLNS %>"));
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
