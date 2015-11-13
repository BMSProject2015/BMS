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
    String MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    String MaPhanHe = Convert.ToString(Request.QueryString["MaPhanHe"]);
    String ParentID = "Edit";
    DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(true,"---Chọn phân hệ---");
    DataTable dtNhomNguoiDung = PhanHe_TrangThaiDuyetModel.DT_NguoiDung(true, "--- Chọn nhóm người dùng ---");
    
    DataTable dt = PhanHe_TrangThaiDuyetModel.GetRow_PhanHe_TrangThaiDuyet(MaTrangThaiDuyet);
    SelectOptionList optPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
    SelectOptionList optNhomNguoiDung = new SelectOptionList(dtNhomNguoiDung, "iID_MaNhomNguoiDung", "sTen");
    
    String sTen = "", MaNhomNguoiDung = "", LoaiTrangThaiDuyet = "";
    String sMauSac = "#ffffff";  
    // Binding
    if (dt.Rows.Count>0)
    {
        sTen = dt.Rows[0]["sTen"].ToString();       
        sMauSac = dt.Rows[0]["sMauSac"].ToString();
        MaPhanHe = dt.Rows[0]["iID_MaPhanHe"].ToString();
        MaNhomNguoiDung = dt.Rows[0]["iID_MaNhomNguoiDung"].ToString();
        LoaiTrangThaiDuyet = dt.Rows[0]["iLoaiTrangThaiDuyet"].ToString();
    }
    DataTable dtLoaiTrangThaiDuyet = PhanHe_TrangThaiDuyetModel.DT_LoaiTrangThaiDuyet(true, "---Chọn loại trạng thái duyệt---");
    SelectOptionList optLoaiTrangThaiDuyet = new SelectOptionList(dtLoaiTrangThaiDuyet, "iLoaiTrangThaiDuyet", "sTen");
    using (Html.BeginForm("EditSubmit", "PhanHe_TrangThaiDuyet", new { ParentID = ParentID, MaTrangThaiDuyet = MaTrangThaiDuyet }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "iID_MaTrangThaiDuyet", MaTrangThaiDuyet)%>
    <div class="box_tong">
        <div class="title_tong">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
        	<tr>
        		<td>
                <span>Nhập thông tin phân hệ trạng thái duyệt</span>
                </td>
        	</tr>
        </table>
        </div>
        <div id="nhapform">          
           <table border="0" cellspacing="0" cellpadding="0" width="70%">
                <tr>
                		<td class="td_form2_td1"><div> Phân hệ</div></td>
                        <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optPhanHe, MaPhanHe, "iID_MaPhanHe", null, "style=\"width: 49%;\"")%>                         
                              <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhanHe")%>
                        </div>
                        </td>
                </tr>  
                <tr>
                		<td class="td_form2_td1"><div> Nhóm người dùng sửa</div></td>
                        <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optNhomNguoiDung, MaNhomNguoiDung, "iID_MaNhomNguoiDung", null, "style=\"width: 49%;\"")%>
                              <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNhomNguoiDung")%>
                        </div>                     
                        </td>
                </tr>        	            
                <tr>
                    <td class="td_form2_td1"><div>Tên trạng thái duyệt</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:80%;\"")%><br />
                         <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>               
                        </div>                                            
                    </td>
                </tr>
                <tr>
                		<td class="td_form2_td1"><div>Loại trạng thái duyệt</div></td>
                        <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optLoaiTrangThaiDuyet, LoaiTrangThaiDuyet, "iLoaiTrangThaiDuyet", null, "style=\"width: 49%;\"")%>
                             <%= Html.ValidationMessage(ParentID + "_" + "err_iLoaiTrangThaiDuyet")%>  
                        </div>                     
                        </td>
                </tr>                                
                <tr>
                    <td class="td_form2_td1"><div>Màu sắc</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sMauSac, "sMauSac", "", "style=\"width:20%;\"")%><br />
                             <%= Html.ValidationMessage(ParentID + "_" + "err_sMauSac")%>               
                        </div>
                    </td>
                </tr>
            </table>
            </div>
       <br />
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
    </div>
    <%
    }
%>
</asp:Content>
