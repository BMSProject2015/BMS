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
    String sMaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
    int iID_MaPhongBanDonVi = Convert.ToInt32(ViewData["iID_MaPhongBanDonVi"]);
    String ParentID = "Edit";
    String MaPhongBan = "", MaDonVi = "", bPublic = "";
    
    MaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
    MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    MaDonVi = PhongBan_DonViModels.getPhongBanDonVi(MaPhongBan);
    bPublic = Convert.ToString(ViewData["bPublic"]);
    DataTable dtPhongBan = DanhMucModels.getPhongBanByCombobox(true, "--- Chọn phòng ban ---");
   DataTable dtDonVi = DanhMucModels.getDonViByCombobox();
    //DataTable dtDonVi = PhongBan_DonViModels.DS_PhongBan_DonVi(Convert.ToString(MaPhongBan));
   if (String.IsNullOrEmpty(MaDonVi))
   {
       MaDonVi = "0000";
   }
   String[] arrDonVi = MaDonVi.Split(',');
    SelectOptionList optDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    SelectOptionList optPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
  
    


    //DataTable tbl = PhongBan_DonViModels.getDetail(iID_MaPhongBanDonVi);
    //if (tbl.Rows.Count > 0)
    //{
    //    MaPhongBan = tbl.Rows[0]["iID_MaPhongBan"].ToString();
    //    MaDonVi = tbl.Rows[0]["iID_MaDonVi"].ToString();
    //    bPublic = tbl.Rows[0]["bPublic"].ToString();
    //}
    //if (tbl != null) tbl.Dispose();
    String path = "~/Views/DungChung/PhongBanDonVi/PhongBanDonVi_DanhSach.ascx";
    using (Html.BeginForm("EditSubmit", "PhongBanDonVi", new { ParentID = ParentID, iID_MaPhongBanDonVi = iID_MaPhongBanDonVi }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBanDonVi", iID_MaPhongBanDonVi)%>
  
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |<%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhongBanDonVi"), "Phòng ban - Đơn vị")%>
                </div>
            </td>
               <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
                	<span>Nhập thông tin Phòng ban đơn vị</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin Phòng ban đơn vị</span>
                    <% } %>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing="0" border="0" width="100%">
                
                <tr>
                    <td class="td_form2_td1"><div><b>Phòng ban</b> &nbsp;<span  style="color:Red;">*</span></div></td>
                    <td>
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optPhongBan, MaPhongBan, "iID_MaPhongBan", null, "style=\"width: 50%;\" onchange=\"Chon_DonVi(this.value)\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_MaPhongBan")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" style="width: 10%;"><div> <b>Đơn vị &nbsp;</b><span  style="color:Red;">*</span></div></td>
                    <%--<td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optDonVi, MaDonVi, "iID_MaDonVi", null, "style=\"width: 50%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_MaDonVi")%>
                        </div>
                    </td>--%>
                    <td valign="top" align="left" style="width: 100%%;">
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
            Tên đơn vị
        </th>
       
    </tr>
</table>
                                </div>                                 
                                    <div id="divDonVi" style="width: 100%; max-height:400px; min-height:0px; overflow: scroll;">
                                        <%Html.RenderPartial(path, new { ControlID = ParentID, dtDonVi = dtDonVi,  sMaDonVi = MaDonVi }); %>
                                                
                                    </div>
                                </td>
                               
                            </tr>
                            <tr>
                             <td >
                                <div> <%= Html.ValidationMessage(ParentID + "_" + "err_MaDonVi")%></div>
                                </td>
                            </tr>
                        </table>
                    </td>    
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
                <td colspan="4" style="height: 30px;"></td>
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
            </table>
        </div>
    </div>
</div>
<%
    }
%>
 <script type="text/javascript">
     function ChonDV(DV) {
         $("input:checkbox[check-group='iID_MaDonVi']").each(function (i) {
             this.checked = DV;
         });
     }

    </script>

      <script type="text/javascript">

          function Chon_DonVi(MaPhongBan) {
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("EditNew?Code=#1", "PhongBanDonVi")%>');
              url = unescape(url.replace("#1", MaPhongBan));
              location.href = url;

          }        
        
    </script>     
    
</asp:Content>
