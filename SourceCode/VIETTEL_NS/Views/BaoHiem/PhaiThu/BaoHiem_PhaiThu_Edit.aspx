<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String MaBaoHiemPhaiThu = Convert.ToString(ViewData["iID_MaBaoHiemPhaiThu"]);
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    
    DataTable dtChungTu = BaoHiem_PhaiThuModels.GetChungTu(MaBaoHiemPhaiThu);
    DataRow R;
    int iSoChungTu = 0, bLoaiThang_Quy = -1;
    String sLNS="", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "", sThang = "", sQuy = "", sNam = "", iID_MaDonVi = "", sTienToChungTu = "";
    if (Convert.ToString(ViewData["DuLieuMoi"])=="1")
        {
            sThang = "0";
        }
    if (dtChungTu!=null && dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        if (R["dNgayChungTu"] != DBNull.Value)
        {
            dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        }
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);
        sThang = Convert.ToString(R["iThang_Quy"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);        
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);           
      
        sLNS = Convert.ToString(R["sDSLNS"]);
        switch (bLoaiThang_Quy)
        {
            case 0:
                sThang = Convert.ToString(R["iThang_Quy"]);
                break;
            case 1:
                sQuy = Convert.ToString(R["iThang_Quy"]);
                break;
            case 2:
                sNam = Convert.ToString(R["iThang_Quy"]);
                break;
            default:
                
                break;
        }
        iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    if (ViewData["DuLieuMoi"] == "1")
    {
        sTienToChungTu = PhanHeModels.LayTienToChungTu(PhanHeModels.iID_MaPhanHeBaoHiem);
        iSoChungTu = BaoHiem_PhaiThuModels.GetMaxChungTu() + 1;
    }
    dtChungTu.Dispose();


   
    
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

   


    using (Html.BeginForm("EditSubmit", "BaoHiem_PhaiThu", new { ParentID = ParentID, MaBaoHiemPhaiThu = MaBaoHiemPhaiThu}))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<%= Html.Hidden(ParentID + "_iSoChungTu", iSoChungTu)%>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThu"), "Bảo hiểm")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 60%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">                
                    <tr>
                        <td class="td_form2_td1" style="width: 30%;">
                            <div><b>Số chứng từ</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <b><%=sTienToChungTu%><%=iSoChungTu%></b>
                            </div>
                        </td>
                    </tr>               
                      <tr>
                        <td class="td_form2_td1">
                            <div><b>Đơn vị</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Tháng/Quý</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>                                                         
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang_Quy", "", "class=\"input1_2\" style=\"width:17%;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                                
                                <%= Html.ValidationMessage(ParentID + "_" + "err_ThangQuy")%>
                            </div>
                        </td>
                    </tr>                   
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Nội dung</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px;\"")%></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
</asp:Content>



