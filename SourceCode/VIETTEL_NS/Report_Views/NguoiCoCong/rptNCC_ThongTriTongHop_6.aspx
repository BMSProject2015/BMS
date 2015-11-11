<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.NguoiCoCong" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 278px;
        }
        .style2
        {
            width: 113px;
        }
    </style>
</head>
<body>
    <%
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "NguoiCoCong";
        String NamLamViec = Request.QueryString["NamLamViec"];
        String UserID = User.Identity.Name;
        String Thang_Quy = "", LoaiThangQuy = "";
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String sLNS = Request.QueryString["sLNS"];
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = Guid.Empty.ToString();
        }

        Thang_Quy = Convert.ToString(Request.QueryString["Thang_Quy"]);

        if (String.IsNullOrEmpty(Thang_Quy))
        {
            Thang_Quy = Guid.Empty.ToString();
        }
        LoaiThangQuy = Convert.ToString(Request.QueryString["LoaiThangQuy"]);
        if (String.IsNullOrEmpty(LoaiThangQuy))
        {
            LoaiThangQuy = Guid.Empty.ToString();
        }
        if (String.IsNullOrEmpty(NamLamViec))
        {
            NamLamViec = DateTime.Now.Year.ToString();
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
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNguoiCoCong(false);
        SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "sLNS");
        dtLNS.Dispose();
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        String urlExport = Url.Action("ExportToExcel", "rptNCC_ThongTriTongHop_6", new { NamLamViec = NamLamViec, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi });
        using (Html.BeginForm("EditSubmit", "rptNCC_ThongTriTongHop_6", new { ParentID = ParentID, NamLamViec = NamLamViec, sLNS = sLNS, Thang_Quy = Thang_Quy, iID_MaDonVi = iID_MaDonVi }))
        {
    %>
    <%=MyHtmlHelper.Hidden(ParentID, sLNS, "sLNS", "")%>
    <%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
   <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Thông Tri Tổng Hợp quyết toán</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1"> 
<div style="background-color:#F0F9FE;margin:0 0 0 0;padding-top:8px;">
 <table width="1200" height="115" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td align="right" class="style2"><b><%=NgonNgu.LayXau("Chọn năm làm việc :")%></b></td>
    <td class="style1"><%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 90%;height:23px;\"")%></td>
    <td width="136" rowspan="4" align="center" valign="middle"><b>Loại ngân sách :</b></td>
    <td width="147" rowspan="4" valign="top"> <div style="width: 250px; height: 100px; overflow: scroll; border:#006666 1px solid;">
                    <table  class="mGrid">
                        <%
                            String strLNS = "";
                            String strMoTa = "";
                            for (int i = 0; i < dtLNS.Rows.Count; i++)
                            {
                                strLNS = Convert.ToString(dtLNS.Rows[i]["sLNS"]);
                                strMoTa = Convert.ToString(dtLNS.Rows[i]["TenHT"]);
                          %>
                        <tr>
                            <td style="width: 15%;">
                                <input type="radio" value="<%=strLNS %>" check-group="sLNS" id="Radio1" name="sLNS" />
                            </td>
                            <td>
                                <%=strMoTa%>
                            </td>
                        </tr>
                        <%}%>
                    </table>
                </div></td>
    <td width="92" rowspan="4" align="center"><input type="button" class="button" value="Thực hiện" onclick="ChonLNS();" /></td>
    <td width="134" rowspan="4" align="left" valign="top" id="<%= ParentID %>_tdDonVi" style="width: 190px;height:100px;"></td>
    <td width="84" rowspan="4" align="center" valign="middle"><input type="submit" class="button" value="Xem báo cáo" /></td>
  </tr>
  <tr>
    <td align="right" class="style2"><b>Tháng/Quý :</b></td>
    <td class="style1"><div>                               
                                <%=MyHtmlHelper.Option(ParentID, "0","0", "ThangQuy", "")%>Tháng&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, "", "iThang", "", "class=\"input1_2\" style=\"width:80px;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <%=MyHtmlHelper.Option(ParentID,"1", "","ThangQuy","")%>Quý&nbsp;&nbsp;
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, "", "iQuy", "", "class=\"input1_2\" style=\"width:80px;\"")%><br />
                            </div></td>
  </tr>
  <tr>
    <td class="style2">&nbsp;</td>
    <td class="style1">&nbsp;</td>
  </tr>
  <tr>
    <td class="style2">&nbsp;</td>
    <td class="style1">&nbsp;</td>
  </tr>
</table>
</div>

        <script type="text/javascript">
            function ChonLNS(LNS) {
                var xaunoi = "";
                $("input:radio[check-group='sLNS']").each(function (i) {
                    if (this.checked) {
                        if (xaunoi != "") xaunoi += ",";
                        xaunoi += this.value;
                    }
                });

                jQuery.ajaxSetup({ cache: false });
                var url = unescape('<%= Url.Action("ds_DonVi?sLNS=#0","rptNCC_ThongTriTongHop_6") %>');
                url = unescape(url.replace("#0", xaunoi));

                $.getJSON(url, function (data) {
                    document.getElementById("<%= ParentID %>_tdDonVi").innerHTML = data.DonVi;

                });

            }                                            
    </script>
     <div>
     </div>
     </div>
    <%=MyHtmlHelper.ActionLink(urlExport, "Export To Excel") %>
    </div>
    <%}%>
    <iframe src="<%=Url.Action("ViewPDF","rptNCC_ThongTriTongHop_6", new{NamLamViec=NamLamViec,LoaiThangQuy=LoaiThangQuy,sLNS=sLNS,Thang_Quy=Thang_Quy,iID_MaDonVi=iID_MaDonVi})%>" height="600px" width="100%">
    </iframe>
</body>
</html>
