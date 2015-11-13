<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.CapPhat" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%
    String ParentID = "CapPhat";
    String iNamLamViec = Convert.ToString(ViewData["iNamLamViec"]);
    if (String.IsNullOrEmpty(iNamLamViec))
    {
        iNamLamViec = DateTime.Now.Year.ToString();
    }
    DateTime dNgayHienTai = DateTime.Now;
    String NamHienTai = Convert.ToString(dNgayHienTai.Year);
    DataTable dtNam = DanhMucModels.DT_Nam();
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");      
    dtThang.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    String LoaiThang_Quy = Convert.ToString(ViewData["LoaiThang_Quy"]);
    if (String.IsNullOrEmpty(LoaiThang_Quy))
    {
        LoaiThang_Quy = "0";
    }
    String Thang_Quy = Convert.ToString(ViewData["Thang_Quy"]);
    String Thang = "0";
    String Quy = "0";
    if (String.IsNullOrEmpty(Thang_Quy))
    {
        Thang_Quy = "1";
    }
    if (LoaiThang_Quy == "0")
    {
        Thang = Thang_Quy;
        Quy = "0";
    }
    else
    {
        Thang = "0";
        Quy = Thang_Quy;
    }

    //dt Trạng thái duyệt
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    DataTable dtTrangThai = rptCapPhat_TongHopChiTieuCapNganSach_81Controller.tbTrangThai();
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
    String URL = Url.Action("Index", "CapPhat_Report");
    String urlReport = "";
    String PageLoad = Convert.ToString(ViewData["PageLoad"]);
    if (PageLoad.Equals("1"))
    {
        urlReport = Url.Action("ViewPDF", "rptCapPhat_TongHopChiTieuCapNganSach_81", new { iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, Thang_Quy = Thang_Quy, LoaiThang_Quy = LoaiThang_Quy });
    } 
    using (Html.BeginForm("EditSubmit", "rptCapPhat_TongHopChiTieuCapNganSach_81", new { ParentID = ParentID }))
      { 
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo tổng hợp chỉ tiêu cấp (LNS)</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1">
        <div id="Div2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                <tr>
                   <td width="15%">&nbsp;</td>
						<td class="td_form2_td1" width="10%">
                            <div>
                                Trạng Thái :</div>
                        </td>
						  <td width="10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" style=\"width: 100%\" onchange=\"Chon()\"")%>
                            </div>
                        </td>
                    
						 <td class="td_form2_td1" width="10%">
                            <div>
                                Tháng/Quý:</div>
                        </td>
						 <td width="30%">
                          
                                <%=MyHtmlHelper.Option(ParentID, "0", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" ")%>Tháng
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, Thang, "iThang", "", "class=\"input1_2\" style=\"width:27%;\" ")%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp
                                <%=MyHtmlHelper.Option(ParentID, "1", LoaiThang_Quy, "LoaiThang_Quy", "", "style=\"width:10%;\" ")%>Quý
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, Quy, "iQuy", "", "class=\"input1_2\" style=\"width:27%;\" ")%>
                           
                        </td>
                        <td></td>
                </tr>
                <tr>
                        <td>&nbsp;</td>
    					             
                        <td colspan="4">
                            <table cellpadding="0" cellspacing="0" border="0" align="left" style="margin: 10px;" width="100%">
                                <tr>
                                    <td width="45%" align="right">
                                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                    </td>
                                    <td width="10px">
                                    </td>
                                    <td width="45%">
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                            </table>
                        </td>                     
                         <td></td>
                         </tr>

            </table>
        </div>
    </div>
</div>
<%} %>
   <script type="text/javascript">
       function Huy() {
           window.location.href = '<%=URL %>';
       }
    </script>
<div>
    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</div>
