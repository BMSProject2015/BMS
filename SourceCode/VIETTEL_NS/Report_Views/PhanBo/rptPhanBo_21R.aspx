<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.PhanBo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<%
   
    String ParentID = "PhanBo_21R";
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
    dtNam.Dispose();
    String TruongTien = Convert.ToString(ViewData["TruongTien"]);
    if(String.IsNullOrEmpty(TruongTien))
    {
        TruongTien = "rTuChi";
    }
    //dt Trạng thái duyệt
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
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
    DataTable dtDonVi = rptPhanBo_21RController.DanhSach_DonVi(MaND, iID_MaTrangThaiDuyet, TruongTien);
    SelectOptionList slTenDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if(Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"])!="-2")
        {
            iID_MaDonVi=Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        }
        else
        {
            iID_MaDonVi=Guid.Empty.ToString();
        }
    }
    dtDonVi.Dispose();
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    String URLView = "";
    String BackURL = Url.Action("Index", "PhanBo_Report");
    if (PageLoad == "1")
        URLView = Url.Action("ViewPDF", "rptPhanBo_21R", new { iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });
    using (Html.BeginForm("EditSubmit", "rptPhanBo_21R", new { ParentID = ParentID }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo chỉ tiêu duyệt QT</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
          <div id="Div2">
            <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td width="15%"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng Thái:")%></div>                        </td>
                        <td width="10%">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%;heigh:22px;\"onchange=ChonDonVi()")%> </div></td>                 
                         <td style="width: 10%;" class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn Đơn Vị:")%></div>                        </td>
                        <td width="15%"> <div id="<%=ParentID %>_divDonVi">
                                <%rptPhanBo_21RController rptTB1 = new rptPhanBo_21RController();%>
                                <%=rptTB1.obj_DSDonVi(ParentID, MaND, iID_MaTrangThaiDuyet, iID_MaDonVi, TruongTien)%>
                            </div> 
                        </td>
                        <td class="td_form2_td1" width="10%">
                        <div>
                               Trường Tiền:                           </div>                        </td>
                        <td width="13%">
                            <div>
                              <div>
                                 <%=MyHtmlHelper.Option(ParentID, "rTuChi", TruongTien, "TruongTien", "", "onchange=ChonDonVi()")%>Tự Chi&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 <%=MyHtmlHelper.Option(ParentID, "rHienVat", TruongTien, "TruongTien", "", "onchange=ChonDonVi()")%>Hiện Vật&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                            </div>
                            </div>                        </td>
                         <td></td>
                    </tr>
					 <tr>
                      <td></td>
                      <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="2%">
                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table></td>
                      <td></td>
                    </tr>
                 </table>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         function ChonDonVi() {
         var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value;
         var _TruongTien = document.getElementById("<%=ParentID %>_TruongTien").checked;
         var TruongTien;
         if (_TruongTien == true) {
             TruongTien = "rTuChi";
         }
         else
         {
         TruongTien="rHienVat";
         }            
             jQuery.ajaxSetup({ cache: false });
             var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&MaND=#1&iID_MaDonVi=#2&TruongTien=#3&iID_MaTrangThaiDuyet=#4", "rptPhanBo_21R") %>');
             url = unescape(url.replace("#0", "<%= ParentID %>"));
             url = unescape(url.replace("#1", "<%= MaND%>"));
             url = unescape(url.replace("#2", "<%= iID_MaDonVi %>"));
             url = unescape(url.replace("#3", TruongTien));
             url = unescape(url.replace("#4", iID_MaTrangThaiDuyet));       
             $.getJSON(url, function (data) {
                 document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
             });
         }                                            
     </script>
      <script type="text/javascript">
          function Huy() {
              window.location.href = '<%=BackURL%>';
          }
  </script>
    <%} %> 
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptPhanBo_21R", new { iID_MaDonVi = iID_MaDonVi, TruongTien = TruongTien, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra Excel")%>
   <iframe src="<%=URLView%>" height="600px" width="100%"></iframe>
</body>
</html>
