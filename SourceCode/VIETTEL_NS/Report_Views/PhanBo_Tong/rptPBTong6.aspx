<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo_Tong" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript" language="javascript" src="../../../Scripts/jquery-latest.js"></script>
 <style type="text/css">
     div.login1 {
            text-align : center;    
        }    
        div.login1 a {
            color: #545998;
            text-decoration: none;
            font: bold 12px "Museo 700";
            display: block;
            width: 250px; height: 20px;
            line-height: 20px;
            margin: 0px auto;
            -webkit-border-radius:2px;
            border-radius:2px;
        }
        div.login1 a:hover
        {
            text-decoration:underline;
            color:#471083;
        }    
        div.login1 a.active {
            background-position:  20px 1px;
        }
        div.login1 a:active, a:focus {
            outline: none;
        }
 </style>
</head>
<body>
      <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            iID_MaDotPhanBo = Guid.Empty.ToString();
        }
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = Convert.ToString(ViewData["sLNS"]);
        }

        String MaND = User.Identity.Name;
        String LuyKe = Convert.ToString(ViewData["LuyKe"]);
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
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
        String[] arrLNS = sLNS.Split(',');
        DataTable dtLNS = rptPBTong6Controller.NS_LoaiNganSach();
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLNS, "sLNS", "sLNS");
        if (String.IsNullOrEmpty(sLNS))
        {
            if (dtLNS.Rows.Count > 0)
            {
                sLNS = dtLNS.Rows[0]["sLNS"].ToString();
            }
            else
            {
                sLNS = Guid.Empty.ToString();
            }
        }
        dtLNS.Dispose();
        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
     
       String ToDaXem = Convert.ToString(ViewData["ToDaXem"]);


       DataTable dtDotPhanBo = PhanBo_ReportModels.LayDSDotPhanBoTong(MaND,iID_MaTrangThaiDuyet, sLNS, TruongTien);

        SelectOptionList slDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");

        if (String.IsNullOrEmpty(iID_MaDotPhanBo))
        {
            if (dtDotPhanBo.Rows.Count > 0)
            {
                iID_MaDotPhanBo = Convert.ToString(dtDotPhanBo.Rows[0]["iID_MaDotPhanBo"]);
            }
            else
            {
                iID_MaDotPhanBo = Guid.Empty.ToString();
            }
        }
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        String[] arrDSDuocNhapTruongTien = { "rTuChi", "rHienVat" };
        if (String.IsNullOrEmpty(TruongTien))
            TruongTien = arrDSDuocNhapTruongTien[0];

        
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptPBTong6", new { MaND = MaND, sLNS = sLNS, iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay, LuyKe = LuyKe, ToSo = ToSo });
        String BackURL = Url.Action("Index", "PhanBoTong_Report");
        String urlExport = Url.Action("ExportToExcel", "rptPBTong6", new { MaND = MaND, sLNS = sLNS, iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay, LuyKe = LuyKe, ToSo = ToSo });
        using (Html.BeginForm("EditSubmit", "rptPBTong6", new { ParentID = ParentID, ToDaXem = ToDaXem }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo tổng hợp chỉ tiêu ngân sách quốc phòng</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
           <div id="Div1" style="background-color:#F0F9FE;">
            <div id="rptMain">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 194px">
  <tr>
    <td width="151" rowspan="14" align="right"><b>Loại ngân sách : </b></td>
    <td width="500" rowspan="14"><div style="width: 100%; height: 400px; overflow: scroll; border:1px solid black;margin-top:4px;">
                            <table class="mGrid">
                                <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)" onchange="Chon()"></td>
                                <td> Chọn tất cả LNS </td>
                                </tr>                                                                                 
                                    <%
                                    String TenLNS = ""; String sLNS1 = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtLNS.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenLNS = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                                        sLNS1 = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                                        for (int j = 0; j < arrLNS.Length; j++)
                                        {
                                            if (sLNS1 == arrLNS[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 10%;">
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="Checkbox1" onchange="Chon()"
                                                name="sLNS" />                                        </td>
                                        <td>
                                            <%=TenLNS%>                                        </td>
                                    </tr>
                                  <%}%>
                                </table> 
                            </div> <br />Các tờ đã xem : (<%=ToDaXem %>)</td>
    <td width="40">&nbsp;</td>
    <td width="102" align="right"><b>Trạng thái : </b></td>
    <td width="314"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 80%;heigh:22px;\"onchange=Chon()")%></td>
    <td width="44">&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="right"><b>Trường tiền : </b></td>
    <td> &nbsp;&nbsp; &nbsp;&nbsp;Tự chi:
                                &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=Chon()")
                                 %>
                                &nbsp;&nbsp; &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp;  Hiện vật &nbsp;&nbsp;
                                   &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=Chon()")
                                 %></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="right"><b>Đợt phân bổ :</b> </td>
    <td><div id="<%=ParentID %>_divDotPhanBo" style="height:23px;">
                                    <% rptPBTong6Controller rpt = new rptPBTong6Controller();
                                       rptPBTong6Controller.LNSdata _Data = new rptPBTong6Controller.LNSdata();
                               _Data = rpt.obj_DotPhanBo(ParentID,MaND,sLNS,iID_MaDotPhanBo,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,LuyKe,ToSo);
                            %>
                            <%=_Data.iID_MaDotPhanBo%>
                                     &nbsp;&nbsp;
                                     </div></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="right"><strong>Khổ giấy :</strong></td>
    <td> &nbsp;&nbsp; &nbsp;&nbsp;A3 - Giấy to:
                                &nbsp;&nbsp; <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "onchange=Chon()")
                                 %>
                                &nbsp;&nbsp; &nbsp;&nbsp;  &nbsp;&nbsp; &nbsp;&nbsp;  A4 - Giấy nhỏ &nbsp;&nbsp;
                                   &nbsp;&nbsp;<%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "", "onchange=Chon()")
                                 %></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td align="right"><strong>Chọn tờ :</strong></td>
    <td id="<%= ParentID %>_tdToSo"><%=_Data.ToSo %></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td></b></td>
    <td><b>Lũy kế đến đợt được chọn :<%=MyHtmlHelper.CheckBox(ParentID, LuyKe, "LuyKe", "", "onclick=\"Chon()\"")%></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-left: 100px;margin-top:10px;">
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
       </table></td>
  </tr>
</table>

            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $("div#rptMain").hide();
                $('div.login1 a').click(function () {
                    $('div#rptMain').slideToggle('fast');
                    $(this).toggleClass('active');
                    return false;
                });
            });
    </script>
        <script type="text/javascript">
            function Chon() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;

                var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
                var TruongTien = "";
                if (bTruongTien) TruongTien = "rTuChi";
                else TruongTien = "rHienVat";

                var bKhoGiay = document.getElementById("<%= ParentID %>_KhoGiay").checked;
                var KhoGiay = "";
                if (bKhoGiay) KhoGiay = "1";
                else KhoGiay = "2";

                var Toso = document.getElementById("<%=ParentID%>_ToSo").value;

                var bLuyKe = document.getElementById("<%=ParentID %>_LuyKe").checked;
                var LuyKe = "";
                if (bLuyKe) LuyKe = "on";

                var sLNS = "";
                $("input:checkbox[check-group='sLNS']").each(function (i) {
                    if (this.checked) {
                        if (sLNS != "") sLNS += ",";
                        sLNS += this.value;
                    }
                });
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DotPhanBo?ParentID=#0&MaND=#1&sLNS=#2&iID_MaDotPhanBo=#3&iID_MaTrangThaiDuyet=#4&TruongTien=#5&KhoGiay=#6&LuyKe=#7&ToSo=#8", "rptPBTong6") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%=MaND %>"));
                url = unescape(url.replace("#2", sLNS));
                url = unescape(url.replace("#3", iID_MaDotPhanBo));
                url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#5", TruongTien));
                url = unescape(url.replace("#6", KhoGiay));
                url = unescape(url.replace("#7", LuyKe));
                url = unescape(url.replace("#8", Toso));
                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data.iID_MaDotPhanBo;
                    document.getElementById("<%= ParentID %>_tdToSo").innerHTML = data.ToSo;
                });
            }                                            
     </script>
      <script type="text/javascript">
          function Chonall(sLNS) {
              $("input:checkbox[check-group='sLNS']").each(function (i) {
                  if (sLNS) {
                      this.checked = true;
                  }
                  else {
                      this.checked = false;
                  }

              });
              Chon();
          }                                            
      </script>  
    </div>
  <script type="text/javascript">
      function Huy() {
          window.location.href = '<%=BackURL%>';
      }
  </script>
    <%} %>
    <div>
     <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%
    %>
    <iframe src="<%=URLView%>" height="600px" width="100%">
    </iframe>
</body>
</html>
