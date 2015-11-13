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
    int iID_MaNguoiDungDonVi = Convert.ToInt32(ViewData["iID_MaNguoiDungDonVi"]);
    String ParentID = "Edit";  
  
    String MaNguoiDung = "", MaDonVi = "", bPublic = "";
    MaNguoiDung = Convert.ToString(ViewData["sMaNguoiDung"]);
    MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    bPublic = Convert.ToString(ViewData["bPublic"]);

    MaDonVi = NguoiDung_DonViModels.getDonViByNguoiDung(sMaNguoiDung);
  
 

    //đon vị/ người dùng
    //DataTable dtDonVi = DanhMucModels.getDonViByCombobox(true, "--- Chọn nhóm đơn vị ---");
    DataTable dtDonVi = NguoiDung_DonViModels.DS_NguoiDung_DonVi(Convert.ToString(sMaNguoiDung));
    if (String.IsNullOrEmpty(MaDonVi))
    {
        MaDonVi = "0000";
    }
    String[] arrDonVi = MaDonVi.Split(',');
    DataTable dtNguoiDung = DanhMucModels.getNguoiDung(true, "--- Chọn nhóm người dùng ---");

    SelectOptionList optNguoiDung = new SelectOptionList(dtNguoiDung, "sID_MaNguoiDung", "sHoTen");
    SelectOptionList optDonvi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    String path = "~/Views/DungChung/DonVi/DonVi_DanhSach.ascx";
    using (Html.BeginForm("EditSubmit", "NguoiDungDonVi", new { ParentID = ParentID, sMaNguoiDung = sMaNguoiDung }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaNguoiDungDonVi", iID_MaNguoiDungDonVi)%>

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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |<%=MyHtmlHelper.ActionLink(Url.Action("Index", "NguoiDungDonVi"), "Phòng ban - Đơn vị")%>
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
                	<span>Nhập thông tin Người dùng theo đơn vị</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin Người dùng theo đơn vị</span>
                    <% } %>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="100%">
                <tr>
                    <td class="td_form2_td1"><div style="font-weight: bold;">Người dùng &nbsp;<span  style="color:Red;">*</span></div></td>
                    <td >
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optNguoiDung, MaNguoiDung, "sMaNguoiDung", null, "style=\"width: 50%;\" onchange=\"Chon_DonVi(this.value)\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaNguoiDung")%>

                        </div>
                    </td>
                </tr>
                <tr style=" width:5px;"> </tr>
                <tr>
                   <td class="td_form2_td1" style =" width :10%;"><div style="font-weight: bold;">Đơn vị &nbsp;<span  style="color:Red;">*</span></div></td>
                    <td valign="top" align="left" style="width: 90%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>                                    
                                <td style =" width :100%;">      
                                
                                 <div>
                                                        <table class="mGrid">
   <%-- <tr>
        <th align="center" style="width: 40px;">
            <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
        </th>
      
        <th>
        Đơn vị
        </th>
       
    </tr>--%>
</table>
                                </div>                                   
                                    <div id="divDonVi" style="width: 100%;  max-height:450px; min-height:0px; overflow: scroll;">
                                        <%Html.RenderPartial(path, new { ControlID = ParentID, dtDonVi = dtDonVi, MaND = MaNguoiDung, sMaDonVi = MaDonVi }); %>
                                                   
                                    </div>
                                </td>
                               
                            </tr>
                            <tr>
                             <td style =" width :100%;">
                                <div> <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%></div>
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
         
          function Chon_DonVi(maNguoiDung) {
              jQuery.ajaxSetup({ cache: false });
              var url = unescape('<%= Url.Action("EditNew?Code=#1", "NguoiDungDonVi")%>');
              url = unescape(url.replace("#1", maNguoiDung));
              location.href = url;
             
          }        
        
    </script>     
    
</asp:Content>
