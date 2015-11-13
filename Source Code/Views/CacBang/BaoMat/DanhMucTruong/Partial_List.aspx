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

    string TenBang = (string)dicData["sTenBang"];
    SqlCommand cmd = new SqlCommand("SELECT * FROM PQ_DanhMucTruong WHERE sTenBang=@sTenBang ORDER BY iSTT;");
    cmd.Parameters.AddWithValue("@sTenBang", TenBang);
    DataTable dt = Connection.GetDataTable(cmd);
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td><span>Danh mục trường</span></td>
                <td align="right" style="padding-right: 10px;">
                    <b class="btn_form3">
                        <%= MyHtmlHelper.ActionLink(Url.Action("Create", "DanhMucTruong",new {TenBang = TenBang}), NgonNgu.LayXau("Thêm trường"), "Create", null)%>
                    </b>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th align="center"><%=NgonNgu.LayXau("Tên trường") %></th>
            <th align="center"><%=NgonNgu.LayXau("Tên hiển thị")%></th>
            <th align="center" style="width:140px">
                <%=NgonNgu.LayXau("Luôn được xem")%>
            </th>
            <th style="width: 5%" align="center">Sửa</th>
            <th style="width: 5%" align="center">Xóa</th>
        </tr>
            <%
            int i;
            String strSua_TruongCam = NgonNgu.LayXau("Sửa");
            String TenTruong, TenTruongHT;
            String strLuonDuocXem = "" ;
            for (i = 0; i <= dt.Rows.Count - 1; i++)
            {
                DataRow Row = dt.Rows[i];
                TenTruong = (string)Row["sTenTruong"];
                TenTruongHT = (string)Row["sTenTruongHT"];
                strLuonDuocXem = "";
                if ((Boolean)Row["bLuonDuocXem"])
                {
                    strLuonDuocXem = " checked='checked'";
                }
                strLuonDuocXem = String.Format("<input type=\"checkbox\" disabled {0}/>", strLuonDuocXem);

                string urlEdit = Url.Action("Edit", "DanhMucTruong", new { MaDanhMucTruong = Row["iID_MaDanhMucTruong"], TenBang = TenBang });
                string urlDelete = Url.Action("Delete", "DanhMucTruong", new { MaDanhMucTruong = Row["iID_MaDanhMucTruong"], TenBang = TenBang });
                String classtr = "";
                if (i % 2 == 0)
                {
                    classtr = "class=\"alt\"";
                }
                %>
                <tr <%=classtr %>>
                    <td>
                        <%= TenTruong%>
                    </td>
                    <td>
                        <%= TenTruongHT%>
                    </td>
                    <td align="center">
                        <%= strLuonDuocXem%>
                    </td>
                    <td align="center">
                        <%= MyHtmlHelper.ActionLink(urlEdit, "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", null)%>
                    </td>
                    <td align="center">
                        <%= MyHtmlHelper.ActionLink(urlDelete, "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", null)%>
                    </td>
                </tr>
             <%   
            }
            dt.Dispose();
            %>
    </table>
</div>

