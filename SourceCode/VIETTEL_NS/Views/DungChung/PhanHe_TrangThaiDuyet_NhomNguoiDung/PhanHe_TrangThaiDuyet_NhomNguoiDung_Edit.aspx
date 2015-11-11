<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PhanHe_TrangThaiDuyet_DuocXem_Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%
    String ParentID = "PhanHe_TrangThaiDuyet_NhomNguoiDung";
    SqlCommand cmd;
    String MaPhanHe = Request.QueryString["MaPhanHe"];
    DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(false, "---Chọn phân hệ---");
    if (String.IsNullOrEmpty(MaPhanHe)) MaPhanHe = Convert.ToString(dtPhanHe.Rows[0]["iID_MaPhanHe"]);    
    SelectOptionList slPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
     if (String.IsNullOrEmpty(MaPhanHe)) MaPhanHe = Convert.ToString(dtPhanHe.Rows[0]["iID_MaPhanHe"]);
    DataTable dtNhomNguoiDung = PhanHe_TrangThaiDuyetModel.DT_NguoiDung(false, "--- Chọn nhóm người dùng ---");
    SelectOptionList optNhomNguoiDung = new SelectOptionList(dtNhomNguoiDung, "iID_MaNhomNguoiDung", "sTen");
    String iID_MaNhomNguoiDung = Request.QueryString["MaNhomNguoiDung"];
     if (String.IsNullOrEmpty(iID_MaNhomNguoiDung)) iID_MaNhomNguoiDung = Convert.ToString(dtNhomNguoiDung.Rows[0]["iID_MaNhomNguoiDung"]); 
    DataTable dt = LuongCongViecModel.Get_dtDSTrangThaiDuyet(Convert.ToInt32(MaPhanHe));
    DataTable dtTrangThaiDuocXem=LuongCongViecModel.Get_dtDSTrangThaiDuyet_NhomNguoiDung_DuocXem(Convert.ToInt32(MaPhanHe),iID_MaNhomNguoiDung);
    using (Html.BeginForm("Loc", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { ParentID = ParentID }))
    {
 %>
 <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                 <tr>
                    <td class="td_form2_td1">
                        <div>
                            Phân hệ</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, slPhanHe, MaPhanHe, "MaPhanHe", null, "style=\"width:20%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhanHe")%>
                        </div>
                    </td>
                    <td>
                        Nhóm người dùng sửa
                    </td>
                    <td>
                    <%=MyHtmlHelper.DropDownList(ParentID, optNhomNguoiDung, iID_MaNhomNguoiDung, "MaNhomNguoiDung", null, "style=\"width:98%;\"")%>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" colspan="2">
                        <div style="float:right;">
                            <input id="Button2" type="submit" class="button" value="Lọc"/>
                        </div>
                    </td>
                </tr>
                </table>
                 <%} %>
                 <% using (Html.BeginForm("EditSubmit", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { ParentID = ParentID }))
                    { %>
                    <%=MyHtmlHelper.Hidden(ParentID,MaPhanHe,"iID_MaPhanHe","") %>
                    <%=MyHtmlHelper.Hidden(ParentID, iID_MaNhomNguoiDung, "iID_MaNhomNguoiDung", "")%>
                <table class="mGrid">
                    <tr>
                        <th style="width: 3%;" align="center">
                            STT
                        </th>
                        <th style="width: 15%;" align="center">
                            Tên trạng thái duyệt
                        </th>
                    </tr>
                    <%for (int i = 0; i < dt.Rows.Count; i++)
                      {
                          DataRow R = dt.Rows[i];
                          String _check = "";
                          String inputcheck = "<input type=\"checkbox\" {0} value={1} name={2} id={2}/>";
                          for (int j = 0; j < dtTrangThaiDuocXem.Rows.Count; j++)
                          {

                              if (Convert.ToString(R["iID_MaTrangThaiDuyet"]).Equals(Convert.ToString(dtTrangThaiDuocXem.Rows[j]["iID_MaTrangThaiDuyet"])))
                              {
                                  _check = "checked";
                                  break;
                              }                              
                          }
                          inputcheck = String.Format(inputcheck, _check, R["iID_MaTrangThaiDuyet"], ParentID + "_iID_MaTrangThaiDuyet");
                      %>
                      <tr>
                        <td>
                           <%=inputcheck%>
                        </td>
                        <td><%=R["sTen"]%></td>
                      </tr>
                    <%}%>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
	                    <td width="70%">&nbsp;</td>
	                    <td width="30%" align="right">
                            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	                    <tr>
            	                    <td>
            	                        <input type="submit" class="button4"  value="Lưu" />
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
                <%} %>
            </div>
        </div>
    </div>
   
</asp:Content>
