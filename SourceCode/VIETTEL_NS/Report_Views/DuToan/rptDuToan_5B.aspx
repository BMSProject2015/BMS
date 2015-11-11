<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.DuToan" %>
<%@ Import Namespace="FlexCel.Core" %>
<%@ Import Namespace="FlexCel.Render" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
     String ParentID = "BaoCaoNganSachNam";
    String NamLamViec = Request.QueryString["NamLamViec"];
   
    if (String.IsNullOrEmpty(NamLamViec))
    {
        NamLamViec = DateTime.Now.Year.ToString();
    }
  
    DateTime dNgayHienTai = DateTime.Now;
    String MaND = User.Identity.Name;
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
    String Nganh = Request.QueryString["Nganh"];
    DataTable dtNganh = Connection.GetDataTable("SELECT iID_MaDanhMuc,sTenKhoa, DC_DanhMuc.sTen FROM DC_LoaiDanhMuc INNER JOIN DC_DanhMuc ON DC_DanhMuc.iID_MaLoaiDanhMuc = DC_LoaiDanhMuc.iID_MaLoaiDanhMuc WHERE DC_DanhMuc.bHoatDong=1 AND DC_LoaiDanhMuc.sTenBang=N'Nganh' ORDER BY sTenKhoa");
    if (String.IsNullOrEmpty(Nganh))
    {
        if(dtNganh.Rows.Count>0)
        {
            Nganh=dtNganh.Rows[0]["sTenKhoa"].ToString();
        }
        else
        {
            Nganh = "000";
        }
    }
        SelectOptionList slNganh = new SelectOptionList(dtNganh, "sTenKhoa", "sTen");
    dtNganh.Dispose();
     String sLNS = Request.QueryString["sLNS"];
    DataTable dtLNS = DanhMucModels.NS_LoaiNganSachNhaNuoc();
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
       
    String ToSo = Request.QueryString["ToSo"];
    if (String.IsNullOrEmpty(ToSo))
    {
        ToSo = "1";
    }
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtToSo = rptDuToan_5bController.DanhSachToIn(NamLamViec, Nganh, ToSo, sLNS, MaND, iID_MaTrangThaiDuyet);
        SelectOptionList slTo = new SelectOptionList(dtToSo, "MaTo", "TenTo");
        DataTable dtTrangThai = ReportModels.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeDuToan);
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
      
        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
        {
            iID_MaTrangThaiDuyet = "0";
        }
            
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="0"});
    using (Html.BeginForm("EditSubmit", "rptDuToan_5b", new { ParentID = ParentID }))
    {
    %>
     <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách nhà nước 5b</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td  style="width: 10%;"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng thái: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                             </div>   
                       </td>
                    <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Loại ngân sách: ")%></div>
                        </td>
                        <td  style="width: 15%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slLNS, sLNS, "sLNS", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                               
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 7%;">
                            <div><%=NgonNgu.LayXau("Ngành: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNganh, Nganh, "Nganh", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                               
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 7%;">
                            <div><%=NgonNgu.LayXau("Tờ số: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div id="<%=ParentID %>_divDonVi">
                            <%rptDuToan_5bController rptTB1 = new rptDuToan_5bController();%>                                   
                                     <%=rptTB1.obj_DSDonVi(ParentID, NamLamViec, Nganh, ToSo, sLNS, MaND, iID_MaTrangThaiDuyet)%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>                       
                        <td>
                        </td>                        
                        <td colspan="8"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                            <tr>
                                    <td style="width: 49%;" align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                                    </td>
                                    <td width="2%">                                    </td>
                                    <td style="width: 49%;" align="left">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />                                    </td>
                                </tr>
                           </table></td> 
<td>
                        </td>
                    </tr>
                    
                 </table>
            </div>
        </div>
         <script type="text/javascript">
             function Huy() {
                 window.location.href = '<%=BackURL%>';
             }
    </script>
    </div>
    <%}
        dtNam.Dispose();
         %>
    <div>    
      <script type="text/javascript">
          function Chon() {
              var NamLamViec = document.getElementById("<%=ParentID %>_iNamLamViec").value;
              var Nganh = document.getElementById("<%=ParentID %>_Nganh").value;
              var sLNS = document.getElementById("<%=ParentID %>_sLNS").value;
             
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&NamLamViec=#1&Nganh=#2&ToSo=#3&sLNS=#4", "rptDuToan_5b") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", NamLamViec));
              url = unescape(url.replace("#2", Nganh));
              url = unescape(url.replace("#3", "<%= ToSo %>"));
              url = unescape(url.replace("#4", sLNS));
              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;

              });
          }                                            
      </script>
       <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToan_5b", new { NamLamViec = NamLamViec, Nganh = Nganh, ToSo = ToSo, sLNS = sLNS, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra Excels")%>
    <iframe src="<%=Url.Action("ViewPDF","rptDuToan_5b",new{NamLamViec=NamLamViec,Nganh=Nganh,ToSo=ToSo,sLNS=sLNS,iID_MaTrangThaiDuyet=iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
