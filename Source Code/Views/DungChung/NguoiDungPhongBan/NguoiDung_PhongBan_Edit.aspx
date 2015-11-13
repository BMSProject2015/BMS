<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
    String sMaNguoiDung = Convert.ToString(ViewData["sMaNguoiDung"]);
    int iID_MaNguoiDungPhongBan = Convert.ToInt32(ViewData["iID_MaNguoiDungPhongBan"]);
    String ParentID = "Edit";
    String MaNguoiDung = "", MaPhongBan = "", bPublic = "";
    MaNguoiDung = Convert.ToString(ViewData["sMaNguoiDung"]);
    MaPhongBan = Convert.ToString(ViewData["iID_MaPhongBan"]);
    bPublic = Convert.ToString(ViewData["bPublic"]);
    MaPhongBan = NguoiDung_DonViModels.getPhongBanByNguoiDung(sMaNguoiDung);
    DataTable dtPhongBan = DanhMucModels.getPhongBanByCombobox();
    if (String.IsNullOrEmpty(MaPhongBan))
    {
        MaPhongBan = "0000";
    }
    String[] arrPhongBan = MaPhongBan.Split(',');
    DataTable dtNguoiDung = DanhMucModels.getNguoiDung(true, "--- Chọn nhóm người dùng ---");

    SelectOptionList optNguoiDung = new SelectOptionList(dtNguoiDung, "sID_MaNguoiDung", "sHoTen");
    SelectOptionList optDonvi = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");

    DataTable tbl = NguoiDung_PhongBanModels.getDetail(iID_MaNguoiDungPhongBan);
    if (tbl.Rows.Count > 0)
    {
        MaNguoiDung = tbl.Rows[0]["sMaNguoiDung"].ToString();
        MaPhongBan = tbl.Rows[0]["iID_MaPhongBan"].ToString();
        bPublic = tbl.Rows[0]["bPublic"].ToString();
    }
    if (tbl != null) tbl.Dispose();
    String path = "~/Views/DungChung/PhongBan/PhongBan_DanhSach.ascx";
    using (Html.BeginForm("EditSubmit", "NguoiDungPhongBan", new { ParentID = ParentID, sMaNguoiDung = sMaNguoiDung }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaNguoiDungPhongBan", iID_MaNguoiDungPhongBan)%>

    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 12%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |<%=MyHtmlHelper.ActionLink(Url.Action("Index", "NguoiDungPhongBan"), "Người dùng - Phòng ban")%>
                </div>
            </td>
            
        </tr>
    </table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	<span>Nhập thông tin Người dùng theo phòng ban</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin Người dùng theo phòng ban</span>
                    <% } %>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1"><div style="font-weight: bold;">Người dùng &nbsp;<span  style="color:Red;">*</span></div></td>
                    <td>
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optNguoiDung, MaNguoiDung, "sMaNguoiDung", null, "style=\"width: 50%;\" onchange=\"Chon_DonVi(this.value)\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaNguoiDung")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="width: 20%; font-weight: bold;"><div>Phòng ban/BQL &nbsp;<span  style="color:Red;">*</span></div></td>
                    <%--<td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optDonvi, MaDonVi, "iID_MaPhongBan", null, "style=\"width: 50%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhongBan")%>
                        </div>
                    </td>--%>
                    <td valign="top" align="left" style="width: 100%;" >
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>                                    
                                <td style =" width :100%;">    
                                               <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>                                    
                                <td style =" width :98%;">    
                                
                                       <div>
                                                        <table class="mGrid">
    <tr>
        <th align="center" style="width: 40px;">
            <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
        </th>
      
        <th>
         Phòng ban/BQL
        </th>
       
    </tr>
</table>
                                </div>   
                                                 
                                    <div id="divDonVi" style="width: 100%; height: 400px; overflow: scroll;">
                                        <%Html.RenderPartial(path, new { ControlID = ParentID, dtDonVi = dtPhongBan, MaND = MaNguoiDung, sMaPhongBan = MaPhongBan }); %>
                                                  
                                    </div>
                                </td>
                                
                            </tr>
                            <tr>
                            <td >                                      
                                    <div >
                                        
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhongBan")%>       
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>      
                </tr>
                 <tr>
                <td colspan="4" style="height: 30px;"></td>
                </tr> 
                  <tr style="display: none;">
                    <td class="td_form2_td1"><div>Hoạt động</div></td>
                    <td class="td_form2_td5" align="left">
                        <div>
                        <% if (ViewData["DuLieuMoi"] == "1") { %>
                        <%  bPublic = "True";
                           } %>
                            <%=MyHtmlHelper.CheckBox(ParentID, bPublic, "bPublic", "")%>
                        </div>
                    </td>
                </tr>

                 <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                 <tr>
            	    <td>
            	        <input type="submit" class="button" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
                            </table>
                        </td>
                    </tr>
            </table></table>
    
    </div>   </div>
</div>
<%
    }
%>
<script type="text/javascript">

    function Chon_DonVi(sMaNguoiDung) {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("EditDetail?sMaNguoiDung=#1", "NguoiDungPhongBan")%>');
        url = unescape(url.replace("#1", sMaNguoiDung));
        location.href = url;

    }        
        
    </script>    
</asp:Content>
