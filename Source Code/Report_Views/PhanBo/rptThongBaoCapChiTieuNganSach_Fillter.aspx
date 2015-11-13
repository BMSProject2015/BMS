<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
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
     .style1
     {
         width: 9px;
     }
     .style2
     {
         width: 4px;
     }
     .style3
     {
         width: 289px;
     }
     .style4
     {
         width: 292px;
     }
     .style5
     {
         width: 93px;
     }
 </style>
</head>
<body>
    
     <%
        
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";
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
        //don vi
        String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        DataTable dtDotPhanBo = rptThongBaoCapChiTieuNganSachController.LayDotPhanBo1(MaND,iID_MaTrangThaiDuyet);
        SelectOptionList slTenDotPhanBo = new SelectOptionList(dtDotPhanBo, "iID_MaDotPhanBo", "dNgayDotPhanBo");
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
        dtDotPhanBo.Dispose();
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        if (String.IsNullOrEmpty(iID_MaDonVi))
        {
            iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        }
        DataTable dtNguonDonVi = rptThongBaoCapChiTieuNganSachController.DSDonVi(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet);
        SelectOptionList slTenDonVi = new SelectOptionList(dtNguonDonVi, "iID_MaDonVi", "sTen");
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
        dtNguonDonVi.Dispose();
        String[] arrDonVi = iID_MaDonVi.Split(',');
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
        String BackURL = Url.Action("Index", "PhanBo_Report");
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String URLView = "";
        if (PageLoad == "1")
            URLView = Url.Action("ViewPDF", "rptThongBaoCapChiTieuNganSach", new { iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaDonVi = iID_MaDonVi, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,ThongBao=ThongBao,Muc=Muc,KieuTrang=KieuTrang });
        using (Html.BeginForm("EditSubmit", "rptThongBaoCapChiTieuNganSach", new { ParentID = ParentID}))
        {%>
    
    <div class="box_tong" style="background-color:#F0F9FE;">
     <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo THông báo cấp chỉ tiêu ngân sách</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
           <div id="Div1">
            <div id="rptMain" style="padding-bottom:5px;padding-top:10px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 131px">
  <tr>
    <td align="right" class="style5" valign="top"><b>Trạng thái : </b></td>
    <td class="style4" valign="top"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 95%;heigh:22px;\"onchange=ChonDonViPB()")%></td>
    <td rowspan="3" id="<%= ParentID %>_DonVi" class="style3" valign="top"><%rptThongBaoCapChiTieuNganSachController rptTB1 = new rptThongBaoCapChiTieuNganSachController();%>                                    
                                <% rptThongBaoCapChiTieuNganSachController.ThongBaoChiNganSach _data= new rptThongBaoCapChiTieuNganSachController.ThongBaoChiNganSach();
                                   _data = rptTB1.obj_DSDonVi(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet, iID_MaDonVi);%>
                                <%=_data.iID_MaDonVi%></td>
    <td width="131" rowspan="3" valign="top" style="margin:0; padding:0;"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Chọn thông báo") %></b></legend>
                           <%=MyHtmlHelper.Option(ParentID, "1", ThongBao, "ThongBao", "")%>&nbsp;&nbsp;Cấp<br />
                           <%=MyHtmlHelper.Option(ParentID, "2", ThongBao, "ThongBao", "")%>&nbsp;&nbsp;Thu
                           </fieldset></td>
    <td class="style1">&nbsp;</td>
    <td width="129" rowspan="3" valign="top"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Đến mục") %></b></legend>
                           <%=MyHtmlHelper.Option(ParentID, "1", Muc, "Muc", "")%>&nbsp;&nbsp;Ngành<br />
                           <%=MyHtmlHelper.Option(ParentID, "2", Muc, "Muc", "")%>&nbsp;&nbsp;Tiểu Ngành
                           </fieldset></td>
    <td class="style2">&nbsp;</td>
    <td width="146" rowspan="3" valign="top"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:130px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Kiểu trang") %></b></legend>
                          <%=MyHtmlHelper.Option(ParentID, "1", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Dọc<br />
                            <%=MyHtmlHelper.Option(ParentID, "2", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Ngang
                           </fieldset></td>
    <td width="20" rowspan="3" valign="top">&nbsp;</td>
  </tr>
  <tr>
    <td align="right" class="style5"><b>Đợt phân bổ : </b></td>
    <td id="<%= ParentID %>_iID_MaDonPhanBo" class="style4"><%=_data.iID_MaDonPhanBo %></td>
    <td class="style1">&nbsp;</td>
    <td class="style2">&nbsp;</td>
  </tr>
  <tr>
    <td height="26" class="style5">&nbsp;</td>
    <td class="style4">&nbsp;</td>
    <td class="style1">&nbsp;</td>
    <td class="style2">&nbsp;</td>
  </tr>
  <tr>
    <td colspan="8" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin: 10px;">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
    <td align="center">&nbsp;</td>
  </tr>
</table>
</div>
</div>
</div>
 <script type="text/javascript">
     $(function () {
         $("div#rptMain").show();
         $('div.login1 a').click(function () {
             $('div#rptMain').slideToggle('fast');
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
       function Chonall(sLNS) {
           $("input:checkbox[check-group='MaDonVi']").each(function (i) {
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
            function ChonDonViPB() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&iID_MaDotPhanBo=#2&iID_MaTrangThaiDuyet=#3&iID_MaDonVi=#4","rptThongBaoCapChiTieuNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%=MaND %>"));
                url = unescape(url.replace("#2", iID_MaDotPhanBo));
                url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#4", "<%=iID_MaDonVi %>"));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_iID_MaDonPhanBo").innerHTML = data.iID_MaDonPhanBo;
                    document.getElementById("<%= ParentID %>_DonVi").innerHTML = data.iID_MaDonVi;

                });

            } 
            function ChonDonVi() {
                var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
                var iID_MaDotPhanBo = document.getElementById("<%=ParentID %>_iID_MaDotPhanBo").value;
                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DonVi?ParentID=#0&MaND=#1&iID_MaDotPhanBo=#2&iID_MaTrangThaiDuyet=#3&iID_MaDonVi=#4","rptThongBaoCapChiTieuNganSach") %>');
                url = unescape(url.replace("#0", "<%= ParentID %>"));
                url = unescape(url.replace("#1", "<%=MaND %>"));
                url = unescape(url.replace("#2", iID_MaDotPhanBo));
                url = unescape(url.replace("#3", iID_MaTrangThaiDuyet));
                url = unescape(url.replace("#4", "<%=iID_MaDonVi %>"));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_DonVi").innerHTML =data.iID_MaDonVi;

                });

            }                                            
        </script>
    <%}%>
     <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
