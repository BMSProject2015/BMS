<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "DanhMucThuBaoHiem";
    String DuLieuMoi=Convert.ToString(ViewData["DuLieuMoi"]);
    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
     SelectOptionList slThangCopy = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
    DataTable dtCauHinh=  NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    String iNamLamViec=Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang="0";
    String sThangCopy = Convert.ToString(ViewData["ThangCopy"]);
    DataTable dtMucLucDoiTuong = new DataTable();
    String[] arrTruongTienBaoHiem = new String[] { "rBHXH_CN","rBHYT_CN","rBHTN_CN","rBHXH_DV","rBHYT_DV","rBHTN_DV","rBHXH_CS","rLuongToiThieu" };
    if(DuLieuMoi=="1" && iThang == "0")
    {
        dtMucLucDoiTuong = NganSach_DoiTuongModels.DT_MucLucDoiTuong();
        for(int i=0;i<arrTruongTienBaoHiem.Length;i++)    
            {
                dtMucLucDoiTuong.Columns.Add(arrTruongTienBaoHiem[i], typeof(Decimal));
            }
    }
    else
    {
        dtMucLucDoiTuong = DanhMucThuBaoHiemModels.Get_DTDanhMucThuBaoHiem(iNamLamViec, iThang);
    }

    String sThang =Convert.ToString(ViewData["Thang"]);
    using (Html.BeginForm("EditSubmit", "BaoHiem_DanhMucThuBaoHiem", new { ParentID = ParentID }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", DuLieuMoi)%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-top: 5px; padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "MucLucDoiTuong"), "Danh sách đối tượng")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin mục lục đối tượng ngân sách</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%">
                     <tr>
                        <td class="td_form2_td1">
                            <div><b>Tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang", "", "class=\"input1_2\" style=\"width:98%;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iThang")%>
                            </div>
                        </td>
                    </tr>              
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Copy từ tháng</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slThangCopy,sThangCopy, "iThangCopy", "", "class=\"input1_2\" style=\"width:98%;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iThangCopy")%>
                            </div>
                        </td>
                    </tr>
            </table>
        </div>
    </div>
</div><br />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" class="button4" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>  

<%
 }    
%>
</asp:Content>
