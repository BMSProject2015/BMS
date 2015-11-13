<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 300px;
        }
        .style2
        {
            width: 200px;
        }
        .style3
        {
            width: 237px;
        }
        .style4
        {
            width: 96px;
        }
    </style>
    <script type="text/javascript" src="../../../Scripts/jquery-latest.js"></script>
</head>
<body>
      <%
        String ParentID = "QuyetToanNganSach";
        String MaND = User.Identity.Name;
        String TruongTien = "";

       
        TruongTien = Convert.ToString(ViewData["TruongTien"]);
        if (String.IsNullOrEmpty(TruongTien))
        {
            TruongTien = "rTuChi";
        }
          String Thang = "0", Quy = "0";
          String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = "1";
        }
        String LoaiThangQuy = Convert.ToString(ViewData["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = "0";
        }
        if (LoaiThangQuy == "0")
        {
            Thang = Thang_Quy;
            Quy = "0";
        }
        else
        {
            Thang = "0";
            Quy = Thang_Quy;
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
        dtNam.Dispose();
        DataTable dtThang = DanhMucModels.DT_Thang();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtQuy = DanhMucModels.DT_Quy();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        dtQuy.Dispose();
          //dt Trạng thái duyệt
        String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
        DataTable dtTrangThai = rptQuyetToan_ThongTri_5_NganhController.tbTrangThai();
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
          //dt Loại ngân sách
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        DataTable dtLNS = rptQuyetToan_ThongTri_5Controller.DSLoaiNganSach(MaND);
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
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
        String LoaiIn = Convert.ToString(ViewData["LoaiIn"]);
        if (String.IsNullOrEmpty(LoaiIn))
        {
            LoaiIn = "ChiTiet";
        }
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
        DataTable dtDonVi = rptQuyetToan_ThongTri_5Controller.LayDSDonVi(LoaiThangQuy, Thang_Quy, sLNS, iID_MaTrangThaiDuyet,MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        //if (String.IsNullOrEmpty(iID_MaDonVi))
        //{
        //    if (dtDonVi.Rows.Count > 0)
        //    {
        //        iID_MaDonVi = dtDonVi.Rows[0]["sTen"].ToString();
        //    }
        //    else
        //    {
        //        iID_MaDonVi = Guid.Empty.ToString();
        //    }
        //}
        dtDonVi.Dispose();
        String[] arrMaDonVi;
        String MaDonVi = "-111";//= arrMaDonVi[ChiSo];
        int ChiSo = 0;
            if (String.IsNullOrEmpty(iID_MaDonVi) == false)
        {
            arrMaDonVi = iID_MaDonVi.Split(',');

            ChiSo = Convert.ToInt16(Request.QueryString["ChiSo"]);
            if(ChiSo==arrMaDonVi.Length)
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
                MaDonVi = arrMaDonVi[0];
            }
        }
        else
        {
            iID_MaDonVi = "-111";
        }
            if (LoaiIn == "TongHop")
            {
                ChiSo = 0;
                MaDonVi = iID_MaDonVi;
            }
        String BackURL = Url.Action("Index", "QuyetToan_Report", new { Loai = 1 });
        String PageLoad = Convert.ToString(ViewData["PageLoad"]);
        String urlReport = "";

        if (PageLoad.Equals("1"))
        {
            urlReport = Url.Action("ViewPDF", "rptQuyetToan_ThongTri_5", new { LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sMaDonVi = iID_MaDonVi, LoaiIn = LoaiIn,ChiSo=ChiSo });
        }
        String urlExport = Url.Action("ExportToExcel", "rptQuyetToan_ThongTri_5", new { LoaiThangQuy = LoaiThangQuy, Thang_Quy = Thang_Quy, sLNS = sLNS, iID_MaDonVi = MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,sMaDonVi=iID_MaDonVi,LoaiIn=LoaiIn });
        using (Html.BeginForm("EditSubmit", "rptQuyetToan_ThongTri_5", new { ParentID = ParentID,ChiSo=ChiSo }))
        {
    %>
   
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Quyết toán thông tri</span>
                    </td>
                     <td width="52%" style="text-align: left;">
                        <div class="login1" style="width: 50px; height: 20px; text-align: left;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="rptMain" style="background-color:#F0F9FE;">
            <div id="Div2" style="margin-left:10px;">
<table width="100%" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td rowspan="4" width="131px" align="right"><b>Loại Ngân Sách : </b> </td>
    <td class="style1" rowspan="4"><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\"style=\"width: 100%\" size='10' tabindex='-1' onchange=Chon()")%><br /></td>
    <td align="right" class="style4"><b>Tháng / Quý :</b> </td>
    <td class="style2"><%=MyHtmlHelper.Option(ParentID, "0", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:27%;\"onchange=Chon() ")%>
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThangQuy, "LoaiThangQuy", "", "onchange=Chon()")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:27%;\"onchange=Chon()")%><br /></td>
    
         
    <td rowspan="4" width="80px" align="right"><b>Đơn vị :</b> </td>
    <td rowspan="4">
        <div id="<%= ParentID %>_tdDonVi" style="height:150px; overflow:scroll;" >
            
        </div>
    
    </td>
  </tr>
  <tr>
        <td class="style4" align="right"><b>Trạng Thái : </b></td>
        <td class="style2" ><div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=Chon()")%>  <br /></div></td>
  </tr>
  <tr>
    <td align="right" class="style4"><b>Tổng hợp dữ liệu theo: </b></td>
    <td class="style2" > &nbsp;
        <%=MyHtmlHelper.Option(ParentID, "ChiTiet", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Chi tiết&nbsp;&nbsp;
        <%=MyHtmlHelper.Option(ParentID, "TongHop", LoaiIn, "LoaiIn", "")%>&nbsp;&nbsp;Tổng hợp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </td>

  </tr>
  <tr>
    <td align="right" class="style4"><b>Trường tiền</b></td>
    <td class="style2" > &nbsp;
    <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=Chon()")%>&nbsp;&nbsp;Tự Chi&nbsp;&nbsp;
    <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=Chon()")%>&nbsp;&nbsp;Hiện Vật&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
  </td>

  </tr>
  <tr>
    <td colspan="6" align="center">  <table cellpadding="0" cellspacing="0" border="0" align="center">
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
          });
          Chon();
          function Chon() {
              
              var LoaiThang_QuyCheck = document.getElementById("<%= ParentID %>_LoaiThangQuy").checked;
              var LoaiThangQuy
              var Thang
              if (LoaiThang_QuyCheck == true) {
                  Thang = document.getElementById("<%= ParentID %>_iThang").value;
                  LoaiThangQuy = 0;
              }
              else {
                  Thang = document.getElementById("<%= ParentID %>_iQuy").value;
                  LoaiThangQuy = 1;
              }
              var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID%>_iID_MaTrangThaiDuyet").value;
              var chkTruongTien = document.getElementById("<%=ParentID %>_TruongTien").checked;
              var TruongTien = "";
              if (chkTruongTien)
                  TruongTien = "rTuChi";
              else
                  TruongTien = "rHienVat";
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Ds_DonVi?ParentID=#0&LoaiThangQuy=#1&Thang_Quy=#2&sLNS=#3&iID_MaDonVi=#4&iID_MaTrangThaiDuyet=#5&TruongTien=#6", "rptQuyetToan_ThongTri_5_Nganh") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", LoaiThangQuy));
              url = unescape(url.replace("#2", Thang));              
              url = unescape(url.replace("#3", sLNS));
              url = unescape(url.replace("#4", "<%= iID_MaDonVi %>"));
              url = unescape(url.replace("#5", iID_MaTrangThaiDuyet));
              url = unescape(url.replace("#6", TruongTien));
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
         <div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    </div>
    <%} %>
        <iframe src="<%=urlReport%>" height="600px" width="100%">
        </iframe>
</body>
</html>
