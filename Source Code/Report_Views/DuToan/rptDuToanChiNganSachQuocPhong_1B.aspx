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
    String MaND = User.Identity.Name;
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    DataTable dtTrangThai = rptDuToanChiNganSachQuocPhongNamTuyVien_DonViController.tbTrangThai();
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
   
    String ToSo = Request.QueryString["ToSo"];
        String Nganh = Request.QueryString["Nganh"];
        if (String.IsNullOrEmpty(Nganh))
        {
            Nganh = "-1";
        }
    if (String.IsNullOrEmpty(ToSo))
    {
        ToSo = "1";
    }
    DataTable dtToSo = rptDuToanChiNganSachQuocPhong_1BController.DanhSachToIn(MaND, Nganh, ToSo, iID_MaTrangThaiDuyet);
        SelectOptionList slTo = new SelectOptionList(dtToSo, "MaTo", "TenTo");

        DataTable dtNganh = MucLucNganSach_NganhModels.LayDanhSachNganh();
        SelectOptionList slNganh = new SelectOptionList(dtNganh, "sTenKhoa", "TenHT");
    String BackURL = Url.Action("Index", "DuToan_Report", new { sLoai="1"});
    using (Html.BeginForm("EditSubmit", "rptDuToanChiNganSachQuocPhong_1B", new { ParentID = ParentID, MaND = MaND, Nganh = Nganh, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }))
    {
    %>   
    <div class="box_tong">
         <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	    <tr>
            	    <td>
                	    <span>Báo cáo dự toán chi ngân sách quốc phòng 1020500</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0"  cellspacing="0" border="0" class="table_form2">
                    <tr>
                    <td  style="width: 20%;"></td>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Trạng thái: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                             </div>   
                       </td>
                          <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Ngành: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNganh, Nganh, "Nganh", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                             </div>   
                       </td>
                         <td class="td_form2_td1" style="width: 10%;">
                            <div><%=NgonNgu.LayXau("Tờ số: ")%></div>
                        </td>
                        <td  style="width: 10%;">
                            <div id="<%=ParentID %>_divDonVi">
                            <%rptDuToanChiNganSachQuocPhong_1BController rptTB1 = new rptDuToanChiNganSachQuocPhong_1BController();%>                                   
                                     <%=rptTB1.obj_DSDonVi(ParentID,MaND,Nganh,ToSo,iID_MaTrangThaiDuyet)%>
                            </div>
                        </td>
                        <td></td>
                    </tr>
                    <tr>                       
                        <td>
                        </td>                        
                        <td colspan="6"><table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
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
              var iID_MaTrangThaiDuyet = document.getElementById("<%=ParentID %>_iID_MaTrangThaiDuyet").value
              var Nganh = document.getElementById("<%=ParentID %>_Nganh").value
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("Get_dsDonVi?ParentID=#0&MaND=#1&Nganh=#2&ToSo=#3&iID_MaTrangThaiDuyet=#4", "rptDuToanChiNganSachQuocPhong_1B") %>');
              url = unescape(url.replace("#0", "<%= ParentID %>"));
              url = unescape(url.replace("#1", "<%= MaND %>"));
              url = unescape(url.replace("#2", Nganh));
              url = unescape(url.replace("#3", "<%= ToSo %>"));
              url = unescape(url.replace("#4", iID_MaTrangThaiDuyet ));

              $.getJSON(url, function (data) {
                  document.getElementById("<%= ParentID %>_divDonVi").innerHTML = data;
                  
              });
          }                                            
      </script>
       <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptDuToanChiNganSachQuocPhong_1B", new { MaND = MaND, Nganh = Nganh, ToSo = ToSo, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Xuất ra Excels")%>
    <iframe src="<%=Url.Action("ViewPDF","rptDuToanChiNganSachQuocPhong_1B",new{MaND=MaND,Nganh=Nganh,ToSo=ToSo,iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet})%>" height="600px" width="100%"></iframe>
    </div>
</body>
</html>
