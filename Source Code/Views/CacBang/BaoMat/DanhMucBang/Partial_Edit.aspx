<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%@ Import Namespace="System.Reflection" %>

<%
PartialModel dlChuyen = (PartialModel)Model;
String ParentID = dlChuyen.ControlID;
using (Html.BeginForm("EditSubmit", "DanhMucBang", new { ControlID = ParentID }))
{
    Dictionary<string, object> dicData = dlChuyen.dicData;
    string TenBang = (string)dicData["TenBang"];
    string TruongKhoa = (string)dicData["TruongKhoa"];
    string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(Page.User.Identity.Name, TenBang);
    NameValueCollection data = (NameValueCollection)dicData["data"];
    String SQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
    SelectOptionList slBang = new SelectOptionList(Connection.GetDataTable(SQL), "TABLE_NAME", "TABLE_NAME");
    %>
    <%= Html.Hidden(ParentID + "_" + TruongKhoa, data[TruongKhoa])%>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", dicData["DuLieuMoi"])%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>
                        <%
                        if (dicData["DuLieuMoi"] == "1")
                        {
                            %>
                            <%=NgonNgu.LayXau("Nhập mới bảng")%>
                            <%
                        }
                        else
                        {
                            %>
                            <%=NgonNgu.LayXau("Thay đổi bảng")%>
                            <%
                        }
                        %>&nbsp; &nbsp;
                    </span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.DropDownList(ParentID, slBang, data, "sTenBang", sDanhSachTruongCam)%>
                     <%= Html.ValidationMessage(ParentID + "_" + "err_sTenBang")%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên hiển thị") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.TextBox(ParentID, data, "sTenBangHT", sDanhSachTruongCam)%>
                     <%= Html.ValidationMessage(ParentID + "_" + "err_sTenBangHT")%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng xem") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bXem", sDanhSachTruongCam)%>
                </div></td>
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng thêm") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bThem", sDanhSachTruongCam)%>
                </div></td>
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng xóa") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bXoa", sDanhSachTruongCam)%>
                </div></td>
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng sửa") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bSua", sDanhSachTruongCam)%>
                </div></td>
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng chia sẻ") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bChiaSe", sDanhSachTruongCam)%>
                </div></td>
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Có chức năng giao phụ trách") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bGiaoPhuTrach", sDanhSachTruongCam)%>
                </div></td>
            </tr>
        </table>
      </div>
    </div>
</div>

<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td width="70%">&nbsp;
	    </td>
	    <td  width="30%" align="right">						
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                    <td width="5px"></td>
                    <td><input type="button" value="Hủy" class="button4" onclick="javascript:history.go(-1);" /></td>
                </tr>
            </table>         
	    </td>
    </tr>
</table>
<div class="cao5px">&nbsp;</div>
<%}%>