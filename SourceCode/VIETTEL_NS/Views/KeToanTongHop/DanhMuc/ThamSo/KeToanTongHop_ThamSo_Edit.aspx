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
    String iID_MaThamSo = Convert.ToString(ViewData["iID_MaThamSo"]);
    String ParentID = "Edit";
    DataTable dt = KeToan_DanhMucThamSoModels.Get_ChiTiet(iID_MaThamSo);


    String sKyHieu = "", sNoiDung = "", sThamSo = "", sBaoCao_ControllerName = "";
    sKyHieu = Convert.ToString(ViewData["sKyHieu"]);
    sNoiDung = Convert.ToString(ViewData["sNoiDung"]);
    sThamSo = Convert.ToString(ViewData["sThamSo"]);  
    if (dt.Rows.Count > 0)
    {
        sKyHieu = HamChung.ConvertToString(dt.Rows[0]["sKyHieu"]);
        sNoiDung = HamChung.ConvertToString(dt.Rows[0]["sNoiDung"]);
        sThamSo = HamChung.ConvertToString(dt.Rows[0]["sThamSo"]);
        sBaoCao_ControllerName = HamChung.ConvertToString(dt.Rows[0]["sBaoCao_ControllerName"]);    
    }
    //Loai bao cao
    DataTable dtNhomDN = KeToan_DanhMucThamSoModels.DT_BaoCao(true, " -- Chọn báo cáo --");
    SelectOptionList optLoaiBaoCao = new SelectOptionList(dtNhomDN, "iID_MaBaoCao", "sTen");
    if (dtNhomDN != null) dtNhomDN.Dispose();
    using (Html.BeginForm("EditSubmit", "KeToanTongHop_ThamSo", new { ParentID = ParentID, iID_MaThamSo = iID_MaThamSo }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "iID_MaThamSo", iID_MaThamSo)%>

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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop_ThamSo"), "Danh sách tham số")%>
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
                       <span>Nhập thông tin tham số</span>
                        <% 
                            }
                           else
                           { %>
                       <span>Sửa thông tin tham số</span>
                        <% } %>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1"><div>Ký hiệu&nbsp;<span style="color: Red;">*</span></div></td>
                     <td class="td_form2_td5">
                        <div>
                           

                               <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                      <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", "style=\"width:100%;\" tab-index='-1'")%><br />
                             <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                        <% 
                            }
                           else
                           { %>
                      

                       <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", "style=\"width:100%;\" tab-index='-1' readonly=\"readonly\"")%>


                        <% } %>
                        </div>
                    </td>
                </tr>
               <tr>
                    <td class="td_form2_td1"><div>Nội dung&nbsp;<span style="color: Red;">*</span></div></td>
                     <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, sNoiDung, "sNoiDung", "", "style=\"width:100%;\"")%><br />
                             <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDung")%>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td class="td_form2_td1"><div>Tham số&nbsp;<span style="color: Red;">*</span></div></td>
                     <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextBox(ParentID, sThamSo, "sThamSo", "", "style=\"width:100%;\"")%><br />
                             <%= Html.ValidationMessage(ParentID + "_" + "err_sThamSo")%>
                        </div>
                    </td>
                </tr>
                    <tr>
                    <td class="td_form2_td1"><div>Phương án thuộc báo cáo&nbsp;<span style="color: Red;">*</span></div></td>
                     <td class="td_form2_td5">
                       <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, optLoaiBaoCao, sBaoCao_ControllerName, "sBaoCao_ControllerName", null, "style=\"width:100%;\"")%>
                           <br />  <%= Html.ValidationMessage(ParentID + "_" + "err_optLoaiBaoCao")%>
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
