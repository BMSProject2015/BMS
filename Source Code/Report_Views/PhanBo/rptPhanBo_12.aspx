<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
         .style2
     {
         width: 146px;
     }
    
 </style>
</head>
<body>
    <%  String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";
        String MaND = User.Identity.Name;
       
        //dt Trạng thái duyệt
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
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
       
        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String KhoGiay = Convert.ToString(ViewData["KhoGiay"]);
        if (String.IsNullOrEmpty(KhoGiay))
        {
            KhoGiay = "1";
        }
        //Loai ngan sach
        String sLNS =Convert.ToString(ViewData["sLNS"]);
        String iID_MaPhongBan = NguoiDung_PhongBanModels.getMaPhongBan_NguoiDung(MaND);
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_PhongBan(iID_MaPhongBan);
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
        String[] arrLNS = sLNS.Split(',');

        String ToSo = Convert.ToString(ViewData["ToSo"]);
        if (String.IsNullOrEmpty(ToSo))
        {
            PageLoad = "0";
            ToSo = "1";
        }
       
        String ToDaXem = Convert.ToString(ViewData["ToDaXem"]);
        //ToDaXem += ToSo + ",";
        //if (ToDaXem == ",") ToDaXem = "";
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtNguonDonVi = rptPhanBo_12Controller.DanhSachDonVi(MaND, sLNS, iID_MaTrangThaiDuyet, TruongTien);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            if (dtNguonDonVi.Rows.Count > 0)
            {
                iID_MaDonVi = Convert.ToString(dtNguonDonVi.Rows[0]["iID_MaDonVi"]);
            }
            else
            {
                iID_MaDonVi = Guid.Empty.ToString();
            }
        }
        SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");

       
        String UrlReport = "";
        if (PageLoad == "1")
            UrlReport = Url.Action("ViewPDF", "rptPhanBo_12", new { MaND=MaND,sLNS=sLNS,iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay,ToSo=ToSo });
        
        String BackURL = Url.Action("Index", "PhanBo_Report");
        String urlExport = Url.Action("ExportToExcel", "rptPhanBo_12", new { MaND = MaND, sLNS = sLNS, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KhoGiay = KhoGiay, ToSo = ToSo });
        using (Html.BeginForm("EditSubmit", "rptPhanBo_12", new { ParentID = ParentID,ToDaXem=ToDaXem}))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Tổng hợp chỉ tiêu ngân sách Quốc Phòng</span>
                    </td>
                    <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
<div id="Div1" style="background-color:#F0F9FE;">
   <div id="rptMain" style="background-color:#F0F9FE;padding-top:5px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 125px">
  <tr>
    <td width="500" rowspan="25" align="right" valign="top"><fieldset style="text-align:left;padding:3px 6px;font-size:11px;width:500px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Chọn Loại ngân sách ") %></b></legend>
                            <div style="width: 100%; height: 500px; overflow: auto; border: 1px solid #006666;vertical-align:top;">
                                <table class="mGrid">
                                 &nbsp;<input type="checkbox" id="Checkbox1" onchange="ChonNLV()" onclick="ChonLNS(this.checked)" /> &nbsp;&nbsp;&nbsp;&nbsp; <b> Chọn tất cả</b>
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
                                            <input type="checkbox" value="<%=sLNS1 %>" <%=_Checked %> check-group="sLNS" id="sLNS" onchange="ChonNLV()"
                                                name="sLNS" />
                                        </td>
                                        <td>
                                            <%=TenLNS%>
                                        </td>
                                    </tr>
                                    <%}%>
                                </table>
                            </div>
                           </fieldset></td>
    <td align="right" class="style2"><b>Trạng thái duyệt : </b></td>
    <td width="142"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 90%;heigh:22px;\"onchange=ChonNLV()")%></td>
    <td width="184" rowspan="24" valign="top" align="center"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In cho loại") %></b></legend>
                            <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=ChonNLV()")%>Tự Chi<br />
                            <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=ChonNLV()")%>Hiện Vật
                           </fieldset><br />Các tờ đã xem :(<%=ToDaXem %>)</td>
    <td width="170" rowspan="24" valign="top" align="center"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("In trên giấy") %></b></legend>
                            <%=MyHtmlHelper.Option(ParentID, "1", KhoGiay, "KhoGiay", "", "onchange=ChonNLV()")%>A3 - Giấy to<br />
                            <%=MyHtmlHelper.Option(ParentID, "2", KhoGiay, "KhoGiay", "", "onchange=ChonNLV()")%>A4 - Giấy nhỏ
                           </fieldset></td>
  </tr>
  <tr>
    <td align="right" class="style2"><b>Đơn vị : </b></td>
    <td><div id="<%=ParentID %>_divDonVi" style="width:90%;height:23px;">
                             <% rptPhanBo_12Controller rpt = new rptPhanBo_12Controller();
                                       rptPhanBo_12Controller.LNSdata _Data = new rptPhanBo_12Controller.LNSdata();
                                       _Data = rpt.obj_DonVi(ParentID,MaND,sLNS,iID_MaDonVi,iID_MaTrangThaiDuyet,TruongTien,KhoGiay,ToSo);
                            %>
                            <%=_Data.iID_MaDonVi%>
                                     </div></td>
  </tr>
  <tr>
    <td align="right" class="style2"><strong>Chọn tờ :</strong></td>
    <td id="<%=ParentID %>_divToSo"><%=_Data.ToSo %></td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style2">&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td colspan="4"> <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin-left: 100px;">
                                <tr>
                                    <td><input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" /></td>
                                    <td width="5px"></td>
                                    <td><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" /></td>
                                </tr>
                            </table></td>
  </tr>
</table>

            </div>
        </div>
        <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
    <script type="text/javascript">
        function ChonLNS(sLNS) {
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (sLNS) {
                    this.checked = true;
                }
                else {
                    this.checked = false;
                }
            });

        }                                            
    </script>
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
        function ChonNLV() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var iID_MaDonVi = document.getElementById("<%=ParentID %>_iID_MaDonVi").value;

            var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
            var TruongTien = "";
            if (bTruongTien) TruongTien = "rTuChi";
            else TruongTien = "rHienVat";

            var bKhoGiay = document.getElementById("<%= ParentID %>_KhoGiay").checked;
            var KhoGiay = "";
            if (bKhoGiay) KhoGiay = "1";
            else KhoGiay = "2";

            var Toso = document.getElementById("<%=ParentID%>_ToSo").value;

            var sLNS = "";
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                if (this.checked) {
                    if (sLNS != "") sLNS += ",";
                    sLNS += this.value;
                }
            });
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&sLNS=#2&iID_MaDonVi=#3&iID_MaTrangThaiDuyet=#4&TruongTien=#5&KhoGiay=#6&ToSo=#7", "rptPhanBo_12") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%=MaND %>"));
            url = unescape(url.replace("#2", sLNS));
            url = unescape(url.replace("#3", iID_MaDonVi));
            url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#5", TruongTien));
            url = unescape(url.replace("#6", KhoGiay));
            url = unescape(url.replace("#7", Toso));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data.iID_MaDonVi;
                document.getElementById("<%= ParentID %>_divToSo").innerHTML = data.ToSo;
            });
        }                                            
     </script>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL%>';
         }
  </script>
    <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
