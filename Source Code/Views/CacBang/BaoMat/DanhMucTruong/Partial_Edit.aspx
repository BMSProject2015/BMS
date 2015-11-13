<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%@ Import Namespace="System.Reflection" %>

<%
PartialModel dlChuyen = (PartialModel)Model;
String ParentID = dlChuyen.ControlID;
Dictionary<string, object> dicData = dlChuyen.dicData;
using (Html.BeginForm("EditSubmit", "DanhMucTruong", new { ControlID = ParentID, TenBang = dicData["sTenBang"] }))
{
    SqlCommand cmd = new SqlCommand("SELECT sTenBangHT FROM PQ_DanhMucBang WHERE sTenBang=@sTenBang");
    cmd.Parameters.AddWithValue("@sTenBang", dicData["sTenBang"]);
    string TenBangHT = (String)(Connection.GetValue(cmd,""));
    string TenBang = (string)dicData["TenBang"];
    string TruongKhoa = (string)dicData["TruongKhoa"];
    string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(Page.User.Identity.Name, TenBang);
    cmd = new SqlCommand();
    cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE_NAME;";
    cmd.Parameters.AddWithValue("@TABLE_NAME", dicData["sTenBang"]);
    SelectOptionList slTruong = new SelectOptionList(Connection.GetDataTable(cmd), "COLUMN_NAME", "COLUMN_NAME");
    NameValueCollection data = (NameValueCollection)dicData["data"];
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
                        <%=NgonNgu.LayXau("Nhập mới trường")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Thay đổi trường")%>
                        <%
                    }
                    %>
                    </span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên bảng") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= TenBangHT%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên trường") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.DropDownList(ParentID, slTruong, data, "sTenTruong", sDanhSachTruongCam)%>
                     <%= Html.ValidationMessage(ParentID + "_" + "err_sTenTruong")%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tên hiển thị") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.TextBox(ParentID, data, "sTenTruongHT", sDanhSachTruongCam)%>
                     <%= Html.ValidationMessage(ParentID + "_" + "err_sTenTruongHT")%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Luôn được xem") %></div></td>
               <td class="td_form2_td5"><div>
                     <%= MyHtmlHelper.CheckBox(ParentID, data, "bLuonDuocXem", sDanhSachTruongCam)%>
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