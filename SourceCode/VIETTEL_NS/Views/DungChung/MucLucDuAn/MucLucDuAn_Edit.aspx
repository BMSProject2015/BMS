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
    String ID = Convert.ToString(ViewData["iID_MaDanhMucDuAn"]);
    String ParentID = "Edit";

    DataTable dtThamQuyen = DanhMucModels.DT_DanhMuc("ThamQuyenCongTrinhDuAn", true, "--- Chọn thẩm quyền công trình dự án ---");
    DataTable dtLoaiCT = DanhMucModels.DT_DanhMuc("LoaiCongTrinhDuAn", true, "--- Chọn loại công trình dự án ---");
    DataTable dtTinhChatCT = DanhMucModels.DT_DanhMuc("TinhChatCongTrinhDuAn", true, "--- Chọn tính chất công trình dự án ---");
    DataTable dt = MucLucDuAnModels.getDetail(ID);

    SelectOptionList optThamQuyen = new SelectOptionList(dtThamQuyen, "iID_MaDanhMuc", "sTen");
    dtThamQuyen.Dispose();
    SelectOptionList optLoaiCT = new SelectOptionList(dtLoaiCT, "iID_MaDanhMuc", "sTen");
    dtLoaiCT.Dispose();
    SelectOptionList optTinhChatCT = new SelectOptionList(dtTinhChatCT, "iID_MaDanhMuc", "sTen");
    dtTinhChatCT.Dispose();
    DataTable dtDonVi=DonViModels.Get_dtDonVi();
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    String sTen = "", sMoTa = "", sMaCongTrinh = "", MaThamQuyen = "", MaLoaiCT = "", MaTinhChatCT = "", bHoanThanh = "", iID_MaDonVi = "";   
    if (dt.Rows.Count > 0)
    {
        iID_MaDonVi = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);
        sMaCongTrinh = Convert.ToString(dt.Rows[0]["sMaCongTrinh"]);
        sTen = Convert.ToString(dt.Rows[0]["sTen"]);
        sMoTa = Convert.ToString(dt.Rows[0]["sMoTa"]);
        MaThamQuyen = Convert.ToString(dt.Rows[0]["iID_MaThamQuyen"]);
        MaLoaiCT = Convert.ToString(dt.Rows[0]["iID_LoaiDuAn"]);
        MaTinhChatCT = Convert.ToString(dt.Rows[0]["iID_TinhChatDuAn"]);
        bHoanThanh = Convert.ToString(dt.Rows[0]["bHoanThanh"]);    
    }
    dt.Dispose();
       
    String strReadOnlyMa = "";
    String strIcon = "";
    if (ViewData["DuLieuMoi"] == "0") {
        strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
        strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
    }
    String urlIndex = Url.Action("Index", "MucLucDuAn");
    using (Html.BeginForm("EditSubmit", "MucLucDuAn", new { ParentID = ParentID, ID = ID }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaDanhMucDuAn", ID)%>
<table><tr><td align="right"><%=MyHtmlHelper.ActionLink(urlIndex,"   >>Danh sách dự án") %></td></tr></table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
               
            	<td align="left">
                <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	<span>Nhập thông tin mục lục dự án</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin mục lục dự án</span>
                    <% } %>
                </td>
                
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing=""="0" border="0" width="70%">
                  <tr>
                    <td class="td_form2_td1"><div>Loại dự án&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optLoaiCT, MaLoaiCT, "iID_LoaiDuAn", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_LoaiDuAn")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Thẩm quyền dự án&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optThamQuyen, MaThamQuyen, "iID_MaThamQuyen", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaThamQuyen")%>
                        </div>
                    </td>
                </tr>
              
                <tr>
                    <td class="td_form2_td1"><div>Tính chất dự án&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optTinhChatCT, MaTinhChatCT, "iID_TinhChatDuAn", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_TinhChatDuAn")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Đơn vị</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                        </div>
                    </td>
                </tr>
              <%--  <tr>
                    <td class="td_form2_td1"><div>Mã dự án&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sMaCongTrinh, "sMaCongTrinh", "", "style=\"width:20%;\" " + strReadOnlyMa + "")%><%=strIcon %>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sMaCongTrinh")%>
                        </div>
                    </td>
                </tr>--%>
                <tr>
                    <td class="td_form2_td1"><div>Tên dự án&nbsp;<span  style="color:Red;">*</span></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:80%;\"")%><br />
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div>Mô tả</div></td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "style=\"width:80%;resize:none;\"")%>
                        </div>
                    </td>
                </tr>
             <tr>
                    <td class="td_form2_td1"><div>Hoàn thành</div></td>
                    <td class="td_form2_td5" align="left">
                        <div>
                            <%=MyHtmlHelper.CheckBox(ParentID, bHoanThanh, "bHoanThanh", "")%>
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
