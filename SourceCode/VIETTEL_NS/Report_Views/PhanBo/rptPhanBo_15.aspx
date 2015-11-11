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
        
    
     .style3
     {
         width: 199px;
     }
        
    
 </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "TongHopChiTieu";

        String TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
        String MaND = User.Identity.Name;
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
         String KieuTrang = Convert.ToString(ViewData["KieuTrang"]);
            if (String.IsNullOrEmpty(KieuTrang))
            {
                KieuTrang = "1";
            }
        String iID_MaDotPhanBo = Convert.ToString(ViewData["iID_MaDotPhanBo"]);
        DataTable dtDotPhanBo = rptPhanBo_15Controller.DanhSach_DotPhanBo(MaND, iID_MaTrangThaiDuyet,TruongTien);
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
        dtDotPhanBo.Dispose();
        String[] arrDSDuocNhapTruongTien = { "rTuChi", "rHienVat" };
        if(String.IsNullOrEmpty(TruongTien))
            TruongTien = arrDSDuocNhapTruongTien[0];
         String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptPhanBo_15", new { iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien,KieuTrang=KieuTrang  });
        
        String BackURL = Url.Action("Index", "PhanBo_Report");
        String urlExport = Url.Action("ExportToExcel", "rptPhanBo_15", new { iID_MaDotPhanBo = iID_MaDotPhanBo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, TruongTien = TruongTien, KieuTrang = KieuTrang });
        using (Html.BeginForm("EditSubmit", "rptPhanBo_15", new { ParentID = ParentID}))
        {
    %>
    <div class="box_tong" style="background-color:#F0F9FE;">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Tổng hợp ngân sách</span>
                    </td>
                     <td width="52%" style=" text-align:left;">
                           <div class="login1" style=" width:50px; height:20px; text-align:left;"><a style="cursor:pointer;"></a></div>
                </td>
                </tr>
            </table>
        </div>
        <div id="Div1" style="margin:0 auto;">
            <div id="rptMain" style="margin-top:5px;">


<table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 97px">
  <tr>
    <td width="39" align="right">&nbsp;</td>
    <td width="198" align="right"><b>Trạng thái :</b> </td>
    <td width="277"><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 70%;heigh:22px;\"onchange=ChonNLV()")%> </td>
    <td width="198" rowspan="2" align="center"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Trường tiền") %></b></legend>
                           <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "","onchange=\"ChonNLV()\"")%>&nbsp;&nbsp;Tự chi<br />
                           <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "","onchange=\"ChonNLV()\"")%>&nbsp;&nbsp;Hiện vật
                           </fieldset></td>
    <td rowspan="2" align=left class="style3"><fieldset style="text-align:left;padding:5px 5px 8px 8px;font-size:11px;width:160px; -moz-border-radius: 3px;-webkit-border-radius: 3px;-khtml-border-radius: 3px;border-radius: 3px;border:1px #C0C0C0 solid;">
                            <legend><b><%=NgonNgu.LayXau("Kiểu trang") %></b></legend>
                          <%=MyHtmlHelper.Option(ParentID, "1", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Dọc<br />
                            <%=MyHtmlHelper.Option(ParentID, "2", KieuTrang, "KieuTrang", "")%>&nbsp;&nbsp;Ngang
                           </fieldset></td>
    <td width="29">&nbsp;</td>
  </tr>
  <tr>
    <td align="right">&nbsp;</td>
    <td align="right"><b>Đợt phân bổ :</b> </td>
    <td><div id="<%=ParentID %>_divDotPhanBo">
                                      <%rptPhanBo_15Controller rptTB1 = new rptPhanBo_15Controller();                          
                              
                            %>
                              <%=rptTB1.obj_DanhSachDotPhatBo(ParentID, MaND, iID_MaDotPhanBo, iID_MaTrangThaiDuyet,TruongTien)%>   
                                     </div></td>
    <td>&nbsp;</td>
  </tr>
  <tr align="right">
    <td colspan="6" align="center"><table cellpadding="0" cellspacing="0" border="0" align="center" style="margin-left: 10px;">
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
    </div>
    <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
     <script type="text/javascript">
         function Huy() {
             window.location.href = '<%=BackURL%>';
         }
  </script>
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
    <%--<script type="text/javascript">
        function ChonLNS(LNS) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsTruongTien?ParentID=#0&LNS=#1", "rptTongHopChiTieuDonVi") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", LNS));

            $.getJSON(url, function (data) {               
               document.getElementById("<%= ParentID %>_divTruongTien").innerHTML = data;
            });
           
        }                                            
    </script>--%>
    <script type="text/javascript">
        function ChonNLV() {
            var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
            var bTruongTien = document.getElementById("<%= ParentID %>_TruongTien").checked;
            var TruongTien = "";
            if (bTruongTien) TruongTien = "rTuChi";
            else TruongTien = "rHienVat";                  
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("Get_dsDotPhanBo?ParentID=#0&MaND=#1&iID_MaTrangThaiDuyet=#2&TruongTien=#3", "rptPhanBo_15") %>');
            url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#1", "<%=MaND %>"));
            url = unescape(url.replace("#2", iID_MaTrangThaiDuyet));
            url = unescape(url.replace("#3", TruongTien));

            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_divDotPhanBo").innerHTML = data;
            });
        }                                            
    </script>
    <%} %>
    <div>
    </div>
    <%
    %>
    <iframe src="<%=URLView%>" height="600px" width="100%">
    </iframe>
</body>
</html>
