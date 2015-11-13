<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 176px;
        }
        .style2
        {
            width: 49%;
        }
    </style>
</head>
<body>
    <%
        String ParentID = "BaoCaoNganSachNam";
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
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBo2(MaND, iID_MaTrangThaiDuyet);
            if (dtDotPhanBo.Rows.Count > 1)
            {
                iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[1]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
            dtDotPhanBo.Dispose();
        }
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = PhanBo_ReportModels.DanhSachDonVi2(MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, false, false);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (!String.IsNullOrEmpty(iID_MaDotPhanBo))
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
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        String ThongBao = Convert.ToString(ViewData["ThongBao"]);
        if (String.IsNullOrEmpty(ThongBao))
        {
            ThongBao = "1";
        }
        String Muc = Convert.ToString(ViewData["Muc"]);
        if (String.IsNullOrEmpty(Muc))
        {
            Muc = "1";
        }
        String KieuTrang = Convert.ToString(ViewData["KieuTrang"]);
        if (String.IsNullOrEmpty(KieuTrang))
        {
            KieuTrang = "1";
        }
        int iFile = Convert.ToInt16(ViewData["iFile"]);
        String BackURL = Url.Action("Index", "PhanBo_report");
        dtDonVi.Dispose();
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
        if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if (ChiSo == arrMaDonVi.Length)
            {
                ChiSo = 0;
            }
            if (ChiSo <= arrMaDonVi.Length - 1)
            {
                MaDonVi = arrMaDonVi[ChiSo];
                ChiSo = ChiSo + 1;
            }
            else
            {
                ChiSo = 0;
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptThongBaoCapChiTieuNganSachNam", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = MaDonVi, iID_MaDotPhanBo = iID_MaDotPhanBo, ThongBao = ThongBao, iFiLe = iFile, KieuTrang = KieuTrang });
         if (PageLoad != "1") ChiSo = 0;
        using (Html.BeginForm("EditSubmit", "rptThongBaoCapChiTieuNganSachNam", new { ParentID = ParentID, ChiSo = ChiSo }))
        {
    %>
    <div class="box_tong" style="background-color: #F0F9FE;">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo thông báo ngân sách -Thông báo</span>
                    </td>
                    <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
       <div id="rptMain">
            <div id="Div2" style="margin-top: 5px;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 152px">
                    <tr>
                        <td width="50">
                            &nbsp;
                        </td>
                        <td width="112" align="right">
                            <b>Trạng thái : </b>
                        </td>
                        <td width="200">
                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"ChonNLV()\"")%>
                        </td>
                        <td width="27">
                            &nbsp;
                        </td>
                        <td rowspan="2" class="style1">
                            <fieldset style="text-align: left; padding: 5px 5px 8px 8px; font-size: 11px; width: 160px;
                                -moz-border-radius: 3px; -webkit-border-radius: 3px; -khtml-border-radius: 3px;
                                border-radius: 3px; border: 1px #C0C0C0 solid;">
                                <legend><b>
                                    <%=NgonNgu.LayXau("Chọn thông báo") %></b></legend>
                                <%=MyHtmlHelper.Option(ParentID, "1", ThongBao, "ThongBao", "")%>&nbsp;&nbsp;Cấp<br />
                                <%=MyHtmlHelper.Option(ParentID, "2", ThongBao, "ThongBao", "")%>&nbsp;&nbsp;Thu
                            </fieldset>
                        </td>
                        <td width="150" rowspan="2">
                            <fieldset style="text-align: left; padding: 5px 5px 8px 8px; font-size: 11px; width: 160px;
                                -moz-border-radius: 3px; -webkit-border-radius: 3px; -khtml-border-radius: 3px;
                                border-radius: 3px; border: 1px #C0C0C0 solid;">
                                <legend><b>
                                    <%=NgonNgu.LayXau("Kiểu trang") %></b></legend>
                                <%=MyHtmlHelper.Option(ParentID, "1", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Dọc<br />
                                <%=MyHtmlHelper.Option(ParentID, "2", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Ngang
                            </fieldset>
                        </td>
                        <td width="12">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            <b>Đợt phân bổ : </b>
                        </td>
                        <td id="<%=ParentID %>_divDotPhanBo">
                            <% rptThongBaoCapChiTieuNganSachNamController rpt = new rptThongBaoCapChiTieuNganSachNamController();
                               rptThongBaoCapChiTieuNganSachNamController.cl_DotPhanBo _Data = new rptThongBaoCapChiTieuNganSachNamController.cl_DotPhanBo();
                               _Data = rpt.obj_DSDotPhanBo(ParentID, MaND, iID_MaTrangThaiDuyet, iID_MaDotPhanBo, iID_MaDonVi);
                            %>
                            <%=_Data.iID_MaDotPhanBo%>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="vertical-align:top;text-align:right">
                            <b>Đơn vị :</b>
                        </td>
                        <td>
                            <div id="<%= ParentID %>_divDonVi" style="height: 400px; overflow: scroll;">
                               
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;"
                                width="100%">
                                <tr>
                                    <td align="right" class="style2">
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td align="right" class="style2" style="padding-right: 150px; vertical-align: bottom;">
                                        <%=MyHtmlHelper.ActionLink(Url.Action("DongNoiDung", "rptThongBaoCapChiTieuNganSachNam", new { sKyHieu = "rptPhanBo_4_1" }), "Dòng nội dung")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 49%; padding-right: 10px;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
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
    <script type="text/javascript">
        ChonNLV();
        function ChonNLV() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDotPhanBo=#2&iID_MaDonVi=#3&MaND=#4", "rptThongBaoCapChiTieuNganSachNam") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", iID_MaDotPhanBo));
            url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#4", "<%= MaND %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data.iID_MaDotPhanBo;
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data.iID_MaDonVi;
            });
        }                                            
    </script>
    <script type="text/javascript">
        function ChonDotPhanBo() {
            jQuery.ajaxSetup({ cache: false });
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
            var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&iID_MaTrangThaiDuyet=#1&iID_MaDotPhanBo=#2&iID_MaDonVi=#3&MaND=#4", "rptThongBaoCapChiTieuNganSachNam") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#2", iID_MaDotPhanBo));
            url = unescape(url.replace("#3", "<%= iID_MaDonVi %>"));
            url = unescape(url.replace("#4", "<%= MaND %>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data.iID_MaDonVi;
            });
        }                                            
    </script>
     <script type="text/javascript">
         $(function () {
             $("div#rptMain").hide();
             $('div.login1 a').click(function () {
                 $('div#rptMain').slideToggle('normal');
                 $(this).toggleClass('active');
                 return false;
             });
         });       
    </script>
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=BackURL%>';
        }
    </script>
      <script type="text/javascript">
          function CheckAll(value) {
              $("input:checkbox[check-group='DonVi']").each(function (i) {
                  this.checked = value;
              });
          }                                            
     </script>
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThongBaoCapChiTieuNganSachNam", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iID_MaDonVi = iID_MaDonVi, iID_MaDotPhanBo = iID_MaDotPhanBo,ThongBao=ThongBao,Muc=Muc,iFile=iFile,KieuTrang=KieuTrang }), "Xuất ra Excels")%>
    <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
